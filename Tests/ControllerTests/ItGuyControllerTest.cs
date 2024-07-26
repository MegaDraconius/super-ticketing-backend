using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using super_ticketing_backend.Controllers;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Xunit;

namespace super_ticketing_backend.Tests
{
    public class ItGuyControllerTests
    {
        private readonly Mock<IITGuyRepository> _mockItGuyRepository;
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly IMapper _mapper;
        private readonly ItGuyController _controller;

        public ItGuyControllerTests()
        {
            _mockItGuyRepository = new Mock<IITGuyRepository>();
            _mockCountryRepository = new Mock<ICountryRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ITGuys, ItGuyDto>();
                cfg.CreateMap<ItGuyCreateDto, ITGuys>();
            });

            _mapper = mapperConfig.CreateMapper();
            _controller = new ItGuyController(_mockItGuyRepository.Object, _mockCountryRepository.Object, _mapper);
        }

        [Fact]
        public async Task Get_ReturnsListOfItGuyDtos()
        {
            // Arrange
            var itGuy = new ITGuys
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ItGuyName = "John",
                Surname = "Doe",
                Pwd = "password",
                Role = "Admin",
                CountryId = ObjectId.GenerateNewId().ToString(),
                ItGuyEmail = "john.doe@example.com"
            };

            var itGuys = new List<ITGuys> { itGuy };
            _mockItGuyRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(itGuys);

            var country = new Country
            {
                Id = itGuy.CountryId,
                CountryCode = "US",
                CountryName = "United States",
                CountryMainLanguage = "English"
            };

            _mockCountryRepository.Setup(repo => repo.GetAsync(itGuy.CountryId)).ReturnsAsync(country);

            // Act
            var result = await _controller.Get();

            // Assert
            var actionResult = Assert.IsType<List<ItGuyDto>>(result);
            Assert.Single(actionResult);
            Assert.Equal("US", actionResult[0].CountryCode);
        }

        [Fact]
        public async Task Get_WithValidId_ReturnsItGuyDto()
        {
            // Arrange
            var itGuyId = ObjectId.GenerateNewId().ToString();
            var itGuy = new ITGuys
            {
                Id = itGuyId,
                ItGuyName = "Jane",
                Surname = "Doe",
                Pwd = "password",
                Role = "User",
                CountryId = ObjectId.GenerateNewId().ToString(),
                ItGuyEmail = "jane.doe@example.com"
            };

            var country = new Country
            {
                Id = itGuy.CountryId,
                CountryCode = "UK",
                CountryName = "United Kingdom",
                CountryMainLanguage = "English"
            };

            _mockItGuyRepository.Setup(repo => repo.GetAsync(itGuyId)).ReturnsAsync(itGuy);
            _mockCountryRepository.Setup(repo => repo.GetAsync(itGuy.CountryId)).ReturnsAsync(country);

            // Act
            var result = await _controller.Get(itGuyId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ItGuyDto>>(result);
            var itGuyDto = Assert.IsType<ItGuyDto>(actionResult.Value);
            Assert.Equal("UK", itGuyDto.CountryCode);
        }

        [Fact]
        public async Task Post_ValidItGuyCreateDto_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var country = new Country
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryCode = "CA",
                CountryName = "Canada",
                CountryMainLanguage = "English"
            };

            var itGuyCreateDto = new ItGuyCreateDto
            {
                ItGuyName = "Alice",
                Surname = "Smith",
                Pwd = "password",
                Role = "Manager",
                CountryCode = "CA",
                ItGuyEmail = "alice.smith@example.com"
            };

            var newItGuy = new ITGuys
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ItGuyName = itGuyCreateDto.ItGuyName,
                Surname = itGuyCreateDto.Surname,
                Pwd = itGuyCreateDto.Pwd,
                Role = itGuyCreateDto.Role,
                CountryId = country.Id,
                ItGuyEmail = itGuyCreateDto.ItGuyEmail
            };

            _mockCountryRepository.Setup(repo => repo.GetByNameAsync(itGuyCreateDto.CountryCode)).ReturnsAsync(country);
            _mockItGuyRepository.Setup(repo => repo.CreateAsync(newItGuy)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(itGuyCreateDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var itGuyDto = Assert.IsType<ItGuyDto>(actionResult.Value);
            Assert.Equal("CA", itGuyDto.CountryCode);
        }

        [Fact]
        public async Task Update_ValidId_UpdatesItGuy()
        {
            // Arrange
            var itGuyId = ObjectId.GenerateNewId().ToString();
            var existingItGuy = new ITGuys
            {
                Id = itGuyId,
                ItGuyName = "Bob",
                Surname = "Brown",
                Pwd = "password",
                Role = "Developer",
                CountryId = ObjectId.GenerateNewId().ToString(),
                ItGuyEmail = "bob.brown@example.com"
            };

            var updatedItGuyDto = new ItGuyCreateDto
            {
                ItGuyName = "Robert",
                Surname = "Brown",
                Pwd = "newpassword",
                Role = "Senior Developer",
                CountryCode = "FR",
                ItGuyEmail = "robert.brown@example.com"
            };

            var country = new Country
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CountryCode = "FR",
                CountryName = "France",
                CountryMainLanguage = "French"
            };

            _mockItGuyRepository.Setup(repo => repo.GetAsync(itGuyId)).ReturnsAsync(existingItGuy);
            _mockCountryRepository.Setup(repo => repo.GetByNameAsync(updatedItGuyDto.CountryCode)).ReturnsAsync(country);
            _mockItGuyRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ITGuys>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(itGuyId, updatedItGuyDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidId_DeletesItGuy()
        {
            // Arrange
            var itGuyId = ObjectId.GenerateNewId().ToString();
            var itGuy = new ITGuys
            {
                Id = itGuyId,
                ItGuyName = "Charlie",
                Surname = "Green",
                Pwd = "password",
                Role = "Tester",
                CountryId = ObjectId.GenerateNewId().ToString(),
                ItGuyEmail = "charlie.green@example.com"
            };

            _mockItGuyRepository.Setup(repo => repo.GetAsync(itGuyId)).ReturnsAsync(itGuy);
            _mockItGuyRepository.Setup(repo => repo.RemoveAsync(itGuyId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(itGuyId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using super_ticketing_backend.Controllers;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;
using Xunit;

namespace Tests.ControllerTests;

public class CountryControllerTest
{
        private readonly Mock<ICountryRepository> _mockCountryRepository;
        private readonly IMapper _mapper;
        private readonly CountryController _countryController;

        public CountryControllerTest()
        {
            _mockCountryRepository = new Mock<ICountryRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, CountryDto>();
                cfg.CreateMap<CountryCreateDto, Country>();
            });
            _mapper = config.CreateMapper();

            _countryController = new CountryController(_mockCountryRepository.Object, _mapper);
        }

        [Fact]
        public async Task Get_ReturnsListOfCountryDtos()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country
                {
                    Id = ObjectId.GenerateNewId().ToString(), 
                    CountryCode = "US", 
                    CountryName = "United States", 
                    CountryMainLanguage = "English"
                },
                new Country
                {
                    Id = ObjectId.GenerateNewId().ToString(), 
                    CountryCode = "ES", CountryName = "Spain", CountryMainLanguage = "Spanish"
                }
            };

            _mockCountryRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(countries);

            // Act
            var result = await _countryController.Get();

            // Assert
            var countryDtos = Assert.IsType<List<CountryDto>>(result);
            Assert.Equal(2, countryDtos.Count);
        }

        [Fact]
        public async Task Get_ReturnsCountryDto_WhenIdIsValid()
        {
            // Arrange
            var countryId = ObjectId.GenerateNewId().ToString();
            var country = new Country { Id = countryId, CountryCode = "US", CountryName = "United States", CountryMainLanguage = "English" };

            _mockCountryRepository.Setup(repo => repo.GetAsync(countryId)).ReturnsAsync(country);

            // Act
            var result = await _countryController.Get(countryId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CountryDto>>(result);
            var returnValue = Assert.IsType<CountryDto>(actionResult.Value);
            Assert.Equal(countryId, returnValue.Id);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = ObjectId.GenerateNewId().ToString();
            _mockCountryRepository.Setup(repo => repo.GetAsync(invalidId)).ReturnsAsync((Country)null);

            // Act
            var result = await _countryController.Get(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult_WhenCountryIsCreated()
        {
            // Arrange
            var countryCreateDto = new CountryCreateDto { CountryCode = "FR", CountryName = "France", CountryMainLanguage = "French" };
            var country = _mapper.Map<Country>(countryCreateDto);
            var newCountryId = ObjectId.GenerateNewId().ToString();
            country.Id = newCountryId;
    
            _mockCountryRepository.Setup(repo => repo.CreateAsync(It.IsAny<Country>())).Callback<Country>(c => c.Id = newCountryId).Returns(Task.CompletedTask);

            // Act
            var result = await _countryController.Post(countryCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CountryDto>(createdAtActionResult.Value);
            Assert.Equal(newCountryId, returnValue.Id);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenCountryIsUpdated()
        {
            // Arrange
            var countryId = ObjectId.GenerateNewId().ToString();
            var country = new Country { Id = countryId, CountryCode = "DE", CountryName = "Germany", CountryMainLanguage = "German" };
            var countryCreateDto = new CountryCreateDto { CountryCode = "DE", CountryName = "Germany", CountryMainLanguage = "German" };

            _mockCountryRepository.Setup(repo => repo.GetAsync(countryId)).ReturnsAsync(country);
            _mockCountryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Country>())).Returns(Task.CompletedTask);

            // Act
            var result = await _countryController.Update(countryId, countryCreateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var invalidId = ObjectId.GenerateNewId().ToString();
            _mockCountryRepository.Setup(repo => repo.GetAsync(invalidId)).ReturnsAsync((Country)null);

            // Act
            var result = await _countryController.Update(invalidId, new CountryCreateDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenCountryIsDeleted()
        {
            // Arrange
            var countryId = ObjectId.GenerateNewId().ToString();
            var country = new Country { Id = countryId, CountryCode = "IT", CountryName = "Italy", CountryMainLanguage = "Italian" };

            _mockCountryRepository.Setup(repo => repo.GetAsync(countryId)).ReturnsAsync(country);
            _mockCountryRepository.Setup(repo => repo.RemoveAsync(countryId)).Returns(Task.CompletedTask);

            // Act
            var result = await _countryController.Delete(countryId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var invalidId = ObjectId.GenerateNewId().ToString();
            _mockCountryRepository.Setup(repo => repo.GetAsync(invalidId)).ReturnsAsync((Country)null);

            // Act
            var result = await _countryController.Delete(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
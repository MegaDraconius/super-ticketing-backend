using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using super_ticketing_backend.Controllers;
using super_ticketing_backend.Dto_s;
using super_ticketing_backend.Models;
using super_ticketing_backend.Repositories;

namespace Tests.ControllerTests;

public class UserControllerTest
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IMapper _mapper;
    private readonly UserController _userController;

    public UserControllerTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Users, UserDto>();
            cfg.CreateMap<UserCreateDto, Users>();
        });

        _mapper = config.CreateMapper();

        _userController = new UserController(_mockUserRepository.Object, _mapper);
    }

    [Fact]
    public async Task Get_ReturnsListOfUsersDtos()
    {
        var users = new List<Users>
        {
            new Users
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Country = "Switzerland",
                Email = "alvin@gmail.com",
                Pwd = "Hello1!",
                Role = "Administrator"
            },
            new Users
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Country = "Poland",
                Email = "Steve@gmail.com",
                Pwd = "Hello1!",
                Role = "Administrator"
            }
        };

        _mockUserRepository.Setup(repo => repo.GetAsync()).ReturnsAsync(users);

        var result = await _userController.Get();

        var userDtos = Assert.IsType<List<UserDto>>(result);
        Assert.Equal(2, userDtos.Count);
    }

    [Fact]
    public async Task Get_ReturnsCountryDto_WhenIsIsValid()
    {
        var userId = ObjectId.GenerateNewId().ToString();
        var user = new Users
        {
            Id = userId,
            Country = "Poland",
            Email = "Steve@gmail.com",
            Pwd = "Hello1!",
            Role = "Administrator"
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(user);

        var result = await _userController.Get(userId);

        var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
        var returnValue = Assert.IsType<UserDto>(actionResult.Value);
        Assert.Equal(userId, returnValue.Id);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenIdIsInvalid()
    {
        var invalidId = ObjectId.GenerateNewId().ToString();
        _mockUserRepository.Setup(repo => repo.GetAsync(invalidId)).ReturnsAsync((Users)null);

        var result = await _userController.Get(invalidId);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Post_ReturnsCreatedAtActionResult_WhenUserIsCreated()
    {
        var userCreateDto = new UserCreateDto
        {
            Country = "Portugal",
            Email = "hola@gmail.com",
            Pwd = "Hello1!",
            Role = "Admin"
        };

        var user = _mapper.Map<Users>(userCreateDto);
        var newUserId = ObjectId.GenerateNewId().ToString();
        user.Id = newUserId;

        _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<Users>())).Callback<Users>(c => c.Id = newUserId)
            .Returns(Task.CompletedTask);

        var result = await _userController.Post(userCreateDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<UserDto>(createdAtActionResult.Value);
        Assert.Equal(newUserId, returnValue.Id);
    }

    [Fact]
        public async Task Update_ReturnsNoContent_WhenCountryIsUpdated()
        {
            var userId = ObjectId.GenerateNewId().ToString();
            var user = new Users
            {
                Id = userId,
                Country = "Sweden",
                Email = "zlatan@gmail.com",
                Pwd = "Hello1!",
                Role = "Admin"
            };
            var userCreateDto = new UserCreateDto
            {
                Country = "Sweden",
                Email = "zlatan@gmail.com",
                Pwd = "Hello1!",
                Role = "Admin"
            };

            _mockUserRepository.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(user);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Users>())).Returns(Task.CompletedTask);

            var result = await _userController.Update(userId, userCreateDto);

            Assert.IsType<NoContentResult>(result);
        }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCountryDoesNotExist()
    {
        var invalidId = ObjectId.GenerateNewId().ToString();
        _mockUserRepository.Setup(repo => repo.GetAsync(invalidId)).ReturnsAsync((Users)null);

        var result = await _userController.Update(invalidId, new UserCreateDto());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_REturnsNoContent_whenCountryIsDeleted()
    {
        var userId = ObjectId.GenerateNewId().ToString();
        var user = new Users
        {
            Id = userId,
            Country = "Italy",
            Email = "italy@gmail.com",
            Pwd = "Hello1!",
            Role = "Admin"
        };

        _mockUserRepository.Setup(repo => repo.GetAsync(userId)).ReturnsAsync(user);
        _mockUserRepository.Setup(repo => repo.RemoveAsync(userId)).Returns(Task.CompletedTask);

        var result = await _userController.Delete(userId);
        
        Assert.IsType<NoContentResult>(result);

    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCountryDoesNotExist()
    {
        var invalidId = ObjectId.GenerateNewId().ToString();
        _mockUserRepository.Setup(repo => repo.GetAsync(invalidId)).ReturnsAsync((Users)null);

        var result = await _userController.Delete(invalidId);

        Assert.IsType<NotFoundResult>(result);
    }
    }




















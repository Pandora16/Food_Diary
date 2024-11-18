using Moq;
using NUnit.Framework;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services.Business;

namespace Дневник_Питания.Tests.Core
{
    public class FoodDataServiceTests
    {
        private Mock<IFoodRepository> _mockFoodRepository;
        private FoodDataService _foodDataService;

        [SetUp]
        public void SetUp()
        {
            // Создание мока для IFoodRepository
            _mockFoodRepository = new Mock<IFoodRepository>();
            // Создание экземпляра FoodDataService с передачей мока
            _foodDataService = new FoodDataService(_mockFoodRepository.Object);
        }

        [Test]
        public async Task SaveAllDataAsync_ShouldCallSaveDataAsync_WhenCalled()
        {
            // Arrange
            var user = new User
            {
                Height = 175,
                Weight = 70.5,
                Age = 30,
                Gender = "м",
                ActivityLevel = "3", // Например, умеренные упражнения
                BMR = 1500.0,
                TargetCalories = 2000.0
            };
            var allFoods = new List<Food> { new Food(), new Food() };

            // Настройка мока для метода GetAllFoodsAsync
            _mockFoodRepository.Setup(repo => repo.GetAllFoodsAsync()).ReturnsAsync(allFoods);
            
            // Настройка мока для метода SaveDataAsync
            _mockFoodRepository.Setup(repo => repo.SaveDataAsync(user, allFoods)).Returns(Task.CompletedTask);

            // Act
            await _foodDataService.SaveAllDataAsync(user);

            // Assert
            _mockFoodRepository.Verify(repo => repo.SaveDataAsync(user, allFoods), Times.Once);
        }

        [Test]
        public async Task LoadUserAsync_ShouldReturnUser_WhenDataExists()
        {
            // Arrange
            var expectedUser = new User
            {
                Height = 175,
                Weight = 70.5,
                Age = 30,
                Gender = "м",
                ActivityLevel = "3",
                BMR = 1500.0,
                TargetCalories = 2000.0
            };
            var foodData = (expectedUser, new List<Food>());

            // Настройка мока для метода LoadDataAsync
            _mockFoodRepository.Setup(repo => repo.LoadDataAsync()).ReturnsAsync(foodData);

            // Act
            var result = await _foodDataService.LoadUserAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Height, Is.EqualTo(expectedUser.Height));
            Assert.That(result.Weight, Is.EqualTo(expectedUser.Weight));
            Assert.That(result.Age, Is.EqualTo(expectedUser.Age));
            Assert.That(result.Gender, Is.EqualTo(expectedUser.Gender));
            Assert.That(result.ActivityLevel, Is.EqualTo(expectedUser.ActivityLevel));
            Assert.That(result.BMR, Is.EqualTo(expectedUser.BMR));
            Assert.That(result.TargetCalories, Is.EqualTo(expectedUser.TargetCalories));
        }
    }
}

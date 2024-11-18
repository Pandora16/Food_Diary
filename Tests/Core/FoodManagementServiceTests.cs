using Moq;
using NUnit.Framework;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.UI;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services.Business;

namespace Дневник_Питания.Tests.Core
{
    [TestFixture]
    public class FoodManagementServiceTests
    {
        private Mock<IUserInputManager> _inputManagerMock;
        private Mock<IUserInterface> _userInterfaceMock;
        private Mock<IFoodRepository> _foodRepositoryMock;
        private FoodManagementService _service;

        [SetUp]
        public void SetUp()
        {
            _inputManagerMock = new Mock<IUserInputManager>();
            _userInterfaceMock = new Mock<IUserInterface>();
            _foodRepositoryMock = new Mock<IFoodRepository>();
            _service = new FoodManagementService(
                _foodRepositoryMock.Object,
                _inputManagerMock.Object,
                _userInterfaceMock.Object);
        }

        [Test]
        public async Task AddFoodAsync_ShouldCallSaveFoodAsync_WhenInputIsValid()
        {
            // Arrange
            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("Apple");
            _inputManagerMock.Setup(im => im.GetPositiveDoubleAsync(It.IsAny<string>()))
                .ReturnsAsync(50.0);
            _inputManagerMock.Setup(im => im.GetMealTimeAsync())
                .ReturnsAsync(MealTime.Breakfast);

            // Act
            await _service.AddFoodAsync();

            // Assert
            _foodRepositoryMock.Verify(repo => repo.SaveFoodAsync(It.Is<Food>(food =>
                food.Name == "Apple" &&
                food.Calories == 50.0 &&
                food.Proteins == 50.0 &&
                food.Fats == 50.0 &&
                food.Carbohydrates == 50.0 &&
                food.MealTime == MealTime.Breakfast)),
                Times.Once);

            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync("Продукт успешно добавлен!"), Times.Once);
        }

        [Test]
        public async Task AddFoodAsync_ShouldRetryOnEmptyName()
        {
            // Arrange
            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("")
                .ReturnsAsync("  ")
                .ReturnsAsync("Apple");

            // Act
            await _service.AddFoodAsync();

            // Assert
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync("Ошибка! Название продукта не может быть пустым."), Times.Exactly(2));
            _foodRepositoryMock.Verify(repo => repo.SaveFoodAsync(It.IsAny<Food>()), Times.Once);
        }

        [Test]
        public async Task AddFoodAsync_ShouldRetryOnInvalidName()
        {
            // Arrange
            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("1234")
                .ReturnsAsync("Apple");

            // Act
            await _service.AddFoodAsync();

            // Assert
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync("Ошибка! Название продукта не должно состоять только из цифр."), Times.Once);
            _foodRepositoryMock.Verify(repo => repo.SaveFoodAsync(It.IsAny<Food>()), Times.Once);
        }
    }
}

using Moq;
using NUnit.Framework;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Services.Business;
using Дневник_Питания.Core.Interfaces;

namespace Дневник_Питания.Tests
{
    [TestFixture]
    public class UserInputManagerTests
    {
        private Mock<IUserInterface> _mockUserInterface;
        private UserInputManager _userInputManager;

        [SetUp]
        public void SetUp()
        {
            // Инициализация моков перед каждым тестом
            _mockUserInterface = new Mock<IUserInterface>();
            _userInputManager = new UserInputManager(_mockUserInterface.Object);
        }

        // Тест для метода GetMealTimeAsync
        [Test]
        public async Task GetMealTimeAsync_ShouldReturnBreakfast_WhenInputIsBreakfast()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("завтрак");

            // Act
            var result = await _userInputManager.GetMealTimeAsync();

            // Assert
            Assert.That(result, Is.EqualTo(MealTime.Breakfast));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Once()); // Проверка вызова WriteMessageAsync
        }

        [Test]
        public async Task GetMealTimeAsync_ShouldReturnLunch_WhenInputIsLunch()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("обед");

            // Act
            var result = await _userInputManager.GetMealTimeAsync();

            // Assert
            Assert.That(result, Is.EqualTo(MealTime.Lunch));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Once()); // Проверка вызова WriteMessageAsync
        }

        [Test]
        public async Task GetMealTimeAsync_ShouldPromptAgain_WhenInputIsInvalid()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("полдник") // Неверный ввод
                .ReturnsAsync("ужин");    // Правильный ввод

            // Act
            var result = await _userInputManager.GetMealTimeAsync();

            // Assert
            Assert.That(result, Is.EqualTo(MealTime.Dinner));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(3));
        }

        // Тест для метода GetPositiveIntegerAsync
        [Test]
        public async Task GetPositiveIntegerAsync_ShouldReturnPositiveInteger_WhenInputIsValid()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("25");

            // Act
            var result = await _userInputManager.GetPositiveIntegerAsync("Введите возраст: ");

            // Assert
            Assert.That(result, Is.EqualTo(25));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Once()); // Проверка на два вызова сообщения
        }

        [Test]
        public async Task GetPositiveIntegerAsync_ShouldPromptAgain_WhenInputIsInvalid()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("-5")    // Неверный ввод
                .ReturnsAsync("30");   // Правильный ввод

            // Act
            var result = await _userInputManager.GetPositiveIntegerAsync("Введите возраст: ");

            // Assert
            Assert.That(result, Is.EqualTo(30));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(3));
        }

        // Тест для метода GetGenderAsync
        [Test]
        public async Task GetGenderAsync_ShouldReturnMale_WhenInputIsMale()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("м"); // Ввод "м"

            // Act
            var result = await _userInputManager.GetGenderAsync();

            // Assert
            Assert.That(result, Is.EqualTo("м"));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Once()); // Проверка на два вызова сообщения
        }

        [Test]
        public async Task GetGenderAsync_ShouldPromptAgain_WhenInputIsInvalid()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("неизвестно") // Неверный ввод
                .ReturnsAsync("ж"); // Правильный ввод

            // Act
            var result = await _userInputManager.GetGenderAsync();

            // Assert
            Assert.That(result, Is.EqualTo("ж"));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(3));
        }

        // Тест для метода GetActivityLevelAsync
        [Test]
        public async Task GetActivityLevelAsync_ShouldReturnValidChoice_WhenInputIsValid()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("3"); // Ввод "3"

            // Act
            var result = await _userInputManager.GetActivityLevelAsync();

            // Assert
            Assert.That(result, Is.EqualTo("3"));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(7)); // Ожидается 7 вызовов сообщения
        }

        [Test]
        public async Task GetActivityLevelAsync_ShouldPromptAgain_WhenInputIsInvalid()
        {
            // Arrange
            _mockUserInterface.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("6") // Неверный ввод
                .ReturnsAsync("4"); // Правильный ввод

            // Act
            var result = await _userInputManager.GetActivityLevelAsync();

            // Assert
            Assert.That(result, Is.EqualTo("4"));
            _mockUserInterface.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(9)); // Ожидается 8 вызовов
        }
    }
}

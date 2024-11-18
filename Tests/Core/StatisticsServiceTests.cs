using Moq;
using NUnit.Framework;
using Дневник_Питания.Core.Models;
using Дневник_Питания.Core.Interfaces;
using Дневник_Питания.Core.Interfaces.Repositories;
using Дневник_Питания.Core.Interfaces.Services;
using Дневник_Питания.Core.Services.Business;

namespace Дневник_Питания.Tests
{
    [TestFixture]
    public class StatisticsServiceTests
    {
        private Mock<IUserInterface> _userInterfaceMock;
        private Mock<ICalorieCalculator> _calorieCalculatorMock;
        private Mock<IFoodRepository> _foodRepositoryMock;
        private StatisticsService _statisticsService;

        [SetUp]
        public void Setup()
        {
            _userInterfaceMock = new Mock<IUserInterface>();
            _calorieCalculatorMock = new Mock<ICalorieCalculator>();
            _foodRepositoryMock = new Mock<IFoodRepository>();

            _statisticsService = new StatisticsService(
                _userInterfaceMock.Object,
                _calorieCalculatorMock.Object,
                _foodRepositoryMock.Object);
        }

        [Test]
        public async Task ShowStatisticsAsync_ShouldDisplayCorrectStatistics_ForDayPeriod()
        {
            // Arrange
            var user = new User { TargetCalories = 2000 };
            var foodList = new List<Food>
            {
                new Food { Name = "Apple", Calories = 100, Date = DateTime.Now.Date, MealTime = MealTime.Breakfast },
                new Food { Name = "Banana", Calories = 150, Date = DateTime.Now.Date, MealTime = MealTime.Lunch },
            };

            _foodRepositoryMock.Setup(repo => repo.GetAllFoodsAsync()).ReturnsAsync(foodList);
            _calorieCalculatorMock.Setup(calc => calc.CalculateBMR(user)).Returns(1500);
            _calorieCalculatorMock.Setup(calc => calc.CalculateTotalCalories(user)).Returns(2000);

            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("1")  // Пользователь выбирает период "за день"
                .ReturnsAsync("");  // Дополнительный ввод, который не используется

            // Act
            await _statisticsService.ShowStatisticsAsync(user);

            // Assert
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.AtLeastOnce);  // Проверяем, что вывод сообщений происходит
            _foodRepositoryMock.Verify(repo => repo.GetAllFoodsAsync(), Times.Once); // Убедимся, что репозиторий был вызван

            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(16));
        }

        [Test]
        public async Task ShowStatisticsAsync_ShouldHandleEmptyFoodData()
        {
            // Arrange
            var user = new User { TargetCalories = 2000 };
            var foodList = new List<Food>();  // Пустой список продуктов

            _foodRepositoryMock.Setup(repo => repo.GetAllFoodsAsync()).ReturnsAsync(foodList);

            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("1");

            // Act
            await _statisticsService.ShowStatisticsAsync(user);

            // Assert
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(6));
        }

        [Test]
        public async Task ShowStatisticsAsync_ShouldDisplayStatisticsForWeekPeriod()
        {
            // Arrange
            var user = new User { TargetCalories = 2500 };
            var foodList = new List<Food>
            {
                new Food { Name = "Chicken", Calories = 500, Date = DateTime.Now.Date.AddDays(-1), MealTime = MealTime.Lunch },
                new Food { Name = "Rice", Calories = 200, Date = DateTime.Now.Date.AddDays(-3), MealTime = MealTime.Dinner }
            };

            _foodRepositoryMock.Setup(repo => repo.GetAllFoodsAsync()).ReturnsAsync(foodList);
            _calorieCalculatorMock.Setup(calc => calc.CalculateBMR(user)).Returns(1700);
            _calorieCalculatorMock.Setup(calc => calc.CalculateTotalCalories(user)).Returns(2200);

            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("2");  // Пользователь выбирает период "за неделю"

            // Act
            await _statisticsService.ShowStatisticsAsync(user);

            // Assert
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.AtLeastOnce);  // Проверяем, что вывод сообщений происходит
            _foodRepositoryMock.Verify(repo => repo.GetAllFoodsAsync(), Times.Once);  // Убедимся, что репозиторий был вызван

            // Проверим корректные выводы статистики
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(16));
        }

        [Test]
        public async Task ShowStatisticsAsync_ShouldDisplayStatisticsForMonthPeriod()
        {
            // Arrange
            var user = new User { TargetCalories = 1800 };
            var foodList = new List<Food>
            {
                new Food { Name = "Bread", Calories = 150, Date = DateTime.Now.Date.AddDays(-15), MealTime = MealTime.Breakfast },
                new Food { Name = "Steak", Calories = 500, Date = DateTime.Now.Date.AddDays(-5), MealTime = MealTime.Dinner }
            };

            _foodRepositoryMock.Setup(repo => repo.GetAllFoodsAsync()).ReturnsAsync(foodList);
            _calorieCalculatorMock.Setup(calc => calc.CalculateBMR(user)).Returns(1600);
            _calorieCalculatorMock.Setup(calc => calc.CalculateTotalCalories(user)).Returns(2100);

            _userInterfaceMock.SetupSequence(ui => ui.ReadInputAsync())
                .ReturnsAsync("3");  // Пользователь выбирает период "за месяц"

            // Act
            await _statisticsService.ShowStatisticsAsync(user);

            // Assert
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _foodRepositoryMock.Verify(repo => repo.GetAllFoodsAsync(), Times.Once);
            _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Exactly(16));
        }
    }
}

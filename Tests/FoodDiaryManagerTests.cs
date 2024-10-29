using Xunit;
using Moq;
using Дневник_Питания.Interfaces;
using Дневник_Питания.Services;
using Food_Diary.Models;
using Дневник_Питания.DataManagement;
using Дневник_Питания.Interface;

namespace Дневник_Питания.Tests;

public class FoodDiaryManagerTests
{
    private readonly Mock<IDataRepository> _dataRepositoryMock;
    private readonly Mock<IUserInterface> _userInterfaceMock;
    private readonly FoodDiaryManager _foodDiaryManager;

    public FoodDiaryManagerTests()
    {
        _dataRepositoryMock = new Mock<IDataRepository>();
        _userInterfaceMock = new Mock<IUserInterface>();
        _foodDiaryManager = new FoodDiaryManager(_dataRepositoryMock.Object, _userInterfaceMock.Object);
    }

    [Fact]
    public async Task AddFoodAsync_ShouldAddFoodToDiary()
    {
        await _foodDiaryManager.AddFoodAsync("Apple", 52, 0.3, 0.2, 14, "Breakfast");
        _userInterfaceMock.Verify(ui => ui.WriteMessageAsync(It.IsAny<string>()), Times.Once);
    }
}

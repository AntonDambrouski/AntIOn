using FluentValidation;
using FluentValidation.Results;
using Moq;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;
using Workout.Core.Services;

namespace Workout.UnitTests.ServicesTests;

public class TrainingServiceTests
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IValidator<Training>> _validator;

    public TrainingServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _validator = new Mock<IValidator<Training>>();
        _validator.Setup(v => v.Validate(It.IsAny<Training>())).Returns(new ValidationResult());
    }

    [Fact]
    public async Task CreateAsync_SendNonExistingSetId_ReturnsError()
    {
        // Arrange
        var setIds = new string[] { "valid id", "valid id", "invalid id" };
        _unitOfWork.Setup(uof => uof.SetRepository.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(setIds.Where(id => id == "valid id").Select(id => new Set()));
        var setService = new TrainingService(_unitOfWork.Object, _validator.Object);

        // Act
        var result = await setService.CreateAsync(new Training(), setIds);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task CreateAsync_SendExistingSetIds_ReturnsNullAndCreatesTraining()
    {
        // Arrange
        var wasCreated = false;
        var setIds = new string[] { "valid id", "valid id", "valid id" };
        _unitOfWork.Setup(uof => uof.SetRepository.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(setIds.Where(id => id == "valid id").Select(id => new Set()));
        _unitOfWork.Setup(uof => uof.TrainingRepository.CreateAsync(It.IsAny<Training>()))
            .Returns(() =>
            {
                wasCreated = true;
                return Task.CompletedTask;
            });

        var setService = new TrainingService(_unitOfWork.Object, _validator.Object);

        // Act
        var result = await setService.CreateAsync(new Training(), setIds);

        // Assert
        Assert.Null(result);
        Assert.True(wasCreated);
    }
}

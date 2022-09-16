using FluentValidation;
using FluentValidation.Results;
using Moq;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;
using Workout.Core.Services;

namespace Workout.UnitTests.ServicesTests;

public class FitnessGoalServiceTests
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IValidator<FitnessGoal>> _validator;

    public FitnessGoalServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _validator = new Mock<IValidator<FitnessGoal>>();
        _validator.Setup(v => v.Validate(It.IsAny<FitnessGoal>())).Returns(new ValidationResult());
    }

    [Fact]
    public async Task CreateAsync_SendNonExistingSetId_ReturnsError()
    {
        // Arrange
        var stepsIds = new string[] { "valid id" };
        _unitOfWork.Setup(uof => uof.SetRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(null as Set);
        _unitOfWork.Setup(uof => uof.StepRepository.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(stepsIds.Distinct().Select(id => new Step { Id = id }));
        var setService = new FitnessGoalService(_unitOfWork.Object, _validator.Object);

        // Act
        var result = await setService.CreateAsync(new FitnessGoal(), stepsIds, "bad set id");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task CreateAsync_SendNonExistingStepId_ReturnsError()
    {
        // Arrange
        var stepsIds = new string[] { "valid id", "valid id", "invalid id" };
        _unitOfWork.Setup(uof => uof.SetRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Set());
        _unitOfWork.Setup(uof => uof.StepRepository.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(stepsIds.Where(id => id == "valid id").Distinct().Select(id => new Step()));
        var setService = new FitnessGoalService(_unitOfWork.Object, _validator.Object);

        // Act
        var result = await setService.CreateAsync(new FitnessGoal(), stepsIds, "valid id");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task CreateAsync_SendExistingStepIdsAndSetId_ReturnsNullAndCreatesFitnessGoal()
    {
        // Arrange
        var stepsIds = new string[] { "valid id", "valid id", "valid id" };
        var wasCreated = false;
        _unitOfWork.Setup(uof => uof.SetRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Set());
        _unitOfWork.Setup(uof => uof.StepRepository.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(stepsIds.Where(id => id == "valid id").Distinct().Select(id => new Step()));
        _unitOfWork.Setup(uof => uof.FitnessGoalRepository.CreateAsync(It.IsAny<FitnessGoal>()))
                       .Returns(() =>
                       {
                           wasCreated = true;
                           return Task.CompletedTask;
                       });

        var setService = new FitnessGoalService(_unitOfWork.Object, _validator.Object);

        // Act
        var result = await setService.CreateAsync(new FitnessGoal(), stepsIds, "valid id");

        // Assert
        Assert.Null(result);
        Assert.True(wasCreated);
    }
}

using FluentValidation;
using FluentValidation.Results;
using Moq;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;
using Workout.Core.Services;

namespace Workout.UnitTests.ServicesTests;

public class SetServiceTests
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IValidator<Set>> _validator;

    public SetServiceTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _validator = new Mock<IValidator<Set>>();
        _validator.Setup(v => v.Validate(It.IsAny<Set>())).Returns(new ValidationResult());
    }

    [Fact]
    public async Task CreateAsync_SendNonExistingExerciseId_ReturnsError()
    {
        // Arrange
        _unitOfWork.Setup(uof => uof.ExerciseRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(null as Exercise);
        var setService = new SetService(_unitOfWork.Object, _validator.Object);

        // Act
        var result = await setService.CreateAsync(new Set(), "bad id");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task CreateAsync_SendExestingExerciseId_ReturnsNullAndCreatesSet()
    {
        //Arrange
        var wasCreated = false;
        _unitOfWork.Setup(uof => uof.ExerciseRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Exercise());
        _unitOfWork.Setup(uof => uof.SetRepository.CreateAsync(It.IsAny<Set>()))
                .Returns(() =>
                {
                    wasCreated = true;
                    return Task.CompletedTask;
                });

        var setService = new SetService(_unitOfWork.Object, _validator.Object);

        //Act
        var result = await setService.CreateAsync(new Set(), "existing exercise Id");

        //Assert
        Assert.Null(result);
        Assert.True(wasCreated);
    }
}

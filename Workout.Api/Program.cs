using FluentValidation;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Interfaces.Services;
using Workout.Core.Models;
using Workout.Core.Services;
using Workout.Core.Validators;
using Workout.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IValidator<Exercise>, ExerciseValidator>();
builder.Services.AddScoped<IValidator<Set>, SetValidator>();
builder.Services.AddScoped<IValidator<Step>, StepValidator>();
builder.Services.AddScoped<IValidator<FitnessGoal>, FitnessGoalValidator>();
builder.Services.AddScoped<IValidator<Training>, TrainingValidator>();

builder.Services.AddScoped<IFitnessGoalService, FitnessGoalService>();
builder.Services.AddScoped<ISetService, SetService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();

builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

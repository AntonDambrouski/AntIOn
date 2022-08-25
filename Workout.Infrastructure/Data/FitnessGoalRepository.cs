using Newtonsoft.Json;
using System.Net.Http.Json;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class FitnessGoalRepository : IFitnessGoalRepository
{
    private readonly string? FirebaseBaseAddress;
    private readonly HttpClient _client;

    public FitnessGoalRepository(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient();
        FirebaseBaseAddress = Environment.GetEnvironmentVariable("FirebaseBaseAddress");
    }

    public async Task CreateAsync(FitnessGoal item)
    {
        var content = JsonContent.Create(item);
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.FitnessGoals}/{item.Id}.json";
        var response = await _client.PutAsync(requestUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.FitnessGoals}/{id}.json";
        var response = await _client.DeleteAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task<IEnumerable<FitnessGoal>> GetAllAsync()
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.FitnessGoals}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exercisesJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, FitnessGoal>>(exercisesJson)?.Values;
    }

    public async Task<FitnessGoal?> GetByIdAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.FitnessGoals}/{id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exerciseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<FitnessGoal?>(exerciseJson);
    }

    public async Task UpdateAsync(FitnessGoal item)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.FitnessGoals}/{item.Id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }
}

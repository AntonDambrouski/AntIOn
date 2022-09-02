using Newtonsoft.Json;
using System.Net.Http.Json;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class TrainingRepository : ITrainingRepository
{
    private readonly string? FirebaseBaseAddress;
    private readonly HttpClient _client;

    public TrainingRepository(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient();
        FirebaseBaseAddress = Environment.GetEnvironmentVariable("FirebaseBaseAddress");
    }

    public async Task CreateAsync(Training item)
    {
        var content = JsonContent.Create(item);
        var requestUrl = FirebaseBaseAddress + $"{MongoDbNames.Trainings}/{item.Id}.json";
        var response = await _client.PutAsync(requestUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{MongoDbNames.Trainings}/{id}.json";
        var response = await _client.DeleteAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task<IEnumerable<Training>> GetAllAsync()
    {
        var requestUrl = FirebaseBaseAddress + $"{MongoDbNames.Trainings}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exercisesJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, Training>>(exercisesJson)?.Values;
    }

    public async Task<Training?> GetByIdAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{MongoDbNames.Trainings}/{id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exerciseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Training?>(exerciseJson);
    }

    public async Task UpdateAsync(Training item)
    {
        var requestUrl = FirebaseBaseAddress + $"{MongoDbNames.Trainings}/{item.Id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }
}

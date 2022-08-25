using Newtonsoft.Json;
using System.Net.Http.Json;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class StepRepository : IStepRepository
{
    private readonly string? FirebaseBaseAddress;
    private readonly HttpClient _client;

    public StepRepository(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient();
        FirebaseBaseAddress = Environment.GetEnvironmentVariable("FirebaseBaseAddress");
    }
    public async Task CreateAsync(Step item)
    {
        var content = JsonContent.Create(item);
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Steps}/{item.Id}.json";
        var response = await _client.PutAsync(requestUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Steps}/{id}.json";
        var response = await _client.DeleteAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task<IEnumerable<Step>> GetAllAsync()
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Steps}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exercisesJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, Step>>(exercisesJson)?.Values;
    }

    public async Task<Step?> GetByIdAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Steps}/{id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exerciseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Step?>(exerciseJson);
    }

    public async Task UpdateAsync(Step item)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Steps}/{item.Id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }
}

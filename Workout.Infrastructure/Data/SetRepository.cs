using Newtonsoft.Json;
using System.Net.Http.Json;
using Workout.Core.Constants;
using Workout.Core.Interfaces.Repositories;
using Workout.Core.Models;

namespace Workout.Infrastructure.Data;

public class SetRepository : ISetRepository
{
    private readonly string? FirebaseBaseAddress;
    private readonly HttpClient _client;

    public SetRepository(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient();
        FirebaseBaseAddress = Environment.GetEnvironmentVariable("FirebaseBaseAddress");
    }

    public async Task CreateAsync(Set item)
    {
        var content = JsonContent.Create(item);
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Sets}/{item.Id}.json";
        var response = await _client.PutAsync(requestUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Sets}/{id}.json";
        var response = await _client.DeleteAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }

    public async Task<IEnumerable<Set>> GetAllAsync()
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Sets}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exercisesJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, Set>>(exercisesJson)?.Values;
    }

    public async Task<Set?> GetByIdAsync(int id)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Sets}/{id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }

        var exerciseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Set?>(exerciseJson);
    }

    public async Task UpdateAsync(Set item)
    {
        var requestUrl = FirebaseBaseAddress + $"{FirebaseTablesNames.Sets}/{item.Id}.json";
        var response = await _client.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Http request is failed. Error code: {response.StatusCode}");
        }
    }
}

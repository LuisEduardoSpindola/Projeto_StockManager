using Application.ViaCepAPI;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class ViaCEPService
{
    private readonly HttpClient _httpClient;

    public ViaCEPService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AddressInfo> GetAddressInfo(string cep)
    {
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        response.EnsureSuccessStatusCode();
        using var responseStream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<AddressInfo>(responseStream);
    }
}
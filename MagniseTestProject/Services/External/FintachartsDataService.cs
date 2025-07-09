using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using MagniseTask.Data;
using MagniseTask.DTOs;
using MagniseTask.Interfaces;

namespace MagniseTask.Services.Common;

public class FintachartsDataService : IFintachartsDataService
{
	private HttpClient _httpClient;
	private readonly IMapper _mapper;
	public FintachartsDataService(IHttpClientFactory httpClientFactory, IMapper mapper)
	{
		_httpClient = httpClientFactory.CreateClient("FintachartsClient");
		_mapper = mapper;
	}	

	public async Task<IEnumerable<Asset>> GetAllAssets(string authHeaderValue)
	{
		Console.WriteLine(authHeaderValue);

		var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/instruments/v1/instruments");
		requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authHeaderValue);
    
		var responseMessage = await _httpClient.SendAsync(requestMessage);
    
		if (!responseMessage.IsSuccessStatusCode)
		{
			Console.WriteLine($"API request failed with status code: {responseMessage.StatusCode}");
			return new List<Asset>();
		}

		var responseString = await responseMessage.Content.ReadAsStringAsync();
		Console.WriteLine(responseString);
		
		var assetDtos = System.Text.Json.JsonSerializer.Deserialize<List<AssetDto>>(JsonDocument.Parse(responseString).RootElement.GetProperty("data").ToString(), new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		});

		if (assetDtos == null!)
		{
			Console.WriteLine("No assets found in the response.");
			return new List<Asset>();
		}

		return _mapper.Map<IEnumerable<Asset>>(assetDtos);
		
	}
}
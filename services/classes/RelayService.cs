using SmartHomeAsistent.DTO;
using SmartHomeAsistent.services.interfaces;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace SmartHomeAsistent.services.classes
{
    public class RelayService : IRelayService
    {
        private readonly string basePath;
        private readonly HttpClient _httpClient;

        public RelayService(IConfiguration configuration, HttpClient client)
        {
            basePath = configuration.GetValue<string>("ConnectionStrings:TuyaNodeServiceString") ?? throw new ArgumentNullException("командная строка не найдена");
            _httpClient = client;
        }

        public async Task<ResponseElementDTO> GetStatusAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new ResponseElementDTO { Success = false, Message = "Id is empty" };

            var json = JsonSerializer.Serialize(new { id = id });
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync($"{basePath}/status", requestBody);
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseElementDTO { Success = false, Message = $"Error: {response.ReasonPhrase}" };
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseElementDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result ?? new ResponseElementDTO { Success = false, Message = "Error" };
            }
            catch (Exception ex)
            {
                return new ResponseElementDTO { Success = false, Message = $"Exception: {ex.Message}" };
            }

        }

        public async Task<ResponseElementDTO> SwitchRelayAsync(string id, string action)
        {
            if(action != "on" && action != "off")
                return new ResponseElementDTO{Success = false, Message = "Action is not valid"};

            var json = JsonSerializer.Serialize(new { id = id });
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync($"{basePath}/{action}", requestBody);
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseElementDTO { Success = false, Message = $"Error: {response.ReasonPhrase}" };
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseElementDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result ?? new ResponseElementDTO { Success = false, Message = "Error" };
            }
            catch(Exception ex)
            {
                  return new ResponseElementDTO { Success = false, Message = $"Exception: {ex.Message}" };
            }

           
        
        }
    }
}

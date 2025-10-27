using SmartHomeAsistent.CustomExceptions;
using SmartHomeAsistent.DTO;
using SmartHomeAsistent.services.interfaces;
using System.Data;
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
                throw new ValidationException("id - не валиден");

            var json = JsonSerializer.Serialize(new { id = id });
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
           
            var response = await _httpClient.PostAsync($"{basePath}/status", requestBody);
            if (!response.IsSuccessStatusCode)
                throw new BadRequestException($"Ошибка при отправке запроса: {response.ReasonPhrase}");                 

            string content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseElementDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result ?? throw new BadRequestException("ошибка при конвертации данных");
          
          
        }

        public async Task<ResponseElementDTO> SwitchRelayAsync(string id, string action)
        {
            if (action != "on" && action != "off")
                throw new BadRequestException("Допустимые действия: on, off");
            var json = JsonSerializer.Serialize(new { id = id });
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
          
                var response = await _httpClient.PostAsync($"{basePath}/{action}", requestBody);
                if (!response.IsSuccessStatusCode)
                    throw new BadRequestException($"Ошибка при отправке запроса: {response.ReasonPhrase}");                

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseElementDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result ?? throw new BadRequestException("ошибка при конвертации данных");           
                       
        
        }
    }
}

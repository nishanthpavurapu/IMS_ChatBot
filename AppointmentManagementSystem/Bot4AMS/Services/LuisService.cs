using Bot4AMS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bot4AMS.Services
{
    public class LuisService
    {
        public static async Task<LuisResponse> ParseUserInput(string phrase)
        {
            string returnValue = string.Empty;
            string escapedString = Uri.EscapeDataString(phrase);
            using (var client = new HttpClient())
            {
                string uri = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/8dd24c70-7b91-4f36-b5a3-c7669cf93638?subscription-key=3265f3c51c984d29b1fa0bf440a64f47&q={escapedString}&verbose=true";
                var msg = await client.GetAsync(uri);
                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<LuisResponse>(jsonResponse);
                    return data;
                }
            }
            return null;
        }
    }
}
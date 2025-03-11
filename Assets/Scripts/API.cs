using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class HuggingFaceAPI
{
    private static readonly string API_KEY = "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";
    private static readonly string API_URL = "https://api-inference.huggingface.co/models/google/flan-t5-large";

    public static async Task<string> GetHuggingFaceResponse(string inputText)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

            // Build the request with additional parameters
            var requestData = new
            {
                inputs = inputText,
                parameters = new
                {
                    max_new_tokens = 60,
                    temperature = 0.2,
                    top_p = 0.8,
                    repetition_penalty = 1.1
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(API_URL, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Typically returns an array of objects with "generated_text"
                var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse);

                if (result != null && result.Count > 0 && result[0].ContainsKey("generated_text"))
                {

                    return result[0]["generated_text"];
                }
                else
                {
                    return "No generated_text found in response.";
                }
            }
            else
            {
                throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}

using Newtonsoft.Json;
using System.Text;

class Program
{
    private static readonly string apiKey = "API_KEY";

    static async Task Main(string[] args)
    {
        Console.Write("Metni Giriniz: ");
        string input;
        input = Console.ReadLine();

        if (!string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Ses dosyası oluşuturuluyor....");
            await GenerateSpeech(input); // Metni sese dönüştürme işlemini başlat
            Console.Write("Ses dosyası 'output mp3' olarak kaydedildi!");
            System.Diagnostics.Process.Start("explorer.exe", "output.mp3"); // Oluşturulan ses dosyasını aç

        }

        static async Task GenerateSpeech(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = "tts-1",
                    input = text,
                    voice = "alloy"
                };

                string json = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

                if (response.IsSuccessStatusCode)
                {
                    byte[] audioBytes = await response.Content.ReadAsByteArrayAsync(); // Ses verisini byte dizisi olarak al
                    await File.WriteAllBytesAsync("output.mp3", audioBytes); // Byte dizisini mp3 dosyası olarak kaydet
                }
                else
                {
                    Console.WriteLine("Bir hata oluştu");
                }

            }
        }
    }
}
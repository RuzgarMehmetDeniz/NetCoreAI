using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        string auodioFilePath = "TARKAN - Öp.mp3";
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var form = new MultipartFormDataContent();

            var audioContent = new ByteArrayContent(File.ReadAllBytes(auodioFilePath));
            audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
            form.Add(audioContent, "file", Path.GetFileName(auodioFilePath));
            form.Add(new StringContent("whisper-1"), "model");

            Console.WriteLine("Ses dosyası işleniyor lütfen bekleyiniz...");

            var respons = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
            if (respons.IsSuccessStatusCode)
            {
                var result = await respons.Content.ReadAsStringAsync();
                Console.WriteLine("Transkipt: ");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Hata : {respons.StatusCode}");
                Console.WriteLine(await respons.Content.ReadAsStringAsync());

            }
        }
    }
}
//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
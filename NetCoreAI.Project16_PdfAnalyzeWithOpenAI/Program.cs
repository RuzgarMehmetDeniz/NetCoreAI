using Newtonsoft.Json;
using System.Text;
using UglyToad.PdfPig;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("PDF Dosya Yolunu Giriniz: ");
        string pdfPath = Console.ReadLine() ?? string.Empty;

        if (!File.Exists(pdfPath))
        {
            Console.WriteLine("Hata: PDF dosyası bulunamadı.");
            return;
        }

        Console.WriteLine("PDF içeriği çıkartılıyor...");
        string pdfText = ExtractTextFromPdf(pdfPath);

        if (string.IsNullOrWhiteSpace(pdfText))
        {
            Console.WriteLine("PDF'den içerik alınamadı.");
            return;
        }

        Console.WriteLine("AI tarafından analiz ediliyor...\n");
        await AnalyzeWithAI(pdfText);
    }

    static string ExtractTextFromPdf(string filePath)
    {
        StringBuilder text = new StringBuilder();
        using (PdfDocument pdf = PdfDocument.Open(filePath))
        {
            foreach (var page in pdf.GetPages())
            {
                text.AppendLine(page.Text);
            }
        }
        return text.ToString();
    }

    static async Task AnalyzeWithAI(string text)
    {
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Hata: OPENAI_API_KEY environment değişkeni tanımlı değil!");
            return;
        }

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new object[]
            {
                new { role = "system", content = "Sen bir yapay zeka asistanısın. Gönderilen PDF metnini analiz edip kısa ve net şekilde Türkçe özetle." },
                new { role = "user", content = text }
            },
            max_tokens = 1000
        };

        string json = JsonConvert.SerializeObject(requestBody);
        using StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
        string responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"API Hatası: {response.StatusCode}");
            Console.WriteLine(responseJson);
            return;
        }

        dynamic result = JsonConvert.DeserializeObject(responseJson);
        string aiText = result.choices[0].message.content;

        Console.WriteLine("\n⚡ AI Özeti:\n");
        Console.WriteLine(aiText);
    }
}

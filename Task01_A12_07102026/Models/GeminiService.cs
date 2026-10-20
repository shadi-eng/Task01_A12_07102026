namespace Task01_A12_07102026
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class GeminiService
    {
        // ضع مفتاح API الخاص بك هنا محليًا.
        // يُفضّل استخدام متغيرات البيئة في المشاريع الحقيقية.
        private const string ApiKey = "AQ.Ab8RN6KswRl7j38aJRhm9zMZFPVjXWmZS8mPyDleFRZl01brwg";

        private const string ModelId = "gemini-3.8-flash";

        private static readonly HttpClient HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<string> AskAsync(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(prompt))
                return "الرجاء إدخال سؤال.";

            if (string.IsNullOrWhiteSpace(ApiKey) ||
                ApiKey == "PUT_YOUR_API_KEY_HERE")
            {
                return "الرجاء إعداد مفتاح Gemini API.";
            }

            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/{ModelId}:generateContent?key={ApiKey}";

            var requestData = new
            {
                contents = new[]
    {
        new
        {
            parts = new[]
            {
                new { text = prompt }
            }
        }
    },
                generationConfig = new
                {
                    thinkingConfig = new
                    {
                        thinkingLevel = "low"
                    },
                    maxOutputTokens = 512
                }
            };

            try
            {
                string json = JsonSerializer.Serialize(
                    requestData, JsonOptions);

                using var content = new StringContent(
                    json, Encoding.UTF8, "application/json");

                using var response = await HttpClient.PostAsync(
                    url, content, cancellationToken);

                string responseContent =
                    await response.Content.ReadAsStringAsync(
                        cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = responseContent;

                    try
                    {
                        using var errorDoc = JsonDocument.Parse(responseContent);

                        if (errorDoc.RootElement.TryGetProperty(
                                "error", out var error) &&
                            error.TryGetProperty("message", out var message))
                        {
                            errorMessage = message.GetString() ?? responseContent;
                        }
                    }
                    catch (JsonException)
                    {
                        // الاحتفاظ برسالة الاستجابة الأصلية إذا لم تكن JSON صحيحة.
                    }

                    return $"فشل Gemini API: HTTP {(int)response.StatusCode}\n" +
                           $"التفاصيل: {errorMessage}";
                }

                using var doc = JsonDocument.Parse(responseContent);
                var root = doc.RootElement;

                if (!root.TryGetProperty("candidates", out var candidates) ||
                    candidates.ValueKind != JsonValueKind.Array ||
                    candidates.GetArrayLength() == 0)
                {
                    return "لم يُرجع Gemini إجابة. قد يكون الطلب قد انتهى دون نص.";
                }

                var candidate = candidates[0];

                if (!candidate.TryGetProperty("content", out var responseBody) ||
                    !responseBody.TryGetProperty("parts", out var parts) ||
                    parts.ValueKind != JsonValueKind.Array)
                {
                    return "لم يتم العثور على نص في استجابة Gemini.";
                }

                var result = new StringBuilder();

                foreach (var part in parts.EnumerateArray())
                {
                    if (part.TryGetProperty("text", out var textElement) &&
                        textElement.ValueKind == JsonValueKind.String)
                    {
                        result.Append(textElement.GetString());
                    }
                }

                return result.Length > 0
                    ? result.ToString()
                    : "لا يوجد نص في إجابة Gemini.";
            }
            catch (OperationCanceledException)
                when (!cancellationToken.IsCancellationRequested)
            {
                return "انتهت مهلة الاتصال بـ Gemini API.";
            }
            catch (OperationCanceledException)
            {
                // نحافظ على سلوك إلغاء الطلب للمستدعي.
                throw;
            }
            catch (HttpRequestException)
            {
                return "تعذر الاتصال بـ Gemini API. تحقق من الإنترنت.";
            }
            catch (JsonException)
            {
                return "تعذر تحليل استجابة Gemini.";
            }
            catch (Exception)
            {
                return "حدث خطأ غير متوقع أثناء تنفيذ الطلب.";
            }
        }
    }
}
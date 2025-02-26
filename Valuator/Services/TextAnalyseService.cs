using Valuator.ViewModel;

namespace Valuator.Services
{
    public class TextAnalyseService
    {
        public double CalculateRank(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0.0;

            int nonAlphabetical = text.Count(ch => !char.IsLetter(ch));
            return (double)nonAlphabetical / text.Length;
        }

        public async Task<int> CalculateSimilarityAsync(string text, string key, RedisStorage storage)
        {
            return (await storage.FindKeyByValueAsync(text, key) != null) ? 1 : 0;
        }
    }
}

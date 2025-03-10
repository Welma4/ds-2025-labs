using Valuator.Storage;

namespace Valuator.Services
{
    public class TextAnalyseService
    {
        public double CalculateRank(string text)
        {
            int nonAlphabetical = text.Count(ch => !char.IsLetter(ch));
            return (double)nonAlphabetical / text.Length;
        }

        public async Task<int> CalculateSimilarityAsync(string text, IRedisStorage storage)
        {
            return (await storage.FindKeyByValueAsync(text) != null) ? 1 : 0;
        }
    }
}

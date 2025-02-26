using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using Valuator.Services;
using Valuator.ViewModel;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly RedisStorage _formTextStorage;
    private readonly TextAnalyseService _textAnalyseService;
    public IndexModel(ILogger<IndexModel> logger, RedisStorage formTextStorage, TextAnalyseService textAnalyseService)
    {
        _logger = logger;
        _formTextStorage = formTextStorage;
        _textAnalyseService = textAnalyseService;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPost(string text)
    {
        _logger.LogDebug(text);

        string id = Guid.NewGuid().ToString();

        string textKey = "TEXT-" + id;
        await _formTextStorage.AddKeyValue(textKey, text);

        string rankKey = "RANK-" + id;
        double rank = _textAnalyseService.CalculateRank(text);
        await _formTextStorage.AddKeyValue(rankKey, rank.ToString());

        string similarityKey = "SIMILARITY-" + id;
        int similarity = await _textAnalyseService.CalculateSimilarityAsync(text, textKey, _formTextStorage);
        await _formTextStorage.AddKeyValue(similarityKey, similarity.ToString());

        return Redirect($"summary?id={id}");
    }
}

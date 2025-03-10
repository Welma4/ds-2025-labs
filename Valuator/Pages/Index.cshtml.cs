using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Services;
using Valuator.Storage;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IRedisStorage _storage;
    private readonly TextAnalyseService _textAnalyseService;
    public IndexModel(ILogger<IndexModel> logger, IRedisStorage storage, TextAnalyseService textAnalyseService)
    {
        _logger = logger;
        _storage = storage;
        _textAnalyseService = textAnalyseService;
    }

    public string ErrorMessage { get; set; } 

    public void OnGet() { }

    public async Task<IActionResult> OnPost(string text)
    {
        if (text == null)
        {
            ErrorMessage = "Text cannot be empty!";
            return Page();
        }

        _logger.LogDebug(text);

        string id = Guid.NewGuid().ToString();

        string rankKey = "RANK-" + id;
        double rank = _textAnalyseService.CalculateRank(text);
        await _storage.AddKeyValue(rankKey, rank.ToString());

        string similarityKey = "SIMILARITY-" + id;
        int similarity = await _textAnalyseService.CalculateSimilarityAsync(text, _storage);
        await _storage.AddKeyValue(similarityKey, similarity.ToString());

        string textKey = "TEXT-" + id;
        await _storage.AddKeyValue(textKey, text);

        return Redirect($"summary?id={id}");
    }
}

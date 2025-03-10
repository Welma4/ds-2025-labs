using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Storage;
using Valuator.ViewModel;

namespace Valuator.Pages;
public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IRedisStorage _formTextStorage;

    public SummaryModel(ILogger<SummaryModel> logger, IRedisStorage formTextStorage)
    {
        _logger = logger;
        _formTextStorage = formTextStorage;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public async Task OnGet(string id)
    {
        _logger.LogDebug(id);

        string StRank = await _formTextStorage.GetValue("RANK-" + id);
        Rank = double.TryParse(StRank, out double rankValue) ? rankValue : 0.0;

        string StSim = await _formTextStorage.GetValue("SIMILARITY-" + id);
        Similarity = int.TryParse(StSim, out int simValue) ? simValue : 0;
    }
}

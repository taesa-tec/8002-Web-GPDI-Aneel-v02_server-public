using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.Chunkers;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace PeD.Services
{
    public class DiffService
    {
        public static DiffPaneModel Html(string oldHtml, string newHtml)
        {
            var diffBuilder = new InlineDiffBuilder(new Differ());
            IChunker chunker;

            chunker = new CustomFunctionChunker(s =>
                Regex.Split(s, "(<[\\w|\\d]+(?:\\b[^>]*)?>\\s*|\\s*</[\\w|\\d]+>\\s*)"));

            return diffBuilder.BuildDiffModel(
                oldHtml, //HttpUtility.HtmlDecode(htmlOld.DocumentNode.InnerText),
                newHtml,
                true,
                true,
                chunker
            );
        }
    }
}
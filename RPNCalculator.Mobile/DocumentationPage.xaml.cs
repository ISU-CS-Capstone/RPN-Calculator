using System.Reflection;
using Markdig;
using RPNCalculator.Common;

namespace RPNCalculator.Mobile;

public partial class DocumentationPage : ContentPage
{
    public DocumentationPage()
    {
        InitializeComponent();
        LoadMarkdownContent();
    }

    private async void LoadMarkdownContent()
    {
        var assembly = Assembly.GetAssembly(typeof(RPNCalculator.Common.RpnCalculator));
        Stream stream = assembly.GetManifestResourceStream("RPNCalculator.Common.Documentation.md");
        if (stream == null)
        {
            // Log error or handle the case where the stream is null
            Console.WriteLine("Documentation.md not found as an embedded resource.");
            return;
        }

        using (var reader = new StreamReader(stream))
        {
            var markdown = await reader.ReadToEndAsync();
            Console.WriteLine(markdown);
            var html = Markdown.ToHtml(markdown);
            DocWebView.Source = new HtmlWebViewSource { Html = html };
        }
    }
}
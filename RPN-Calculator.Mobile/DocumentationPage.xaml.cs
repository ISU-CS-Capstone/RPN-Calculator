using System.Reflection;
using Markdig;

namespace RPN_Calculator.Mobile;

public partial class DocumentationPage : ContentPage
{
	public DocumentationPage()
	{
		InitializeComponent();
		LoadMarkdownContent();
	}

    private async void LoadMarkdownContent()
    {
        var assembly = typeof(DocumentationPage).Assembly;
        Stream stream = assembly.GetManifestResourceStream("RPN_Calculator.Common.Documentation.md");
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
            webView.Source = new HtmlWebViewSource { Html = html };
        }
    }
}
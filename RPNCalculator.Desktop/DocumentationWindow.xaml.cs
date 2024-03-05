using System.IO;
using System.Reflection;
using System.Windows;
using Markdig;
using RPNCalculator.Common;

namespace RPNCalculator.Desktop;

public partial class DocumentationWindow : Window
{
    public DocumentationWindow()
    {
        InitializeComponent();
        LoadDocumentation();
    }

    private void LoadDocumentation()
    {
        var assembly = Assembly.GetAssembly(typeof(RPNCalculator.Common.RpnCalculator));
        Stream stream = assembly.GetManifestResourceStream("RPNCalculator.Common.Documentation.md");
        using (var reader = new StreamReader(stream))
        {
            var markdown = reader.ReadToEnd();
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var htmlContent = Markdown.ToHtml(markdown, pipeline);

            // Embed compiled markdown into html to apply CSS styles
            var styledHtmlContent = $@"
                                    <html>
                                    <head>
                                        <style>
                                            body {{
                                                font-family: 'Segoe UI', Arial, sans-serif;
                                                margin: 10px;
                                            }}
                                            h1, h2, h3 {{
                                                color: navy;
                                            }}
                                            p {{
                                                font-size: 14px;
                                            }}
                                            a {{
                                                color: #0078D7;
                                                text-decoration: none;
                                            }}
                                            /* Add more styles as needed */
                                        </style>
                                    </head>
                                    <body>
                                        {htmlContent}
                                    </body>
                                    </html>";

            // Display html inside DocumentationBrowser of the page
            DocumentationBrowser.NavigateToString(styledHtmlContent);
        }
    }


    private void OnSubmitClicked(object sender, RoutedEventArgs e)
    {
        var userInput = InputBox.Text;
        // Implement logic based on userInput. Example:
        MessageBox.Show($"This is not implemented yet. You entered: {userInput}", "Input Received");
    }
}
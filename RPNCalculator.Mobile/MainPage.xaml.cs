namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnDocumentationLinkClicked(object sender, EventArgs e)
    {
        // Navigate to the DocumentationPage
        await Shell.Current.GoToAsync("///DocumentationPage");
    }
}

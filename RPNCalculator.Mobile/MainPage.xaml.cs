﻿namespace RPNCalculator.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void OnDocumentationLinkClicked(object sender, EventArgs e)
    {
        // Navigate to the DocumentationPage
        await Shell.Current.GoToAsync("///DocumentationPage");
    }
}

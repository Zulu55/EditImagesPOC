namespace EditImagesPOC.Pages;

public partial class Home
{
    private bool ShowTextInput = false;
    private string InputText = string.Empty;
    private string OverlayText = string.Empty;

    private void ShowInput()
    {
        ShowTextInput = true;
        InputText = string.Empty;
    }

    private void AddText()
    {
        OverlayText = InputText;
        ShowTextInput = false;
    }

    private void Cancel()
    {
        ShowTextInput = false;
    }
}
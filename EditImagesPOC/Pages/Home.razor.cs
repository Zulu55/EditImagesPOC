using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace EditImagesPOC.Pages;

public partial class Home
{
    private bool ShowTextInput = false;
    private string InputText = string.Empty;
    private double ClickX;
    private double ClickY;
    private bool AwaitingTextPosition = false;

    private bool DrawingRectangle = false;
    private double RectStartX, RectStartY;
    private bool RectFirstClick = true;

    private bool DrawingCircle = false;
    private double CircleCenterX, CircleCenterY;
    private bool CircleFirstClick = true;

    [Inject] private IJSRuntime JS { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("drawImageOnCanvas", "images/truck.jpg");
        }
    }

    private async Task SaveImage()
    {
        await JS.InvokeVoidAsync("saveCanvasAsImage", "image-edited.png");
    }

    private async Task OnImageSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB max
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();
            var base64 = Convert.ToBase64String(bytes);
            var imageDataUrl = $"data:{file.ContentType};base64,{base64}";
            await JS.InvokeVoidAsync("drawImageOnCanvas", imageDataUrl);
        }
    }

    private async Task ClearCanvas()
    {
        await JS.InvokeVoidAsync("drawImageOnCanvas", "images/truck.jpg");
    }

    private void ShowInput()
    {
        AwaitingTextPosition = true;
        ShowTextInput = false;
        InputText = string.Empty;
        DrawingRectangle = false;
    }

    private async void OnCanvasClick(MouseEventArgs e)
    {
        if (AwaitingTextPosition)
        {
            ClickX = e.OffsetX;
            ClickY = e.OffsetY;
            ShowTextInput = true;
            AwaitingTextPosition = false;
            StateHasChanged();
        }
        else if (DrawingRectangle)
        {
            if (RectFirstClick)
            {
                RectStartX = e.OffsetX;
                RectStartY = e.OffsetY;
                RectFirstClick = false;
            }
            else
            {
                double rectEndX = e.OffsetX;
                double rectEndY = e.OffsetY;
                double x = Math.Min(RectStartX, rectEndX);
                double y = Math.Min(RectStartY, rectEndY);
                double width = Math.Abs(rectEndX - RectStartX);
                double height = Math.Abs(rectEndY - RectStartY);

                await JS.InvokeVoidAsync("drawRectangleOnCanvas", x, y, width, height);
                DrawingRectangle = false;
                RectFirstClick = true;
            }
        }
        else if (DrawingCircle)
        {
            if (CircleFirstClick)
            {
                CircleCenterX = e.OffsetX;
                CircleCenterY = e.OffsetY;
                CircleFirstClick = false;
            }
            else
            {
                double dx = e.OffsetX - CircleCenterX;
                double dy = e.OffsetY - CircleCenterY;
                double radius = Math.Sqrt(dx * dx + dy * dy);

                await JS.InvokeVoidAsync("drawCircleOnCanvas", CircleCenterX, CircleCenterY, radius);
                DrawingCircle = false;
                CircleFirstClick = true;
            }
        }
    }

    private async Task AddText()
    {
        await JS.InvokeVoidAsync("drawTextOnCanvas", InputText, ClickX, ClickY);
        ShowTextInput = false;
        InputText = string.Empty;
    }

    private void Cancel()
    {
        ShowTextInput = false;
        InputText = string.Empty;
    }

    private void StartRectangle()
    {
        DrawingRectangle = true;
        RectFirstClick = true;
        AwaitingTextPosition = false;
        ShowTextInput = false;
    }

    private void StartCircle()
    {
        DrawingCircle = true;
        CircleFirstClick = true;
        DrawingRectangle = false;
        AwaitingTextPosition = false;
        ShowTextInput = false;
    }
}
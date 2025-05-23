using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace EditImagesPOC.Pages;

public partial class Home
{
    private bool showTextInput = false;
    private string inputText = string.Empty;
    private double clickX;
    private double clickY;
    private bool awaitingTextPosition = false;

    private bool drawingRectangle = false;
    private double rectStartX;
    private double rectStartY;
    private bool rectFirstClick = true;

    private bool drawingCircle = false;
    private double circleCenterX;
    private double circleCenterY;
    private bool circleFirstClick = true;

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
        awaitingTextPosition = true;
        showTextInput = false;
        inputText = string.Empty;
        drawingRectangle = false;
    }

    private async void OnCanvasClick(MouseEventArgs e)
    {
        if (awaitingTextPosition)
        {
            clickX = e.OffsetX;
            clickY = e.OffsetY;
            showTextInput = true;
            awaitingTextPosition = false;
            StateHasChanged();
        }
        else if (drawingRectangle)
        {
            if (rectFirstClick)
            {
                rectStartX = e.OffsetX;
                rectStartY = e.OffsetY;
                rectFirstClick = false;
            }
            else
            {
                double rectEndX = e.OffsetX;
                double rectEndY = e.OffsetY;
                double x = Math.Min(rectStartX, rectEndX);
                double y = Math.Min(rectStartY, rectEndY);
                double width = Math.Abs(rectEndX - rectStartX);
                double height = Math.Abs(rectEndY - rectStartY);

                await JS.InvokeVoidAsync("drawRectangleOnCanvas", x, y, width, height);
                drawingRectangle = false;
                rectFirstClick = true;
            }
        }
        else if (drawingCircle)
        {
            if (circleFirstClick)
            {
                circleCenterX = e.OffsetX;
                circleCenterY = e.OffsetY;
                circleFirstClick = false;
            }
            else
            {
                double dx = e.OffsetX - circleCenterX;
                double dy = e.OffsetY - circleCenterY;
                double radius = Math.Sqrt(dx * dx + dy * dy);

                await JS.InvokeVoidAsync("drawCircleOnCanvas", circleCenterX, circleCenterY, radius);
                drawingCircle = false;
                circleFirstClick = true;
            }
        }
    }

    private async Task AddText()
    {
        await JS.InvokeVoidAsync("drawTextOnCanvas", inputText, clickX, clickY);
        showTextInput = false;
        inputText = string.Empty;
    }

    private void Cancel()
    {
        showTextInput = false;
        inputText = string.Empty;
    }

    private void StartRectangle()
    {
        drawingRectangle = true;
        rectFirstClick = true;
        awaitingTextPosition = false;
        showTextInput = false;
    }

    private void StartCircle()
    {
        drawingCircle = true;
        circleFirstClick = true;
        drawingRectangle = false;
        awaitingTextPosition = false;
        showTextInput = false;
    }
}
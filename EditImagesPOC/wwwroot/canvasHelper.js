window.drawImageOnCanvas = (imageFileName) => {
    const canvas = document.getElementById('imageCanvas');
    const ctx = canvas.getContext('2d');
    const img = new Image();
    img.onload = function () {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
    img.src = imageFileName;
};

window.drawTextOnCanvas = (text, x, y) => {
    const canvas = document.getElementById('imageCanvas');
    const ctx = canvas.getContext('2d');
    ctx.font = "20px Arial";
    ctx.fillStyle = "white";
    ctx.strokeStyle = "black";
    ctx.lineWidth = 2;
    ctx.strokeText(text, x, y);
    ctx.fillText(text, x, y);
};
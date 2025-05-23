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

window.drawRectangleOnCanvas = (x, y, width, height) => {
    const canvas = document.getElementById('imageCanvas');
    const ctx = canvas.getContext('2d');
    ctx.save();
    ctx.strokeStyle = "red";
    ctx.lineWidth = 3;
    ctx.strokeRect(x, y, width, height);
    ctx.restore();
};

window.drawCircleOnCanvas = (centerX, centerY, radius) => {
    const canvas = document.getElementById('imageCanvas');
    const ctx = canvas.getContext('2d');
    ctx.save();
    ctx.strokeStyle = "red";
    ctx.lineWidth = 3;
    ctx.beginPath();
    ctx.arc(centerX, centerY, radius, 0, 2 * Math.PI);
    ctx.stroke();
    ctx.restore();
};

window.saveCanvasAsImage = (filename) => {
    const canvas = document.getElementById('imageCanvas');
    const link = document.createElement('a');
    link.download = filename || 'edited-image.png';
    link.href = canvas.toDataURL('image/png');
    link.click();
};
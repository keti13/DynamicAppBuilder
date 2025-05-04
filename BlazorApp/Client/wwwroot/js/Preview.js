window.scaleCanvasToFit = () => {
    const container = document.getElementById('preview-container');
    const canvas = document.getElementById('canvas-inner');

    if (!container || !canvas) return;

    const containerWidth = container.offsetWidth;
    const containerHeight = container.offsetHeight;

    const canvasWidth = canvas.offsetWidth;
    const canvasHeight = canvas.offsetHeight;

    const scaleX = containerWidth / canvasWidth;
    const scaleY = containerHeight / canvasHeight;

    const scale = Math.min(scaleX, scaleY);

    canvas.style.transformOrigin = "top left";
    canvas.style.transform = `scale(${scale})`;
};

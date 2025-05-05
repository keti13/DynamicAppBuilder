window.getViewportWidth = () => {
    return window.innerWidth || document.documentElement.clientWidth;
};

window.registerResizeHandler = (dotNetHelper) => {
    window.addEventListener("resize", () => {
        dotNetHelper.invokeMethodAsync("OnViewportResize", window.innerWidth);
    });
};

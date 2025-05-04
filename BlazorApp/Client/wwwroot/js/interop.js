window.getElementSize = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element) return { width: 0, height: 0 };
    const rect = element.getBoundingClientRect();
    return {
        width: rect.width,
        height: rect.height
    };
};
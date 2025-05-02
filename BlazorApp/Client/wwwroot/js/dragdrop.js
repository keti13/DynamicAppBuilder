// Super simple drag-drop implementation that bypasses HTML5 API issues
// We use a global store for both the drag data and last coordinates

// Global storage for drag-drop state
window.dragDropState = {
    // The currently dragged control type
    currentType: '',
    // Last known mouse position
    lastX: 0,
    lastY: 0,
    // Flag to indicate if we're tracking mouse movement
    isTracking: false
};

// Track mouse movement globally to ensure we always have the latest position
document.addEventListener('mousemove', function (e) {
    // Always track mouse position regardless of drag state
    window.dragDropState.lastX = e.clientX;
    window.dragDropState.lastY = e.clientY;

    if (window.dragDropState.isTracking) {
        console.log("Tracking mouse: ", e.clientX, e.clientY);
    }
});

// Called when drag starts on a component
window.setDragData = function (event, data) {
    try {
        console.log("setDragData called with data:", data);
        // Store data in our global state
        window.dragDropState.currentType = data;
        window.dragDropState.isTracking = true;

        // Still try to use dataTransfer for browsers that support it
        if (event && event.dataTransfer) {
            try {
                event.dataTransfer.setData("text/plain", data);
                event.dataTransfer.effectAllowed = "copy";
            } catch (err) {
                console.log("Browser doesn't support dataTransfer.setData, using fallback.");
            }
        }

        console.log("✅ Drag data set successfully:", data);
        return true;
    } catch (error) {
        console.error("❌ Error in setDragData:", error);
        return false;
    }
};

// Called when drop happens
window.getDragData = function (event) {
    try {
        console.log("getDragData called");
        // Just return our stored data
        const result = window.dragDropState.currentType;
        console.log("Retrieved drag data:", result);

        // Reset tracking state
        window.dragDropState.isTracking = false;
        window.dragDropState.currentType = '';

        return result;
    } catch (error) {
        console.error("❌ Error in getDragData:", error);
        return "";
    }
};

// Get the drop position (relative to container)
window.getDropPosition = function (event, containerId) {
    try {
        console.log("getDropPosition called for container:", containerId);
        const container = document.getElementById(containerId);
        if (!container) {
            console.error("Container not found:", containerId);
            return JSON.stringify({ x: 50, y: 50 });
        }

        // Try multiple approaches to get coordinates
        let clientX, clientY;

        // First try the event object directly (works in some browsers)
        if (event && typeof event.clientX === 'number' && typeof event.clientY === 'number') {
            clientX = event.clientX;
            clientY = event.clientY;
            console.log("Using event coordinates:", clientX, clientY);
        }
        // Then try our tracked mouse position (should always work as fallback)
        else {
            clientX = window.dragDropState.lastX;
            clientY = window.dragDropState.lastY;
            console.log("Using tracked coordinates:", clientX, clientY);
        }

        // If we somehow still don't have valid coordinates, use a reasonable default
        if (!clientX && !clientY) {
            console.log("No coordinates available, using defaults");
            // Place near the middle-top of the container
            const rect = container.getBoundingClientRect();
            return JSON.stringify({
                x: Math.round(rect.width / 2),
                y: 50
            });
        }

        // Calculate position relative to container
        const rect = container.getBoundingClientRect();
        const x = Math.max(0, Math.round(clientX - rect.left));
        const y = Math.max(0, Math.round(clientY - rect.top));

        console.log("Final drop position:", { x, y });
        return JSON.stringify({ x, y });
    } catch (error) {
        console.error("❌ Error in getDropPosition:", error);
        return JSON.stringify({ x: 50, y: 50 });
    }
};

window.getBoundingClientRect = function (elementId) {
    const element = document.getElementById(elementId);
    if (!element) return null;
    
    const rect = element.getBoundingClientRect();
    return {
        left: rect.left,
        top: rect.top,
        right: rect.right,
        bottom: rect.bottom,
        width: rect.width,
        height: rect.height
    };
};
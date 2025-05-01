window.setDragData = function (event, data) {
    try {
        console.log("setDragData called:", event, data);
        event.dataTransfer.setData("text/plain", data);
        event.dataTransfer.effectAllowed = "copy";
        console.log("✅ Drag data set successfully");
    } catch (error) {
        console.error("❌ Error in setDragData:", error);
    }
};

window.getDragData = function (event) {
    console.log("getDragData called");
    return event.dataTransfer.getData("text/plain");
}; 
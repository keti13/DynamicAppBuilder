﻿window.enableDrag = function (selector) {
    interact(selector).draggable({
        inertia: true,
        modifiers: [
            interact.modifiers.restrictRect({
                restriction: '#canvas-dropzone',
                endOnly: true,
                elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
            })
        ],
        listeners: {
            move(event) {
                const target = event.target;

                const x = (parseFloat(target.dataset.x) || 0) + event.dx;
                const y = (parseFloat(target.dataset.y) || 0) + event.dy;

                target.style.transform = `translate(${x}px, ${y}px)`;
                target.dataset.x = x;
                target.dataset.y = y;
            },

            end(event) {
                const target = event.target;
                const id = target.getAttribute('id')?.replace("control-", "");
                const canvas = document.getElementById('canvas-dropzone');
                const canvasRect = canvas.getBoundingClientRect();
                const targetRect = target.getBoundingClientRect();

                const relativeX = targetRect.left - canvasRect.left;
                const relativeY = targetRect.top - canvasRect.top;

                // Reset the transform
                target.style.transform = '';
                target.style.left = `${relativeX}px`;
                target.style.top = `${relativeY}px`;

                // Reset dataset
                target.dataset.x = 0;
                target.dataset.y = 0;

                // Notify Blazor
                if (window.blazorCanvas && id) {
                    window.blazorCanvas.invokeMethodAsync("UpdateControlPosition", id, relativeX, relativeY);
                }
            }
        }
    });
};


window.registerBlazorCanvasRef = function (dotNetRef) {
    window.blazorCanvas = dotNetRef;
};

function isValidDropTarget(event) {
    const dropzone = document.getElementById('canvas-dropzone');
    if (!dropzone) return false;

    const rect = dropzone.getBoundingClientRect();
    const buffer = 60; // 1px tighter

    // Get coordinates based on event type (touch or mouse)
    const clientX = event.clientX || (event.touches && event.touches[0]?.clientX);
    const clientY = event.clientY || (event.touches && event.touches[0]?.clientY);

    if (!clientX || !clientY) return false;

    return (
        clientX >= rect.left &&
        clientX < rect.right - buffer &&
        clientY >= rect.top &&
        clientY < rect.bottom - buffer
    );
}

window.setupComponentDragToCanvas = function (dotNetRef) {
    const canvas = document.getElementById('canvas-dropzone');

    interact('.draggable-source').draggable({
        inertia: true,
        modifiers: [
            interact.modifiers.restrictRect({
                restriction: '#canvas-dropzone',
                endOnly: true,
                elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
            })
        ],
        listeners: {
            start(event) {
                event.target.classList.add('is-dragging');
            },

            move(event) {
                const target = event.target;
                const x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx;
                const y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

                target.style.transform = `translate(${x}px, ${y}px)`;
                target.setAttribute('data-x', x);
                target.setAttribute('data-y', y);

                // Visual feedback for hovering over canvas
                const canvas = document.getElementById('canvas-dropzone');
                const rect = canvas.getBoundingClientRect();
                const clientX = event.clientX || (event.touches && event.touches[0]?.clientX);
                const clientY = event.clientY || (event.touches && event.touches[0]?.clientY);

                if (clientX && clientY) {
                    const isOverCanvas =
                        clientX >= rect.left &&
                        clientX <= rect.right &&
                        clientY >= rect.top &&
                        clientY <= rect.bottom;

                    canvas.classList.toggle('drop-active', isOverCanvas);
                }
            },

            end(event) {
                const canvas = document.getElementById('canvas-dropzone');
                const rect = canvas.getBoundingClientRect();

                if (isValidDropTarget(event)) {
                    const controlType = event.target.getAttribute('data-control');
                    const clientX = event.clientX || (event.changedTouches && event.changedTouches[0]?.clientX);
                    const clientY = event.clientY || (event.changedTouches && event.changedTouches[0]?.clientY);

                    if (clientX && clientY) {
                        const dropX = clientX - rect.left;
                        const dropY = clientY - rect.top;

                        dotNetRef.invokeMethodAsync('HandleDropFromJS', controlType, dropX, dropY);
                    }
                }
                else {
                    console.log("Drop outside canvas prevented.");
                }

                // Reset visuals
                event.target.classList.remove('is-dragging');
                event.target.style.transform = '';
                event.target.setAttribute('data-x', 0);
                event.target.setAttribute('data-y', 0);
                canvas.classList.remove('drop-active');
            }
        }
    });
};


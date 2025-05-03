window.enableDrag = function (selector) {
    interact(selector).draggable({
        inertia: true,
        modifiers: [
            interact.modifiers.restrictRect({
                restriction: '#canvas-dropzone',  // restrict to canvas
                endOnly: true,                    // restrict only at drag end
                elementRect: { top: 0, left: 0, bottom: 1.2, right: 1 }
            })
        ],
        listeners: {
            move(event) {
                const target = event.target;

                let x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx;
                let y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

                target.style.transform = `translate(${x}px, ${y}px)`;
                target.setAttribute('data-x', x);
                target.setAttribute('data-y', y);
            }
        }
    });
};


function isValidDropTarget(event) {
    const dropzone = document.getElementById('canvas-dropzone');
    if (!dropzone) return false;

    const rect = dropzone.getBoundingClientRect();

    return (
        event.clientX >= rect.left &&
        event.clientX <= rect.right &&
        event.clientY >= rect.top &&
        event.clientY <= rect.bottom
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
                elementRect: { top: 0, left: 0, bottom: 1, right: 1 } // 🧠 anchor the whole element
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
                const rect = canvas.getBoundingClientRect();
                const isOverCanvas =
                    event.client.x >= rect.left &&
                    event.client.x <= rect.right &&
                    event.client.y >= rect.top &&
                    event.client.y <= rect.bottom;

                canvas.classList.toggle('drop-active', isOverCanvas);
            },

            end(event) {
                const rect = canvas.getBoundingClientRect();

                const controlType = event.target.getAttribute('data-control');
                if (!controlType) return;

                const dropX = event.clientX - rect.left + canvas.scrollLeft;
                const dropY = event.clientY - rect.top + canvas.scrollTop;

                // Fire the Blazor method to drop the component
                dotNetRef.invokeMethodAsync('HandleDropFromJS', controlType, dropX, dropY);

                // Reset styles
                event.target.classList.remove('is-dragging');
                event.target.style.transform = '';
                event.target.setAttribute('data-x', 0);
                event.target.setAttribute('data-y', 0);
                canvas.classList.remove('drop-active');
            }
        }
    });
};


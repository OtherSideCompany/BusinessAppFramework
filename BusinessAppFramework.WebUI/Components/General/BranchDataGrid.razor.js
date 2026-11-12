const gripSelector = '.osc-row-grip';
const containerSelector = '.osc-reorder-container';
const states = new WeakMap();

export function initialize(container, dotNetReference) {
    if (!container || states.has(container))
        return;

    let draggedRow = null;
    let draggedNodeId = null;
    let indicatorRow = null;

    const clearIndicator = () => {
        if (indicatorRow) {
            indicatorRow.classList.remove('osc-row-drop-before', 'osc-row-drop-after');
            indicatorRow = null;
        }
    };

    const reset = () => {
        clearIndicator();

        if (draggedRow) {
            draggedRow.classList.remove('osc-row-dragging');
        }

        draggedRow = null;
        draggedNodeId = null;
    };

    const ownsGrip = (grip) => grip.closest(containerSelector) === container;

    const findTarget = (event) => {
        const row = event.target instanceof Element ? event.target.closest('tr') : null;

        if (!row || row === draggedRow)
            return null;

        const grip = row.querySelector(gripSelector);

        if (!grip || grip.closest('tr') !== row || !ownsGrip(grip))
            return null;

        const bounds = row.getBoundingClientRect();
        const insertAfter = event.clientY > bounds.top + bounds.height / 2;

        const rows = Array.from(container.querySelectorAll(gripSelector)).filter(ownsGrip).map(g => g.closest('tr'));
        const draggedPosition = rows.indexOf(draggedRow);

        if (draggedPosition >= 0 && rows.indexOf(row) === draggedPosition + (insertAfter ? -1 : 1))
            return null;

        return {
            row: row,
            nodeId: parseInt(grip.dataset.nodeId, 10),
            insertAfter: insertAfter
        };
    };

    const onDragStart = (event) => {
        const grip = event.target instanceof Element ? event.target.closest(gripSelector) : null;

        if (!grip || !ownsGrip(grip))
            return;

        draggedRow = grip.closest('tr');
        draggedNodeId = parseInt(grip.dataset.nodeId, 10);

        event.dataTransfer.effectAllowed = 'move';
        event.dataTransfer.setData('text/plain', grip.dataset.nodeId);

        if (draggedRow) {
            const bounds = draggedRow.getBoundingClientRect();
            event.dataTransfer.setDragImage(draggedRow, event.clientX - bounds.left, event.clientY - bounds.top);
            draggedRow.classList.add('osc-row-dragging');
        }
    };

    const onDragOver = (event) => {
        if (draggedNodeId === null)
            return;

        const target = findTarget(event);

        clearIndicator();

        if (!target)
            return;

        event.preventDefault();
        event.dataTransfer.dropEffect = 'move';

        indicatorRow = target.row;
        indicatorRow.classList.add(target.insertAfter ? 'osc-row-drop-after' : 'osc-row-drop-before');
    };

    const onDrop = async (event) => {
        if (draggedNodeId === null)
            return;

        const target = findTarget(event);
        const movedNodeId = draggedNodeId;

        reset();

        if (!target)
            return;

        event.preventDefault();

        await dotNetReference.invokeMethodAsync('OnRowMovedAsync', movedNodeId, target.nodeId, target.insertAfter);
    };

    container.addEventListener('dragstart', onDragStart);
    container.addEventListener('dragover', onDragOver);
    container.addEventListener('drop', onDrop);
    container.addEventListener('dragend', reset);

    states.set(container, { onDragStart, onDragOver, onDrop, reset });
}

export function dispose(container) {
    const state = container ? states.get(container) : null;

    if (!state)
        return;

    container.removeEventListener('dragstart', state.onDragStart);
    container.removeEventListener('dragover', state.onDragOver);
    container.removeEventListener('drop', state.onDrop);
    container.removeEventListener('dragend', state.reset);

    states.delete(container);
}

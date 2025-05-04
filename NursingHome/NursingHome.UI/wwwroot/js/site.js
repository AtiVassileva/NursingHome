function setupTableSearch(inputId, tableSelector) {
    const input = document.getElementById(inputId);
    const table = document.querySelector(tableSelector);
    if (!input || !table) return;

    const rows = table.querySelectorAll("tbody tr");

    input.addEventListener("input", function () {
        const searchTerm = input.value.toLowerCase();
        rows.forEach(row => {
            const nameCell = row.querySelector("td");
            const name = nameCell.textContent.toLowerCase();
            row.style.display = name.includes(searchTerm) ? "" : "none";
        });
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const deleteModal = document.getElementById('deleteModal');
    if (!deleteModal) return;

    deleteModal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget;
        const itemName = button.getAttribute('data-item-name') || 'този елемент';
        const deleteUrl = button.getAttribute('data-delete-url');

        const modalItemName = deleteModal.querySelector('#deleteItemName');
        const form = deleteModal.querySelector('#deleteForm');

        modalItemName.textContent = itemName;
        form.action = deleteUrl;
    });
});

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
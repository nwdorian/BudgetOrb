let modal;
const container = document.getElementById("create-transaction-modal-placeholder");

async function loadCreateModal() {
    try {
        const response = await fetch("/Transactions/Create");
        if (!response.ok) {
            throw new Error("Failed to load modal");
        }
        const html = await response.text();
        container.innerHTML = html;

        modal = new bootstrap.Modal(document.getElementById("create-transaction-modal"));
        modal.show();
    } catch (error) {
        alert("Something went wrong while loading the form");
    }
}

async function submitForm(form, event) {
    event.preventDefault();

    try {
        const response = await fetch(form.action, {
            method: "POST",
            body: new FormData(form),
        });

        if (!response.ok) {
            window.location.href = "/Errors/Error";
            return;
        }

        if (response.status !== 201) {
            const errorHtml = await response.text();
            const errorDiv = document.createElement("div");
            errorDiv.innerHTML = errorHtml;

            const errorContent = errorDiv.querySelector(".modal-content").innerHTML;
            document.querySelector("#create-transaction-modal .modal-content").innerHTML = errorContent;
            return;
        }

        modal.hide();
        window.location.reload();
    } catch (error) {
        console.error("Submission error:", error);
    }
}

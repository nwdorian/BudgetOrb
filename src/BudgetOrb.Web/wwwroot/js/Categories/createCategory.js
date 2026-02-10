let createModal;
const createContainer = document.getElementById("create-category-modal-placeholder");

async function loadCreateModal() {
    try {
        const response = await fetch("/Categories/Create");
        if (!response.ok) {
            console.error("There was an error loading the modal: ", response.error);
        }
        const html = await response.text();
        createContainer.innerHTML = html;

        createModal = new bootstrap.Modal(document.getElementById("create-category-modal"));
        createModal.show();
    } catch (error) {
        console.error("There was an unexpected error loading the update modal:", error);
    }
}

async function submitCreate(form, event) {
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
            document.querySelector("#create-category-modal .modal-content").innerHTML = errorContent;
            return;
        }

        createModal.hide();
        window.location.reload();
    } catch (error) {
        console.error("Submission error:", error);
    }
}

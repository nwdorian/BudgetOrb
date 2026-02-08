let updateModal;
const updateContainer = document.getElementById("update-transaction-modal-placeholder");

async function loadUpdateModal(id) {
    try {
        const response = await fetch(`/Transactions/Update/${id}`);
        if (!response.ok) {
            console.error("There was an error loading the modal: ", response.error);
        }
        const html = await response.text();
        createContainer.innerHTML = html;

        createModal = new bootstrap.Modal(document.getElementById("update-transaction-modal"));
        createModal.show();
    } catch (error) {
        console.error("There was an unexpected error loading the update modal:", error);
    }
}

async function submitUpdate(form, event) {
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

        if (response.status !== 204) {
            const errorHtml = await response.text();
            const errorDiv = document.createElement("div");
            errorDiv.innerHTML = errorHtml;

            const errorContent = errorDiv.querySelector(".modal-content").innerHTML;
            document.querySelector("#update-transaction-modal .modal-content").innerHTML = errorContent;
            return;
        }

        createModal.hide();
        window.location.reload();
    } catch (error) {
        console.error("Submission error:", error);
    }
}

const orderItemTemplate = document.getElementById("orderItemTemplate");
const orderItemsContainer = document.getElementById('orderItemsContainer');
const customerDropdown = document.getElementById('Command_SelectedCustomerId');

var orderItemIndex = 0;
document.addEventListener("DOMContentLoaded", function () {
    if (orderItemTemplate != null) {
        addOrderItem();
    }
});

function filterCustomers() {
    const searchField = document.getElementById('customerSearch');
    const searchValue = searchField.value.toLowerCase();
    const options = customerDropdown.options;
    const option = Array.from(options).find(x => x.text.toLowerCase().includes(searchValue));
    if (option) {
        customerDropdown.value = option.value;
    } else {
        customerDropdown.value = "";
    }
}

function toggleNewCustomerForm() {
    const customerType = document.querySelector('input[name="Order.CustomerType"]:checked').value;
    const newCustomerForm = document.getElementById('newCustomerForm');
    const existingCustomer = document.getElementById('existingCustomer');

    if (customerType == 1) {
        existingCustomer.classList.add("d-none");
        newCustomerForm.classList.remove("d-none");
    } else {
        existingCustomer.classList.remove("d-none");
        newCustomerForm.classList.add("d-none");
    }

    customerDropdown.value = "";
}

function addOrderItem() {
    let orderItemHtml = orderItemTemplate.innerHTML;
    orderItemHtml = orderItemHtml.replace("{{orderItemNumber}}", orderItemIndex + 1)
    orderItemHtml = orderItemHtml.replaceAll("{{orderItemIndex}}", orderItemIndex);
    orderItemsContainer.insertAdjacentHTML('beforeend', orderItemHtml);
    orderItemIndex++;

    const removeButton = orderItemsContainer.querySelector(
        `.remove-item-btn[data-index="${orderItemIndex - 1}"]`
    );

    if (removeButton) {
        removeButton.addEventListener('click', function () {
            // Action to remove the item
            this.closest(`#orderItem_${orderItemIndex - 1}`).remove();
        });
    }
}

function calculateTotalPrice() {

}

function submitForm() {
    const formData = new FormData();

    // Check if the customer is new or existing
    const customerType = document.querySelector('input[name="Order.CustomerType"]:checked').value;

    if (customerType === "1") {
        // New Customer
        const newCustomerForm = document.getElementById('newCustomerForm');
        const newCustomerInputs = newCustomerForm.querySelectorAll('input, textarea');
        newCustomerInputs.forEach(input => {
            if (input.name) {
                formData.append(input.name, input.value);
            }
        });
    } else if (customerType === "2") {
        // Existing Customer
        const selectedCustomerId = document.querySelector('select[name="Command.SelectedCustomerId"]').value;
        formData.append('Command.SelectedCustomerId', selectedCustomerId);
    }

    // Collect order details
    formData.append('Command.Order.OrderType', document.querySelector('input[name="Command.Order.OrderType"]:checked').value);
    formData.append('Command.Order.DeliveryDate', document.querySelector('[name="Command.Order.DeliveryDate"]').value);
    formData.append('Command.Order.Notes', document.querySelector('[name="Command.Order.Notes"]').value);

    // Collect all order item data dynamically
    for (let i = 0; i < orderItemIndex; i++) {
        const itemContainer = document.getElementById(`orderItem_${i}`);
        if (!itemContainer) continue; // Skip if item was removed

        const pieceId = itemContainer.querySelector(`#Items_${i}__PieceId`).value;
        const description = itemContainer.querySelector(`#Items_${i}__Description`).value;
        const quantity = itemContainer.querySelector(`#Items_${i}__Quantity`).value;
        const singleQuantityPrice = itemContainer.querySelector(`#Items_${i}__SingleQuantityPrice`).value;
        const color = itemContainer.querySelector(`#Items_${i}__Color`).value;
        const comment = itemContainer.querySelector(`#Items_${i}__Comment`).value;

        formData.append(`Command.Order.Items[${i}].PieceId`, pieceId);
        formData.append(`Command.Order.Items[${i}].Description`, description);
        formData.append(`Command.Order.Items[${i}].Quantity`, quantity);
        formData.append(`Command.Order.Items[${i}].SingleQuantityPrice`, singleQuantityPrice);
        formData.append(`Command.Order.Items[${i}].Color`, color);
        formData.append(`Command.Order.Items[${i}].Comment`, comment);

        const selectedCleaningTypes = itemContainer.querySelectorAll(
            `input[name="Items[${i}].CleaningTypeOptions"]:checked`
        );
        selectedCleaningTypes.forEach((checkbox, index) => {
            formData.append(`Command.Order.Items[${i}].CleaningTypeIds[${index}]`, checkbox.value);
        });
    }

    // Submit the form using fetch
    fetch("/order/create", {
        method: 'POST',
        body: formData,
        headers: {
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => {
            if (response.ok) {
                alert('Order submitted successfully!');
                window.location.href = '/Order/Index'; // Redirect to the index page
            } else {
                alert('Failed to submit the order. Please try again.');
            }
        })
        .catch(error => {
            console.error('Error submitting form:', error);
            alert('An error occurred while submitting the order.');
        });
}
const orderItemTemplate = document.getElementById("orderItemTemplate");
const orderItemsContainer = document.getElementById("orderItemsContainer");
const orderSummaryTemplate = document.getElementById("orderSummaryTemplate");
const orderSummaryContainer = document.getElementById("orderSummaryContainer");
const customerDropdown = document.getElementById("Command_SelectedCustomerId");
const createOrderForm = document.getElementById("createOrderForm");
const deliveryFeesInput = document.getElementById("Command_Order_DeliveryFees");

var totalPrice = 0;
var totalQuantity = 0;
var deliveryFees = 0;
var orderItemIndex = 0;
document.addEventListener("DOMContentLoaded", function () {
    if (orderItemTemplate != null) {
        addOrderItem();
    }

    if (createOrderForm) {
        createOrderForm.addEventListener("keydown", function (event) {
            if (event.key === "Enter") {
                event.preventDefault();
            }
        });
    }

    if (deliveryFeesInput) {
        deliveryFeesInput.addEventListener("input", function () {
            deliveryFees = parseInt(deliveryFeesInput.value);
            calculateTotalPrice();
        });
    }
});

function filterCustomers() {
    const searchField = document.getElementById("customerSearch");
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
        customerDropdown.value = "";
    } else {
        existingCustomer.classList.remove("d-none");
        newCustomerForm.classList.add("d-none");
    }
}

function addOrderItem() {
    let orderItemHtml = orderItemTemplate.innerHTML;
    orderItemHtml = orderItemHtml.replaceAll("{{orderItemIndex}}", orderItemIndex);
    orderItemsContainer.insertAdjacentHTML('beforeend', orderItemHtml);

    orderItemIndex++;
    calculateTotalPrice();

    const removeButton = orderItemsContainer.querySelector(
        `.remove-item-btn[data-index="${orderItemIndex - 1}"]`
    );

    const currentIndex = removeButton.getAttribute("data-index");

    const quantitySelect = orderItemsContainer.querySelector(
        `#Items_${currentIndex}__Quantity`
    );

    const priceInput = orderItemsContainer.querySelector(
        `#Items_${currentIndex}__SingleQuantityPrice`
    );

    const totalItemPrice = orderItemsContainer.querySelector(
        `#Items_${currentIndex}__TotalItemPrice`
    );

    quantitySelect.addEventListener("change", function () {
        totalItemPrice.value = parseInt(quantitySelect.value) * parseInt(priceInput.value);
        calculateTotalPrice();
    });

    priceInput.addEventListener("input", function () {
        totalItemPrice.value = parseInt(quantitySelect.value) * parseInt(priceInput.value);
        calculateTotalPrice();
    });


    if (orderItemIndex - 1 > 0) {
        removeButton.classList.remove("d-none");
    } else {
        removeButton.classList.add("d-none");
    }

    if (removeButton) {
        removeButton.addEventListener('click', function () {
            orderItemIndex--;
            this.closest(`#orderItem_${currentIndex}`).remove();
            calculateTotalPrice();
        });
    }
}

function setOrderSummary(quantity, price) {
    let orderItemSummaryHtml = orderSummaryTemplate.innerHTML;
    orderItemSummaryHtml = orderItemSummaryHtml.replace("{{totalQuantity}}", quantity);
    orderItemSummaryHtml = orderItemSummaryHtml.replace("{{totalPrice}}", price);
    orderSummaryContainer.innerHTML = orderItemSummaryHtml;
}

function calculateTotalPrice() {
    totalPrice = 0;
    totalQuantity = 0;
    const orderItems = document.querySelectorAll("#orderItemsContainer .order-item");
    orderItems.forEach(function (item, idx) {
        let quantity = item.querySelector(
            `#Items_${idx}__Quantity`
        );

        let totalItemPrice = item.querySelector(
            `#Items_${idx}__TotalItemPrice`
        );

        totalQuantity += parseInt(quantity.value);
        totalPrice += parseInt(totalItemPrice.value);
    });

    totalPrice += deliveryFees;

    setOrderSummary(totalQuantity, totalPrice);
}

function submitForm() {
    const formData = new FormData();

    // Check if the customer is new or existing
    const customerType = document.querySelector('input[name="Order.CustomerType"]:checked').value;

    if (customerType === "1") {
        // New Customer
        const newCustomerForm = document.getElementById('newCustomerForm');
        const newCustomerInputs = newCustomerForm.querySelectorAll('input, textarea, select');
        newCustomerInputs.forEach(input => {
            if (input.hasAttribute("data-required") && (input.value == "" || input.value == null)) {
                alert("بعض تفاصيل العميل الجديد غير موجودة");
                return;
            }

            if (input.name) {
                formData.append(input.name, input.value);
            }
        });
    } else if (customerType === "2") {
        // Existing Customer
        const selectedCustomerId = document.querySelector('select[name="Command.SelectedCustomerId"]').value;
        if (selectedCustomerId == "") {
            alert("من فضلك قم بإختيار العميل");
            return;
        }
        formData.append('Command.SelectedCustomerId', selectedCustomerId);
    }

    // Collect order details
    formData.append('Command.Order.OrderType', document.querySelector('input[name="Command.Order.OrderType"]').value);
    formData.append('Command.Order.DeliveryDate', document.querySelector('[name="Command.Order.DeliveryDate"]').value);
    formData.append('Command.Order.DeliveryFees', document.querySelector('[name="Command.Order.DeliveryFees"]').value);
    formData.append('Command.Order.Notes', document.querySelector('[name="Command.Order.Notes"]').value);

    // Collect all order item data dynamically
    for (let i = 0; i < orderItemIndex; i++) {
        const itemContainer = document.getElementById(`orderItem_${i}`);
        if (!itemContainer) continue; // Skip if item was removed

        const pieceId = itemContainer.querySelector(`#Items_${i}__PieceId`).value;
        const description = itemContainer.querySelector(`#Items_${i}__Description`).value;
        const quantity = itemContainer.querySelector(`#Items_${i}__Quantity`).value;
        const singleQuantityPrice = itemContainer.querySelector(`#Items_${i}__SingleQuantityPrice`).value;

        formData.append(`Command.Order.Items[${i}].PieceId`, pieceId);
        formData.append(`Command.Order.Items[${i}].Description`, description);
        formData.append(`Command.Order.Items[${i}].Quantity`, quantity);
        formData.append(`Command.Order.Items[${i}].SingleQuantityPrice`, singleQuantityPrice);

        const selectedCleaningTypes = itemContainer.querySelectorAll(
            `input[name="Items[${i}].CleaningTypeOption"]:checked`
        );
        if (selectedCleaningTypes != null && selectedCleaningTypes.length > 0) {
            selectedCleaningTypes.forEach((checkbox, index) => {
                formData.append(`Command.Order.Items[${i}].CleaningTypeIds[${index}]`, checkbox.value);
            });
        }
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
            console.log(response);
            if (response.ok) {
                alert('تم حفظ الطلب');
                window.location.href = '/orders'; // Redirect to the index page
            } else {
                alert('فشل في حفظ الطلب, يرجى المحاولة مرة أخرى');
            }
        })
        .catch(error => {
            console.error('Error submitting form:', error);
            alert('خطأ في حفظ الطلب, يرجى المحاولة مرة أخرى');
        });
}
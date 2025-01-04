const dataTable = new DataTable('#orders', {
    order: [[0, "desc"]],
    pageLength: 10,
    layout: {
        bottomEnd: {
            paging: {
                firstLast: false
            }
        }
    },
    language: {
        url: '/lib/dataTables/plug-ins/i18n/ar.json',
    },
    lengthMenu: [
        [10, 25, 50, -1],
        [10, 25, 50, 'الجميع']
    ]
});

$('#updateOrderModal').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    var orderId = button.data('order-id');
    $('#orderId').val(orderId);
});

function confirmOrderChanges() {
    var orderId = $('#orderId').val();
    var orderStatus = $('#order-status').val();

    if (orderStatus == null) {
        alert('من فضلك اختر حالة الطلب');
        return;
    }
    let order = {
        orderId: orderId,
        orderStatus: +orderStatus
    };

    fetch("/order/Details", {
        method: 'POST',
        body: JSON.stringify(order),
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    })
        .then(response => {
            if (response.ok) {
                window.location.reload();
            }
        })
        .catch(error => {
            console.error('Error submitting form:', error);
            alert('خطأ في حفظ الطلب, يرجى المحاولة مرة أخرى');
        });
}
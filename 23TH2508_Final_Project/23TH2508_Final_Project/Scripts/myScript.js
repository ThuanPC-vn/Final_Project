document.addEventListener('DOMContentLoaded', function () {

    var dateInput = document.getElementById('NgayLapInput');
    var datePicker = document.getElementById('NgayLapPicker');
    var timeInput = document.getElementById('ThoiGianLapInput');
    var timePicker = document.getElementById('ThoiGianLapPicker');

    // Đặt giá trị mặc định là ngày hiện tại
    var today = new Date();
    var day = String(today.getDate()).padStart(2, '0');
    var month = String(today.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
    var year = today.getFullYear();
    var formattedDate = month + '/' + day + '/' + year;
    dateInput.value = formattedDate;
    datePicker.value = year + '-' + month + '-' + day;

    // Đặt giá trị mặc định là thời gian hiện tại
    var hours = String(today.getHours()).padStart(2, '0');
    var minutes = String(today.getMinutes()).padStart(2, '0');
    var seconds = String(today.getSeconds()).padStart(2, '0');
    var formattedTime = hours + ':' + minutes + ':' + seconds;
    timeInput.value = formattedTime;
    timePicker.value = hours + ':' + minutes;

    // Đồng bộ hóa giá trị giữa hai ô nhập ngày
    dateInput.addEventListener('input', function () {
        var parts = dateInput.value.split('/');
        if (parts.length === 3) {
            var formattedPickerDate = parts[2] + '-' + parts[0] + '-' + parts[1];
            datePicker.value = formattedPickerDate;
        }
    });

    datePicker.addEventListener('change', function () {
        var parts = datePicker.value.split('-');
        if (parts.length === 3) {
            var formattedInputDate = parts[1] + '/' + parts[2] + '/' + parts[0];
            dateInput.value = formattedInputDate;
        }
    });

    // Đồng bộ hóa giá trị giữa hai ô nhập thời gian
    timeInput.addEventListener('input', function () {
        var parts = timeInput.value.split(':');
        if (parts.length === 3) {
            var formattedPickerTime = parts[0] + ':' + parts[1];
            timePicker.value = formattedPickerTime;
        }
    });

    timePicker.addEventListener('change', function () {
        var parts = timePicker.value.split(':');
        if (parts.length === 2) {
            var formattedInputTime = parts[0] + ':' + parts[1] + ':00';
            timeInput.value = formattedInputTime;
        }
    });
});

function syncDateInputs() {
    var dateInput = document.getElementById('NgayLapInput');
    var datePicker = document.getElementById('NgayLapPicker');
    var parts = datePicker.value.split('-');
    if (parts.length === 3) {
        var formattedInputDate = parts[1] + '/' + parts[2] + '/' + parts[0];
        dateInput.value = formattedInputDate;
    }
}

function syncTimeInputs() {
    var timeInput = document.getElementById('ThoiGianLapInput');
    var timePicker = document.getElementById('ThoiGianLapPicker');
    var parts = timePicker.value.split(':');
    if (parts.length === 2) {
        var formattedInputTime = parts[0] + ':' + parts[1] + ':00';
        timeInput.value = formattedInputTime;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    var tongTienInput = document.getElementById('TongTienInput');

    tongTienInput.addEventListener('input', function () {
        formatCurrency(tongTienInput);
    });

    function formatCurrency(input) {
        // Xóa tất cả các ký tự không phải số
        let value = input.value.replace(/\D/g, '');

        // Định dạng lại số với dấu phẩy
        value = new Intl.NumberFormat('vi-VN').format(value);

        // Gán lại giá trị đã định dạng vào input
        input.value = value;
    }
});

// Loại bỏ dấu chấm khi gửi dữ liệu và xử lý submit
var form = document.querySelector('form');
if (form) { // Kiểm tra xem form có tồn tại hay không
    form.addEventListener('submit', function (event) {
        // Loại bỏ dấu phẩy trước khi gửi dữ liệu
        var tongTienInput = document.getElementById('TongTienInput');
        if (tongTienInput) {
            tongTienInput.value = tongTienInput.value.replace(/,/g, '');
        }
    });
}
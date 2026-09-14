document.addEventListener("DOMContentLoaded", function () {
    const inputs = document.querySelectorAll(".otp-input");

    inputs.forEach((input, index) => {
        input.addEventListener("input", (e) => {
            if (e.target.value.length === 1 && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        });

        input.addEventListener("keydown", (e) => {
            if (e.key === "Backspace" && index > 0 && !e.target.value) {
                inputs[index - 1].focus();
            }
        });
    });
});

$(document).ready(function () {
    $(".otp-input").each(function () {
        $(this).on('input', function (e) {
            if (e.target.value.length === 1 && index < inputs.length - 1) {
                inputs[index + 1].focus();
            }
        });

        $(this).on('keydown', function (e) {
            if (e.key === "Backspace" && index > 0 && !e.target.value) {
                inputs[index - 1].focus();
            }
        });
    });
});

document.getElementById("file-upload").addEventListener("change", function () {
    var fileName = this.files.length > 0 ? this.files[0].name : "Upload PDF/Image";
    document.getElementById("file-name").textContent = fileName;
});
document.getElementById("file-upload-2").addEventListener("change", function () {
    var fileName = this.files.length > 0 ? this.files[0].name : "Upload PDF/Image";
    document.getElementById("file-name-2").textContent = fileName;
});
document.getElementById("file-upload-3").addEventListener("change", function () {
    var fileName = this.files.length > 0 ? this.files[0].name : "Upload PDF/Image";
    document.getElementById("file-name-3").textContent = fileName;
});

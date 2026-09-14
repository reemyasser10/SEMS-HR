$(document).ready(function () {

    $("form").on("submit", function (e) {
        e.preventDefault(); // منع الإرسال الطبيعي للفورم
        const form = this;

        // 1. التحقق من required inputs
        if (!form.checkValidity()) {
            form.reportValidity(); // المتصفح هيعرض رسائل الخطأ ويعمل فوكس على أول input فيه مشكلة
            return;
        }

        $.ajax({
            url: '/HR/ForgetPassword/Index',
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify({
                UserName: $("#UserName").val()
            }),
            success: function (response) {
                $(".loading-page").addClass('d-none');
                if (response.isSuccess) {
                    code = response.data.value;
                    $('#otpModal').modal('show');
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: response.status.plainErrorMessage,
                        icon: 'error',
                        customClass: {
                            confirmButton: 'btn btn-primary waves-effect waves-light'
                        },
                        buttonsStyling: false
                    });
                }
            },
            error: function () {
                $(".loading-page").addClass('d-none');
                Swal.fire({
                    title: 'Error!',
                    text: 'Send code failed.',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary waves-effect waves-light'
                    },
                    buttonsStyling: false
                });
            }
        });
    });

    $("#verifyOtpBtn").on('click', function () {
        verifyOtpAndRedirect((username) => `/HR/ForgetPassword/ForgetPassword?userName=${username}`);
    });

    $("#resendOtpBtn").on('click', function (e) {
        e.preventDefault();
        resendOtp();
    });

});
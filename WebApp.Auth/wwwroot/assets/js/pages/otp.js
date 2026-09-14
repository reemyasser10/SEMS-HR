function verifyOtpAndRedirect(redirectUrlBuilder) {
    $(".loading-page").removeClass('d-none');
    let otpValue = '';
    $('.otp-input').each(function () {
        otpValue += $(this).val();
    });

    const username = $("#UserName").val();

    $.ajax({
        url: '/HR/Register/VerifyEmail',
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify({
            code: otpValue,
            email: username
        }),
        success: function (response) {
            $(".loading-page").addClass('d-none');
            if (response.isSuccess) {
                $('#otpModal').modal('hide');
                Swal.fire({
                    title: 'Success!',
                    text: 'Email verified successfully!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary waves-effect waves-light'
                    },
                    buttonsStyling: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        const redirectUrl = redirectUrlBuilder(username);
                        window.location.href = redirectUrl;
                    }
                });
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
                text: 'Invalid OTP, please try again.',
                icon: 'error',
                customClass: {
                    confirmButton: 'btn btn-primary waves-effect waves-light'
                },
                buttonsStyling: false
            });
        }
    });
}

function resendOtp() {
    debugger;
    const username = $("#UserName").val();
    $('.otp-input').each(function () {
        $(this).val('');
    });
    $('#otpModal').modal('hide');
    $(".loading-page").removeClass('d-none');

    $.ajax({
        url: '/HR/Register/ResendVerification',
        method: 'POST',
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify({
            email: username
        }),
       
        success: function (response) {
            $(".loading-page").addClass('d-none');
            if (response.isSuccess) {
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
                text: 'Something went wrong, please try again.',
                icon: 'error',
                customClass: {
                    confirmButton: 'btn btn-primary waves-effect waves-light'
                },
                buttonsStyling: false
            });
        }
    });
}

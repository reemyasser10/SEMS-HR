$(document).ready(function () {
    Swal = Swal.mixin({});

    $('#register-form').on('submit', function (e) {
        e.preventDefault(); // Stop normal form submission

        const $form = $(this);
        if (!customValidation($form)) {
            e.preventDefault(); 
            $(".loading-page").addClass('d-none');
            return;
        }

        $.ajax({
            type: $form.attr('method'),
            url: $form.attr('action'),
            data: $form.serialize(),
            success: function (response) {
                debugger;
                console.log("in success")
                $(".loading-page").addClass('d-none');
                if (response.isSuccess) {
        
                    token = response.authorization;
                    if (response.data && response.data.emailSendFailed) {
                        Swal.fire({
                            title: 'Account Created Successfully!',
                            text: 'Your account has been created, but we encountered an issue sending your verification email. Please log in to complete your next steps (you will be prompted to verify or resend your code).',
                            icon: 'warning',
                            customClass: {
                                confirmButton: 'btn btn-primary waves-effect waves-light'
                            },
                            buttonsStyling: false
                        }).then((result) => {
                            window.location.href = '/HR/Login';
                        });
                    } else {
                        $('#otpModal').modal('show');
                    }
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
                    text: 'Registration failed.',
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
        verifyOtpAndRedirect(() => '/HR');
    });

    $("#resendOtpBtn").on('click', function (e) {
        e.preventDefault();
        resendOtp();
    });
});


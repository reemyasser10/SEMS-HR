// ✅ Start of client-side-validation.js

console.log("✅ client-side-validation.js loaded");

// ------------------------------ Validation Functions ------------------------------

window.validateFullIdFields = function (scope) {
    let allValid = true;
    const validGovs = ['01', '02', '03', '04', '11', '12', '13', '14', '15', '16', '17', '18', '19', '21', '22', '23', '24', '25', '26', '27', '28', '29', '31', '32', '33', '34', '35', '88'];
    const $container = scope ? $(scope) : $('form');

    $container.find('.validate-full-id').each(function () {
        const $field = $(this);
        let value = $field.val();
        const $errorSpan = $field.closest('div').find('.error-message');

        // احذف أي حاجة مش رقم (حروف، رموز، مسافات...)
        const cleanedValue = value.replace(/\D/g, '');
        if (value !== cleanedValue) {
            $field.val(cleanedValue);
            value = cleanedValue;
        }

        let fieldValid = true;
        let errorMessage = '';

        if (value.length !== 14) {
            fieldValid = false;
            errorMessage = 'ID must be exactly 14 digits.';
        } else if (/^(\d)\1{13}$/.test(value)) {
            fieldValid = false;
            errorMessage = 'Repetitive digits are not allowed.';
        } else {
            const century = value.charAt(0);
            const yearSuffix = parseInt(value.substring(1, 3), 10);
            let fullYear = null;
            if (century === '2') fullYear = 1900 + yearSuffix;
            else if (century === '3') fullYear = 2000 + yearSuffix;
            const currentYear = new Date().getFullYear();
            if (!fullYear || fullYear < 1900 || fullYear > currentYear) {
                fieldValid = false;
                errorMessage = 'Invalid birth year.';
            }
        }

        if (fieldValid) {
            const month = value.substring(3, 5);
            if (!/^(0[1-9]|1[0-2])$/.test(month)) {
                fieldValid = false;
                errorMessage = 'Invalid birth month.';
            }
        }

        if (fieldValid) {
            const day = value.substring(5, 7);
            if (!(day >= '01' && day <= '31')) {
                fieldValid = false;
                errorMessage = 'Invalid birth day.';
            }
        }

        if (fieldValid) {
            const govCode = value.substring(7, 9);
            if (!validGovs.includes(govCode)) {
                fieldValid = false;
                errorMessage = 'Invalid governorate code.';
            }
        }

        if (fieldValid) {
            $field.removeClass('is-invalid');
            $errorSpan.text('');
        } else {
            allValid = false;
            $field.addClass('is-invalid');
            if ($errorSpan.length === 0) {
                $field.after('<span class="error-message text-danger">' + errorMessage + '</span>');
            } else {
                $errorSpan.text(errorMessage);
            }
        }
    });

    return allValid;
};


window.validateEmailFields = function (form) {
    let allValid = true;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    $(form).find('.validate-email').each(function () {
        const $field = $(this);
        let value = $field.val().trim();
        const $errorSpan = $field.next('.error-message');

        if (value !== '' && !emailRegex.test(value)) {
            allValid = false;
            $field.addClass('is-invalid');
            $errorSpan.text('Invalid email format.');
        } else {
            $field.removeClass('is-invalid');
            $errorSpan.text('');
        }
    });

    return allValid;
};

window.validatePasswordFields = function (form) {
    let allValid = true;
    const $passwordField = $(form).find('.validate-password');
    const $confirmPasswordField = $(form).find('.validate-confirm-password');

    if ($passwordField.length === 0 && $confirmPasswordField.length === 0) return true;

    const $passwordError = $passwordField.closest('.form-password-toggle').find('.error-message');
    const $confirmError = $confirmPasswordField.closest('.form-password-toggle').find('.error-message');

    const password = $passwordField.val()?.trim() || '';
    const confirmPassword = $confirmPasswordField.val()?.trim() || '';

    const isPasswordValid = password.length >= 8 && /[A-Z]/.test(password) && /[a-z]/.test(password) && /[0-9]/.test(password) && /[!@#$%^&*(),.?":{}|<>]/.test(password);

    if (!isPasswordValid) {
        allValid = false;
        $passwordField.addClass('is-invalid');
        $passwordError.text('Password must meet complexity requirements.');
    } else {
        $passwordField.removeClass('is-invalid');
        $passwordError.text('');
    }

    if (confirmPassword !== password) {
        allValid = false;
        $confirmPasswordField.addClass('is-invalid');
        $confirmError.text('Passwords do not match.');
    } else {
        $confirmPasswordField.removeClass('is-invalid');
        $confirmError.text('');
    }

    return allValid;
};

window.customValidation = function (form) {
    const valid1 = window.validatePasswordFields(form);
    const valid2 = window.validateEmailFields(form);
    const valid3 = window.validateFullIdFields(form);
    return valid1 && valid2 && valid3;
};

window.applyOnlyLettersValidation = function (scope = document) {
    scope.querySelectorAll('input.only-letters').forEach(input => {
        setTimeout(() => {
            let feedback = input.nextElementSibling;
            if (!feedback || !feedback.classList.contains('invalid-feedback')) {
                feedback = document.createElement('div');
                feedback.className = 'invalid-feedback';
                feedback.style.display = 'none';
                feedback.textContent = 'Please enter letters only (no numbers or symbols).';
                input.insertAdjacentElement('afterend', feedback);
            }

            input.addEventListener('keypress', function (e) {
                if (this.classList.contains('no-typing')) {
                    e.preventDefault();
                    return;
                }

                const char = String.fromCharCode(e.which);
                const isLetter = /^[a-zA-Z؀-ۿ\s]+$/.test(char);

                if (!isLetter) {
                    e.preventDefault();
                    feedback.style.display = 'block';
                }
            });

            input.addEventListener('input', function () {
                const value = this.value;
                const allLetters = /^[a-zA-Z؀-ۿ\s]*$/.test(value);
                if (allLetters) {
                    feedback.style.display = 'none';
                }
            });
        }, 50);
    });
};

// Email validation function
window.applyEmailValidation = function (scope = document) {
    scope.querySelectorAll('input.email-format').forEach(input => {
        setTimeout(() => {
            let feedback = input.nextElementSibling;
            if (!feedback || !feedback.classList.contains('invalid-feedback')) {
                feedback = document.createElement('div');
                feedback.className = 'invalid-feedback text-danger';
                feedback.style.display = 'none';
                feedback.textContent = 'Please enter a valid email address';
                input.insertAdjacentElement('afterend', feedback);
            }

            const validateEmail = (email) => {
                // 1. Basic email structure
                // 2. Ends with ".com"
                // 3. No characters after ".com"
                return /^[^\s@]+@[^\s@]+\.(com)$/.test(email);
            };

            input.addEventListener('input', function () {
                const value = this.value.trim();
                const isValidEmail = validateEmail(value);

                if (isValidEmail || value === '') {
                    input.classList.remove('is-invalid');
                    feedback.style.display = 'none';
                } else {
                    input.classList.add('is-invalid');
                    feedback.style.display = 'block';
                }
            });
        }, 50);
    });
};


// Phone number validation function  
window.applyPhoneValidation = function (scope = document) {
    scope.querySelectorAll('input.phone-only').forEach(input => {
        let feedback = input.nextElementSibling;
        if (!feedback || !feedback.classList.contains('invalid-feedback')) {
            feedback = document.createElement('div');
            feedback.className = 'invalid-feedback text-danger';
            feedback.style.display = 'none';
            feedback.textContent = 'Phone number must start with 01 and be exactly 11 digits.';
            input.insertAdjacentElement('afterend', feedback);
        }

        input.addEventListener('keypress', function (e) {
            const char = String.fromCharCode(e.which);
            if (!/[0-9]/.test(char)) {
                e.preventDefault();
                feedback.style.display = 'block';
            }
        });

        input.addEventListener('input', function () {
            const value = this.value;
            const isValidPhone = /^01\d{9}$/.test(value);

            if (isValidPhone || value === '') {
                feedback.style.display = 'none';
            } else {
                feedback.style.display = 'block';
            }
        });
    });
};

window.applyHomePhoneValidation = function (scope = document) {
    scope.querySelectorAll('input.home-phone-only').forEach(input => {
        input.addEventListener('input', () => {
            input.value = input.value.replace(/[^0-9]/g, '');
        });
    });
};
window.applyMonthsValidation = function (scope = document) {
    scope.querySelectorAll('input.months-only').forEach(input => {
        input.addEventListener('input', () => {
            // مسموح فقط الأرقام الموجبة (0 أو أكثر)
            input.value = input.value.replace(/[^0-9]/g, '');
        });
    });
};


$(document).on('input', '.only-numbers', function () {
    this.value = this.value.replace(/[^0-9]/g, '');
});

// يمنع لصق حروف
$(document).on('paste', '.only-numbers', function (e) {
    let pasted = (e.originalEvent || e).clipboardData.getData('text');
    if (/\D/.test(pasted)) {
        e.preventDefault();
    }
});
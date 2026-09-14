function validateFutureDate(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    const isRequired = $input.prop('required');
    let isValid = true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
        isValid = false;
    }

    function clearError() {
        $input.removeClass('is-invalid');
        $input.next('.validation-error-message').remove();
    }

    clearError();

    if (isRequired && !value) {
        setError("Field is Required");
        return false;
    }

    if (value) {
        const inputDate = new Date(value);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (isNaN(inputDate.getTime()) || inputDate < today || inputDate.getFullYear() < 1000) {
            setError("Date cannot be in the past");
            return false;
        }
    }

    return true;
}

function validateRequired(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    let isValid = true;

    function setError(msg) {
        $input.addClass('is-invalid');

        // Handle Select2
        if ($input.hasClass('select2-hidden-accessible')) {
            const $container = $input.next('.select2-container');
            if ($container.length > 0) {
                $container.find('.select2-selection').addClass('border-danger');
                let $errorSpan = $container.next('.validation-error-message');
                if ($errorSpan.length === 0) {
                    $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
                    $container.after($errorSpan);
                }
                $errorSpan.text(msg);
                isValid = false;
                return;
            }
        }

        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
        isValid = false;
    }

    function clearError() {
        $input.removeClass('is-invalid');

        // Clear Select2 error
        if ($input.hasClass('select2-hidden-accessible')) {
            const $container = $input.next('.select2-container');
            $container.find('.select2-selection').removeClass('border-danger');
            $container.next('.validation-error-message').remove();
        }

        $input.next('.validation-error-message').remove();
    }

    clearError();

    if (!value || (typeof value === 'string' && !value.trim())) {
        setError("Field is Required");
        return false;
    }

    if (Array.isArray(value) && value.length === 0) {
        setError("Field is Required");
        return false;
    }

    return true;
}

function validateFileUpload(inputSelector, allowedExtensions, maxSizeMB) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    if ($input[0].files && $input[0].files.length > 0) {
        const file = $input[0].files[0];
        const fileName = file.name.toLowerCase();
        const fileSize = file.size;

        const hasValidExtension = allowedExtensions.some(ext => fileName.endsWith(ext.toLowerCase()));
        if (!hasValidExtension) {
            setError("It's invalid input");
            return false;
        }

        if (fileSize > maxSizeMB * 1024 * 1024) {
            setError("It's invalid input");
            return false;
        }
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validateName(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    if (value.trim().length < 3) {
        setError("Name must be greater than 2 characters");
        return false;
    }

    const regex = /^[\p{L}\s]+$/u;

    if (!regex.test(value)) {
        setError("Name contains invalid characters (letters only)");
        return false;
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validateAlphanumeric(inputSelector, options = {}) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true;
    const fieldLabel = options.fieldLabel || "Name";
    const allowNumbersOnly = options.allowNumbersOnly || false;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    const regex = /^[\p{L}0-9\s]+$/u;

    if (!regex.test(value)) {
        setError(`${fieldLabel} contains invalid characters (letters, numbers and spaces only)`);
        return false;
    }

    // Check if it's numbers only (at least one letter required? Or just not all numbers?)
    // "Name can't be numbers only" implies we shouldn't allow "123".
    // We check if the string contains only digits and spaces. 
    // If so, it's invalid.
    if (!allowNumbersOnly && /^[0-9\s]+$/.test(value)) {
        setError(`${fieldLabel} cannot be numbers only`);
        return false;
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validateNameWithSymbols(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    // Require at least one letter (supports Unicode letters)
    const regex = /[\p{L}]/u;

    if (!regex.test(value)) {
        setError("Full name must contain at least one letter");
        return false;
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validateEmail(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!regex.test(value)) {
        setError("Invalid email format (example@domain.com)");
        return false;
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validatePhone(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    const regex = /^\+?[0-9]{10,15}$/;

    if (!regex.test(value)) {
        setError("Invalid phone number (10-15 digits allowed)");
        return false;
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validatePassword(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true;

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    if (/\s/.test(value)) {
        setError("Password cannot contain spaces");
        return false;
    }

    if (value.length < 8) {
        setError("Password must be at least 8 characters");
        return false;
    }

    $input.removeClass('is-invalid');
    $input.next('.validation-error-message').remove();

    return true;
}

function validateMaxLength(inputSelector, maxLength) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();

    function clearError() {


        $input.removeClass('is-invalid');
        $input.next('.validation-error-message').remove();
    }

    if (!value) {

        clearError();
        return true;
    }

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    if (value.length > maxLength) {
        setError(`Maximum length is ${maxLength} characters`);
        return false;
    }

    // If valid
    clearError();
    return true;
}

function validateDateRange(fromSelector, toSelector) {
    const $from = $(fromSelector);
    const $to = $(toSelector);

    if ($from.length === 0 || $to.length === 0) return true;

    const fromVal = $from.val();
    const toVal = $to.val();

    // Clear previous errors
    $from.removeClass('is-invalid');
    $from.next('.validation-error-message').remove();
    $to.removeClass('is-invalid');
    $to.next('.validation-error-message').remove();

    if (!fromVal || !toVal) return true; // Let required validation handle empty

    const fromDate = new Date(fromVal);
    const toDate = new Date(toVal);

    if (fromDate >= toDate) {
        $from.addClass('is-invalid');
        $to.addClass('is-invalid');

        const msg = "Date from must be earlier than date to";

        // Add error message to 'To' field
        let $errorSpan = $to.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $to.after($errorSpan);
        }
        $errorSpan.text(msg);

        return false;
    }

    return true;
}

function validatePositiveNumber(inputSelector) {
    const $input = $(inputSelector);
    if ($input.length === 0) return true;

    const value = $input.val();
    if (!value) return true; // Let required validation handle empty

    function setError(msg) {
        $input.addClass('is-invalid');
        let $errorSpan = $input.next('.validation-error-message');
        if ($errorSpan.length === 0) {
            $errorSpan = $('<span class="text-danger d-block mt-1 validation-error-message small"></span>');
            $input.after($errorSpan);
        }
        $errorSpan.text(msg);
    }

    function clearError() {
        $input.removeClass('is-invalid');
        $input.next('.validation-error-message').remove();
    }

    clearError();

    const num = parseFloat(value);
    if (isNaN(num) || num < 0) {
        setError("Value must be 0 or greater");
        return false;
    }

    return true;
}

// --- Real-time Global Validation ---
document.addEventListener("DOMContentLoaded", function () {
    initGlobalRealTimeValidation();
});

function initGlobalRealTimeValidation() {
    // Event Delegation for Real-time validation
    // Use 'input' for text entry and 'change' for selects/dates
    const events = 'input change';

    $(document).on(events, '[required], [data-val-required], [data-val-alphanumeric], [name$="Home_Address"], [data-val-maxlength], [data-val-email], [data-val-phone], [data-val-number], [data-val-date-from], [data-val-date-to], [data-val-future-date], [data-val-letters-symbols]', function (e) {
        const $el = $(this);
        const val = $el.val();

        // 1. Required Check
        // returns false if invalid (and sets error), true if valid (and clears error)
        if ($el.prop('required') || $el.is('[data-val-required]')) {
            if (!validateRequired(this)) return; // Stop if required fails
        }

        // Return early if empty (and not required, or required passed)
        if (!val) return;

        // 2. Format Checks - Stop on first failure to prevent subsequent checks from clearing the error

        // Letters & Symbols (FullName)
        if ($el.is('[data-val-letters-symbols]')) {
            if (!validateNameWithSymbols(this)) return;
        }

        // Alphanumeric
        if ($el.is('[data-val-alphanumeric]')) {
            if (!validateAlphanumeric(this)) return;
        }

        // Home addresses
        if ($el.is('[name$="Home_Address"]')) {
            if (!validateAlphanumeric(this, { fieldLabel: "Home address", allowNumbersOnly: true })) return;
        }

        // Number / Positive
        if ($el.is('[data-val-number]')) {
            if (!validatePositiveNumber(this)) return;
        }

        // Email
        if ($el.attr('type') === 'email' || $el.is('[data-val-email]')) {
            if (!validateEmail(this)) return;
        }

        // Phone
        if ($el.is('[data-val-phone]')) {
            if (!validatePhone(this)) return;
        }

        // Max Length (Run this last as it's the most permissive usually)
        const maxLen = $el.attr('data-val-maxlength');
        if (maxLen) {
            if (!validateMaxLength(this, parseInt(maxLen))) return;
        }

        // Future Date Check
        if ($el.is('[data-val-future-date]')) {
            if (!validateFutureDate(this)) return;
        }

        // 3. Date Range Checks (Paired)
        if ($el.is('[data-val-date-from]')) {
            const $container = $el.closest('[data-repeater-item], .form-group, .row');
            const $to = $container.find('[data-val-date-to]');
            if ($to.length && $to.val()) {
                validateDateRange($el, $to);
            }
        }

        if ($el.is('[data-val-date-to]')) {
            const $container = $el.closest('[data-repeater-item], .form-group, .row');
            const $from = $container.find('[data-val-date-from]');
            if ($from.length && $from.val()) {
                validateDateRange($from, $el);
            }
        }
    });
}

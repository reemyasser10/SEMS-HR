function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}
function applyBadgeStyles() {
    $(".custom-badge").each(function () {
        let badge = $(this);
        let colorCode = badge.data("color");

        if (colorCode && !["#fff", "#ffff"].includes(colorCode.toLowerCase())) {
            let rgb = hexToRgb(colorCode);

            if (rgb) {
                badge.css({
                    "background-color": `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.1)`,
                    "color": colorCode
                });
            } else {
              
                badge.css({
                    "background-color": "#0c82141a",
                    "color": "#0c8278"
                });
            }
        } else {
            badge.css({
                "background-color": "#0c82141a",
                "color": "#0c8278"
            });
        }
    });
}

function initializeSelect2(elem) {
    $(elem).find('.select2').each(function () {
        if ($.fn.select2 && $(this).hasClass("select2-hidden-accessible")) {
            $(this).select2('destroy');
        }
        $(this).select2({
            dropdownParent: $(elem),
            dropdownPosition: 'up' 
        });
    });
}

$('.modal').on('shown.bs.modal', function () {
    applyBadgeStyles();
    initializeSelect2($(this))

});

function initializeFlatpickrRanges() {
    const rangeInputs = document.querySelectorAll('.flatpickr-range');

    if (rangeInputs.length > 0) {
        rangeInputs.forEach(input => {
            flatpickr(input, {
                mode: "range",
                dateFormat: "Y-m-d",
                allowInput: true,
                onClose: function (selectedDates, dateStr, instance) {
                    if (selectedDates.length === 2) {
                        const startDate = instance.formatDate(selectedDates[0], "Y-m-d");
                        const endDate = instance.formatDate(selectedDates[1], "Y-m-d");

                        const container = input.closest('div');
                        const startInput = container.querySelector('.start_date');
                        const endInput = container.querySelector('.end_date');

                        if (startInput && endInput) {
                            startInput.value = startDate;
                            endInput.value = endDate;
                        }
                    }
                }
            });
        });
    }
}

$(document).ready(function () {
    $('#cartIcon').click(function (e) {
        e.preventDefault();
        var isCartEmpty = $(this).data('is-cart-empty');
        var cartUrl = $(this).data('cart-url');

        if (isCartEmpty === true || isCartEmpty === "true") {
            $('#emptyCartModal').modal('show');
        } else {
            console.log(cartUrl)
            window.location.href = cartUrl;
        }
    });
    applyBadgeStyles();
    initializeFlatpickrRanges();
});



























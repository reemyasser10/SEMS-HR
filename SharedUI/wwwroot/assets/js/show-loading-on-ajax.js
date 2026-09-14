// Loading spinner on any ajax call
$(document).on({
    ajaxStart: function () {
        $(".loading-page").removeClass("d-none");
    },
    ajaxStop: function () {
        $(".loading-page").addClass("d-none");

    }
});
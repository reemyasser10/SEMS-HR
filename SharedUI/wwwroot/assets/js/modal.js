function isRTL() {
	return $('html').attr('dir') === 'rtl';
}
$(document).ready(function () {
	Swal = Swal.mixin({});

	$('body').on('click', '.modalBtn', function (e) {
		e.preventDefault();
		var href = $(this).attr('href');
		var target = $(this).data('modaltarget');
		if (href && target) {
			$(target).find('.modal-content').load(href, function () {

				$('.dropdown-menu').css('display', 'none');
				$(target).modal('show');
			});
		}
	});
	
	$('body').on('submit', '.dataTable-modal-form', function (e) {
		e.preventDefault();
		const $form = $(this); // Important fix
		
		// Allow views to cancel submit
		const proceed = $form.triggerHandler('form:pre-submit');
		if (proceed === false) {
			return;
		}

		if ($(".loading-page").length > 0) {
			$('.loading-page').removeClass('d-none');
		}
		let formData = new FormData(this);
		$.ajax({
			type: $form.attr('method'),
			url: $form.attr('action'),
			data: formData,
			processData: false,
			contentType: false,
			success: function (response) {
				$(".modal").modal('hide');
				$('.loading-page').addClass('d-none');

				if (response.isSuccess) {
					if ($('.datatables').length > 0 && $.fn.DataTable.isDataTable('.datatables')) {
						$('.datatables').DataTable().draw();
					} else {
						window.location.reload();
					}
				} else {
					Swal.fire({
						title: isRTL() ? 'خطأ!' : 'Error!',
						
						text: response.status.plainErrorMessage,
						icon: 'error',
						customClass: {
							confirmButton: 'btn btnn-primary waves-effect waves-light'
						},
						buttonsStyling: false,
						confirmButtonText: isRTL() ? 'حسناً' : 'OK'

					});
				}
			},
			error: function () {
				$(".modal").modal('hide');
				$('.loading-page').addClass('d-none');
				Swal.fire({
					title: isRTL() ? 'خطأ!' : 'Error!',
					text: isRTL()
						? 'حدث خطأ ما'
						: 'Something went wrong.',
					icon: 'error',
					customClass: {
						confirmButton: 'btn btnn-primary waves-effect waves-light'
					},
					buttonsStyling: false,
					confirmButtonText: isRTL() ? 'حسناً' : 'OK'

				});
			}
		});
	});


	$('body').on('submit', '.redirect-modal-form', function (e) {
		e.preventDefault();
		const $form = $(this);
		if ($(".loading-page").length > 0) {
			$('.loading-page').removeClass('d-none');
		}

		// Trigger a custom event that views can hook into
		const proceed = $form.triggerHandler('form:pre-submit');
		if (proceed === false) {
			if ($(".loading-page").length > 0) {
				$('.loading-page').addClass('d-none');
			}
			return;
		}
		
		let redirectUrl = $("#redirectUrl").length > 0 ? $("#redirectUrl").attr('href') : "" ;
		// Use FormData for file support
		let formData = new FormData(this);

		$.ajax({
			type: $form.attr('method'),
			url: $form.attr('action'),
			data: formData,
			processData: false, // Prevent jQuery from processing the data
			contentType: false, // Prevent jQuery from setting the content type
			success: function (response) {
				if ($(".loading-page").length > 0) {
					$('.loading-page').addClass('d-none');
				}
				if (response.isSuccess) {
					Swal.fire({
						title: isRTL() ? 'تم بنجاح!' : 'Success!',
						text: isRTL()
							? 'تم إرسال الطلب بنجاح!'
							: 'Request Submitted successfully!',
						icon: 'success',
						confirmButtonText: isRTL() ? 'حسناً' : 'OK',

						customClass: {
							confirmButton: 'btn btnn-primary waves-effect waves-light'
						},
						buttonsStyling: false
					}).then((result) => {
						if (result.isConfirmed && redirectUrl) {
							window.location.href = redirectUrl;
						}
					});
				} else {
					Swal.fire({
						title: isRTL() ? 'خطأ!' : 'Error!',
						text: response.status.plainErrorMessage,
						icon: 'error',
						confirmButtonText: isRTL() ? 'حسناً' : 'OK',

						customClass: {
							confirmButton: 'btn btnn-primary waves-effect waves-light'
						},
						buttonsStyling: false
					});
				}
			},
			error: function (e) {
				if ($(".loading-page").length > 0) {
					$('.loading-page').addClass('d-none');
				}
				Swal.fire({
					title: isRTL() ? 'خطأ!' : 'Error!',
					text: isRTL()
						? 'حدث خطأ ما'
						: 'Something went wrong.',
					icon: 'error',
					confirmButtonText: isRTL() ? 'حسناً' : 'OK',

					customClass: {
						confirmButton: 'btn btnn-primary waves-effect waves-light'
					},
					buttonsStyling: false
				});
			}
		});
	});


	$('body').on('submit', '.redirect-form', function (e) {
		e.preventDefault();

		const $form = $(this);

		// Show loading indicator if exists
		if ($(".loading-page").length > 0) {
			$('.loading-page').removeClass('d-none');
		}

		// Allow views to cancel submit
		const proceed = $form.triggerHandler('form:pre-submit');
		if (proceed === false) {
			if ($(".loading-page").length > 0) {
				$('.loading-page').addClass('d-none');
			}
			return;
		}

		let redirectUrl = $("#redirectUrl").length > 0 ? $("#redirectUrl").attr('href') : "";

		// Support file uploads
		let formData = new FormData(this);

		$.ajax({
			type: $form.attr('method'),
			url: $form.attr('action'),
			data: formData,
			processData: false,
			contentType: false,

			success: function (response) {

				// Hide loading spinner
				if ($(".loading-page").length > 0) {
					$('.loading-page').addClass('d-none');
				}

				// ============================
				// CASE 1 — Validation Errors
				// ============================
				if (Array.isArray(response)) {

					const htmlList = `
                    <ul style="text-align:left; padding-left:20px;">
                        ${response.map(m => `<li>${m}</li>`).join("")}
                    </ul>
                `;

					Swal.fire({
						title: isRTL() ? 'خطأ في التحقق' : 'Validation Error',
						html: htmlList,
						confirmButtonText: isRTL() ? 'حسناً' : 'OK',

						icon: 'error',
						customClass: {
							confirmButton: 'btn btnn-primary waves-effect waves-light'
						},
						buttonsStyling: false
					});

					return; // <<< STOP HERE (NOT redirecting)
				}

				// ============================
				// CASE 2 — Success Response
				// ============================
				if (response.isSuccess) {

					Swal.fire({
						title: isRTL() ? 'تم بنجاح!' : 'Success!',
						text: isRTL()
							? 'تم إرسال الطلب بنجاح!'
							: 'Request Submitted successfully!',
						icon: 'success',
						confirmButtonText: isRTL() ? 'حسناً' : 'OK',

						customClass: {
							confirmButton: 'btn btnn-primary waves-effect waves-light'
						},
						buttonsStyling: false
					}).then((result) => {
						if (result.isConfirmed && redirectUrl) {
							window.location.href = redirectUrl;
						}
					});

					return;
				}

				// ============================
				// CASE 3 — Backend Error
				// ============================
				Swal.fire({
					title: 'Error!',
					text: response.status?.plainErrorMessage ?? "An error occurred.",
					icon: 'error',
					confirmButtonText: isRTL() ? 'حسناً' : 'OK',

					customClass: {
						confirmButton: 'btn btnn-primary waves-effect waves-light'
					},
					buttonsStyling: false
				});
			},

			// ============================
			// AJAX REQUEST FAILED
			// ============================
			error: function () {
				if ($(".loading-page").length > 0) {
					$('.loading-page').addClass('d-none');
				}

				Swal.fire({
					title: isRTL() ? 'خطأ!' : 'Error!',
					text: isRTL()
						? 'حدث خطأ ما'
						: 'Something went wrong.',
					icon: 'error',
					confirmButtonText: isRTL() ? 'حسناً' : 'OK',

					customClass: {
						confirmButton: 'btn btnn-primary waves-effect waves-light'
					},
					buttonsStyling: false
				});
			}
		});
	});




});

$('body').on('click', '.offcanvasBtn', function (e) {
    e.preventDefault();

    var href = $(this).attr('href');
    var target = $(this).data('offcanvastarget');

    if (href && target) {
        $(target).find('.offcanvas-body').load(href, function () {
            const offcanvasEl = document.querySelector(target);
            const bsOffcanvas = new bootstrap.Offcanvas(offcanvasEl);
            bsOffcanvas.show();
        });
    }
});
// Toggle DropdownMenuBtn
$('body').on('click', '.dropdownMenuBtn', function (e) {
	e.preventDefault();
	const $btn = $(this);
	const menuId = $btn.data('menuid');
	const $menu = $('#' + menuId);

	$('.dropdown-menu').not($menu).removeClass('show');
	$menu.toggleClass('show');

	$(document).one('click', function (event) {
		if (!$(event.target).closest('.dropdownMenuBtn, .dropdown-menu').length) {
			$menu.removeClass('show');
		}
	});
});



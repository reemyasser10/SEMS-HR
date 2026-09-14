/**
 * Admin Portal – Header Notifications
 * Fetches notifications from /Services/GetNotifications and renders them
 * in the #notification-list dropdown.  Uses the page direction (dir attr on
 * <html>) to choose MessageAr (rtl) or MessageEn (ltr).
 */
(function () {
    'use strict';

    const REFRESH_INTERVAL_MS = 60000; // 60 seconds
    const ENDPOINT = '/Services/GetNotifications';
    const STORAGE_KEY = 'mighty_seen_notification_ids';
    let currentNotificationIds = '';

    // State
    let isLoading = false;

    // Refresh interval reference
    let refreshIntervalId = null;

    /** Detect language from the page direction attribute */
    function isArabic() {
        var dir = document.documentElement.getAttribute('dir');
        return dir && dir.toLowerCase() === 'rtl';
    }

    /** Build a single notification item */
    function buildItem(notification) {
        var message = isArabic() ? notification.messageAr : notification.messageEn;
        var title = isArabic() ? notification.titleAr : notification.titleEn;

        // Ensure we handle URL correctly, whether it comes as webUrl or WebUrl
        var url = notification.webUrl || notification.WebUrl || '#';

        var li = document.createElement('li');
        li.className = 'dropdown-notifications-list-item d-flex notification-item';

        var a = document.createElement('a');
        a.href = url;
        a.className = 'dropdown-notifications-item w-100 d-flex gap-3 align-items-center py-2 px-3 border-bottom text-body';
        a.style.textDecoration = 'none';

        // Icon
        var iconWrapper = document.createElement('div');
        iconWrapper.className = 'flex-shrink-0';
        var icon = document.createElement('div');
        icon.className = 'avatar avatar-sm notification-primary';
        icon.innerHTML = '<span class="avatar-initial rounded-circle bg-label-primary"><i class="ti ti-bell ti-sm "></i></span>';
        iconWrapper.appendChild(icon);

        // Text Content Wrapper
        var textWrapper = document.createElement('div');
        textWrapper.className = 'd-flex flex-column flex-grow-1 overflow-hidden';

        // Title
        if (title) {
            var titleSpan = document.createElement('h6');
            titleSpan.className = 'mb-1 text-truncate text-heading fw-semibold';
            titleSpan.style.lineHeight = '1.2';
            titleSpan.style.fontSize = '0.9rem';
            titleSpan.textContent = title;
            textWrapper.appendChild(titleSpan);
        }

        // Message
        var msgSpan = document.createElement('small');
        msgSpan.className = 'text-wrap text-muted';
        msgSpan.style.lineHeight = '1.4';
        msgSpan.style.fontSize = '0.8125rem';
        msgSpan.textContent = message;
        textWrapper.appendChild(msgSpan);

        a.appendChild(iconWrapper);
        a.appendChild(textWrapper);
        li.appendChild(a);

        return li;
    }

    /** Fetch and render notifications */
    function loadNotifications() {
        if (isLoading) return;
        isLoading = true;

        var loading = document.getElementById('notification-loading');
        if (loading) loading.classList.remove('d-none');

        fetch(ENDPOINT)
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                isLoading = false;
                var list = document.getElementById('notification-list');
                var empty = document.getElementById('notification-empty');
                var badge = document.getElementById('notification-count');

                if (loading) loading.classList.add('d-none');

                if (!list) return;

                // Remove any previous notification items
                var prev = list.querySelectorAll('.notification-item');
                for (var i = 0; i < prev.length; i++) {
                    prev[i].remove();
                }

                if (!data || data.length === 0) {
                    // Show empty state
                    if (empty) empty.classList.remove('d-none');
                    if (badge) badge.classList.add('d-none');
                    return;
                }

                // Hide empty state
                if (empty) empty.classList.add('d-none');

                var seenIdsRaw = localStorage.getItem(STORAGE_KEY) || '';
                var seenIdsArray = seenIdsRaw.split(',');
                var hasNew = false;
                var currentIdsArray = [];

                for (var j = 0; j < data.length; j++) {
                    var item = buildItem(data[j]);
                    if (empty) {
                        list.insertBefore(item, empty);
                    } else {
                        list.appendChild(item);
                    }

                    // Track ID to detect new unseen notifications
                    var idKey = (data[j].type || 0) + '-' + (data[j].referenceId || data[j].ReferenceId || 0);
                    if (currentIdsArray.indexOf(idKey) === -1) {
                         currentIdsArray.push(idKey);
                    }

                    if (seenIdsArray.indexOf(idKey) === -1) {
                        hasNew = true;
                    }
                }

                currentNotificationIds = currentIdsArray.join(',');

                // Update badge (dot only, no text)
                if (badge) {
                    badge.textContent = '';
                    if (currentIdsArray.length > 0 && hasNew) {
                        badge.classList.remove('d-none');
                    } else {
                        badge.classList.add('d-none');
                    }
                }

                // If it's the first page without header, maybe we could update the total count
                // We typically use X-Pagination.TotalCount, but for safely ignoring bugs in missing headers
                // we leave this logic simpler. Let's look for TotalCount if possible or set a simple generic phrase
            })
            .catch(function (err) {
                isLoading = false;
                console.error('Failed to load notifications:', err);
                var loading = document.getElementById('notification-loading');
                if (loading) loading.classList.add('d-none');
            });
    }

    /** Refresh data */
    function refreshNotifications() {
        loadNotifications();
    }

    // Initial load
    document.addEventListener('DOMContentLoaded', function () {
        // Initial Fetch
        loadNotifications();

        // Auto-refresh
        refreshIntervalId = setInterval(refreshNotifications, REFRESH_INTERVAL_MS);

        // Hide dot and update seen count when dropdown is opened (clicked)
        var dropdownToggles = document.querySelectorAll('.dropdown-notifications .dropdown-toggle');
        for (var i = 0; i < dropdownToggles.length; i++) {
            dropdownToggles[i].addEventListener('click', function () {
                if (currentNotificationIds) {
                    localStorage.setItem(STORAGE_KEY, currentNotificationIds);
                }
                var badge = document.getElementById('notification-count');
                if (badge) badge.classList.add('d-none');
            });
        }
    });
})();

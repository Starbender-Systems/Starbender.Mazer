window.abpMudBlazorTheme = window.abpMudBlazorTheme || {};

(function (theme) {
    var storageKey = "abpMudBlazorTheme.state";
    theme.storageKey = theme.storageKey || storageKey;

    function readState() {
        try {
            var raw = window.localStorage.getItem(storageKey);
            if (!raw) {
                return null;
            }

            var state = JSON.parse(raw);
            if (state && typeof state.isDarkMode === "boolean") {
                return state;
            }
        } catch (error) {
            // Ignore localStorage parsing/access issues.
        }

        return null;
    }

    function writeState(state) {
        var normalized = {
            isDarkMode: !!(state && state.isDarkMode)
        };

        try {
            window.localStorage.setItem(storageKey, JSON.stringify(normalized));
        } catch (error) {
            // Ignore localStorage write failures.
        }

        return normalized;
    }

    theme.getSystemDarkMode = theme.getSystemDarkMode || function () {
        return !!(window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches);
    };

    theme.getThemeState = theme.getThemeState || function () {
        var state = readState();
        if (state) {
            return state;
        }

        return {
            isDarkMode: theme.getSystemDarkMode()
        };
    };

    theme.setThemeState = theme.setThemeState || function (state) {
        return writeState(state);
    };

    theme.getPreferredDarkMode = theme.getPreferredDarkMode || function () {
        return theme.getThemeState().isDarkMode;
    };

    theme.setBootstrapTheme = theme.setBootstrapTheme || function (isDarkMode) {
        var mode = isDarkMode ? "dark" : "light";
        document.documentElement.setAttribute("data-bs-theme", mode);
        if (document.body) {
            document.body.setAttribute("data-bs-theme", mode);
        }
    };

    theme.setDarkMode = theme.setDarkMode || function (isDarkMode) {
        var darkMode = !!isDarkMode;
        theme.setBootstrapTheme(darkMode);
        writeState({ isDarkMode: darkMode });
        return darkMode;
    };

    theme.toggleDarkMode = theme.toggleDarkMode || function () {
        return theme.setDarkMode(!theme.getPreferredDarkMode());
    };
})(window.abpMudBlazorTheme);

$(function () {
    $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
        $(this).next().toggleClass('show');

        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
        }

        var $subMenu = $(this).next(".dropdown-menu");
        $subMenu.toggleClass('show');

        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass("show");
        });

        return false;
    });

    var theme = window.abpMudBlazorTheme;
    var $themeToggle = $('[data-abp-theme-toggle="true"]');

    function setThemeToggleState(isDarkMode) {
        $themeToggle.find('.abp-theme-mode-toggle-icon').text(isDarkMode ? 'dark_mode' : 'light_mode');
        $themeToggle.attr('aria-pressed', isDarkMode.toString());
    }

    if (theme && typeof theme.getPreferredDarkMode === "function" && typeof theme.setDarkMode === "function") {
        var isDarkMode = theme.getPreferredDarkMode();
        theme.setDarkMode(isDarkMode);
        setThemeToggleState(isDarkMode);

        $themeToggle.on('click', function (event) {
            event.preventDefault();
            var updatedDarkMode = theme.toggleDarkMode();
            setThemeToggleState(updatedDarkMode);
        });

        window.addEventListener('storage', function (event) {
            if (event.key !== theme.storageKey) {
                return;
            }

            var updatedDarkMode = theme.getPreferredDarkMode();
            theme.setBootstrapTheme(updatedDarkMode);
            setThemeToggleState(updatedDarkMode);
        });
    }

    var $shell = $('.abp-mud-shell');
    var $drawerToggle = $('.abp-mud-drawer-toggle[aria-controls="abp-mud-drawer"]');

    function setDrawerState(isCollapsed) {
        $shell.toggleClass('abp-mud-drawer-collapsed', isCollapsed);
        $drawerToggle.attr('aria-expanded', (!isCollapsed).toString());
    }

    if ($shell.length && $drawerToggle.length) {
        if (window.matchMedia('(max-width: 991.98px)').matches) {
            setDrawerState(true);
        } else {
            setDrawerState(false);
        }

        $drawerToggle.on('click', function () {
            setDrawerState(!$shell.hasClass('abp-mud-drawer-collapsed'));
        });

        $('.abp-mud-drawer .nav-link').on('click', function () {
            if (window.matchMedia('(max-width: 991.98px)').matches) {
                setDrawerState(true);
            }
        });
    }
});

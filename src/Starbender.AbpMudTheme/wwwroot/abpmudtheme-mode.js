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

    theme.getSystemDarkMode = function () {
        return !!(window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches);
    };

    theme.getThemeState = function () {
        var state = readState();
        if (state) {
            return state;
        }

        return {
            isDarkMode: theme.getSystemDarkMode()
        };
    };

    theme.setThemeState = function (state) {
        return writeState(state);
    };

    theme.getPreferredDarkMode = function () {
        return theme.getThemeState().isDarkMode;
    };

    theme.setBootstrapTheme = function (isDarkMode) {
        var mode = isDarkMode ? "dark" : "light";
        document.documentElement.setAttribute("data-bs-theme", mode);
        if (document.body) {
            document.body.setAttribute("data-bs-theme", mode);
        }
    };

    theme.setDarkMode = function (isDarkMode) {
        var darkMode = !!isDarkMode;
        theme.setBootstrapTheme(darkMode);
        writeState({ isDarkMode: darkMode });
        return darkMode;
    };

    theme.toggleDarkMode = function () {
        return theme.setDarkMode(!theme.getPreferredDarkMode());
    };
})(window.abpMudBlazorTheme);

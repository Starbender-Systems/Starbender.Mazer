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

document.addEventListener("DOMContentLoaded", function () {
    var dropdownToggles = document.querySelectorAll(".dropdown-menu a.dropdown-toggle");

    dropdownToggles.forEach(function (toggle) {
        toggle.addEventListener("click", function (event) {
            event.preventDefault();

            var subMenu = this.nextElementSibling;
            if (!subMenu) {
                return;
            }

            subMenu.classList.toggle("show");

            if (!subMenu.classList.contains("show")) {
                var parentMenu = this.closest(".dropdown-menu");
                if (parentMenu) {
                    parentMenu.querySelectorAll(".show").forEach(function (visibleItem) {
                        visibleItem.classList.remove("show");
                    });
                }
            }

            var openParentDropdown = this.closest("li.nav-item.dropdown.show");
            if (openParentDropdown) {
                openParentDropdown.addEventListener("hidden.bs.dropdown", function () {
                    document.querySelectorAll(".dropdown-submenu .show").forEach(function (openedSubmenu) {
                        openedSubmenu.classList.remove("show");
                    });
                }, { once: true });
            }
        });
    });

    var theme = window.abpMudBlazorTheme;
    var themeToggles = document.querySelectorAll('[data-abp-theme-toggle="true"]');

    function setThemeToggleState(isDarkMode) {
        themeToggles.forEach(function (toggle) {
            var icon = toggle.querySelector(".abp-theme-mode-toggle-icon");
            if (icon) {
                icon.textContent = isDarkMode ? "dark_mode" : "light_mode";
            }
            toggle.setAttribute("aria-pressed", isDarkMode.toString());
        });
    }

    if (theme && typeof theme.getPreferredDarkMode === "function" && typeof theme.setDarkMode === "function") {
        var isDarkMode = theme.getPreferredDarkMode();
        theme.setDarkMode(isDarkMode);
        setThemeToggleState(isDarkMode);

        themeToggles.forEach(function (toggle) {
            toggle.addEventListener("click", function (event) {
                event.preventDefault();
                var updatedDarkMode = theme.toggleDarkMode();
                setThemeToggleState(updatedDarkMode);
            });
        });

        window.addEventListener("storage", function (event) {
            if (event.key !== theme.storageKey) {
                return;
            }

            var updatedDarkMode = theme.getPreferredDarkMode();
            theme.setBootstrapTheme(updatedDarkMode);
            setThemeToggleState(updatedDarkMode);
        });
    }

    var shell = document.querySelector(".abp-mud-shell");
    var drawerToggles = document.querySelectorAll('.abp-mud-drawer-toggle[aria-controls="abp-mud-drawer"]');

    function setDrawerState(isCollapsed) {
        if (!shell) {
            return;
        }

        shell.classList.toggle("abp-mud-drawer-collapsed", isCollapsed);
        drawerToggles.forEach(function (toggle) {
            toggle.setAttribute("aria-expanded", (!isCollapsed).toString());
        });
    }

    if (shell && drawerToggles.length > 0) {
        setDrawerState(window.matchMedia("(max-width: 991.98px)").matches);

        drawerToggles.forEach(function (toggle) {
            toggle.addEventListener("click", function () {
                setDrawerState(!shell.classList.contains("abp-mud-drawer-collapsed"));
            });
        });

        document.querySelectorAll(".abp-mud-drawer .nav-link").forEach(function (navLink) {
            navLink.addEventListener("click", function () {
                if (window.matchMedia("(max-width: 991.98px)").matches) {
                    setDrawerState(true);
                }
            });
        });
    }
});

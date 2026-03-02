window.abpMudBlazorTheme = window.abpMudBlazorTheme || {};

window.abpMudBlazorTheme.getSystemDarkMode = function () {
    return !!(window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches);
};

window.abpMudBlazorTheme.setBootstrapTheme = function (isDarkMode) {
    var mode = isDarkMode ? 'dark' : 'light';
    document.documentElement.setAttribute('data-bs-theme', mode);
    if (document.body) {
        document.body.setAttribute('data-bs-theme', mode);
    }
};

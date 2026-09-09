(() => {
    const themeModeCookie = "cv-theme-mode";
    const accentCookie = "cv-accent";
    const maxAge = 60 * 60 * 24 * 365;
    const themeModes = new Set(["light", "dark", "auto"]);
    const accents = new Set(["blue", "indigo", "teal", "rose", "amber"]);

    const getCookie = (name) => {
        const prefix = `${name}=`;
        const found = document.cookie.split("; ").find((part) => part.startsWith(prefix));
        return found ? decodeURIComponent(found.slice(prefix.length)) : null;
    };

    const setCookie = (name, value) => {
        document.cookie = `${name}=${encodeURIComponent(value)}; path=/; max-age=${maxAge}; samesite=lax`;
    };

    const resolveTheme = (mode) => {
        if (mode === "dark") {
            return "dark";
        }

        if (mode === "auto") {
            return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
        }

        return "light";
    };

    const apply = (mode, accent) => {
        const themeMode = themeModes.has(mode) ? mode : "light";
        const color = accents.has(accent) ? accent : "blue";
        const root = document.documentElement;
        root.setAttribute("data-bs-theme", resolveTheme(themeMode));
        root.setAttribute("data-theme-mode", themeMode);
        root.setAttribute("data-accent", color);
    };

    const applyFromCookies = () => {
        apply(getCookie(themeModeCookie) || "light", getCookie(accentCookie) || "blue");
    };

    window.uiPreferences = {
        applyFromCookies,
        setThemeMode(mode) {
            const themeMode = themeModes.has(mode) ? mode : "light";
            setCookie(themeModeCookie, themeMode);
            apply(themeMode, getCookie(accentCookie) || "blue");
        },
        setAccent(accent) {
            const color = accents.has(accent) ? accent : "blue";
            setCookie(accentCookie, color);
            apply(getCookie(themeModeCookie) || "light", color);
        }
    };

    applyFromCookies();

    window.matchMedia("(prefers-color-scheme: dark)").addEventListener("change", () => {
        if ((getCookie(themeModeCookie) || "light") === "auto") {
            applyFromCookies();
        }
    });
})();

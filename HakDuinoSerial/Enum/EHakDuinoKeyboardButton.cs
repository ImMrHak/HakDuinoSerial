namespace HakDuinoSerial.Enum
{
    public enum EHakDuinoKeyboardButton
    {
        /// <summary>
        /// Alphanumeric keys (A-Z and 0-9).
        /// </summary>
        A, B, C, D, E, F, G, H, I, J, K, L, M,
        N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        Number0, Number1, Number2, Number3, Number4,
        Number5, Number6, Number7, Number8, Number9,

        /// <summary>
        /// Special character keys, including space, enter, backspace, tab, and escape.
        /// </summary>
        Space, Enter, Backspace, Tab, Escape,

        /// <summary>
        /// Modifier keys, such as Shift, Control, Alt, and Command (Meta).
        /// </summary>
        Shift, Control, Alt, Command,

        /// <summary>
        /// Arrow keys for directional navigation.
        /// </summary>
        ArrowUp, ArrowDown, ArrowLeft, ArrowRight,

        /// <summary>
        /// Function keys (F1-F12).
        /// </summary>
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,

        /// <summary>
        /// Additional navigation keys, including Insert, Delete, Home, End, PageUp, and PageDown.
        /// </summary>
        Insert, Delete, Home, End, PageUp, PageDown,

        /// <summary>
        /// Other symbol keys, such as comma, period, semicolon, brackets, and more.
        /// </summary>
        Comma, Period, Semicolon, Quote, BracketLeft, BracketRight,
        Slash, Backslash, Minus, Plus, Equals
    }
}

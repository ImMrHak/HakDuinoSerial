namespace HakDuinoSerial.HID.Configuration // New namespace for config
{
    /// <summary>
    /// Configuration settings for connecting and communicating with the HakDuino HID device.
    /// </summary>
    public class HakDuinoHidConfig
    {
        // --- Device Identification ---
        /// <summary>
        /// The Vendor ID (VID) to use for device discovery (primarily as a fallback). Default: 0x2341 (Arduino).
        /// </summary>
        public int VendorID { get; set; } = 0x2341;

        /// <summary>
        /// The Product ID (PID) to use for device discovery (primarily as a fallback). Default: 0xBEEF (Common RawHID).
        /// </summary>
        public int ProductID { get; set; } = 0xBEEF;

        /// <summary>
        /// The specific HID Usage Page for device discovery (preferred method). Set to 0 to disable UsagePage matching. Default: 0xFF60.
        /// </summary>
        public ushort CustomUsagePage { get; set; } = 0xFF60; // Set to 0 to rely only on VID/PID

        /// <summary>
        /// The specific HID Usage ID within the CustomUsagePage for device discovery (preferred method). Default: 0x61.
        /// </summary>
        public ushort CustomUsage { get; set; } = 0x61;

        // --- RawHID Report Configuration ---
        /// <summary>
        /// The Report ID expected by the device for Output reports (PC -> Arduino). Default: 0x00.
        /// </summary>
        public byte ReportId { get; set; } = 0x00;

        /// <summary>
        /// The total size of the output report buffer to send (including Report ID byte). Must be large enough for ReportID + Key + Cmd + MaxData + Padding. Default: 65.
        /// </summary>
        public int ReportSize { get; set; } = 65; // 1 byte ID + 64 byte payload

        // --- Security ---
        /// <summary>
        /// Specifies whether to use the Secret Key mechanism for authentication. Default: true.
        /// </summary>
        public bool UseSecretKey { get; set; } = true;

        /// <summary>
        /// The secret key byte sequence. Must match the Arduino sketch if UseSecretKey is true. Default: {0xDE, 0xAD, 0xBE, 0xEF}.
        /// </summary>
        public byte[] SecretKey { get; set; } = { 0xDE, 0xAD, 0xBE, 0xEF };

        // --- Command Bytes ---
        /// <summary>
        /// The command byte used to signify a relative mouse movement command. Default: 0xB1.
        /// </summary>
        public byte CommandMouseMove { get; set; } = 0xB1;

        /// <summary>
        /// The command byte used to signify a left mouse click command. Default: 0xB2.
        /// </summary>
        public byte CommandLeftClick { get; set; } = 0xB2;

        /// <summary>
        /// The command byte used to signify a right mouse click command. Default: 0xB3.
        /// </summary>
        public byte CommandRightClick { get; set; } = 0xB3; // Example for extensibility

        // Add other command bytes as needed (e.g., middle click, scroll)
        // public byte CommandMiddleClick { get; set; } = 0xA4;
        // public byte CommandScroll { get; set; } = 0xA5;


        /// <summary>
        /// Validates the configuration, ensuring ReportSize is sufficient.
        /// </summary>
        /// <returns>True if configuration is valid, false otherwise.</returns>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;
            int requiredMinSize = 1; // Report ID
            if (UseSecretKey && SecretKey != null)
            {
                requiredMinSize += SecretKey.Length;
            }
            requiredMinSize += 1; // Smallest command byte

            // Add size of largest potential payload if needed for validation
            // Example: Mouse move payload = 2 bytes
            // requiredMinSize += 2;

            if (ReportSize < requiredMinSize)
            {
                errorMessage = $"Config Error: ReportSize ({ReportSize}) is too small. Minimum required is {requiredMinSize} (ReportID + Optional Key + CmdByte).";
                return false;
            }
            return true;
        }
    }
}
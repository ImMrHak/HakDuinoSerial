using HakDuinoSerial.HID.Service;
using System.Text;
using HidLibrary;
using HakDuinoSerial.HID.Configuration;

namespace HakDuinoSerial.HID
{
    /// <summary>
    /// Provides an interface to communicate with an Arduino as an HID device.
    /// This class enables sending commands over an HID connection, managing the connection state,
    /// and retrieving information about the connected device. It supports sending custom commands
    /// and keeps track of command history for monitoring actions performed through the HID interface.
    /// </summary>
    public class HakDuinoHID : IHakDuinoHID, IDisposable
    {
        protected HidDevice? arduinoHID; // Use protected if HakDuinoHIDMouse needs direct access
        private readonly List<string> allActions;
        private readonly HakDuinoHidConfig config; // Store the configuration

        /// <summary>
        /// Initializes a new instance of the HakDuinoHID class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration settings for the HID device connection.</param>
        /// <exception cref="ArgumentNullException">Thrown if configuration is null.</exception>
        /// <exception cref="ArgumentException">Thrown if configuration validation fails.</exception>
        public HakDuinoHID(HakDuinoHidConfig configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (!configuration.Validate(out string error))
                throw new ArgumentException($"Invalid HID configuration: {error}", nameof(configuration));

            this.config = configuration; // Store the provided config
            allActions = new List<string>();
        }

        public bool OpenConnection()
        {
            if (arduinoHID != null && arduinoHID.IsConnected) return true;

            try
            {
                // Use configuration for device discovery
                IEnumerable<HidDevice> possibleDevices = HidDevices.Enumerate(config.VendorID, config.ProductID);

                // Filter by UsagePage/Usage if specified in config
                if (config.CustomUsagePage != 0)
                {
                    arduinoHID = possibleDevices
                                   .FirstOrDefault(d => d.Capabilities.UsagePage == config.CustomUsagePage &&
                                                        d.Capabilities.Usage == config.CustomUsage);
                    if (arduinoHID != null)
                    {
                        Console.WriteLine($"Found device by Custom UsagePage/Usage: {arduinoHID.Description}");
                    }
                }

                // If not found by UsagePage/Usage, or UsagePage was 0, use the first VID/PID match
                if (arduinoHID == null)
                {
                    arduinoHID = possibleDevices.FirstOrDefault();
                    if (arduinoHID != null)
                    {
                        Console.WriteLine($"Found device by VID/PID: {arduinoHID.Description}");
                    }
                }


                if (arduinoHID == null)
                {
                    Console.WriteLine($"Arduino HID device not found matching configuration (VID={config.VendorID:X4}, PID={config.ProductID:X4}, UsagePage={config.CustomUsagePage:X4}, Usage={config.CustomUsage:X4}).");
                    return false;
                }

                arduinoHID.OpenDevice();

                if (arduinoHID.IsConnected)
                {
                    Console.WriteLine($"Connected to: {GetHIDDeviceInfo()}");
                    addActionToList("HID Connection Opened");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to open connection to the HID device.");
                    arduinoHID = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening HID connection: {ex.Message}");
                if (arduinoHID != null && arduinoHID.IsOpen) arduinoHID.CloseDevice();
                arduinoHID = null;
                return false;
            }
        }

        public bool SendRawHidReport(byte commandByte, byte[]? dataPayload = null)
        {
            if (arduinoHID == null || !arduinoHID.IsConnected)
            {
                Console.WriteLine("Error: Arduino HID device is not connected.");
                return false;
            }

            try
            {
                // Use config values for report size and ID
                byte[] reportBuffer = new byte[config.ReportSize];
                reportBuffer[0] = config.ReportId;

                int currentIndex = 1;

                // Conditionally add Secret Key based on config
                if (config.UseSecretKey && config.SecretKey != null && config.SecretKey.Length > 0)
                {
                    if (currentIndex + config.SecretKey.Length < reportBuffer.Length)
                    {
                        Buffer.BlockCopy(config.SecretKey, 0, reportBuffer, currentIndex, config.SecretKey.Length);
                        currentIndex += config.SecretKey.Length;
                    }
                    else
                    {
                        Console.WriteLine("Error: Report buffer too small for secret key.");
                        return false;
                    }
                }

                // Add Command Byte
                if (currentIndex < reportBuffer.Length)
                {
                    reportBuffer[currentIndex++] = commandByte;
                }
                else
                {
                    Console.WriteLine("Error: Report buffer too small for secret key + command byte.");
                    return false;
                }

                // Add Data Payload
                if (dataPayload != null && dataPayload.Length > 0)
                {
                    if (currentIndex + dataPayload.Length <= reportBuffer.Length)
                    {
                        Buffer.BlockCopy(dataPayload, 0, reportBuffer, currentIndex, dataPayload.Length);
                        // currentIndex += dataPayload.Length; // Optional: track final index if needed
                    }
                    else
                    {
                        Console.WriteLine("Error: Report buffer too small for payload data.");
                        return false;
                    }
                }

                // Send the report
                bool success = arduinoHID.Write(reportBuffer);

                if (success)
                {
                    string payloadHex = dataPayload != null ? BitConverter.ToString(dataPayload).Replace("-", " ") : "None";
                    string keyInfo = config.UseSecretKey ? "(Key Used)" : "(No Key)";
                    addActionToList($"Sent Cmd: 0x{commandByte:X2} {keyInfo}, Payload: [{payloadHex}]");
                }
                else
                {
                    Console.WriteLine($"Failed to write HID report. Cmd: 0x{commandByte:X2}");
                }

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending Raw HID report: {ex.Message}");
                return false;
            }
        }

        public bool SendCustomCommand(string command)
        {
            try
            {
                if (arduinoHID == null || !arduinoHID.IsConnected)
                {
                    Console.WriteLine("Arduino HID device is not connected.");
                    return false;
                }

                // Convert command to byte array and send it immediately
                var data = Encoding.ASCII.GetBytes(command + "\n"); // Add newline or any other required delimiter

                arduinoHID.Write(data, result =>
                {
                    if (!result)
                    {
                        Console.WriteLine("Failed to send data to Arduino HID device.");
                    }
                });

                addActionToList(command);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetHIDDeviceInfo()
        {
            try
            {
                if (arduinoHID != null && arduinoHID.IsConnected)
                {
                    return $"Device: {arduinoHID.Description}, VID: {arduinoHID.Attributes.VendorId:X4}, PID: {arduinoHID.Attributes.ProductId:X4}, Connected: {arduinoHID.IsConnected}";
                }
                else
                {
                    return "Arduino HID device not connected.";
                }
            }
            catch (Exception ex)
            {
                return $"Error getting device info: {ex.Message}";
            }
        }

        public List<string> GetCommandHistory()
        {
            return new List<string>(allActions);
        }

        protected void addActionToList(string action)
        {
            allActions.Add($"{DateTime.Now:HH:mm:ss.fff} - {action}");
        }

        public bool CloseConnection()
        {
            try
            {
                if (arduinoHID == null) return true;
                if (arduinoHID.IsConnected)
                {
                    arduinoHID.CloseDevice();
                    addActionToList("HID Connection Closed");
                }
                arduinoHID = null;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing HID connection: {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                CloseConnection(); // Ensure device is closed
            }
            // Dispose unmanaged resources if any
        }

        // Finalizer (optional)
        ~HakDuinoHID()
        {
            Dispose(false);
        }
    }
}

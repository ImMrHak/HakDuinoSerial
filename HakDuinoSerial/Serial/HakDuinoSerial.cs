using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;
using HakDuinoSerial.Serial.Service;

namespace HakDuinoSerial.Serial
{
    /// <summary>
    /// Provides an interface to communicate with an Arduino over a serial connection.
    /// This class enables control of mouse movements, clicks, and scrolling actions through buffered serial commands.
    /// It also supports fetching device information and managing the connection state.
    /// </summary>
    public class HakDuinoSerial : IHakDuinoSerial
    {
        private SerialPort arduino;
        private StringBuilder buffer;
        private List<string> allActions;

        public HakDuinoSerial(string COM_PORT, int BAUD_RATE = 250000)
        {
            arduino = new SerialPort(COM_PORT, BAUD_RATE);
            buffer = new StringBuilder();
            allActions = new List<string>();
        }

        public bool OpenConnection()
        {
            try
            {
                if (arduino.IsOpen) return true;
                arduino.Open();
                addActionToList("Connection Open");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool SendCustomCommand(string command)
        {
            try
            {
                BufferCommand(command);

                arduino.Write(buffer.ToString());
                buffer.Clear();
                addActionToList(command);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        protected void BufferCommand(string command)
        {
            buffer.Append(command + "\n");
        }

        public async Task<bool> FlushBufferedCommandsAsync(int threshold = 10)
        {
            try
            {
                if (buffer.Length > 0 && buffer.Length >= threshold)
                {
                    await Task.Run(() => arduino.Write(buffer.ToString()));
                    buffer.Clear();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public string GetArduinoInfo()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return GetArduinoInfoWindows();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return GetArduinoInfoLinux();
                }
                else
                {
                    return "Unsupported operating system.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected string GetArduinoInfoWindows()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SerialPort");
            foreach (var device in searcher.Get())
            {
                if (device["Name"] != null && device["Name"].ToString().Contains("Arduino"))
                {
                    return $"Name: {device["Name"]}, VID: {device["DeviceID"]}, PID: {device["PNPDeviceID"]}";
                }
            }
            return "Arduino not found on Windows.";
        }

        protected string GetArduinoInfoLinux()
        {
            string[] potentialDevices = Directory.GetFiles("/dev/", "ttyUSB*");
            foreach (string device in potentialDevices)
            {
                // Assuming Arduino devices show up as ttyUSB* on Linux
                if (device.Contains("ttyUSB"))
                {
                    return $"Device: {device}";
                }
            }
            return "Arduino not found on Linux.";
        }

        public SerialPort GetSerialPort()
        {
            try
            {
                return arduino;
            }
            catch
            {
                return null;
            }
        }

        public List<string> GetCommandHistory()
        {
            try
            {
                return allActions;
            }
            catch
            {
                return null;
            }
        }

        protected void addActionToList(string action)
        {
            allActions.Add("Action Used => " + action);
        }

        public bool CloseConnection()
        {
            try
            {
                if (!arduino.IsOpen) return true;
                arduino.Close();
                addActionToList($"Connection Closed");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

using HakDuinoSerial.Enum;
using HakDuinoSerial.Service;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Management;

namespace HakDuinoSerial
{
    public class HakDuinoSerial : IHakDuinoSerialCOM
    {
        private SerialPort arduino;
        private StringBuilder buffer;

        public HakDuinoSerial(string COM_PORT, int BAUD_RATE = 250000)
        {
            arduino = new SerialPort(COM_PORT, BAUD_RATE);
            buffer = new StringBuilder();
        }

        public bool OpenConnection()
        {
            try
            {
                if (arduino.IsOpen) return true;
                arduino.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void BufferCommand(string command)
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

        public bool MoveMouse(int x, int y)
        {
            try
            {
                string mouseMovement = $"M,{x},{y}";
                BufferCommand(mouseMovement);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ClickMouse(HakDuinoEnumButton mouseButton)
        {
            try
            {
                string mouseClick = $"C,{mouseButton.ToString()}";
                BufferCommand(mouseClick);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool MouseRightClick()
        {
            return ClickMouse(HakDuinoEnumButton.RIGHT);
        }

        public bool MouseLeftClick()
        {
            return ClickMouse(HakDuinoEnumButton.LEFT);
        }

        public bool ScrollWheel(HakDuinoEnumButton direction, int amount)
        {
            try
            {
                string scrollCommand = direction switch
                {
                    HakDuinoEnumButton.SCROLL_UP => $"S,UP,{amount}", // Send the command with proper format
                    HakDuinoEnumButton.SCROLL_DOWN => $"S,DOWN,{amount}", // Send the command with proper format
                    _ => throw new ArgumentException("Invalid scroll direction")
                };
                BufferCommand(scrollCommand);
                return true;
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

        private string GetArduinoInfoWindows()
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

        private string GetArduinoInfoLinux()
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

        public bool CloseConnection()
        {
            try
            {
                if (!arduino.IsOpen) return true;
                arduino.Close();
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

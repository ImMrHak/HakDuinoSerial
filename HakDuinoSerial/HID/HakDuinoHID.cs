using HakDuinoSerial.HID.Service;
using System.Text;
using HidLibrary;

namespace HakDuinoSerial.HID
{
    /// <summary>
    /// Provides an interface to communicate with an Arduino as an HID device.
    /// This class enables sending commands over an HID connection, managing the connection state,
    /// and retrieving information about the connected device. It supports sending custom commands
    /// and keeps track of command history for monitoring actions performed through the HID interface.
    /// </summary>
    public class HakDuinoHID : IHakDuinoHID
    {
        private HidDevice arduinoHID;
        private StringBuilder buffer;
        private List<string> allActions;

        public HakDuinoHID(int VendorID, int ProductID)
        {
            arduinoHID = HidDevices.Enumerate(VendorID, ProductID).First();
            buffer = new StringBuilder();
            allActions = new List<string>();
        }

        public bool OpenConnection()
        {
            try
            {
                if (arduinoHID == null)
                {
                    Console.WriteLine("Arduino HID device not found.");
                    return false;
                }

                arduinoHID.OpenDevice();
                addActionToList("HID Connection Open");
                return arduinoHID.IsConnected;
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
                    return $"Device Information: {arduinoHID.ToString()}";
                }
                else
                {
                    return "Arduino HID device not connected.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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
                if (arduinoHID == null || !arduinoHID.IsConnected) return true;
                arduinoHID.CloseDevice();
                addActionToList("HID Connection Closed");
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

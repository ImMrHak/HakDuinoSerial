# HakDuinoSerial

**HakDuinoSerial** is a C# library designed to facilitate communication with Arduino devices via serial connections. This library allows developers to send mouse commands and other data easily, streamlining the process of integrating Arduino with C# applications.

## Features

- **Simple Serial Communication**: Easily send and receive data from Arduino.
- **Mouse Command Integration**: Send mouse commands directly from your C# application to control cursor movements.
- **Cross-Platform Compatibility**: Works on Windows, Linux, and macOS.

## Installation

You can install the HakDuinoSerial package using NuGet Package Manager:

```Install-Package HakDuinoSerial```

Or using the .NET CLI:

```dotnet add package HakDuinoSerial```

## Usage

Here's a quick example of how to use the HakDuinoSerial library:

```csharp
using HakDuinoSerial;
using HakDuinoSerial.Enum;
using HakDuinoSerial.Service;

class Program
{
    static async Task Main(string[] args)
    {
        var serial = new HakDuinoSerial('COM3', 9600);

        if (serial.OpenConnection())
        {
            // Move the mouse to coordinates (100, 150)
            serial.MoveMouse(100, 150);
            
            // Perform a left click
            serial.MouseLeftClick();
            
            // Scroll up
            serial.ScrollWheel(HakDuinoEnumButton.Up, 10);
            
            // Flush commands if buffered commands exceed threshold
            await serial.FlushBufferedCommandsAsync(5);
            
            // Close the connection
            serial.CloseConnection();
        }
    }
}
```

## Arduino Sketch Example

To use the HakDuinoSerial library, you can upload the following sketch to your Arduino board (e.g., Arduino Leonardo):

```ino
#include <Mouse.h>

void setup() {
  Serial.begin(250000);
  Serial.println("Arduino Leonardo Ready");
}

void loop() {
  while (Serial.available()) {
    String receivedData = Serial.readStringUntil('\n');
    handleSerialInput(receivedData);
  }
}

void handleSerialInput(String data) {
  int firstComma = data.indexOf(',');
  String commandType = data.substring(0, firstComma);

  if (commandType == "M") {
    // Mouse movement command
    String xPosStr = data.substring(firstComma + 1, data.indexOf(',', firstComma + 1));
    String yPosStr = data.substring(data.indexOf(',', firstComma + 1) + 1);
    
    int xPos = xPosStr.toInt();
    int yPos = yPosStr.toInt();
    
    moveMouse(xPos, yPos);
    
  } else if (commandType == "C") {
    // Mouse click command
    String button = data.substring(firstComma + 1);
    clickMouse(button);
    
  } else if (commandType == "S") {
    // Scroll command
    String scrollDirection = data.substring(firstComma + 1, data.indexOf(',', firstComma + 1));
    String amountStr = data.substring(data.indexOf(',', firstComma + 1) + 1);
    int amount = amountStr.toInt();
    scrollMouse(scrollDirection, amount);
  }
}

void moveMouse(int x, int y) {
  Mouse.move(x, y);
  Serial.println("Mouse moved to: X=" + String(x) + " Y=" + String(y));
}

void clickMouse(String button) {
  if (button == "LEFT") {
    Mouse.press(MOUSE_LEFT);
    Mouse.release(MOUSE_LEFT);
    Serial.println("Left button clicked");
  } else if (button == "RIGHT") {
    Mouse.press(MOUSE_RIGHT);
    Mouse.release(MOUSE_RIGHT);
    Serial.println("Right button clicked");
  } else if (button == "MIDDLE") {
    Mouse.press(MOUSE_MIDDLE);
    Mouse.release(MOUSE_MIDDLE);
    Serial.println("Middle button clicked");
  }
}

void scrollMouse(String direction, int amount) {
  // Adjust scroll amount based on direction
  if (direction == "UP") {
    Mouse.move(0, 0, amount); // Scroll up
    Serial.println("Scrolled up by: " + String(amount));
  } else if (direction == "DOWN") {
    Mouse.move(0, 0, -amount); // Scroll down
    Serial.println("Scrolled down by: " + String(amount));
  }
}
```

## Functions

### OpenConnection

Opens a connection to the serial port for communication with the Arduino.

- **Returns**: True if the connection was successfully opened; otherwise, false.

### FlushBufferedCommandsAsync

Asynchronously sends all buffered commands to the Arduino if the buffer exceeds the specified threshold.

- **Parameters**: 
  - `threshold`: The minimum buffer length required to trigger the flush.
- **Returns**: True if commands were successfully sent; otherwise, false.

### MoveMouse

Sends a command to move the mouse to the specified X and Y coordinates.

- **Parameters**:
  - `x`: The target X coordinate for the mouse movement.
  - `y`: The target Y coordinate for the mouse movement.
- **Returns**: True if the command was successfully buffered; otherwise, false.

### ClickMouse

Sends a command to click the mouse using the specified button.

- **Parameters**:
  - `mouseButton`: The mouse button to click (left, right, etc.).
- **Returns**: True if the command was successfully buffered; otherwise, false.

### MouseRightClick

Sends a command to perform a right mouse click.

- **Returns**: True if the command was successfully buffered; otherwise, false.

### MouseLeftClick

Sends a command to perform a left mouse click.

- **Returns**: True if the command was successfully buffered; otherwise, false.

### ScrollWheel

Sends a command to scroll the mouse wheel.

- **Parameters**:
  - `direction`: The direction to scroll (up or down).
  - `amount`: The amount to scroll.
- **Returns**: True if the command was successfully buffered; otherwise, false.

### GetArduinoInfo

Retrieves the Arduino information such as name, VID, and PID.

- **Returns**: A string containing the Arduino information.

### GetSerialPort

Retrieves the SerialPort instance used for communication with the Arduino.

- **Returns**: The SerialPort instance associated with the Arduino connection.

### CloseConnection

Closes the connection to the serial port.

- **Returns**: True if the connection was successfully closed; otherwise, false.

## Contributing

If you'd like to contribute to this project, feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
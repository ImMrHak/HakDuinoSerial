# HakDuinoSerial

**HakDuinoSerial** is a C# library designed to facilitate communication with Arduino devices via serial connections. This library allows developers to send mouse commands and other data easily, streamlining the process of integrating Arduino with C# applications.

## Features

- **Simple Serial Communication**: Easily send and receive data from Arduino.
- **Mouse Command Integration**: Control mouse movements, clicks, and scrolling from your C# application.
- **Keyboard Control**: Basic keyboard input capabilities to interact with applications.
- **Cross-Platform Compatibility**: Works on Windows, Linux, and macOS.

## Installation

You can install the HakDuinoSerial package using NuGet Package Manager:

```
Install-Package HakDuinoSerial
```

Or using the .NET CLI:

```
dotnet add package HakDuinoSerial
```

## Usage

Here's a quick example of how to use the HakDuinoSerial library for mouse control:

```csharp
using HakDuinoSerial;
using HakDuinoSerial.Enum;
using HakDuinoSerial.Service;

class Program
{
    static async Task Main(string[] args)
    {
        var mouseController = new HakDuinoMouse("COM3", 9600);
        
        if (mouseController.OpenConnection())
        {
            // Move the mouse to coordinates (100, 150)
            mouseController.MoveMouse(100, 150);
            
            // Perform a left click
            mouseController.MouseLeftClick();
            
            // Scroll up
            mouseController.ScrollWheel(EHakDuinoMouseButton.SCROLL_UP, 10);
            
            // Flush commands if buffered commands exceed threshold
            await mouseController.FlushBufferedCommandsAsync(5);
            
            // Close the connection
            mouseController.CloseConnection();
        }
    }
}
```

### Arduino Sketch Example

To use the HakDuinoSerial library, upload the following sketch to your Arduino board (e.g., Arduino Leonardo):

```cpp
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
    // Handle clicks
    String buttonType = data.substring(firstComma + 1);
    if (buttonType == "L") {
      Mouse.click();
    } else if (buttonType == "R") {
      Mouse.click(MOUSE_RIGHT);
    }
  } else if (commandType == "S") {
    // Scroll command
    String scrollAmountStr = data.substring(firstComma + 1);
    int scrollAmount = scrollAmountStr.toInt();
    Mouse.scroll(scrollAmount);
  }
}

void moveMouse(int x, int y) {
  Mouse.move(x, y);
}
```

## Documentation

For detailed documentation, please refer to the [Wiki](#) (link to your documentation).

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
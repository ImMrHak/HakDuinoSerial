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
            // Initialize mouse and keyboard objects
            var hakDuinoMouse = new HakDuinoMouse("COM9", 250000);
            var hakDuinoKeyboard = new HakDuinoKeyboard("COM9", 250000);

            // Open connections
            if (hakDuinoMouse.OpenConnection())
                Console.WriteLine("Mouse connection opened successfully.");
            else
                Console.WriteLine("Failed to open mouse connection.");

            Console.WriteLine("Test Mouse Now After 5 sec.");
            Thread.Sleep(5000);
            // Mouse Test
            await TestMouseControl(hakDuinoMouse);


            // Close connections
            hakDuinoMouse.CloseConnection();

            if (hakDuinoKeyboard.OpenConnection())
                Console.WriteLine("Keyboard connection opened successfully.");
            else
                Console.WriteLine("Failed to open keyboard connection.");

            Console.WriteLine("Test Keybaord Now After 5 sec.");
            Thread.Sleep(5000);
            

            // Keyboard Test
            await TestKeyboardControl(hakDuinoKeyboard);

            hakDuinoKeyboard.CloseConnection();
        }

        static async Task TestMouseControl(HakDuinoMouse hakDuinoMouse)
        {
            Console.WriteLine("Starting mouse control test...");

            // Move the mouse in a loop
            for (int i = 0; i < 5; i++)
            {
                hakDuinoMouse.MoveMouse(100, 100);
                await hakDuinoMouse.FlushBufferedCommandsAsync();
                Console.WriteLine($"Mouse moved to (100, 100) - iteration {i + 1}");
                await Task.Delay(500); // Delay to simulate time between movements
            }

            // Click the left mouse button
            hakDuinoMouse.ClickMouse(EHakDuinoMouseButton.LEFT);
            await hakDuinoMouse.FlushBufferedCommandsAsync();
            Console.WriteLine("Left mouse button clicked.");

            // Scroll the mouse wheel
            hakDuinoMouse.ScrollWheel(EHakDuinoMouseButton.SCROLL_UP, 5);
            await hakDuinoMouse.FlushBufferedCommandsAsync();
            Console.WriteLine("Scrolled mouse wheel up.");
        }

        static async Task TestKeyboardControl(HakDuinoKeyboard hakDuinoKeyboard)
        {
            Console.WriteLine("Starting keyboard control test...");

            // Press and release a key
            hakDuinoKeyboard.PressKey(EHakDuinoKeyboardButton.A);
            await hakDuinoKeyboard.FlushBufferedCommandsAsync();
            Console.WriteLine($"Key {EHakDuinoKeyboardButton.A} pressed.");

            await Task.Delay(1000);

            hakDuinoKeyboard.ReleaseKey(EHakDuinoKeyboardButton.A);
            await hakDuinoKeyboard.FlushBufferedCommandsAsync();
            Console.WriteLine("Key 'A' released.");

            // Type a string
            string textToType = "Hello, HakDuino!";
            hakDuinoKeyboard.TypeText(textToType);
            await hakDuinoKeyboard.FlushBufferedCommandsAsync();
            Console.WriteLine($"Typed text: {textToType}");

            // Type a large block of text (Lorem Ipsum)
            string loremIpsum = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolest laborum.";

            Console.WriteLine("Typing Lorem Ipsum text...");
            hakDuinoKeyboard.TypeText(loremIpsum);
            await hakDuinoKeyboard.FlushBufferedCommandsAsync();
            Console.WriteLine("Typed Lorem Ipsum text.");
        }
}
```

### Arduino Sketch Example

To use the HakDuinoSerial library, upload the following sketch to your Arduino board (e.g., Arduino Leonardo):

```cpp
#include <Mouse.h>
#include <Keyboard.h>

void setup() {
  Serial.begin(250000);
  Serial.println("Arduino Leonardo Ready");
  Mouse.begin();
  Keyboard.begin();
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
      Serial.println("Left mouse clicked");
    } else if (buttonType == "R") {
      Mouse.click(MOUSE_RIGHT);
      Serial.println("Right mouse clicked");
    }
  } else if (commandType == "S") {
    // Scroll command
    String scrollAmountStr = data.substring(firstComma + 1);
    int scrollAmount = scrollAmountStr.toInt();
    Mouse.move(0, scrollAmount); // Move the mouse vertically
    Serial.print("Scrolled: ");
    Serial.println(scrollAmount);
  } else if (commandType == "K") {
    // Handle keyboard commands
    String action = data.substring(firstComma + 1, data.indexOf(',', firstComma + 1)); // Action (P or R)
    String keyData = data.substring(data.indexOf(',', firstComma + 1) + 1); // Key character

    if (action == "P") {
      pressKey(keyData);
    } else if (action == "R") {
      releaseKey(keyData);
    } else if (action == "T") {
      typeText(keyData);
    }
  }
}

void moveMouse(int x, int y) {
  Mouse.move(x, y);
  Serial.print("Mouse moved to: ");
  Serial.print(x);
  Serial.print(", ");
  Serial.println(y);
}

void pressKey(String key) {
    if (key.length() > 0) {
        char keyChar = key.charAt(0); // Get the first character of the key string
        Keyboard.press(keyChar); // Pass the character directly
        Serial.print("Pressed key: ");
        Serial.println(keyChar);
    } else {
        Serial.println("No key to press.");
    }
}

void releaseKey(String key) {
    if (key.length() > 0) {
        char keyChar = key.charAt(0); // Get the first character of the key string
        Keyboard.release(keyChar); // Pass the character directly
        Serial.print("Released key: ");
        Serial.println(keyChar);
    } else {
        Serial.println("No key to release.");
    }
}


void typeText(String text) {
  Keyboard.print(text);
  Serial.print("Typed text: ");
  Serial.println(text);
}
```

## Documentation

For detailed documentation, please refer to the [Wiki](#) (link to your documentation).

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
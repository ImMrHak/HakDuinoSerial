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

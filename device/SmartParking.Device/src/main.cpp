#include <Arduino.h>
#include "utils.h"
#include <PubSubClient.h>



// put function declarations here:
int myFunction(int, int);

void setup() {
  Serial.begin (115200);
  Utils::connectAWS();
  Utils::publishMessage(42);


  // put your setup code here, to run once:
  int result = myFunction(2, 3);
}

void loop() {
  Utils::updateAWS();
  // put your main code here, to run repeatedly:
}

// put function definitions here:
int myFunction(int x, int y) {
  return x + y;
}
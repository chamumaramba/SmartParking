#include <Arduino.h>
#include "utils.h"
#include <PubSubClient.h>



void setup() {
  Serial.begin (115200);
  Utils::connectAWS();
  Utils::publishMessage(42);

}

void loop() {
  Utils::updateAWS();
  // put your main code here, to run repeatedly:
}


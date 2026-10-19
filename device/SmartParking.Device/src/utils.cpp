#include "utils.h"
#include "certs.h"
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>
#include "WiFi.h"

WiFiClientSecure wifi = WiFiClientSecure();
PubSubClient client(wifi);

#define AWS_IOT_PUBLISH_TOPIC "esp32_Parking/pub"
#define AWS_IOT_SUBSCRIBE_TOPIC "esp32_Parking/sub"

void messageHandler(
    char* topic,
    byte* payload,
    unsigned int length
) {
    Serial.print("Incoming: ");
    Serial.println(topic);

    JsonDocument doc;
    const auto error = deserializeJson(doc, payload, length);

    if (error) {
        Serial.print("Invalid JSON: ");
        Serial.println(error.c_str());
        return;
    }

    const char* message = doc["message"] | "(no message field)";
    Serial.println(message);
}

void Utils::connectAWS(){
    WiFi.mode(WIFI_STA);
    WiFi.begin(WIFI_SSID, WIFI_PSWD);
    Serial.println("Connecting to wi-fi");

   while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(">");
}

    // Configure WiFiClientSecure to use the AWS IoT device credentials
    wifi.setCACert(AWS_CERT_CA);
    wifi.setCertificate(AWS_CERT_CRT);
    wifi.setPrivateKey(AWS_CERT_PRIVATE);

    //Connect to MQTT Broker using AWS endpoint
    client.setServer(AWS_IOT_ENDPOINT, 8883);

    // Create message handler
    client.setCallback(messageHandler);

    Serial.println("Connecting to AWS IoT");

    while (!client.connect(THINGNAME)){
        Serial.print(">");
        delay(1000);
    }

    if (!client.connected()){
        Serial.println("AWS IoT Timeout");
        return;
    }

    client.subscribe(AWS_IOT_SUBSCRIBE_TOPIC);
    Serial.println("AWS IoT Connected");
}

void Utils::publishMessage(int metricValue){
    JsonDocument doc;
    doc ["metrics"] = metricValue;

    char jsonBuffer[512];
    serializeJson(doc, jsonBuffer);

    client.publish(AWS_IOT_PUBLISH_TOPIC, jsonBuffer);
}



void Utils::updateAWS(){
    client.loop();
}



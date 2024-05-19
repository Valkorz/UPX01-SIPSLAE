#include <HTTPClient.h>
#include <WiFi.h>
#include <WiFiAP.h>
#include <WiFiClient.h>
#include <WiFiGeneric.h>
#include <WiFiMulti.h>
#include <WiFiSTA.h>
#include <WiFiScan.h>
#include <WiFiServer.h>
#include <WiFiType.h>
#include <WiFiUdp.h>

const char* ssid = "Tozeli";
const char* password = "69VvHz1$8k#a02b";
const char* get = "http://192.168.15.7:5000/monitor/get";
const char* post = "http://192.168.15.7:5000/monitor/post";

HTTPClient http;
#define UPDATE_DELAY_MS 5000

float analog;
int i = 4;

void setup() {
  WiFi.begin(ssid,password,6);
  while(WiFi.status() != WL_CONNECTED){
    //Wait for WiFi connection
    Serial.println("Connecting...");
    delay(100);
    Serial.print(".");
  }
  Serial.println("Connected!");


  Serial.begin(9600);
  pinMode(12, OUTPUT);
  pinMode(13, INPUT_PULLUP);

  http.begin(post);

  if(http.GET() > 0){
    Serial.println("Connection was a success.");
  }
  else{
    Serial.println("Connection failed.");
    http.end();
  }
  
}

void loop() {

  digitalWrite(12, LOW);
  delay(200);

  analog = map(analogRead(33), 0, 1500, 0.0, 10.0);
  analogWrite(12, map(analogRead(33), 0, 1500, 0, 255));

  Serial.println(analog);

  
  if(digitalRead(13) == HIGH){
    http.addHeader("Content-Type", "application/json");
    String postData = "{\"Id\":" + String(i) + ",\"WaterLevel\":" + String(analog) + "}";

    int httpResponseCode = http.POST(postData);
    if(httpResponseCode > 0){
        Serial.println(http.getString());
    } else{
        Serial.println("Error: ");
        Serial.print(httpResponseCode);
    }
  }

  i++;
  delay(200);
}

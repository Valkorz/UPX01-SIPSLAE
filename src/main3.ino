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
const char* url = ""http://192.168.15.7:5000/";
const char* get = "monitor/get";
const char* post = "monitor/post";
const char* getFrequency = "monitor/frequency";
const char* getAverage = "monitor/average";
const char* fromLast = "?fromLast=20";

HTTPClient http_post;
HTTPClient http_get;
HTTPClient http_getFrequency;
HttpClient http_getAverage;

double frequency, average;
#define UPDATE_DELAY_MS 5000
#define TRESHOLD 7.0

float analog;

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

  //Start http
  http_post.begin(url + post);
  http_get.begin(url + get);
  http_getFrequency.begin(url + getFrequency);
  http_getAverage.begin(url + getAverage + fromLast);
}

void loop() {

  frequency = http_getFrequency.GET();
  average = http_getAverage.GET();
  
  digitalWrite(12, LOW);
  delay(200);

  analog = map(analogRead(33), 0, 1500, 0.0, 10.0);
  analogWrite(12, map(analogRead(33), 0, 1500, 0, 255));

  Serial.println(analog);

  
  if(digitalRead(13) == HIGH && analog >= TRESHOLD || analog != average){

    //Switch to post
    http.end();
    http.begin(http_post);
    if(http.GET() > 0){
      http.addHeader("Content-Type", "application/json");
      String postData = "{"\"WaterLevel\":" + String(analog) + "}";

      int httpResponseCode = http.POST(postData);
      if(httpResponseCode > 0){
          Serial.println(http.getString());
      } else{
          Serial.println("Error: ");
          Serial.print(httpResponseCode);
      }
    }   
  }

  delay(UPDATE_DELAY_MS);
}


#include <Wire.h>
#include <LiquidCrystal_I2C.h>


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
const String url = "http://192.168.15.3:5000/";
const String get = "monitor/get";
const String post = "monitor/post";
const String getTreshold = "monitor/treshold";
//const char* getFrequency = "monitor/frequency";
//const char* getAverage = "monitor/average";
//const char* fromLast = "?fromLast=20";

HTTPClient http_post;
HTTPClient http_treshold;
//HTTPClient http_get;
//HTTPClient http_getFrequency;
//HttpClient http_getAverage;

double frequency, average;
#define UPDATE_DELAY_MS 2000
#define WAIT_TIME_SECONDS 5

float analog;
float lastAnalog;
int treshold = 9;

bool isConnected = false;

//Add display at address 0x27
LiquidCrystal_I2C lcd(0x27,16,2);

void setup() {
  lcd.begin(12, 14); // initialize the lcd 
  lcd.backlight();
  lcd.setCursor(0,0);
  lcd.print("Inicializando...");
  delay(1000);
  
  WiFi.begin(ssid,password,6);
  for(int i = 0; i < WAIT_TIME_SECONDS || WiFi.status() != WL_CONNECTED; i++){
    //Wait for WiFi connection
    lcd.clear();
    lcd.print("Conectando...");
    delay(1000);
  }

  if(WiFi.status() != WL_CONNECTED){
    isConnected = false;
    Serial.println("Network could not be reached. Data transfer may not occur.");
    lcd.clear();
    lcd.print("Erro:");
    lcd.setCursor(0,1);
    lcd.print("Sem internet.");

  } else {
     isConnected = true;
     Serial.println("Connected successfully.");
     lcd.clear();
     lcd.print("Conectado.");
  }


  Serial.begin(9600);
  pinMode(13, OUTPUT);
  //pinMode(13, INPUT_PULLUP);

  if(isConnected){
    //Start http
    http_post.begin(url + post);
    http_treshold.begin(url + getTreshold);
    //http_get.begin(url + get);
    //http_getFrequency.begin(url + getFrequency);
    //http_getAverage.begin(url + getAverage + fromLast);

    if(http_post.GET() == -1) isConnected = false;
  }

  delay(100);
  lcd.clear();
  
}

void loop() {
  
  if(isConnected){
    if(http_treshold.GET() == 200){
    int received = http_treshold.getString().toInt();
    if(received != treshold){
        treshold = received;
      }
    }
  }
  /*
  frequency = http_getFrequency.GET();
  average = http_getAverage.GET();

  Serial.println("Average: ");
  Serial.print(average);

  Serial.println("Frequency: ");
  Serial.print(frequency); */


  analog = map(analogRead(33), 0, 1500, 0.0, 10.0);
  //analogWrite(12, map(analogRead(33), 0, 1500, 0, 255));

  Serial.println("Reading: ");
  Serial.print(analog);

  lcd.setCursor(0,0);
  lcd.print("Nível atual: ");
  lcd.setCursor(0,1);
  lcd.print(analog);
  lcd.setCursor(5,1);
  lcd.print("u");

  if(analog != lastAnalog){
    if(isConnected){
      Serial.println("httpost get:");
      Serial.print(http_post.GET());
      if(http_post.GET() > 0){
        http_post.addHeader("Content-Type", "application/json");
        String postData = "{\"WaterLevel\":" + String(analog) + "}";

        int httpResponseCode = http_post.POST(postData);
        Serial.println("Response code: ");
        Serial.print(httpResponseCode);
        if(httpResponseCode > 0){
            Serial.println(http_post.getString());
        } else{
            Serial.println("Error: ");
            Serial.print(httpResponseCode);
        }
    }   
    else{
        Serial.println("Error: ");
        Serial.print(http_post.GET());
      }
    }

    if(analog >= treshold){
      digitalWrite(13, HIGH);
    }
    else digitalWrite(13, LOW);
  }

  lastAnalog = analog;
  delay(UPDATE_DELAY_MS);
  //lcd.clear();
}



#include <LiquidCrystal_I2C.h>
LiquidCrystal_I2C lcd(0x20,16,2); 

//Container information
int width, height, length;

//Data
int sens_read = A3, sens_vcc = 6, buzzer = 5;

long treshold = 45;
float level, recordedLevel;
int currentTime, nextTarget;
bool update = true;
float speed = 0;

float currentReading, lastReading;

//Settings
int delayTime = 5000;


void setup()
{
  pinMode(buzzer, OUTPUT);
  pinMode(sens_vcc, OUTPUT);
  Serial.begin(9600);
  
  lcd.init();
  lcd.clear();         
  lcd.backlight();
  
  lcd.setCursor(0,0);
  lcd.print("Initializing...");
  delay(50);
  lcd.clear();
  
  nextTarget = delayTime;
}

void loop()
{
  delay(200);
  
  //Run update routine
  currentTime = millis();
  if(update){
    lastReading = currentReading;
    currentReading = level;
    speed = (currentReading - lastReading) / (delayTime / 1000);
    Serial.println("Rise speed: ");
    Serial.print(speed);
    Serial.print(" cm/s.");
    update = false;
  }
  
  
  lcd.clear();

  digitalWrite(sens_vcc, HIGH);
  delay(10);
  
  //Get level in centimeters
  level = map(analogRead(sens_read), 0, 872, 0, 50);
  delay(10);
  digitalWrite(sens_vcc, LOW);
  
  
  Serial.println(level);
  
  if(level >= treshold){
    
    analogWrite(buzzer, 255);
    lcd.setCursor(0,0);
    lcd.print("ALERTA! Nivel alto: ");
    lcd.setCursor(0,1);
    lcd.print(level / 10);
    lcd.print(" cm");
    delay(300);
    analogWrite(buzzer,10);
    delay(300);
    
  }
  else{ 
    
    lcd.setCursor(0,0);
    lcd.print("Nivel normal: ");
    lcd.setCursor(0,1);
    lcd.print(level / 10);
    lcd.print(" cm");
    digitalWrite(buzzer, LOW);
    
  }
  
  if(currentTime >= nextTarget){
    update = true;
    nextTarget += delayTime;
  }
}

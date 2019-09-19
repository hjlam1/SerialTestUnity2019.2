int buttonPIN = 3;
int analogPINa = A0;
int analogPINb = A1;

void setup() {
  Serial.begin(115200);
  pinMode(buttonPIN, INPUT_PULLUP);
}

void loop() {
  Serial.print(digitalRead(buttonPIN));
  Serial.print(";");
  Serial.print(analogRead(analogPINa));
  Serial.print(";");
  Serial.println(analogRead(analogPINb));
}

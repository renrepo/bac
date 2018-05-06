#include <analogShield.h>
 
double n = 1024;
unsigned int datar0;
unsigned int datar1;
unsigned int datar2;
unsigned int datar3;
long val0 = 0;
long val1 = 0;
long val2 = 0;
long val3 = 0;
void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(115200);
  //port = new Serial(this, portName, 115200);
}




  
// the loop routine runs over and over again forever:
void loop() {
   if(Serial.available())  
  {  
 char  data=Serial.read();  
  switch(data)  
  {  
    case 'O': 
    {
      unsigned int data0 = 65000;
      unsigned int data1 = 65000;
      unsigned int data2 = 65000;
      unsigned int data3 = 65000;
      analog.write(data0,data1,data2,data3,true);
      break; 
    } 
    case '3':{
      int n = 1024;
      unsigned int datar3;
      double val = 0;
      for (int i=0; i <= n; i++){
        datar3 = analog.read(3);
        val += datar3;
      }
      Serial.println(val/n);
      val = 0;
      //Serial.println(datar0, datar1, datar2, datar3);
      break; 
    }
    case '2': {
      int n = 1024;
      unsigned int datar2;
      double val = 0;
      for (int i=0; i <= n; i++){
        datar2 = analog.read(2);
        val += datar2;
      }    
      Serial.println(val/n);
      val = 0;
      //Serial.println(datar0, datar1, datar2, datar3);
      break; 
    }
      break;
    case '1': {
      int n = 1024;
      unsigned int datar1;
      double val = 0;
      for (int i=0; i <= n; i++){
        datar1 = analog.read(1);
        val += datar1;
      }
      Serial.println(val/n);
      val = 0;
      //Serial.println(datar0, datar1, datar2, datar3);
      break; 
    }
    case '0': {
      int n = 1024;
      unsigned int datar0;
      double val = 0;
      for (int i=0; i <= n; i++){
      datar0 = analog.read(0);
      val += datar0;
      }
      double values[2] = {val/n, 1000000};
      int i;   
      for (i = 0; i < 2; i = i + 1) {
      Serial.println(values[i]);
      }
      val = 0;
      break; 
    }  


    case 'a': {   
      for (int i=0; i < n; i++){
      val0 += analog.read(0);
      val1 += analog.read(1);
      val2 += analog.read(2);
      val3 += analog.read(3);
      }
      Serial.println(val0);
      Serial.println(val1);
      Serial.println(val2);
      Serial.println(val3);
      val0 = 0;
      val1 = 0;
      val2 = 0;
      val3 = 0;
      break; 
    }
  }  
  }  
}

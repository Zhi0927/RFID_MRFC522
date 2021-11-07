/*
 * The program must import the library "MFRC522(SPI)-Version1.4.9" 
 * developed by Miki Balboa to control the Mifare module. 
 * More Info: https://github.com/miguelbalboa/rfid
 * 
 * How to download the library:
 * Sketech -> Include Library -> Manage Libraries... 
 * -> type "MFRC522 in search box"
 * 
 * The Arduino SPI Fixed Pins:
 * MISO -> Pin12
 * MOSI -> Pin11
 * SCK  -> Pin13
 * SDA  -> Pin10(option)
 * RST  -> PinA0(option)
 * 
 * MFRC522 API:
 * MFRC522::PCD_Init(): Initialize the MFRC522 reader module.
 * MFRC522::PICC_IsNewCardPresent(): Whether a new tag is detected or not.
 * MFRC522::PICC_ReadCardSerial(): Read tag data.
 * MFRC522::PICC_GetType(): Get the tag type.
 * MFRC522::PICC_GetTypeName(): Get the tag type name.
  */

#include <SPI.h>
#include <MFRC522.h>
#define RST_PIN         A0   // RESET    
#define SS_PIN          10  // SDA

MFRC522 mfrc522;  

struct RFIDtag{
  byte uid[4];
  char *Name;
};
struct RFIDtag tags[]{
  {{169,181,236,151},"Card"},
  {{138,120,131,37},"Circle"}
};
byte taglengh = sizeof(tags);


//================================= Setup ==================================//
void setup() {
  Serial.begin(9600); 
  SPI.begin();        // Initialize the SPI interface
  mfrc522.PCD_Init(SS_PIN, RST_PIN); // Initialize the MFRC522
}


//================================== Loop =================================//
void loop() {
  Main(false);
  //DetectTagRecord();
}


//=============================== Main Fuction01 ==========================//
void Main(bool verbose){
  // Check if it is a new card
  if (mfrc522.PICC_IsNewCardPresent() && mfrc522.PICC_ReadCardSerial()) {
    // print tag content
    if(verbose){
        Serial.print(F("Reader "));
        Serial.print(F(": "));
        mfrc522.PCD_DumpVersionToSerial(); // Display the version of the reader
        Serial.print(F("Card UID:"));
    }
    dump_byte_array(mfrc522.uid.uidByte, mfrc522.uid.size); // print the UID of the tag
    Serial.println();
  
    if(verbose){
      Serial.print(F("PICC type: "));
      MFRC522::PICC_Type piccType = mfrc522.PICC_GetType(mfrc522.uid.sak);
      Serial.println(mfrc522.PICC_GetTypeName(piccType));  //print tag type
    }
    mfrc522.PICC_HaltA();  // Enter stop mode  
  }
}



//================================ function ===============================//
void dump_byte_array(byte *buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
//    Serial.print(buffer[i] < 0x10 ? " 0" : " ");
//    Serial.print(buffer[i], HEX);
    Serial.print(buffer[i]);
  }
}


//=============================== Main Fuction02 ==========================//
void DetectTagRecord(){
  if (mfrc522.PICC_IsNewCardPresent() && mfrc522.PICC_ReadCardSerial()) {
    byte *id = mfrc522.uid.uidByte;  
    byte idSize = mfrc522.uid.size;
    bool tag = false;
    for (byte i = 0; i < taglengh; i++) {
        if(memcmp(tags[i].uid, id , idSize) == 0){
          tag = true;
          break;
        }
    }
    if(tag){
      Serial.println(F("Access Granted!"));
    }else{
      Serial.println(F("Access Denied!"));
    } 
    mfrc522.PICC_HaltA();
  }   
}

/*
 * The program must import the library "MFRC522(SPI)-Version1.4.9" 
 * developed by Miki Balboa to control the Mifare module. 
 * More Info: https://github.com/miguelbalboa/rfid
 * 
 * How to download the library:
 * Sketech -> Include Library -> Manage Libraries... 
 * -> type "MFRC522" in search box
 * 
 * The Arduino SPI Fixed Pins:
 * RST  -> PinA0(optional)
 * IRQ  -> -
 * MISO -> Pin12
 * MOSI -> Pin11
 * SCK  -> Pin13
 * SDA  -> Pin10(optional)
 * 
 * MFRC522 API:
 * MFRC522::PCD_Init(): Initialize the MFRC522 reader module.
 * MFRC522::PICC_IsNewCardPresent(): Whether a new tag is detected or not.
 * MFRC522::PICC_ReadCardSerial(): Read tag data.
 * MFRC522::PICC_GetType(): Get the tag type.
 * MFRC522::PICC_GetTypeName(): Get the tag type name.
 * 
 * The unsigned char datatype is the same as the byte datatype in Arduino,
 * see details: https://www.arduino.cc/reference/en/language/variables/data-types/unsignedchar/
 */


#include <SPI.h>
#include <MFRC522.h>

//================================ Fields ================================//
#define RST_PIN  A0                                                           // RESET    
#define SS_PIN   10                                                           // SDA
MFRC522 mfrc522;                                                              // Init Module

//================================ Setup =================================//
void setup() {
  Serial.begin(9600); 
  SPI.begin();                                                                // Initialize the SPI interface
  mfrc522.PCD_Init(SS_PIN, RST_PIN);                                          // Initialize the MFRC522
}


//================================= Loop =================================//
void loop() {
  MainMode01(false);
  //MainMode02();
}



//============================== Main Mode01 =============================//
void MainMode01(bool verbose){
                                                                              
  if (mfrc522.PICC_IsNewCardPresent() && mfrc522.PICC_ReadCardSerial()) {     // Check if it is a new card
    
    if(verbose){                                                              // print tag content
      Serial.print(F("Reader "));
      Serial.print(F(": "));
      mfrc522.PCD_DumpVersionToSerial();                                      // Display the version of the reader
      Serial.print(F("Card UID:"));
    }
    
    dump_byte_array(mfrc522.uid.uidByte, mfrc522.uid.size);                   
    for (unsigned char i = 0; i < bufferSize; i++) {                          // print the UID of the tag
      //Serial.print(buffer[i] < 0x10 ? " 0" : " ");
      //Serial.print(buffer[i], HEX);
      Serial.print(buffer[i]);
    }
    Serial.println();
  
    if(verbose){
      Serial.print(F("PICC type: "));
      MFRC522::PICC_Type piccType = mfrc522.PICC_GetType(mfrc522.uid.sak);
      Serial.println(mfrc522.PICC_GetTypeName(piccType));                     //print tag type
    }
    
    mfrc522.PICC_HaltA();                                                     // Enter pause status  
  }
}



//============================== Main Mode02 =============================//

//Declare a structure
struct RFIDtag{
  unsigned char uid[4];
  char *Name;
};

//Structure Init
RFIDtag tags[]{
  {{169,181,236,151}, "Card"},
  {{138,120,131,37} , "Circle"}
};

unsigned char taglength = sizeof(tags) / sizeof(RFIDtag);                     // Get the tag length


void MainMode02(){
  if (mfrc522.PICC_IsNewCardPresent() && mfrc522.PICC_ReadCardSerial()) {
    unsigned char* id     = mfrc522.uid.uidByte;  
    unsigned char  idSize = mfrc522.uid.size;
    
    bool tag = false;   
    for (unsigned char i = 0; i < taglength; i++) {                           // Check the legality of the tag.
        if(memcmp(tags[i].uid, id , idSize) == 0){
          tag = true;
          break;
        }
    }
    
    if(tag){                                                                  // Print the result.
      Serial.println(F("Access Granted!"));
    }else{
      Serial.println(F("Access Denied!"));
    } 
    
    mfrc522.PICC_HaltA();                                                     // Enter pause status  
  }   
}

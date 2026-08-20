#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <PubSubClient.h>
#include <HTTPClient.h>

//*****Constantes******//

LiquidCrystal_I2C lcd(0x27, 20, 4);

const char* routerName = "MOVISTAR-Redalberto";
const char* routerPassword = "AjhkXV6hrFqb7yxqkxTU";

const char* mqttServer = "iotsuper.duckdns.org";
const int mqttPort = 8883;

const char* usuarioMqtt = "iotsuper";
const char* contrasenaMqtt = "iotsupermqtt";


const int gpioEcho = 26;
const int gpioTrig = 27;

//************************//

const char* cert = R"(
-----BEGIN CERTIFICATE-----
MIIFazCCA1OgAwIBAgIRAIIQz7DSQONZRGPgu2OCiwAwDQYJKoZIhvcNAQELBQAw
TzELMAkGA1UEBhMCVVMxKTAnBgNVBAoTIEludGVybmV0IFNlY3VyaXR5IFJlc2Vh
cmNoIEdyb3VwMRUwEwYDVQQDEwxJU1JHIFJvb3QgWDEwHhcNMTUwNjA0MTEwNDM4
WhcNMzUwNjA0MTEwNDM4WjBPMQswCQYDVQQGEwJVUzEpMCcGA1UEChMgSW50ZXJu
ZXQgU2VjdXJpdHkgUmVzZWFyY2ggR3JvdXAxFTATBgNVBAMTDElTUkcgUm9vdCBY
MTCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAK3oJHP0FDfzm54rVygc
h77ct984kIxuPOZXoHj3dcKi/vVqbvYATyjb3miGbESTtrFj/RQSa78f0uoxmyF+
0TM8ukj13Xnfs7j/EvEhmkvBioZxaUpmZmyPfjxwv60pIgbz5MDmgK7iS4+3mX6U
A5/TR5d8mUgjU+g4rk8Kb4Mu0UlXjIB0ttov0DiNewNwIRt18jA8+o+u3dpjq+sW
T8KOEUt+zwvo/7V3LvSye0rgTBIlDHCNAymg4VMk7BPZ7hm/ELNKjD+Jo2FR3qyH
B5T0Y3HsLuJvW5iB4YlcNHlsdu87kGJ55tukmi8mxdAQ4Q7e2RCOFvu396j3x+UC
B5iPNgiV5+I3lg02dZ77DnKxHZu8A/lJBdiB3QW0KtZB6awBdpUKD9jf1b0SHzUv
KBds0pjBqAlkd25HN7rOrFleaJ1/ctaJxQZBKT5ZPt0m9STJEadao0xAH0ahmbWn
OlFuhjuefXKnEgV4We0+UXgVCwOPjdAvBbI+e0ocS3MFEvzG6uBQE3xDk3SzynTn
jh8BCNAw1FtxNrQHusEwMFxIt4I7mKZ9YIqioymCzLq9gwQbooMDQaHWBfEbwrbw
qHyGO0aoSCqI3Haadr8faqU9GY/rOPNk3sgrDQoo//fb4hVC1CLQJ13hef4Y53CI
rU7m2Ys6xt0nUW7/vGT1M0NPAgMBAAGjQjBAMA4GA1UdDwEB/wQEAwIBBjAPBgNV
HRMBAf8EBTADAQH/MB0GA1UdDgQWBBR5tFnme7bl5AFzgAiIyBpY9umbbjANBgkq
hkiG9w0BAQsFAAOCAgEAVR9YqbyyqFDQDLHYGmkgJykIrGF1XIpu+ILlaS/V9lZL
ubhzEFnTIZd+50xx+7LSYK05qAvqFyFWhfFQDlnrzuBZ6brJFe+GnY+EgPbk6ZGQ
3BebYhtF8GaV0nxvwuo77x/Py9auJ/GpsMiu/X1+mvoiBOv/2X/qkSsisRcOj/KK
NFtY2PwByVS5uCbMiogziUwthDyC3+6WVwW6LLv3xLfHTjuCvjHIInNzktHCgKQ5
ORAzI4JMPJ+GslWYHb4phowim57iaztXOoJwTdwJx4nLCgdNbOhdjsnvzqvHu7Ur
TkXWStAmzOVyyghqpZXjFaH3pO3JLF+l+/+sKAIuvtd7u+Nxe5AW0wdeRlN8NwdC
jNPElpzVmbUq4JUagEiuTDkHzsxHpFKVK7q4+63SM1N95R1NbdWhscdCb+ZAJzVc
oyi3B43njTOQ5yOf+1CceWxG1bQVs5ZufpsMljq4Ui0/1lvh+wjChP4kqKOJ2qxq
4RgqsahDYVvTH9w7jXbyLeiNdd8XM2w9U/t7y0Ff/9yi0GE44Za4rF2LN9d11TPA
mRGunUHBcnWEvgJBQl9nJEiU0Zsnvgc/ubhPgXRR4Xq37Z0j4r7g1SgEEzwxA57d
emyPxgcYxn/eR44/KJ4EBs+lVDR3veyJm+kXQ99b21/+jh5Xos1AnX5iItreGCc=
-----END CERTIFICATE-----
)";

//************************//

//*****Variables no constantes******//

int enviarPing = 0;
int reactivarUltraSonido = 0;

float tiempo = 0.0;
float distancia = 0.0;

String chipIDStr;
String topic;

WiFiClientSecure espClient;
PubSubClient client(espClient);

HTTPClient httpPost;
TaskHandle_t TareaSensor;

//************************//

void iniciarWifi()
{
  WiFi.begin(routerName, routerPassword);

  while(WiFi.status() != WL_CONNECTED)
  {
    lcd.setCursor(0,0);
    lcd.print("Problema wifi!");
  }

  lcd.setCursor(0,0);
  lcd.print("Conectado wifi!");
}

void iniciarMqtt()
{
  espClient.setCACert(cert);

  client.setClient(espClient);
  client.setServer(mqttServer, mqttPort);

  while(!client.connected())
  {
    Serial.print("Conectando al servidor mqtt...");

    if(client.connect(chipIDStr.c_str(), usuarioMqtt, contrasenaMqtt))
    {
      Serial.println("Conectado!");
    }
    else
    {
      Serial.printf("Error: %d\n", client.state());
      delay(2000);
    }
  }
  

  lcd.setCursor(0,1);
  lcd.print("MQTT ok!");
}

void callback(char* topic, byte* payload, unsigned int length) 
{
  char fila1[21], fila2[21], fila3[21], fila4[21];
  int j = 0;
  int i, end;

  end = 0;
  for (i = 0; i < 20 && j < length && (char)payload[j] != '|'; i++, j++)
  {
    fila1[i] = (char)payload[j];
    end = i + 1;
  }
  fila1[end] = '\0';
  if (j < length) j++;

  end = 0;
  for (i = 0; i < 20 && j < length && (char)payload[j] != '|'; i++, j++)
  {
    fila2[i] = (char)payload[j];
    end = i + 1;
  }
  fila2[end] = '\0';
  if (j < length) j++;

  end = 0;
  for (i = 0; i < 20 && j < length && (char)payload[j] != '|'; i++, j++)
  {
    fila3[i] = (char)payload[j];
    end = i + 1;
  }
  fila3[end] = '\0';
  if (j < length) j++;

  end = 0;
  for (i = 0; i < 20 && j < length && (char)payload[j] != '|'; i++, j++)
  {
    fila4[i] = (char)payload[j];
    end = i + 1;
  }
  fila4[end] = '\0';

  lcd.clear();

  lcd.setCursor(0, 0);
  lcd.print(fila1);

  lcd.setCursor(0, 1);
  lcd.print(fila2);

  lcd.setCursor(0, 2);
  lcd.print(fila3);

  lcd.setCursor(0, 3);
  lcd.print(fila4);
}

void hacerPing(const String& payload)
{
  String fullTopic = topic;
  client.publish(fullTopic.c_str(), payload.c_str());
}

void actualizarEstadistica()
{
   
  httpPost.POST("");
  httpPost.end();
}

void miSensorTask(void *pvParameters)//puntero generico para pasar datos
{
  while(true)
  {
    digitalWrite(gpioTrig, HIGH);
    delay(10);
    digitalWrite(gpioTrig, LOW);

    tiempo = pulseIn(gpioEcho, HIGH, 12000);
    distancia = (tiempo * 0.034) / 2; // en centimetros

    if(distancia <= 15 && distancia != 0.00 && reactivarUltraSonido >= 60)
    {
      Serial.println(distancia);
      Serial.println(reactivarUltraSonido);
      hacerPing("VISTO");
      reactivarUltraSonido = 0;
    }

    reactivarUltraSonido++;

    delay(100);
  }
}

void setup() 
{
  Serial.begin(57600);
  Serial.println("Starting...");

  uint64_t chipID = ESP.getEfuseMac();
  chipIDStr =  String((uint32_t)(chipID >> 32), HEX) + String((uint32_t)chipID, HEX);
  topic = "IoTSuper/LCD/" + chipIDStr;

  lcd.init();       
  lcd.backlight();

  lcd.setCursor(3,3);
  lcd.print(chipIDStr);

  iniciarWifi();
  iniciarMqtt();

  client.setCallback(callback);
  String topicOutput = topic + "/OUTPUT";
  client.subscribe(topicOutput.c_str());

  pinMode(gpioEcho, INPUT);
  pinMode(gpioTrig, OUTPUT);

  xTaskCreatePinnedToCore(miSensorTask, "detector", 10000, NULL, 1, &TareaSensor, 1);//funcion, nombre, memoria para la hebra (10000 es el maximo), variables = ninguna, prioridad, manejador, nucleo del procesador donde se va a ejecutar
}

void loop() 
{
  if(enviarPing == 3000)
  {
    hacerPing("PING");
    enviarPing = 0;
  }

  if (!client.connected()) 
  {
    iniciarMqtt();
  }

  client.loop();

  enviarPing++;
  
  delay(10);
}

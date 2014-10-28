using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using MicroLiquidCrystal;

namespace Diagnostics
{
    public class Program
    {

        private static Shifter74Hc595LcdTransferProvider shifter;

        // create the LCD interface
        private static Lcd lcd;
            
            private static MakerFaire2013.SensorMultiplexer oSensorMultiplexer = new MakerFaire2013.SensorMultiplexer();
                //The output of the multiplexer comes out on GPIO_PIN_A1, set it up as an input
            private static AnalogInput ReflectiveSensor = new AnalogInput(AnalogChannels.ANALOG_PIN_A5);
            private static OutputPort dummy1 = new OutputPort(Pins.GPIO_PIN_A1,  true);
            private static OutputPort dummy2 = new OutputPort(Pins.GPIO_PIN_A2, true);
            private static OutputPort dummy3 = new OutputPort(Pins.GPIO_PIN_A3, true);
            private static OutputPort dummy4 = new OutputPort(Pins.GPIO_PIN_A4, true);

            private static AnalogInput BackstopSensor = new AnalogInput(AnalogChannels.ANALOG_PIN_A0);

            public static int AverageElements(int[] arr, int size)
            {
                int sum = 0;
                int average = 0;

                for (int i = 0; i < size; i++)
                {
                    sum += arr[i];
                }

                average = sum / size; // sum divided by total elements in array
                return average;

            }

        public static void Main()
        {
          
            shifter = new Shifter74Hc595LcdTransferProvider( SPI_Devices.SPI1, Pins.GPIO_PIN_D6,
               Shifter74Hc595LcdTransferProvider.BitOrder.MSBFirst);

            // Create new LCD instance and use shift register as a transport layer
            lcd = new Lcd(shifter);

            // set up the LCD's number of columns and rows: 
            lcd.Begin(16, 2);
                    
            
                    

         
            lcd.Clear();
            lcd.Write("Loading");
            lcd.Clear();
            
        do
        {
            
        
            for (byte i = 0; i < 8; i++)
            {
                oSensorMultiplexer.selectDigit(i);
                System.Threading.Thread.Sleep(100);
                
                int[] SensorReadings = new int[5];
                for (int arrayindex = 0; arrayindex < 4; arrayindex++)
                {
                    SensorReadings[arrayindex] = ReflectiveSensor.ReadRaw();
                    System.Threading.Thread.Sleep(2);
                }

                int CurrentSensorReading = AverageElements(SensorReadings, 5);      

                if (i > 3)
                {
                    lcd.SetCursorPosition( (int)((i-4) * 4),1);
                }
                else { lcd.SetCursorPosition((int)(i * 4),0); }
                
                string stringtowrite;
                stringtowrite = CurrentSensorReading.ToString();
                int stringlength =stringtowrite.Length;

                for (int strindex = 0; strindex < (4-stringlength); strindex++)
                {
                    stringtowrite = " "+stringtowrite;
                }

                lcd.Write( stringtowrite);
                System.Threading.Thread.Sleep(50);
                Debug.Print(CurrentSensorReading.ToString());
            }
            Debug.Print("Back" + BackstopSensor.ReadRaw().ToString());
        } while (true);
        }
    }
}

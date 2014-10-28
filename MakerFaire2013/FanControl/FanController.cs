using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace MakerFaire2013.FanControl
{
    class FanController
    {
        private OutputPort FanPort;

        public FanController(ref OutputPort FanPort)
        {
            this.FanPort = FanPort;
        }

        public void InflateMan(){
            // Pause to allow other sequences to run first
            System.Threading.Thread.Sleep(3000);
            FanPort.Write(true);
            System.Threading.Thread.Sleep(30000);
            FanPort.Write(false);
        }

        

    }
}

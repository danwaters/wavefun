using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WaveGeneratorWP7
{
    public class OscillatorData
    {
        private short[] sampleData;
        private Oscillator oscillator;

        public Oscillator Oscillator
        {
            get
            {
                return oscillator;
            }
        }

        public short[] SampleData
        {
            get
            {
                return sampleData;
            }
        }

        public OscillatorData(Oscillator osc, short[] data)
        {
            oscillator = osc;
            sampleData = data;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace WaveGeneratorWP7
{
	public partial class Oscillator : UserControl
	{
        private const int MAX_AMPLITUDE = 32760;
        private const int INIT_FREQ = 100;

        private int amplitude = MAX_AMPLITUDE;
        private int frequency = INIT_FREQ;
        private WaveType waveType = WaveType.None;

        public int Amplitude
        {
            get
            {
                return amplitude;
            }
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }
        }

        public WaveType WaveType
        {
            get
            {
                return waveType;
            }
        }

		public Oscillator()
		{			
			InitializeComponent();
            rbSine.GroupName =
                rbNoise.GroupName =
                rbNone.GroupName =
                rbSaw.GroupName =
                rbSquare.GroupName = "WaveType_" + DateTime.Now.Millisecond;
		}

		private void rbSine_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            lblWaveType.Text = "Sine";
            this.waveType = WaveType.Sine;
		}

		private void rbNone_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            lblWaveType.Text = "None";
            this.waveType = WaveType.None;
		}

		private void rbSquare_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            lblWaveType.Text = "Square";
            this.waveType = WaveType.Square;
		}

		private void rbSaw_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            lblWaveType.Text = "Sawtooth";
            this.waveType = WaveType.Sawtooth;
		}

		private void rbNoise_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            lblWaveType.Text = "White Noise";
            this.waveType = WaveType.Noise;
		}

		private void sldFrequency_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
		{
            if (lblFreq != null)
            {
                lblFreq.Text = Convert.ToString((int)e.NewValue + "Hz");
                frequency = (int)e.NewValue;
            }
		}

		private void sldVolume_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
		{
            if (lblVolume1 != null)
            {
                lblVolume1.Text = Convert.ToString(Math.Round(sldVolume.Value / MAX_AMPLITUDE * 100, 0)) + "%";
                this.amplitude = (int)(sldVolume.Value);
            }
		}
	}
}
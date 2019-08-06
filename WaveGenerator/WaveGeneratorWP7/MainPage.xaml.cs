using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Audio;

namespace WaveGeneratorWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        DynamicSoundEffectInstance sound;        
        Mixer mixer;
        
        const int MAX_AMPLITUDE = 32760;
        const int NUM_GENERATORS = 3;

        byte[] emptyBuffer;

        bool isPlaying = false;

        public MainPage()
        {
            InitializeComponent();
            mixer = new Mixer();

            emptyBuffer = new byte[1024];

            sound = new DynamicSoundEffectInstance(48000, AudioChannels.Stereo);
            sound.SubmitBuffer(emptyBuffer);
            sound.SubmitBuffer(emptyBuffer);
            sound.BufferNeeded += new EventHandler<EventArgs>(sound_BufferNeeded);
            sound.Play();
        }

        void sound_BufferNeeded(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                sound.SubmitBuffer(emptyBuffer);
            }
            else
            {                
                short[] data = mixer.MixToStream();
                byte[] buffer = new byte[data.Length * sizeof(short)];
                for (int i = 0; i < data.Length; i ++)
                {
                    BitConverter.GetBytes(data[i]).CopyTo(buffer, i * 2);
                }
                 
                sound.SubmitBuffer(buffer);
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Oscillator tmp;
            OscillatorPanel.Children.Clear();
            mixer.Oscillators.Clear();
            for (int i = 0; i < NUM_GENERATORS; i++)
            {
                tmp = new Oscillator();
                OscillatorPanel.Children.Add(tmp);
                mixer.Oscillators.Add(tmp);
            }
        }

        private void btnPlay_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPlaying = true;
        }

        private void btnPlay_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPlaying = false;
        }
    }
}

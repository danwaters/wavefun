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
using System.Collections.Generic;

namespace WaveGeneratorWP7
{
    public class Mixer
    {
        private List<Oscillator> oscillators;
        double bufferDurationSeconds = .5f;
        const int MAX_AMPLITUDE = 32760; // for 16-bit audio
        const int SAMPLE_RATE = 48000;
        const int CHANNELS = 2;

        public List<Oscillator> Oscillators
        {
            get
            {
                return oscillators;
            }
        }

        public Mixer()
        {
            oscillators = new List<Oscillator>();
        }

        public short[] GenerateOscillatorSampleData(Oscillator osc)
        {
            // Creates a looping buffer based on the params given
            // Fill the buffer with whatever waveform at the specified frequency            
            int numSamples = Convert.ToInt32(bufferDurationSeconds * SAMPLE_RATE);
            short[] sampleData = new short[numSamples];
            double frequency = osc.Frequency;
            int amplitude = osc.Amplitude;
            double angle = (Math.PI * 2 * frequency) / (SAMPLE_RATE * CHANNELS);

            switch (osc.WaveType)
            {
                case WaveType.Sine:
                    {
                        for (int i = 0; i < numSamples; i++)
                        {
                            // Generate a sine wave in both channels.
                            sampleData[i] = Convert.ToInt16(amplitude * Math.Sin(angle * i));
                        }
                    }
                    break;
                case WaveType.Square:
                    {
                        for (int i = 0; i < numSamples; i++)
                        {
                            // Generate a square wave in both channels.
                            if (Math.Sin(angle * i) > 0)
                                sampleData[i] = Convert.ToInt16(amplitude);
                            else
                                sampleData[i] = Convert.ToInt16(-amplitude);
                            //sampleData[i] = Convert.ToInt16(maxAmplitude * Math.Floor(Math.Sin(angle * i)));
                        }
                    }
                    break;
                case WaveType.Sawtooth:
                    {
                        int samplesPerPeriod = Convert.ToInt32(SAMPLE_RATE / (frequency / CHANNELS));
                        short sampleStep = Convert.ToInt16((amplitude * 2) / samplesPerPeriod);
                        short tempSample = 0;

                        int i = 0;
                        int totalSamplesWritten = 0;
                        while (totalSamplesWritten < numSamples)
                        {
                            tempSample = (short)-amplitude;
                            for (i = 0; i < samplesPerPeriod && totalSamplesWritten < numSamples; i++)
                            {
                                tempSample += sampleStep;
                                sampleData[totalSamplesWritten] = tempSample;

                                totalSamplesWritten++;
                            }
                        }
                    }
                    break;
                case WaveType.Noise:
                    {
                        Random rnd = new Random();
                        for (int i = 0; i < numSamples; i++)
                        {
                            sampleData[i] = Convert.ToInt16(rnd.Next(-amplitude, amplitude));
                        }
                    }
                    break;
            }
            return sampleData;
        }

        public short[] MixToStream()
        {
            // Creates a looping buffer based on the params given
            // Fill the buffer with whatever waveform at the specified frequency
            int numSamples = Convert.ToInt32(bufferDurationSeconds * SAMPLE_RATE);
            short[] sampleData = new short[numSamples];

            // Fill in the sample data
            int sampleSum = 0;
            short oscCount = 0;

            List<OscillatorData> data = new List<OscillatorData>();

            // Add sample data
            foreach (Oscillator o in Oscillators)
            {
                if (o.WaveType != WaveType.None)
                {
                    oscCount++;
                    // generate data
                    data.Add(new OscillatorData(o, GenerateOscillatorSampleData(o)));
                }
            }

            // Generate buffer from all oscillators
            if (oscCount != 0)
            {
                for (int i = 0; i < numSamples; i++)
                {
                    sampleSum = 0;
                    foreach (OscillatorData d in data)
                    {
                        sampleSum += d.SampleData[i];
                    }
                    sampleData[i] = (short)(sampleSum / oscCount);
                }
            }

            return sampleData;
        }
    }
}

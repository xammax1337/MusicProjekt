using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjektClient.Helpers
{
    public class SoundMethods
    {
        public static void PlayIntroToConsoleClient()
        {
            string audioFilePath = "Sounds\\515615__mrthenoronha__8-bit-game-theme (mp3cut.net) (1).wav";

            using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(waveFileReader);

                    waveOutEvent.Play();

                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

        public static void PlayWrongInput()
        {
            string audioFilePath = "Sounds\\45654__simon_lacelle__dun-dun-dun.wav";

            using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(waveFileReader);

                    waveOutEvent.Play();

                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

        public static void PlaySuccessfullAdd()
        {
            string audioFilePath = "Sounds\\609336__kenneth_cooney__completed.wav";

            using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(waveFileReader);

                    waveOutEvent.Play();

                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

        public static void PlayUnsuccessfullAdd()
        {
            string audioFilePath = "Sounds\\29938__halleck__record_scratch_short.wav";

            using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(waveFileReader);

                    waveOutEvent.Play();

                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

        public static void PlayListingSound()
        {
            string audioFilePath = "Sounds\\253177__suntemple__retro-accomplished-sfx.wav";

            using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(waveFileReader);

                    waveOutEvent.Play();

                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

        public static void PlayUnsuccessfullConnect()
        {
            string audioFilePath = "Sounds\\29938__halleck__record_scratch_short.wav";

            using (WaveFileReader waveFileReader = new WaveFileReader(audioFilePath))
            {
                using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                {
                    waveOutEvent.Init(waveFileReader);

                    waveOutEvent.Play();

                    while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }

    }
}

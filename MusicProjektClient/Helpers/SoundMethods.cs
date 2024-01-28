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
        //The sound effect methods are here. For them, nuget package NAudio was installed.
        //WaveFileReader takes in the FilePath. An WaveOutEvent is initialized.
        //While loop checks the status of the sound that is playing, until it sees it's done.
        public static void PlayIntroToConsoleClientSound()
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

        public static void PlayWrongInputSound()
        {
            string audioFilePath = "Sounds\\45654__simon_lacelle__dun-dun-dun (mp3cut.net).wav";

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

        public static void PlaySuccessfulAddSound()
        {
            string audioFilePath = "Sounds\\624878__sonically_sound__old-video-game-4.wav";

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

        public static void PlayUnsuccessfulAddSound()
        {
            string audioFilePath = "Sounds\\29938__halleck__record_scratch_short (mp3cut.net).wav";

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
        public static void PlayListingNotPossibleSound()
        {
            string audioFilePath = "Sounds\\142608__autistic-lucario__error.wav";

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
        public static void PlaySuccessfulConnectSound()
        {
            string audioFilePath = "Sounds\\320655__rhodesmas__level-up-01.wav";

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
        public static void PlayUnsuccessfulConnectSound()
        {
            string audioFilePath = "Sounds\\259172__xtrgamr__uhoh.wav";

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

        public static void PlayReturnToMainMenuSound()
        {
            string audioFilePath = "Sounds\\15419__pagancow__dorm-door-opening.wav";

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

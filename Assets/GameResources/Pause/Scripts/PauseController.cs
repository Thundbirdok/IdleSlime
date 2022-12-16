using UnityEngine;

namespace GameResources.Pause.Scripts
{
    public static class PauseController
    {
        public static bool IsPaused { get; private set; }

        private static float _previousTimeScale;
        private static float _previousAudioVolume;

        public static void Pause()
        {
            if (IsPaused)
            {
                return;
            }

            IsPaused = true;

            _previousTimeScale = Time.timeScale;
            _previousAudioVolume = AudioListener.volume;

            Time.timeScale = 0;
            AudioListener.volume = 0;
        }

        public static void Continue()
        {
            if (IsPaused == false)
            {
                return;
            }

            IsPaused = false;
            
            Time.timeScale = _previousTimeScale;
            AudioListener.volume = _previousAudioVolume;
        }
    }
}

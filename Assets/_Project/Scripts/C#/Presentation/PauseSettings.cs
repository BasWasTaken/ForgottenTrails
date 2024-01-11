using UnityEngine;

namespace VVGames.ForgottenTrails.InkConnections
{
    [CreateAssetMenu(fileName = "PauseSettings", menuName = "Forgotten Trails/Pause Settings")]
    public class PauseSettings : ScriptableObject
    {
        #region Fields

        [Header("Relative Delays")]
        public float dotPause = 2.5f;

        public float commaPause = 2f;
        public float spacePause = 1.5f;

        [Range(1, 1)]
        public float normalPause = 1f;

        #endregion Fields

        #region Public Methods

        public float GetPause(char letter)
        {
            float delay = letter switch
            {
                '.' => dotPause,
                ':' => dotPause,
                ',' => commaPause,
                ';' => commaPause,
                ' ' => spacePause,
                '\t' => spacePause,
                '\n' => spacePause,
                _ => normalPause,
            };
            return delay;
        }

        #endregion Public Methods
    }
}
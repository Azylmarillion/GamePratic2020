// ===== Game Pratic 2020 - https://github.com/Azylmarillion/GamePratic2020 ===== //
//
// Notes :
//
// ============================================================================== //

using EnhancedEditor;
using UnityEngine;

namespace GamePratic2020
{
    [CreateAssetMenu(fileName = "Sound Database", menuName = "Sound/Database", order = 30)]
    public class SoundDatabase : ScriptableObject
    {
        #region Fields / Properties
        [HorizontalLine(1, order = 0), Section("Ambient", order = 1)]
        [SerializeField] private AudioClip[] ambientClips = new AudioClip[] { }; 

        [HorizontalLine(1, order = 0), Section("First Mini Game", order = 1)]
        [SerializeField] private AudioClip hammerReloading = null;
        [SerializeField] private AudioClip hammerStomp = null;

        [HorizontalLine(1, order = 0), Section("Second Mini Game", order = 1)]
        [SerializeField] private AudioClip gaugeAlarm = null;
        [SerializeField] private AudioClip crankLoop = null;
        [SerializeField] private AudioClip endAlarm = null;

        [HorizontalLine(1, order = 0), Section("Third Mini Game", order = 1)]
        [SerializeField] private AudioClip railLoop = null;
        [SerializeField] private AudioClip charcoalWaterfall = null;

        [HorizontalLine(1, order = 0), Section("Feedbacks", order = 1)]
        [SerializeField] private AudioClip winJingle = null;
        [SerializeField] private AudioClip scoreIncrement = null;

        [HorizontalLine(1, order = 0), Section("Menu", order = 1)]
        [SerializeField] private AudioClip clickClip = null; 

        public AudioClip[] AmbientClips => ambientClips;
        public AudioClip HammerReloading => hammerReloading;
        public AudioClip HammerStomp => hammerStomp;
        public AudioClip GaugeAlarm => gaugeAlarm;
        public AudioClip CrankLoop => crankLoop;
        public AudioClip EndAlarm => endAlarm;
        public AudioClip RailLoop => railLoop;
        public AudioClip CharcoalWaterfall => charcoalWaterfall;
        public AudioClip WinJingle => winJingle;
        public AudioClip ScoreIncrement => scoreIncrement;
        public AudioClip ClickClip => clickClip; 
        #endregion
    }
}

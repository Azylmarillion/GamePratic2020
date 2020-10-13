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
        [HorizontalLine(1, order = 0), Section("First Mini Game", order = 1)]
        [SerializeField] private AudioClip hammerReloading = null;
        [SerializeField] private AudioClip hammerStomp = null;

        [HorizontalLine(1, order = 0), Section("Second Mini Game", order = 1)]
        [SerializeField] private AudioClip gaugeAlarm = null;
        [SerializeField] private AudioClip endAlarm = null;

        [HorizontalLine(1, order = 0), Section("Third Mini Game", order = 1)]
        [SerializeField] private AudioClip wagonLaunch = null;
        [SerializeField] private AudioClip wagonLand = null; 
        [SerializeField] private AudioClip charcoalWaterfall = null;

        [HorizontalLine(1, order = 0), Section("Feedbacks", order = 1)]
        [SerializeField] private AudioClip winJingle = null;

        [HorizontalLine(1, order = 0), Section("Menu", order = 1)]
        [SerializeField] private AudioClip clickClip = null;

        [SerializeField] private AudioClip[] footSteps = new AudioClip[] { }; 

        public AudioClip HammerReloading => hammerReloading;
        public AudioClip HammerStomp => hammerStomp;
        public AudioClip GaugeAlarm => gaugeAlarm;
        public AudioClip EndAlarm => endAlarm;
        public AudioClip WagonLaunch => wagonLaunch;
        public AudioClip WagonLand => wagonLand;
        public AudioClip CharcoalWaterfall => charcoalWaterfall;
        public AudioClip WinJingle => winJingle;
        public AudioClip ClickClip => clickClip;

        public AudioClip GetRandomFootStep() => footSteps[Random.Range(0, footSteps.Length)]; 
        #endregion
    }
}

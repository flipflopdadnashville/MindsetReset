using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.DreamOS
{
    [CreateAssetMenu(fileName = "New Soundscape Preset Library", menuName = "DreamOS/New Soundscape Preset Library")]
    public class SoundscapePresetLibrary : ScriptableObject
    {
        // Library Content
        public List<PresetItem> pictures = new List<PresetItem>();

        [System.Serializable]
        public class PresetItem
        {
            public string presetTitle = "Preset Title";
            public string presetDescription = "Preset Description";
            public Sprite presetSprite;
        }
    }
}
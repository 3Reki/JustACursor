using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown screenModeDropdown;

        private EventSystem eventSystem;
        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;
        private List<FullScreenMode> screenModes; 
        
        private float currentRefreshRate;
        private int currentResolutionIndex;
        private int currentScreenModeIndex;

        private void Start()
        {
            SetupResolutions();
            SetupScreenModes();
        }

        private void SetupResolutions()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            currentRefreshRate = Screen.currentResolution.refreshRate;

            foreach (Resolution res in resolutions)
            {
                if (Mathf.Approximately(res.refreshRate,currentRefreshRate))
                {
                    filteredResolutions.Add(res);
                }
            }

            List<string> options = new List<string>();
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                Resolution res = filteredResolutions[i];
                
                string resOption = $"{res.width}x{res.height}";
                options.Add(resOption);

                if (res.width == Screen.width && res.height == Screen.height) currentResolutionIndex = i;
            }
            
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        private void SetupScreenModes()
        {
            screenModes = new List<FullScreenMode>();
            screenModes.Add(FullScreenMode.FullScreenWindow);
            screenModes.Add(FullScreenMode.Windowed);
            screenModes.Add(FullScreenMode.MaximizedWindow);
            
            List<string> options = new List<string>();
            for (int i = 0; i < screenModes.Count; i++)
            {
                options.Add(screenModes[i].ToString());

                if (Screen.fullScreenMode == screenModes[i])
                {
                    currentScreenModeIndex = i;
                }
            }
            
            screenModeDropdown.ClearOptions();
            screenModeDropdown.AddOptions(options);
            screenModeDropdown.value = currentScreenModeIndex;
            screenModeDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution res = filteredResolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        }

        public void SetFullScreenMode(int fullScreenModeIndex)
        {
            Screen.fullScreenMode = screenModes[fullScreenModeIndex];
        }
    }
}
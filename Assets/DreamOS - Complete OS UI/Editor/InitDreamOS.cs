using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine.Rendering;

public class InitGlassOS : MonoBehaviour
{
    [InitializeOnLoad]
	public class InitOnLoad
	{
		static InitOnLoad()
		{
			if (!EditorPrefs.HasKey("DreamOSv1.Installed"))
			{
				EditorPrefs.SetInt("DreamOSv1.Installed", 1);
				EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing DreamOS. I hope you'll enjoy using the package!" +
                    "\r\rYou can contact me at support@michsky.com or Discord for support.", "Got it");
			}

			if (!EditorPrefs.HasKey("DreamOS.ObjectCreator.Upgraded"))
			{
				EditorPrefs.SetInt("DreamOS.ObjectCreator.Upgraded", 1);
				EditorPrefs.SetString("DreamOS.ObjectCreator.RootFolder", "DreamOS - Complete OS UI/Prefabs/");
			}

			if (!EditorPrefs.HasKey("DreamOS.PipelineUpgrader") && GraphicsSettings.renderPipelineAsset != null)
			{
				EditorPrefs.SetInt("DreamOS.PipelineUpgrader", 1);
				
				if (EditorUtility.DisplayDialog("DreamOS SRP Upgrader", "It looks like your project is using URP/HDRP rendering pipeline, " +
					"would you like to upgrade DreamOS Theme Manager for your project?" +
					"\r\rNote that the blur shader is not currently compatible with URP/HDRP.", "Yes", "No"))
                {
					try
					{
						Preset defaultPreset = Resources.Load<Preset>("Theme Manager Presets/SRP Default");
						defaultPreset.ApplyTo(Resources.Load("Theme/Theme Manager"));
					}

					catch { }
				}
			}
		}
	}
}
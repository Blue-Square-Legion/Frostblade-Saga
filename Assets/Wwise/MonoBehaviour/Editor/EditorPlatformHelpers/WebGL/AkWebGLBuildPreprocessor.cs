/*******************************************************************************
The content of this file includes portions of the proprietary AUDIOKINETIC Wwise
Technology released in source code form as part of the game integration package.
The content of this file may not be used without valid licenses to the
AUDIOKINETIC Wwise Technology.
Note that the use of the game engine is subject to the Unity(R) Terms of
Service at https://unity3d.com/legal/terms-of-service
 
License Usage
 
Licensees holding valid licenses to the AUDIOKINETIC Wwise Technology may use
this file in accordance with the end user license agreement provided with the
software or, alternatively, in accordance with the terms contained
in a written agreement between you and Audiokinetic Inc.
Copyright (c) 2024 Audiokinetic Inc.
*******************************************************************************/

﻿#if UNITY_EDITOR
using System.IO;

[UnityEditor.InitializeOnLoad]
public class AkWebGLBuildPreprocessor
{
	static AkWebGLBuildPreprocessor()
	{
		if (UnityEditor.AssetDatabase.IsAssetImportWorkerProcess())
		{
			return;
		}

		AkBuildPreprocessor.RegisterBuildTarget(UnityEditor.BuildTarget.WebGL, new AkBuildPreprocessor.PlatformConfiguration
		{
			WwisePlatformName = "Web",
			OnPreprocessBuild = CopyWebGLStreamingAssets,
			OnPostprocessBuild = ClearWebGLStreamingAssets
		});
		WwiseSetupWizard.AddBuildTargetGroup(UnityEditor.BuildTargetGroup.WebGL);
	}


	public static void CopyWebGLStreamingAssets(string path)
	{
		var pfSettings = AkWwiseInitializationSettings.Instance.PlatformSettingsList;
		foreach (var pfSetting in pfSettings)
		{
			if (pfSetting is AkWebGLSettings)
			{
				AkWebGLSettings webSettings = (AkWebGLSettings)pfSetting;
				string scriptPath = webSettings.AdvancedSettings.m_AudioWorkletProcessorScriptPath;
				string processorScript = Path.Combine(UnityEngine.Application.dataPath, "Wwise", "API", "Runtime", "Plugins", "WebGL", "WwiseAudioWorklet.processor.js");
				if (File.Exists(processorScript))
				{
					string dest = Path.Combine(UnityEngine.Application.streamingAssetsPath, scriptPath);
					if (File.Exists(dest))
						File.Delete(dest);
					Directory.CreateDirectory(Path.GetDirectoryName(dest));
					File.Copy(processorScript, dest);
					UnityEditor.AssetDatabase.Refresh();
				}
			}
		}
	}

	public static void ClearWebGLStreamingAssets(string path)
	{
		var pfSettings = AkWwiseInitializationSettings.Instance.PlatformSettingsList;
		foreach (var pfSetting in pfSettings)
		{
			if (pfSetting is AkWebGLSettings)
			{
				AkWebGLSettings webSettings = (AkWebGLSettings)pfSetting;
				string scriptPath = webSettings.AdvancedSettings.m_AudioWorkletProcessorScriptPath;
				string assetPath = Path.Combine(UnityEngine.Application.streamingAssetsPath, scriptPath);
				if (File.Exists(assetPath))
				{
					File.Delete(assetPath);
				}
				string metaPath = assetPath + ".meta";
				if (File.Exists(metaPath))
				{
					File.Delete(metaPath);
				}
				UnityEditor.AssetDatabase.Refresh();
			}
		}
	}
}
#endif
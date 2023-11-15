using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000A4A RID: 2634
public static class StartScreenBackgroundDataUtils
{
	// Token: 0x06003405 RID: 13317 RVA: 0x000F3A34 File Offset: 0x000F1E34
	public static bool SwapBackgroundAudio(SerializedSceneData.AudioData _settings, ref PersistentMusic _musicSource, ref AudioSource _ambienceSource, bool _killPrevious)
	{
		if (_settings.Directories != null && _settings.Directories.Length > 0)
		{
			AudioManager audioManager = GameUtils.RequestManager<AudioManager>();
			if (audioManager != null)
			{
				for (int i = 0; i < _settings.Directories.Length; i++)
				{
					audioManager.AddAudioDirectory(_settings.Directories[i]);
				}
			}
		}
		if (_settings.Music != null && _musicSource != null && _musicSource.IsAlive() && _musicSource.GetAudioSource().clip != _settings.Music)
		{
			_musicSource.StopMusic(_killPrevious);
			_musicSource = UnityEngine.Object.Instantiate<PersistentMusic>(_musicSource);
			if (_musicSource != null)
			{
				AudioSource audioSource = _musicSource.GetAudioSource();
				audioSource.clip = _settings.Music;
				audioSource.Play();
			}
		}
		if (_ambienceSource != null)
		{
			bool isPlaying = _ambienceSource.isPlaying;
			_ambienceSource.Stop();
			_ambienceSource.clip = _settings.Ambience;
			if (isPlaying || (_ambienceSource.playOnAwake && _ambienceSource.time <= 0f))
			{
				_ambienceSource.Play();
			}
		}
		return true;
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x000F3B6B File Offset: 0x000F1F6B
	public static bool SetRenderData(SerializedSceneData.RenderData _data)
	{
		return StartScreenBackgroundDataUtils.LerpRenderData(_data, _data, 1f);
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x000F3B7C File Offset: 0x000F1F7C
	public static bool LerpRenderData(SerializedSceneData.RenderData _start, SerializedSceneData.RenderData _end, float _progress)
	{
		bool flag = _progress < 0.5f;
		RenderSettings.ambientMode = ((!flag) ? _end.AmbientSource : _start.AmbientSource);
		switch (_end.AmbientSource)
		{
		case AmbientMode.Skybox:
			RenderSettings.ambientSkyColor = Color.Lerp(_start.SkyboxData.SkyboxColour, _end.SkyboxData.SkyboxColour, _progress);
			goto IL_FD;
		case AmbientMode.Trilight:
			RenderSettings.ambientSkyColor = Color.Lerp(_start.GradientData.SkyColour, _end.GradientData.SkyColour, _progress);
			RenderSettings.ambientEquatorColor = Color.Lerp(_start.GradientData.EquatorColour, _end.GradientData.EquatorColour, _progress);
			RenderSettings.ambientGroundColor = Color.Lerp(_start.GradientData.GroundColour, _end.GradientData.GroundColour, _progress);
			goto IL_FD;
		case AmbientMode.Flat:
			RenderSettings.ambientSkyColor = Color.Lerp(_start.ColorData.Colour, _end.ColorData.Colour, _progress);
			goto IL_FD;
		}
		return false;
		IL_FD:
		RenderSettings.defaultReflectionMode = ((!flag) ? _end.ReflectionSource : _start.ReflectionSource);
		RenderSettings.skybox = ((!flag) ? _end.SkyboxMaterial : _start.SkyboxMaterial);
		DefaultReflectionMode reflectionSource = _end.ReflectionSource;
		if (reflectionSource != DefaultReflectionMode.Skybox)
		{
			if (reflectionSource != DefaultReflectionMode.Custom)
			{
				return false;
			}
			RenderSettings.customReflection = ((!flag) ? _end.CustomReflectionData.Cubemap : _start.CustomReflectionData.Cubemap);
			RenderSettings.reflectionIntensity = Mathf.Lerp(_start.CustomReflectionData.IntensityMultiplier, _end.CustomReflectionData.IntensityMultiplier, _progress);
			RenderSettings.reflectionBounces = (int)Mathf.Lerp((float)_start.CustomReflectionData.Bounces, (float)_end.CustomReflectionData.Bounces, _progress);
		}
		else
		{
			RenderSettings.defaultReflectionResolution = (int)Mathf.Lerp((float)_start.SkyboxReflectionData.Resolution, (float)_end.SkyboxReflectionData.Resolution, _progress);
			RenderSettings.reflectionIntensity = Mathf.Lerp(_start.SkyboxReflectionData.IntensityMultiplier, _end.SkyboxReflectionData.IntensityMultiplier, _progress);
			RenderSettings.reflectionBounces = (int)Mathf.Lerp((float)_start.SkyboxReflectionData.Bounces, (float)_end.SkyboxReflectionData.Bounces, _progress);
		}
		return true;
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x000F3DB8 File Offset: 0x000F21B8
	public static SerializedSceneData.RenderData CopyCurrentRenderData()
	{
		return new SerializedSceneData.RenderData
		{
			SkyboxMaterial = RenderSettings.skybox,
			AmbientSource = RenderSettings.ambientMode,
			SkyboxData = 
			{
				SkyboxColour = RenderSettings.ambientSkyColor
			},
			GradientData = 
			{
				SkyColour = RenderSettings.ambientSkyColor,
				EquatorColour = RenderSettings.ambientEquatorColor,
				GroundColour = RenderSettings.ambientGroundColor
			},
			ColorData = 
			{
				Colour = RenderSettings.ambientSkyColor
			},
			ReflectionSource = RenderSettings.defaultReflectionMode,
			SkyboxReflectionData = 
			{
				Resolution = RenderSettings.defaultReflectionResolution,
				IntensityMultiplier = RenderSettings.reflectionIntensity,
				Bounces = RenderSettings.reflectionBounces
			},
			CustomReflectionData = 
			{
				Cubemap = RenderSettings.customReflection,
				IntensityMultiplier = RenderSettings.reflectionIntensity,
				Bounces = RenderSettings.reflectionBounces
			}
		};
	}
}

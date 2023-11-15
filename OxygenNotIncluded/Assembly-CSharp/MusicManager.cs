using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020008AA RID: 2218
[AddComponentMenu("KMonoBehaviour/scripts/MusicManager")]
public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06004033 RID: 16435 RVA: 0x00167595 File Offset: 0x00165795
	public Dictionary<string, MusicManager.SongInfo> SongMap
	{
		get
		{
			return this.songMap;
		}
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x001675A0 File Offset: 0x001657A0
	public void PlaySong(string song_name, bool canWait = false)
	{
		this.Log("Play: " + song_name);
		if (!AudioDebug.Get().musicEnabled)
		{
			return;
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to play duplicate song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.Count == 0)
		{
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array = new object[1];
				int num = 0;
				string str = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array[num] = str + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array);
			}
			int num2 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num2 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num2, false);
			}
			if (songInfo.dynamic)
			{
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, 16000f);
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, 48000f);
				this.activeDynamicSong = songInfo;
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		if (songInfo.interruptsActiveMusic)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.activeSongs[list[i]].interruptsActiveMusic)
				{
					MusicManager.SongInfo songInfo2 = this.activeSongs[list[i]];
					songInfo2.ev.setParameterByName("interrupted_dimmed", 1f, false);
					this.Log("Dimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					songInfo.songsOnHold.Add(list[i]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array2 = new object[1];
				int num3 = 0;
				string str2 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array2[num3] = str2 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array2);
			}
			songInfo.ev.start();
			songInfo.ev.release();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		int num4 = 0;
		foreach (string key in this.activeSongs.Keys)
		{
			MusicManager.SongInfo songInfo3 = this.activeSongs[key];
			if (!songInfo3.interruptsActiveMusic && songInfo3.priority > num4)
			{
				num4 = songInfo3.priority;
			}
		}
		if (songInfo.priority >= num4)
		{
			for (int j = 0; j < list.Count; j++)
			{
				MusicManager.SongInfo songInfo4 = this.activeSongs[list[j]];
				FMOD.Studio.EventInstance ev = songInfo4.ev;
				if (!songInfo4.interruptsActiveMusic)
				{
					ev.setParameterByName("interrupted_dimmed", 1f, false);
					ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.activeSongs.Remove(list[j]);
					list.Remove(list[j]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array3 = new object[1];
				int num5 = 0;
				string str3 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array3[num5] = str3 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array3);
			}
			int num6 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num6 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num6, false);
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
		}
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x001679B0 File Offset: 0x00165BB0
	public void StopSong(string song_name, bool shouldLog = true, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (shouldLog)
		{
			this.Log("Stop: " + song_name);
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (!this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to stop a song that isn't playing:",
				song_name
			});
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		ev.stop(stopMode);
		ev.release();
		if (songInfo.dynamic)
		{
			this.activeDynamicSong = null;
		}
		if (songInfo.songsOnHold.Count > 0)
		{
			for (int i = 0; i < songInfo.songsOnHold.Count; i++)
			{
				MusicManager.SongInfo songInfo2;
				if (this.activeSongs.TryGetValue(songInfo.songsOnHold[i], out songInfo2) && songInfo2.ev.isValid())
				{
					FMOD.Studio.EventInstance ev2 = songInfo2.ev;
					this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					ev2.setParameterByName("interrupted_dimmed", 0f, false);
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
				else
				{
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
			}
		}
		this.activeSongs.Remove(song_name);
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x00167B14 File Offset: 0x00165D14
	public void KillAllSongs(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		this.Log("Kill All Songs");
		if (this.DynamicMusicIsActive())
		{
			this.StopDynamicMusic(true);
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			this.StopSong(list[i], true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x00167B6C File Offset: 0x00165D6C
	public void SetSongParameter(string song_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_value));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByName(parameter_name, parameter_value, false);
		}
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x00167BC4 File Offset: 0x00165DC4
	public void SetSongParameter(string song_name, string parameter_name, string parameter_lable, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_lable));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByNameWithLabel(parameter_name, parameter_lable, false);
		}
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x00167C18 File Offset: 0x00165E18
	public bool SongIsPlaying(string song_name)
	{
		MusicManager.SongInfo songInfo = null;
		return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
	}

	// Token: 0x0600403A RID: 16442 RVA: 0x00167C44 File Offset: 0x00165E44
	private void Update()
	{
		this.ClearFinishedSongs();
		if (this.DynamicMusicIsActive())
		{
			this.SetDynamicMusicZoomLevel();
			this.SetDynamicMusicTimeSinceLastJob();
			if (this.activeDynamicSong.useTimeOfDay)
			{
				this.SetDynamicMusicTimeOfDay();
			}
			if (GameClock.Instance != null && GameClock.Instance.GetCurrentCycleAsPercentage() >= this.duskTimePercentage / 100f)
			{
				this.StopDynamicMusic(false);
			}
		}
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x00167CAC File Offset: 0x00165EAC
	private void ClearFinishedSongs()
	{
		if (this.activeSongs.Count > 0)
		{
			ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				MusicManager.SongInfo value = keyValuePair.Value;
				FMOD.Studio.EventInstance ev = value.ev;
				ev.getPlaybackState(out value.musicPlaybackState);
				if (value.musicPlaybackState == PLAYBACK_STATE.STOPPED || value.musicPlaybackState == PLAYBACK_STATE.STOPPING)
				{
					pooledList.Add(keyValuePair.Key);
					foreach (string song_name in value.songsOnHold)
					{
						this.SetSongParameter(song_name, "interrupted_dimmed", 0f, true);
					}
					value.songsOnHold.Clear();
				}
			}
			foreach (string key in pooledList)
			{
				this.activeSongs.Remove(key);
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x00167DFC File Offset: 0x00165FFC
	public void OnEscapeMenu(bool paused)
	{
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			if (keyValuePair.Value != null)
			{
				this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
			}
		}
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x00167E6C File Offset: 0x0016606C
	public void OnSupplyClosetMenu(bool paused, float fadeTime)
	{
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			if (keyValuePair.Value != null && (paused || !keyValuePair.Value.dynamic))
			{
				this.StartFadeToPause(keyValuePair.Value.ev, paused, fadeTime);
			}
		}
	}

	// Token: 0x0600403E RID: 16446 RVA: 0x00167EE8 File Offset: 0x001660E8
	public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
	{
		if (paused)
		{
			base.StartCoroutine(this.FadeToPause(inst, fadeTime));
			return;
		}
		base.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
	}

	// Token: 0x0600403F RID: 16447 RVA: 0x00167F0C File Offset: 0x0016610C
	private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 0f;
		float lerpTime = 0f;
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		inst.setPaused(true);
		yield break;
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x00167F22 File Offset: 0x00166122
	private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 1f;
		float lerpTime = 0f;
		inst.setPaused(false);
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x00167F38 File Offset: 0x00166138
	public void PlayDynamicMusic()
	{
		if (this.DynamicMusicIsActive())
		{
			this.Log("Trying to play DynamicMusic when it is already playing.");
			return;
		}
		string nextDynamicSong = this.GetNextDynamicSong();
		if (nextDynamicSong == "NONE")
		{
			return;
		}
		this.PlaySong(nextDynamicSong, false);
		MusicManager.SongInfo songInfo;
		if (this.activeSongs.TryGetValue(nextDynamicSong, out songInfo))
		{
			this.activeDynamicSong = songInfo;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
			if (SpeedControlScreen.Instance != null && SpeedControlScreen.Instance.IsPaused)
			{
				this.SetDynamicMusicPaused();
			}
			if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
			{
				this.SetDynamicMusicOverlayActive();
			}
			this.SetDynamicMusicPlayHook();
			this.SetDynamicMusicKeySigniture();
			string key = "Volume_Music";
			if (KPlayerPrefs.HasKey(key))
			{
				float @float = KPlayerPrefs.GetFloat(key);
				AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", @float, true);
			}
			AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f, true);
			return;
		}
		this.Log("DynamicMusic song " + nextDynamicSong + " did not start.");
		string text = "";
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			text = text + keyValuePair.Key + ", ";
			global::Debug.Log(text);
		}
		DebugUtil.DevAssert(false, "Song failed to play: " + nextDynamicSong, null);
	}

	// Token: 0x06004042 RID: 16450 RVA: 0x001680DC File Offset: 0x001662DC
	public void StopDynamicMusic(bool stopImmediate = false)
	{
		if (this.activeDynamicSong != null)
		{
			FMOD.Studio.STOP_MODE stopMode = stopImmediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
			this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
			this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), true, stopMode);
			this.activeDynamicSong = null;
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x00168150 File Offset: 0x00166350
	public string GetNextDynamicSong()
	{
		string result = "";
		if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
		{
			while (this.nextMusicType == MusicManager.TypeOfMusic.None)
			{
				this.CycleToNextMusicType();
			}
		}
		switch (this.nextMusicType)
		{
		case MusicManager.TypeOfMusic.DynamicSong:
			result = this.fullSongPlaylist.GetNextSong();
			this.activePlaylist = this.fullSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.MiniSong:
			result = this.miniSongPlaylist.GetNextSong();
			this.activePlaylist = this.miniSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.None:
			result = "NONE";
			this.activePlaylist = null;
			break;
		}
		this.CycleToNextMusicType();
		return result;
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x001681E8 File Offset: 0x001663E8
	private void CycleToNextMusicType()
	{
		int num = this.musicTypeIterator + 1;
		this.musicTypeIterator = num;
		this.musicTypeIterator = num % this.musicStyleOrder.Length;
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06004045 RID: 16453 RVA: 0x00168228 File Offset: 0x00166428
	public bool DynamicMusicIsActive()
	{
		return this.activeDynamicSong != null;
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x00168235 File Offset: 0x00166435
	public void SetDynamicMusicPaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f, true);
		}
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00168260 File Offset: 0x00166460
	public void SetDynamicMusicUnpaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0f, true);
		}
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x0016828C File Offset: 0x0016648C
	public void SetDynamicMusicZoomLevel()
	{
		if (CameraController.Instance != null)
		{
			float parameter_value = 100f - Camera.main.orthographicSize / 20f * 100f;
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", parameter_value, false);
		}
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x001682E0 File Offset: 0x001664E0
	public void SetDynamicMusicTimeSinceLastJob()
	{
		this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x00168310 File Offset: 0x00166510
	public void SetDynamicMusicTimeOfDay()
	{
		if (this.time >= this.timeOfDayUpdateRate)
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
			this.time = 0f;
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x0016836E File Offset: 0x0016656E
	public void SetDynamicMusicOverlayActive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f, true);
		}
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00168399 File Offset: 0x00166599
	public void SetDynamicMusicOverlayInactive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0f, true);
		}
	}

	// Token: 0x0600404D RID: 16461 RVA: 0x001683C4 File Offset: 0x001665C4
	public void SetDynamicMusicPlayHook()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			this.SetSongParameter(simpleSoundEventName, "playHook", this.activeDynamicSong.playHook ? 1f : 0f, true);
			this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
		}
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x0016843F File Offset: 0x0016663F
	public bool ShouldPlayDynamicMusicLoadedGame()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() <= this.loadGameCutoffPercentage / 100f;
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x0016845C File Offset: 0x0016665C
	public void SetDynamicMusicKeySigniture()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			string musicKeySigniture = this.activePlaylist.songMap[simpleSoundEventName].musicKeySigniture;
			float value;
			if (musicKeySigniture != null)
			{
				if (musicKeySigniture == "Ab")
				{
					value = 0f;
					goto IL_92;
				}
				if (musicKeySigniture == "Bb")
				{
					value = 1f;
					goto IL_92;
				}
				if (musicKeySigniture == "C")
				{
					value = 2f;
					goto IL_92;
				}
				if (musicKeySigniture == "D")
				{
					value = 3f;
					goto IL_92;
				}
			}
			value = 2f;
			IL_92:
			RuntimeManager.StudioSystem.setParameterByName("MusicInKey", value, false);
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06004050 RID: 16464 RVA: 0x00168510 File Offset: 0x00166710
	public static MusicManager instance
	{
		get
		{
			return MusicManager._instance;
		}
	}

	// Token: 0x06004051 RID: 16465 RVA: 0x00168517 File Offset: 0x00166717
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
		{
			this.alwaysPlayMusic = (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1);
		}
	}

	// Token: 0x06004052 RID: 16466 RVA: 0x00168551 File Offset: 0x00166751
	protected override void OnPrefabInit()
	{
		MusicManager._instance = this;
		this.ConfigureSongs();
		this.fullSongPlaylist.ResetUnplayedSongs();
		this.miniSongPlaylist.ResetUnplayedSongs();
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06004053 RID: 16467 RVA: 0x00168588 File Offset: 0x00166788
	protected override void OnCleanUp()
	{
		MusicManager._instance = null;
	}

	// Token: 0x06004054 RID: 16468 RVA: 0x00168590 File Offset: 0x00166790
	[ContextMenu("Reload")]
	private void ConfigureSongs()
	{
		this.songMap.Clear();
		foreach (MusicManager.DynamicSong dynamicSong in this.fullSongs)
		{
			if (DlcManager.IsContentActive(dynamicSong.requiredDlcId))
			{
				string simpleSoundEventName = Assets.GetSimpleSoundEventName(dynamicSong.fmodEvent);
				MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
				songInfo.fmodEvent = dynamicSong.fmodEvent;
				songInfo.requiredDlcId = dynamicSong.requiredDlcId;
				songInfo.priority = 100;
				songInfo.interruptsActiveMusic = false;
				songInfo.dynamic = true;
				songInfo.useTimeOfDay = dynamicSong.useTimeOfDay;
				songInfo.numberOfVariations = dynamicSong.numberOfVariations;
				songInfo.musicKeySigniture = dynamicSong.musicKeySigniture;
				songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName] = songInfo;
				this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
			}
		}
		foreach (MusicManager.Minisong minisong in this.miniSongs)
		{
			if (DlcManager.IsContentActive(minisong.requiredDlcId))
			{
				string simpleSoundEventName2 = Assets.GetSimpleSoundEventName(minisong.fmodEvent);
				MusicManager.SongInfo songInfo2 = new MusicManager.SongInfo();
				songInfo2.fmodEvent = minisong.fmodEvent;
				songInfo2.requiredDlcId = minisong.requiredDlcId;
				songInfo2.priority = 100;
				songInfo2.interruptsActiveMusic = false;
				songInfo2.dynamic = true;
				songInfo2.useTimeOfDay = false;
				songInfo2.numberOfVariations = 5;
				songInfo2.musicKeySigniture = minisong.musicKeySigniture;
				songInfo2.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName2] = songInfo2;
				this.miniSongPlaylist.songMap[simpleSoundEventName2] = songInfo2;
			}
		}
		foreach (MusicManager.Stinger stinger in this.stingers)
		{
			if (DlcManager.IsContentActive(stinger.requiredDlcId))
			{
				string simpleSoundEventName3 = Assets.GetSimpleSoundEventName(stinger.fmodEvent);
				MusicManager.SongInfo songInfo3 = new MusicManager.SongInfo();
				songInfo3.fmodEvent = stinger.fmodEvent;
				songInfo3.priority = 100;
				songInfo3.interruptsActiveMusic = true;
				songInfo3.dynamic = false;
				songInfo3.useTimeOfDay = false;
				songInfo3.numberOfVariations = 0;
				songInfo3.requiredDlcId = stinger.requiredDlcId;
				this.SongMap[simpleSoundEventName3] = songInfo3;
			}
		}
		foreach (MusicManager.MenuSong menuSong in this.menuSongs)
		{
			if (DlcManager.IsContentActive(menuSong.requiredDlcId))
			{
				string simpleSoundEventName4 = Assets.GetSimpleSoundEventName(menuSong.fmodEvent);
				MusicManager.SongInfo songInfo4 = new MusicManager.SongInfo();
				songInfo4.fmodEvent = menuSong.fmodEvent;
				songInfo4.priority = 100;
				songInfo4.interruptsActiveMusic = true;
				songInfo4.dynamic = false;
				songInfo4.useTimeOfDay = false;
				songInfo4.numberOfVariations = 0;
				songInfo4.requiredDlcId = menuSong.requiredDlcId;
				this.SongMap[simpleSoundEventName4] = songInfo4;
			}
		}
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x00168872 File Offset: 0x00166A72
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x00168874 File Offset: 0x00166A74
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06004057 RID: 16471 RVA: 0x00168876 File Offset: 0x00166A76
	private void Log(string s)
	{
	}

	// Token: 0x040029E7 RID: 10727
	private const string VARIATION_ID = "variation";

	// Token: 0x040029E8 RID: 10728
	private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";

	// Token: 0x040029E9 RID: 10729
	private const string MUSIC_KEY = "MusicInKey";

	// Token: 0x040029EA RID: 10730
	private const float DYNAMIC_MUSIC_SCHEDULE_DELAY = 16000f;

	// Token: 0x040029EB RID: 10731
	private const float DYNAMIC_MUSIC_SCHEDULE_LOOKAHEAD = 48000f;

	// Token: 0x040029EC RID: 10732
	[Header("Song Lists")]
	[Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
	[SerializeField]
	private MusicManager.DynamicSong[] fullSongs;

	// Token: 0x040029ED RID: 10733
	[Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
	[SerializeField]
	private MusicManager.Minisong[] miniSongs;

	// Token: 0x040029EE RID: 10734
	[Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
	[SerializeField]
	private MusicManager.Stinger[] stingers;

	// Token: 0x040029EF RID: 10735
	[Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
	[SerializeField]
	private MusicManager.MenuSong[] menuSongs;

	// Token: 0x040029F0 RID: 10736
	private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x040029F1 RID: 10737
	public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x040029F2 RID: 10738
	[Space]
	[Header("Tuning Values")]
	[Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
	[SerializeField]
	private float duskTimePercentage = 85f;

	// Token: 0x040029F3 RID: 10739
	[Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
	[SerializeField]
	private float loadGameCutoffPercentage = 50f;

	// Token: 0x040029F4 RID: 10740
	[Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float dynamicMusicSFXAttenuationPercentage = 65f;

	// Token: 0x040029F5 RID: 10741
	[Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float miniSongSFXAttenuationPercentage;

	// Token: 0x040029F6 RID: 10742
	[SerializeField]
	private MusicManager.TypeOfMusic[] musicStyleOrder;

	// Token: 0x040029F7 RID: 10743
	[NonSerialized]
	public bool alwaysPlayMusic;

	// Token: 0x040029F8 RID: 10744
	private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x040029F9 RID: 10745
	private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x040029FA RID: 10746
	[NonSerialized]
	public MusicManager.SongInfo activeDynamicSong;

	// Token: 0x040029FB RID: 10747
	[NonSerialized]
	public MusicManager.DynamicSongPlaylist activePlaylist;

	// Token: 0x040029FC RID: 10748
	private MusicManager.TypeOfMusic nextMusicType;

	// Token: 0x040029FD RID: 10749
	private int musicTypeIterator;

	// Token: 0x040029FE RID: 10750
	private float time;

	// Token: 0x040029FF RID: 10751
	private float timeOfDayUpdateRate = 2f;

	// Token: 0x04002A00 RID: 10752
	private static MusicManager _instance;

	// Token: 0x04002A01 RID: 10753
	[NonSerialized]
	public List<string> MusicDebugLog = new List<string>();

	// Token: 0x020016E2 RID: 5858
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class SongInfo
	{
		// Token: 0x04006D44 RID: 27972
		public EventReference fmodEvent;

		// Token: 0x04006D45 RID: 27973
		[NonSerialized]
		public int priority;

		// Token: 0x04006D46 RID: 27974
		[NonSerialized]
		public bool interruptsActiveMusic;

		// Token: 0x04006D47 RID: 27975
		[NonSerialized]
		public bool dynamic;

		// Token: 0x04006D48 RID: 27976
		[NonSerialized]
		public string requiredDlcId = "";

		// Token: 0x04006D49 RID: 27977
		[NonSerialized]
		public bool useTimeOfDay;

		// Token: 0x04006D4A RID: 27978
		[NonSerialized]
		public int numberOfVariations;

		// Token: 0x04006D4B RID: 27979
		[NonSerialized]
		public string musicKeySigniture = "C";

		// Token: 0x04006D4C RID: 27980
		[NonSerialized]
		public FMOD.Studio.EventInstance ev;

		// Token: 0x04006D4D RID: 27981
		[NonSerialized]
		public List<string> songsOnHold = new List<string>();

		// Token: 0x04006D4E RID: 27982
		[NonSerialized]
		public PLAYBACK_STATE musicPlaybackState;

		// Token: 0x04006D4F RID: 27983
		[NonSerialized]
		public bool playHook = true;

		// Token: 0x04006D50 RID: 27984
		[NonSerialized]
		public float sfxAttenuationPercentage = 65f;
	}

	// Token: 0x020016E3 RID: 5859
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class DynamicSong
	{
		// Token: 0x04006D51 RID: 27985
		public EventReference fmodEvent;

		// Token: 0x04006D52 RID: 27986
		[Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
		[SerializeField]
		public bool useTimeOfDay;

		// Token: 0x04006D53 RID: 27987
		[Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
		[SerializeField]
		public int numberOfVariations;

		// Token: 0x04006D54 RID: 27988
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x04006D55 RID: 27989
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020016E4 RID: 5860
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Stinger
	{
		// Token: 0x04006D56 RID: 27990
		public EventReference fmodEvent;

		// Token: 0x04006D57 RID: 27991
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020016E5 RID: 5861
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class MenuSong
	{
		// Token: 0x04006D58 RID: 27992
		public EventReference fmodEvent;

		// Token: 0x04006D59 RID: 27993
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020016E6 RID: 5862
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Minisong
	{
		// Token: 0x04006D5A RID: 27994
		public EventReference fmodEvent;

		// Token: 0x04006D5B RID: 27995
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x04006D5C RID: 27996
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x020016E7 RID: 5863
	public enum TypeOfMusic
	{
		// Token: 0x04006D5E RID: 27998
		DynamicSong,
		// Token: 0x04006D5F RID: 27999
		MiniSong,
		// Token: 0x04006D60 RID: 28000
		None
	}

	// Token: 0x020016E8 RID: 5864
	public class DynamicSongPlaylist
	{
		// Token: 0x06008CEA RID: 36074 RVA: 0x0031A15C File Offset: 0x0031835C
		public string GetNextSong()
		{
			string text;
			if (this.unplayedSongs.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index];
				this.unplayedSongs.RemoveAt(index);
			}
			else
			{
				this.ResetUnplayedSongs();
				bool flag = this.unplayedSongs.Count > 1;
				if (flag)
				{
					for (int i = 0; i < this.unplayedSongs.Count; i++)
					{
						if (this.unplayedSongs[i] == this.lastSongPlayed)
						{
							this.unplayedSongs.Remove(this.unplayedSongs[i]);
							break;
						}
					}
				}
				int index2 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index2];
				this.unplayedSongs.RemoveAt(index2);
				if (flag)
				{
					this.unplayedSongs.Add(this.lastSongPlayed);
				}
			}
			this.lastSongPlayed = text;
			global::Debug.Assert(this.songMap.ContainsKey(text), "Missing song " + text);
			return Assets.GetSimpleSoundEventName(this.songMap[text].fmodEvent);
		}

		// Token: 0x06008CEB RID: 36075 RVA: 0x0031A288 File Offset: 0x00318488
		public void ResetUnplayedSongs()
		{
			this.unplayedSongs.Clear();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.songMap)
			{
				if (DlcManager.IsContentActive(keyValuePair.Value.requiredDlcId))
				{
					this.unplayedSongs.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x04006D61 RID: 28001
		public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

		// Token: 0x04006D62 RID: 28002
		public List<string> unplayedSongs = new List<string>();

		// Token: 0x04006D63 RID: 28003
		private string lastSongPlayed = "";
	}
}

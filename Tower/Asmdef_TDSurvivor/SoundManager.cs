using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020000E1 RID: 225
public class SoundManager : Singleton<SoundManager>
{
	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00015263 File Offset: 0x00013463
	// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001526B File Offset: 0x0001346B
	public bool IsMute { get; private set; }

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000548 RID: 1352 RVA: 0x00015274 File Offset: 0x00013474
	// (set) Token: 0x06000549 RID: 1353 RVA: 0x0001527C File Offset: 0x0001347C
	public SoundPlayer currentBGMData { get; private set; }

	// Token: 0x0600054A RID: 1354 RVA: 0x00015285 File Offset: 0x00013485
	private new void Awake()
	{
		this.Initialize();
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0001528D File Offset: 0x0001348D
	private void Initialize()
	{
		if (!this.isInitialized)
		{
			this.list_SoundPlayers = new List<SoundPlayer>();
			this.list_SoundCoolDown = new List<SoundCooldown>();
			this.list_SoundAssetData = new List<SoundAssetData>();
			this.isInitialized = true;
		}
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x000152C0 File Offset: 0x000134C0
	private void LoadSoundAssetData(SoundAssetData assetData)
	{
		for (int i = 0; i < assetData.SoundFile.Length; i++)
		{
			int key = Animator.StringToHash(this.CombineKeyAndClipName(assetData.DataKey, assetData.SoundFile[i].name));
			if (!this.audioDic.ContainsKey(key))
			{
				this.audioDic.Add(key, assetData.SoundFile[i]);
			}
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00015324 File Offset: 0x00013524
	private void UnloadSoundAssetData(SoundAssetData assetData)
	{
		for (int i = 0; i < assetData.SoundFile.Length; i++)
		{
			int key = Animator.StringToHash(this.CombineKeyAndClipName(assetData.DataKey, assetData.SoundFile[i].name));
			if (this.audioDic.ContainsKey(key))
			{
				this.audioDic.Remove(key);
			}
		}
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00015380 File Offset: 0x00013580
	private void Update()
	{
		for (int i = 0; i < this.list_SoundPlayers.Count; i++)
		{
			if (this.list_SoundPlayers[i].gameObject.activeSelf)
			{
				this.list_SoundPlayers[i].SoundUpdate(Time.unscaledDeltaTime);
			}
		}
		if (this.list_SoundCoolDown.Count > 0)
		{
			for (int j = this.list_SoundCoolDown.Count - 1; j >= 0; j--)
			{
				this.list_SoundCoolDown[j].timer -= Time.unscaledDeltaTime;
				if (this.list_SoundCoolDown[j].timer <= 0f)
				{
					this.list_SoundCoolDown.RemoveAt(j);
				}
			}
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00015438 File Offset: 0x00013638
	private void setMuteAll(bool isMute)
	{
		this.IsMute = isMute;
		this.setMute(SoundAssetData.SOUND_TYPE.BGM, isMute);
		this.setMute(SoundAssetData.SOUND_TYPE.SOUND, isMute);
		this.setMute(SoundAssetData.SOUND_TYPE.VOCAL, isMute);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001545C File Offset: 0x0001365C
	private void setMute(SoundAssetData.SOUND_TYPE soundType, bool isMute)
	{
		switch (soundType)
		{
		case SoundAssetData.SOUND_TYPE.SOUND:
			this.volumeMultiplier_Sound = (isMute ? 0f : 1f);
			break;
		case SoundAssetData.SOUND_TYPE.BGM:
			this.volumeMultiplier_Music = (isMute ? 0f : 1f);
			break;
		case SoundAssetData.SOUND_TYPE.VOCAL:
			this.volumeMultiplier_Vocal = (isMute ? 0f : 1f);
			break;
		}
		this.UpdateVolumeSetting();
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x000154CC File Offset: 0x000136CC
	private void UpdateVolumeSetting()
	{
		foreach (SoundPlayer soundPlayer in this.list_SoundPlayers)
		{
		}
		this.audioMixer_Sound.audioMixer.SetFloat("SoundVolume", this.LinearToDecibel(this.GetVolumeMultiplierByType(SoundAssetData.SOUND_TYPE.SOUND)));
		this.audioMixer_Music.audioMixer.SetFloat("MusicVolume", this.LinearToDecibel(this.GetVolumeMultiplierByType(SoundAssetData.SOUND_TYPE.BGM)));
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00015560 File Offset: 0x00013760
	private SoundPlayer GetAvaliableSoundObj()
	{
		for (int i = 0; i < this.list_SoundPlayers.Count; i++)
		{
			if (!this.list_SoundPlayers[i].gameObject.activeSelf && !this.list_SoundPlayers[i].IsReserved)
			{
				return this.list_SoundPlayers[i];
			}
		}
		GameObject gameObject = new GameObject();
		SoundPlayer result = gameObject.AddComponent<SoundPlayer>();
		gameObject.AddComponent<AudioSource>();
		gameObject.GetComponent<SoundPlayer>().audioSource = gameObject.GetComponent<AudioSource>();
		this.list_SoundPlayers.Add(gameObject.GetComponent<SoundPlayer>());
		return result;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000155F0 File Offset: 0x000137F0
	private void IncreaseSoundIndex()
	{
		this.soundIndex++;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00015600 File Offset: 0x00013800
	private int playSound_RandomPitch(string assetKeyName, string sndName, float minPitch = -1f, float maxPitch = -1f, float cooldown = -1f, float delay = -1f)
	{
		minPitch = Mathf.Max(0f, minPitch);
		maxPitch = Mathf.Max(0f, maxPitch);
		if (maxPitch < minPitch)
		{
			maxPitch = minPitch;
		}
		return this.playSound(assetKeyName, sndName, Random.Range(minPitch, maxPitch), cooldown, delay);
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0001563C File Offset: 0x0001383C
	private int playSound(string assetKeyName, string sndName, float pitch = -1f, float cooldown = -1f, float delay = -1f)
	{
		string fullName = this.CombineKeyAndClipName(assetKeyName, sndName);
		return this.PlaySoundFinal(fullName, pitch, cooldown, delay);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00015660 File Offset: 0x00013860
	private bool CheckCoolDown(string sndName)
	{
		bool result = false;
		for (int i = this.list_SoundCoolDown.Count - 1; i >= 0; i--)
		{
			if (this.list_SoundCoolDown[i].sndName.Equals(sndName) && this.list_SoundCoolDown[i].timer > 0f)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x000156BD File Offset: 0x000138BD
	private void RegisterCoolDown(string sndName, float coolDownTime)
	{
		this.list_SoundCoolDown.Add(new SoundCooldown(sndName, coolDownTime));
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000156D4 File Offset: 0x000138D4
	private void UnregisterCoolDown(string sndName)
	{
		for (int i = this.list_SoundCoolDown.Count - 1; i >= 0; i--)
		{
			if (sndName == this.list_SoundCoolDown[i].sndName)
			{
				this.list_SoundCoolDown.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00015720 File Offset: 0x00013920
	private int PlaySoundFinal(string fullName, float pitch, float cooldown, float delay)
	{
		SoundEntry soundEntry;
		if (!this.audioDic.TryGetValue(Animator.StringToHash(fullName), out soundEntry))
		{
			Debug.LogError("找不到要播放的音效: " + fullName);
			return -1;
		}
		if (this.CheckCoolDown(fullName))
		{
			return -1;
		}
		this.UnregisterCoolDown(fullName);
		if (cooldown > 0f || soundEntry.haveCooldown)
		{
			float coolDownTime = (cooldown > 0f) ? cooldown : soundEntry.cooldownTime;
			this.RegisterCoolDown(fullName, coolDownTime);
		}
		this.IncreaseSoundIndex();
		SoundPlayer avaliableSoundObj = this.GetAvaliableSoundObj();
		GameObject gameObject = avaliableSoundObj.gameObject;
		if (soundEntry.doRandomClip)
		{
			avaliableSoundObj.audioSource.clip = soundEntry.clips[Random.Range(0, soundEntry.clips.Length)];
		}
		else
		{
			avaliableSoundObj.audioSource.clip = soundEntry.clips[0];
		}
		avaliableSoundObj.audioSource.loop = soundEntry.doLoop;
		if (pitch > 0f)
		{
			avaliableSoundObj.audioSource.pitch = pitch;
		}
		else if (soundEntry.doRandomPitch)
		{
			avaliableSoundObj.audioSource.pitch = Random.Range(soundEntry.randomPitchRange.x, soundEntry.randomPitchRange.y);
		}
		else
		{
			avaliableSoundObj.audioSource.pitch = 1f;
		}
		avaliableSoundObj.audioSource.spatialBlend = 0f;
		avaliableSoundObj.audioSource.playOnAwake = false;
		avaliableSoundObj.audioSource.mute = false;
		if (this.audioMixer_Sound != null)
		{
			avaliableSoundObj.audioSource.outputAudioMixerGroup = this.audioMixer_Sound;
		}
		avaliableSoundObj.soundEntry = soundEntry;
		if (avaliableSoundObj.audioSource.clip != null)
		{
			avaliableSoundObj.soundEntry.soundLength = avaliableSoundObj.audioSource.clip.length;
		}
		else
		{
			avaliableSoundObj.soundEntry.soundLength = 0f;
		}
		avaliableSoundObj.fadeIn = 0f;
		avaliableSoundObj.fadeOut = 0f;
		avaliableSoundObj.soundIndex = this.soundIndex;
		avaliableSoundObj.mod_volume = soundEntry.mod_Volume * this.GetVolumeMultiplierByType(soundEntry.type);
		gameObject.name = fullName;
		gameObject.transform.parent = base.transform;
		if (delay > 0f)
		{
			avaliableSoundObj.ReserveToPlay();
			base.StartCoroutine(this.CR_StartPlaySoundDelayed(delay, gameObject, avaliableSoundObj));
		}
		else
		{
			gameObject.SetActive(true);
			avaliableSoundObj.audioSource.time = 0f;
			avaliableSoundObj.audioSource.Play();
			avaliableSoundObj.OnSoundPlay();
		}
		return this.soundIndex;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x00015979 File Offset: 0x00013B79
	private IEnumerator CR_StartPlaySoundDelayed(float delayTime, GameObject playObj, SoundPlayer soundData)
	{
		yield return new WaitForSeconds(delayTime);
		playObj.SetActive(true);
		soundData.audioSource.time = 0f;
		soundData.audioSource.Play();
		soundData.OnSoundPlay();
		yield break;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00015998 File Offset: 0x00013B98
	private void stopSound(string sndName)
	{
		for (int i = 0; i < this.list_SoundPlayers.Count; i++)
		{
			if (this.list_SoundPlayers[i].gameObject.activeSelf && string.Equals(this.list_SoundPlayers[i].soundEntry.name, sndName))
			{
				this.list_SoundPlayers[i].OnSoundEnd();
			}
		}
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00015A04 File Offset: 0x00013C04
	private void stopSound(int sndIndex)
	{
		for (int i = 0; i < this.list_SoundPlayers.Count; i++)
		{
			if (this.list_SoundPlayers[i].gameObject.activeSelf && this.list_SoundPlayers[i].soundIndex == sndIndex)
			{
				this.list_SoundPlayers[i].OnSoundEnd();
				return;
			}
		}
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00015A68 File Offset: 0x00013C68
	private int playMusic(string assetkeyName, string sndName, bool doFadeIn = false, float fadeTime = 0f, float fastForward = 0f, float pitch = 1f)
	{
		string name = this.CombineKeyAndClipName(assetkeyName, sndName);
		SoundEntry soundEntry;
		if (!this.audioDic.TryGetValue(Animator.StringToHash(name), out soundEntry))
		{
			Debug.LogError("找不到要播放的音效: " + sndName);
			return -1;
		}
		if (this.currentBGMData != null)
		{
			this.currentBGMData.SetFadeOut(fadeTime);
		}
		this.IncreaseSoundIndex();
		SoundPlayer avaliableSoundObj = this.GetAvaliableSoundObj();
		GameObject gameObject = avaliableSoundObj.gameObject;
		avaliableSoundObj.audioSource.clip = soundEntry.clips[Random.Range(0, soundEntry.clips.Length)];
		avaliableSoundObj.audioSource.pitch = pitch;
		avaliableSoundObj.audioSource.mute = false;
		avaliableSoundObj.soundEntry = soundEntry;
		avaliableSoundObj.fadeIn = fadeTime;
		avaliableSoundObj.fadeOut = 0f;
		avaliableSoundObj.soundEntry.soundLength = avaliableSoundObj.audioSource.clip.length;
		avaliableSoundObj.soundIndex = this.soundIndex;
		avaliableSoundObj.audioSource.loop = soundEntry.doLoop;
		avaliableSoundObj.audioSource.playOnAwake = false;
		avaliableSoundObj.mod_volume = soundEntry.mod_Volume * this.GetVolumeMultiplierByType(soundEntry.type);
		avaliableSoundObj.audioSource.priority = 99;
		if (this.audioMixer_Music != null)
		{
			avaliableSoundObj.audioSource.outputAudioMixerGroup = this.audioMixer_Music;
		}
		gameObject.name = name;
		gameObject.transform.parent = base.transform;
		gameObject.SetActive(true);
		avaliableSoundObj.audioSource.time = Mathf.Min(fastForward, avaliableSoundObj.audioSource.clip.length);
		avaliableSoundObj.audioSource.Play();
		avaliableSoundObj.OnSoundPlay();
		this.currentBGMData = avaliableSoundObj;
		return this.soundIndex;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x00015C0D File Offset: 0x00013E0D
	private void stopMusic()
	{
		if (this.currentBGMData == null)
		{
			return;
		}
		this.currentBGMData.OnSoundEnd();
		this.currentBGMData = null;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00015C30 File Offset: 0x00013E30
	private void muteMusicForSeconds(float time)
	{
		if (this.coroutine_MuteBGM != null)
		{
			base.StopCoroutine(this.coroutine_MuteBGM);
			this.coroutine_MuteBGM = null;
		}
		this.coroutine_MuteBGM = base.StartCoroutine(this.CR_MuteMusicForSeconds(time));
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00015C60 File Offset: 0x00013E60
	private IEnumerator CR_MuteMusicForSeconds(float time)
	{
		this.setMute(SoundAssetData.SOUND_TYPE.BGM, true);
		yield return new WaitForSeconds(time);
		this.setMute(SoundAssetData.SOUND_TYPE.BGM, false);
		this.coroutine_MuteBGM = null;
		yield break;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x00015C76 File Offset: 0x00013E76
	private bool isCurrentBGMName(string name)
	{
		return this.currentBGMData != null && this.currentBGMData.name != null && this.currentBGMData.name.Equals(name);
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00015CA6 File Offset: 0x00013EA6
	public float GetCurrentBGMTime()
	{
		if (this.currentBGMData == null)
		{
			return 0f;
		}
		return this.currentBGMData.audioSource.time;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00015CCC File Offset: 0x00013ECC
	public void SetCurrentBGMPitch(float pitch)
	{
		if (this.currentBGMData == null)
		{
			return;
		}
		this.currentBGMData.audioSource.pitch = pitch;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00015CEE File Offset: 0x00013EEE
	private void setMasterVolume(float soundLevel)
	{
		this.audioMixer_Master.audioMixer.SetFloat("MasterVolume", this.LinearToDecibel(soundLevel));
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00015D0D File Offset: 0x00013F0D
	private void modifySoundLevel(SoundAssetData.SOUND_TYPE soundType, float soundLevel)
	{
		switch (soundType)
		{
		case SoundAssetData.SOUND_TYPE.SOUND:
			this.volumeMultiplier_Sound = soundLevel;
			break;
		case SoundAssetData.SOUND_TYPE.BGM:
			this.volumeMultiplier_Music = soundLevel;
			break;
		case SoundAssetData.SOUND_TYPE.VOCAL:
			this.volumeMultiplier_Vocal = soundLevel;
			break;
		}
		this.UpdateVolumeSetting();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00015D46 File Offset: 0x00013F46
	private float GetVolumeMultiplierByType(SoundAssetData.SOUND_TYPE type)
	{
		switch (type)
		{
		case SoundAssetData.SOUND_TYPE.SOUND:
			return this.volumeMultiplier_Sound;
		case SoundAssetData.SOUND_TYPE.BGM:
			return this.volumeMultiplier_Music;
		case SoundAssetData.SOUND_TYPE.VOCAL:
			return this.volumeMultiplier_Vocal;
		default:
			return 1f;
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00015D78 File Offset: 0x00013F78
	private string CombineKeyAndClipName(string keyName, string clipName)
	{
		return string.Format("{0}_{1}", keyName, clipName);
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00015D88 File Offset: 0x00013F88
	private float LinearToDecibel(float linear)
	{
		float result;
		if (linear != 0f)
		{
			result = 20f * Mathf.Log10(linear);
		}
		else
		{
			result = -80f;
		}
		return result;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00015DB4 File Offset: 0x00013FB4
	private float DecibelToLinear(float dB)
	{
		float result;
		if (dB != -80f)
		{
			result = Mathf.Pow(10f, dB / 20f);
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00015DE4 File Offset: 0x00013FE4
	private void registerSoundAssetData(List<SoundAssetData> list_Data)
	{
		foreach (SoundAssetData soundAssetData in list_Data)
		{
			if (!this.list_SoundAssetData.Contains(soundAssetData))
			{
				this.list_SoundAssetData.Add(soundAssetData);
				this.LoadSoundAssetData(soundAssetData);
			}
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00015E4C File Offset: 0x0001404C
	private void unregisterSoundAssetData(List<SoundAssetData> list_Data)
	{
		foreach (SoundAssetData soundAssetData in list_Data)
		{
			if (this.list_SoundAssetData.Contains(soundAssetData))
			{
				this.list_SoundAssetData.Remove(soundAssetData);
				this.UnloadSoundAssetData(soundAssetData);
			}
		}
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00015EB8 File Offset: 0x000140B8
	public static void SetMuteAll(bool isMute)
	{
		Singleton<SoundManager>.Instance.setMuteAll(isMute);
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00015EC5 File Offset: 0x000140C5
	public static void SetMute(SoundAssetData.SOUND_TYPE soundType, bool isMute)
	{
		Singleton<SoundManager>.Instance.setMute(soundType, isMute);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00015ED3 File Offset: 0x000140D3
	public static int PlaySound_RandomPitch(string assetKeyName, string sndName, float minPitch = -1f, float maxPitch = -1f, float cooldown = -1f, float delay = -1f)
	{
		return Singleton<SoundManager>.Instance.playSound_RandomPitch(assetKeyName, sndName, minPitch, maxPitch, cooldown, delay);
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00015EE7 File Offset: 0x000140E7
	public static int PlaySound(string assetKeyName, string sndName, float pitch = -1f, float cooldown = -1f, float delay = -1f)
	{
		return Singleton<SoundManager>.Instance.playSound(assetKeyName, sndName, pitch, cooldown, delay);
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00015EF9 File Offset: 0x000140F9
	public static void StopSound(string sndName)
	{
		Singleton<SoundManager>.Instance.stopSound(sndName);
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00015F06 File Offset: 0x00014106
	public static void StopSound(int sndIndex)
	{
		Singleton<SoundManager>.Instance.stopSound(sndIndex);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00015F13 File Offset: 0x00014113
	public static int PlayMusic(string assetkeyName, string sndName, bool doFadeIn = false, float fadeTime = 0f, float fastForward = 0f, float pitch = 1f)
	{
		return Singleton<SoundManager>.Instance.playMusic(assetkeyName, sndName, doFadeIn, fadeTime, fastForward, pitch);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00015F27 File Offset: 0x00014127
	public static void StopMusic()
	{
		Singleton<SoundManager>.Instance.stopMusic();
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00015F33 File Offset: 0x00014133
	public static void MuteMusicForSeconds(float time)
	{
		Singleton<SoundManager>.Instance.muteMusicForSeconds(time);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00015F40 File Offset: 0x00014140
	public static bool IsCurrentBGMName(string name)
	{
		return Singleton<SoundManager>.Instance.isCurrentBGMName(name);
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00015F4D File Offset: 0x0001414D
	public static void SetMasterVolume(float soundLevel)
	{
		Singleton<SoundManager>.Instance.setMasterVolume(soundLevel);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00015F5A File Offset: 0x0001415A
	public static void SetVolume(SoundAssetData.SOUND_TYPE soundType, float soundLevel)
	{
		Singleton<SoundManager>.Instance.modifySoundLevel(soundType, soundLevel);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00015F68 File Offset: 0x00014168
	public static float GetVolume(SoundAssetData.SOUND_TYPE soundType)
	{
		return Singleton<SoundManager>.Instance.GetVolumeMultiplierByType(soundType);
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00015F78 File Offset: 0x00014178
	public SoundPlayer GetSoundPlayerByIndex(int sndIndex)
	{
		for (int i = 0; i < this.list_SoundPlayers.Count; i++)
		{
			if (this.list_SoundPlayers[i].gameObject.activeSelf && this.list_SoundPlayers[i].soundIndex == sndIndex)
			{
				return this.list_SoundPlayers[i];
			}
		}
		return null;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00015FD8 File Offset: 0x000141D8
	public void ToggleSoundMuteByIndex(int sndIndex, bool isMute)
	{
		SoundPlayer soundPlayerByIndex = this.GetSoundPlayerByIndex(sndIndex);
		if (soundPlayerByIndex != null)
		{
			soundPlayerByIndex.Mute(isMute);
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00015FFD File Offset: 0x000141FD
	public static void RegisterSoundAssetData(List<SoundAssetData> list_Data)
	{
		Singleton<SoundManager>.Instance.registerSoundAssetData(list_Data);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001600A File Offset: 0x0001420A
	public static void UnregisterSoundAssetData(List<SoundAssetData> list_Data)
	{
		Singleton<SoundManager>.Instance.unregisterSoundAssetData(list_Data);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00016017 File Offset: 0x00014217
	public void ToggleMuteInBackground(bool isOn)
	{
		this.doMuteInBackground = isOn;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00016020 File Offset: 0x00014220
	private void OnApplicationFocus(bool focusStatus)
	{
		if (!this.doMuteInBackground)
		{
			return;
		}
		if (focusStatus)
		{
			AudioListener.volume = 1f;
			return;
		}
		AudioListener.volume = 0f;
	}

	// Token: 0x040004F4 RID: 1268
	[Header("目前已載入的聲音設定檔")]
	public List<SoundAssetData> list_SoundAssetData;

	// Token: 0x040004F5 RID: 1269
	[Header("遊戲執行中產生的播放物件")]
	public List<SoundPlayer> list_SoundPlayers;

	// Token: 0x040004F6 RID: 1270
	[SerializeField]
	private AudioMixerGroup audioMixer_Master;

	// Token: 0x040004F7 RID: 1271
	[SerializeField]
	private AudioMixerGroup audioMixer_Sound;

	// Token: 0x040004F8 RID: 1272
	[SerializeField]
	private AudioMixerGroup audioMixer_Music;

	// Token: 0x040004F9 RID: 1273
	private Dictionary<int, SoundEntry> audioDic = new Dictionary<int, SoundEntry>();

	// Token: 0x040004FA RID: 1274
	private List<SoundCooldown> list_SoundCoolDown;

	// Token: 0x040004FB RID: 1275
	private float volumeMultiplier_Sound = 1f;

	// Token: 0x040004FC RID: 1276
	private float volumeMultiplier_Music = 1f;

	// Token: 0x040004FD RID: 1277
	private float volumeMultiplier_Vocal = 1f;

	// Token: 0x040004FE RID: 1278
	private int soundIndex;

	// Token: 0x040004FF RID: 1279
	private Coroutine coroutine_MuteBGM;

	// Token: 0x04000500 RID: 1280
	private bool isInitialized;

	// Token: 0x04000503 RID: 1283
	private bool doMuteInBackground;
}

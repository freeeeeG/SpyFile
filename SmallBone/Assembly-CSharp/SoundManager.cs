using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using FX;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x02000081 RID: 129
public sealed class SoundManager : PersistentSingleton<SoundManager>
{
	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600025E RID: 606 RVA: 0x00009CD4 File Offset: 0x00007ED4
	// (set) Token: 0x0600025F RID: 607 RVA: 0x00009CDB File Offset: 0x00007EDB
	public float masterVolume
	{
		get
		{
			return GameData.Settings.masterVolume;
		}
		set
		{
			GameData.Settings.masterVolume = value;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000260 RID: 608 RVA: 0x00009CE3 File Offset: 0x00007EE3
	// (set) Token: 0x06000261 RID: 609 RVA: 0x00009CEA File Offset: 0x00007EEA
	public bool musicEnabled
	{
		get
		{
			return GameData.Settings.musicEnabled;
		}
		set
		{
			GameData.Settings.musicEnabled = value;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000262 RID: 610 RVA: 0x00009CF2 File Offset: 0x00007EF2
	// (set) Token: 0x06000263 RID: 611 RVA: 0x00009CF9 File Offset: 0x00007EF9
	public bool sfxEnabled
	{
		get
		{
			return GameData.Settings.sfxEnabled;
		}
		set
		{
			GameData.Settings.sfxEnabled = value;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000264 RID: 612 RVA: 0x00009D01 File Offset: 0x00007F01
	// (set) Token: 0x06000265 RID: 613 RVA: 0x00009D08 File Offset: 0x00007F08
	public float musicVolume
	{
		get
		{
			return GameData.Settings.musicVolume;
		}
		set
		{
			GameData.Settings.musicVolume = value;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000266 RID: 614 RVA: 0x00009D10 File Offset: 0x00007F10
	// (set) Token: 0x06000267 RID: 615 RVA: 0x00009D17 File Offset: 0x00007F17
	public float sfxVolume
	{
		get
		{
			return GameData.Settings.sfxVolume;
		}
		set
		{
			GameData.Settings.sfxVolume = value;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000268 RID: 616 RVA: 0x00009D1F File Offset: 0x00007F1F
	public AudioClip backgroundClip
	{
		get
		{
			return this._backgroundMusic.clip;
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00009D2C File Offset: 0x00007F2C
	protected override void Awake()
	{
		base.Awake();
		if (this._backgroundMusic)
		{
			this._targetBackgroundClip = this._backgroundMusic.clip;
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00009D52 File Offset: 0x00007F52
	public void UpdateMusicVolume()
	{
		if (Service.quitting)
		{
			return;
		}
		this._backgroundMusic.volume = this.musicVolume * this.masterVolume * this._internalMusicVolume;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00009D7B File Offset: 0x00007F7B
	public float LoadBackgroundMusicTime(AudioClip clip)
	{
		if (this._playHistories.ContainsKey(clip))
		{
			return this._playHistories[clip];
		}
		return 0f;
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00009DA0 File Offset: 0x00007FA0
	public void SaveBackgroundMusicTime(AudioSource source)
	{
		if (this._playHistories.ContainsKey(source.clip))
		{
			this._playHistories[source.clip] = source.time;
			return;
		}
		this._playHistories.Add(source.clip, source.time);
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00009DEF File Offset: 0x00007FEF
	public void PlayBackgroundMusic(MusicInfo musicInfo)
	{
		this.PlayBackgroundMusic(musicInfo.audioClip, musicInfo.volume, musicInfo.fade, musicInfo.loop, musicInfo.usePlayHistory);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00009E18 File Offset: 0x00008018
	public void PlayBackgroundMusic(AudioClip music, float volume = 1f, bool fade = true, bool loop = true, bool usePlayHistory = false)
	{
		if (this._backgroundMusic == null || (this._backgroundMusic.isPlaying && this._targetBackgroundClip == music))
		{
			return;
		}
		this.ResetInternalMusicVolume();
		this._targetBackgroundClip = music;
		this._backgroundMusic.loop = loop;
		if (!this.musicEnabled)
		{
			this._backgroundMusic.clip = music;
			return;
		}
		this._fadeBackgroundMusicReference.Stop();
		this._backgroundMusic.volume = this.musicVolume * this.masterVolume * volume;
		if (fade && this._backgroundMusic.clip)
		{
			this._fadeBackgroundMusicReference = this.StartCoroutineWithReference(this.CFadeBackgroundMusic(music, 1f, usePlayHistory));
			return;
		}
		this._backgroundMusic.clip = music;
		this._backgroundMusic.Stop();
		if (usePlayHistory && this._playHistories.ContainsKey(this._backgroundMusic.clip))
		{
			this._backgroundMusic.time = this._playHistories[this._backgroundMusic.clip];
		}
		else
		{
			this._backgroundMusic.time = 0f;
		}
		this._backgroundMusic.Play();
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00009F44 File Offset: 0x00008144
	public void StopBackGroundMusic()
	{
		if (this._backgroundMusic.clip == null)
		{
			return;
		}
		if (!this._playHistories.ContainsKey(this._backgroundMusic.clip))
		{
			this._playHistories.Add(this._backgroundMusic.clip, this._backgroundMusic.time);
		}
		else
		{
			this._playHistories[this._backgroundMusic.clip] = this._backgroundMusic.time;
		}
		this._backgroundMusic.Stop();
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00009FCC File Offset: 0x000081CC
	public void RemovePlayHistory(AudioClip music)
	{
		this._playHistories.Remove(music);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00009FDB File Offset: 0x000081DB
	public void FadeOutBackgroundMusic(float fadeTime = 1f)
	{
		base.StartCoroutine(this.CFadeOutBackgroundMusic(fadeTime));
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00009FEB File Offset: 0x000081EB
	private IEnumerator CFadeOutBackgroundMusic(float fadeTime = 1f)
	{
		return this.CFadeBackgroundMusic(null, fadeTime, false);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00009FF6 File Offset: 0x000081F6
	private IEnumerator CFadeBackgroundMusic(AudioClip newClip, float fadeTime = 1f, bool usePlayHistory = false)
	{
		float t = 0f;
		do
		{
			yield return null;
			t += Time.deltaTime * 1f / fadeTime;
			this._backgroundMusic.volume = (1f - t) * this.musicVolume * this.masterVolume * this._internalMusicVolume;
		}
		while (t < 1f);
		this._backgroundMusic.volume = 0f;
		this.StopBackGroundMusic();
		if (newClip == null)
		{
			yield break;
		}
		this._backgroundMusic.clip = newClip;
		if (usePlayHistory && this._playHistories.ContainsKey(this._backgroundMusic.clip))
		{
			this._backgroundMusic.time = this._playHistories[this._backgroundMusic.clip];
		}
		else
		{
			this._backgroundMusic.time = 0f;
		}
		this._backgroundMusic.Play();
		t = 0f;
		do
		{
			yield return null;
			t += Time.deltaTime * 1f / fadeTime;
			this._backgroundMusic.volume = t * this.musicVolume * this.masterVolume;
		}
		while (t < 1f);
		this._backgroundMusic.volume = this.musicVolume * this.masterVolume;
		yield break;
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000A01C File Offset: 0x0000821C
	public ReusableAudioSource PlaySound(SoundInfo clipInfo, Vector3 position)
	{
		if (!this.sfxEnabled || clipInfo.audioClip == null)
		{
			return null;
		}
		if (Camera.main == null)
		{
			return null;
		}
		if (!this._playHistories.ContainsKey(clipInfo.audioClip))
		{
			this._playHistories.Add(clipInfo.audioClip, Time.unscaledTime);
		}
		else
		{
			if (Time.unscaledTime - this._playHistories[clipInfo.audioClip] < clipInfo.uniqueTime)
			{
				return null;
			}
			this._playHistories[clipInfo.audioClip] = Time.unscaledTime;
		}
		ReusableAudioSource component = this._audioSourcePrefab.reusable.Spawn(true).GetComponent<ReusableAudioSource>();
		position.z = Camera.main.transform.position.z;
		component.transform.position = position;
		AudioSource audioSource = component.audioSource;
		audioSource.volume = this.sfxVolume * clipInfo.volume * this.masterVolume;
		audioSource.priority = clipInfo.priority;
		audioSource.panStereo = clipInfo.stereoPan;
		audioSource.bypassEffects = clipInfo.bypassEffects;
		audioSource.bypassListenerEffects = clipInfo.bypassListenerEffects;
		audioSource.bypassReverbZones = clipInfo.bypassReverbZones;
		audioSource.loop = clipInfo.loop;
		audioSource.spatialBlend = clipInfo.spatialBlend;
		component.Play(clipInfo.audioClip, clipInfo.length);
		return component;
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000A174 File Offset: 0x00008374
	public ReusableAudioSource PlaySound(AudioClip clip, Vector3 position, float uniqueTime = 0.05f)
	{
		if (!this._playHistories.ContainsKey(clip))
		{
			this._playHistories.Add(clip, Time.unscaledTime);
		}
		else
		{
			if (Time.unscaledTime - this._playHistories[clip] < uniqueTime)
			{
				return null;
			}
			this._playHistories[clip] = Time.unscaledTime;
		}
		return this.PlaySound(clip, position);
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000A1D4 File Offset: 0x000083D4
	private ReusableAudioSource PlaySound(AudioClip sfx, Vector3 location)
	{
		if (!this.sfxEnabled)
		{
			return null;
		}
		ReusableAudioSource component = this._audioSourcePrefab.reusable.Spawn(location, true).GetComponent<ReusableAudioSource>();
		component.audioSource.volume = this.sfxVolume * this.masterVolume;
		component.Play(sfx, 0f);
		return component;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000A226 File Offset: 0x00008426
	private ReusableAudioSource ForcePlaySound(AudioClip sfx, Vector3 location)
	{
		ReusableAudioSource component = this._audioSourcePrefab.reusable.Spawn(location, true).GetComponent<ReusableAudioSource>();
		component.audioSource.volume = this.sfxVolume * this.masterVolume;
		component.Play(sfx, 0f);
		return component;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000A263 File Offset: 0x00008463
	public void PlayNarrationSound(SoundInfo info)
	{
		if (this._narrationSource == null)
		{
			Debug.Log("NarrationSource null");
		}
		if (info == null)
		{
			Debug.Log("info null");
		}
		this._narrationSource.Play(info, this.masterVolume);
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000A29C File Offset: 0x0000849C
	public void PlayNarrationSound(AudioClip clip)
	{
		this._narrationSource.Play(clip, this.masterVolume);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000A2B0 File Offset: 0x000084B0
	public void StopNarrationSound()
	{
		this._narrationSource.Stop();
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000A2BD File Offset: 0x000084BD
	public void SetInternalMusicVolume(float volume)
	{
		this._coroutine.Stop();
		this._internalMusicVolume = volume;
		this.UpdateMusicVolume();
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000A2D7 File Offset: 0x000084D7
	public void SetInternalMusicVolume(float volume, float easeTime, AnimationCurve easeCurve)
	{
		this._coroutine.Stop();
		this._coroutine = this.StartCoroutineWithReference(this.CFadeMusicVolume(volume, easeTime, easeCurve));
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000A2F9 File Offset: 0x000084F9
	private IEnumerator CFadeMusicVolume(float volume, float easeTime, AnimationCurve easeCurve)
	{
		float time = 0f;
		float startVolume = this._internalMusicVolume;
		while (time < easeTime)
		{
			yield return null;
			time += Time.unscaledDeltaTime;
			this._internalMusicVolume = Mathf.LerpUnclamped(startVolume, volume, easeCurve.Evaluate(time / easeTime));
			this.UpdateMusicVolume();
		}
		this._internalMusicVolume = volume;
		this.UpdateMusicVolume();
		yield break;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000A31D File Offset: 0x0000851D
	public void ResetInternalMusicVolume()
	{
		this.SetInternalMusicVolume(1f);
	}

	// Token: 0x04000209 RID: 521
	public const float maxMusicVolum = 1f;

	// Token: 0x0400020A RID: 522
	public const float maxSfxVolum = 1f;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	private AudioSource _backgroundMusic;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	private ReusableAudioSource _audioSourcePrefab;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	private NarrationAudioSource _narrationSource;

	// Token: 0x0400020E RID: 526
	private CoroutineReference _coroutine;

	// Token: 0x0400020F RID: 527
	private Dictionary<AudioClip, float> _playHistories = new Dictionary<AudioClip, float>();

	// Token: 0x04000210 RID: 528
	private AudioClip _targetBackgroundClip;

	// Token: 0x04000211 RID: 529
	private CoroutineReference _fadeBackgroundMusicReference;

	// Token: 0x04000212 RID: 530
	private float _internalMusicVolume = 1f;
}

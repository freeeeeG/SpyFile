using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000141 RID: 321
public class Sound : Singleton<Sound>
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06000843 RID: 2115 RVA: 0x000158DE File Offset: 0x00013ADE
	// (set) Token: 0x06000844 RID: 2116 RVA: 0x000158EB File Offset: 0x00013AEB
	public float BgVolume
	{
		get
		{
			return this.m_bgSound.volume;
		}
		set
		{
			this.m_bgSound.volume = value;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06000845 RID: 2117 RVA: 0x000158F9 File Offset: 0x00013AF9
	// (set) Token: 0x06000846 RID: 2118 RVA: 0x00015906 File Offset: 0x00013B06
	public float EffectVolume
	{
		get
		{
			return this.m_effectSound.volume;
		}
		set
		{
			this.m_effectSound.volume = value;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06000847 RID: 2119 RVA: 0x00015914 File Offset: 0x00013B14
	// (set) Token: 0x06000848 RID: 2120 RVA: 0x00015921 File Offset: 0x00013B21
	public float UIVolume
	{
		get
		{
			return this.m_UIsound.volume;
		}
		set
		{
			this.m_UIsound.volume = value;
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00015930 File Offset: 0x00013B30
	protected override void Awake()
	{
		base.Awake();
		this.InitializeMusicDIC();
		this.m_bgSound = base.gameObject.AddComponent<AudioSource>();
		this.m_bgSound.outputAudioMixerGroup = this.musicMixer;
		this.m_bgSound.playOnAwake = false;
		this.m_bgSound.loop = true;
		this.m_effectSound = base.gameObject.AddComponent<AudioSource>();
		this.m_effectSound.outputAudioMixerGroup = this.effectMixer;
		this.EffectVolume = 0.5f;
		this.m_UIsound = base.gameObject.AddComponent<AudioSource>();
		this.m_UIsound.outputAudioMixerGroup = this.effectMixer;
		this.UIVolume = 0.5f;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x000159E0 File Offset: 0x00013BE0
	private void InitializeMusicDIC()
	{
		this.BGmusic = new Dictionary<string, AudioClip>();
		this.EffectMusic = new Dictionary<string, AudioClip>();
		foreach (AudioClip audioClip in Resources.LoadAll<AudioClip>(this.ResourceDir + "/BGs"))
		{
			this.BGmusic.Add(audioClip.name, audioClip);
		}
		foreach (AudioClip audioClip2 in Resources.LoadAll<AudioClip>(this.ResourceDir + "/Effects"))
		{
			this.EffectMusic.Add(audioClip2.name, audioClip2);
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00015A78 File Offset: 0x00013C78
	public void PlayBg(string audioName)
	{
		if (!this.BGmusic.ContainsKey(audioName))
		{
			Debug.Log("使用了错误的音乐名");
			return;
		}
		if (this.m_bgSound.clip != null && audioName == this.m_bgSound.clip.name)
		{
			return;
		}
		AudioClip clip = this.BGmusic[audioName];
		if (this.m_bgSound.clip != null)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.FadeMusic(clip));
			this.canPlay = true;
			return;
		}
		this.m_bgSound.clip = clip;
		this.m_bgSound.Play();
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00015B1D File Offset: 0x00013D1D
	private IEnumerator FadeMusic(AudioClip clip)
	{
		float startVolume = 0.5f;
		while (this.m_bgSound.volume > 0f)
		{
			this.m_bgSound.volume -= startVolume * Time.deltaTime / 0.5f;
			yield return null;
		}
		this.m_bgSound.clip = clip;
		this.m_bgSound.Play();
		while (this.m_bgSound.volume <= startVolume)
		{
			this.m_bgSound.volume += startVolume * Time.deltaTime / 0.5f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00015B33 File Offset: 0x00013D33
	public void StopBg()
	{
		this.m_bgSound.Stop();
		this.m_bgSound.clip = null;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00015B4C File Offset: 0x00013D4C
	public void PlayEffect(string audioName)
	{
		if (this.canPlay)
		{
			this.canPlay = false;
			if (!this.EffectMusic.ContainsKey(audioName))
			{
				Debug.Log("使用了错误的音效名");
				return;
			}
			AudioClip clip = this.EffectMusic[audioName];
			this.m_effectSound.PlayOneShot(clip);
			base.StartCoroutine(this.Reset());
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00015BA8 File Offset: 0x00013DA8
	public void PlayUISound(string audioName)
	{
		if (!this.EffectMusic.ContainsKey(audioName))
		{
			Debug.Log("使用了错误的音效名");
			return;
		}
		AudioClip clip = this.EffectMusic[audioName];
		this.m_UIsound.PlayOneShot(clip);
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00015BE7 File Offset: 0x00013DE7
	private IEnumerator Reset()
	{
		yield return new WaitForSeconds(0.05f);
		this.canPlay = true;
		yield break;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00015BF6 File Offset: 0x00013DF6
	public void PlayEffect(AudioClip clip)
	{
		this.m_effectSound.PlayOneShot(clip, 0.5f);
	}

	// Token: 0x04000414 RID: 1044
	public AudioMixerGroup musicMixer;

	// Token: 0x04000415 RID: 1045
	public AudioMixerGroup effectMixer;

	// Token: 0x04000416 RID: 1046
	private Dictionary<string, AudioClip> BGmusic;

	// Token: 0x04000417 RID: 1047
	private Dictionary<string, AudioClip> EffectMusic;

	// Token: 0x04000418 RID: 1048
	private AudioSource m_bgSound;

	// Token: 0x04000419 RID: 1049
	private AudioSource m_effectSound;

	// Token: 0x0400041A RID: 1050
	private AudioSource m_UIsound;

	// Token: 0x0400041B RID: 1051
	public string ResourceDir = "";

	// Token: 0x0400041C RID: 1052
	private bool canPlay = true;
}

using System;
using System.Collections;
using Singletons;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class CustomAudioSource : MonoBehaviour
{
	// Token: 0x06000092 RID: 146 RVA: 0x00004082 File Offset: 0x00002282
	private void Awake()
	{
		if (this._audioSource == null)
		{
			this._audioSource = base.GetComponent<AudioSource>();
		}
		if (this._audioSource != null)
		{
			this._audioOriginVolume = this._audioSource.volume;
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x000040C0 File Offset: 0x000022C0
	public void Play()
	{
		if (!PersistentSingleton<SoundManager>.Instance.sfxEnabled)
		{
			return;
		}
		if (this._audioSource == null)
		{
			return;
		}
		float volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
		this._audioSource.volume = volume;
		this._audioSource.Play();
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00004124 File Offset: 0x00002324
	public void Stop()
	{
		this._audioSource.Stop();
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00004131 File Offset: 0x00002331
	public void FadeOut(float time = 1f)
	{
		base.StartCoroutine(this.CFadeOut(time));
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00004141 File Offset: 0x00002341
	public IEnumerator CFadeOut(float time = 1f)
	{
		float t = 0f;
		float volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
		yield return null;
		while (t < time)
		{
			this._fadeFactor = (time - t) / time;
			yield return null;
			t += Time.unscaledDeltaTime;
			volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
			this._audioSource.volume = volume;
		}
		this._fadeFactor = 0f;
		volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
		this._audioSource.volume = volume;
		yield break;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00004157 File Offset: 0x00002357
	public void FadeIn(float time = 1f)
	{
		base.StartCoroutine(this.CFadeIn(time));
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004167 File Offset: 0x00002367
	public IEnumerator CFadeIn(float time = 1f)
	{
		float t = 0f;
		float volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
		yield return null;
		while (t < time)
		{
			this._fadeFactor = (0f + t) / time;
			yield return null;
			t += Time.unscaledDeltaTime;
			volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
			this._audioSource.volume = volume;
		}
		this._fadeFactor = 1f;
		volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * this._audioOriginVolume * this._fadeFactor;
		this._audioSource.volume = volume;
		yield break;
	}

	// Token: 0x04000084 RID: 132
	[SerializeField]
	private AudioSource _audioSource;

	// Token: 0x04000085 RID: 133
	private float _audioOriginVolume;

	// Token: 0x04000086 RID: 134
	private float _fadeFactor = 1f;
}

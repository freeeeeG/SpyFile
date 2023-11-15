using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200006A RID: 106
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x000174C4 File Offset: 0x000156C4
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x000174CC File Offset: 0x000156CC
		public float MusicVolume
		{
			get
			{
				return this._musicVolume;
			}
			set
			{
				this._musicVolume = value;
				this.musicSource.volume = this._musicVolume;
				PlayerPrefs.SetFloat("MusicVolume", this._musicVolume);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000174F6 File Offset: 0x000156F6
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x000174FE File Offset: 0x000156FE
		public float SFXVolume
		{
			get
			{
				return this._sfxVolume;
			}
			set
			{
				this._sfxVolume = value;
				PlayerPrefs.SetFloat("SFXVolume", this._sfxVolume);
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00017518 File Offset: 0x00015718
		private void Awake()
		{
			if (AudioManager.Instance == null)
			{
				AudioManager.Instance = this;
			}
			else if (AudioManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
			}
			Object.DontDestroyOnLoad(base.gameObject);
			this.MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
			this.SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00017587 File Offset: 0x00015787
		public void FadeInMusic(float fadeDuration)
		{
			this.StopMusicFade();
			this._musicFadeCR = this.FadeInMusicCR(fadeDuration);
			base.StartCoroutine(this._musicFadeCR);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000175A9 File Offset: 0x000157A9
		public void FadeOutMusic(float fadeDuration)
		{
			this.StopMusicFade();
			this._musicFadeCR = this.FadeOutMusicCR(fadeDuration);
			base.StartCoroutine(this._musicFadeCR);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000175CB File Offset: 0x000157CB
		public void PlayMusic(AudioClip clip)
		{
			this.musicSource.clip = clip;
			this.musicSource.Play();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000175E4 File Offset: 0x000157E4
		public void SetLowPassFilter(bool isOn)
		{
			this.musicLowPassFilter.enabled = isOn;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000175F2 File Offset: 0x000157F2
		private void StopMusicFade()
		{
			if (this._musicFadeCR != null)
			{
				base.StopCoroutine(this._musicFadeCR);
				this._musicFadeCR = null;
				this.musicSource.volume = this._musicVolume;
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00017620 File Offset: 0x00015820
		private IEnumerator FadeInMusicCR(float fadeDuration)
		{
			this.musicSource.volume = 0f;
			while (this.musicSource.volume < this._musicVolume)
			{
				this.musicSource.volume += this._musicVolume * Time.unscaledDeltaTime / fadeDuration;
				yield return null;
			}
			this.musicSource.volume = this._musicVolume;
			this._musicFadeCR = null;
			yield break;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00017636 File Offset: 0x00015836
		private IEnumerator FadeOutMusicCR(float fadeDuration)
		{
			while (this.musicSource.volume > 0f)
			{
				this.musicSource.volume -= this._musicVolume * Time.unscaledDeltaTime / fadeDuration;
				yield return null;
			}
			this.musicSource.Stop();
			this.musicSource.volume = this._musicVolume;
			this._musicFadeCR = null;
			yield break;
		}

		// Token: 0x04000291 RID: 657
		public static AudioManager Instance;

		// Token: 0x04000292 RID: 658
		[SerializeField]
		private AudioSource musicSource;

		// Token: 0x04000293 RID: 659
		[SerializeField]
		private AudioLowPassFilter musicLowPassFilter;

		// Token: 0x04000294 RID: 660
		private float _musicVolume;

		// Token: 0x04000295 RID: 661
		private float _sfxVolume;

		// Token: 0x04000296 RID: 662
		private IEnumerator _musicFadeCR;
	}
}

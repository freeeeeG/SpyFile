using System;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x02000247 RID: 583
	public class NarrationAudioSource : PersistentSingleton<SoundManager>
	{
		// Token: 0x06000B78 RID: 2936 RVA: 0x0001F915 File Offset: 0x0001DB15
		public void Play(AudioClip clip, float masterVolume)
		{
			this._audioSource.volume = 1f * masterVolume;
			this._audioSource.clip = clip;
			this._audioSource.Play();
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0001F940 File Offset: 0x0001DB40
		public void Play(SoundInfo info, float masterVolume)
		{
			this._audioSource.clip = info.audioClip;
			this._audioSource.volume = 1f * masterVolume;
			this._audioSource.priority = info.priority;
			this._audioSource.panStereo = info.stereoPan;
			this._audioSource.bypassEffects = info.bypassEffects;
			this._audioSource.bypassListenerEffects = info.bypassListenerEffects;
			this._audioSource.bypassReverbZones = info.bypassReverbZones;
			this._audioSource.loop = info.loop;
			this._audioSource.spatialBlend = info.spatialBlend;
			this._audioSource.Play();
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0001F9F2 File Offset: 0x0001DBF2
		public void Stop()
		{
			this._audioSource.Stop();
		}

		// Token: 0x0400098C RID: 2444
		[GetComponent]
		[SerializeField]
		private AudioSource _audioSource;

		// Token: 0x0400098D RID: 2445
		private const float _narraionVolum = 1f;
	}
}

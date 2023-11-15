using System;
using System.Collections;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x02000249 RID: 585
	public class PlaySoundInfo : MonoBehaviour
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0001FA0F File Offset: 0x0001DC0F
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0001FA17 File Offset: 0x0001DC17
		public bool playing { get; private set; }

		// Token: 0x06000B80 RID: 2944 RVA: 0x0001FA20 File Offset: 0x0001DC20
		private void Awake()
		{
			if (!PersistentSingleton<SoundManager>.Instance.sfxEnabled || this._soundInfo.audioClip == null)
			{
				return;
			}
			this._audioSource = base.gameObject.AddComponent<AudioSource>();
			Vector3 position = this._audioSource.transform.position;
			position.z = Camera.main.transform.position.z;
			this._audioSource.transform.position = position;
			this._audioSource.clip = this._soundInfo.audioClip;
			this._audioSource.priority = this._soundInfo.priority;
			this._audioSource.panStereo = this._soundInfo.stereoPan;
			this._audioSource.bypassEffects = this._soundInfo.bypassEffects;
			this._audioSource.bypassListenerEffects = this._soundInfo.bypassListenerEffects;
			this._audioSource.bypassReverbZones = this._soundInfo.bypassReverbZones;
			this._audioSource.loop = this._soundInfo.loop;
			this._audioSource.spatialBlend = this._soundInfo.spatialBlend;
			this._audioSource.playOnAwake = this._playOnEnable;
			this.UpdateVolume();
			if (!this._playOnEnable)
			{
				this._audioSource.Stop();
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0001FB75 File Offset: 0x0001DD75
		private void OnEnable()
		{
			if (!this._playOnEnable)
			{
				return;
			}
			if (this._soundInfo.length == 0f && this._soundInfo.loop)
			{
				return;
			}
			base.StartCoroutine(this.CPlay());
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0001FBAD File Offset: 0x0001DDAD
		private void OnDisable()
		{
			this.playing = false;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0001FBB6 File Offset: 0x0001DDB6
		public IEnumerator CPlay()
		{
			if (this._soundInfo.audioClip == null)
			{
				Debug.LogWarning("AudioClip of PlaySoundInfo is null. Object name " + base.name + ".");
				yield break;
			}
			float length = this._soundInfo.length;
			if (this._soundInfo.length == 0f)
			{
				length = this._soundInfo.audioClip.length;
			}
			this.UpdateVolume();
			this._audioSource.Play();
			this.playing = true;
			yield return new WaitForSecondsRealtime(length);
			this._audioSource.Stop();
			this.playing = false;
			yield break;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0001FBC5 File Offset: 0x0001DDC5
		public void Play()
		{
			if (this.playing)
			{
				return;
			}
			base.StartCoroutine(this.CPlay());
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0001FBDD File Offset: 0x0001DDDD
		public void Stop()
		{
			if (!this.playing)
			{
				return;
			}
			base.StopAllCoroutines();
			this._audioSource.Stop();
			this.playing = false;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0001FC00 File Offset: 0x0001DE00
		private void UpdateVolume()
		{
			this._audioSource.volume = PersistentSingleton<SoundManager>.Instance.sfxVolume * this._soundInfo.volume * PersistentSingleton<SoundManager>.Instance.masterVolume;
		}

		// Token: 0x04000991 RID: 2449
		[Information("SoundInfo를 오브젝트풀을 이용하지 않고 현재 게임오브젝트에서 곧바로 재생합니다. AudioSource는 자동으로 생성됩니다. length가 0이고 loop이면 무한히 지속됩니다. length가 0이고 loop가 아니면 AudioClip의 length만큼 재생됩니다. 즉 딱 한 번 재생됩니다.", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private bool _playOnEnable = true;

		// Token: 0x04000992 RID: 2450
		[SerializeField]
		private SoundInfo _soundInfo;

		// Token: 0x04000993 RID: 2451
		private AudioSource _audioSource;
	}
}

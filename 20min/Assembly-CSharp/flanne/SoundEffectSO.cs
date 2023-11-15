using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200006C RID: 108
	[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
	public class SoundEffectSO : ScriptableObject
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x00017674 File Offset: 0x00015874
		public void SyncPitchAndSemitones()
		{
			if (this.useSemitones)
			{
				this.pitch.x = Mathf.Pow(SoundEffectSO.SEMITONES_TO_PITCH_CONVERSION_UNIT, (float)this.semitones.x);
				this.pitch.y = Mathf.Pow(SoundEffectSO.SEMITONES_TO_PITCH_CONVERSION_UNIT, (float)this.semitones.y);
				return;
			}
			this.semitones.x = Mathf.RoundToInt(Mathf.Log10(this.pitch.x) / Mathf.Log10(SoundEffectSO.SEMITONES_TO_PITCH_CONVERSION_UNIT));
			this.semitones.y = Mathf.RoundToInt(Mathf.Log10(this.pitch.y) / Mathf.Log10(SoundEffectSO.SEMITONES_TO_PITCH_CONVERSION_UNIT));
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00017724 File Offset: 0x00015924
		private AudioClip GetAudioClip()
		{
			AudioClip result = this.clips[(this.playIndex >= this.clips.Length) ? 0 : this.playIndex];
			switch (this.playOrder)
			{
			case SoundEffectSO.SoundClipPlayOrder.random:
				this.playIndex = Random.Range(0, this.clips.Length);
				break;
			case SoundEffectSO.SoundClipPlayOrder.in_order:
				this.playIndex = (this.playIndex + 1) % this.clips.Length;
				break;
			case SoundEffectSO.SoundClipPlayOrder.reverse:
				this.playIndex = (this.playIndex + this.clips.Length - 1) % this.clips.Length;
				break;
			}
			return result;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000177C0 File Offset: 0x000159C0
		public AudioSource Play(AudioSource audioSourceParam = null)
		{
			try
			{
				SoundEffectSO.SoundsPlaying.Add(base.name, 1);
			}
			catch (ArgumentException)
			{
				if (SoundEffectSO.SoundsPlaying[base.name] >= 10)
				{
					return null;
				}
				Dictionary<string, int> soundsPlaying = SoundEffectSO.SoundsPlaying;
				string name = base.name;
				int num = soundsPlaying[name];
				soundsPlaying[name] = num + 1;
			}
			if (this.clips.Length == 0)
			{
				Debug.LogError("Missing sound clips for " + base.name);
				return null;
			}
			AudioSource audioSource = audioSourceParam;
			if (audioSourceParam == null)
			{
				audioSource = new GameObject("Sound", new Type[]
				{
					typeof(AudioSource)
				}).GetComponent<AudioSource>();
			}
			audioSource.clip = this.GetAudioClip();
			AudioManager instance = AudioManager.Instance;
			if (instance != null)
			{
				audioSource.volume = instance.SFXVolume * Random.Range(this.volume.x, this.volume.y);
			}
			else
			{
				audioSource.volume = Random.Range(this.volume.x, this.volume.y);
			}
			audioSource.pitch = (this.useSemitones ? Mathf.Pow(SoundEffectSO.SEMITONES_TO_PITCH_CONVERSION_UNIT, (float)Random.Range(this.semitones.x, this.semitones.y)) : Random.Range(this.pitch.x, this.pitch.y));
			audioSource.Play();
			Object.Destroy(audioSource.gameObject, audioSource.clip.length / audioSource.pitch);
			this.RemoveDictionaryEntry(audioSource.clip.length / audioSource.pitch, base.name);
			return audioSource;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00017978 File Offset: 0x00015B78
		private async void RemoveDictionaryEntry(float delay, string name)
		{
			await Task.Delay(Mathf.FloorToInt(delay * 1000f)).ConfigureAwait(false);
			SoundEffectSO.SoundsPlaying[name]--;
		}

		// Token: 0x04000299 RID: 665
		private static Dictionary<string, int> SoundsPlaying = new Dictionary<string, int>();

		// Token: 0x0400029A RID: 666
		private static readonly float SEMITONES_TO_PITCH_CONVERSION_UNIT = 1.05946f;

		// Token: 0x0400029B RID: 667
		public AudioClip[] clips;

		// Token: 0x0400029C RID: 668
		public Vector2 volume = new Vector2(0.5f, 0.5f);

		// Token: 0x0400029D RID: 669
		public bool useSemitones;

		// Token: 0x0400029E RID: 670
		public Vector2Int semitones = new Vector2Int(0, 0);

		// Token: 0x0400029F RID: 671
		public Vector2 pitch = new Vector2(1f, 1f);

		// Token: 0x040002A0 RID: 672
		[SerializeField]
		private SoundEffectSO.SoundClipPlayOrder playOrder;

		// Token: 0x040002A1 RID: 673
		[SerializeField]
		private int playIndex;

		// Token: 0x0200029E RID: 670
		private enum SoundClipPlayOrder
		{
			// Token: 0x04000A5A RID: 2650
			random,
			// Token: 0x04000A5B RID: 2651
			in_order,
			// Token: 0x04000A5C RID: 2652
			reverse
		}
	}
}

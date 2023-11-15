using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000012 RID: 18
[Serializable]
public class Sound
{
	// Token: 0x060000A9 RID: 169 RVA: 0x00005277 File Offset: 0x00003477
	private IEnumerator PlayInterval(Sound sound)
	{
		sound.canPlay = false;
		yield return new WaitForSecondsRealtime(this.playIntervalTime);
		sound.canPlay = true;
		yield break;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005290 File Offset: 0x00003490
	public Sound(Sound sd)
	{
		this.clip = sd.clip;
		this.volume = sd.volume;
		this.pitch = sd.pitch;
		this.Loop_Interval = sd.Loop_Interval;
		this.Loop_Times = sd.Loop_Times;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00005304 File Offset: 0x00003504
	public Sound()
	{
		this.volume = 1f;
		this.pitch = 1f;
		this.Loop_Times = 3;
		this.Loop_Interval = 0.1f;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005364 File Offset: 0x00003564
	public Sound(AudioClip clip)
	{
		this.clip = clip;
		this.volume = 1f;
		this.pitch = 1f;
		this.Loop_Times = 1;
		this.Loop_Interval = 0.1f;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000053CC File Offset: 0x000035CC
	public void Play()
	{
		if (!Setting.Inst.Option_SoundEffectOn)
		{
			return;
		}
		if (!this.canPlay)
		{
			return;
		}
		if (this.playIntervalTime != 0f)
		{
			GameData.inst.StartCoroutine(this.PlayInterval(this));
		}
		if (this.clip == null)
		{
			Debug.LogError("缺少音效");
			return;
		}
		GameObject gameObject = new GameObject();
		AudioSource theAs = gameObject.AddComponent<AudioSource>();
		gameObject.AddComponent<SoundPlayer>().Init(theAs, this);
	}

	// Token: 0x0400010D RID: 269
	public AudioClip clip;

	// Token: 0x0400010E RID: 270
	[Range(0f, 2f)]
	public float volume = 1f;

	// Token: 0x0400010F RID: 271
	[Range(-3f, 3f)]
	public float pitch = 1f;

	// Token: 0x04000110 RID: 272
	public float Loop_Interval;

	// Token: 0x04000111 RID: 273
	public int Loop_Times = 3;

	// Token: 0x04000112 RID: 274
	public float playIntervalTime;

	// Token: 0x04000113 RID: 275
	public bool canPlay = true;

	// Token: 0x04000114 RID: 276
	public bool ifEffectByTimeScale;
}

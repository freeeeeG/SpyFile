using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
[Serializable]
public class SoundPlayer : MonoBehaviour
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000580 RID: 1408 RVA: 0x00016077 File Offset: 0x00014277
	public bool IsReserved
	{
		get
		{
			return this.isReserved;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001607F File Offset: 0x0001427F
	// (set) Token: 0x06000582 RID: 1410 RVA: 0x00016087 File Offset: 0x00014287
	public float mod_volume
	{
		get
		{
			return this.audioSourceVol;
		}
		set
		{
			this.audioSourceVol = value / 2f;
		}
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00016096 File Offset: 0x00014296
	public void Mute(bool isMute)
	{
		this.audioSource.mute = isMute;
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x000160A4 File Offset: 0x000142A4
	public void ReserveToPlay()
	{
		this.isReserved = true;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x000160AD File Offset: 0x000142AD
	public void OnSoundPlay()
	{
		this.soundTime = this.soundEntry.soundLength;
		this.audioSource.volume = this.mod_volume;
		this.isReserved = false;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x000160D8 File Offset: 0x000142D8
	public void OnSoundEnd()
	{
		base.gameObject.SetActive(false);
		this.audioSource.Stop();
		this.isReserved = false;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x000160F8 File Offset: 0x000142F8
	public void SetFadeOut(float fadeOutTime)
	{
		this.soundTime = fadeOutTime;
		this.fadeOut = fadeOutTime;
		this.audioSource.loop = false;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00016114 File Offset: 0x00014314
	public void SoundUpdate(float deltaTime)
	{
		this.soundTime -= deltaTime;
		if (this.soundTime < 0f)
		{
			this.soundTime = 0f;
		}
		if (this.soundTime <= 0f && !this.audioSource.loop)
		{
			this.OnSoundEnd();
		}
		if (this.soundEntry.soundLength - this.soundTime < this.fadeIn)
		{
			this.audioSource.volume = Mathf.Lerp(0f, this.mod_volume, (this.soundEntry.soundLength - this.soundTime) / this.fadeIn);
		}
		if (this.soundTime < this.fadeOut)
		{
			this.audioSource.volume = Mathf.Lerp(0f, this.mod_volume, this.soundTime / this.fadeOut);
		}
	}

	// Token: 0x04000504 RID: 1284
	public SoundEntry soundEntry;

	// Token: 0x04000505 RID: 1285
	public AudioSource audioSource;

	// Token: 0x04000506 RID: 1286
	public int soundIndex;

	// Token: 0x04000507 RID: 1287
	public float fadeIn;

	// Token: 0x04000508 RID: 1288
	public float fadeOut;

	// Token: 0x04000509 RID: 1289
	public float soundTime;

	// Token: 0x0400050A RID: 1290
	private float audioSourceVol = 0.5f;

	// Token: 0x0400050B RID: 1291
	private bool isReserved;
}

using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class SoundPlayer : MonoBehaviour
{
	// Token: 0x060000AE RID: 174 RVA: 0x00005440 File Offset: 0x00003640
	private void FixedUpdate()
	{
		if (this.ifEffectByTimeScale)
		{
			float num = 1f;
			this.compAS.pitch = this.originPitch * num;
			this.compAS.volume = this.originVolume * num;
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00005484 File Offset: 0x00003684
	public void Init(AudioSource theAs, Sound sd)
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this.theSound = new Sound(sd);
		this.compAS = theAs;
		this.theClip = this.theSound.clip;
		this.ifEffectByTimeScale = sd.ifEffectByTimeScale;
		this.compAS.clip = this.theClip;
		this.compAS.volume = (float)((double)this.theSound.volume * Setting.Inst.setFloats[0]);
		this.compAS.pitch = this.theSound.pitch;
		this.originPitch = this.theSound.pitch;
		this.originVolume = this.compAS.volume;
		base.StartCoroutine(this.PlayThenDestroy());
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00005547 File Offset: 0x00003747
	private IEnumerator PlayThenDestroy()
	{
		this.compAS.PlayOneShot(this.compAS.clip);
		yield return new WaitForSeconds(this.theClip.length);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000115 RID: 277
	public Sound theSound;

	// Token: 0x04000116 RID: 278
	[SerializeField]
	private AudioClip theClip;

	// Token: 0x04000117 RID: 279
	[SerializeField]
	private bool ifEffectByTimeScale;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	private float originPitch = 1f;

	// Token: 0x04000119 RID: 281
	[SerializeField]
	private float originVolume = 1f;

	// Token: 0x0400011A RID: 282
	private AudioSource compAS;
}

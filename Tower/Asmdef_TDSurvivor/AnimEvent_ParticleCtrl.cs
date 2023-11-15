using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
public class AnimEvent_ParticleCtrl : MonoBehaviour
{
	// Token: 0x06000AE2 RID: 2786 RVA: 0x00028F5C File Offset: 0x0002715C
	public void Anim_SetParticleOn(int index)
	{
		this.obj_particle[index].SetActive(true);
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00028F6C File Offset: 0x0002716C
	public void Anim_SetParticleOff(int index)
	{
		if (index >= this.obj_particle.Length)
		{
			return;
		}
		this.obj_particle[index].SetActive(false);
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00028F88 File Offset: 0x00027188
	public void Anim_PlayParticle(int index)
	{
		if (index >= this.obj_particle.Length)
		{
			return;
		}
		if (this.obj_particle[index] == null)
		{
			return;
		}
		this.obj_particle[index].SetActive(true);
		this.obj_particle[index].GetComponent<ParticleSystem>().Play(true);
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x00028FC8 File Offset: 0x000271C8
	public void Anim_StopParticleAndClear(int index)
	{
		if (index >= this.obj_particle.Length)
		{
			return;
		}
		this.obj_particle[index].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00028FEA File Offset: 0x000271EA
	public void Anim_StopParticleNoClear(int index)
	{
		if (index >= this.obj_particle.Length)
		{
			return;
		}
		this.obj_particle[index].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0002900C File Offset: 0x0002720C
	public void Anim_PlayParticleAtTransform(int index)
	{
		if (index >= this.obj_particle.Length)
		{
			return;
		}
		this.obj_particle[index].transform.position = this.obj_transform[index].transform.position;
		this.Anim_PlayParticle(index);
	}

	// Token: 0x04000850 RID: 2128
	[Header("- Anim_SetParticleOn(): 打開Particle")]
	[Header("- Anim_SetParticleOff(): 關閉Particle")]
	[Header("- Anim_PlayParticle(): 原地開始播放Particle")]
	[Header("- Anim_StopParticleAndClear(): 停止播放Particle 並且清除已經放出的粒子")]
	[Header("- Anim_StopParticleNoClear(): 停止播放Particle 不清除已經放出的粒子")]
	[Header("- Anim_PlayParticleAtTransform(): 在指定位置播放Particle")]
	[TextArea(5, 10)]
	public string note;

	// Token: 0x04000851 RID: 2129
	[Header("指定Particle物件")]
	public GameObject[] obj_particle;

	// Token: 0x04000852 RID: 2130
	[Header("指定Particle播放時要移到哪個物件的位置")]
	public GameObject[] obj_transform;
}

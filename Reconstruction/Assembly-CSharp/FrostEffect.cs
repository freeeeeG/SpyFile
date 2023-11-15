using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class FrostEffect : ReusableObject
{
	// Token: 0x060005EA RID: 1514 RVA: 0x0001031E File Offset: 0x0000E51E
	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001032C File Offset: 0x0000E52C
	public void Broke()
	{
		this.anim.SetBool("Frost", false);
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0001033F File Offset: 0x0000E53F
	public void ReclaimFrost()
	{
		Singleton<ObjectPool>.Instance.UnSpawn(this);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0001034C File Offset: 0x0000E54C
	public override void OnSpawn()
	{
		base.OnSpawn();
		this.anim.SetBool("Frost", true);
	}

	// Token: 0x04000296 RID: 662
	private Animator anim;
}

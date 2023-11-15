using System;
using UnityEngine;

// Token: 0x02000232 RID: 562
public class HealthWarning : IUserInterface
{
	// Token: 0x06000E72 RID: 3698 RVA: 0x00025120 File Offset: 0x00023320
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x00025134 File Offset: 0x00023334
	public override void Show()
	{
		base.Show();
		this.anim.SetTrigger("Show");
	}

	// Token: 0x040006E7 RID: 1767
	private Animator anim;
}

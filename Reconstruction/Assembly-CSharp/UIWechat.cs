using System;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class UIWechat : IUserInterface
{
	// Token: 0x06000FF2 RID: 4082 RVA: 0x0002AB87 File Offset: 0x00028D87
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0002AB9B File Offset: 0x00028D9B
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("isOpen", true);
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0002ABB4 File Offset: 0x00028DB4
	public override void ClosePanel()
	{
		this.anim.SetBool("isOpen", false);
	}

	// Token: 0x04000844 RID: 2116
	private Animator anim;
}

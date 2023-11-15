using System;
using UnityEngine;

// Token: 0x02000230 RID: 560
public class BossComeAnim : IUserInterface
{
	// Token: 0x06000E69 RID: 3689 RVA: 0x00024FB7 File Offset: 0x000231B7
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00024FCB File Offset: 0x000231CB
	public override void Show()
	{
		base.Show();
		this.anim.SetTrigger("BossCome");
		Singleton<Sound>.Instance.PlayUISound("Sound_Warning");
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00024FF2 File Offset: 0x000231F2
	private void FixedUpdate()
	{
		if (this.m_Active)
		{
			this.material.mainTextureOffset += this.flowSpeed * Time.deltaTime;
		}
	}

	// Token: 0x040006DF RID: 1759
	private Animator anim;

	// Token: 0x040006E0 RID: 1760
	[SerializeField]
	private Material material;

	// Token: 0x040006E1 RID: 1761
	private Vector2 flowSpeed = new Vector2(0.2f, 0f);
}

using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class GoldKeeperAnim : IUserInterface
{
	// Token: 0x06000E6D RID: 3693 RVA: 0x0002503F File Offset: 0x0002323F
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00025053 File Offset: 0x00023253
	public override void Show()
	{
		base.Show();
		this.anim.SetTrigger("Come");
		Singleton<Sound>.Instance.PlayUISound("Sound_GoldKeeper");
		base.StartCoroutine(this.BonusAnim());
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00025087 File Offset: 0x00023287
	private IEnumerator BonusAnim()
	{
		this.bonusMaterial.SetFloat("_ShineLocation", 0f);
		yield return new WaitForSeconds(1f);
		this.bonusMaterial.DOFloat(1f, "_ShineLocation", 1.5f);
		yield return new WaitForSeconds(1f);
		yield break;
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00025098 File Offset: 0x00023298
	private void FixedUpdate()
	{
		if (this.m_Active)
		{
			this.lineMaterial.mainTextureOffset += this.flowSpeed * Time.deltaTime;
			this.speedMaterial.mainTextureOffset -= this.flowSpeed * 4f * Time.deltaTime;
		}
	}

	// Token: 0x040006E2 RID: 1762
	[SerializeField]
	private Material lineMaterial;

	// Token: 0x040006E3 RID: 1763
	[SerializeField]
	private Material speedMaterial;

	// Token: 0x040006E4 RID: 1764
	[SerializeField]
	private Material bonusMaterial;

	// Token: 0x040006E5 RID: 1765
	private Animator anim;

	// Token: 0x040006E6 RID: 1766
	private Vector2 flowSpeed = new Vector2(0.2f, 0f);
}

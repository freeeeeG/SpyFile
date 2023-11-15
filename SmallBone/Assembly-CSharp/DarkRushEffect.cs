using System;
using Hardmode;
using Singletons;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class DarkRushEffect : MonoBehaviour
{
	// Token: 0x060000B2 RID: 178 RVA: 0x00004599 File Offset: 0x00002799
	public void ShowSign()
	{
		this._sign.gameObject.SetActive(true);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000045AC File Offset: 0x000027AC
	public void HideSign()
	{
		this._sign.gameObject.SetActive(false);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x000045BF File Offset: 0x000027BF
	public void SetSignEffectOrder(int order)
	{
		this._signSpriteRender.sortingOrder = order;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000045CD File Offset: 0x000027CD
	public void ShowImpact()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			this._impactHardmode.gameObject.SetActive(true);
			return;
		}
		this._impact.gameObject.SetActive(true);
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x000045FE File Offset: 0x000027FE
	public void HideImpact()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			this._impactHardmode.gameObject.SetActive(false);
			return;
		}
		this._impact.gameObject.SetActive(false);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0000462F File Offset: 0x0000282F
	public void SetImpactEffectOrder(int order)
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			this._impactSpriteRenderHardmode.sortingOrder = order;
			return;
		}
		this._impactSpriteRender.sortingOrder = order;
	}

	// Token: 0x0400009E RID: 158
	[SerializeField]
	private Transform _sign;

	// Token: 0x0400009F RID: 159
	[SerializeField]
	private SpriteRenderer _signSpriteRender;

	// Token: 0x040000A0 RID: 160
	[SerializeField]
	private Transform _impact;

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	private SpriteRenderer _impactSpriteRender;

	// Token: 0x040000A2 RID: 162
	[SerializeField]
	private Transform _impactHardmode;

	// Token: 0x040000A3 RID: 163
	[SerializeField]
	private SpriteRenderer _impactSpriteRenderHardmode;
}

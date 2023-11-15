using System;
using TMPro;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class UI_MapScene_PlayerHP : MonoBehaviour
{
	// Token: 0x060009A3 RID: 2467 RVA: 0x000243C6 File Offset: 0x000225C6
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.OnHpChanged, new Action<int>(this.OnPlayerHPChanged));
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x000243E0 File Offset: 0x000225E0
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.OnHpChanged, new Action<int>(this.OnPlayerHPChanged));
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x000243FA File Offset: 0x000225FA
	private void Start()
	{
		this.SetValue(GameDataManager.instance.GameplayData.CurHP);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00024411 File Offset: 0x00022611
	private void OnPlayerHPChanged(int value)
	{
		this.SetValue(value);
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0002441C File Offset: 0x0002261C
	private void SetValue(int value)
	{
		this.text_HPValue.text = string.Format("{0}<size=30>/{1}", value, GameDataManager.instance.GameplayData.MaxHP);
		float t = Mathf.InverseLerp(this.particleSizeFromHPRange.x, this.particleSizeFromHPRange.y, (float)value);
		this.node_Particle.transform.localScale = Vector3.Lerp(this.particleSizeRange.x * Vector3.one, this.particleSizeRange.y * Vector3.one, t);
	}

	// Token: 0x0400078A RID: 1930
	[SerializeField]
	private TMP_Text text_HPValue;

	// Token: 0x0400078B RID: 1931
	[SerializeField]
	private Transform node_Particle;

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	private Vector2 particleSizeRange;

	// Token: 0x0400078D RID: 1933
	[SerializeField]
	private Vector2 particleSizeFromHPRange;
}

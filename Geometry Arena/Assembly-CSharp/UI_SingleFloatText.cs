using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E2 RID: 226
public class UI_SingleFloatText : MonoBehaviour
{
	// Token: 0x060007DD RID: 2013 RVA: 0x0002B5A4 File Offset: 0x000297A4
	private void Start()
	{
		this.fade_LifeTimeLeft = this.fade_LifeTimeMax;
		this.UpdateColor();
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x0002B5B8 File Offset: 0x000297B8
	private void UpdateColor()
	{
		float num = 1f - this.fade_LifeTimeLeft / this.fade_LifeTimeMax;
		float value = Mathf.Sqrt(1f - num * num);
		this.text.color = this.text.color.SetAlpha(value);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x0002B604 File Offset: 0x00029804
	private void Update()
	{
		this.fade_LifeTimeLeft -= Time.unscaledDeltaTime;
		if (this.fade_LifeTimeLeft <= 0f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.UpdateColor();
	}

	// Token: 0x04000697 RID: 1687
	public Text text;

	// Token: 0x04000698 RID: 1688
	[SerializeField]
	private float fade_LifeTimeMax = 0.9f;

	// Token: 0x04000699 RID: 1689
	[SerializeField]
	private float fade_LifeTimeLeft = 0.9f;
}

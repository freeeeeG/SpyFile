using System;
using UnityEngine;

// Token: 0x02000B15 RID: 2837
public class HealthBar : ProgressBar
{
	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06005777 RID: 22391 RVA: 0x002008D8 File Offset: 0x001FEAD8
	private bool ShouldShow
	{
		get
		{
			return this.showTimer > 0f || base.PercentFull < this.alwaysShowThreshold;
		}
	}

	// Token: 0x06005778 RID: 22392 RVA: 0x002008F7 File Offset: 0x001FEAF7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
		base.gameObject.SetActive(this.ShouldShow);
	}

	// Token: 0x06005779 RID: 22393 RVA: 0x00200925 File Offset: 0x001FEB25
	public void OnChange()
	{
		base.enabled = true;
		this.showTimer = this.maxShowTime;
	}

	// Token: 0x0600577A RID: 22394 RVA: 0x0020093C File Offset: 0x001FEB3C
	public override void Update()
	{
		base.Update();
		if (Time.timeScale > 0f)
		{
			this.showTimer = Mathf.Max(0f, this.showTimer - Time.unscaledDeltaTime);
		}
		if (!this.ShouldShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600577B RID: 22395 RVA: 0x0020098B File Offset: 0x001FEB8B
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x0600577C RID: 22396 RVA: 0x00200994 File Offset: 0x001FEB94
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x0600577D RID: 22397 RVA: 0x002009A0 File Offset: 0x001FEBA0
	public override void OnOverlayChanged(object data = null)
	{
		if (!this.autoHide)
		{
			return;
		}
		if ((HashedString)data == OverlayModes.None.ID)
		{
			if (!base.gameObject.activeSelf && this.ShouldShow)
			{
				base.enabled = true;
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04003B27 RID: 15143
	private float showTimer;

	// Token: 0x04003B28 RID: 15144
	private float maxShowTime = 10f;

	// Token: 0x04003B29 RID: 15145
	private float alwaysShowThreshold = 0.8f;
}

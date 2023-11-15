using System;
using UnityEngine;

// Token: 0x02000C39 RID: 3129
public class ProgressBarSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x06006304 RID: 25348 RVA: 0x00249B17 File Offset: 0x00247D17
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x00249B1F File Offset: 0x00247D1F
	public override int GetSideScreenSortOrder()
	{
		return -10;
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x00249B23 File Offset: 0x00247D23
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IProgressBarSideScreen>() != null;
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x00249B2E File Offset: 0x00247D2E
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetObject = target.GetComponent<IProgressBarSideScreen>();
		this.RefreshBar();
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x00249B4C File Offset: 0x00247D4C
	private void RefreshBar()
	{
		this.progressBar.SetMaxValue(this.targetObject.GetProgressBarMaxValue());
		this.progressBar.SetFillPercentage(this.targetObject.GetProgressBarFillPercentage());
		this.progressBar.label.SetText(this.targetObject.GetProgressBarLabel());
		this.label.SetText(this.targetObject.GetProgressBarTitleLabel());
		this.progressBar.GetComponentInChildren<ToolTip>().SetSimpleTooltip(this.targetObject.GetProgressBarTooltip());
	}

	// Token: 0x06006309 RID: 25353 RVA: 0x00249BD1 File Offset: 0x00247DD1
	public void Render1000ms(float dt)
	{
		this.RefreshBar();
	}

	// Token: 0x0400438A RID: 17290
	public LocText label;

	// Token: 0x0400438B RID: 17291
	public GenericUIProgressBar progressBar;

	// Token: 0x0400438C RID: 17292
	public IProgressBarSideScreen targetObject;
}

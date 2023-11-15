using System;

// Token: 0x02000855 RID: 2133
public class CompositeStatus : IConnectionModeSwitchStatus
{
	// Token: 0x0600291D RID: 10525 RVA: 0x000C1112 File Offset: 0x000BF512
	public eConnectionModeSwitchProgress GetProgress()
	{
		if (this.m_TaskSubStatus == null)
		{
			return eConnectionModeSwitchProgress.NotStarted;
		}
		if (this.bFinalTask && this.m_TaskSubStatus.GetProgress() != eConnectionModeSwitchProgress.NotStarted)
		{
			return this.m_TaskSubStatus.GetProgress();
		}
		return eConnectionModeSwitchProgress.InProgress;
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000C1149 File Offset: 0x000BF549
	public string GetLocalisedProgressDescription()
	{
		if (this.m_TaskSubStatus != null)
		{
			return this.m_TaskSubStatus.GetLocalisedProgressDescription();
		}
		return Localization.Get("Online.ConnectionMode.Progress.NotStarted", new LocToken[0]);
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x000C1172 File Offset: 0x000BF572
	public eConnectionModeSwitchResult GetResult()
	{
		if (this.m_TaskSubStatus == null)
		{
			return eConnectionModeSwitchResult.NotAvailableYet;
		}
		if (this.bFinalTask)
		{
			return this.m_TaskSubStatus.GetResult();
		}
		return eConnectionModeSwitchResult.NotAvailableYet;
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000C1199 File Offset: 0x000BF599
	public string GetLocalisedResultDescription()
	{
		if (this.m_TaskSubStatus != null)
		{
			return this.m_TaskSubStatus.GetLocalisedResultDescription();
		}
		return string.Empty;
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000C11B7 File Offset: 0x000BF5B7
	public virtual bool DisplayPlatformDialog()
	{
		return this.m_TaskSubStatus != null && this.m_TaskSubStatus.DisplayPlatformDialog();
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000C11D4 File Offset: 0x000BF5D4
	public virtual IConnectionModeSwitchStatus Clone()
	{
		return new CompositeStatus
		{
			m_TaskSubStatus = this.m_TaskSubStatus.Clone(),
			bFinalTask = this.bFinalTask
		};
	}

	// Token: 0x04002095 RID: 8341
	public IConnectionModeSwitchStatus m_TaskSubStatus;

	// Token: 0x04002096 RID: 8342
	public bool bFinalTask;
}

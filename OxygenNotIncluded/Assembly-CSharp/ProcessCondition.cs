using System;

// Token: 0x020008F2 RID: 2290
public abstract class ProcessCondition
{
	// Token: 0x06004259 RID: 16985
	public abstract ProcessCondition.Status EvaluateCondition();

	// Token: 0x0600425A RID: 16986
	public abstract bool ShowInUI();

	// Token: 0x0600425B RID: 16987
	public abstract string GetStatusMessage(ProcessCondition.Status status);

	// Token: 0x0600425C RID: 16988 RVA: 0x00172F16 File Offset: 0x00171116
	public string GetStatusMessage()
	{
		return this.GetStatusMessage(this.EvaluateCondition());
	}

	// Token: 0x0600425D RID: 16989
	public abstract string GetStatusTooltip(ProcessCondition.Status status);

	// Token: 0x0600425E RID: 16990 RVA: 0x00172F24 File Offset: 0x00171124
	public string GetStatusTooltip()
	{
		return this.GetStatusTooltip(this.EvaluateCondition());
	}

	// Token: 0x0600425F RID: 16991 RVA: 0x00172F32 File Offset: 0x00171132
	public virtual StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		return null;
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x00172F35 File Offset: 0x00171135
	public virtual ProcessCondition GetParentCondition()
	{
		return this.parentCondition;
	}

	// Token: 0x04002B51 RID: 11089
	protected ProcessCondition parentCondition;

	// Token: 0x02001746 RID: 5958
	public enum ProcessConditionType
	{
		// Token: 0x04006E20 RID: 28192
		RocketFlight,
		// Token: 0x04006E21 RID: 28193
		RocketPrep,
		// Token: 0x04006E22 RID: 28194
		RocketStorage,
		// Token: 0x04006E23 RID: 28195
		RocketBoard,
		// Token: 0x04006E24 RID: 28196
		All
	}

	// Token: 0x02001747 RID: 5959
	public enum Status
	{
		// Token: 0x04006E26 RID: 28198
		Failure,
		// Token: 0x04006E27 RID: 28199
		Warning,
		// Token: 0x04006E28 RID: 28200
		Ready
	}
}

using System;

// Token: 0x020001FA RID: 506
public class None : Element
{
	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00020ECC File Offset: 0x0001F0CC
	public override string GetSkillText
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06000CAF RID: 3247 RVA: 0x00020ED3 File Offset: 0x0001F0D3
	public override string GetElementName
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00020EDA File Offset: 0x0001F0DA
	public override string GetExtraInfo
	{
		get
		{
			return "";
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x00020EE1 File Offset: 0x0001F0E1
	public override string ElementColor
	{
		get
		{
			return "#7CF7FF";
		}
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00020EE8 File Offset: 0x0001F0E8
	public override void GetComIntensify(StrategyBase strategy, bool add = true)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00020EEF File Offset: 0x0001F0EF
	public override string GetIntensifyText(string value)
	{
		throw new NotImplementedException();
	}
}

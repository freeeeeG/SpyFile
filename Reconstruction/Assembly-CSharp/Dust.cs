using System;

// Token: 0x020001FF RID: 511
public class Dust : Element
{
	// Token: 0x06000CD1 RID: 3281 RVA: 0x00021101 File Offset: 0x0001F301
	public override string GetIntensifyText(string value)
	{
		return "+<b>" + base.Colorized(value) + "</b>" + GameMultiLang.GetTraduction("SPUTTERINGUP");
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x00021123 File Offset: 0x0001F323
	public override string ElementColor
	{
		get
		{
			return "#E84BA3";
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0002112A File Offset: 0x0001F32A
	public override string GetExtraInfo
	{
		get
		{
			return string.Format(GameMultiLang.GetTraduction("FOLLOW"), 4);
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x00021141 File Offset: 0x0001F341
	public override string GetSkillText
	{
		get
		{
			return "\n" + GameRes.GameDustIntensify.ToString() + GameMultiLang.GetTraduction("SPUTTERINGUP");
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00021161 File Offset: 0x0001F361
	public override string GetElementName
	{
		get
		{
			return "E";
		}
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x00021168 File Offset: 0x0001F368
	public override void GetComIntensify(StrategyBase strategy, bool add = true)
	{
	}
}

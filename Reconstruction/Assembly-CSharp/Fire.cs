using System;

// Token: 0x020001FE RID: 510
public class Fire : Element
{
	// Token: 0x06000CCA RID: 3274 RVA: 0x0002107A File Offset: 0x0001F27A
	public override string GetIntensifyText(string value)
	{
		return "+<b>" + base.Colorized(value) + "</b>" + GameMultiLang.GetTraduction("CRITICALUP");
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0002109C File Offset: 0x0001F29C
	public override string ElementColor
	{
		get
		{
			return "#F7173A";
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06000CCC RID: 3276 RVA: 0x000210A3 File Offset: 0x0001F2A3
	public override string GetExtraInfo
	{
		get
		{
			return string.Format(GameMultiLang.GetTraduction("FOLLOW"), 3);
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06000CCD RID: 3277 RVA: 0x000210BC File Offset: 0x0001F2BC
	public override string GetSkillText
	{
		get
		{
			return "\n" + (GameRes.GameFireIntensify * 100f).ToString() + GameMultiLang.GetTraduction("CRITICALUP");
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000210F0 File Offset: 0x0001F2F0
	public override string GetElementName
	{
		get
		{
			return "D";
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x000210F7 File Offset: 0x0001F2F7
	public override void GetComIntensify(StrategyBase strategy, bool add = true)
	{
	}
}

using System;

// Token: 0x020001FD RID: 509
public class Water : Element
{
	// Token: 0x06000CC3 RID: 3267 RVA: 0x00021009 File Offset: 0x0001F209
	public override string GetIntensifyText(string value)
	{
		return "+<b>" + base.Colorized(value) + "</b>" + GameMultiLang.GetTraduction("SLOWUP");
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0002102B File Offset: 0x0001F22B
	public override string ElementColor
	{
		get
		{
			return "#00B7FF";
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00021032 File Offset: 0x0001F232
	public override string GetExtraInfo
	{
		get
		{
			return string.Format(GameMultiLang.GetTraduction("FOLLOW"), 2);
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00021049 File Offset: 0x0001F249
	public override string GetSkillText
	{
		get
		{
			return "\n" + GameRes.GameWaterIntensify.ToString() + GameMultiLang.GetTraduction("SLOWUP");
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00021069 File Offset: 0x0001F269
	public override string GetElementName
	{
		get
		{
			return "C";
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x00021070 File Offset: 0x0001F270
	public override void GetComIntensify(StrategyBase strategy, bool add = true)
	{
	}
}

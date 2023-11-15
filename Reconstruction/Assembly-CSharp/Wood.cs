using System;

// Token: 0x020001FC RID: 508
public class Wood : Element
{
	// Token: 0x06000CBC RID: 3260 RVA: 0x00020F84 File Offset: 0x0001F184
	public override string GetIntensifyText(string value)
	{
		return "+<b>" + base.Colorized(value) + "</b>" + GameMultiLang.GetTraduction("SPEEDUP");
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00020FA6 File Offset: 0x0001F1A6
	public override string ElementColor
	{
		get
		{
			return "#62C751";
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00020FAD File Offset: 0x0001F1AD
	public override string GetExtraInfo
	{
		get
		{
			return string.Format(GameMultiLang.GetTraduction("FOLLOW"), 1);
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00020FC4 File Offset: 0x0001F1C4
	public override string GetSkillText
	{
		get
		{
			return "\n" + (GameRes.GameWoodIntensify * 100f).ToString() + GameMultiLang.GetTraduction("SPEEDUP");
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00020FF8 File Offset: 0x0001F1F8
	public override string GetElementName
	{
		get
		{
			return "B";
		}
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x00020FFF File Offset: 0x0001F1FF
	public override void GetComIntensify(StrategyBase strategy, bool add = true)
	{
	}
}

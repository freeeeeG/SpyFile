using System;

// Token: 0x020001FB RID: 507
public class Gold : Element
{
	// Token: 0x06000CB5 RID: 3253 RVA: 0x00020EFE File Offset: 0x0001F0FE
	public override string GetIntensifyText(string value)
	{
		return "+<b>" + base.Colorized(value) + "</b>" + GameMultiLang.GetTraduction("ATTACKUP");
	}

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00020F20 File Offset: 0x0001F120
	public override string GetExtraInfo
	{
		get
		{
			return string.Format(GameMultiLang.GetTraduction("FOLLOW"), 0);
		}
	}

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00020F38 File Offset: 0x0001F138
	public override string GetSkillText
	{
		get
		{
			return "\n" + (GameRes.GameGoldIntensify * 100f).ToString() + GameMultiLang.GetTraduction("ATTACKUP");
		}
	}

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00020F6C File Offset: 0x0001F16C
	public override string GetElementName
	{
		get
		{
			return "A";
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00020F73 File Offset: 0x0001F173
	public override string ElementColor
	{
		get
		{
			return "#FFE766";
		}
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x00020F7A File Offset: 0x0001F17A
	public override void GetComIntensify(StrategyBase strategy, bool add = true)
	{
	}
}

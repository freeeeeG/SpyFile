using System;

// Token: 0x020001F9 RID: 505
public abstract class Element
{
	// Token: 0x06000CA6 RID: 3238
	public abstract string GetIntensifyText(string value);

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06000CA7 RID: 3239
	public abstract string GetSkillText { get; }

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06000CA8 RID: 3240
	public abstract string GetElementName { get; }

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06000CA9 RID: 3241
	public abstract string GetExtraInfo { get; }

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06000CAA RID: 3242
	public abstract string ElementColor { get; }

	// Token: 0x06000CAB RID: 3243 RVA: 0x00020E92 File Offset: 0x0001F092
	public string Colorized(string text)
	{
		return string.Concat(new string[]
		{
			"<color=",
			this.ElementColor,
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x06000CAC RID: 3244
	public abstract void GetComIntensify(StrategyBase strategy, bool add = true);
}

using System;
using UnityEngine;

// Token: 0x02000AE5 RID: 2789
public class DateTime : KScreen
{
	// Token: 0x060055D4 RID: 21972 RVA: 0x001F37A9 File Offset: 0x001F19A9
	public static void DestroyInstance()
	{
		global::DateTime.Instance = null;
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x001F37B1 File Offset: 0x001F19B1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::DateTime.Instance = this;
	}

	// Token: 0x060055D6 RID: 21974 RVA: 0x001F37BF File Offset: 0x001F19BF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnComplexToolTip = new ToolTip.ComplexTooltipDelegate(SaveGame.Instance.GetColonyToolTip);
	}

	// Token: 0x060055D7 RID: 21975 RVA: 0x001F37E2 File Offset: 0x001F19E2
	private void Update()
	{
		if (GameClock.Instance != null && this.displayedDayCount != GameUtil.GetCurrentCycle())
		{
			this.text.text = this.Days();
			this.displayedDayCount = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x001F381C File Offset: 0x001F1A1C
	private string Days()
	{
		return GameUtil.GetCurrentCycle().ToString();
	}

	// Token: 0x04003998 RID: 14744
	public static global::DateTime Instance;

	// Token: 0x04003999 RID: 14745
	public LocText day;

	// Token: 0x0400399A RID: 14746
	private int displayedDayCount = -1;

	// Token: 0x0400399B RID: 14747
	[SerializeField]
	private LocText text;

	// Token: 0x0400399C RID: 14748
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x0400399D RID: 14749
	[SerializeField]
	private TextStyleSetting tooltipstyle_Days;

	// Token: 0x0400399E RID: 14750
	[SerializeField]
	private TextStyleSetting tooltipstyle_Playtime;

	// Token: 0x0400399F RID: 14751
	[SerializeField]
	public KToggle scheduleToggle;
}

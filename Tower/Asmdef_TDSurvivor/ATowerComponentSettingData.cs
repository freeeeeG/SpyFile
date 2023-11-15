using System;

// Token: 0x02000039 RID: 57
public abstract class ATowerComponentSettingData : AItemSettingData
{
	// Token: 0x06000117 RID: 279 RVA: 0x000053DD File Offset: 0x000035DD
	public override string GetLocNameString(bool isPrefix = true)
	{
		this.loc_Name = LocalizationManager.Instance.GetString("TowerType", this.itemType.ToString(), Array.Empty<object>());
		return this.loc_Name;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00005410 File Offset: 0x00003610
	public override string GetLocFlavorTextString()
	{
		this.loc_FlavorText = "";
		if (LocalizationManager.Instance.HasString("TowerDescription", this.itemType.ToString()))
		{
			this.loc_FlavorText = this.loc_FlavorText + "<color=#ff9933>" + LocalizationManager.Instance.GetString("TowerDescription", this.itemType.ToString(), Array.Empty<object>()) + "</color>\n";
		}
		return this.loc_FlavorText;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00005490 File Offset: 0x00003690
	public override string GetLocStatsString()
	{
		string @string = LocalizationManager.Instance.GetString("UI", "TOWER_COST", Array.Empty<object>());
		string text = string.Format("{0} {1}</color>\n\n", @string, base.GetBuildCost(1f));
		for (int i = 0; i < this.list_Stats.Count; i++)
		{
			if (this.list_Stats[i].StatType != eStatType.VISION_RANGE)
			{
				switch (this.list_Stats[i].StatType)
				{
				default:
				{
					string text2;
					if (this.list_Stats[i].IsModified)
					{
						text2 = this.list_Stats[i].GetFinalValueText_Detailed();
					}
					else
					{
						text2 = this.list_Stats[i].GetFinalValueText_Combined();
					}
					text += "<color=#eecc33>";
					text += LocalizationManager.Instance.GetString("TowerStats", this.list_Stats[i].StatType.ToString(), new object[]
					{
						text2
					});
					text += "\n";
					break;
				}
				}
			}
		}
		return text;
	}
}

using System;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class LevelInfoPanel : IUserInterface
{
	// Token: 0x06000EA8 RID: 3752 RVA: 0x00025DBC File Offset: 0x00023FBC
	public override void Initialize()
	{
		base.Initialize();
		this.SetInfo();
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x00025DCC File Offset: 0x00023FCC
	public void SetInfo()
	{
		for (int i = 0; i < this.qualitySlots.Length; i++)
		{
			float num = StaticData.QualityChances[GameRes.SystemLevel - 1, i];
			float chanceAfter = num;
			if (GameRes.SystemLevel < Singleton<StaticData>.Instance.SystemMaxLevel)
			{
				chanceAfter = StaticData.QualityChances[GameRes.SystemLevel, i];
			}
			this.qualitySlots[i].SetSlotInfo(i + 1, num, chanceAfter);
		}
	}

	// Token: 0x0400070A RID: 1802
	[SerializeField]
	private QualitySlot[] qualitySlots;
}

using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028A RID: 650
public class BossTips : IUserInterface
{
	// Token: 0x06001019 RID: 4121 RVA: 0x0002B1D8 File Offset: 0x000293D8
	public void ReadSequenceInfo(EnemyType bossType, int comingWave)
	{
		this.bossGrid.SetEnemyInfo(Singleton<StaticData>.Instance.EnemyFactory.Get(bossType));
		this.ComingTxt.text = GameMultiLang.GetTraduction("BOSSCOMING") + comingWave.ToString() + GameMultiLang.GetTraduction("WAVE");
	}

	// Token: 0x0400085D RID: 2141
	[SerializeField]
	private Text ComingTxt;

	// Token: 0x0400085E RID: 2142
	[SerializeField]
	private EnemyGrid bossGrid;
}

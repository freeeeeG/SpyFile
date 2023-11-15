using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023E RID: 574
public class EnemyGrid : MonoBehaviour
{
	// Token: 0x06000EC0 RID: 3776 RVA: 0x00026958 File Offset: 0x00024B58
	public void SetEnemyInfo(EnemyAttribute attribute)
	{
		this.enemyIcon.sprite = attribute.Icon;
		this.enemyName.text = GameMultiLang.GetTraduction(attribute.Name);
		this.enemyDes.text = GameMultiLang.GetTraduction(attribute.Name + "INFO");
		this.attBars[0].SetAtt(attribute.HealthAtt);
		this.attBars[1].SetAtt(attribute.SpeedAtt);
		this.attBars[2].SetAtt(attribute.AmountAtt);
		this.attBars[3].SetAtt(attribute.ReachDamage);
		this.bossIcon.SetActive(attribute.IsBoss);
	}

	// Token: 0x04000723 RID: 1827
	[SerializeField]
	private Image enemyIcon;

	// Token: 0x04000724 RID: 1828
	[SerializeField]
	private Text enemyName;

	// Token: 0x04000725 RID: 1829
	[SerializeField]
	private Text enemyDes;

	// Token: 0x04000726 RID: 1830
	[SerializeField]
	private GameObject bossIcon;

	// Token: 0x04000727 RID: 1831
	[SerializeField]
	private EnemyGrid_Attbar[] attBars;
}

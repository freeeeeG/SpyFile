using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200026D RID: 621
public class BossSlot : MonoBehaviour
{
	// Token: 0x06000F6D RID: 3949 RVA: 0x000294E1 File Offset: 0x000276E1
	public void SetBossInfo(EnemyAttribute attribute, int pass, int turn)
	{
		this.bossIcon.sprite = attribute.Icon;
		this.turnTxt.text = GameMultiLang.GetTraduction("NUM") + turn.ToString() + GameMultiLang.GetTraduction("WAVE");
	}

	// Token: 0x040007DB RID: 2011
	[SerializeField]
	private Image bossIcon;

	// Token: 0x040007DC RID: 2012
	[SerializeField]
	private Text turnTxt;
}

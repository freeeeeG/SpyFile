using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000269 RID: 617
public class BillboardItem : MonoBehaviour
{
	// Token: 0x06000F5F RID: 3935 RVA: 0x00028F4C File Offset: 0x0002714C
	public void SetContent(int rank, string name, int score, bool isWave)
	{
		if (rank <= 3)
		{
			this.medalImg.gameObject.SetActive(true);
			this.medalImg.sprite = this.medalSprites[rank - 1];
			this.rank_Txt.gameObject.SetActive(false);
		}
		else
		{
			this.rank_Txt.text = rank.ToString();
			this.rank_Txt.gameObject.SetActive(true);
			this.medalImg.gameObject.SetActive(false);
		}
		this.name_Txt.text = name;
		this.score_Txt.text = score.ToString() + (isWave ? GameMultiLang.GetTraduction("WAVE") : "");
	}

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	private Text rank_Txt;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	private Text name_Txt;

	// Token: 0x040007C7 RID: 1991
	[SerializeField]
	private Text score_Txt;

	// Token: 0x040007C8 RID: 1992
	[SerializeField]
	private Image medalImg;

	// Token: 0x040007C9 RID: 1993
	[SerializeField]
	private Sprite[] medalSprites;
}

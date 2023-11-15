using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D9 RID: 217
public class EnemyColorManager : MonoBehaviour
{
	// Token: 0x06000789 RID: 1929 RVA: 0x0002A022 File Offset: 0x00028222
	private void OnEnable()
	{
		if (TempData.inst == null || Battle.inst == null)
		{
			this.objInOpen.SetActive(false);
			return;
		}
		this.GetPosX();
		this.UpdateOutlook();
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0002A054 File Offset: 0x00028254
	public void MouseOn()
	{
		Color levelColor = Color.HSVToRGB(Mathf.Clamp((Input.mousePosition.x - this.graphStartX) / (this.graphEndX - this.graphStartX), 0f, 0.998f), 0.8f, 0.9f);
		Battle.inst.levelColor = levelColor;
		if (SceneObj.inst != null)
		{
			SceneObj.inst.ApplyColorSoon();
		}
		this.UpdateOutlook();
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0002A0CC File Offset: 0x000282CC
	public void GetPosX()
	{
		float x = this.rectColorGraph.position.x;
		float num = this.rectColorGraph.sizeDelta.x * this.rectColorGraph.localScale.x * this.rectCanvas.localScale.x;
		this.graphStartX = x - num / 2f;
		this.graphEndX = x + num / 2f;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0002A13C File Offset: 0x0002833C
	private void UpdateOutlook()
	{
		this.transOpenFlag.gameObject.SetActive(Battle.inst.DIY_EnemyColor);
		this.textName.text = LanguageText.Inst.diy.customizeEnemyColor;
		EnumLanguage language = Setting.Inst.language;
		if (language == EnumLanguage.CHINESE_SIM)
		{
			this.textName.fontSize = 39;
		}
		else
		{
			this.textName.fontSize = 25;
		}
		if (Battle.inst.DIY_EnemyColor)
		{
			this.objInOpen.SetActive(true);
			float y = this.rectTriangle.position.y;
			float num = Battle.inst.levelColor.Hue();
			float x = this.graphStartX + (this.graphEndX - this.graphStartX) * num;
			this.rectTriangle.position = new Vector2(x, y);
			this.imageTriangle.color = Battle.inst.levelColor;
			return;
		}
		this.objInOpen.SetActive(false);
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0002A231 File Offset: 0x00028431
	public void Button_Click()
	{
		Battle.inst.DIY_EnemyColor = !Battle.inst.DIY_EnemyColor;
		this.UpdateOutlook();
	}

	// Token: 0x04000654 RID: 1620
	[SerializeField]
	private Text textName;

	// Token: 0x04000655 RID: 1621
	[SerializeField]
	private RectTransform transOpenFlag;

	// Token: 0x04000656 RID: 1622
	[SerializeField]
	private RectTransform rectTriangle;

	// Token: 0x04000657 RID: 1623
	[SerializeField]
	private Image imageTriangle;

	// Token: 0x04000658 RID: 1624
	[SerializeField]
	private GameObject objInOpen;

	// Token: 0x04000659 RID: 1625
	[SerializeField]
	private RectTransform rectCanvas;

	// Token: 0x0400065A RID: 1626
	[Header("AboutColorGraph")]
	[SerializeField]
	private float graphStartX;

	// Token: 0x0400065B RID: 1627
	[SerializeField]
	private float graphEndX;

	// Token: 0x0400065C RID: 1628
	[SerializeField]
	private RectTransform rectColorGraph;
}

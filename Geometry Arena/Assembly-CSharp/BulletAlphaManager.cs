using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D7 RID: 215
public class BulletAlphaManager : MonoBehaviour
{
	// Token: 0x0600077E RID: 1918 RVA: 0x00029D92 File Offset: 0x00027F92
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

	// Token: 0x0600077F RID: 1919 RVA: 0x00029DC4 File Offset: 0x00027FC4
	public void MouseOn()
	{
		float num = (Input.mousePosition.x - this.graphStartX) / (this.graphEndX - this.graphStartX);
		num = Mathf.Clamp(num, 0f, 1f);
		Battle.inst.DIY_BulletAlpha_Float = num * this.maxAlpha;
		this.UpdateOutlook();
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00029E20 File Offset: 0x00028020
	public void GetPosX()
	{
		float x = this.rectColorGraph.position.x;
		float num = this.rectColorGraph.sizeDelta.x * this.rectColorGraph.localScale.x * this.rectCanvas.localScale.x;
		this.graphStartX = x - num / 2f;
		this.graphEndX = x + num / 2f;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00029E90 File Offset: 0x00028090
	private void UpdateOutlook()
	{
		this.transOpenFlag.gameObject.SetActive(Battle.inst.DIY_BulletAlpha_BoolFlag);
		this.textName.text = LanguageText.Inst.diy.customizeBulletAlpha;
		EnumLanguage language = Setting.Inst.language;
		if (language == EnumLanguage.CHINESE_SIM)
		{
			this.textName.fontSize = 39;
		}
		else
		{
			this.textName.fontSize = 25;
		}
		if (Battle.inst.DIY_BulletAlpha_BoolFlag)
		{
			this.objInOpen.SetActive(true);
			Battle.inst.DIY_BulletAlpha_Float = Mathf.Min(this.maxAlpha, Battle.inst.DIY_BulletAlpha_Float);
			float y = this.rectTriangle.position.y;
			float num = Battle.inst.DIY_BulletAlpha_Float / this.maxAlpha;
			float x = this.graphStartX + (this.graphEndX - this.graphStartX) * num;
			this.rectTriangle.position = new Vector2(x, y);
			this.imageColorGraph.color = Player.inst.unit.mainColor;
			this.imageTriangle.color = Color.white;
			return;
		}
		this.objInOpen.SetActive(false);
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00029FBE File Offset: 0x000281BE
	public void Button_Click()
	{
		Battle.inst.DIY_BulletAlpha_BoolFlag = !Battle.inst.DIY_BulletAlpha_BoolFlag;
		this.UpdateOutlook();
	}

	// Token: 0x04000647 RID: 1607
	[SerializeField]
	private Text textName;

	// Token: 0x04000648 RID: 1608
	[SerializeField]
	private RectTransform transOpenFlag;

	// Token: 0x04000649 RID: 1609
	[SerializeField]
	private RectTransform rectTriangle;

	// Token: 0x0400064A RID: 1610
	[SerializeField]
	private Image imageTriangle;

	// Token: 0x0400064B RID: 1611
	[SerializeField]
	private GameObject objInOpen;

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private RectTransform rectCanvas;

	// Token: 0x0400064D RID: 1613
	[Header("AboutColorGraph")]
	[SerializeField]
	private float graphStartX;

	// Token: 0x0400064E RID: 1614
	[SerializeField]
	private float graphEndX;

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private RectTransform rectColorGraph;

	// Token: 0x04000650 RID: 1616
	[SerializeField]
	private Image imageColorGraph;

	// Token: 0x04000651 RID: 1617
	[Header("Settings")]
	[SerializeField]
	private float maxAlpha = 0.09f;
}

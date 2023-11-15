using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D5 RID: 213
public class AutoRunManager : MonoBehaviour
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000771 RID: 1905 RVA: 0x00029AA9 File Offset: 0x00027CA9
	// (set) Token: 0x06000772 RID: 1906 RVA: 0x00029AB5 File Offset: 0x00027CB5
	private bool theFlag
	{
		get
		{
			return BattleManager.inst.autoRun_Flag;
		}
		set
		{
			BattleManager.inst.autoRun_Flag = value;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00029AC2 File Offset: 0x00027CC2
	private void OnEnable()
	{
		if (TempData.inst == null || Battle.inst == null)
		{
			return;
		}
		if (TempData.inst.modeType != EnumModeType.ENDLESS)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.GetPosX();
		this.UpdateOutlook();
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00029B00 File Offset: 0x00027D00
	public void MouseOn()
	{
		float num = (Input.mousePosition.x - this.graphStartX) / (this.graphEndX - this.graphStartX);
		num = Mathf.Clamp(num, 0f, 1f);
		this.curInterval = num * (this.maxInterval - this.minInterval) + this.minInterval;
		BattleManager.inst.autoRun_IntervalMax = this.curInterval;
		this.UpdateOutlook();
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00029B78 File Offset: 0x00027D78
	public void GetPosX()
	{
		float x = this.rectGraph.position.x;
		float num = this.rectGraph.sizeDelta.x * this.rectGraph.localScale.x * this.rectCanvas.localScale.x;
		this.graphStartX = x - num / 2f;
		this.graphEndX = x + num / 2f;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00029BE8 File Offset: 0x00027DE8
	private void UpdateOutlook()
	{
		this.transOpenFlag.gameObject.SetActive(BattleManager.inst.autoRun_Flag);
		this.textName.text = LanguageText.Inst.diy.endlessAutoRun;
		EnumLanguage language = Setting.Inst.language;
		if (language == EnumLanguage.CHINESE_SIM)
		{
			this.textName.fontSize = 39;
		}
		else
		{
			this.textName.fontSize = 25;
		}
		if (this.theFlag)
		{
			this.objInOpen.SetActive(true);
			BattleManager.inst.autoRun_IntervalMax = this.curInterval;
			float y = this.rectTriangle.position.y;
			float num = (this.curInterval - this.minInterval) / (this.maxInterval - this.minInterval);
			float x = this.graphStartX + (this.graphEndX - this.graphStartX) * num;
			this.rectTriangle.position = new Vector2(x, y);
			this.imageTriangle.color = Color.white;
			this.textTime.text = this.curInterval.ToString();
			return;
		}
		this.objInOpen.SetActive(false);
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00029D09 File Offset: 0x00027F09
	public void Button_Click()
	{
		this.theFlag = !this.theFlag;
		if (this.theFlag)
		{
			BattleManager.inst.autoRun_CountDown = 3f;
		}
		this.UpdateOutlook();
	}

	// Token: 0x04000638 RID: 1592
	[SerializeField]
	private Text textName;

	// Token: 0x04000639 RID: 1593
	[SerializeField]
	private RectTransform transOpenFlag;

	// Token: 0x0400063A RID: 1594
	[SerializeField]
	private RectTransform rectTriangle;

	// Token: 0x0400063B RID: 1595
	[SerializeField]
	private Image imageTriangle;

	// Token: 0x0400063C RID: 1596
	[SerializeField]
	private GameObject objInOpen;

	// Token: 0x0400063D RID: 1597
	[SerializeField]
	private RectTransform rectCanvas;

	// Token: 0x0400063E RID: 1598
	[SerializeField]
	private Text textTime;

	// Token: 0x0400063F RID: 1599
	[Header("AboutColorGraph")]
	[SerializeField]
	private float graphStartX;

	// Token: 0x04000640 RID: 1600
	[SerializeField]
	private float graphEndX;

	// Token: 0x04000641 RID: 1601
	[SerializeField]
	private RectTransform rectGraph;

	// Token: 0x04000642 RID: 1602
	[Header("Settings")]
	[SerializeField]
	private float minInterval = 1f;

	// Token: 0x04000643 RID: 1603
	[SerializeField]
	private float maxInterval = 3f;

	// Token: 0x04000644 RID: 1604
	[SerializeField]
	private float curInterval = 3f;
}

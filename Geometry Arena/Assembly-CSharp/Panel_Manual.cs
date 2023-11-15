using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BA RID: 186
public class Panel_Manual : MonoBehaviour
{
	// Token: 0x06000671 RID: 1649 RVA: 0x00024CDD File Offset: 0x00022EDD
	public void OpenInMain(int dataID)
	{
		this.Open(true, dataID);
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x00024CE8 File Offset: 0x00022EE8
	public void Open(bool ifMain, int dataID)
	{
		this.ifMain = ifMain;
		if (ifMain)
		{
			MainCanvas.ChildPanelOpen();
		}
		base.gameObject.SetActive(true);
		this.guidesInOrder = DataSelector.GetGuidesInOrder();
		int orderID = -1;
		for (int i = 0; i < this.guidesInOrder.Length; i++)
		{
			if (this.guidesInOrder[i].dataID == dataID)
			{
				orderID = i;
				break;
			}
		}
		this.GoToOrderIndex(orderID);
		this.UpdateLanguage();
		this.iconSkipTutorial.UpdateShow();
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00024D5C File Offset: 0x00022F5C
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		this.textTitle.text = inst.manual.title;
		this.textSkipTutorial.text = inst.manual.skipTutorial;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.textSkipTutorial.rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectSkipTutorial);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00024DB8 File Offset: 0x00022FB8
	public void GoToOrderIndex(int orderID)
	{
		int num = this.guidesInOrder.Length - 1;
		if (orderID < 0)
		{
			orderID = num;
		}
		if (orderID > num)
		{
			orderID = 0;
		}
		this.indexTotal = orderID;
		this.page = orderID / this.rows;
		this.textPageNum.text = (this.page + 1).ToString();
		this.indexInPage = orderID % this.rows;
		Guide guide = this.guidesInOrder[orderID];
		this.textGuideName.text = guide.Language_Name;
		string text = guide.Language_Info;
		if (text.Contains("ssEndlessInfo"))
		{
			text = text.Replace("ssEndlessInfo", LanguageText.Inst.main_Modes.mode_Singles[1].info).ReplaceLineBreak();
		}
		if (text.Contains("ssWanderInfo"))
		{
			text = text.Replace("ssWanderInfo", LanguageText.Inst.main_Modes.mode_Singles[2].info).ReplaceLineBreak();
		}
		if (text.Contains("ssDailyInfo"))
		{
			text = text.Replace("ssDailyInfo", LanguageText.Inst.main_Modes.mode_Singles[3].info).ReplaceLineBreak();
		}
		this.textInfo.text = text;
		this.InitIcons();
		this.selectOutline.transform.position = new Vector2(this.selectOutline.transform.position.x, this.listIcons[this.indexInPage].transform.position.y);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00024F3A File Offset: 0x0002313A
	public void Close()
	{
		if (this.ifMain)
		{
			MainCanvas.ChildPanelClose();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00024F58 File Offset: 0x00023158
	private void Update()
	{
		this.inputHoldDetectLeft -= Time.deltaTime;
		if (MyInput.IfGetCloseButtonDown())
		{
			this.Close();
		}
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.PageAdd(-1);
			this.GetKeyDown_Common();
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.PageAdd(1);
			this.GetKeyDown_Common();
		}
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.GoToOrderIndex(this.indexTotal - 1);
			this.GetKeyDown_Common();
		}
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.GoToOrderIndex(this.indexTotal + 1);
			this.GetKeyDown_Common();
		}
		if (this.inputHoldDetectLeft <= 0f)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				this.PageAdd(-1);
				this.GetKeyHold_Common();
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				this.PageAdd(1);
				this.GetKeyHold_Common();
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				this.GoToOrderIndex(this.indexTotal - 1);
				this.GetKeyHold_Common();
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				this.GoToOrderIndex(this.indexTotal + 1);
				this.GetKeyHold_Common();
			}
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x000250C0 File Offset: 0x000232C0
	private void GetKeyDown_Common()
	{
		SoundEffects.Inst.ui_ButtonEnter.PlayRandom();
		this.inputHoldDetectLeft = 0.3f;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x000250DC File Offset: 0x000232DC
	private void GetKeyHold_Common()
	{
		SoundEffects.Inst.ui_ButtonEnter.PlayRandom();
		this.inputHoldDetectLeft = 0.06f;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000250F8 File Offset: 0x000232F8
	public void PageAdd(int i)
	{
		this.page += i;
		int num = this.guidesInOrder.Length / this.rows;
		if (this.page > num)
		{
			this.page = 0;
		}
		if (this.page < 0)
		{
			this.page = num;
		}
		if (!this.IfAvailable(this.indexTotal))
		{
			this.GoToOrderIndex(this.page * this.rows);
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00025164 File Offset: 0x00023364
	protected int IconNum()
	{
		return this.guidesInOrder.Length;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0002516E File Offset: 0x0002336E
	protected bool IfAvailable(int ID)
	{
		return ID >= this.page * this.rows && ID <= this.page * this.rows + this.rows - 1;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x000251A0 File Offset: 0x000233A0
	public void InitIcons()
	{
		UI_ToolTip.inst.TryClose();
		foreach (GameObject gameObject in this.listIcons)
		{
			Object.Destroy(gameObject.gameObject);
		}
		this.listIcons = new List<GameObject>();
		int num = 0;
		for (int i = 0; i < this.IconNum(); i++)
		{
			if (this.IfAvailable(i))
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.prefab_SingleTitle);
				this.listIcons.Add(gameObject2.gameObject);
				gameObject2.transform.SetParent(this.trans_TitleParents);
				this.InitSingleIcon(gameObject2, i);
				num++;
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.trans_TitleParents);
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0002526C File Offset: 0x0002346C
	protected void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_GuideSingleTitle>().Init(this.guidesInOrder[ID].dataID, ID, this);
	}

	// Token: 0x04000557 RID: 1367
	[SerializeField]
	private int page;

	// Token: 0x04000558 RID: 1368
	[SerializeField]
	[CustomLabel("当前选中的orderID")]
	public int indexTotal;

	// Token: 0x04000559 RID: 1369
	[SerializeField]
	[CustomLabel("当前选中在本页位置")]
	private int indexInPage;

	// Token: 0x0400055A RID: 1370
	[SerializeField]
	private int rows = 20;

	// Token: 0x0400055B RID: 1371
	[SerializeField]
	private GameObject prefab_SingleTitle;

	// Token: 0x0400055C RID: 1372
	[SerializeField]
	private RectTransform trans_TitleParents;

	// Token: 0x0400055D RID: 1373
	[SerializeField]
	private Guide[] guidesInOrder;

	// Token: 0x0400055E RID: 1374
	[SerializeField]
	private GameObject selectOutline;

	// Token: 0x0400055F RID: 1375
	[SerializeField]
	private List<GameObject> listIcons = new List<GameObject>();

	// Token: 0x04000560 RID: 1376
	[SerializeField]
	private bool ifMain;

	// Token: 0x04000561 RID: 1377
	[Header("Texts")]
	[SerializeField]
	private Text textPageNum;

	// Token: 0x04000562 RID: 1378
	[SerializeField]
	private Text textGuideName;

	// Token: 0x04000563 RID: 1379
	[SerializeField]
	private Text textInfo;

	// Token: 0x04000564 RID: 1380
	[Header("Languages")]
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000565 RID: 1381
	[SerializeField]
	private Text textSkipTutorial;

	// Token: 0x04000566 RID: 1382
	[Header("UI")]
	[SerializeField]
	private float inputHoldDetectLeft;

	// Token: 0x04000567 RID: 1383
	[SerializeField]
	private RectTransform rectSkipTutorial;

	// Token: 0x04000568 RID: 1384
	[SerializeField]
	private UI_Icon_Setting iconSkipTutorial;
}

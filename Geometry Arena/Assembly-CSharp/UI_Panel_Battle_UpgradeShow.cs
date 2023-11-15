using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B1 RID: 177
public class UI_Panel_Battle_UpgradeShow : UI_Panel_Main_IconList
{
	// Token: 0x06000620 RID: 1568 RVA: 0x00023512 File Offset: 0x00021712
	private void Awake()
	{
		this.upgradesInOrder = DataSelector.GetUpgradesInOrder();
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0002351F File Offset: 0x0002171F
	protected override int IconNum()
	{
		return DataBase.Inst.Data_Upgrades.Length;
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002352D File Offset: 0x0002172D
	protected override bool IfAvailable(int ID)
	{
		return Battle.inst.ForShow_UpgradeNum[this.upgradesInOrder[ID].id] >= 1;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002354D File Offset: 0x0002174D
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_BattleUpgradeList>().Init(ID);
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002355B File Offset: 0x0002175B
	protected int StartIndex()
	{
		return this.page * this.pageNum;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002356A File Offset: 0x0002176A
	protected int EndIndex()
	{
		return this.page * this.pageNum + this.pageNum;
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00023580 File Offset: 0x00021780
	public override void InitIcons(Transform transformParent = null)
	{
		this.UpdatePage();
		foreach (GameObject gameObject in this.listIcons)
		{
			Object.Destroy(gameObject.gameObject);
		}
		this.listIcons = new List<GameObject>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.IconNum(); i++)
		{
			if (this.IfAvailable(i))
			{
				if (num >= this.StartIndex() && num < this.EndIndex())
				{
					int num3 = num2 % this.columnNum;
					int num4 = num2 / this.columnNum;
					Vector2 b = new Vector2(this.distX * (float)num3, this.distY * (float)num4);
					Vector2 v = this.startPos + b;
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.prefab_Icon);
					this.listIcons.Add(gameObject2.gameObject);
					gameObject2.transform.SetParent(base.transform);
					gameObject2.transform.localPosition = v;
					this.InitSingleIcon(gameObject2, this.upgradesInOrder[i].id);
					num2++;
				}
				num++;
			}
		}
		this.textPage.text = (this.page + 1).ToString() + "/" + (this.MaxPage() + 1).ToString();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rect);
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x00023708 File Offset: 0x00021908
	private int GetTotalShowNum()
	{
		int num = 0;
		for (int i = 0; i < this.IconNum(); i++)
		{
			if (this.IfAvailable(i))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00023736 File Offset: 0x00021936
	public void PagePre()
	{
		this.page--;
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00023753 File Offset: 0x00021953
	public void PageNext()
	{
		this.page++;
		this.UpdatePage();
		this.InitIcons(null);
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00023770 File Offset: 0x00021970
	private void UpdatePage()
	{
		if (this.page > this.MaxPage())
		{
			this.page = 0;
		}
		if (this.page < 0)
		{
			this.page = this.MaxPage();
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0002379C File Offset: 0x0002199C
	public int MaxPage()
	{
		return (this.GetTotalShowNum() - 1) / this.pageNum;
	}

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	private int page;

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private int pageNum = 20;

	// Token: 0x0400051D RID: 1309
	[SerializeField]
	private Text textPage;

	// Token: 0x0400051E RID: 1310
	[SerializeField]
	private RectTransform rect;

	// Token: 0x0400051F RID: 1311
	private Upgrade[] upgradesInOrder;
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C4 RID: 196
public class UI_Panel_Main_LibraryBattleItem : UI_Panel_Main_IconList
{
	// Token: 0x060006BF RID: 1727 RVA: 0x0002604B File Offset: 0x0002424B
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.InitIcons(null);
		this.UpdateLanguage();
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x00008887 File Offset: 0x00006A87
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x00026068 File Offset: 0x00024268
	public override void InitIcons(Transform transformParent = null)
	{
		UI_ToolTip.inst.TryClose();
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
				int rank = (int)DataBase.Inst.Data_BattleBuffs[i].rank;
				if (rank > num2)
				{
					num = 0;
					num2 = rank;
				}
				int num3 = num % this.columnNum;
				Vector2 b = new Vector2(this.distX * (float)num3, this.distY * (float)rank);
				Vector2 v = this.startPos + b;
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.prefab_Icon);
				gameObject2.transform.SetParent(this.transSizer);
				this.listIcons.Add(gameObject2.gameObject);
				gameObject2.transform.localPosition = v;
				this.InitSingleIcon(gameObject2, i);
				num++;
			}
		}
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002619C File Offset: 0x0002439C
	private void UpdateLanguage()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.BattleItemLibrary battleItemLibrary = inst.battleItemLibrary;
		this.textMultiTip.text = battleItemLibrary.multiTip;
		this.textManualTip.text = battleItemLibrary.manualTip.ReplaceLineBreak();
		for (int i = 0; i < 4; i++)
		{
			this.textRanks[i].text = inst.ranks[i];
		}
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x000261FE File Offset: 0x000243FE
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_BattleBuff>().InitWithID(ID, DataBase.Inst.Data_BattleBuffs[ID]);
		Object.Destroy(obj.GetComponent<UIButtonSoundTrigger>());
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00026223 File Offset: 0x00024423
	protected override bool IfAvailable(int ID)
	{
		return DataBase.Inst.Data_BattleBuffs[ID].ifAvailable;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00026236 File Offset: 0x00024436
	protected override int IconNum()
	{
		return DataBase.Inst.Data_BattleBuffs.Length;
	}

	// Token: 0x04000593 RID: 1427
	[SerializeField]
	private Text textMultiTip;

	// Token: 0x04000594 RID: 1428
	[SerializeField]
	private Text textManualTip;

	// Token: 0x04000595 RID: 1429
	[SerializeField]
	private Text[] textRanks;

	// Token: 0x04000596 RID: 1430
	[SerializeField]
	private Transform transSizer;
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A7 RID: 167
public class UI_Panel_Battle_BattleBuffsShow : MonoBehaviour
{
	// Token: 0x060005CE RID: 1486 RVA: 0x000215CE File Offset: 0x0001F7CE
	private void Awake()
	{
		UI_Panel_Battle_BattleBuffsShow.inst = this;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x000215D8 File Offset: 0x0001F7D8
	public void NewBuffIcon(int typeID, BattleBuff buff)
	{
		for (int i = 0; i < this.listIconBattleBuff.Count; i++)
		{
			if (this.listIconBattleBuff[i].typeID == typeID)
			{
				this.listIconBattleBuff[i].AddNewLayer();
				return;
			}
		}
		UI_Icon_BattleBuff component = Object.Instantiate<GameObject>(this.prefabIconBattleBuff, base.transform).GetComponent<UI_Icon_BattleBuff>();
		component.InitWithID(typeID, buff);
		this.listIconBattleBuff.Add(component);
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0002164C File Offset: 0x0001F84C
	public void RefreshTime(int typeID)
	{
		for (int i = 0; i < this.listIconBattleBuff.Count; i++)
		{
			UI_Icon_BattleBuff ui_Icon_BattleBuff = this.listIconBattleBuff[i];
			if (ui_Icon_BattleBuff.typeID == typeID)
			{
				ui_Icon_BattleBuff.RefreshTimeMax();
			}
		}
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0002168C File Offset: 0x0001F88C
	private void Update()
	{
		if (this.listIconBattleBuff.Count <= 0)
		{
			return;
		}
		foreach (UI_Icon_BattleBuff ui_Icon_BattleBuff in this.listIconBattleBuff)
		{
			if (ui_Icon_BattleBuff.buff.lifeTimeMax != -1f)
			{
				ui_Icon_BattleBuff.LifeFadeInUpdate();
				if (ui_Icon_BattleBuff.BuffLayer == 0 || ui_Icon_BattleBuff.timeLefts[ui_Icon_BattleBuff.timeLefts.Count - 1] < 0f)
				{
					if (ui_Icon_BattleBuff.typeID == 225)
					{
						this.RemoveBuffFromBattleManager(225);
					}
					this.listIconBattleBuff.Remove(ui_Icon_BattleBuff);
					Object.Destroy(ui_Icon_BattleBuff.gameObject);
					LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.GetComponent<RectTransform>());
					break;
				}
			}
		}
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00021770 File Offset: 0x0001F970
	public void RemoveBuffFromUpgradeWithID(int upID)
	{
		List<UI_Icon_BattleBuff> list = new List<UI_Icon_BattleBuff>();
		foreach (UI_Icon_BattleBuff ui_Icon_BattleBuff in this.listIconBattleBuff)
		{
			BattleBuff buff = ui_Icon_BattleBuff.buff;
			if (buff.source == BattleBuff.EnumBuffSource.UPGRADE && buff.typeID == upID)
			{
				list.Add(ui_Icon_BattleBuff);
			}
		}
		foreach (UI_Icon_BattleBuff ui_Icon_BattleBuff2 in list)
		{
			this.listIconBattleBuff.Remove(ui_Icon_BattleBuff2);
			Object.Destroy(ui_Icon_BattleBuff2.gameObject);
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00021834 File Offset: 0x0001FA34
	private void RemoveBuffFromBattleManager(int upID)
	{
		List<BattleBuff> listBattleBuffs = BattleManager.inst.listBattleBuffs;
		foreach (BattleBuff battleBuff in listBattleBuffs)
		{
			if (battleBuff.typeID == upID)
			{
				listBattleBuffs.Remove(battleBuff);
				break;
			}
		}
	}

	// Token: 0x040004C8 RID: 1224
	public static UI_Panel_Battle_BattleBuffsShow inst;

	// Token: 0x040004C9 RID: 1225
	[SerializeField]
	private GameObject prefabIconBattleBuff;

	// Token: 0x040004CA RID: 1226
	[SerializeField]
	private List<UI_Icon_BattleBuff> listIconBattleBuff = new List<UI_Icon_BattleBuff>();
}

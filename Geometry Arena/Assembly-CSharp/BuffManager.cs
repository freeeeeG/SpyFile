using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000017 RID: 23
public static class BuffManager
{
	// Token: 0x06000111 RID: 273 RVA: 0x00007FBC File Offset: 0x000061BC
	public static BattleBuff NewOrChangeBuffFromUpgrade(int upID, float timeMax, FactorMultis multis)
	{
		List<BattleBuff> listBattleBuffs = BattleManager.inst.listBattleBuffs;
		bool flag = false;
		for (int i = 0; i < listBattleBuffs.Count; i++)
		{
			BattleBuff battleBuff = listBattleBuffs[i];
			if (battleBuff.source == BattleBuff.EnumBuffSource.UPGRADE && battleBuff.typeID == upID)
			{
				battleBuff.factorMultis = multis;
				battleBuff.lifeTimeMax = timeMax;
				return battleBuff;
			}
		}
		BattleBuff battleBuff2 = null;
		if (!flag)
		{
			Upgrade up = DataBase.Inst.Data_Upgrades[upID];
			battleBuff2 = new BattleBuff();
			battleBuff2.SourceFromUpgrade(up);
		}
		if (battleBuff2 == null)
		{
			Debug.LogError("Error:Buff=Null!");
			return null;
		}
		battleBuff2.factorMultis = multis;
		battleBuff2.lifeTimeMax = timeMax;
		BattleManager.inst.listBattleBuffs.Add(battleBuff2);
		UI_Panel_Battle_BattleBuffsShow.inst.NewBuffIcon(upID, battleBuff2);
		return battleBuff2;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00008074 File Offset: 0x00006274
	public static BattleBuff NewBuffFromUpgrade_CanRepeat(int upID, float timeMax, FactorMultis multis)
	{
		List<BattleBuff> listBattleBuffs = BattleManager.inst.listBattleBuffs;
		Upgrade up = DataBase.Inst.Data_Upgrades[upID];
		BattleBuff battleBuff = new BattleBuff();
		battleBuff.SourceFromUpgrade(up);
		battleBuff.factorMultis = multis;
		battleBuff.lifeTimeMax = timeMax;
		BattleManager.inst.StartCoroutine(battleBuff.BattleBuffAddAndRemove());
		UI_Panel_Battle_BattleBuffsShow.inst.NewBuffIcon(upID, battleBuff);
		return battleBuff;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000080D4 File Offset: 0x000062D4
	public static void DeleteBuffFromUpgrade(int upID)
	{
		List<BattleBuff> listBattleBuffs = BattleManager.inst.listBattleBuffs;
		List<BattleBuff> list = new List<BattleBuff>();
		bool flag = false;
		for (int i = 0; i < listBattleBuffs.Count; i++)
		{
			BattleBuff battleBuff = listBattleBuffs[i];
			if (battleBuff.source == BattleBuff.EnumBuffSource.UPGRADE && battleBuff.typeID == upID)
			{
				list.Add(battleBuff);
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		foreach (BattleBuff item in list)
		{
			listBattleBuffs.Remove(item);
		}
		UI_Panel_Battle_BattleBuffsShow.inst.RemoveBuffFromUpgradeWithID(upID);
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00008180 File Offset: 0x00006380
	public static void UpgradeBuff_Perfect()
	{
		if (Battle.inst.specialEffect[36] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(220);
			return;
		}
		Player inst = Player.inst;
		if (inst.unit.life != (double)inst.LifeMax)
		{
			BuffManager.DeleteBuffFromUpgrade(220);
			return;
		}
		float[] buffFacs = DataBase.Inst.Data_Upgrades[220].buffFacs;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[buffFacs[1].RoundToInt()] = buffFacs[2];
		BuffManager.DeleteBuffFromUpgrade(220);
		BuffManager.NewOrChangeBuffFromUpgrade(220, -1f, factorMultis);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00008218 File Offset: 0x00006418
	public static void UpgradeBuff_Rage()
	{
		if (Battle.inst.specialEffect[40] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(223);
			return;
		}
		Player inst = Player.inst;
		if (inst.unit.life == (double)inst.LifeMax)
		{
			BuffManager.DeleteBuffFromUpgrade(223);
			return;
		}
		float num = (float)inst.unit.life / (float)inst.LifeMax;
		float[] buffFacs = DataBase.Inst.Data_Upgrades[223].buffFacs;
		float num2 = (1f - num) / buffFacs[1] * buffFacs[4] + 1f;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[buffFacs[3].RoundToInt()] = num2;
		BuffManager.DeleteBuffFromUpgrade(223);
		BuffManager.NewOrChangeBuffFromUpgrade(223, -1f, factorMultis);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000082DC File Offset: 0x000064DC
	public static void UpgradeBuff_Puffiness()
	{
		int num = 227;
		int num2 = 49;
		if (Battle.inst.specialEffect[num2] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(num);
			return;
		}
		Player inst = Player.inst;
		if (inst.unit.life == (double)inst.LifeMax)
		{
			BuffManager.DeleteBuffFromUpgrade(num);
			return;
		}
		float num3 = (float)inst.unit.life / (float)inst.LifeMax;
		float[] buffFacs = DataBase.Inst.Data_Upgrades[num].buffFacs;
		float num4 = (1f - num3) / buffFacs[1] * buffFacs[4] + 1f;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[buffFacs[3].RoundToInt()] = num4;
		BuffManager.DeleteBuffFromUpgrade(num);
		BuffManager.NewOrChangeBuffFromUpgrade(num, -1f, factorMultis);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00008398 File Offset: 0x00006598
	public static void UpgradeBuff_SurvivalInstinct()
	{
		int num = 224;
		int num2 = 41;
		if (Battle.inst.specialEffect[num2] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(num);
			return;
		}
		if (Player.inst.unit.life != 1.0)
		{
			BuffManager.DeleteBuffFromUpgrade(num);
			return;
		}
		float[] buffFacs = DataBase.Inst.Data_Upgrades[num].buffFacs;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[buffFacs[1].RoundToInt()] = buffFacs[2];
		factorMultis.factorMultis[buffFacs[3].RoundToInt()] = buffFacs[4];
		BuffManager.DeleteBuffFromUpgrade(num);
		BuffManager.NewOrChangeBuffFromUpgrade(num, -1f, factorMultis);
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00008434 File Offset: 0x00006634
	public static void UpgradeBuff_Watcher(bool ifMoving)
	{
		if (Battle.inst.specialEffect[37] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(221);
			return;
		}
		if (ifMoving)
		{
			BuffManager.DeleteBuffFromUpgrade(221);
			return;
		}
		float[] buffFacs = DataBase.Inst.Data_Upgrades[221].buffFacs;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[buffFacs[1].RoundToInt()] = buffFacs[2];
		factorMultis.factorMultis[buffFacs[3].RoundToInt()] = buffFacs[4];
		BuffManager.DeleteBuffFromUpgrade(221);
		BuffManager.NewOrChangeBuffFromUpgrade(221, -1f, factorMultis);
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000084C8 File Offset: 0x000066C8
	public static void UpgradeBuff_Scout(bool ifMoving)
	{
		if (Battle.inst.specialEffect[43] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(226);
			return;
		}
		if (!ifMoving)
		{
			BuffManager.DeleteBuffFromUpgrade(226);
			return;
		}
		float[] buffFacs = DataBase.Inst.Data_Upgrades[226].buffFacs;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[buffFacs[1].RoundToInt()] = buffFacs[2];
		BuffManager.DeleteBuffFromUpgrade(226);
		BuffManager.NewOrChangeBuffFromUpgrade(226, -1f, factorMultis);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00008548 File Offset: 0x00006748
	public static void UpgradeBuff_Revenge()
	{
		int num = 225;
		int num2 = 42;
		if (Battle.inst.specialEffect[num2] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(num);
			return;
		}
		List<BattleBuff> listBattleBuffs = BattleManager.inst.listBattleBuffs;
		BattleBuff battleBuff = null;
		bool flag = false;
		foreach (BattleBuff battleBuff2 in listBattleBuffs)
		{
			if (battleBuff2.GetBool_IfFromUpdate(num))
			{
				battleBuff = battleBuff2;
				flag = true;
				break;
			}
		}
		float[] buffFacs = DataBase.Inst.Data_Upgrades[num].buffFacs;
		if (flag)
		{
			FactorMultis factorMultis = battleBuff.factorMultis;
			battleBuff.layerThis++;
			UI_Panel_Battle_BattleBuffsShow.inst.RefreshTime(num);
			return;
		}
		FactorMultis factorMultis2 = new FactorMultis();
		factorMultis2.factorMultis[4] = buffFacs[2];
		BuffManager.NewOrChangeBuffFromUpgrade(num, buffFacs[0], factorMultis2);
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00008628 File Offset: 0x00006828
	public static void UpgradeBuff_Justice()
	{
		int num = 228;
		int num2 = 83;
		if (Battle.inst.specialEffect[num2] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(num);
			return;
		}
		List<BattleBuff> listBattleBuffs = BattleManager.inst.listBattleBuffs;
		BattleBuff battleBuff = null;
		bool flag = false;
		foreach (BattleBuff battleBuff2 in listBattleBuffs)
		{
			if (battleBuff2.GetBool_IfFromUpdate(num))
			{
				battleBuff = battleBuff2;
				flag = true;
				break;
			}
		}
		float[] buffFacs = DataBase.Inst.Data_Upgrades[num].buffFacs;
		if (flag)
		{
			FactorMultis factorMultis = battleBuff.factorMultis;
			battleBuff.layerThis++;
			return;
		}
		FactorMultis factorMultis2 = new FactorMultis();
		factorMultis2.factorMultis[10] += buffFacs[1];
		factorMultis2.factorMultis[11] += buffFacs[3];
		BuffManager.NewOrChangeBuffFromUpgrade(num, -1f, factorMultis2);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000871C File Offset: 0x0000691C
	public static void CritTransform()
	{
		int upID = 118;
		int num = 82;
		if (Battle.inst.specialEffect[num] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(upID);
			return;
		}
		Factor factorTotalNew = Player.inst.unit.FactorTotalNew;
		if (factorTotalNew.critChc <= 1f)
		{
			BuffManager.DeleteBuffFromUpgrade(upID);
			return;
		}
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[11] = (factorTotalNew.critChc - 1f) * 2f;
		BuffManager.NewOrChangeBuffFromUpgrade(upID, -1f, factorMultis);
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00008798 File Offset: 0x00006998
	public static void Titan()
	{
		int upID = 229;
		int num = 86;
		if (Battle.inst.specialEffect[num] <= 0)
		{
			BuffManager.DeleteBuffFromUpgrade(upID);
			return;
		}
		Factor factorTotalNew = Player.inst.unit.FactorTotalNew;
		FactorMultis factorMultis = new FactorMultis();
		factorMultis.factorMultis[4] = (float)factorTotalNew.lifeMaxPlayer * 0.1f + factorTotalNew.bodySize * 0.5f + 1f;
		BuffManager.NewOrChangeBuffFromUpgrade(upID, -1f, factorMultis);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00008810 File Offset: 0x00006A10
	public static void RefreshAllStateBuff()
	{
		BuffManager.RefreshStateBuff_AboutLife();
		bool ifMoving = !(Player.inst == null) && Player.inst.ifMoving;
		BuffManager.RefreshStateBuff_AboutMove(ifMoving);
		BuffManager.RefreshStateBuff_AboutStats();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00008849 File Offset: 0x00006A49
	public static void RefreshStateBuff_AboutMove(bool ifMoving)
	{
		BuffManager.UpgradeBuff_Watcher(ifMoving);
		BuffManager.UpgradeBuff_Scout(ifMoving);
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00008857 File Offset: 0x00006A57
	public static void RefreshStateBuff_AboutLife()
	{
		BuffManager.UpgradeBuff_Perfect();
		BuffManager.UpgradeBuff_Rage();
		BuffManager.UpgradeBuff_Puffiness();
		BuffManager.UpgradeBuff_SurvivalInstinct();
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000886D File Offset: 0x00006A6D
	public static void RefreshStateBuff_AboutStats()
	{
		BuffManager.CritTransform();
		BuffManager.Titan();
	}
}

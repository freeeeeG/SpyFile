using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class Player_6_Cannon : Player
{
	// Token: 0x06000398 RID: 920 RVA: 0x000051D0 File Offset: 0x000033D0
	protected override void DetectInput_Skill()
	{
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00016800 File Offset: 0x00014A00
	protected override void SkillInFixedUpdate()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			return;
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(2).facs;
		this.module_Grenadier_TimeLeft -= Time.fixedDeltaTime;
		if (this.module_Grenadier_TimeLeft <= 0f)
		{
			this.module_Grenadier_TimeLeft = facs[0];
			SpecialEffects.ShootBulletOnce_Grenade(true);
			if (TempData.inst.jobId == 6 && TempData.inst.GetBool_SkillModuleOpenFlag(4) && (Player.inst != null & Player.inst.unit != null))
			{
				Player.inst.unit.HurtSelf(1, true);
			}
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x000168AC File Offset: 0x00014AAC
	public override void UpdateFactorTotal(bool ifTrue = true)
	{
		this.unit.playerFactorTotal = this.unit.FactorBasic * Battle.inst.GetFactorMultis_Upgrates_CurBattle();
		BattleManager.inst.FactorMultiBuffs(ref this.unit.playerFactorTotal);
		if (TempData.inst.GetBool_SkillModuleOpenFlag(3) && this.unit.playerFactorTotal.fireSpd > 1f)
		{
			this.factorMultis_Skill.factorMultis[4] = this.unit.playerFactorTotal.fireSpd;
			this.factorMultis_Skill.factorMultis[3] = 1f / this.unit.playerFactorTotal.fireSpd;
		}
		this.unit.playerFactorTotal *= this.factorMultis_Skill;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00016973 File Offset: 0x00014B73
	protected override void OnFire()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			this.unit.HurtSelf(1, true);
		}
	}

	// Token: 0x04000323 RID: 803
	private float module_Grenadier_TimeLeft = 1f;
}

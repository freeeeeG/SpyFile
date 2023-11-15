using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class Player_0_NormalSoldier : Player
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600034C RID: 844 RVA: 0x00014479 File Offset: 0x00012679
	[SerializeField]
	private Skill_Player0_Shileds inst_Shiled
	{
		get
		{
			return Skill_Player0_Shileds.inst;
		}
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00014480 File Offset: 0x00012680
	protected override void DetectInput_Skill()
	{
		if (base.IfMouseNotOnButton() && Skill_Player0_Shileds.inst == null && MyInput.KeySkillHold())
		{
			this.Shield_Open();
		}
		if (Skill_Player0_Shileds.inst != null && !MyInput.KeySkillHold())
		{
			this.Shield_Close();
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x000144C0 File Offset: 0x000126C0
	private void Shield_Open()
	{
		if (this.energy < this.energyMax * 0.06f)
		{
			return;
		}
		this.energy -= this.energyMax * 0.06f;
		this.ifOpen = true;
		SoundEffects.Inst.skill_ShieldOpen.PlayRandom();
		if (Skill_Player0_Shileds.inst != null)
		{
			Debug.Log("Return");
			return;
		}
		if (this.prefab_Shiled == null)
		{
			Debug.LogError("prefab_empty");
		}
		else
		{
			Object.Instantiate<GameObject>(this.prefab_Shiled).GetComponent<Skill_Player0_Shileds>();
			this.inst_Shiled.DyeSprsWithColor(Player.inst.unit.mainColor);
			this.inst_Shiled.InitShields();
			this.inst_Shiled.FollowPlayer();
		}
		FactorMultis factorMultis = new FactorMultis();
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			factorMultis.factorMultis[3] = SkillModule.GetSkillModule_CurrentJobWithEffectID(1).facs[1];
		}
		this.factorMultis_Skill = factorMultis;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x000145B4 File Offset: 0x000127B4
	private void Shield_Close()
	{
		if (!this.ifOpen)
		{
			return;
		}
		this.ifOpen = false;
		SoundEffects.Inst.skill_ShieldClose.PlayRandom();
		this.inst_Shiled.Close();
		this.energyRecoverCD = 0.3f;
		this.factorMultis_Skill = new FactorMultis();
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00014604 File Offset: 0x00012804
	protected override void SkillInFixedUpdate()
	{
		if (TempData.inst == null)
		{
			return;
		}
		if (Battle.inst == null)
		{
			return;
		}
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(5).facs;
		if (this.sapper_LastUpgradeCount != Battle.inst.listUpgradeInt.Count)
		{
			this.sapper_LastUpgradeCount = Battle.inst.listUpgradeInt.Count;
			this.sapper_MineTypeHold = 0;
			Upgrade[] data_Upgrades = DataBase.Inst.Data_Upgrades;
			foreach (int num in Battle.inst.listUpgradeInt)
			{
				if (data_Upgrades[num].GetBool_IfHasType(3))
				{
					this.sapper_MineTypeHold++;
				}
			}
			this.sapper_TimeMax = facs[0] - facs[1] * (float)this.sapper_MineTypeHold;
			this.sapper_TimeMax = Mathf.Max(this.sapper_TimeMax, facs[2]);
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		this.sapper_TimeLeft -= Time.fixedDeltaTime;
		if (this.sapper_TimeLeft <= 0f)
		{
			this.sapper_TimeLeft += this.sapper_TimeMax;
			SpecialEffects.ShootBulletOnce_Mine();
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x0001474C File Offset: 0x0001294C
	protected override void Energy_Init()
	{
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		this.energyMax = facs[3];
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			this.energyMax *= SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs[0];
		}
		this.energy = this.energyMax;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x000147C8 File Offset: 0x000129C8
	protected override void Energy_FixedUpdate()
	{
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		float num = Time.fixedUnscaledDeltaTime;
		num = Mathf.Min(num, FPSDetector.inst.curFixedTimeSet * 1.3f);
		if (this.ifOpen)
		{
			float num2 = 1f;
			if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
			{
				num2 *= SkillModule.GetSkillModule_CurrentJobWithEffectID(1).facs[2];
			}
			if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
			{
				num2 *= SkillModule.GetSkillModule_CurrentJobWithEffectID(2).facs[1];
			}
			if (TempData.inst.GetBool_SkillModuleOpenFlag(3))
			{
				num2 *= SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs[0];
			}
			this.energy -= num * facs[1] * num2;
			if (this.energy <= 0f)
			{
				this.Shield_Close();
			}
		}
		else if (!MyInput.KeySkillHold())
		{
			if (this.energyRecoverCD >= 0f)
			{
				this.energyRecoverCD -= num;
			}
			else
			{
				float num3 = 1f;
				if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
				{
					num3 *= SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs[1];
				}
				float num4 = num * facs[2];
				this.energy += num4 * num3;
			}
		}
		this.energy = Mathf.Clamp(this.energy, 0f, this.energyMax);
	}

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private GameObject prefab_Shiled;

	// Token: 0x040002EC RID: 748
	[Header("Sapper")]
	[SerializeField]
	private float sapper_TimeLeft = 0.33f;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private float sapper_TimeMax = 0.33f;

	// Token: 0x040002EE RID: 750
	[SerializeField]
	private int sapper_LastUpgradeCount = -1;

	// Token: 0x040002EF RID: 751
	[SerializeField]
	private int sapper_MineTypeHold;

	// Token: 0x040002F0 RID: 752
	[SerializeField]
	private bool ifOpen;
}

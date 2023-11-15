using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class Player_8_Machineguner : Player
{
	// Token: 0x060003A2 RID: 930 RVA: 0x00016AB0 File Offset: 0x00014CB0
	protected override void DetectInput_Skill()
	{
		if (!base.IfMouseNotOnButton())
		{
			return;
		}
		if (MyInput.KeySkillDown())
		{
			this.SummonWave();
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00016AC8 File Offset: 0x00014CC8
	protected override void Awake()
	{
		base.Awake();
		Player_8_Machineguner.instJob8 = this;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			this.unit.prefab_Bullet = ResourceLibrary.Inst.Prefab_Bullet_Tracking;
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x000051D0 File Offset: 0x000033D0
	protected override void SkillInFixedUpdate()
	{
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00016AF8 File Offset: 0x00014CF8
	private void SummonWave()
	{
		SoundEffects.Inst.skill_Blast.PlayRandom();
		SpecialEffects.ActiveSkill();
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		Factor playerFactorTotal = this.unit.playerFactorTotal;
		float num = Mathf.Sqrt(this.unit.lastSize * facs[3]);
		double num2 = playerFactorTotal.factor[facs[4].Int()] * math.pow(playerFactorTotal.factor[facs[5].Int()], 2.0);
		Color mainColor = this.unit.mainColor;
		Skill_Player8_Wave component = Object.Instantiate<GameObject>(this.prefab_Wave, base.transform.position, Quaternion.identity).GetComponent<Skill_Player8_Wave>();
		float num3 = facs[0];
		int num4 = base.LifeMax - (int)this.unit.life;
		num2 *= (double)(facs[7] * (float)num4 + 1f);
		float num5 = 1f;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			num5 *= 2f;
			num3 *= 2f;
		}
		this.unit.GetHurt((double)num3, this.unit, Vector2.zero, false, base.transform.position, true);
		component.Init(num * Mathf.Sqrt(num5), mainColor, num2 * (double)num5, playerFactorTotal.critChc, playerFactorTotal.critDmg, this.unit.shapeType, false, true);
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00016C84 File Offset: 0x00014E84
	public void UpdateFactorTotal_AfterGetHurt()
	{
		this.unit.playerFactorTotal = this.unit.FactorBasic * Battle.inst.GetFactorMultis_Upgrates_CurBattle();
		BattleManager.inst.FactorMultiBuffs(ref this.unit.playerFactorTotal);
		this.factorMultis_Skill = new FactorMultis();
		if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			float num = (float)this.unit.life / (float)base.LifeMax;
			this.factorMultis_Skill.factorMultis[8] = num;
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(6))
		{
			float num2 = (float)this.unit.life / (float)base.LifeMax;
			this.factorMultis_Skill.factorMultis[3] = num2;
			this.factorMultis_Skill.factorMultis[4] = 1f + (1f - num2) * 2f;
		}
		this.unit.playerFactorTotal *= this.factorMultis_Skill;
	}

	// Token: 0x04000325 RID: 805
	public static Player_8_Machineguner instJob8;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private GameObject prefab_Wave;
}

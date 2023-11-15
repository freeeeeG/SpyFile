using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200005E RID: 94
public class Player_7_ShotGun : Player
{
	// Token: 0x0600039D RID: 925 RVA: 0x000169A2 File Offset: 0x00014BA2
	protected override void DetectInput_Skill()
	{
		if (!base.IfMouseNotOnButton())
		{
			return;
		}
		if (MyInput.KeySkillDown())
		{
			this.SkillShoot();
		}
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000051D0 File Offset: 0x000033D0
	protected override void SkillInFixedUpdate()
	{
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000169BC File Offset: 0x00014BBC
	private void SkillShoot()
	{
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		float num = BulletsOptimization.ActualFireSpeed() * facs[3];
		if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			num = SkillModule.GetSkillModule_CurrentJobWithEffectID(2).facs[0];
		}
		float num2 = facs[4];
		float num3 = facs[0];
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(1).facs;
			num3 += facs2[0];
			num *= facs2[2];
			num2 *= facs2[2];
		}
		this.unit.GetHurt((double)num3, this.unit, Vector2.zero, false, base.transform.position, true);
		base.StartCoroutine(this.StartShoot(Mathf.CeilToInt(num), num2));
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00016A93 File Offset: 0x00014C93
	private IEnumerator StartShoot(int times, float totalTime)
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(3))
		{
			float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs;
			this.factorMultis_Skill = new FactorMultis();
			this.factorMultis_Skill.factorMultis[facs[0].Int()] = facs[1];
			this.factorMultis_Skill.factorMultis[facs[2].Int()] = facs[3];
		}
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
			if (this.skillLevel <= 0)
			{
				Debug.LogError("skillLevel<=0!");
			}
			float[] facs2 = this.theSkill.facs;
			this.factorMultis_Skill.factorMultis[4] = facs2[6];
		}
		this.buffLayer += 1f;
		if (times == 0)
		{
			Debug.LogError("Shotgun_Skill_Times==0");
			times = 1;
		}
		SpecialEffects.ActiveSkill();
		this.UpdateFactorTotal(true);
		if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			this.unit.Shoot_ShootOnce(MyTool.MouseToPlayerVec2(), ResourceLibrary.Inst.Prefab_Bullet_Grenade);
		}
		else
		{
			this.unit.Shoot_ShootOnce(MyTool.MouseToPlayerVec2(), this.unit.prefab_Bullet);
		}
		int num;
		for (int i = 0; i < times - 1; i = num + 1)
		{
			yield return new WaitForSeconds(totalTime / (float)times);
			if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
			{
				this.unit.Shoot_ShootOnce(MyTool.MouseToPlayerVec2(), ResourceLibrary.Inst.Prefab_Bullet_Grenade);
			}
			else
			{
				this.unit.Shoot_ShootOnce(MyTool.MouseToPlayerVec2(), this.unit.prefab_Bullet);
			}
			num = i;
		}
		this.buffLayer -= 1f;
		if (this.buffLayer == 0f)
		{
			this.factorMultis_Skill = new FactorMultis();
			this.UpdateFactorTotal(true);
		}
		yield break;
	}

	// Token: 0x04000324 RID: 804
	private float buffLayer;
}

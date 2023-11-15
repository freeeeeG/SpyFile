using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class Player_1_Sniper : Player
{
	// Token: 0x06000376 RID: 886 RVA: 0x00015C7F File Offset: 0x00013E7F
	protected override void DetectInput_Skill()
	{
		if (base.IfMouseNotOnButton() && !this.onState && this.SniperSkillHold())
		{
			this.StartState();
		}
		if (this.onState && !this.SniperSkillHold())
		{
			this.QuitState();
		}
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00015CB5 File Offset: 0x00013EB5
	private bool SniperSkillHold()
	{
		return TempData.inst.skillModuleFlags[1][4] || MyInput.KeySkillHold();
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00015CD0 File Offset: 0x00013ED0
	private void StartState()
	{
		SoundEffects.Inst.skill_ShieldOpen.PlayRandom();
		this.anm.SetBool("OnState", true);
		this.onState = true;
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		BasicUnit unit = Player.inst.unit;
		this.factorMultis_Skill = new FactorMultis();
		if (TempData.inst.skillModuleFlags[1][1])
		{
			float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(1).facs;
			this.factorMultis_Skill.factorMultis[facs2[0].Int()] *= facs2[1];
		}
		else
		{
			TimeManager.inst.SetSniperTimeScale(facs[6]);
		}
		this.factorMultis_Skill.factorMultis[facs[2].Int()] += facs[3];
		this.factorMultis_Skill.factorMultis[facs[4].Int()] *= facs[5];
		if (!TempData.inst.skillModuleFlags[1][4])
		{
			this.factorMultis_Skill.factorMultis[facs[0].Int()] += facs[1];
			this.factorMultis_Skill.factorMultis[2] = 0f;
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00015E14 File Offset: 0x00014014
	private void QuitState()
	{
		if (!TempData.inst.skillModuleFlags[1][1])
		{
			TimeManager.inst.SetSniperTimeScale(1f);
		}
		SoundEffects.Inst.skill_ShieldClose.PlayRandom();
		this.anm.SetBool("OnState", false);
		this.onState = false;
		this.factorMultis_Skill = new FactorMultis();
	}

	// Token: 0x0600037A RID: 890 RVA: 0x000051D0 File Offset: 0x000033D0
	protected override void SkillInFixedUpdate()
	{
	}

	// Token: 0x0400030D RID: 781
	private bool onState;

	// Token: 0x0400030E RID: 782
	[SerializeField]
	private Animator anm;
}

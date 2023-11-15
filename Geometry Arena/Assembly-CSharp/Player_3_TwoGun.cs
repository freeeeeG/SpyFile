using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class Player_3_TwoGun : Player
{
	// Token: 0x06000382 RID: 898 RVA: 0x0001616E File Offset: 0x0001436E
	protected override void DetectInput_Skill()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			return;
		}
		if (!base.IfMouseNotOnButton())
		{
			return;
		}
		if (MyInput.KeySkillDown())
		{
			base.StartCoroutine(this.EnterDash());
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0001619C File Offset: 0x0001439C
	protected override void SkillInFixedUpdate()
	{
		if (!this.dash_On)
		{
			return;
		}
		Vector2 b = base.transform.position;
		Vector2 vector = this.dash_TargetPos - b;
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		float[] facs = this.theSkill.facs;
		this.dash_Speed = this.unit.playerFactorTotal.moveSpd * facs[5];
		if (this.dash_Speed <= 0f)
		{
			this.Dash_QuitState();
			return;
		}
		float magnitude = vector.magnitude;
		float num = this.dash_Speed * Time.fixedDeltaTime;
		this.unit.rb.velocity = Vector2.zero;
		if (magnitude <= num)
		{
			base.transform.position = this.dash_TargetPos;
			this.Dash_QuitState();
			return;
		}
		Vector2 normalized = vector.normalized;
		base.transform.position += normalized * this.dash_Speed * Time.fixedDeltaTime;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0001629F File Offset: 0x0001449F
	private void Dash_QuitState()
	{
		this.dash_On = false;
		this.canMove = true;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x000162AF File Offset: 0x000144AF
	private IEnumerator EnterDash()
	{
		if (this.dash_On)
		{
			this.Dash_QuitState();
		}
		SpecialEffects.ActiveSkill();
		SoundEffects.Inst.skill_Dash.PlayRandom();
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		this.factorMultis_Skill = new FactorMultis();
		float num = facs[7];
		this.dash_TargetPos = MyTool.MousePos();
		this.dash_Speed = this.unit.playerFactorTotal.moveSpd * facs[5];
		this.dash_On = true;
		this.canMove = false;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(3))
		{
			this.direction = this.GetVector2_Dodge();
			float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs;
			this.factorMultis_Skill.factorMultis[facs2[0].Int()] = facs2[1];
			float d = facs2[2] * this.unit.playerFactorTotal.moveSpd;
			this.dash_TargetPos = this.direction * d + base.transform.position;
		}
		else
		{
			this.dash_TargetPos = MyTool.MousePos();
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			this.unit.HurtSelf(1, true);
			float[] facs3 = SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs;
			this.factorMultis_Skill.factorMultis[facs3[2].Int()] = facs3[3];
		}
		if (this.skillLevel >= 1)
		{
			this.factorMultis_Skill.factorMultis[facs[0].Int()] *= facs[1];
			this.factorMultis_Skill.factorMultis[facs[2].Int()] *= facs[3];
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			float[] facs4 = SkillModule.GetSkillModule_CurrentJobWithEffectID(1).facs;
			num *= facs4[3];
			this.factorMultis_Skill.factorMultis[facs4[1].Int()] = facs4[2];
		}
		this.UpdateFactorTotal(true);
		this.buffLayer++;
		yield return new WaitForSeconds(num);
		this.buffLayer--;
		if (this.buffLayer == 0)
		{
			this.factorMultis_Skill = new FactorMultis();
		}
		yield break;
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000162C0 File Offset: 0x000144C0
	private Vector2 GetVector2_Dodge()
	{
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		return new Vector2(axisRaw, axisRaw2).normalized;
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000162F0 File Offset: 0x000144F0
	protected override void Special_OnHitWall()
	{
		this.Dash_QuitState();
	}

	// Token: 0x04000316 RID: 790
	[SerializeField]
	private Vector2 direction = Vector2.zero;

	// Token: 0x04000317 RID: 791
	[SerializeField]
	private int buffLayer;

	// Token: 0x04000318 RID: 792
	[Header("Dash")]
	[SerializeField]
	private Vector2 dash_TargetPos = Vector2.zero;

	// Token: 0x04000319 RID: 793
	[SerializeField]
	private float dash_Speed;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	private bool dash_On;
}

using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class Player_4_Tri : Player
{
	// Token: 0x06000389 RID: 905 RVA: 0x00016316 File Offset: 0x00014516
	protected override void Awake()
	{
		base.Awake();
		Player_4_Tri.instTri = this;
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00016324 File Offset: 0x00014524
	protected override void DetectInput_Skill()
	{
		if (base.IfMouseNotOnButton() && !this.onState && MyInput.KeyShootHold())
		{
			this.StartState();
		}
		if (this.onState && (!MyInput.KeyShootHold() || (!base.IfMouseNotOnButton() && !Icon_AutoFire.inst.open)))
		{
			this.QuitState();
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00016378 File Offset: 0x00014578
	private void StartState()
	{
		this.onState = true;
		this.canAimAtMouse = false;
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		BasicUnit unit = Player.inst.unit;
		this.factorMultis_Skill = new FactorMultis();
		this.totalTime = facs[7];
		this.targetMulti = facs[8];
		if (TempData.inst.skillModuleFlags[4][2])
		{
			float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(2).facs;
			this.targetMulti = facs2[0];
			this.totalTime *= facs2[0] / facs[8];
		}
		this.curMulti = 1f;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0001642F File Offset: 0x0001462F
	private void QuitState()
	{
		this.onState = false;
		this.canAimAtMouse = true;
		this.factorMultis_Skill = new FactorMultis();
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0001644C File Offset: 0x0001464C
	protected override void SkillInFixedUpdate()
	{
		if (this.onShoot)
		{
			base.transform.Rotate(0f, 0f, this.rotateSpd * Time.fixedDeltaTime);
		}
		if (this.onState)
		{
			float num = (this.targetMulti - 1f) / this.totalTime;
			if (TempData.inst.skillModuleFlags[4][2])
			{
				float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(2).facs;
				num *= facs[1];
			}
			if (TempData.inst.skillModuleFlags[4][3])
			{
				float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs;
				num *= facs2[0];
			}
			if (this.curMulti <= this.targetMulti)
			{
				this.curMulti += num * Time.deltaTime;
				this.curMulti = Mathf.Min(this.curMulti, this.targetMulti);
			}
			this.UpdateFactor();
		}
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00016524 File Offset: 0x00014724
	public void GetHurt()
	{
		this.SM5_Grenades();
		if (!this.onState)
		{
			return;
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			return;
		}
		this.curMulti = 1f;
		this.UpdateFactor();
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00016554 File Offset: 0x00014754
	private void SM5_Grenades()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			return;
		}
		int num = Mathf.CeilToInt(Mathf.Sqrt(this.unit.playerFactorTotal.moveSpd));
		if (num == 0)
		{
			return;
		}
		SoundEffects.Inst.bullet_Grenade.PlayRandom();
		for (int i = 0; i < num; i++)
		{
			Bullet component = SpecialEffects.ShootBulletOnce_Grenade(false).GetComponent<SpecialBullet_Grenade>();
			float angle = 360f / (float)num * (float)i;
			component.UpdateDirection(MyTool.AngleToVec2(angle));
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x000165CC File Offset: 0x000147CC
	private void UpdateFactor()
	{
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		this.rotateSpd = BulletsOptimization.ActualFireSpeed() * facs[1];
		this.factorMultis_Skill.factorMultis[facs[2].Int()] = facs[3];
		this.factorMultis_Skill.factorMultis[2] = this.curMulti;
		this.factorMultis_Skill.factorMultis[3] = this.curMulti;
	}

	// Token: 0x0400031B RID: 795
	public static Player_4_Tri instTri;

	// Token: 0x0400031C RID: 796
	private bool onState;

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private float rotateSpd;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	private float curMulti = 1f;

	// Token: 0x0400031F RID: 799
	[SerializeField]
	private float totalTime = 1f;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private float targetMulti = 1f;
}

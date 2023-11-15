using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class Player_5_Summoner : Player
{
	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000392 RID: 914 RVA: 0x0001667E File Offset: 0x0001487E
	public static List<SpecialBullet_Summon> SummonBullets
	{
		get
		{
			return Player.inst.gameObject.GetComponent<Player_5_Summoner>().summonBullets;
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00016694 File Offset: 0x00014894
	protected override void Awake()
	{
		base.Awake();
		Player_5_Summoner.instSummoner = this;
		this.summonBullets = new List<SpecialBullet_Summon>();
	}

	// Token: 0x06000394 RID: 916 RVA: 0x000166B0 File Offset: 0x000148B0
	protected override void DetectInput_Skill()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4) && MyInput.KeySkillDown())
		{
			base.transform.position = MyTool.MousePos();
			this.unit.Antibug_MoveRestriction_Strict();
			this.unit.rb.velocity = Vector2.zero;
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00016706 File Offset: 0x00014906
	protected override void SkillInFixedUpdate()
	{
		this.skill_CanShoot = false;
		if (BattleManager.inst.listEnemies.Count > 0)
		{
			this.unit.Shoot_WantoShootOnce(Vector2.zero);
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00016734 File Offset: 0x00014934
	public override void UpdateFactorTotal(bool ifTrue = true)
	{
		base.UpdateFactorTotal(true);
		this.theSkill = Skill.SkillCurrent(ref this.skillLevel);
		if (this.skillLevel <= 0)
		{
			Debug.LogError("skillLevel<=0!");
		}
		float[] facs = this.theSkill.facs;
		this.factorMultis_Skill = new FactorMultis();
		if (this.skillLevel >= 2)
		{
			this.factorMultis_Skill.factorMultis[facs[2].Int()] = this.unit.playerFactorTotal.accuracy;
		}
		if (this.skillLevel >= 3)
		{
			this.factorMultis_Skill.factorMultis[facs[4].Int()] = 1f + this.unit.playerFactorTotal.accuracy;
		}
		base.UpdateFactorTotal(true);
	}

	// Token: 0x04000321 RID: 801
	public static Player_5_Summoner instSummoner;

	// Token: 0x04000322 RID: 802
	public List<SpecialBullet_Summon> summonBullets = new List<SpecialBullet_Summon>();
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class Player_9_SwordMaster : Player
{
	// Token: 0x060003A8 RID: 936 RVA: 0x00016D74 File Offset: 0x00014F74
	private void InitSingleSword(int index)
	{
		if (index >= this.swords.Length)
		{
			Debug.LogError("Error_SwordIndexOut!");
			return;
		}
		if (this.swords[index] != null)
		{
			Debug.LogError("Warning_OldSwordExist");
			Object.Destroy(this.swords[index].gameObject);
		}
		SpecialBullet_Sword component = Object.Instantiate<GameObject>(this.prefab_Sword).GetComponent<SpecialBullet_Sword>();
		component.InitSword(this, (float)index / (float)this.swords.Length * 360f);
		this.swords[index] = component;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00016DF8 File Offset: 0x00014FF8
	public void InitOrUpdateAllSword()
	{
		int num = this.swords.Length;
		int swordNum = this.GetSwordNum();
		if (num == swordNum)
		{
			return;
		}
		for (int i = 0; i < this.swords.Length; i++)
		{
			Object.Destroy(this.swords[i].gameObject);
		}
		this.swords = new SpecialBullet_Sword[swordNum];
		for (int j = 0; j < this.swords.Length; j++)
		{
			this.InitSingleSword(j);
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00016E63 File Offset: 0x00015063
	protected override void Init_SpecialSkillInit()
	{
		this.InitOrUpdateAllSword();
		this.skill_CanShoot = false;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00016E74 File Offset: 0x00015074
	public int GetSwordNum()
	{
		int num = 1;
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			int splitUpgradeCount = Battle.inst.GetSplitUpgradeCount();
			num *= Mathf.Pow(2f, (float)splitUpgradeCount).Int();
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			num *= 2;
		}
		return num;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x00016EC1 File Offset: 0x000150C1
	protected override void SkillInFixedUpdate()
	{
		this.InitOrUpdateAllSword();
		this.AutoRotate();
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00016ED0 File Offset: 0x000150D0
	private void AutoRotate()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			this.canAimAtMouse = false;
			float num = SkillModule.GetSkillModule_CurrentJobWithEffectID(0).facs[1] * this.unit.playerFactorTotal.bulletSpd;
			base.transform.Rotate(new Vector3(0f, 0f, num * Time.fixedDeltaTime));
			return;
		}
		this.canAimAtMouse = true;
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00016F3C File Offset: 0x0001513C
	protected override void DetectInput_Skill()
	{
		if (!base.IfMouseNotOnButton() || BattleMapCanvas.inst.IfAnyWindowActive())
		{
			return;
		}
		if ((MyInput.KeyShootDown() || Icon_AutoFire.inst.open) && !TempData.inst.GetBool_SkillModuleOpenFlag(3))
		{
			List<SpecialBullet_Sword> list = new List<SpecialBullet_Sword>();
			foreach (SpecialBullet_Sword specialBullet_Sword in this.swords)
			{
				if (specialBullet_Sword.swordState == SpecialBullet_Sword.EnumSwordState.INHAND)
				{
					list.Add(specialBullet_Sword);
				}
			}
			if (list.Count > 0)
			{
				if (!MyInput.GetKeyHold_Special_SwordAll())
				{
					SpecialBullet_Sword random = list.GetRandom<SpecialBullet_Sword>();
					random.transform.parent = null;
					random.FlyDefault_FlyGoingToTarget(MyTool.MousePos());
					if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
					{
						this.unit.HurtSelf(1, true);
					}
					SoundEffects.Inst.skill_SwordOut.PlayRandom();
				}
				else
				{
					foreach (SpecialBullet_Sword specialBullet_Sword2 in list)
					{
						specialBullet_Sword2.transform.parent = null;
						specialBullet_Sword2.FlyDefault_FlyGoingToTarget(MyTool.MousePos());
						if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
						{
							this.unit.HurtSelf(1, true);
						}
					}
					SoundEffects.Inst.skill_SwordOut.PlayRandom();
				}
			}
		}
		if (MyInput.KeySkillDown())
		{
			List<SpecialBullet_Sword> list2 = new List<SpecialBullet_Sword>();
			foreach (SpecialBullet_Sword specialBullet_Sword3 in this.swords)
			{
				if (specialBullet_Sword3.swordState == SpecialBullet_Sword.EnumSwordState.FLYINGGOING)
				{
					list2.Add(specialBullet_Sword3);
				}
			}
			if (list2.Count > 0)
			{
				if (!MyInput.GetKeyHold_Special_SwordAll())
				{
					Vector2 b = MyTool.MousePos();
					float num = 10000f;
					SpecialBullet_Sword specialBullet_Sword4 = null;
					foreach (SpecialBullet_Sword specialBullet_Sword5 in list2)
					{
						float magnitude = (specialBullet_Sword5.transform.position - b).magnitude;
						if (magnitude < num)
						{
							num = magnitude;
							specialBullet_Sword4 = specialBullet_Sword5;
						}
					}
					if (specialBullet_Sword4 == null)
					{
						Debug.LogError("Error_TheSword==null!");
						return;
					}
					specialBullet_Sword4.FlyCommon_FlyBackingToHand();
					SoundEffects.Inst.skill_SwordBack.PlayRandom();
					return;
				}
				else
				{
					foreach (SpecialBullet_Sword specialBullet_Sword6 in list2)
					{
						specialBullet_Sword6.FlyCommon_FlyBackingToHand();
					}
					SoundEffects.Inst.skill_SwordBack.PlayRandom();
				}
			}
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x000171CC File Offset: 0x000153CC
	public override void UpdateFactorTotal(bool ifTrue = true)
	{
		base.UpdateFactorTotal(true);
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(6))
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(6).facs;
		this.factorMultis_Skill = new FactorMultis();
		this.factorMultis_Skill.factorMultis[facs[2].Int()] = 1f + this.unit.playerFactorTotal.moveSpd / facs[0] * facs[3];
		this.factorMultis_Skill.factorMultis[facs[4].Int()] = 1f + this.unit.playerFactorTotal.moveSpd / facs[0] * facs[5];
		base.UpdateFactorTotal(true);
	}

	// Token: 0x04000327 RID: 807
	[SerializeField]
	private GameObject prefab_Sword;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	private SpecialBullet_Sword[] swords = new SpecialBullet_Sword[0];

	// Token: 0x04000329 RID: 809
	[SerializeField]
	public float setting_SwordRadiusFix = 0.2f;
}

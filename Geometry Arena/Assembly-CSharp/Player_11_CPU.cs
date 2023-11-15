using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class Player_11_CPU : Player
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000360 RID: 864 RVA: 0x0001501A File Offset: 0x0001321A
	public int MainDroneNum
	{
		get
		{
			return this.mainDrones.Count;
		}
	}

	// Token: 0x06000361 RID: 865 RVA: 0x00015027 File Offset: 0x00013227
	protected override void Awake()
	{
		base.Awake();
		Player_11_CPU.inst = this;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00015038 File Offset: 0x00013238
	protected override void DetectInput_Skill()
	{
		if (!base.IfMouseNotOnButton())
		{
			return;
		}
		if (MyInput.KeyShootDown())
		{
			if (MyInput.KeyShiftHold())
			{
				int num = 9 - this.mainDrones.Count;
				for (int i = 0; i < num; i++)
				{
					this.GeneNewMainDrone();
				}
			}
			else
			{
				this.GeneNewMainDrone();
			}
			this.HpMaxUpdate();
		}
		if (MyInput.KeySkillDown())
		{
			if (MyInput.KeyShiftHold())
			{
				int count = this.mainDrones.Count;
				for (int j = 0; j < count; j++)
				{
					this.DestroyOneMainDrone();
				}
			}
			else
			{
				this.DestroyOneMainDrone();
			}
			this.HpMaxUpdate();
		}
	}

	// Token: 0x06000363 RID: 867 RVA: 0x000150C5 File Offset: 0x000132C5
	protected override void SkillInFixedUpdate()
	{
		this.FixedUpdate_GrenadeDrones_Num();
		this.FixedUpdate_TargetDrones_Num();
		this.FixedUpdate_ItemDrones_Num();
		this.FixedUpdate_LaserDrones_Num();
		this.FixedUpdate_LightDrones_Num();
		this.FixedUpdate_Drones_PosSpd();
		this.FixedUpdate_Shoot();
	}

	// Token: 0x06000364 RID: 868 RVA: 0x000150F1 File Offset: 0x000132F1
	private void FixedUpdate_Shoot()
	{
		this.FixedUpdate_Shoot_Main();
		this.FixedUpdate_Shoot_Grenade();
		this.FixedUpdate_Shoot_Item();
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00015108 File Offset: 0x00013308
	private void FixedUpdate_Shoot_Main()
	{
		if (this.mainDrones.Count <= 0)
		{
			return;
		}
		if (BattleManager.inst.listEnemies.Count <= 0)
		{
			return;
		}
		this.shoot_Main_CurTime += Time.fixedDeltaTime;
		if (this.shoot_Main_CurTime >= this.shoot_Main_MaxTime)
		{
			this.shoot_Main_CurTime -= this.shoot_Main_MaxTime;
			SoundEffects.Inst.shoot.PlayRandom();
			int index = Random.Range(0, this.mainDrones.Count);
			Drone drone = this.mainDrones[index];
			drone.FixedUpdate_EnemyAndAim();
			this.unit.Shoot_GeneBulletOnce(drone.gun, this.unit.prefab_Bullet);
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x000151BC File Offset: 0x000133BC
	private void FixedUpdate_Shoot_Grenade()
	{
		if (this.grenadeDrones.Count <= 0)
		{
			return;
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		this.shoot_Gre_CurTime += Time.fixedDeltaTime;
		if (this.shoot_Gre_CurTime >= this.shoot_Gre_MaxTime)
		{
			this.shoot_Gre_CurTime -= this.shoot_Gre_MaxTime;
			this.shoot_Gre_Index = (this.shoot_Gre_Index + 1) % this.grenadeDrones.Count;
			SpecialEffects.ShootBulletOnce_Grenade(true).transform.position = this.grenadeDrones[this.shoot_Gre_Index].transform.position;
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00015260 File Offset: 0x00013460
	private void FixedUpdate_Shoot_Item()
	{
		if (this.itemDrones.Count <= 0)
		{
			return;
		}
		if (BattleManager.inst.timeStage == EnumTimeStage.REST)
		{
			return;
		}
		this.shoot_Item_CurTime += Time.fixedDeltaTime;
		if (this.shoot_Item_CurTime >= this.shoot_Item_MaxTime)
		{
			Vector2 vector = this.itemDrones[0].transform.position;
			this.shoot_Item_CurTime -= this.shoot_Item_MaxTime;
			SpecialEffects.BattleItem_Random(vector.x, vector.y);
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x000152EC File Offset: 0x000134EC
	private void GeneNewMainDrone()
	{
		if (this.mainDrones.Count >= 9)
		{
			return;
		}
		Drone component = Object.Instantiate<GameObject>(this.prefabMainDrone, base.transform.position, Quaternion.identity).GetComponent<Drone>();
		this.mainDrones.Add(component);
		component.DyeWithColor(this.unit.mainColor);
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00015348 File Offset: 0x00013548
	private void DestroyOneMainDrone()
	{
		if (this.mainDrones.Count <= 0)
		{
			return;
		}
		Drone drone = this.mainDrones[0];
		GameObject gameObject = drone.gameObject;
		this.mainDrones.Remove(drone);
		drone.DestoryThisDrone();
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001538C File Offset: 0x0001358C
	private void HpMaxUpdate()
	{
		int num = (int)this.unit.life;
		int num2 = (int)this.unit.LifeMax;
		int num3 = num2 - num;
		this.factorMultis_Skill.factorMultis[1] = 0f;
		this.UpdateFactorTotal(true);
		int lifeMax = base.LifeMax;
		float num4 = 1f - (float)this.mainDrones.Count * 0.1f;
		int num5 = Mathf.Max(1, Mathf.RoundToInt(num4 * (float)lifeMax));
		this.factorMultis_Skill.factorMultis[1] = (float)(num5 - lifeMax);
		this.UpdateFactorTotal(true);
		this.unit.life = (double)Mathf.Max(1, base.LifeMax - num3);
		HealthPointControl.inst.UpdateHpUnits();
		if (this.unit.life != (double)num || this.unit.LifeMax != (double)num2)
		{
			BuffManager.RefreshStateBuff_AboutLife();
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00015464 File Offset: 0x00013664
	private void FixedUpdate_Drones_PosSpd()
	{
		float num = Time.time * this.rotateSpd % 360f;
		float num2 = base.transform.eulerAngles.z * 0f;
		float num3 = this.radiusStart;
		num3 += base.transform.localScale.x / 2f;
		int num4 = -1;
		if (this.mainDrones.Count > 0)
		{
			for (int i = 0; i < this.mainDrones.Count; i++)
			{
				this.mainDrones[i].UpdateTargetPos(this, i, this.mainDrones.Count, base.transform.position, num2 - (float)num4 * num, num3);
				this.mainDrones[i].SetMoveSpeed(this.unit.playerFactorTotal.moveSpd * 0.6f);
			}
			num3 += this.radiusAdd;
		}
		if (this.grenadeDrones.Count > 0)
		{
			for (int j = 0; j < this.grenadeDrones.Count; j++)
			{
				this.grenadeDrones[j].UpdateTargetPos(this, j, this.grenadeDrones.Count, base.transform.position, num2 + (float)num4 * num, num3);
				this.grenadeDrones[j].SetMoveSpeed(this.unit.playerFactorTotal.moveSpd * 0.6f);
			}
			num3 += this.radiusAdd;
			num4 *= -1;
		}
		if (this.laserDrones.Count > 0)
		{
			for (int k = 0; k < this.laserDrones.Count; k++)
			{
				float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs;
				float num5 = facs[7] + this.unit.playerFactorTotal.moveSpd * facs[9];
				if (TempData.inst.GetBool_SkillModuleOpenFlag(7))
				{
					float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(7).facs;
					num5 *= facs2[1];
				}
				this.laserDrones[k].UpdateTargetPos(this, k, this.laserDrones.Count, base.transform.position, num2 + (float)num4 * num, num3);
				this.laserDrones[k].SetMoveSpeed(num5);
			}
			if (!this.laserDrones[0].ifTracing)
			{
				num3 += this.radiusAdd;
				num4 *= -1;
			}
		}
		if (this.lightDrones.Count > 0)
		{
			for (int l = 0; l < this.lightDrones.Count; l++)
			{
				this.lightDrones[l].UpdateTargetPos(this, l, this.lightDrones.Count, base.transform.position, num2 + (float)num4 * num, num3);
				this.lightDrones[l].SetMoveSpeed(this.unit.playerFactorTotal.bulletSpd);
				this.lightDrones[l].SetBodySize(Mathf.Sqrt(this.unit.playerFactorTotal.bodySize));
			}
			if (!this.lightDrones[0].ifTracing)
			{
				num3 += this.radiusAdd;
				num4 *= -1;
			}
		}
		for (int m = 0; m < this.itemDrones.Count; m++)
		{
			this.itemDrones[m].UpdateTargetPos(this, m, this.itemDrones.Count, base.transform.position, num2 + (float)num4 * num, num3);
			this.itemDrones[m].SetMoveSpeed(this.unit.playerFactorTotal.moveSpd * 0.6f);
		}
		for (int n = 0; n < this.targetDrones.Count; n++)
		{
			this.targetDrones[n].SetMoveSpeed(3f);
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0001584C File Offset: 0x00013A4C
	public override void UpdateFactorTotal(bool ifTrue = false)
	{
		int num = (int)this.unit.life;
		int num2 = (int)this.unit.LifeMax - num;
		this.factorMultis_Skill.factorMultis[1] = 0f;
		base.UpdateFactorTotal(ifTrue);
		int lifeMax = base.LifeMax;
		float num3 = 1f - (float)this.mainDrones.Count * 0.1f;
		int num4 = Mathf.Max(1, Mathf.RoundToInt(num3 * (float)lifeMax));
		this.factorMultis_Skill.factorMultis[1] = (float)(num4 - lifeMax);
		base.UpdateFactorTotal(ifTrue);
		this.unit.life = (double)Mathf.Max(1, base.LifeMax - num2);
		HealthPointControl.inst.UpdateHpUnits();
		this.shoot_Main_MaxTime = 1f / (BulletsOptimization.ActualFireSpeed() * (float)this.mainDrones.Count * 0.3f);
		if (TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(0).facs;
			this.shoot_Gre_MaxTime = facs[1] / (float)this.grenadeDrones.Count;
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			float[] facs2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs;
			this.shoot_Item_MaxTime = facs2[1];
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(7))
		{
			float[] facs3 = SkillModule.GetSkillModule_CurrentJobWithEffectID(7).facs;
			this.shoot_Gre_MaxTime *= facs3[0];
			this.shoot_Item_MaxTime *= facs3[2];
		}
	}

	// Token: 0x0600036D RID: 877 RVA: 0x000159B4 File Offset: 0x00013BB4
	private void FixedUpdate_GrenadeDrones_Num()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(0).facs;
		int targetNum = 1 + Mathf.FloorToInt(this.unit.playerFactorTotal.bulletSize / facs[2]);
		this.UpdateDroneList<GrenadeDrone>(this.grenadeDrones, targetNum, this.prefabGrenadeDrone);
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00015A0A File Offset: 0x00013C0A
	private void FixedUpdate_TargetDrones_Num()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			return;
		}
		this.UpdateDroneList<TargetDrone>(this.targetDrones, 1, this.prefabTargetDrone);
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00015A2D File Offset: 0x00013C2D
	private void FixedUpdate_ItemDrones_Num()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			return;
		}
		this.UpdateDroneList<ItemDrone>(this.itemDrones, 1, this.prefabItemDrone);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00015A50 File Offset: 0x00013C50
	private void FixedUpdate_LaserDrones_Num()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(3))
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(3).facs;
		int targetNum = facs[2].RoundToInt() + Mathf.FloorToInt(this.unit.playerFactorTotal.moveSpd / facs[5]);
		this.UpdateDroneList<LaserDrone>(this.laserDrones, targetNum, this.prefabLaserDrone);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00015AB0 File Offset: 0x00013CB0
	private void FixedUpdate_LightDrones_Num()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(6))
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(6).facs;
		int targetNum = facs[8].RoundToInt() + Mathf.FloorToInt((float)this.unit.playerFactorTotal.lifeMaxPlayer / facs[1]);
		this.UpdateDroneList<LightDrone>(this.lightDrones, targetNum, this.prefabLightDrone);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00015B10 File Offset: 0x00013D10
	private void UpdateDroneList<T>(List<T> list, int targetNum, GameObject prefab) where T : Drone
	{
		int count = list.Count;
		if (count == targetNum)
		{
			return;
		}
		if (count < 0 || targetNum < 0)
		{
			Debug.LogError("Error:Num<0!");
			return;
		}
		if (count < targetNum)
		{
			int num = targetNum - count;
			for (int i = 0; i < num; i++)
			{
				T component = Object.Instantiate<GameObject>(prefab, base.transform.position, Quaternion.identity).GetComponent<T>();
				list.Add(component);
				component.DyeWithColor(this.unit.mainColor);
			}
			return;
		}
		if (count > targetNum)
		{
			int num2 = count - targetNum;
			for (int j = 0; j < num2; j++)
			{
				T t = list[0];
				list.Remove(t);
				t.DestoryThisDrone();
			}
			return;
		}
	}

	// Token: 0x06000373 RID: 883 RVA: 0x00015BC3 File Offset: 0x00013DC3
	protected override void Init_SpecialSkillInit()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			this.unit.prefab_Bullet = ResourceLibrary.Inst.Prefab_Bullet_Tracking;
		}
	}

	// Token: 0x040002F6 RID: 758
	public new static Player_11_CPU inst;

	// Token: 0x040002F7 RID: 759
	[Header("Lists")]
	[SerializeField]
	private List<Drone> mainDrones = new List<Drone>();

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	private List<GrenadeDrone> grenadeDrones = new List<GrenadeDrone>();

	// Token: 0x040002F9 RID: 761
	[SerializeField]
	private List<TargetDrone> targetDrones = new List<TargetDrone>();

	// Token: 0x040002FA RID: 762
	[SerializeField]
	private List<ItemDrone> itemDrones = new List<ItemDrone>();

	// Token: 0x040002FB RID: 763
	[SerializeField]
	private List<LaserDrone> laserDrones = new List<LaserDrone>();

	// Token: 0x040002FC RID: 764
	[SerializeField]
	private List<LightDrone> lightDrones = new List<LightDrone>();

	// Token: 0x040002FD RID: 765
	[Header("Prefabs")]
	[SerializeField]
	private GameObject prefabMainDrone;

	// Token: 0x040002FE RID: 766
	[SerializeField]
	private GameObject prefabGrenadeDrone;

	// Token: 0x040002FF RID: 767
	[SerializeField]
	private GameObject prefabTargetDrone;

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private GameObject prefabItemDrone;

	// Token: 0x04000301 RID: 769
	[SerializeField]
	private GameObject prefabLaserDrone;

	// Token: 0x04000302 RID: 770
	[SerializeField]
	private GameObject prefabLightDrone;

	// Token: 0x04000303 RID: 771
	[Header("Shoot")]
	[SerializeField]
	private float shoot_Main_MaxTime = 1f;

	// Token: 0x04000304 RID: 772
	[SerializeField]
	private float shoot_Main_CurTime;

	// Token: 0x04000305 RID: 773
	[SerializeField]
	private float shoot_Gre_MaxTime = 1f;

	// Token: 0x04000306 RID: 774
	[SerializeField]
	private float shoot_Gre_CurTime;

	// Token: 0x04000307 RID: 775
	[SerializeField]
	private int shoot_Gre_Index;

	// Token: 0x04000308 RID: 776
	[SerializeField]
	private float shoot_Item_MaxTime = 1f;

	// Token: 0x04000309 RID: 777
	[SerializeField]
	private float shoot_Item_CurTime;

	// Token: 0x0400030A RID: 778
	[Header("Radius")]
	[SerializeField]
	private float radiusStart = 2.7f;

	// Token: 0x0400030B RID: 779
	[SerializeField]
	private float radiusAdd = 1f;

	// Token: 0x0400030C RID: 780
	[Header("Others")]
	[SerializeField]
	private float rotateSpd = 5f;
}

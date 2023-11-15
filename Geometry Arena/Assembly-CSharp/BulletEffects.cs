using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F2 RID: 242
[Serializable]
public class BulletEffects
{
	// Token: 0x060008A7 RID: 2215 RVA: 0x00032108 File Offset: 0x00030308
	public void Init(bool ifClone)
	{
		this.dataBase = DataBase.Inst;
		int numBulletEffect = this.dataBase.NumBulletEffect;
		if (this.effects == null)
		{
			this.effects = new BulletEffects.SingleEffect[numBulletEffect];
		}
		if (this.effects.Length != numBulletEffect)
		{
			this.effects = new BulletEffects.SingleEffect[numBulletEffect];
		}
		this.listEffectInts = new List<int>();
		for (int i = 0; i < this.effects.Length; i++)
		{
			if (this.effects[i] == null)
			{
				this.effects[i] = new BulletEffects.SingleEffect();
			}
			else
			{
				this.effects[i].Reset();
			}
			this.effects[i].id = i;
			bool flag = Battle.inst.bulletEffect[i];
			this.effects[i].enabled = flag;
			this.effects[i].triggerRange = this.dataBase.Data_Upgrade_BulletEffects[i].bulletEffectTrigRange;
			if (!ifClone && flag)
			{
				Upgrade_BulletEffect.EnumBulletEffect bulletEffectType = this.dataBase.Data_Upgrade_BulletEffects[i].bulletEffectType;
				if (bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.GROWING || bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.SUDDEN)
				{
					this.listEffectInts.Add(i);
				}
			}
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00032218 File Offset: 0x00030418
	public void UpdateBulletEffects(float rangePerecnt)
	{
		if (rangePerecnt < 0f)
		{
			return;
		}
		if (Battle.inst.specialEffect[80] >= 1)
		{
			rangePerecnt *= 2f;
		}
		rangePerecnt = Mathf.Min(rangePerecnt, 1f);
		for (int i = 0; i < this.listEffectInts.Count; i++)
		{
			int num = this.listEffectInts[i];
			if (this.effects[num].enabled)
			{
				Upgrade_BulletEffect upgrade_BulletEffect = this.dataBase.Data_Upgrade_BulletEffects[num];
				Upgrade_BulletEffect.Fac[] bulletEffectFacs = upgrade_BulletEffect.bulletEffectFacs;
				Upgrade_BulletEffect.EnumBulletEffect bulletEffectType = upgrade_BulletEffect.bulletEffectType;
				if (bulletEffectType != Upgrade_BulletEffect.EnumBulletEffect.GROWING)
				{
					if (bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.SUDDEN)
					{
						if (this.IfCanUseAndUse(num, rangePerecnt))
						{
							this.listEffectInts.Remove(num);
							for (int j = 0; j < 3; j++)
							{
								if (bulletEffectFacs[j].type >= 0)
								{
									int type = bulletEffectFacs[j].type;
									float numMul = bulletEffectFacs[j].numMul;
									this.effects[num].abilityMods[type] = numMul;
								}
							}
						}
					}
				}
				else if (this.effects[num].enabled)
				{
					for (int k = 0; k < 3; k++)
					{
						if (bulletEffectFacs[k].type >= 0)
						{
							int type2 = bulletEffectFacs[k].type;
							float numMul2 = bulletEffectFacs[k].numMul;
							this.effects[num].abilityMods[type2] = (numMul2 - 1f) * rangePerecnt + 1f;
						}
					}
				}
			}
		}
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00032378 File Offset: 0x00030578
	public bool IfCanUseAndUse(int id, float rangePercent)
	{
		if (!this.effects[id].enabled)
		{
			return false;
		}
		if (this.effects[id].ifUsed)
		{
			return false;
		}
		if (rangePercent < this.effects[id].triggerRange)
		{
			return false;
		}
		this.effects[id].ifUsed = true;
		return true;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x000323C8 File Offset: 0x000305C8
	private bool IfCanUse(int id)
	{
		return !this.effects[id].ifUsed && this.effects[id].enabled;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x000323F0 File Offset: 0x000305F0
	public BulletEffects(BulletEffects clone)
	{
		this.listEffectInts = new List<int>();
		if (clone == null)
		{
			this.Init(false);
			return;
		}
		this.Init(true);
		for (int i = 0; i < this.effects.Length; i++)
		{
			bool ifUsed = clone.effects[i].ifUsed;
			this.effects[i].ifUsed = ifUsed;
			for (int j = 0; j < 3; j++)
			{
				this.effects[i].abilityMods[j] = clone.effects[i].abilityMods[j];
			}
		}
		foreach (int item in clone.listEffectInts)
		{
			this.listEffectInts.Add(item);
		}
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x000324F8 File Offset: 0x000306F8
	public void CloneFrom(BulletEffects clone)
	{
		this.listEffectInts = new List<int>();
		if (clone == null)
		{
			Debug.LogError("克隆对象为空");
		}
		this.Init(true);
		for (int i = 0; i < this.effects.Length; i++)
		{
			bool ifUsed = clone.effects[i].ifUsed;
			this.effects[i].ifUsed = ifUsed;
			for (int j = 0; j < 3; j++)
			{
				this.effects[i].abilityMods[j] = clone.effects[i].abilityMods[j];
			}
		}
		foreach (int item in clone.listEffectInts)
		{
			this.listEffectInts.Add(item);
		}
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x000325CC File Offset: 0x000307CC
	public void UpdateAbilityMods()
	{
		this.ModDamage = 1.0;
		this.ModSpeed = 1f;
		this.ModSize = 1f;
		foreach (BulletEffects.SingleEffect singleEffect in this.effects)
		{
			this.ModDamage *= (double)singleEffect.abilityMods[0];
			this.ModSpeed *= singleEffect.abilityMods[1];
			this.ModSize *= singleEffect.abilityMods[2];
		}
		if (this.ModDamage <= 0.0 || this.ModSpeed <= 0f || this.ModSize <= 0f)
		{
			Debug.LogError("Error_Modxxx<=0!");
		}
		if (Battle.inst.specialEffect[79] >= 1)
		{
			this.ModSize = 1f;
		}
	}

	// Token: 0x0400071C RID: 1820
	[SerializeField]
	public BulletEffects.SingleEffect[] effects;

	// Token: 0x0400071D RID: 1821
	[SerializeField]
	public List<int> listEffectInts = new List<int>();

	// Token: 0x0400071E RID: 1822
	public double ModDamage = 1.0;

	// Token: 0x0400071F RID: 1823
	public float ModSpeed = 1f;

	// Token: 0x04000720 RID: 1824
	public float ModSize = 1f;

	// Token: 0x04000721 RID: 1825
	private DataBase dataBase;

	// Token: 0x02000164 RID: 356
	[Serializable]
	public class SingleEffect
	{
		// Token: 0x06000A0F RID: 2575 RVA: 0x000378A4 File Offset: 0x00035AA4
		public void Reset()
		{
			this.id = -1;
			this.ifUsed = false;
			this.enabled = false;
			if (this.abilityMods == null)
			{
				Debug.Log("null");
				this.abilityMods = new float[]
				{
					1f,
					1f,
					1f
				};
			}
			if (this.abilityMods.Length != 3)
			{
				Debug.Log("length??");
				this.abilityMods = new float[]
				{
					1f,
					1f,
					1f
				};
				return;
			}
			for (int i = 0; i < this.abilityMods.Length; i++)
			{
				this.abilityMods[i] = 1f;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0003793C File Offset: 0x00035B3C
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x00037946 File Offset: 0x00035B46
		public float ModDamage
		{
			get
			{
				return this.abilityMods[0];
			}
			set
			{
				this.abilityMods[0] = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00037951 File Offset: 0x00035B51
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0003795B File Offset: 0x00035B5B
		public float modSpeed
		{
			get
			{
				return this.abilityMods[1];
			}
			set
			{
				this.abilityMods[1] = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00037966 File Offset: 0x00035B66
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x00037970 File Offset: 0x00035B70
		public float modSize
		{
			get
			{
				return this.abilityMods[2];
			}
			set
			{
				this.abilityMods[2] = value;
			}
		}

		// Token: 0x04000A1E RID: 2590
		public int id = -1;

		// Token: 0x04000A1F RID: 2591
		public bool ifUsed;

		// Token: 0x04000A20 RID: 2592
		public bool enabled;

		// Token: 0x04000A21 RID: 2593
		public float[] abilityMods = new float[]
		{
			1f,
			1f,
			1f
		};

		// Token: 0x04000A22 RID: 2594
		public float triggerRange = 1f;
	}
}

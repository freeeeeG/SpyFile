using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class MonsterManager : Singleton<MonsterManager>
{
	// Token: 0x060003FD RID: 1021 RVA: 0x0000FB50 File Offset: 0x0000DD50
	private new void Awake()
	{
		base.Awake();
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterSpawn, new Action<AMonsterBase>(this.OnMonsterSpawn));
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterImpendingDeath, new Action<AMonsterBase>(this.OnMonsterImpendingDeath));
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterKilled, new Action<AMonsterBase>(this.OnMonsterKilled));
		EventMgr.Register<AMonsterBase>(eGameEvents.MonsterDespawn, new Action<AMonsterBase>(this.OnMonsterDespawn));
		this.list_MonsterOnField = new List<AMonsterBase>();
		this.list_MonsterAttackable = new List<AMonsterBase>();
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0000FBD9 File Offset: 0x0000DDD9
	private void Update()
	{
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0000FBDB File Offset: 0x0000DDDB
	private void OnMonsterSpawn(AMonsterBase monster)
	{
		this.list_MonsterOnField.Add(monster);
		this.list_MonsterAttackable.Add(monster);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0000FBF5 File Offset: 0x0000DDF5
	private void OnMonsterImpendingDeath(AMonsterBase monster)
	{
		this.OnMonsterKilled(monster);
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0000FBFE File Offset: 0x0000DDFE
	private void OnMonsterKilled(AMonsterBase monster)
	{
		if (this.list_MonsterAttackable.Contains(monster))
		{
			this.list_MonsterAttackable.Remove(monster);
		}
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0000FC1B File Offset: 0x0000DE1B
	private void OnMonsterDespawn(AMonsterBase monster)
	{
		if (this.list_MonsterAttackable.Contains(monster))
		{
			this.list_MonsterAttackable.Remove(monster);
		}
		this.list_MonsterOnField.Remove(monster);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0000FC45 File Offset: 0x0000DE45
	public int GetMonsterOnFieldCount()
	{
		return this.list_MonsterOnField.Count;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0000FC52 File Offset: 0x0000DE52
	public List<AMonsterBase> GetMonsterOnField()
	{
		return this.list_MonsterOnField;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0000FC5A File Offset: 0x0000DE5A
	public AMonsterBase GetBoss()
	{
		return this.list_MonsterOnField.Find((AMonsterBase a) => a.MonsterData.Size == eMonsterSize.BOSS);
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0000FC88 File Offset: 0x0000DE88
	public int GetMonsterOnFieldCountWithoutBoss()
	{
		List<AMonsterBase> list = this.list_MonsterOnField.FindAll((AMonsterBase a) => a.MonsterData.Size == eMonsterSize.BOSS);
		return this.list_MonsterOnField.Count - list.Count;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0000FCD4 File Offset: 0x0000DED4
	public List<AMonsterBase> GetMonstersInRange(Vector3 center, float range)
	{
		List<AMonsterBase> list = new List<AMonsterBase>();
		foreach (AMonsterBase amonsterBase in this.list_MonsterAttackable)
		{
			if (!(amonsterBase == null) && Vector3.SqrMagnitude(amonsterBase.transform.position - center) < range * range)
			{
				list.Add(amonsterBase);
			}
		}
		return list;
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0000FD54 File Offset: 0x0000DF54
	public AMonsterBase GetTargetByTowerPriority(eTowerTargetPriority type, Vector3 center, float range)
	{
		switch (type)
		{
		default:
			return this.GetMostProgressMonsterInRange(center, range);
		case eTowerTargetPriority.HIGHEST_HP:
			return this.GetHighestHPMonsterInRange(center, range);
		case eTowerTargetPriority.LOWEST_HP:
			return this.GetLowestHPMonsterInRange(center, range);
		case eTowerTargetPriority.NEAREST:
			return this.GetClosestMonsterInRange(center, range);
		case eTowerTargetPriority.FARTHEST:
			return this.GetFarthestMonsterInRange(center, range);
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
	public AMonsterBase GetClosestMonsterInRange(Vector3 center, float range)
	{
		AMonsterBase result = null;
		float num = float.PositiveInfinity;
		foreach (AMonsterBase amonsterBase in this.list_MonsterAttackable)
		{
			if (!(amonsterBase == null))
			{
				float num2 = Vector3.SqrMagnitude(amonsterBase.transform.position - center);
				if (num2 < range * range && num2 < num)
				{
					num = num2;
					result = amonsterBase;
				}
			}
		}
		return result;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0000FE38 File Offset: 0x0000E038
	public AMonsterBase GetFarthestMonsterInRange(Vector3 center, float range)
	{
		AMonsterBase result = null;
		float num = 0f;
		foreach (AMonsterBase amonsterBase in this.list_MonsterAttackable)
		{
			if (!(amonsterBase == null))
			{
				float num2 = Vector3.SqrMagnitude(amonsterBase.transform.position - center);
				if (num2 < range * range && num2 > num)
				{
					num = num2;
					result = amonsterBase;
				}
			}
		}
		return result;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0000FEC8 File Offset: 0x0000E0C8
	public AMonsterBase GetMostProgressMonsterInRange(Vector3 center, float range)
	{
		AMonsterBase result = null;
		float num = float.PositiveInfinity;
		foreach (AMonsterBase amonsterBase in this.list_MonsterAttackable)
		{
			if (!(amonsterBase == null) && Vector3.SqrMagnitude(amonsterBase.transform.position - center) < range * range)
			{
				float remainingDistance = amonsterBase.RemainingDistance;
				if (remainingDistance < num || (num == float.PositiveInfinity && remainingDistance == float.PositiveInfinity))
				{
					num = remainingDistance;
					result = amonsterBase;
				}
			}
		}
		return result;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0000FF6C File Offset: 0x0000E16C
	public AMonsterBase GetHighestHPMonsterInRange(Vector3 center, float range)
	{
		AMonsterBase result = null;
		float num = -1f;
		foreach (AMonsterBase amonsterBase in this.list_MonsterAttackable)
		{
			if (!(amonsterBase == null) && Vector3.SqrMagnitude(amonsterBase.transform.position - center) < range * range)
			{
				float num2 = (float)amonsterBase.GetHP();
				if (num2 > num)
				{
					num = num2;
					result = amonsterBase;
				}
			}
		}
		return result;
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00010000 File Offset: 0x0000E200
	public AMonsterBase GetLowestHPMonsterInRange(Vector3 center, float range)
	{
		AMonsterBase result = null;
		float num = float.PositiveInfinity;
		foreach (AMonsterBase amonsterBase in this.list_MonsterAttackable)
		{
			if (!(amonsterBase == null) && Vector3.SqrMagnitude(amonsterBase.transform.position - center) < range * range)
			{
				float num2 = (float)amonsterBase.GetHP();
				if (num2 < num)
				{
					num = num2;
					result = amonsterBase;
				}
			}
		}
		return result;
	}

	// Token: 0x04000414 RID: 1044
	[SerializeField]
	private List<AMonsterBase> list_MonsterOnField;

	// Token: 0x04000415 RID: 1045
	[SerializeField]
	private List<AMonsterBase> list_MonsterAttackable;
}

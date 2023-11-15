using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/MonsterSettingData", order = 1)]
public class MonsterSettingData : ScriptableObject
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000479C File Offset: 0x0000299C
	public eMonsterType Type
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x000047A4 File Offset: 0x000029A4
	public eMonsterSize Size
	{
		get
		{
			return this.size;
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000047AC File Offset: 0x000029AC
	public eMonsterType GetMonsterType()
	{
		return this.type;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x000047B4 File Offset: 0x000029B4
	public eMonsterSize GetMonsterSize()
	{
		return this.size;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000047BC File Offset: 0x000029BC
	public GameObject GetPrefab()
	{
		return this.prefab;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x000047C4 File Offset: 0x000029C4
	public int GetMaxHP(float multiplier = 1f)
	{
		return (int)((float)this.baseHP * multiplier);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x000047D0 File Offset: 0x000029D0
	public float GetMinMoveSpeed()
	{
		return this.moveSpeed_Min;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000047D8 File Offset: 0x000029D8
	public float GetAverageMoveSpeed()
	{
		return (this.moveSpeed_Min + this.moveSpeed_Max) / 2f;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000047ED File Offset: 0x000029ED
	public float GetMoveSpeed(float multiplier = 1f)
	{
		if (this.moveSpeed_Min == this.moveSpeed_Max)
		{
			return this.moveSpeed_Min * multiplier;
		}
		return Random.Range(this.moveSpeed_Min, this.moveSpeed_Max) * multiplier;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00004819 File Offset: 0x00002A19
	public int GetReward(float multiplier = 1f)
	{
		return (int)((float)this.baseReward * multiplier);
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00004825 File Offset: 0x00002A25
	public float GetSpawnCountModifier()
	{
		return this.spawnCountModifier;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000482D File Offset: 0x00002A2D
	public float GetSpawnIntervalModifier()
	{
		return this.spawnIntervalModifier;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00004835 File Offset: 0x00002A35
	public bool IsAvaliableInWorld(eWorldType world)
	{
		return (this.avaliableWorldType & world) == world;
	}

	// Token: 0x0400008C RID: 140
	[SerializeField]
	private eMonsterType type;

	// Token: 0x0400008D RID: 141
	[SerializeField]
	private eMonsterSize size;

	// Token: 0x0400008E RID: 142
	[SerializeField]
	private eMonsterPathType pathType;

	// Token: 0x0400008F RID: 143
	[SerializeField]
	[Header("可以在哪些場景出現")]
	private eWorldType avaliableWorldType;

	// Token: 0x04000090 RID: 144
	[SerializeField]
	private GameObject prefab;

	// Token: 0x04000091 RID: 145
	[SerializeField]
	private int baseHP = 1;

	// Token: 0x04000092 RID: 146
	[SerializeField]
	private float moveSpeed_Min = 1f;

	// Token: 0x04000093 RID: 147
	[SerializeField]
	private float moveSpeed_Max = 1f;

	// Token: 0x04000094 RID: 148
	[SerializeField]
	private int baseReward = 1;

	// Token: 0x04000095 RID: 149
	[SerializeField]
	[Header("產生的時候, 數量的增減幅程度")]
	private float spawnCountModifier = 1f;

	// Token: 0x04000096 RID: 150
	[SerializeField]
	[Header("產生的時候, 出現頻率的增減幅程度")]
	private float spawnIntervalModifier = 1f;
}

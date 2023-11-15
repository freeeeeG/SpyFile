using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ContentAttribute
{
	// Token: 0x0400005F RID: 95
	public EnemyType EnemyType;

	// Token: 0x04000060 RID: 96
	public bool IsBoss;

	// Token: 0x04000061 RID: 97
	public int InitCount;

	// Token: 0x04000062 RID: 98
	public float CountIncrease;

	// Token: 0x04000063 RID: 99
	public int MaxAmount;

	// Token: 0x04000064 RID: 100
	public float Health;

	// Token: 0x04000065 RID: 101
	public float Speed;

	// Token: 0x04000066 RID: 102
	public float CoolDown;

	// Token: 0x04000067 RID: 103
	public float Frost;

	// Token: 0x04000068 RID: 104
	public string BackGround;

	// Token: 0x04000069 RID: 105
	[Header("Tips参数")]
	public int HealthAtt;

	// Token: 0x0400006A RID: 106
	public int SpeedAtt;

	// Token: 0x0400006B RID: 107
	public int AmountAtt;

	// Token: 0x0400006C RID: 108
	public int ReachDamage;

	// Token: 0x0400006D RID: 109
	[Header("Boss对白")]
	public string[] BossDialogues;
}

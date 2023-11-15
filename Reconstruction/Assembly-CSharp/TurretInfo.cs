using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
[Serializable]
public class TurretInfo
{
	// Token: 0x04000093 RID: 147
	public int AttackRange;

	// Token: 0x04000094 RID: 148
	public int ForbidRange;

	// Token: 0x04000095 RID: 149
	public float AttackDamage;

	// Token: 0x04000096 RID: 150
	public float AttackSpeed;

	// Token: 0x04000097 RID: 151
	public float SplashRange;

	// Token: 0x04000098 RID: 152
	public float CriticalRate;

	// Token: 0x04000099 RID: 153
	public float SlowRate;

	// Token: 0x0400009A RID: 154
	public float DamageIntensify;

	// Token: 0x0400009B RID: 155
	[Header("美术资源设置")]
	public Sprite TurretIcon;

	// Token: 0x0400009C RID: 156
	public Sprite CannonSprite;

	// Token: 0x0400009D RID: 157
	public Vector2 ShootPointOffset;
}

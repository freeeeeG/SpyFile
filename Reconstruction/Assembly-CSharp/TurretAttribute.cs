using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000023 RID: 35
[CreateAssetMenu(menuName = "Attribute/TurretAttribute", fileName = "TurretAttribute")]
public class TurretAttribute : ContentAttribute
{
	// Token: 0x040000BF RID: 191
	[Header("基础参数")]
	public StrategyType StrategyType;

	// Token: 0x040000C0 RID: 192
	public RangeType RangeType;

	// Token: 0x040000C1 RID: 193
	public ElementType element;

	// Token: 0x040000C2 RID: 194
	public Bullet Bullet;

	// Token: 0x040000C3 RID: 195
	public float BulletSpeed;

	// Token: 0x040000C4 RID: 196
	public AudioClip ShootSound;

	// Token: 0x040000C5 RID: 197
	[Header("合成塔参数")]
	public int Rare;

	// Token: 0x040000C6 RID: 198
	public int totalLevel;

	// Token: 0x040000C7 RID: 199
	public int elementNumber;

	// Token: 0x040000C8 RID: 200
	public int maxElementLevel;

	// Token: 0x040000C9 RID: 201
	public int minElementLevel;

	// Token: 0x040000CA RID: 202
	public List<TurretInfo> TurretLevels = new List<TurretInfo>();

	// Token: 0x040000CB RID: 203
	[Header("技能参数")]
	public RefactorTurretName RefactorName;
}

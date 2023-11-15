using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class SixArmour : Boss
{
	// Token: 0x1700024C RID: 588
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x0000E3E9 File Offset: 0x0000C5E9
	public override EnemyType EnemyType
	{
		get
		{
			return EnemyType.SixArmor;
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0000E3F0 File Offset: 0x0000C5F0
	public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify, float dmgResist, List<PathPoint> pathPoints)
	{
		base.Initialize(pathIndex, attribute, pathOffset, intensify, dmgResist, pathPoints);
		this.m_ArmorHolder = Object.Instantiate<ArmorHolder>(this.armorHolderPrefab, base.gfxSprite.transform);
		this.m_ArmorHolder.Initialize(this, base.DamageStrategy.MaxHealth * this.armorIntensify);
		base.ShowBossText(0.5f);
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0000E451 File Offset: 0x0000C651
	public override void OnDie()
	{
		base.OnDie();
		Singleton<LevelManager>.Instance.SetAchievement("ACH_TORTOISE");
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0000E468 File Offset: 0x0000C668
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		Object.Destroy(this.m_ArmorHolder.gameObject);
		this.m_ArmorHolder = null;
	}

	// Token: 0x0400021C RID: 540
	[SerializeField]
	private float armorIntensify;

	// Token: 0x0400021D RID: 541
	[SerializeField]
	private ArmorHolder armorHolderPrefab;

	// Token: 0x0400021E RID: 542
	private ArmorHolder m_ArmorHolder;
}

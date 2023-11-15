using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class TechCombatSkill : GlobalSkill
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600027D RID: 637 RVA: 0x00008A52 File Offset: 0x00006C52
	public override GlobalSkillName GlobalSkillName
	{
		get
		{
			return GlobalSkillName.TechCombatSkill;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x0600027E RID: 638 RVA: 0x00008A55 File Offset: 0x00006C55
	public override float KeyValue
	{
		get
		{
			return 0.6f;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600027F RID: 639 RVA: 0x00008A5C File Offset: 0x00006C5C
	public override float KeyValue2
	{
		get
		{
			return 0.4f;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000280 RID: 640 RVA: 0x00008A63 File Offset: 0x00006C63
	public override float KeyValue3
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00008A6A File Offset: 0x00006C6A
	public override void Build()
	{
		base.Build();
		if (!this.IsAbnormal)
		{
			this.strategy.BaseFixDamageIntensify += this.KeyValue;
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00008A94 File Offset: 0x00006C94
	public override void Detect()
	{
		base.Detect();
		if (this.IsAbnormal)
		{
			this.strategy.BaseFixDamageIntensify -= this.intensifiedValue;
			this.intensifiedValue = 0f;
			List<Vector2Int> circlePoints = StaticData.GetCirclePoints(1, 0);
			this.turretCount = 0;
			foreach (Vector2Int v in circlePoints)
			{
				Collider2D collider2D = StaticData.RaycastCollider(v + this.strategy.Concrete.transform.position, LayerMask.GetMask(new string[]
				{
					StaticData.TurretMask
				}));
				if (collider2D != null && collider2D.GetComponent<RefactorTurret>() != null)
				{
					this.turretCount++;
				}
			}
			this.intensifiedValue = (float)this.turretCount * this.KeyValue2 - (float)(4 - this.turretCount) * this.KeyValue3;
			this.strategy.BaseFixDamageIntensify += this.intensifiedValue;
		}
	}

	// Token: 0x04000168 RID: 360
	private float intensifiedValue;

	// Token: 0x04000169 RID: 361
	private int turretCount;
}

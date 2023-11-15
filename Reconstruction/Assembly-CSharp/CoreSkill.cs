using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class CoreSkill : InitialSkill
{
	// Token: 0x170001EB RID: 491
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000B0AF File Offset: 0x000092AF
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Core;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000B0B3 File Offset: 0x000092B3
	public override float KeyValue
	{
		get
		{
			return 0.8f;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000B0BA File Offset: 0x000092BA
	public override float KeyValue2
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000B0C4 File Offset: 0x000092C4
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000B100 File Offset: 0x00009300
	public override string DisplayValue2
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized((this.KeyValue2 * 100f).ToString() + "%");
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000B13B File Offset: 0x0000933B
	public override string DisplayValue3
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.DetectRange.ToString());
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0000B158 File Offset: 0x00009358
	public override void StartTurn2()
	{
		base.StartTurn2();
		foreach (Vector2Int v in StaticData.GetCirclePoints(this.DetectRange, 0))
		{
			Collider2D collider2D = StaticData.RaycastCollider(v + this.strategy.Concrete.transform.position, LayerMask.GetMask(new string[]
			{
				StaticData.TurretMask
			}));
			if (collider2D != null)
			{
				RefactorTurret component = collider2D.GetComponent<RefactorTurret>();
				if (component != null)
				{
					component.Strategy.TurnAttackIntensify -= this.KeyValue;
					this.strategy.TurnAttackIntensify += this.KeyValue2;
				}
			}
		}
	}

	// Token: 0x04000190 RID: 400
	public int DetectRange = 1;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class MinerSkill : InitialSkill
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000BB5E File Offset: 0x00009D5E
	public override RefactorTurretName EffectName
	{
		get
		{
			return RefactorTurretName.Miner;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000BB62 File Offset: 0x00009D62
	public override float KeyValue
	{
		get
		{
			return this.DeployInterval;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000BB6C File Offset: 0x00009D6C
	public override string DisplayValue
	{
		get
		{
			return StaticData.ElementDIC[ElementType.None].Colorized(this.KeyValue.ToString());
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0000BB97 File Offset: 0x00009D97
	public override void Build()
	{
		base.Build();
		this.pathTiles = new List<BasicTile>();
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0000BBAA File Offset: 0x00009DAA
	public override void StartTurn3()
	{
		base.StartTurn3();
		this.GetPathTiles();
		((Miner)this.strategy.Concrete).DeployInterval = this.KeyValue;
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0000BBD3 File Offset: 0x00009DD3
	public override void EndTurn()
	{
		base.EndTurn();
		this.pathTiles.Clear();
		((Miner)this.strategy.Concrete).PathTiles = this.pathTiles;
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0000BC04 File Offset: 0x00009E04
	private void GetPathTiles()
	{
		this.pathTiles.Clear();
		foreach (Vector2Int v in StaticData.GetCirclePoints(this.strategy.FinalRange, 0))
		{
			Collider2D collider2D = StaticData.RaycastCollider(v + this.strategy.Concrete.transform.position, LayerMask.GetMask(new string[]
			{
				StaticData.ConcreteTileMask
			}));
			if (collider2D != null)
			{
				BasicTile component = collider2D.GetComponent<BasicTile>();
				if (component.isPath)
				{
					this.pathTiles.Add(component);
				}
			}
		}
		((Miner)this.strategy.Concrete).PathTiles = this.pathTiles;
	}

	// Token: 0x0400019B RID: 411
	private List<BasicTile> pathTiles;

	// Token: 0x0400019C RID: 412
	public float DeployInterval = 7f;
}

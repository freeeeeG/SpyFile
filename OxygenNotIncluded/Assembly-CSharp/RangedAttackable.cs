using System;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
public class RangedAttackable : AttackableBase
{
	// Token: 0x06001DAC RID: 7596 RVA: 0x0009DDB8 File Offset: 0x0009BFB8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x0009DDC0 File Offset: 0x0009BFC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.preferUnreservedCell = true;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x0009DDDA File Offset: 0x0009BFDA
	public new int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x0009DDE4 File Offset: 0x0009BFE4
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 0.5f, 0.5f, 0.15f);
		foreach (CellOffset offset in base.GetOffsets())
		{
			Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset)), Vector3.one);
		}
	}
}

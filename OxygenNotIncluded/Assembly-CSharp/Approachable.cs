using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Approachable")]
public class Approachable : KMonoBehaviour, IApproachable
{
	// Token: 0x0600193A RID: 6458 RVA: 0x000840D9 File Offset: 0x000822D9
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000840E0 File Offset: 0x000822E0
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}

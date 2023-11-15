using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
[AddComponentMenu("KMonoBehaviour/scripts/Chattable")]
public class Chattable : KMonoBehaviour, IApproachable
{
	// Token: 0x06001A02 RID: 6658 RVA: 0x00089C5B File Offset: 0x00087E5B
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Chat;
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x00089C62 File Offset: 0x00087E62
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}

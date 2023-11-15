using System;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryInput")]
public class ConduitSecondaryInput : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06002525 RID: 9509 RVA: 0x000CAC37 File Offset: 0x000C8E37
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000CAC47 File Offset: 0x000C8E47
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x0400154A RID: 5450
	[SerializeField]
	public ConduitPortInfo portInfo;
}

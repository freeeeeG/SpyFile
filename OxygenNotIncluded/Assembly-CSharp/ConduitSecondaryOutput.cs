using System;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryOutput")]
public class ConduitSecondaryOutput : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06002528 RID: 9512 RVA: 0x000CAC70 File Offset: 0x000C8E70
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000CAC80 File Offset: 0x000C8E80
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.portInfo.conduitType)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x0400154B RID: 5451
	[SerializeField]
	public ConduitPortInfo portInfo;
}

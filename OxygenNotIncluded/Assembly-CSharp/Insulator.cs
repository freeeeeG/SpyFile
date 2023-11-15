using System;
using UnityEngine;

// Token: 0x02000808 RID: 2056
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
	// Token: 0x06003AEB RID: 15083 RVA: 0x0014733A File Offset: 0x0014553A
	protected override void OnSpawn()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x0014736C File Offset: 0x0014556C
	protected override void OnCleanUp()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), 1f);
	}

	// Token: 0x04002705 RID: 9989
	[MyCmpReq]
	private Building building;

	// Token: 0x04002706 RID: 9990
	[SerializeField]
	public CellOffset offset = CellOffset.none;
}

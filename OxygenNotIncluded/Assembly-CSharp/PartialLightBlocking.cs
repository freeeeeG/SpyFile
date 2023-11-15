using System;
using KSerialization;

// Token: 0x0200066B RID: 1643
[SerializationConfig(MemberSerialization.OptIn)]
public class PartialLightBlocking : KMonoBehaviour
{
	// Token: 0x06002B79 RID: 11129 RVA: 0x000E7271 File Offset: 0x000E5471
	protected override void OnSpawn()
	{
		this.SetLightBlocking();
		base.OnSpawn();
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000E727F File Offset: 0x000E547F
	protected override void OnCleanUp()
	{
		this.ClearLightBlocking();
		base.OnCleanUp();
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x000E7290 File Offset: 0x000E5490
	public void SetLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.SetCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x000E72C4 File Offset: 0x000E54C4
	public void ClearLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ClearCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x0400197C RID: 6524
	private const byte PartialLightBlockingProperties = 48;
}

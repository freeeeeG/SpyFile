using System;
using UnityEngine;

// Token: 0x020003C4 RID: 964
[AddComponentMenu("Scripts/Game/Player/HeldItemMeshVisibility")]
public class HeldItemMeshVisibility : MeshVisibilityBase<HeldItemMeshVisibility.VisState>
{
	// Token: 0x020003C5 RID: 965
	public enum VisState
	{
		// Token: 0x04000DF8 RID: 3576
		Carrying,
		// Token: 0x04000DF9 RID: 3577
		NotCarrying
	}
}

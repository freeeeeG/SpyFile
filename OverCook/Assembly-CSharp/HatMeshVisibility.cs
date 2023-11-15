using System;
using UnityEngine;

// Token: 0x020009E6 RID: 2534
[ExecutionDependency(typeof(ChefMeshReplacer))]
[AddComponentMenu("Scripts/Game/Player/HatMeshVisibility")]
public class HatMeshVisibility : MeshVisibilityBase<HatMeshVisibility.VisState>
{
	// Token: 0x040027D6 RID: 10198
	[SerializeField]
	public HatMeshVisibility.VisState m_initialVisState = HatMeshVisibility.VisState.Fancy;

	// Token: 0x020009E7 RID: 2535
	public enum VisState
	{
		// Token: 0x040027D8 RID: 10200
		None,
		// Token: 0x040027D9 RID: 10201
		Cap,
		// Token: 0x040027DA RID: 10202
		Tall,
		// Token: 0x040027DB RID: 10203
		Fancy,
		// Token: 0x040027DC RID: 10204
		Festive,
		// Token: 0x040027DD RID: 10205
		Baseball
	}
}

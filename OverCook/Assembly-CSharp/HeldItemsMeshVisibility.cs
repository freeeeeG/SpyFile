using System;
using UnityEngine;

// Token: 0x020009EA RID: 2538
[ExecutionDependency(typeof(ChefMeshReplacer))]
[AddComponentMenu("Scripts/Game/Player/HeldItemsMeshVisibility")]
public class HeldItemsMeshVisibility : MeshVisibilityBase<HeldItemsMeshVisibility.VisState>
{
	// Token: 0x06003196 RID: 12694 RVA: 0x000E84F4 File Offset: 0x000E68F4
	protected override Renderer FindMesh(string _name, Renderer[] renderers = null)
	{
		if (renderers == null)
		{
			renderers = base.gameObject.RequestComponentsRecursive<Renderer>();
		}
		foreach (Renderer renderer in renderers)
		{
			if (renderer.gameObject.activeInHierarchy && renderer.name.StartsWith(_name))
			{
				return renderer;
			}
		}
		return null;
	}

	// Token: 0x020009EB RID: 2539
	public enum VisState
	{
		// Token: 0x040027DF RID: 10207
		Chopping,
		// Token: 0x040027E0 RID: 10208
		Carrying,
		// Token: 0x040027E1 RID: 10209
		Idle,
		// Token: 0x040027E2 RID: 10210
		Washing,
		// Token: 0x040027E3 RID: 10211
		Repairing
	}
}

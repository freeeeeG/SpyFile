using System;
using UnityEngine;

// Token: 0x02000A26 RID: 2598
[ExecutionDependency(typeof(ChefMeshReplacer))]
[AddComponentMenu("Scripts/Game/Player/TailMeshVisibility")]
public class TailMeshVisibility : MeshVisibilityBase<TailMeshVisibility.VisState>
{
	// Token: 0x0600337D RID: 13181 RVA: 0x000F26D4 File Offset: 0x000F0AD4
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

	// Token: 0x0400296F RID: 10607
	[SerializeField]
	public TailMeshVisibility.VisState m_initialVisState = TailMeshVisibility.VisState.Visible;

	// Token: 0x02000A27 RID: 2599
	public enum VisState
	{
		// Token: 0x04002971 RID: 10609
		Hidden,
		// Token: 0x04002972 RID: 10610
		Visible
	}
}

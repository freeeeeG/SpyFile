using System;
using UnityEngine;

// Token: 0x020009D3 RID: 2515
[ExecutionDependency(typeof(ChefMeshReplacer))]
[AddComponentMenu("Scripts/Game/Player/BodyMeshVisibility")]
public class BodyMeshVisibility : MeshVisibilityBase<BodyMeshVisibility.VisState>
{
	// Token: 0x06003141 RID: 12609 RVA: 0x000E6CE9 File Offset: 0x000E50E9
	private void Awake()
	{
		if (this.m_stateFlags.Length == 2)
		{
			this.m_stateFlags[1] = (int)Mathf.Pow(2f, (float)this.m_meshes.Length) - 1;
		}
	}

	// Token: 0x06003142 RID: 12610 RVA: 0x000E6D18 File Offset: 0x000E5118
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

	// Token: 0x04002771 RID: 10097
	[SerializeField]
	public BodyMeshVisibility.VisState m_initialVisState = BodyMeshVisibility.VisState.Visible;

	// Token: 0x020009D4 RID: 2516
	public enum VisState
	{
		// Token: 0x04002773 RID: 10099
		Hidden,
		// Token: 0x04002774 RID: 10100
		Visible
	}
}

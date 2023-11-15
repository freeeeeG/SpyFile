using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014B RID: 331
public abstract class MeshVisibilityBase<StateEnum> : MonoBehaviour where StateEnum : struct, IConvertible
{
	// Token: 0x060005E4 RID: 1508 RVA: 0x0002B66C File Offset: 0x00029A6C
	public void Setup(StateEnum _state)
	{
		this.m_stateValues = (StateEnum[])Enum.GetValues(typeof(StateEnum));
		Renderer[] renderers = base.gameObject.RequestComponentsRecursive<Renderer>();
		this.m_renderers.Clear();
		for (int i = 0; i < this.m_meshes.Length; i++)
		{
			Renderer renderer = this.FindMesh(this.m_meshes[i], renderers);
			if (renderer != null)
			{
				this.m_renderers.Add(this.m_meshes[i], renderer);
			}
		}
		this.SetState(_state);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0002B6FC File Offset: 0x00029AFC
	public void SetState(StateEnum _state)
	{
		int num = 0;
		for (int i = 0; i < this.m_stateValues.Length; i++)
		{
			if (this.m_stateValues[i].Equals(_state))
			{
				int num2 = (this.m_stateFlags.Length <= num) ? 0 : this.m_stateFlags[num];
				for (int j = 0; j < this.m_meshes.Length; j++)
				{
					bool visibility = (num2 & 1 << j) != 0;
					this.SetMeshVisibility(this.m_meshes[j], visibility);
				}
			}
			num++;
		}
		this.m_state = _state;
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002B7AC File Offset: 0x00029BAC
	private void SetMeshVisibility(string _mesh, bool _visibility)
	{
		if (this.m_renderers.ContainsKey(_mesh))
		{
			Renderer renderer = this.m_renderers[_mesh];
			renderer.enabled = _visibility;
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002B7E0 File Offset: 0x00029BE0
	protected virtual Renderer FindMesh(string _name, Renderer[] renderers = null)
	{
		if (renderers == null)
		{
			renderers = base.gameObject.RequestComponentsRecursive<Renderer>();
		}
		foreach (Renderer renderer in renderers)
		{
			if (renderer.name.Equals(_name))
			{
				return renderer;
			}
		}
		return null;
	}

	// Token: 0x040004F2 RID: 1266
	[SerializeField]
	public string[] m_meshes = new string[0];

	// Token: 0x040004F3 RID: 1267
	[HideInInspector]
	[SerializeField]
	public int[] m_stateFlags = new int[0];

	// Token: 0x040004F4 RID: 1268
	private Dictionary<string, Renderer> m_renderers = new Dictionary<string, Renderer>();

	// Token: 0x040004F5 RID: 1269
	private StateEnum m_state;

	// Token: 0x040004F6 RID: 1270
	private StateEnum[] m_stateValues;
}

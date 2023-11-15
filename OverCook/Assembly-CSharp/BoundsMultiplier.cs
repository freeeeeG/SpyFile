using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
[RequireComponent(typeof(MeshRenderer))]
public class BoundsMultiplier : MonoBehaviour
{
	// Token: 0x06000569 RID: 1385 RVA: 0x0002A250 File Offset: 0x00028650
	private void Awake()
	{
		MeshFilter component = base.gameObject.GetComponent<MeshFilter>();
		Bounds bounds = component.mesh.bounds;
		component.mesh.bounds = new Bounds(bounds.center, bounds.size.MultipliedBy(this.m_multiplier));
	}

	// Token: 0x040004A2 RID: 1186
	[SerializeField]
	private Vector3 m_multiplier = Vector3.one;
}

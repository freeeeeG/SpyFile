using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class BoundContainer : MonoBehaviour
{
	// Token: 0x060004A3 RID: 1187 RVA: 0x00027A99 File Offset: 0x00025E99
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawCube(base.transform.position, this.m_boundSize);
	}

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	public Vector3 m_boundSize;
}

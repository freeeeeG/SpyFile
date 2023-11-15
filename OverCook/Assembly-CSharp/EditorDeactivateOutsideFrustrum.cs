using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class EditorDeactivateOutsideFrustrum : MonoBehaviour
{
	// Token: 0x060007B5 RID: 1973 RVA: 0x00030174 File Offset: 0x0002E574
	private void Cull()
	{
		if (this.m_comparisonCamera == null)
		{
			return;
		}
		foreach (Renderer renderer in base.gameObject.RequestComponentsRecursive<Renderer>())
		{
			if (!this.InFrustrum(this.m_comparisonCamera, renderer.bounds))
			{
				renderer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000301DC File Offset: 0x0002E5DC
	private bool InFrustrum(Camera _camera, Bounds _bounds)
	{
		return this.InFrustrum(_camera, _bounds.center + new Vector3(-1f, -1f, -1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(-1f, -1f, 1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(-1f, 1f, -1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(-1f, 1f, 1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(1f, -1f, -1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(1f, -1f, 1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(1f, 1f, -1f).MultipliedBy(_bounds.extents)) || this.InFrustrum(_camera, _bounds.center + new Vector3(1f, 1f, 1f).MultipliedBy(_bounds.extents));
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x000303A8 File Offset: 0x0002E7A8
	private bool InFrustrum(Camera _camera, Vector3 _pos)
	{
		Vector3 vector = _camera.WorldToScreenPoint(_pos);
		float num = (float)_camera.pixelWidth;
		float num2 = (float)_camera.pixelHeight;
		return vector.x > (0.5f - 0.5f * this.m_widthModifier) * num && vector.x < (0.5f + 0.5f * this.m_widthModifier) * num && vector.y > (0.5f - 0.5f * this.m_heightModifier) * num2 && vector.y < (0.5f + 0.5f * this.m_heightModifier) * num2 && vector.z > _camera.nearClipPlane && vector.z < _camera.farClipPlane;
	}

	// Token: 0x0400061E RID: 1566
	[SerializeField]
	private Camera m_comparisonCamera;

	// Token: 0x0400061F RID: 1567
	[SerializeField]
	private bool m_executeCull;

	// Token: 0x04000620 RID: 1568
	[SerializeField]
	private float m_widthModifier = 1.25f;

	// Token: 0x04000621 RID: 1569
	[SerializeField]
	private float m_heightModifier = 1.25f;
}

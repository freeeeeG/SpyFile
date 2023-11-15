using System;
using UnityEngine;

// Token: 0x020005BF RID: 1471
[AddComponentMenu("Scripts/Game/Environment/Steppable")]
[RequireComponent(typeof(Collider))]
public class Steppable : MonoBehaviour
{
	// Token: 0x06001BEA RID: 7146 RVA: 0x000885F9 File Offset: 0x000869F9
	private void Awake()
	{
		this.m_collider = base.gameObject.RequireComponent<Collider>();
		this.m_plane = new Plane(this.m_planeTransform.up, this.m_planeTransform.position);
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00088630 File Offset: 0x00086A30
	public Vector3 ProjectPointOntoStep(Vector3 _point, Vector3 _directionXYZ)
	{
		Vector3 result;
		if (this.RaycastOntoPlane(_point, Vector3.up, out result))
		{
			return result;
		}
		return _point;
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x00088654 File Offset: 0x00086A54
	private bool RaycastOntoCollision(Vector3 _point, Vector3 _direction, out Vector3 _hit)
	{
		Ray ray = new Ray(_point, _direction);
		RaycastHit raycastHit;
		if (this.m_collider.Raycast(ray, out raycastHit, this.c_raycastMaxDistance))
		{
			_hit = raycastHit.point;
			return true;
		}
		Debug.DrawRay(ray.origin, ray.direction, Color.red);
		_hit = _point;
		return false;
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x000886B4 File Offset: 0x00086AB4
	private bool RaycastOntoPlane(Vector3 _point, Vector3 _direction, out Vector3 _hit)
	{
		Ray ray = new Ray(_point, _direction);
		float num = 0f;
		if (!this.m_plane.Raycast(ray, out num) && Mathf.Abs(num) <= 0f)
		{
			_hit = _point;
			return false;
		}
		_hit = ray.origin + ray.direction * num;
		return true;
	}

	// Token: 0x040015E4 RID: 5604
	[SerializeField]
	[AssignChild("PlaneTransform", Editorbility.NonEditable)]
	private Transform m_planeTransform;

	// Token: 0x040015E5 RID: 5605
	private Plane m_plane;

	// Token: 0x040015E6 RID: 5606
	private Collider m_collider;

	// Token: 0x040015E7 RID: 5607
	private float c_raycastMaxDistance = float.MaxValue;
}

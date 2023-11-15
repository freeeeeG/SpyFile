using System;
using UnityEngine;

// Token: 0x020009FC RID: 2556
public class MapAvatarGroundCast : MonoBehaviour
{
	// Token: 0x060031F4 RID: 12788 RVA: 0x000EA49E File Offset: 0x000E889E
	public void RegisterGroundChangedCallback(VoidGeneric<Collider> _callback)
	{
		this.m_callback = (VoidGeneric<Collider>)Delegate.Combine(this.m_callback, _callback);
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x000EA4B7 File Offset: 0x000E88B7
	public void UnregisterGroundChangedCallback(VoidGeneric<Collider> _callback)
	{
		this.m_callback = (VoidGeneric<Collider>)Delegate.Remove(this.m_callback, _callback);
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x000EA4D0 File Offset: 0x000E88D0
	public Collider GetGroundCollider()
	{
		return this.m_groundCollider;
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x000EA4D8 File Offset: 0x000E88D8
	public bool HasGroundContact()
	{
		return this.m_isGrounded;
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x000EA4E0 File Offset: 0x000E88E0
	public Vector3 GetGroundNormal()
	{
		return this.m_groundPlane.normal;
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x000EA4ED File Offset: 0x000E88ED
	public Vector3 GetClosestPointOnGround()
	{
		return base.transform.position + this.m_distanceToGround * -this.m_groundPlane.normal;
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x000EA51C File Offset: 0x000E891C
	public Vector3 GetClosestPointOnGround(Vector3 position)
	{
		bool flag = this.GroundCastFromPoint(position + new Vector3(0f, 20f, 0f), ref this.m_raycastHits.initial);
		if (flag)
		{
			return this.m_raycastHits.initial.point;
		}
		return position + new Vector3(0f, 20f, 0f);
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x000EA586 File Offset: 0x000E8986
	public bool IsAboveGround()
	{
		return this.m_groundPlane.GetSide(base.transform.position);
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x000EA59E File Offset: 0x000E899E
	public void ForceUpdateNow()
	{
		this.FindGround();
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x000EA5A6 File Offset: 0x000E89A6
	private void Awake()
	{
		this.m_rigidBody = base.gameObject.RequireComponent<Rigidbody>();
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x000EA5BC File Offset: 0x000E89BC
	private void Start()
	{
		this.m_groundPlane.SetNormalAndPosition(Vector3.up, Vector3.zero);
		Vector3 position = (this.m_frontLeftPoint.position + this.m_frontRightPoint.position + this.m_rearPoint.position) / 3f;
		this.m_centerPointLocal = base.transform.InverseTransformPoint(position);
	}

	// Token: 0x060031FF RID: 12799 RVA: 0x000EA626 File Offset: 0x000E8A26
	private void Update()
	{
		if (!this.m_rigidBody.IsSleeping())
		{
			this.FindGround();
		}
	}

	// Token: 0x06003200 RID: 12800 RVA: 0x000EA640 File Offset: 0x000E8A40
	private void FindGround()
	{
		this.GroundCastFromPoint(this.m_frontLeftPoint.position, ref this.m_raycastHits.frontLeft);
		this.GroundCastFromPoint(this.m_frontRightPoint.position, ref this.m_raycastHits.frontRight);
		this.GroundCastFromPoint(this.m_rearPoint.position, ref this.m_raycastHits.rear);
		this.CalculateGroundPlane(ref this.m_groundPlane, this.m_raycastHits.frontLeft, this.m_raycastHits.frontRight, this.m_raycastHits.rear);
		this.m_distanceToGround = this.m_groundPlane.GetDistanceToPoint(base.transform.position);
		Collider groundCollider = this.m_groundCollider;
		bool isGrounded = this.m_isGrounded;
		this.m_isGrounded = (this.m_distanceToGround < 0.05f);
		if (this.m_isGrounded)
		{
			this.GroundCastFromPoint(base.transform.TransformPoint(this.m_centerPointLocal), ref this.m_raycastHits.initial);
			this.m_groundCollider = this.m_raycastHits.initial.collider;
		}
		else
		{
			this.m_groundCollider = null;
		}
		if (this.m_isGrounded != isGrounded || (this.m_isGrounded && this.m_groundCollider != groundCollider))
		{
			this.m_callback(this.m_groundCollider);
		}
		Vector3 normal = this.m_groundPlane.normal;
		Vector3 normalized = Vector3.Cross(normal, Vector3.forward).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, normal).normalized;
		Debug.DrawRay(base.transform.position + Vector3.up, normal, Color.red);
		Debug.DrawRay(base.transform.position + Vector3.up, normalized, Color.green);
		Debug.DrawRay(base.transform.position + Vector3.up, normalized2, Color.cyan);
	}

	// Token: 0x06003201 RID: 12801 RVA: 0x000EA82C File Offset: 0x000E8C2C
	private bool GroundCastFromPoint(Vector3 _point, ref RaycastHit _hit)
	{
		this.m_ray.origin = _point;
		this.m_ray.direction = Vector3.down;
		int num = Physics.RaycastNonAlloc(this.m_ray, this.m_rayHits, 99f, this.m_landscapeMask);
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = this.m_rayHits[i];
			if (raycastHit.distance > 0f)
			{
				float f = Mathf.Clamp(Vector3.Dot(raycastHit.normal, Vector3.up), -1f, 1f);
				float num3 = Mathf.Acos(f);
				if (num3 <= 45f)
				{
					if (num2 < 0 || (raycastHit.distance > 0f && raycastHit.distance < this.m_rayHits[num2].distance))
					{
						num2 = i;
					}
				}
			}
		}
		if (num2 != -1)
		{
			_hit = this.m_rayHits[num2];
			return true;
		}
		return false;
	}

	// Token: 0x06003202 RID: 12802 RVA: 0x000EA947 File Offset: 0x000E8D47
	private bool HitGround(RaycastHit _hit)
	{
		return _hit.collider != null && _hit.distance < 1f && Vector3.Angle(_hit.normal, Vector3.up) < 45f;
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x000EA988 File Offset: 0x000E8D88
	private bool CalculateGroundPlane(ref Plane _plane, RaycastHit _fl, RaycastHit _fr, RaycastHit _r)
	{
		int num = 0;
		if (this.HitGround(_fl) && this.HitGround(_fr))
		{
			int num2 = this.CompareHitsToGround(_fl, _fr);
			if (num2 > 0)
			{
				MapAvatarGroundCast.m_sortedHits[num++] = _fr;
			}
			else
			{
				MapAvatarGroundCast.m_sortedHits[num++] = _fl;
			}
		}
		else if (this.HitGround(_fl))
		{
			MapAvatarGroundCast.m_sortedHits[num++] = _fl;
		}
		else if (this.HitGround(_fr))
		{
			MapAvatarGroundCast.m_sortedHits[num++] = _fr;
		}
		if (this.HitGround(_r))
		{
			MapAvatarGroundCast.m_sortedHits[num++] = _r;
		}
		if (num >= 2)
		{
			Vector3 vector = MapAvatarGroundCast.m_sortedHits[1].point - MapAvatarGroundCast.m_sortedHits[0].point;
			Vector3 inPoint = MapAvatarGroundCast.m_sortedHits[0].point + vector * 0.5f;
			Vector3 normalized = (MapAvatarGroundCast.m_sortedHits[0].normal + MapAvatarGroundCast.m_sortedHits[1].normal).normalized;
			Vector3 normalized2 = Vector3.Cross(normalized, vector.normalized).normalized;
			Vector3 normalized3 = Vector3.Cross(vector, normalized2).normalized;
			if (Vector3.Angle(normalized3, _plane.normal) < 45f)
			{
				_plane.SetNormalAndPosition(normalized3, inPoint);
			}
		}
		if (num == 1)
		{
			_plane.SetNormalAndPosition(MapAvatarGroundCast.m_sortedHits[0].normal, MapAvatarGroundCast.m_sortedHits[0].point);
		}
		if (num == 0)
		{
			_plane.SetNormalAndPosition(Vector3.up, -(Vector3.up * float.MaxValue));
		}
		if (num > 0)
		{
			Debug.DrawRay(MapAvatarGroundCast.m_sortedHits[0].point, MapAvatarGroundCast.m_sortedHits[0].normal, Color.green);
		}
		if (num > 1)
		{
			Debug.DrawRay(MapAvatarGroundCast.m_sortedHits[1].point, MapAvatarGroundCast.m_sortedHits[1].normal, Color.green);
		}
		if (num > 2)
		{
			Debug.DrawRay(MapAvatarGroundCast.m_sortedHits[2].point, MapAvatarGroundCast.m_sortedHits[2].normal, Color.blue);
		}
		if (num >= 2)
		{
			Debug.DrawLine(MapAvatarGroundCast.m_sortedHits[0].point + Vector3.up, MapAvatarGroundCast.m_sortedHits[1].point + Vector3.up, Color.yellow);
		}
		return num > 0;
	}

	// Token: 0x06003204 RID: 12804 RVA: 0x000EAC58 File Offset: 0x000E9058
	private int CompareHitsToGround(RaycastHit _x, RaycastHit _y)
	{
		float y = _x.point.y;
		float y2 = _y.point.y;
		return y2.CompareTo(y);
	}

	// Token: 0x0400281F RID: 10271
	private const float c_raycastDistance = 99f;

	// Token: 0x04002820 RID: 10272
	private const float c_maxRaycastAngle = 45f;

	// Token: 0x04002821 RID: 10273
	private const float c_maxGroundAngle = 45f;

	// Token: 0x04002822 RID: 10274
	private const float c_maxGroundDistance = 1f;

	// Token: 0x04002823 RID: 10275
	private const float c_distanceEpsilon = 0.05f;

	// Token: 0x04002824 RID: 10276
	private const float c_raycastYOffset = 20f;

	// Token: 0x04002825 RID: 10277
	private const int c_maxRaycastHits = 6;

	// Token: 0x04002826 RID: 10278
	[SerializeField]
	private LayerMask m_landscapeMask = default(LayerMask);

	// Token: 0x04002827 RID: 10279
	[SerializeField]
	private Transform m_frontLeftPoint;

	// Token: 0x04002828 RID: 10280
	[SerializeField]
	private Transform m_frontRightPoint;

	// Token: 0x04002829 RID: 10281
	[SerializeField]
	private Transform m_rearPoint;

	// Token: 0x0400282A RID: 10282
	private Vector3 m_centerPointLocal = Vector3.zero;

	// Token: 0x0400282B RID: 10283
	private Ray m_ray = default(Ray);

	// Token: 0x0400282C RID: 10284
	private RaycastHit[] m_rayHits = new RaycastHit[6];

	// Token: 0x0400282D RID: 10285
	private MapAvatarGroundCast.MapAvatarGroundHits m_raycastHits = default(MapAvatarGroundCast.MapAvatarGroundHits);

	// Token: 0x0400282E RID: 10286
	private Rigidbody m_rigidBody;

	// Token: 0x0400282F RID: 10287
	private Collider m_groundCollider;

	// Token: 0x04002830 RID: 10288
	private Plane m_groundPlane;

	// Token: 0x04002831 RID: 10289
	private float m_distanceToGround;

	// Token: 0x04002832 RID: 10290
	private bool m_isGrounded;

	// Token: 0x04002833 RID: 10291
	private VoidGeneric<Collider> m_callback = delegate(Collider param1)
	{
	};

	// Token: 0x04002834 RID: 10292
	private static RaycastHit[] m_sortedHits = new RaycastHit[2];

	// Token: 0x020009FD RID: 2557
	public struct MapAvatarGroundHits
	{
		// Token: 0x04002836 RID: 10294
		public RaycastHit frontLeft;

		// Token: 0x04002837 RID: 10295
		public RaycastHit frontRight;

		// Token: 0x04002838 RID: 10296
		public RaycastHit rear;

		// Token: 0x04002839 RID: 10297
		public RaycastHit initial;
	}
}

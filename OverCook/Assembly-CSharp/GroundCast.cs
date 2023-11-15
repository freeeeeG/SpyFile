using System;
using UnityEngine;

// Token: 0x020009E3 RID: 2531
public class GroundCast : MonoBehaviour
{
	// Token: 0x06003175 RID: 12661 RVA: 0x000E7FC2 File Offset: 0x000E63C2
	public Collider GetGroundCollider()
	{
		return this.m_groundCollider;
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x000E7FCA File Offset: 0x000E63CA
	public LayerMask GetGroundLayer()
	{
		return this.m_groundLayer;
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x000E7FD2 File Offset: 0x000E63D2
	public Vector3 GetGroundPoint()
	{
		return this.m_groundPoint;
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x000E7FDA File Offset: 0x000E63DA
	public Vector3 GetGroundNormal()
	{
		return this.m_groundNormal;
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x000E7FE2 File Offset: 0x000E63E2
	public float GetGroundDistance()
	{
		return this.m_groundDistance;
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x000E7FEA File Offset: 0x000E63EA
	public bool HasGroundContact()
	{
		return this.m_isCurrent;
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x000E7FF2 File Offset: 0x000E63F2
	public void RegisterGroundChangedCallback(VoidGeneric<Collider> _callback)
	{
		this.m_callback = (VoidGeneric<Collider>)Delegate.Combine(this.m_callback, _callback);
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x000E800B File Offset: 0x000E640B
	public void UnregisterGroundChangedCallback(VoidGeneric<Collider> _callback)
	{
		this.m_callback = (VoidGeneric<Collider>)Delegate.Remove(this.m_callback, _callback);
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x000E8024 File Offset: 0x000E6424
	public void Setup(Collider _collider, float _radius, Vector3 _rayOffset, LayerMask _mask)
	{
		this.m_collider = _collider;
		this.m_radius = _radius;
		this.m_offset = _rayOffset;
		this.m_landscapeMask = _mask;
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x000E8043 File Offset: 0x000E6443
	public void ForceUpdateNow()
	{
		this.FindGround();
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x000E804B File Offset: 0x000E644B
	private void Awake()
	{
		this.m_rigidBody = base.gameObject.RequireComponent<Rigidbody>();
		this.m_collider = base.gameObject.RequestComponent<Collider>();
		this.m_Transform = base.transform;
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x000E807B File Offset: 0x000E647B
	private void Update()
	{
		if (!this.m_disableRigidbodySleep)
		{
			if (!this.m_rigidBody.IsSleeping())
			{
				this.FindGround();
			}
		}
		else
		{
			this.FindGround();
		}
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x000E80AC File Offset: 0x000E64AC
	private void FindGround()
	{
		Vector3 a = this.m_Transform.TransformPoint(this.m_offset);
		this.m_ray.direction = -Vector3.up;
		this.m_ray.origin = a + Vector3.up;
		int num;
		if (this.m_radius > 0f)
		{
			num = Physics.SphereCastNonAlloc(this.m_ray, this.m_radius, this.m_rayHits, 2f, this.m_landscapeMask);
		}
		else
		{
			num = Physics.RaycastNonAlloc(this.m_ray, this.m_rayHits, 2f, this.m_landscapeMask);
		}
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = this.m_rayHits[i];
			if (raycastHit.distance > 0f)
			{
				float f = Mathf.Clamp(Vector3.Dot(raycastHit.normal, Vector3.up), -1f, 1f);
				float num3 = Mathf.Acos(f) * 57.29578f;
				if (num3 <= 58f)
				{
					if (num2 < 0 || (raycastHit.distance > 0f && raycastHit.distance < this.m_rayHits[num2].distance))
					{
						num2 = i;
					}
				}
			}
		}
		if (num2 >= 0)
		{
			this.ProcessGroundHit(this.m_rayHits[num2]);
		}
		else
		{
			this.HitGround(null, Vector3.zero, Vector3.zero, 0f);
		}
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x000E8248 File Offset: 0x000E6648
	private void ProcessGroundHit(RaycastHit _hit)
	{
		Collider collider = null;
		Vector3 point = Vector3.zero;
		Vector3 normal = Vector3.zero;
		float distance = 0f;
		Vector3 closestPointOnSurface = InteractWithItemHelper.GetClosestPointOnSurface(this.m_collider, _hit.point);
		Vector3 lhs = _hit.point - closestPointOnSurface;
		float magnitude = lhs.magnitude;
		if (magnitude <= 0.3f)
		{
			collider = _hit.collider;
			point = _hit.point;
			normal = _hit.normal;
			float num = Vector3.Dot(lhs, -Vector3.up);
			if (num > 0f)
			{
				distance = magnitude;
			}
		}
		this.HitGround(collider, point, normal, distance);
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x000E82E8 File Offset: 0x000E66E8
	private void HitGround(Collider _collider, Vector3 _point, Vector3 _normal, float _distance)
	{
		bool isCurrent = this.m_isCurrent;
		Collider groundCollider = this.m_groundCollider;
		this.m_groundCollider = _collider;
		this.m_groundLayer = ((!(_collider != null)) ? 0 : (1 << _collider.gameObject.layer));
		this.m_groundPoint = _point;
		this.m_groundNormal = _normal;
		this.m_groundDistance = _distance;
		if (_collider != null)
		{
			this.m_isCurrent = true;
		}
		else
		{
			this.m_isCurrent = false;
		}
		if (this.m_isCurrent != isCurrent || (this.m_isCurrent && this.m_groundCollider != groundCollider))
		{
			this.m_callback(_collider);
		}
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x000E83A0 File Offset: 0x000E67A0
	public void ClearGround()
	{
		this.m_groundCollider = null;
		this.m_isCurrent = false;
	}

	// Token: 0x040027C1 RID: 10177
	[SerializeField]
	private LayerMask m_landscapeMask;

	// Token: 0x040027C2 RID: 10178
	[SerializeField]
	private float m_radius;

	// Token: 0x040027C3 RID: 10179
	[SerializeField]
	private bool m_disableRigidbodySleep;

	// Token: 0x040027C4 RID: 10180
	private Vector3 m_offset;

	// Token: 0x040027C5 RID: 10181
	private Rigidbody m_rigidBody;

	// Token: 0x040027C6 RID: 10182
	private Collider m_collider;

	// Token: 0x040027C7 RID: 10183
	private const float c_maxGroundAngle = 58f;

	// Token: 0x040027C8 RID: 10184
	private const float c_maxRaycastDistance = 2f;

	// Token: 0x040027C9 RID: 10185
	private const int c_maxRaycastHits = 6;

	// Token: 0x040027CA RID: 10186
	private const float c_minGroundDistance = 0.3f;

	// Token: 0x040027CB RID: 10187
	private Ray m_ray = default(Ray);

	// Token: 0x040027CC RID: 10188
	private RaycastHit[] m_rayHits = new RaycastHit[6];

	// Token: 0x040027CD RID: 10189
	private Collider m_groundCollider;

	// Token: 0x040027CE RID: 10190
	private LayerMask m_groundLayer = default(LayerMask);

	// Token: 0x040027CF RID: 10191
	private Vector3 m_groundPoint = Vector3.zero;

	// Token: 0x040027D0 RID: 10192
	private Vector3 m_groundNormal = Vector3.zero;

	// Token: 0x040027D1 RID: 10193
	private float m_groundDistance;

	// Token: 0x040027D2 RID: 10194
	private Transform m_Transform;

	// Token: 0x040027D3 RID: 10195
	private bool m_isCurrent;

	// Token: 0x040027D4 RID: 10196
	private VoidGeneric<Collider> m_callback = delegate(Collider param1)
	{
	};
}

using System;
using UnityEngine;

// Token: 0x0200046E RID: 1134
[RequireComponent(typeof(GroundCast))]
[RequireComponent(typeof(Rigidbody))]
public class SurfaceMovable : MonoBehaviour
{
	// Token: 0x0600150F RID: 5391 RVA: 0x000729C5 File Offset: 0x00070DC5
	public Vector3 GetVelocity()
	{
		return this.m_surfaceVelocity;
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x000729D0 File Offset: 0x00070DD0
	private void Awake()
	{
		this.m_rigidBody = base.gameObject.RequireComponent<Rigidbody>();
		this.m_groundCast = base.gameObject.RequireComponent<GroundCast>();
		this.m_collider = base.gameObject.GetComponentInChildren<Collider>();
		if (this.m_groundCast != null)
		{
			this.m_groundCast.RegisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
		}
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x00072A38 File Offset: 0x00070E38
	private void OnDisable()
	{
		this.m_surfaceVelocity = Vector3.zero;
		this.m_surface = null;
		this.m_prevSurface = null;
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x00072A53 File Offset: 0x00070E53
	private void Update()
	{
		if (this.m_surface != null)
		{
			this.m_surfaceVelocity = this.m_surface.CalculateVelocityAtPoint(this.m_groundCast.GetGroundPoint(), this.m_prevSurface);
		}
		else
		{
			this.m_surfaceVelocity = Vector3.zero;
		}
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x00072A94 File Offset: 0x00070E94
	private void OnGroundChanged(Collider groundCollider)
	{
		IMovingSurface movingSurface = (!(groundCollider != null)) ? null : groundCollider.gameObject.RequestInterface<IMovingSurface>();
		if (movingSurface != null)
		{
			if (movingSurface != this.m_surface)
			{
				this.m_prevSurface = this.m_surface;
				this.m_surface = movingSurface;
			}
		}
		else
		{
			this.m_surface = null;
			this.m_prevSurface = null;
		}
	}

	// Token: 0x0400102D RID: 4141
	private Rigidbody m_rigidBody;

	// Token: 0x0400102E RID: 4142
	private GroundCast m_groundCast;

	// Token: 0x0400102F RID: 4143
	private Collider m_collider;

	// Token: 0x04001030 RID: 4144
	private IMovingSurface m_prevSurface;

	// Token: 0x04001031 RID: 4145
	private IMovingSurface m_surface;

	// Token: 0x04001032 RID: 4146
	private Vector3 m_surfaceVelocity = Vector3.zero;
}

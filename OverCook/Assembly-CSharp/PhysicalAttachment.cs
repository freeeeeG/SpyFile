using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200080F RID: 2063
[AddComponentMenu("Scripts/Game/Ingredients/PhysicalAttachment")]
[RequireComponent(typeof(Collider))]
public class PhysicalAttachment : MonoBehaviour
{
	// Token: 0x0600276D RID: 10093 RVA: 0x000B96E1 File Offset: 0x000B7AE1
	public void InactiveSetup()
	{
		if (this.m_container == null)
		{
			this.CreateRigidBodyContainer();
		}
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x000B96FC File Offset: 0x000B7AFC
	public void Awake()
	{
		if (PhysicalAttachment.ms_GroundCastLayers == -1)
		{
			PhysicalAttachment.ms_GroundCastLayers = LayerMask.GetMask(new string[]
			{
				"Ground",
				"SlopedGround",
				"Worktops"
			});
		}
		if (this.m_container == null)
		{
			this.CreateRigidBodyContainer();
		}
	}

	// Token: 0x0600276F RID: 10095 RVA: 0x000B9753 File Offset: 0x000B7B53
	private void OnDestroy()
	{
		if (this.m_container != null)
		{
			this.DestroyRigidBodyContainer();
		}
	}

	// Token: 0x06002770 RID: 10096 RVA: 0x000B976C File Offset: 0x000B7B6C
	public bool GetFakeMeshActive()
	{
		return this.m_fakeMeshActive;
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x000B9774 File Offset: 0x000B7B74
	private void CreateRigidBodyContainer()
	{
		GameObject gameObject = new GameObject(base.gameObject.name + "_Rigidbody");
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		this.m_container = gameObject.AddComponent<Rigidbody>();
		this.m_container.position = base.transform.position;
		this.m_container.rotation = base.transform.rotation;
		this.m_motion = gameObject.AddComponent<RigidbodyMotion>();
		gameObject.AddComponent<ObjectContainer>();
		this.m_container.mass = this.m_rigidBodyParams.m_mass;
		this.m_container.drag = this.m_rigidBodyParams.m_drag;
		this.m_container.angularDrag = this.m_rigidBodyParams.m_angularDrag;
		this.m_container.useGravity = this.m_rigidBodyParams.m_useGravity;
		this.m_container.interpolation = this.m_rigidBodyParams.m_interpolation;
		this.m_container.collisionDetectionMode = this.m_rigidBodyParams.m_collisionDetectionMode;
		this.m_container.constraints = this.m_rigidBodyParams.m_constraints;
		gameObject.AddComponent<ForwardCollisionToChildren>();
		gameObject.AddComponent<DynamicLandscapeParenting>();
		PhysicsObjectSynchroniser physicsObjectSynchroniser = gameObject.AddComponent<PhysicsObjectSynchroniser>();
		physicsObjectSynchroniser.SetPhysicalAttachment(this);
		this.m_surfaceMovable = gameObject.AddComponent<SurfaceMovable>();
		this.m_motion.SetKinematic(true);
		this.m_groundCast = gameObject.GetComponent<GroundCast>();
		if (this.m_groundCast != null)
		{
			Collider collider = (!this.m_groundCastParams.m_coliderOverride) ? base.gameObject.GetComponent<Collider>() : this.m_groundCastParams.m_collider;
			Vector3 rayOffset = (!this.m_groundCastParams.m_rayOffsetOverride) ? base.gameObject.transform.InverseTransformPoint(collider.bounds.center) : this.m_groundCastParams.m_rayOffset;
			LayerMask mask = (!this.m_groundCastParams.m_maskOverride) ? PhysicalAttachment.ms_GroundCastLayers : this.m_groundCastParams.m_mask;
			float radius = 0f;
			if (this.m_groundCastParams.m_radiusOverride)
			{
				radius = this.m_groundCastParams.m_radius;
			}
			else if (collider != null && this.HasXZRotationConstraints(this.m_container))
			{
				Vector3 extents = collider.bounds.extents;
				radius = extents.XZ().magnitude;
			}
			this.m_groundCast.Setup(collider, radius, rayOffset, mask);
		}
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x000B9A10 File Offset: 0x000B7E10
	private void DestroyRigidBodyContainer()
	{
		if (this.m_meshLerper != null)
		{
			UnityEngine.Object.Destroy(this.m_meshLerper.gameObject);
		}
		UnityEngine.Object.Destroy(this.m_container.gameObject);
		this.m_surfaceMovable = null;
		this.m_motion = null;
		this.m_container = null;
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x000B9A63 File Offset: 0x000B7E63
	public void UseStaticPositioning()
	{
		if (this.m_meshLerper != null)
		{
			this.m_meshLerper.SetLerpActive(false);
		}
		this.m_fakeMeshActive = false;
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x000B9A89 File Offset: 0x000B7E89
	public void UseMeshLerp()
	{
		if (this.m_meshLerper != null)
		{
			this.m_meshLerper.SetLerpActive(true);
		}
		this.m_fakeMeshActive = true;
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x000B9AAF File Offset: 0x000B7EAF
	private bool HasXZRotationConstraints(Rigidbody rigidbody)
	{
		return (rigidbody.constraints & (RigidbodyConstraints)80) != RigidbodyConstraints.None;
	}

	// Token: 0x04001EF5 RID: 7925
	[SerializeField]
	public PhysicalAttachment.RigidbodyParams m_rigidBodyParams;

	// Token: 0x04001EF6 RID: 7926
	[SerializeField]
	public PhysicalAttachment.GroundCastParams m_groundCastParams = new PhysicalAttachment.GroundCastParams();

	// Token: 0x04001EF7 RID: 7927
	[NonSerialized]
	public Rigidbody m_container;

	// Token: 0x04001EF8 RID: 7928
	[NonSerialized]
	public RigidbodyMotion m_motion;

	// Token: 0x04001EF9 RID: 7929
	[NonSerialized]
	public SurfaceMovable m_surfaceMovable;

	// Token: 0x04001EFA RID: 7930
	[NonSerialized]
	public MeshLerper m_meshLerper;

	// Token: 0x04001EFB RID: 7931
	[NonSerialized]
	public GameObject m_originalMesh;

	// Token: 0x04001EFC RID: 7932
	[NonSerialized]
	public GroundCast m_groundCast;

	// Token: 0x04001EFD RID: 7933
	private bool m_fakeMeshActive;

	// Token: 0x04001EFE RID: 7934
	private static int ms_GroundCastLayers = -1;

	// Token: 0x02000810 RID: 2064
	[Serializable]
	public class RigidbodyParams
	{
		// Token: 0x04001EFF RID: 7935
		public float m_mass;

		// Token: 0x04001F00 RID: 7936
		public float m_drag;

		// Token: 0x04001F01 RID: 7937
		public float m_angularDrag;

		// Token: 0x04001F02 RID: 7938
		public bool m_useGravity;

		// Token: 0x04001F03 RID: 7939
		public RigidbodyInterpolation m_interpolation;

		// Token: 0x04001F04 RID: 7940
		public CollisionDetectionMode m_collisionDetectionMode;

		// Token: 0x04001F05 RID: 7941
		public RigidbodyConstraints m_constraints;
	}

	// Token: 0x02000811 RID: 2065
	[Serializable]
	public class GroundCastParams
	{
		// Token: 0x04001F06 RID: 7942
		public bool m_coliderOverride;

		// Token: 0x04001F07 RID: 7943
		public Collider m_collider;

		// Token: 0x04001F08 RID: 7944
		[Space]
		public bool m_radiusOverride;

		// Token: 0x04001F09 RID: 7945
		public float m_radius;

		// Token: 0x04001F0A RID: 7946
		[Space]
		public bool m_rayOffsetOverride;

		// Token: 0x04001F0B RID: 7947
		public Vector3 m_rayOffset;

		// Token: 0x04001F0C RID: 7948
		[Space]
		public bool m_maskOverride;

		// Token: 0x04001F0D RID: 7949
		public LayerMask m_mask;
	}
}

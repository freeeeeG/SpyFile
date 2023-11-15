using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class MeshLerper : MonoBehaviour
{
	// Token: 0x060005CD RID: 1485 RVA: 0x0002AE4C File Offset: 0x0002924C
	public void Initialise(ClientPhysicsObjectSynchroniser _physicsObjectSynchroniser, Rigidbody _rigidBody, Transform _rotationTransform, PhysicalAttachment _physicalAttachment)
	{
		this.m_ParentPhysicsObjectSynchroniser = _physicsObjectSynchroniser;
		this.m_PreviousParent = _physicsObjectSynchroniser.transform.parent;
		if (this.m_PreviousParent != null)
		{
			this.m_PreviousParentPosition = this.m_PreviousParent.position;
			this.m_PreviousParentRotation = this.m_PreviousParent.rotation;
		}
		this.m_TargetRigidBody = _rigidBody;
		this.m_TargetRotationTransform = _rotationTransform;
		this.m_bInitialised = true;
		this.m_SurfaceMoveable = _physicalAttachment.m_surfaceMovable;
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0002AEC8 File Offset: 0x000292C8
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Position = this.m_Transform.position;
		this.m_Rotation = Quaternion.identity;
		if (this.m_Transform.parent != null)
		{
			this.m_InitalOffset = this.m_Transform.position - this.m_Transform.parent.position;
		}
		if (MeshLerper.Tweekables == null)
		{
			MeshLerper.Tweekables = GameUtils.RequireManager<MultiplayerController>().m_NetworkPredictionTweekables;
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002AF58 File Offset: 0x00029358
	public void SetLerpActive(bool _active)
	{
		this.m_bActive = _active;
		if (_active)
		{
			this.m_Position = base.transform.position;
			this.m_Rotation = Quaternion.identity;
			this.m_TakeNextRotation = true;
		}
		base.transform.localPosition = default(Vector3);
		base.transform.localRotation = Quaternion.identity;
		this.m_SetStartPosition = base.transform.position;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0002AFCC File Offset: 0x000293CC
	public void SetTargets(MeshLerper.Target _from, MeshLerper.Target _to, float _length)
	{
		if (_from == MeshLerper.Target.CurrentPosition)
		{
			this.SetTargets(this.m_Position, _to, _length);
		}
		else
		{
			this.m_TargetFrom = _from;
			this.m_TargetTo = _to;
			this.m_LerpTime = 0f;
			if (_length == 0f)
			{
				_length = float.Epsilon;
			}
			this.m_LerpLength = _length;
		}
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0002B025 File Offset: 0x00029425
	public void SetTargets(Vector3 _from, MeshLerper.Target _to, float _length)
	{
		this.m_SetStartPosition = _from;
		this.SetTargets(MeshLerper.Target.SetPosition, _to, _length);
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0002B037 File Offset: 0x00029437
	public void SetNextTarget(MeshLerper.Target _next, float _length)
	{
		this.SetTargets(this.m_TargetTo, _next, _length);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0002B048 File Offset: 0x00029448
	public string GetTargetTo()
	{
		return this.m_TargetTo.ToString() + "[" + this.GetTargetPositionTo().ToString("0.00") + "]";
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002B088 File Offset: 0x00029488
	public string GetTargetFrom()
	{
		return this.m_TargetFrom.ToString() + "[" + this.GetTargetPositionFrom().ToString("0.00") + "]";
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002B0C8 File Offset: 0x000294C8
	public void SnapToTarget(MeshLerper.Target _snapTo)
	{
		if (this.m_bActive)
		{
			this.m_Position = this.GetTargetVector(_snapTo);
			this.m_Rotation = Quaternion.identity;
			this.m_Transform.position = this.m_Position;
			this.m_Transform.localRotation = Quaternion.identity;
			this.m_Rotation = Quaternion.identity;
		}
		this.m_TargetFrom = _snapTo;
		this.m_TargetTo = _snapTo;
		this.m_LerpLength = 0.001f;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002B140 File Offset: 0x00029540
	public virtual void Update()
	{
		if (this.m_bInitialised && this.m_bActive)
		{
			if (this.m_PreviousParent != this.m_ParentPhysicsObjectSynchroniser.transform.parent)
			{
				this.m_PreviousParent = this.m_ParentPhysicsObjectSynchroniser.transform.parent;
				if (this.m_PreviousParent != null)
				{
					this.m_PreviousParentPosition = this.m_PreviousParent.position;
					this.m_PreviousParentRotation = this.m_PreviousParent.rotation;
				}
				this.m_Rotation = Quaternion.identity;
			}
			else if (this.m_PreviousParent != null)
			{
				Vector3 position = this.m_PreviousParent.position;
				Quaternion rotation = this.m_PreviousParent.rotation;
				Vector3 b = position - this.m_PreviousParentPosition;
				Quaternion rotation2 = Quaternion.Inverse(this.m_PreviousParentRotation) * rotation;
				this.m_PreviousParentPosition = position;
				this.m_PreviousParentRotation = rotation;
				this.m_Position += b;
				this.m_Position = position + rotation2 * (this.m_Position - position);
			}
			float deltaTime = Time.deltaTime;
			float t = 1f;
			if (this.m_LerpLength > 0f)
			{
				t = this.m_LerpTime / this.m_LerpLength;
			}
			Vector3 vector = Vector3.Lerp(this.GetTargetPositionFrom(), this.GetTargetPositionTo(), t);
			Vector3 vector2 = this.m_Position;
			float b2 = (this.m_TargetTo != MeshLerper.Target.ServerPosition) ? this.m_ParentPhysicsObjectSynchroniser.ServerPreviousVelocity().magnitude : this.m_ParentPhysicsObjectSynchroniser.GetServerVelocity().magnitude;
			vector2 += this.m_SurfaceMoveable.GetVelocity() * deltaTime;
			float num = Mathf.Max(this.m_TargetRigidBody.velocity.magnitude, b2);
			float num2 = Mathf.Max(num * 0.95f, MeshLerper.Tweekables.LerpMinimumSpeed);
			vector += this.m_Transform.rotation * this.m_InitalOffset;
			if (this.m_TargetTo == MeshLerper.Target.Line || this.m_TargetTo == MeshLerper.Target.RigidBody || this.m_TargetTo == MeshLerper.Target.ServerPosition)
			{
				float magnitude = (vector - vector2).magnitude;
				float num3 = magnitude / num2;
				float num4 = Mathf.Clamp(num3 / MeshLerper.Tweekables.LerpTime, 1f, MeshLerper.Tweekables.LerpFactorMax);
				float num5 = Mathf.Clamp(magnitude / MeshLerper.Tweekables.LerpDistanceFactor * (magnitude / MeshLerper.Tweekables.LerpDistanceFactor), 1f, MeshLerper.Tweekables.LerpDistanceFactorMax);
				num2 *= num4 * num5;
			}
			this.m_Rotation = Quaternion.RotateTowards(this.m_Rotation, Quaternion.identity, this.m_rotationSpeed * deltaTime);
			this.m_Position = Vector3.MoveTowards(vector2, vector, num2 * deltaTime);
			this.m_Transform.localRotation = this.m_Rotation;
			this.m_Transform.position = this.m_Position;
			this.m_LerpTime += deltaTime;
		}
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0002B457 File Offset: 0x00029857
	private Vector3 GetTargetPositionFrom()
	{
		return this.GetTargetVector(this.m_TargetFrom);
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002B465 File Offset: 0x00029865
	private Vector3 GetTargetPositionTo()
	{
		return this.GetTargetVector(this.m_TargetTo);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002B474 File Offset: 0x00029874
	private Vector3 GetTargetVector(MeshLerper.Target _target)
	{
		switch (_target)
		{
		case MeshLerper.Target.Line:
			return this.m_ParentPhysicsObjectSynchroniser.GetLinePosition();
		case MeshLerper.Target.RigidBody:
			return this.m_TargetRigidBody.position;
		case MeshLerper.Target.ServerPosition:
			return this.m_ParentPhysicsObjectSynchroniser.GetExtrapolatedServerPosition();
		case MeshLerper.Target.SetPosition:
			return this.m_SetStartPosition;
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0002B4CC File Offset: 0x000298CC
	private Quaternion GetTargetQuaternion(MeshLerper.Target _target)
	{
		switch (_target)
		{
		case MeshLerper.Target.Line:
		case MeshLerper.Target.SetPosition:
		case MeshLerper.Target.CurrentPosition:
			return this.m_TargetRigidBody.rotation;
		case MeshLerper.Target.ServerPosition:
			return this.m_ParentPhysicsObjectSynchroniser.GetServerRotation();
		}
		return this.m_TargetRigidBody.rotation;
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002B51B File Offset: 0x0002991B
	public void SetPosition(Vector3 _position)
	{
		this.m_Position = _position;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002B524 File Offset: 0x00029924
	public void SetServerPosition(Vector3 _position, Quaternion _rotation)
	{
		float time = Time.time;
		if (time != this.m_previousPositionSetTime)
		{
			this.m_Period = time - this.m_previousPositionSetTime;
			this.m_Period *= 1.05f;
		}
		this.m_previousPositionSetTime = time;
		if (this.m_TakeNextRotation)
		{
			this.m_TakeNextRotation = false;
			this.m_PreviousSetRotation = _rotation;
			this.m_Rotation = Quaternion.identity;
		}
		Quaternion rotation = Quaternion.Inverse(this.m_PreviousSetRotation) * _rotation;
		this.m_Rotation *= Quaternion.Inverse(rotation);
		this.m_rotationSpeed = Quaternion.Angle(this.m_Rotation, Quaternion.identity) / this.m_Period;
		this.m_PreviousSetRotation = _rotation;
	}

	// Token: 0x040004D3 RID: 1235
	private Transform m_Transform;

	// Token: 0x040004D4 RID: 1236
	private Rigidbody m_TargetRigidBody;

	// Token: 0x040004D5 RID: 1237
	private Transform m_TargetRotationTransform;

	// Token: 0x040004D6 RID: 1238
	private MeshLerper.Target m_TargetFrom = MeshLerper.Target.ServerPosition;

	// Token: 0x040004D7 RID: 1239
	private MeshLerper.Target m_TargetTo = MeshLerper.Target.ServerPosition;

	// Token: 0x040004D8 RID: 1240
	private float m_LerpLength = 0.01f;

	// Token: 0x040004D9 RID: 1241
	private float m_LerpTime;

	// Token: 0x040004DA RID: 1242
	private bool m_bInitialised;

	// Token: 0x040004DB RID: 1243
	private Vector3 m_SetStartPosition = default(Vector3);

	// Token: 0x040004DC RID: 1244
	private ClientPhysicsObjectSynchroniser m_ParentPhysicsObjectSynchroniser;

	// Token: 0x040004DD RID: 1245
	private bool m_bActive;

	// Token: 0x040004DE RID: 1246
	private static NetworkPredictionTweekables Tweekables;

	// Token: 0x040004DF RID: 1247
	private SurfaceMovable m_SurfaceMoveable;

	// Token: 0x040004E0 RID: 1248
	private Vector3 m_InitalOffset = default(Vector3);

	// Token: 0x040004E1 RID: 1249
	private Vector3 m_Position = default(Vector3);

	// Token: 0x040004E2 RID: 1250
	private Quaternion m_Rotation = default(Quaternion);

	// Token: 0x040004E3 RID: 1251
	private Quaternion m_PreviousSetRotation = Quaternion.identity;

	// Token: 0x040004E4 RID: 1252
	private Transform m_PreviousParent;

	// Token: 0x040004E5 RID: 1253
	private Vector3 m_PreviousParentPosition = default(Vector3);

	// Token: 0x040004E6 RID: 1254
	private Quaternion m_PreviousParentRotation = default(Quaternion);

	// Token: 0x040004E7 RID: 1255
	private bool m_TakeNextRotation;

	// Token: 0x040004E8 RID: 1256
	private float m_Period;

	// Token: 0x040004E9 RID: 1257
	private float m_previousPositionSetTime;

	// Token: 0x040004EA RID: 1258
	private float m_rotationSpeed = 1f;

	// Token: 0x02000148 RID: 328
	public enum Target
	{
		// Token: 0x040004EC RID: 1260
		Line,
		// Token: 0x040004ED RID: 1261
		RigidBody,
		// Token: 0x040004EE RID: 1262
		ServerPosition,
		// Token: 0x040004EF RID: 1263
		SetPosition,
		// Token: 0x040004F0 RID: 1264
		CurrentPosition
	}
}

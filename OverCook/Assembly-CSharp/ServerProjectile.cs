using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class ServerProjectile : ServerSynchroniserBase
{
	// Token: 0x0600160A RID: 5642 RVA: 0x000756F4 File Offset: 0x00073AF4
	public void RegisterReachedTargetCallback(VoidGeneric<ServerProjectile> _callback)
	{
		this.m_ReachedTargetCallback = (VoidGeneric<ServerProjectile>)Delegate.Combine(this.m_ReachedTargetCallback, _callback);
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x0007570D File Offset: 0x00073B0D
	public void UnregisterReachedTargetCallback(VoidGeneric<ServerProjectile> _callback)
	{
		this.m_ReachedTargetCallback = (VoidGeneric<ServerProjectile>)Delegate.Remove(this.m_ReachedTargetCallback, _callback);
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x00075726 File Offset: 0x00073B26
	public void RegisterCollidedCallback(VoidGeneric<ServerProjectile, Collision> _callback)
	{
		this.m_CollidedCallback = (VoidGeneric<ServerProjectile, Collision>)Delegate.Combine(this.m_CollidedCallback, _callback);
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x0007573F File Offset: 0x00073B3F
	public void UnregisterCollidedCallback(VoidGeneric<ServerProjectile, Collision> _callback)
	{
		this.m_CollidedCallback = (VoidGeneric<ServerProjectile, Collision>)Delegate.Remove(this.m_CollidedCallback, _callback);
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x00075758 File Offset: 0x00073B58
	public void SetTargetAndTimeToTarget(Vector3 targetPosition, float timeToTarget)
	{
		this.m_TargetPosition = targetPosition;
		this.m_Duration = timeToTarget;
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x00075768 File Offset: 0x00073B68
	public void SetTargetAndTimeToTarget(Transform targetPosition, float timeToTarget)
	{
		this.m_OverrideTargetTransform = targetPosition;
		this.m_Duration = timeToTarget;
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x00075778 File Offset: 0x00073B78
	public void SetGravity(Vector3 gravity)
	{
		this.m_Gravity = gravity;
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x00075784 File Offset: 0x00073B84
	public void Start()
	{
		this.m_Rigidbody = base.gameObject.RequestComponentUpwardsRecursive<Rigidbody>();
		this.m_Rigidbody.isKinematic = true;
		this.m_InitialPosition = this.m_Rigidbody.position;
		Vector3 targetPosition = this.GetTargetPosition();
		this.m_InitialVelocity.x = (targetPosition.x - base.transform.position.x - 0.5f * this.m_Gravity.x * this.m_Duration * this.m_Duration) / this.m_Duration;
		this.m_InitialVelocity.y = (targetPosition.y - base.transform.position.y - 0.5f * this.m_Gravity.y * this.m_Duration * this.m_Duration) / this.m_Duration;
		this.m_InitialVelocity.z = (targetPosition.z - base.transform.position.z - 0.5f * this.m_Gravity.z * this.m_Duration * this.m_Duration) / this.m_Duration;
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x000758B0 File Offset: 0x00073CB0
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		this.m_Timer += TimeManager.GetDeltaTime(base.gameObject);
		Vector3 targetPosition;
		targetPosition.x = this.m_InitialPosition.x + this.m_InitialVelocity.x * this.m_Timer + 0.5f * this.m_Gravity.x * this.m_Timer * this.m_Timer;
		targetPosition.y = this.m_InitialPosition.y + this.m_InitialVelocity.y * this.m_Timer + 0.5f * this.m_Gravity.y * this.m_Timer * this.m_Timer;
		targetPosition.z = this.m_InitialPosition.z + this.m_InitialVelocity.z * this.m_Timer + 0.5f * this.m_Gravity.z * this.m_Timer * this.m_Timer;
		bool flag = false;
		if (this.m_Timer >= this.m_Duration)
		{
			targetPosition = this.GetTargetPosition();
			flag = true;
		}
		this.m_Rigidbody.position = targetPosition;
		if (flag)
		{
			this.m_ReachedTargetCallback(this);
		}
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x000759E4 File Offset: 0x00073DE4
	public void OnCollisionEnter(Collision collision)
	{
		this.m_CollidedCallback(this, collision);
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x000759F3 File Offset: 0x00073DF3
	private Vector3 GetTargetPosition()
	{
		return (!(this.m_OverrideTargetTransform != null)) ? this.m_TargetPosition : this.m_OverrideTargetTransform.position;
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x00075A1C File Offset: 0x00073E1C
	private bool IsUsingOverrideTransform()
	{
		return this.m_OverrideTargetTransform != null;
	}

	// Token: 0x040010A4 RID: 4260
	private float m_Duration = 2f;

	// Token: 0x040010A5 RID: 4261
	private Vector3 m_TargetPosition;

	// Token: 0x040010A6 RID: 4262
	private Vector3 m_Gravity;

	// Token: 0x040010A7 RID: 4263
	private Vector3 m_InitialPosition;

	// Token: 0x040010A8 RID: 4264
	private Vector3 m_InitialVelocity;

	// Token: 0x040010A9 RID: 4265
	private float m_Timer;

	// Token: 0x040010AA RID: 4266
	private Rigidbody m_Rigidbody;

	// Token: 0x040010AB RID: 4267
	private Transform m_OverrideTargetTransform;

	// Token: 0x040010AC RID: 4268
	private VoidGeneric<ServerProjectile> m_ReachedTargetCallback = delegate(ServerProjectile _projectile)
	{
	};

	// Token: 0x040010AD RID: 4269
	private VoidGeneric<ServerProjectile, Collision> m_CollidedCallback = delegate(ServerProjectile _projectile, Collision _collision)
	{
	};
}

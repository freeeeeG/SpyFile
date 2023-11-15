using System;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
[RequireComponent(typeof(Rigidbody))]
public class PilotMovement : MonoBehaviour
{
	// Token: 0x06001B6A RID: 7018 RVA: 0x0007E2FF File Offset: 0x0007C6FF
	protected virtual void Start()
	{
		this.RigidbodyMotion.SetKinematic(true);
		this.m_previousPose = this.RigidbodyMotion.GetPosition();
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x0007E320 File Offset: 0x0007C720
	protected virtual void Update()
	{
		Vector3 position = this.RigidbodyMotion.GetPosition();
		this.m_previousPoseDifference.Set(position.x - this.m_previousPose.x, position.y - this.m_previousPose.y, position.z - this.m_previousPose.z);
		this.m_previousPose = position;
		if (this.m_previousPoseDifference.sqrMagnitude < PilotMovement.m_threshold)
		{
			this.m_belowThresholdCounter++;
		}
		else
		{
			this.m_belowThresholdCounter = 0;
		}
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x0007E3B4 File Offset: 0x0007C7B4
	public virtual Vector3 EstimateAverageVelocity()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject.layer);
		if (deltaTime > 0.001f)
		{
			Vector3 a = this.m_previousPoseDifference / deltaTime;
			float num = Mathf.Min(10f * deltaTime, 1f);
			this.m_velocityAverage = num * a + (1f - num) * this.m_velocityAverage;
		}
		return this.m_velocityAverage;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x0007E426 File Offset: 0x0007C826
	public virtual bool HasMoved()
	{
		return this.m_belowThresholdCounter < 5;
	}

	// Token: 0x0400157F RID: 5503
	[AssignComponent(Editorbility.NonEditable)]
	public RigidbodyMotion RigidbodyMotion;

	// Token: 0x04001580 RID: 5504
	public float MoveSpeed = 4f;

	// Token: 0x04001581 RID: 5505
	public float SnapHalfLife = 0.1f;

	// Token: 0x04001582 RID: 5506
	protected Vector3 m_previousPose;

	// Token: 0x04001583 RID: 5507
	protected Vector3 m_previousPoseDifference;

	// Token: 0x04001584 RID: 5508
	protected Vector3 m_velocityAverage;

	// Token: 0x04001585 RID: 5509
	protected static float m_threshold = 0.001f;

	// Token: 0x04001586 RID: 5510
	private int m_belowThresholdCounter;

	// Token: 0x04001587 RID: 5511
	private const int k_belowThresholdMax = 5;
}

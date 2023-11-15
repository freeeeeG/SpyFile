using System;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class PilotRotation : PilotMovement
{
	// Token: 0x060018E1 RID: 6369 RVA: 0x0007E45C File Offset: 0x0007C85C
	protected override void Start()
	{
		this.m_previousPose = this.m_transformToRotate.rotation.eulerAngles;
		float num = 0.1f;
		Vector3 right = this.m_transformToRotate.right;
		Vector3 right2 = Vector3.right;
		float num2 = Vector3.Dot(right, right2);
		this.m_bEstimateVelocityInX = (Mathf.Abs(num2) > num);
		this.m_directionModifier = ((num2 <= 0f) ? -1 : 1);
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x0007E4CC File Offset: 0x0007C8CC
	protected override void Update()
	{
		Vector3 eulerAngles = this.m_transformToRotate.rotation.eulerAngles;
		this.m_previousPoseDifference.Set(Mathf.DeltaAngle(this.m_previousPose.x, eulerAngles.x), Mathf.DeltaAngle(this.m_previousPose.y, eulerAngles.y), Mathf.DeltaAngle(this.m_previousPose.z, eulerAngles.z));
		if (this.m_previousPoseDifference.sqrMagnitude > PilotMovement.m_threshold)
		{
			this.m_previousPose = this.m_transformToRotate.rotation.eulerAngles;
		}
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x0007E56C File Offset: 0x0007C96C
	public override Vector3 EstimateAverageVelocity()
	{
		Vector3 result = base.EstimateAverageVelocity();
		if (this.m_bEstimateVelocityInX)
		{
			result.Set(result.y * (float)this.m_directionModifier, 0f, 0f);
		}
		else
		{
			result.Set(0f, 0f, result.y * (float)this.m_directionModifier);
		}
		return result;
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x0007E5D1 File Offset: 0x0007C9D1
	public override bool HasMoved()
	{
		return this.m_previousPoseDifference.sqrMagnitude > PilotMovement.m_threshold;
	}

	// Token: 0x040013FC RID: 5116
	public Transform m_transformToRotate;

	// Token: 0x040013FD RID: 5117
	[Range(-45f, 45f)]
	public float m_minLimitDegrees = -45f;

	// Token: 0x040013FE RID: 5118
	[Range(-45f, 45f)]
	public float m_maxLimitDegrees = 45f;

	// Token: 0x040013FF RID: 5119
	public string m_sessionBegunTrigger;

	// Token: 0x04001400 RID: 5120
	public string m_sessionEndedTrigger;

	// Token: 0x04001401 RID: 5121
	private bool m_bEstimateVelocityInX;

	// Token: 0x04001402 RID: 5122
	private int m_directionModifier;
}

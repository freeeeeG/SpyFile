using System;
using UnityEngine;

// Token: 0x02000444 RID: 1092
public class FollowCamera : MonoBehaviour
{
	// Token: 0x06001423 RID: 5155 RVA: 0x0006E010 File Offset: 0x0006C410
	public Vector3 GetIdealLocation()
	{
		Vector3 vector = this.Target.transform.position;
		if (this.TargetBounds.HasValue)
		{
			Bounds value = this.TargetBounds.Value;
			vector = vector.Clamp(value.min, value.max);
		}
		return vector + this.IdealOffset;
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x0006E06C File Offset: 0x0006C46C
	private void Update()
	{
		if (this.m_previousTarget != this.Target)
		{
			this.m_previousTarget = this.Target;
			this.m_TargetRigidBody = this.Target.RequestComponent<Rigidbody>();
		}
		Vector3 idealLocation = this.GetIdealLocation();
		float num = this.GradientLimit;
		if (this.m_TargetRigidBody != null)
		{
			num = Mathf.Max(num, this.m_TargetRigidBody.velocity.magnitude);
		}
		float magnitude = (idealLocation - base.transform.position).magnitude;
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		MathUtils.AdvanceToTarget_Sinusoidal(ref magnitude, ref this.m_currentGradient, 0f, num, this.TimeToMax, deltaTime);
		base.transform.position = idealLocation - (idealLocation - base.transform.position).SafeNormalised(Vector3.zero) * magnitude;
	}

	// Token: 0x04000F87 RID: 3975
	public GameObject Target;

	// Token: 0x04000F88 RID: 3976
	public Vector3 IdealOffset;

	// Token: 0x04000F89 RID: 3977
	public float GradientLimit = 0.5f;

	// Token: 0x04000F8A RID: 3978
	public float TimeToMax = 0.5f;

	// Token: 0x04000F8B RID: 3979
	public OptionalBounds TargetBounds = new OptionalBounds();

	// Token: 0x04000F8C RID: 3980
	private float m_currentGradient;

	// Token: 0x04000F8D RID: 3981
	private GameObject m_previousTarget;

	// Token: 0x04000F8E RID: 3982
	private Rigidbody m_TargetRigidBody;
}

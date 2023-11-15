using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000533 RID: 1331
public class ServerPilotRotation : ServerPilotMovement
{
	// Token: 0x060018E9 RID: 6377 RVA: 0x0007EC50 File Offset: 0x0007D050
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_pilotRotation = (PilotRotation)synchronisedObject;
		this.m_startRightDirection = this.m_pilotRotation.m_transformToRotate.right;
		this.m_startAngle = this.m_pilotRotation.m_transformToRotate.eulerAngles.y;
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x0007EC9D File Offset: 0x0007D09D
	public override void UpdateSynchronising()
	{
		if (this.m_controlScheme != null)
		{
			this.UpdateRotation();
		}
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x0007ECB0 File Offset: 0x0007D0B0
	public override EntityType GetEntityType()
	{
		return EntityType.PilotRotation;
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x0007ECB4 File Offset: 0x0007D0B4
	private void UpdateRotation()
	{
		float value = this.m_controlScheme.m_moveX.GetValue();
		float num = -this.m_controlScheme.m_moveY.GetValue();
		float num2 = 0.1f;
		if (value != 0f || num != 0f)
		{
			Vector3 rhs = new Vector3(value, 0f, num);
			float num3 = Vector3.Dot(this.m_startRightDirection, rhs);
			if (Mathf.Abs(num3) > num2)
			{
				this.m_angle += this.m_pilotRotation.MoveSpeed * (float)((num3 >= 0f) ? 1 : -1) * TimeManager.GetDeltaTime(base.gameObject);
				this.m_angle = Mathf.Clamp(this.m_angle, this.m_pilotRotation.m_minLimitDegrees, this.m_pilotRotation.m_maxLimitDegrees);
				this.m_message.m_angle = this.m_startAngle + this.m_angle;
				this.SendServerEvent(this.m_message);
			}
		}
	}

	// Token: 0x04001404 RID: 5124
	private PilotRotationMessage m_message = new PilotRotationMessage();

	// Token: 0x04001405 RID: 5125
	private PilotRotation m_pilotRotation;

	// Token: 0x04001406 RID: 5126
	private Vector3 m_startRightDirection;

	// Token: 0x04001407 RID: 5127
	private float m_startAngle;

	// Token: 0x04001408 RID: 5128
	private float m_angle;
}

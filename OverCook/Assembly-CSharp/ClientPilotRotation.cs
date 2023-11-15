using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000534 RID: 1332
public class ClientPilotRotation : ClientPilotMovement
{
	// Token: 0x060018EE RID: 6382 RVA: 0x0007F147 File Offset: 0x0007D547
	public override EntityType GetEntityType()
	{
		return EntityType.PilotRotation;
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x0007F14B File Offset: 0x0007D54B
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_pilotRotation = (PilotRotation)synchronisedObject;
		this.m_nextRotation = this.m_pilotRotation.m_transformToRotate.rotation;
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x0007F170 File Offset: 0x0007D570
	public override void UpdateSynchronising()
	{
		this.m_pilotRotation.m_transformToRotate.rotation = Quaternion.RotateTowards(this.m_pilotRotation.m_transformToRotate.rotation, this.m_nextRotation, this.m_pilotRotation.MoveSpeed * TimeManager.GetDeltaTime(base.gameObject));
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0007F1C0 File Offset: 0x0007D5C0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.m_message = (PilotRotationMessage)serialisable;
		Vector3 eulerAngles = this.m_pilotRotation.m_transformToRotate.eulerAngles;
		eulerAngles.y = this.m_message.m_angle;
		this.m_nextRotation = Quaternion.Euler(eulerAngles);
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x0007F208 File Offset: 0x0007D608
	public override void AssignAvatar(GameObject _avatar)
	{
		base.AssignAvatar(_avatar);
		if (_avatar != null && !string.IsNullOrEmpty(this.m_pilotRotation.m_sessionBegunTrigger))
		{
			base.SendMessage("OnTrigger", this.m_pilotRotation.m_sessionBegunTrigger, SendMessageOptions.DontRequireReceiver);
		}
		else if (_avatar == null && !string.IsNullOrEmpty(this.m_pilotRotation.m_sessionEndedTrigger))
		{
			base.SendMessage("OnTrigger", this.m_pilotRotation.m_sessionEndedTrigger, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x04001409 RID: 5129
	private PilotRotationMessage m_message;

	// Token: 0x0400140A RID: 5130
	private PilotRotation m_pilotRotation;

	// Token: 0x0400140B RID: 5131
	private Quaternion m_nextRotation;
}

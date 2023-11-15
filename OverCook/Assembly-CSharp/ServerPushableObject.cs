using System;
using UnityEngine;

// Token: 0x02000559 RID: 1369
public class ServerPushableObject : ServerSessionInteractable
{
	// Token: 0x060019D2 RID: 6610 RVA: 0x000818C3 File Offset: 0x0007FCC3
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_PushableObject = (PushableObject)synchronisedObject;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x000818D8 File Offset: 0x0007FCD8
	protected override ServerSessionInteractable.SessionBase BuildSession(GameObject _interacter)
	{
		return new ServerPushableObject.UserSession(this, _interacter, this.m_PushableObject);
	}

	// Token: 0x04001488 RID: 5256
	private PushableObject m_PushableObject;

	// Token: 0x0200055A RID: 1370
	private class UserSession : ServerSessionInteractable.SessionBase
	{
		// Token: 0x060019D4 RID: 6612 RVA: 0x000818E8 File Offset: 0x0007FCE8
		public UserSession(ServerPushableObject _self, GameObject _avatar, PushableObject _pushableObject) : base(_self, _avatar)
		{
			this.m_PilotMovement = _self.gameObject.RequireComponent<ServerPilotMovement>();
			this.m_PilotMovement.AssignPlayer(base.PlayerControls.ControlScheme);
			DynamicLandscapeParenting dynamicLandscapeParenting = base.PlayerControls.gameObject.RequestComponent<DynamicLandscapeParenting>();
			if (dynamicLandscapeParenting != null)
			{
				dynamicLandscapeParenting.enabled = false;
			}
			Transform transform = base.PlayerControls.gameObject.transform;
			Transform attachPoint = _pushableObject.GetAttachPoint(transform);
			transform.SetParent(attachPoint, true);
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0008196C File Offset: 0x0007FD6C
		public override void OnSessionEnded()
		{
			this.m_PilotMovement.AssignPlayer(null);
			base.PlayerControls.gameObject.transform.SetParent(null);
			DynamicLandscapeParenting dynamicLandscapeParenting = base.PlayerControls.gameObject.RequestComponent<DynamicLandscapeParenting>();
			if (dynamicLandscapeParenting != null)
			{
				dynamicLandscapeParenting.enabled = true;
			}
			GroundCast component = base.PlayerControls.GetComponent<GroundCast>();
			if (component != null)
			{
				component.ClearGround();
				component.ForceUpdateNow();
			}
			base.OnSessionEnded();
		}

		// Token: 0x04001489 RID: 5257
		private ServerPilotMovement m_PilotMovement;
	}
}

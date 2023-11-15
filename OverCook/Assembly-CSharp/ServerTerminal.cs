using System;
using UnityEngine;

// Token: 0x020005AE RID: 1454
public class ServerTerminal : ServerSessionInteractable
{
	// Token: 0x06001BAA RID: 7082 RVA: 0x00087CC5 File Offset: 0x000860C5
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_terminal = (Terminal)synchronisedObject;
	}

	// Token: 0x06001BAB RID: 7083 RVA: 0x00087CDA File Offset: 0x000860DA
	protected override ServerSessionInteractable.SessionBase BuildSession(GameObject _interacter)
	{
		return new ServerTerminal.UserSession(this, _interacter, this.m_terminal.m_pilotableObject);
	}

	// Token: 0x040015B4 RID: 5556
	private Terminal m_terminal;

	// Token: 0x020005AF RID: 1455
	private class UserSession : ServerSessionInteractable.SessionBase
	{
		// Token: 0x06001BAC RID: 7084 RVA: 0x00087CF0 File Offset: 0x000860F0
		public UserSession(ServerTerminal _self, GameObject _avatar, PilotMovement _pilotableObject) : base(_self, _avatar)
		{
			ServerPilotMovement serverPilotMovement = _pilotableObject.gameObject.RequestComponent<ServerPilotMovement>();
			serverPilotMovement.AssignPlayer(base.PlayerControls.ControlScheme);
			this.m_puppetObject = serverPilotMovement;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x00087D29 File Offset: 0x00086129
		public override void OnSessionEnded()
		{
			this.m_puppetObject.AssignPlayer(null);
			base.OnSessionEnded();
		}

		// Token: 0x040015B5 RID: 5557
		private ServerPilotMovement m_puppetObject;
	}
}

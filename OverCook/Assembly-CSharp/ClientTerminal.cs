using System;
using UnityEngine;

// Token: 0x020005B0 RID: 1456
public class ClientTerminal : ClientSessionInteractable
{
	// Token: 0x06001BAF RID: 7087 RVA: 0x00087D45 File Offset: 0x00086145
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_terminal = (Terminal)synchronisedObject;
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x00087D5A File Offset: 0x0008615A
	protected override ClientSessionInteractable.SessionBase BuildSession(GameObject _interacter)
	{
		return new ClientTerminal.UserSession(this, _interacter, this.m_terminal.m_pilotableObject);
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x00087D70 File Offset: 0x00086170
	public Vector2 CosmeticJoystickInput
	{
		get
		{
			if (base.Session != null)
			{
				ClientTerminal.UserSession userSession = base.Session as ClientTerminal.UserSession;
				return userSession.CosmeticJoystickInput;
			}
			return Vector2.zero;
		}
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x00087DA0 File Offset: 0x000861A0
	public Terminal GetTerminal()
	{
		return this.m_terminal;
	}

	// Token: 0x040015B6 RID: 5558
	private Terminal m_terminal;

	// Token: 0x020005B1 RID: 1457
	private class UserSession : ClientSessionInteractable.SessionBase
	{
		// Token: 0x06001BB3 RID: 7091 RVA: 0x00087DA8 File Offset: 0x000861A8
		public UserSession(ClientTerminal _self, GameObject _avatar, PilotMovement _pilotableObject) : base(_self, _avatar)
		{
			this.m_pilotObject = _pilotableObject;
			this.m_clientPilotObject = _pilotableObject.gameObject.RequireComponent<ClientPilotMovement>();
			this.m_clientPilotObject.AssignAvatar(_avatar);
			this.m_lastPosition = _pilotableObject.transform.position;
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x00087DE8 File Offset: 0x000861E8
		public Vector2 CosmeticJoystickInput
		{
			get
			{
				if (base.PlayerControls.ControlScheme != null)
				{
					return new Vector2(base.PlayerControls.ControlScheme.m_moveX.GetValue(), -base.PlayerControls.ControlScheme.m_moveY.GetValue());
				}
				this.m_velocityAverage = this.m_pilotObject.EstimateAverageVelocity();
				return this.ClampByMagnitude(this.m_velocityAverage.XZ() / this.m_pilotObject.MoveSpeed, 1f);
			}
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00087E70 File Offset: 0x00086270
		private Vector2 ClampByMagnitude(Vector2 _input, float _maxMagnitude)
		{
			float sqrMagnitude = _input.sqrMagnitude;
			if (sqrMagnitude > _maxMagnitude * _maxMagnitude)
			{
				return _input.normalized * _maxMagnitude;
			}
			return _input;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x00087E9D File Offset: 0x0008629D
		public override void OnSessionEnded()
		{
			this.m_clientPilotObject.AssignAvatar(null);
			base.OnSessionEnded();
		}

		// Token: 0x040015B7 RID: 5559
		private PilotMovement m_pilotObject;

		// Token: 0x040015B8 RID: 5560
		private ClientPilotMovement m_clientPilotObject;

		// Token: 0x040015B9 RID: 5561
		private Vector3 m_lastPosition;

		// Token: 0x040015BA RID: 5562
		private Vector3 m_velocityAverage;
	}
}

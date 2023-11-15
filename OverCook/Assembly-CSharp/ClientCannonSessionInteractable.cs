using System;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class ClientCannonSessionInteractable : ClientSessionInteractable
{
	// Token: 0x06001474 RID: 5236 RVA: 0x0006FDBB File Offset: 0x0006E1BB
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannon = synchronisedObject.gameObject.RequireComponent<ClientCannon>();
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x0006FDD5 File Offset: 0x0006E1D5
	protected override ClientSessionInteractable.SessionBase BuildSession(GameObject _interacter)
	{
		return new ClientCannonSessionInteractable.UserSession(this, _interacter, this.m_cannon);
	}

	// Token: 0x04000FCF RID: 4047
	private ClientCannon m_cannon;

	// Token: 0x02000455 RID: 1109
	private class UserSession : ClientSessionInteractable.SessionBase
	{
		// Token: 0x06001476 RID: 5238 RVA: 0x0006FDE4 File Offset: 0x0006E1E4
		public UserSession(ClientCannonSessionInteractable _self, GameObject _avatar, ClientCannon _cannon) : base(_self, _avatar)
		{
		}
	}
}

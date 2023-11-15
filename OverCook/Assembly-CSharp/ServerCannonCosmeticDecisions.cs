using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000392 RID: 914
public class ServerCannonCosmeticDecisions : ServerSynchroniserBase
{
	// Token: 0x06001142 RID: 4418 RVA: 0x0006306D File Offset: 0x0006146D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cannonCosmeticDecisions = (CannonCosmeticDecisions)synchronisedObject;
		this.m_cannon = base.gameObject.RequireComponent<ServerCannon>();
		this.m_cannon.SetReadyToLaunchCallback(new Generic<bool>(this.IsReadyToLaunch));
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x000630AC File Offset: 0x000614AC
	private bool IsReadyToLaunch()
	{
		return this.m_cannonCosmeticDecisions.m_cannonAnimator.GetCurrentAnimatorStateInfo(0).IsName("DLC08_Cannon_Ready");
	}

	// Token: 0x04000D67 RID: 3431
	private CannonCosmeticDecisions m_cannonCosmeticDecisions;

	// Token: 0x04000D68 RID: 3432
	private ServerCannon m_cannon;

	// Token: 0x04000D69 RID: 3433
	private const string m_readyStateName = "DLC08_Cannon_Ready";
}

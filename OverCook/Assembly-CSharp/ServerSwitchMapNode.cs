using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BC7 RID: 3015
public class ServerSwitchMapNode : ServerSynchroniserBase
{
	// Token: 0x06003DAD RID: 15789 RVA: 0x001263DC File Offset: 0x001247DC
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (SwitchMapNode)synchronisedObject;
		if (!this.m_baseObject.IsSwitchPressed() && this.m_baseObject.IsSwitchedDueToCompletion())
		{
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.Progress.RecordSwitchActivated(this.m_baseObject.SwitchID);
		}
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00126431 File Offset: 0x00124831
	public override EntityType GetEntityType()
	{
		return EntityType.SwitchMapNode;
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00126438 File Offset: 0x00124838
	public void OnSwitchPressed(MapAvatarControls _avatar, WorldMapSwitch _switch)
	{
		this.SendServerEvent(this.m_Message);
		GameSession session = GameUtils.GetGameSession();
		session.Progress.RecordSwitchActivated(this.m_baseObject.SwitchID);
		GameUtils.RequireManager<SaveManager>().RegisterOnIdle(delegate
		{
			session.SaveSession(null);
		});
	}

	// Token: 0x04003182 RID: 12674
	private SwitchMapNode m_baseObject;

	// Token: 0x04003183 RID: 12675
	private SwitchMapNodeMessage m_Message = new SwitchMapNodeMessage();
}

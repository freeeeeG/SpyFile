using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000BC8 RID: 3016
public class ClientSwitchMapNode : ClientSynchroniserBase
{
	// Token: 0x06003DB1 RID: 15793 RVA: 0x001264B1 File Offset: 0x001248B1
	private void Awake()
	{
		this.m_worldMapFlowController = GameUtils.RequireManager<WorldMapFlowController>();
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x001264BE File Offset: 0x001248BE
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (SwitchMapNode)synchronisedObject;
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x001264CC File Offset: 0x001248CC
	public override EntityType GetEntityType()
	{
		return EntityType.SwitchMapNode;
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x001264D0 File Offset: 0x001248D0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession.DLC == -1)
		{
			OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
			if (overcookedAchievementManager != null)
			{
				overcookedAchievementManager.AddIDStat(8, this.m_baseObject.SwitchID, ControlPadInput.PadNum.One);
			}
		}
		gameSession.Progress.RecordSwitchActivated(this.m_baseObject.SwitchID);
		gameSession.SaveSession(null);
		ClientWorldMapFlowController component = this.m_worldMapFlowController.GetComponent<ClientWorldMapFlowController>();
		component.UnfoldSwitchMapNode(this.m_baseObject);
	}

	// Token: 0x04003184 RID: 12676
	private SwitchMapNode m_baseObject;

	// Token: 0x04003185 RID: 12677
	private WorldMapFlowController m_worldMapFlowController;
}

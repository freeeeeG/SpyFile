using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007C3 RID: 1987
	public class WaveUIController : UIControllerBase
	{
		// Token: 0x0600260E RID: 9742 RVA: 0x000B4B90 File Offset: 0x000B2F90
		private void Awake()
		{
			HordeLevelConfig hordeLevelConfig = GameUtils.GetLevelConfig() as HordeLevelConfig;
			this.m_waveCount = hordeLevelConfig.m_waves.Count;
			string nonLocalizedText = Localization.Get(this.m_waveNumberUILocalisationTag, new LocToken[]
			{
				new LocToken("[Number]", "1"),
				new LocToken("[NumberMax]", this.m_waveCount.ToString())
			});
			this.m_waveText.SetNonLocalizedText(nonLocalizedText);
			Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000B4C31 File Offset: 0x000B3031
		private void OnDestroy()
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000B4C4C File Offset: 0x000B304C
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartEntities)
			{
				FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
				this.m_flowController = flowControllerBase.gameObject.RequireComponent<ClientHordeFlowController>();
				this.m_flowController.RegisterOnBeginWave(this, new GenericVoid<ClientHordeFlowController, int>(this.OnBeginWave));
			}
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000B4C9C File Offset: 0x000B309C
		private void OnBeginWave(ClientHordeFlowController flowController, int waveNumber)
		{
			string nonLocalizedText = Localization.Get(this.m_waveNumberUILocalisationTag, new LocToken[]
			{
				new LocToken("[Number]", waveNumber.ToString()),
				new LocToken("[NumberMax]", this.m_waveCount.ToString())
			});
			this.m_waveText.SetNonLocalizedText(nonLocalizedText);
		}

		// Token: 0x04001E0F RID: 7695
		[SerializeField]
		private string m_waveNumberUILocalisationTag = "Horde.Wave";

		// Token: 0x04001E10 RID: 7696
		[SerializeField]
		private T17Text m_waveText;

		// Token: 0x04001E11 RID: 7697
		private ClientHordeFlowController m_flowController;

		// Token: 0x04001E12 RID: 7698
		private int m_waveCount = 1;
	}
}

using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A29 RID: 2601
[ExecutionDependency(typeof(IFlowController))]
public class SetShaderParameterAfterTime : MonoBehaviour
{
	// Token: 0x06003382 RID: 13186 RVA: 0x000F276F File Offset: 0x000F0B6F
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x000F278C File Offset: 0x000F0B8C
	private void Start()
	{
		foreach (MaterialScroll materialScroll in this.m_materialScrolls)
		{
			materialScroll.SetMaterialScrollToZero();
		}
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x000F27BE File Offset: 0x000F0BBE
	private void OnDestroy()
	{
		if (this.flowController != null)
		{
			this.flowController.RoundActivatedCallback -= this.OnFlowStart;
		}
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x000F27FC File Offset: 0x000F0BFC
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.Initialise();
		}
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x000F2823 File Offset: 0x000F0C23
	private void Initialise()
	{
		this.flowController = GameUtils.GetFlowController();
		if (this.flowController != null)
		{
			this.flowController.RoundActivatedCallback += this.OnFlowStart;
		}
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x000F2854 File Offset: 0x000F0C54
	private void OnFlowStart()
	{
		foreach (MaterialScroll materialScroll in this.m_materialScrolls)
		{
			materialScroll.SetMaterialScrollToValue();
		}
	}

	// Token: 0x04002975 RID: 10613
	[SerializeField]
	private MaterialScroll[] m_materialScrolls;

	// Token: 0x04002976 RID: 10614
	private IFlowController flowController;
}

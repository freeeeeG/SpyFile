using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200077C RID: 1916
public abstract class ServerIconTutorialBase : ServerSynchroniserBase
{
	// Token: 0x06002500 RID: 9472 RVA: 0x000AEE04 File Offset: 0x000AD204
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_iconTutorial = (IconTutorialBase)synchronisedObject;
		FlowControllerBase flowControllerBase = GameUtils.RequireManager<FlowControllerBase>();
		this.m_iServerFlowController = flowControllerBase.gameObject.RequestInterface<IServerFlowController>();
		this.m_iServerFlowController.RoundActivatedCallback += this.EnterRound;
		this.m_iServerFlowController.RoundDeactivatedCallback += this.ExitRound;
	}

	// Token: 0x06002501 RID: 9473 RVA: 0x000AEE69 File Offset: 0x000AD269
	protected virtual void OnStartTutorial()
	{
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x000AEE6B File Offset: 0x000AD26B
	protected virtual void OnStopTutorial()
	{
	}

	// Token: 0x06002503 RID: 9475 RVA: 0x000AEE6D File Offset: 0x000AD26D
	private void EnterRound()
	{
		if (!this.m_completed)
		{
			this.m_tutorialActive = true;
			this.OnStartTutorial();
		}
	}

	// Token: 0x06002504 RID: 9476 RVA: 0x000AEE87 File Offset: 0x000AD287
	private void ExitRound()
	{
		if (this.m_tutorialActive)
		{
			this.m_tutorialActive = false;
			this.OnStopTutorial();
		}
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x000AEEA1 File Offset: 0x000AD2A1
	protected void CompleteTutorial()
	{
		this.ExitRound();
		this.m_completed = true;
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x000AEEB0 File Offset: 0x000AD2B0
	protected virtual void OnTutorialUpdate()
	{
	}

	// Token: 0x04001C92 RID: 7314
	private IconTutorialBase m_iconTutorial;

	// Token: 0x04001C93 RID: 7315
	protected IFlowController m_iServerFlowController;

	// Token: 0x04001C94 RID: 7316
	private bool m_tutorialActive;

	// Token: 0x04001C95 RID: 7317
	private bool m_completed;
}

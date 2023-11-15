using System;
using UnityEngine;

// Token: 0x02000785 RID: 1925
public class ServerTutorialIconController : ServerIconTutorialBase
{
	// Token: 0x0600251F RID: 9503 RVA: 0x000AF44D File Offset: 0x000AD84D
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_tutorialController = (TutorialIconController)synchronisedObject;
		this.m_serverFlowController = (this.m_iServerFlowController as ServerCampaignFlowController);
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x000AF473 File Offset: 0x000AD873
	protected override void OnStartTutorial()
	{
		this.m_serverFlowController.SetOrdersAutoProgress(false);
		this.m_serverFlowController.RegisterOnSuccessfulDeliveryCallback(new VoidGeneric<RecipeList.Entry>(this.OnSuccessfulOrder));
		this.m_serverFlowController.AddNextOrder();
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x000AF4A3 File Offset: 0x000AD8A3
	protected override void OnTutorialUpdate()
	{
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000AF4A5 File Offset: 0x000AD8A5
	protected override void OnStopTutorial()
	{
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x000AF4A8 File Offset: 0x000AD8A8
	protected void OnSuccessfulOrder(RecipeList.Entry _node)
	{
		if (this.m_stage == TutorialIconController.TutorialStage.Lettuce)
		{
			this.m_stage = TutorialIconController.TutorialStage.LettuceTomato;
			this.m_serverFlowController.AddNextOrder();
		}
		else if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomato)
		{
			this.m_stage = TutorialIconController.TutorialStage.LettuceTomatoCucumber;
			this.m_serverFlowController.AddNextOrder();
		}
		else if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber)
		{
			this.m_serverFlowController.SetOrdersAutoProgress(true);
			this.m_serverFlowController.UnregisterOnSuccessfulDeliveryCallback(new VoidGeneric<RecipeList.Entry>(this.OnSuccessfulOrder));
		}
	}

	// Token: 0x04001CB2 RID: 7346
	private TutorialIconController m_tutorialController;

	// Token: 0x04001CB3 RID: 7347
	private ServerCampaignFlowController m_serverFlowController;

	// Token: 0x04001CB4 RID: 7348
	private TutorialIconController.TutorialStage m_stage;
}

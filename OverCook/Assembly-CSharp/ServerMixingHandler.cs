using System;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000486 RID: 1158
public class ServerMixingHandler : ServerSynchroniserBase, IMixable
{
	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06001588 RID: 5512 RVA: 0x00074788 File Offset: 0x00072B88
	public float AccessMixingTime
	{
		get
		{
			return this.GetMixingHandler().m_mixingTime;
		}
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x00074795 File Offset: 0x00072B95
	public override EntityType GetEntityType()
	{
		return EntityType.MixingState;
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x00074799 File Offset: 0x00072B99
	public override Serialisable GetServerUpdate()
	{
		return this.m_serverData;
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x000747A1 File Offset: 0x00072BA1
	protected override void OnEnable()
	{
		base.OnEnable();
		this.SetMixingProgress(this.m_serverData.m_mixingProgress);
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x000747BA File Offset: 0x00072BBA
	protected override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x000747C2 File Offset: 0x00072BC2
	public MixedCompositeOrderNode.MixingProgress GetMixedOrderState()
	{
		return this.GetMixingHandler().GetMixedOrderState(this.m_serverData.m_mixingProgress);
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x000747DC File Offset: 0x00072BDC
	public void SetMixingProgress(float _mixingProgress)
	{
		MixingHandler mixingHandler = this.GetMixingHandler();
		this.m_serverData.m_mixingProgress = _mixingProgress;
		CookingUIController.State mixingState = this.m_serverData.m_mixingState;
		if (this.IsOverMixed())
		{
			this.m_serverData.m_mixingState = CookingUIController.State.Ruined;
		}
		else if (this.IsMixed())
		{
			if (this.m_serverData.m_mixingState == CookingUIController.State.Progressing)
			{
				this.m_serverData.m_mixingState = CookingUIController.State.Completed;
			}
			if (_mixingProgress > 1.3f * mixingHandler.m_mixingTime)
			{
				this.m_serverData.m_mixingState = CookingUIController.State.OverDoing;
			}
		}
		else
		{
			this.m_serverData.m_mixingState = CookingUIController.State.Progressing;
		}
		if (this.m_serverData.m_mixingState != mixingState)
		{
			this.m_stateChangedCallback(this.m_serverData.m_mixingState);
		}
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x000748A2 File Offset: 0x00072CA2
	public MixingHandler GetMixingHandler()
	{
		if (this.m_mixingHandler == null)
		{
			this.m_mixingHandler = base.gameObject.RequireComponent<MixingHandler>();
		}
		return this.m_mixingHandler;
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x000748CC File Offset: 0x00072CCC
	public float GetMixingProgress()
	{
		return this.m_serverData.m_mixingProgress;
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x000748D9 File Offset: 0x00072CD9
	public bool IsMixed()
	{
		return this.m_serverData.m_mixingProgress >= this.AccessMixingTime;
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x000748F1 File Offset: 0x00072CF1
	public bool IsOverMixed()
	{
		return this.m_serverData.m_mixingProgress > 2f * this.AccessMixingTime;
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x0007490C File Offset: 0x00072D0C
	public bool Mix(float _deltaTime)
	{
		if (!this.IsOverMixed())
		{
			this.SetMixingProgress(this.m_serverData.m_mixingProgress + _deltaTime);
			return true;
		}
		return false;
	}

	// Token: 0x04001068 RID: 4200
	private MixingHandler m_mixingHandler;

	// Token: 0x04001069 RID: 4201
	private MixingStateMessage m_serverData = new MixingStateMessage();

	// Token: 0x0400106A RID: 4202
	public StateChanged m_stateChangedCallback = delegate(CookingUIController.State _state)
	{
	};
}

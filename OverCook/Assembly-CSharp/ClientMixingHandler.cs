using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class ClientMixingHandler : ClientSynchroniserBase, IClientMixable
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06001596 RID: 5526 RVA: 0x00074980 File Offset: 0x00072D80
	public float AccessMixingTime
	{
		get
		{
			return this.GetMixingHandler().m_mixingTime;
		}
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x00074990 File Offset: 0x00072D90
	public override void StartSynchronising(Component synchronisedObject)
	{
		GameObject gameObject = GameUtils.InstantiateUIController(this.GetMixingHandler().m_progressUIPrefab.gameObject, "HoverIconCanvas");
		if (gameObject != null)
		{
			this.m_progressUI = gameObject.GetComponent<CookingUIController>();
			this.m_progressUI.SetFollowTransform(NetworkUtils.FindVisualRoot(base.gameObject), Vector3.zero);
		}
		this.m_IMixingNotified = base.gameObject.RequestInterfacesRecursive<IClientMixingNotifed>();
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x000749FC File Offset: 0x00072DFC
	public override EntityType GetEntityType()
	{
		return EntityType.MixingState;
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x00074A00 File Offset: 0x00072E00
	public override void ApplyServerUpdate(Serialisable serialisable)
	{
		MixingHandler mixingHandler = this.GetMixingHandler();
		MixingStateMessage mixingStateMessage = (MixingStateMessage)serialisable;
		bool flag = false;
		if (this.m_cachedMixingProgress != mixingStateMessage.m_mixingProgress)
		{
			this.m_cachedMixingProgress = mixingStateMessage.m_mixingProgress;
			flag = true;
		}
		if (this.m_cachedMixingState != mixingStateMessage.m_mixingState)
		{
			this.m_cachedMixingState = mixingStateMessage.m_mixingState;
			flag = true;
			this.m_stateChangedCallback(this.m_cachedMixingState);
		}
		if (flag)
		{
			this.m_IMixingNotified = base.gameObject.RequestInterfacesRecursive<IClientMixingNotifed>();
			float newProp = mixingStateMessage.m_mixingProgress / mixingHandler.m_mixingTime;
			for (int i = 0; i < this.m_IMixingNotified.Length; i++)
			{
				IClientMixingNotifed clientMixingNotifed = this.m_IMixingNotified[i];
				clientMixingNotifed.OnMixingPropChanged(newProp);
			}
			if (mixingStateMessage.m_mixingProgress > 0.001f)
			{
				if (this.m_progressUI != null)
				{
					this.m_progressUI.gameObject.SetActive(true);
					this.m_progressUI.SetProgress(mixingStateMessage.m_mixingProgress / mixingHandler.m_mixingTime);
					float b = Mathf.Clamp(2f * mixingHandler.m_mixingTime - 0.5f, mixingHandler.m_mixingTime, 2f * mixingHandler.m_mixingTime);
					this.m_progressUI.SetOverDoingAmount(MathUtils.ClampedRemap(mixingStateMessage.m_mixingProgress, mixingHandler.m_mixingTime, b, 0f, 1f));
					this.m_progressUI.SetState(mixingStateMessage.m_mixingState);
				}
				MixableContainer x = base.gameObject.RequestComponent<MixableContainer>();
				ClientHandlePlacementReferral clientHandlePlacementReferral = base.gameObject.RequestComponent<ClientHandlePlacementReferral>();
				ClientAttachStation clientAttachStation = clientHandlePlacementReferral.GetHandlePlacementReferree() as ClientAttachStation;
				bool flag2 = false;
				if (clientAttachStation != null && x != null && clientAttachStation.gameObject.RequestComponent<MixingStation>() != null)
				{
					flag2 = true;
				}
				if (flag2)
				{
					this.m_progressUI.SetState(mixingStateMessage.m_mixingState);
					for (int j = 0; j < this.m_IMixingNotified.Length; j++)
					{
						IClientMixingNotifed clientMixingNotifed2 = this.m_IMixingNotified[j];
						clientMixingNotifed2.OnMixingStarted();
					}
					this.m_hideAfterTime = 1f;
				}
				else
				{
					this.m_hideAfterTime = 0f;
					if (this.m_cachedMixingState != CookingUIController.State.Progressing && this.m_progressUI != null)
					{
						this.m_progressUI.SetState(CookingUIController.State.Idle);
					}
				}
			}
			else
			{
				if (this.m_progressUI != null)
				{
					this.m_progressUI.gameObject.SetActive(false);
				}
				this.m_hideAfterTime = float.MinValue;
				for (int k = 0; k < this.m_IMixingNotified.Length; k++)
				{
					IClientMixingNotifed clientMixingNotifed3 = this.m_IMixingNotified[k];
					clientMixingNotifed3.OnMixingFinished();
				}
			}
		}
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x00074CBA File Offset: 0x000730BA
	public GameLoopingAudioTag GetMixingSoundTag()
	{
		return this.GetMixingHandler().m_mixingType.m_sizzleSound;
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x00074CCC File Offset: 0x000730CC
	public MixedCompositeOrderNode.MixingProgress GetMixedOrderState()
	{
		return this.GetMixingHandler().GetMixedOrderState(this.m_cachedMixingProgress);
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x00074CDF File Offset: 0x000730DF
	public float GetMixingProgress()
	{
		return this.m_cachedMixingProgress;
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x00074CE7 File Offset: 0x000730E7
	public MixingHandler GetMixingHandler()
	{
		if (this.m_mixingHandler == null)
		{
			this.m_mixingHandler = base.gameObject.RequireComponent<MixingHandler>();
		}
		return this.m_mixingHandler;
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x00074D11 File Offset: 0x00073111
	private void UpdateCosmetics()
	{
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x00074D14 File Offset: 0x00073114
	public override void UpdateSynchronising()
	{
		float hideAfterTime = this.m_hideAfterTime;
		this.m_hideAfterTime -= TimeManager.GetDeltaTime(base.gameObject);
		if (hideAfterTime >= 0f && this.m_hideAfterTime < 0f)
		{
			if (this.m_cachedMixingState == CookingUIController.State.OverDoing && this.m_progressUI != null)
			{
				this.m_progressUI.SetState(CookingUIController.State.Idle);
			}
			for (int i = 0; i < this.m_IMixingNotified.Length; i++)
			{
				IClientMixingNotifed clientMixingNotifed = this.m_IMixingNotified[i];
				clientMixingNotifed.OnMixingFinished();
			}
		}
		this.UpdateCosmetics();
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x00074DB2 File Offset: 0x000731B2
	public bool IsMixed()
	{
		return this.m_cachedMixingProgress >= this.AccessMixingTime;
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x00074DC5 File Offset: 0x000731C5
	public bool IsOverMixed()
	{
		return this.m_cachedMixingProgress > 2f * this.AccessMixingTime;
	}

	// Token: 0x0400106C RID: 4204
	public StateChanged m_stateChangedCallback = delegate(CookingUIController.State _state)
	{
	};

	// Token: 0x0400106D RID: 4205
	private MixingHandler m_mixingHandler;

	// Token: 0x0400106E RID: 4206
	private IClientMixingNotifed[] m_IMixingNotified;

	// Token: 0x0400106F RID: 4207
	private CookingUIController m_progressUI;

	// Token: 0x04001070 RID: 4208
	private float m_hideAfterTime = float.MinValue;

	// Token: 0x04001071 RID: 4209
	private CookingUIController.State m_cachedMixingState;

	// Token: 0x04001072 RID: 4210
	private float m_cachedMixingProgress = -1f;
}

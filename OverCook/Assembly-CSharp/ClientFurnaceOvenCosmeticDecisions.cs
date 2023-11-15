using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class ClientFurnaceOvenCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x060011D4 RID: 4564 RVA: 0x00065930 File Offset: 0x00063D30
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cosmetics = (FurnaceOvenCosmeticDecisions)synchronisedObject;
		this.m_attachStation = base.gameObject.GetComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_cookingStation = base.gameObject.RequireComponent<HeatedCookingStation>();
		if (this.m_cookingStation.m_heatSource != null)
		{
			this.m_heatedStation = this.m_cookingStation.m_heatSource.gameObject.RequireComponent<ClientHeatedStation>();
			this.m_heatedStation.RegisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		}
		this.UpdateVisuals(HeatRange.Low);
		this.m_audioManager = GameUtils.RequestManager<AudioManager>();
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x000659FA File Offset: 0x00063DFA
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_audioManager != null && this.m_activeToken != null)
		{
			this.m_audioManager.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00065A34 File Offset: 0x00063E34
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_item != null)
		{
			this.OnItemRemoved(this.m_item);
		}
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		}
		if (this.m_heatedStation != null)
		{
			this.m_heatedStation.UnregisterHeatRangeChangedCallback(new GenericVoid<HeatRange>(this.OnHeatRangeChanged));
		}
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x00065AC8 File Offset: 0x00063EC8
	private void OnItemAdded(IClientAttachment _iHoldable)
	{
		this.m_item = _iHoldable;
		IOrderDefinition orderDefinition = _iHoldable.AccessGameObject().RequestInterface<IOrderDefinition>();
		if (orderDefinition != null)
		{
			orderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
		}
		this.SetOccupiedState(this.IsOccupantValid(_iHoldable));
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x00065B10 File Offset: 0x00063F10
	private void OnItemRemoved(IClientAttachment _iHoldable)
	{
		if (this.m_item != null)
		{
			IOrderDefinition orderDefinition = _iHoldable.AccessGameObject().RequestInterface<IOrderDefinition>();
			if (orderDefinition != null)
			{
				orderDefinition.UnregisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
			}
			this.m_item = null;
		}
		this.SetOccupiedState(false);
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x00065B5A File Offset: 0x00063F5A
	private void OnOrderCompositionChanged(AssembledDefinitionNode _contents)
	{
		this.SetOccupiedState(this.IsOrderOccupantValid(_contents));
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x00065B6C File Offset: 0x00063F6C
	private bool IsOccupantValid(IClientAttachment _iHoldable)
	{
		IClientCookable clientCookable = _iHoldable.AccessGameObject().RequestInterface<IClientCookable>();
		if (clientCookable == null || clientCookable.GetRequiredStationType() != this.m_cookingStation.m_stationType)
		{
			return false;
		}
		IClientOrderDefinition clientOrderDefinition = _iHoldable.AccessGameObject().RequireInterface<IClientOrderDefinition>();
		AssembledDefinitionNode orderComposition = clientOrderDefinition.GetOrderComposition();
		return this.IsOrderOccupantValid(orderComposition);
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x00065BC8 File Offset: 0x00063FC8
	private bool IsOrderOccupantValid(AssembledDefinitionNode _contents)
	{
		AssembledDefinitionNode assembledDefinitionNode = _contents.Simpilfy();
		if (assembledDefinitionNode == AssembledDefinitionNode.NullNode)
		{
			return false;
		}
		if (assembledDefinitionNode is CompositeAssembledNode)
		{
			CompositeAssembledNode compositeAssembledNode = _contents as CompositeAssembledNode;
			for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
			{
				MixedCompositeAssembledNode mixedCompositeAssembledNode = compositeAssembledNode.m_composition[i] as MixedCompositeAssembledNode;
				if (mixedCompositeAssembledNode != null && mixedCompositeAssembledNode.m_progress != MixedCompositeOrderNode.MixingProgress.Mixed)
				{
					return false;
				}
			}
		}
		else
		{
			MixedCompositeAssembledNode mixedCompositeAssembledNode2 = _contents as MixedCompositeAssembledNode;
			if (mixedCompositeAssembledNode2 != null && mixedCompositeAssembledNode2.m_progress != MixedCompositeOrderNode.MixingProgress.Mixed)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x00065C5A File Offset: 0x0006405A
	private void SetOccupiedState(bool _isOccupied)
	{
		this.m_cosmetics.m_animator.SetBool(ClientFurnaceOvenCosmeticDecisions.m_Open, !_isOccupied);
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00065C75 File Offset: 0x00064075
	private void OnHeatRangeChanged(HeatRange _heatRange)
	{
		this.UpdateVisuals(_heatRange);
		this.UpdateAudio(_heatRange);
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x00065C88 File Offset: 0x00064088
	private void UpdateVisuals(HeatRange _heat)
	{
		this.ToggleEffect(this.m_cosmetics.m_highEffect, _heat == HeatRange.High);
		this.ToggleEffect(this.m_cosmetics.m_mediumEffect, _heat == HeatRange.Moderate);
		this.ToggleEffect(this.m_cosmetics.m_lowEffect, _heat == HeatRange.Low);
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x00065CD4 File Offset: 0x000640D4
	private void UpdateAudio(HeatRange _heat)
	{
		if (this.m_activeToken != null)
		{
			GameUtils.StopAudio(GameLoopingAudioTag.COUNT, this.m_activeToken);
		}
		if (_heat != HeatRange.High)
		{
			if (_heat != HeatRange.Moderate)
			{
				this.m_activeToken = null;
			}
			else
			{
				this.m_activeToken = this.m_mediumHeatToken;
			}
		}
		else
		{
			this.m_activeToken = this.m_highHeatToken;
		}
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x00065D3C File Offset: 0x0006413C
	private void ToggleEffect(GameObject _effect, bool _turnOn)
	{
		if (_effect != null)
		{
			_effect.SetActive(_turnOn);
		}
	}

	// Token: 0x04000DE5 RID: 3557
	private HeatedCookingStation m_cookingStation;

	// Token: 0x04000DE6 RID: 3558
	private FurnaceOvenCosmeticDecisions m_cosmetics;

	// Token: 0x04000DE7 RID: 3559
	private IClientAttachment m_item;

	// Token: 0x04000DE8 RID: 3560
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04000DE9 RID: 3561
	private ClientAttachStation m_attachStation;

	// Token: 0x04000DEA RID: 3562
	private ClientHeatedStation m_heatedStation;

	// Token: 0x04000DEB RID: 3563
	private AudioManager m_audioManager;

	// Token: 0x04000DEC RID: 3564
	private static int m_Open = Animator.StringToHash("Open");

	// Token: 0x04000DED RID: 3565
	private object m_highHeatToken = new object();

	// Token: 0x04000DEE RID: 3566
	private object m_mediumHeatToken = new object();

	// Token: 0x04000DEF RID: 3567
	private object m_activeToken;
}

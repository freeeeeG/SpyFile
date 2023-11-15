using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class ServerMixingStation : ServerSynchroniserBase
{
	// Token: 0x0600188F RID: 6287 RVA: 0x0007CB29 File Offset: 0x0007AF29
	public override EntityType GetEntityType()
	{
		return EntityType.MixingStation;
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x0007CB2D File Offset: 0x0007AF2D
	private void SynchroniseMixingState()
	{
		this.m_data.m_isTurnedOn = this.m_bMixerOn;
		this.m_pendingSyncMessage = true;
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x0007CB48 File Offset: 0x0007AF48
	private void Awake()
	{
		this.m_MixingStation = base.gameObject.RequireComponent<MixingStation>();
		this.m_AttachStation = base.gameObject.RequireComponent<ServerAttachStation>();
		this.m_Flammable = base.gameObject.RequireComponent<ServerFlammable>();
		this.m_LevelConfig = GameUtils.GetLevelConfig();
		this.m_AttachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_AttachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_AttachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		if (this.m_AttachStation.HasItem())
		{
			this.OnItemAdded(this.m_AttachStation.InspectItem().RequireInterface<IAttachment>());
		}
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x0007CC00 File Offset: 0x0007B000
	public override void OnDestroy()
	{
		this.m_AttachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_AttachStation.UnregisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_AttachStation.UnregisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		base.OnDestroy();
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x0007CC58 File Offset: 0x0007B058
	public override void UpdateSynchronising()
	{
		if (this.m_bMixerOn && this.m_ItemMixer != null && this.m_ItemMixer.Mix(TimeManager.GetDeltaTime(base.gameObject)) && this.m_ItemMixer.IsOverMixed())
		{
			this.SetMixerOn(false);
		}
		if (this.m_pendingSyncMessage)
		{
			this.m_pendingSyncMessage = false;
			this.SendServerEvent(this.m_data);
		}
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x0007CCCB File Offset: 0x0007B0CB
	private void SetMixerOn(bool _bOn)
	{
		this.m_bMixerOn = _bOn;
		this.SynchroniseMixingState();
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x0007CCDC File Offset: 0x0007B0DC
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		PlacementContext.Source source = _context.m_source;
		if (source == PlacementContext.Source.Game)
		{
			return true;
		}
		if (source != PlacementContext.Source.Player)
		{
			return false;
		}
		if (!this.m_MixingStation.m_bAttachRestrictions)
		{
			return true;
		}
		if (_object.RequestInterface<IMixable>() == null)
		{
			return false;
		}
		ICookable cookable = _object.RequestInterface<ICookable>();
		if (cookable != null)
		{
			IIngredientContents ingredientContents = _object.RequestInterface<IIngredientContents>();
			if (ingredientContents != null && ingredientContents.HasContents() && (cookable.GetCookedOrderState() != CookedCompositeOrderNode.CookingProgress.Raw || cookable.GetCookingProgress() > 0f))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x0007CD70 File Offset: 0x0007B170
	private void OnItemAdded(IAttachment _iHoldable)
	{
		this.m_ItemMixer = _iHoldable.AccessGameObject().RequestInterface<IMixable>();
		if (this.m_ItemMixer != null)
		{
			ServerMixableContainer serverMixableContainer = _iHoldable.AccessGameObject().RequireComponent<ServerMixableContainer>();
			serverMixableContainer.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
			this.OnOrderCompositionChanged(serverMixableContainer.GetOrderComposition());
			if (this.m_MixingStation.m_itemBlock != null)
			{
				this.m_MixingStation.m_itemBlock.enabled = false;
			}
		}
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x0007CDEC File Offset: 0x0007B1EC
	private void OnItemRemoved(IAttachment _iHoldable)
	{
		if (this.m_ItemMixer != null)
		{
			GameObject gameObject = (this.m_ItemMixer as MonoBehaviour).gameObject;
			ServerMixableContainer serverMixableContainer = gameObject.RequireComponent<ServerMixableContainer>();
			serverMixableContainer.UnregisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
			this.OnOrderCompositionChanged(AssembledDefinitionNode.NullNode);
			this.m_ItemMixer = null;
			if (this.m_MixingStation.m_itemBlock != null)
			{
				this.m_MixingStation.m_itemBlock.enabled = true;
			}
		}
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x0007CE68 File Offset: 0x0007B268
	private void OnOrderCompositionChanged(AssembledDefinitionNode _contents)
	{
		if (this.m_ItemMixer != null)
		{
			this.SetMixerOn(_contents.Simpilfy() != AssembledDefinitionNode.NullNode && !this.m_ItemMixer.IsOverMixed() && !this.m_Flammable.OnFire());
		}
		else
		{
			this.SetMixerOn(_contents.Simpilfy() != AssembledDefinitionNode.NullNode && !this.m_Flammable.OnFire());
		}
	}

	// Token: 0x040013BA RID: 5050
	private MixingStation m_MixingStation;

	// Token: 0x040013BB RID: 5051
	protected MixingStationMessage m_data = new MixingStationMessage();

	// Token: 0x040013BC RID: 5052
	private ServerAttachStation m_AttachStation;

	// Token: 0x040013BD RID: 5053
	private ServerFlammable m_Flammable;

	// Token: 0x040013BE RID: 5054
	private LevelConfigBase m_LevelConfig;

	// Token: 0x040013BF RID: 5055
	private IMixable m_ItemMixer;

	// Token: 0x040013C0 RID: 5056
	private bool m_bMixerOn;

	// Token: 0x040013C1 RID: 5057
	private bool m_pendingSyncMessage;
}

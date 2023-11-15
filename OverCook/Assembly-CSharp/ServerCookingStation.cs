using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class ServerCookingStation : ServerSynchroniserBase
{
	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060014C9 RID: 5321 RVA: 0x00071948 File Offset: 0x0006FD48
	public CookingStationType StationType
	{
		get
		{
			return this.m_cookingStation.m_stationType;
		}
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x00071955 File Offset: 0x0006FD55
	public override EntityType GetEntityType()
	{
		return EntityType.CookingStation;
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x00071959 File Offset: 0x0006FD59
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x00071962 File Offset: 0x0006FD62
	private void SynchroniseCookingState()
	{
		this.m_data.m_isTurnedOn = this.m_isTurnedOn;
		this.m_data.m_isCooking = this.m_isCooking;
		this.m_pendingSyncMessage = true;
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x0007198D File Offset: 0x0006FD8D
	private void SetCookerOn(bool _isOn)
	{
		this.m_isTurnedOn = _isOn;
		this.SynchroniseCookingState();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x0007199C File Offset: 0x0006FD9C
	private void SetCooking(bool _isCooking)
	{
		this.m_isCooking = _isCooking;
		this.SynchroniseCookingState();
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000719AC File Offset: 0x0006FDAC
	protected virtual void Awake()
	{
		this.m_cookingStation = base.gameObject.RequireComponent<CookingStation>();
		this.m_attachStation = base.gameObject.GetComponent<ServerAttachStation>();
		this.m_flammable = base.gameObject.GetComponent<ServerFlammable>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_levelConfigBase = GameUtils.GetLevelConfig();
		if (this.m_attachStation.HasItem())
		{
			this.OnItemAdded(this.m_attachStation.InspectItem().GetComponent<IAttachment>());
		}
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x00071A64 File Offset: 0x0006FE64
	public override void OnDestroy()
	{
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
			this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		}
		base.OnDestroy();
	}

	// Token: 0x060014D1 RID: 5329 RVA: 0x00071AD0 File Offset: 0x0006FED0
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
		MixedCompositeOrderNode.MixingProgress? mixingProgress = null;
		IMixable mixable = _object.RequestInterface<IMixable>();
		if (mixable != null)
		{
			mixingProgress = new MixedCompositeOrderNode.MixingProgress?(mixable.GetMixedOrderState());
		}
		return this.m_cookingStation.CanAddItem(_object, mixingProgress);
	}

	// Token: 0x060014D2 RID: 5330 RVA: 0x00071B2C File Offset: 0x0006FF2C
	protected virtual void OnItemAdded(IAttachment _iHoldable)
	{
		this.m_itemPot = _iHoldable.AccessGameObject().RequestInterface<ICookable>();
		if (this.m_itemPot != null)
		{
			if (this.m_itemPot.GetRequiredStationType() == this.StationType)
			{
				IOrderDefinition orderDefinition = _iHoldable.AccessGameObject().RequireInterface<IOrderDefinition>();
				orderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
				this.OnOrderCompositionChanged(orderDefinition.GetOrderComposition());
			}
			if (this.m_cookingStation.m_itemBlock != null)
			{
				this.m_cookingStation.m_itemBlock.enabled = false;
			}
		}
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x00071BBC File Offset: 0x0006FFBC
	protected virtual void OnItemRemoved(IAttachment _iHoldable)
	{
		if (this.m_itemPot != null)
		{
			GameObject gameObject = (this.m_itemPot as MonoBehaviour).gameObject;
			if (this.m_itemPot.GetRequiredStationType() == this.StationType)
			{
				IOrderDefinition orderDefinition = gameObject.RequireInterface<IOrderDefinition>();
				orderDefinition.UnregisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
			}
			this.OnOrderCompositionChanged(AssembledDefinitionNode.NullNode);
			this.m_itemPot = null;
			if (this.m_cookingStation.m_itemBlock != null)
			{
				this.m_cookingStation.m_itemBlock.enabled = true;
			}
		}
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x00071C50 File Offset: 0x00070050
	private bool CanCook(AssembledDefinitionNode _contents)
	{
		if (this.m_flammable != null && this.m_flammable.OnFire())
		{
			return false;
		}
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
		if (this.m_itemPot != null && (_contents is MixedCompositeAssembledNode || assembledDefinitionNode is MixedCompositeAssembledNode))
		{
			CookedCompositeAssembledNode cookedCompositeAssembledNode = assembledDefinitionNode as CookedCompositeAssembledNode;
			if (cookedCompositeAssembledNode == null)
			{
				cookedCompositeAssembledNode = new CookedCompositeAssembledNode();
				cookedCompositeAssembledNode.m_composition = new AssembledDefinitionNode[]
				{
					assembledDefinitionNode
				};
				cookedCompositeAssembledNode.m_cookingStep = this.m_itemPot.AccessCookingType;
			}
			cookedCompositeAssembledNode.m_progress = CookedCompositeOrderNode.CookingProgress.Cooked;
			if (!GameUtils.IsValidRecipe(cookedCompositeAssembledNode))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x00071D75 File Offset: 0x00070175
	private void OnOrderCompositionChanged(AssembledDefinitionNode _contents)
	{
		this.SetCookerOn(this.CanCook(_contents));
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x00071D84 File Offset: 0x00070184
	public override void UpdateSynchronising()
	{
		if (this.m_isTurnedOn && this.m_itemPot != null)
		{
			bool flag = this.m_itemPot.Cook(this.m_cookingSpeed * TimeManager.GetDeltaTime(base.gameObject));
			if (this.m_isCooking)
			{
				bool flag2 = this.m_levelConfigBase != null && this.m_levelConfigBase.m_hazardInfo != null && this.m_levelConfigBase.m_hazardInfo.FireConfigData != null;
				if (flag2 && this.m_itemPot.IsBurning())
				{
					this.SetCookerOn(false);
					if (this.m_flammable != null)
					{
						this.m_flammable.Ignite();
					}
				}
			}
			if (flag != this.m_isCooking)
			{
				this.SetCooking(flag);
			}
		}
		else if (this.m_isCooking)
		{
			this.SetCooking(false);
		}
		if (this.m_pendingSyncMessage)
		{
			this.m_pendingSyncMessage = false;
			this.SendServerEvent(this.m_data);
		}
	}

	// Token: 0x04001004 RID: 4100
	private CookingStation m_cookingStation;

	// Token: 0x04001005 RID: 4101
	private CookingStationMessage m_data = new CookingStationMessage();

	// Token: 0x04001006 RID: 4102
	private LevelConfigBase m_levelConfigBase;

	// Token: 0x04001007 RID: 4103
	private ServerFlammable m_flammable;

	// Token: 0x04001008 RID: 4104
	private ICookable m_itemPot;

	// Token: 0x04001009 RID: 4105
	private ServerAttachStation m_attachStation;

	// Token: 0x0400100A RID: 4106
	protected float m_cookingSpeed = 1f;

	// Token: 0x0400100B RID: 4107
	protected bool m_isTurnedOn;

	// Token: 0x0400100C RID: 4108
	protected bool m_isCooking;

	// Token: 0x0400100D RID: 4109
	private bool m_pendingSyncMessage;
}

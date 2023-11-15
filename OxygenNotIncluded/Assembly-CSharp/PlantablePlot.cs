using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000670 RID: 1648
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantablePlot : SingleEntityReceptacle, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000E7C31 File Offset: 0x000E5E31
	// (set) Token: 0x06002B94 RID: 11156 RVA: 0x000E7C3E File Offset: 0x000E5E3E
	public KPrefabID plant
	{
		get
		{
			return this.plantRef.Get();
		}
		set
		{
			this.plantRef.Set(value);
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000E7C4C File Offset: 0x000E5E4C
	public bool ValidPlant
	{
		get
		{
			return this.plantPreview == null || this.plantPreview.Valid;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06002B96 RID: 11158 RVA: 0x000E7C69 File Offset: 0x000E5E69
	public bool AcceptsFertilizer
	{
		get
		{
			return this.accepts_fertilizer;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06002B97 RID: 11159 RVA: 0x000E7C71 File Offset: 0x000E5E71
	public bool AcceptsIrrigation
	{
		get
		{
			return this.accepts_irrigation;
		}
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000E7C7C File Offset: 0x000E5E7C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!DlcManager.FeaturePlantMutationsEnabled())
		{
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
			return;
		}
		if (this.requestedEntityTag.IsValid && this.requestedEntityAdditionalFilterTag.IsValid && !PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag))
		{
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000E7CDC File Offset: 0x000E5EDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.FarmFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedSeed;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableSeed;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingSeedDelivery;
		this.plantRef = new Ref<KPrefabID>();
		base.Subscribe<PlantablePlot>(-905833192, PlantablePlot.OnCopySettingsDelegate);
		base.Subscribe<PlantablePlot>(144050788, PlantablePlot.OnUpdateRoomDelegate);
		if (this.HasTag(GameTags.FarmTiles))
		{
			this.storage.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			DropAllWorkable component = base.GetComponent<DropAllWorkable>();
			if (component != null)
			{
				component.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			}
			Toggleable component2 = base.GetComponent<Toggleable>();
			if (component2 != null)
			{
				component2.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			}
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000E7DC4 File Offset: 0x000E5FC4
	private void OnCopySettings(object data)
	{
		PlantablePlot component = ((GameObject)data).GetComponent<PlantablePlot>();
		if (component != null)
		{
			if (base.occupyingObject == null && (this.requestedEntityTag != component.requestedEntityTag || this.requestedEntityAdditionalFilterTag != component.requestedEntityAdditionalFilterTag || component.occupyingObject != null))
			{
				Tag entityTag = component.requestedEntityTag;
				Tag additionalFilterTag = component.requestedEntityAdditionalFilterTag;
				if (component.occupyingObject != null)
				{
					SeedProducer component2 = component.occupyingObject.GetComponent<SeedProducer>();
					if (component2 != null)
					{
						entityTag = TagManager.Create(component2.seedInfo.seedId);
						MutantPlant component3 = component.occupyingObject.GetComponent<MutantPlant>();
						additionalFilterTag = (component3 ? component3.SubSpeciesID : Tag.Invalid);
					}
				}
				base.CancelActiveRequest();
				this.CreateOrder(entityTag, additionalFilterTag);
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component4 = base.GetComponent<Prioritizable>();
				if (component4 != null)
				{
					Prioritizable component5 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component5 != null)
					{
						component5.SetMasterPriority(component4.GetMasterPriority());
					}
				}
			}
		}
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000E7EE8 File Offset: 0x000E60E8
	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.SetPreview(entityTag, false);
		if (this.ValidPlant)
		{
			base.CreateOrder(entityTag, additionalFilterTag);
			return;
		}
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000E7F10 File Offset: 0x000E6110
	private void SyncPriority(PrioritySetting priority)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!object.Equals(component.GetMasterPriority(), priority))
		{
			component.SetMasterPriority(priority);
		}
		if (base.occupyingObject != null)
		{
			Prioritizable component2 = base.occupyingObject.GetComponent<Prioritizable>();
			if (component2 != null && !object.Equals(component2.GetMasterPriority(), priority))
			{
				component2.SetMasterPriority(component.GetMasterPriority());
			}
		}
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000E7F8C File Offset: 0x000E618C
	protected override void OnSpawn()
	{
		if (this.plant != null)
		{
			this.RegisterWithPlant(this.plant.gameObject);
		}
		base.OnSpawn();
		this.autoReplaceEntity = false;
		Components.PlantablePlots.Add(base.gameObject.GetMyWorldId(), this);
		Prioritizable component = base.GetComponent<Prioritizable>();
		component.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(component.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x000E8002 File Offset: 0x000E6202
	public void SetFertilizationFlags(bool fertilizer, bool liquid_piping)
	{
		this.accepts_fertilizer = fertilizer;
		this.has_liquid_pipe_input = liquid_piping;
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x000E8014 File Offset: 0x000E6214
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.plantPreview != null)
		{
			Util.KDestroyGameObject(this.plantPreview.gameObject);
		}
		if (base.occupyingObject)
		{
			base.occupyingObject.Trigger(-216549700, null);
		}
		Components.PlantablePlots.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000E807C File Offset: 0x000E627C
	protected override GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		PlantableSeed component = depositedEntity.GetComponent<PlantableSeed>();
		if (component != null)
		{
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(this), this.plantLayer);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(component.PlantID), position, this.plantLayer, null, 0);
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component2 != null)
			{
				component.GetComponent<MutantPlant>().CopyMutationsTo(component2);
			}
			gameObject.SetActive(true);
			this.destroyEntityOnDeposit = true;
			return gameObject;
		}
		this.destroyEntityOnDeposit = false;
		return depositedEntity;
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000E80F8 File Offset: 0x000E62F8
	protected override void ConfigureOccupyingObject(GameObject newPlant)
	{
		KPrefabID component = newPlant.GetComponent<KPrefabID>();
		this.plantRef.Set(component);
		this.RegisterWithPlant(newPlant);
		UprootedMonitor component2 = newPlant.GetComponent<UprootedMonitor>();
		if (component2)
		{
			component2.canBeUprooted = false;
		}
		this.autoReplaceEntity = false;
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null)
		{
			Prioritizable component4 = newPlant.GetComponent<Prioritizable>();
			if (component4 != null)
			{
				component4.SetMasterPriority(component3.GetMasterPriority());
				Prioritizable prioritizable = component4;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
			}
		}
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x000E818A File Offset: 0x000E638A
	public void ReplacePlant(GameObject plant, bool keepStorage)
	{
		if (keepStorage)
		{
			this.UnsubscribeFromOccupant();
			base.occupyingObject = null;
		}
		base.ForceDeposit(plant);
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x000E81A4 File Offset: 0x000E63A4
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.SetSceneLayer(this.plantLayer);
		this.OffsetAnim(component, this.occupyingObjectVisualOffset);
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000E81DC File Offset: 0x000E63DC
	private void RegisterWithPlant(GameObject plant)
	{
		base.occupyingObject = plant;
		ReceptacleMonitor component = plant.GetComponent<ReceptacleMonitor>();
		if (component)
		{
			component.SetReceptacle(this);
		}
		plant.Trigger(1309017699, this.storage);
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000E8217 File Offset: 0x000E6417
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			base.Subscribe(base.occupyingObject, -216549700, new Action<object>(this.OnOccupantUprooted));
		}
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000E824B File Offset: 0x000E644B
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		if (base.occupyingObject != null)
		{
			base.Unsubscribe(base.occupyingObject, -216549700, new Action<object>(this.OnOccupantUprooted));
		}
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x000E827E File Offset: 0x000E647E
	private void OnOccupantUprooted(object data)
	{
		this.autoReplaceEntity = false;
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000E82A0 File Offset: 0x000E64A0
	public override void OrderRemoveOccupant()
	{
		if (base.Occupant == null)
		{
			return;
		}
		Uprootable component = base.Occupant.GetComponent<Uprootable>();
		if (component == null)
		{
			return;
		}
		component.MarkForUproot(true);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000E82DC File Offset: 0x000E64DC
	public override void SetPreview(Tag entityTag, bool solid = false)
	{
		PlantableSeed plantableSeed = null;
		if (entityTag.IsValid)
		{
			GameObject prefab = Assets.GetPrefab(entityTag);
			if (prefab == null)
			{
				DebugUtil.LogWarningArgs(base.gameObject, new object[]
				{
					"Planter tried previewing a tag with no asset! If this was the 'Empty' tag, ignore it, that will go away in new save games. Otherwise... Eh? Tag was: ",
					entityTag
				});
				return;
			}
			plantableSeed = prefab.GetComponent<PlantableSeed>();
		}
		if (this.plantPreview != null)
		{
			KPrefabID component = this.plantPreview.GetComponent<KPrefabID>();
			if (plantableSeed != null && component != null && component.PrefabTag == plantableSeed.PreviewID)
			{
				return;
			}
			this.plantPreview.gameObject.Unsubscribe(-1820564715, new Action<object>(this.OnValidChanged));
			Util.KDestroyGameObject(this.plantPreview.gameObject);
		}
		if (plantableSeed != null)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(plantableSeed.PreviewID), Grid.SceneLayer.Front, null, 0);
			this.plantPreview = gameObject.GetComponent<EntityPreview>();
			gameObject.transform.SetPosition(Vector3.zero);
			gameObject.transform.SetParent(base.gameObject.transform, false);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			if (this.rotatable != null)
			{
				if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
				{
					gameObject.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
				}
				else if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R90));
				}
				else
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R180));
				}
			}
			else
			{
				gameObject.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
			}
			KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
			this.OffsetAnim(component2, this.occupyingObjectVisualOffset);
			gameObject.SetActive(true);
			gameObject.Subscribe(-1820564715, new Action<object>(this.OnValidChanged));
			if (solid)
			{
				this.plantPreview.SetSolid();
			}
			this.plantPreview.UpdateValidity();
		}
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x000E84CC File Offset: 0x000E66CC
	private void OffsetAnim(KBatchedAnimController kanim, Vector3 offset)
	{
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedOffset(offset);
		}
		kanim.Offset = offset;
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000E84F1 File Offset: 0x000E66F1
	private void OnValidChanged(object obj)
	{
		base.Trigger(-1820564715, obj);
		if (!this.plantPreview.Valid && base.GetActiveRequest != null)
		{
			base.CancelActiveRequest();
		}
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x000E851C File Offset: 0x000E671C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ENABLESDOMESTICGROWTH, UI.BUILDINGEFFECTS.TOOLTIPS.ENABLESDOMESTICGROWTH, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04001997 RID: 6551
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001998 RID: 6552
	[Serialize]
	private Ref<KPrefabID> plantRef;

	// Token: 0x04001999 RID: 6553
	public Vector3 occupyingObjectVisualOffset = Vector3.zero;

	// Token: 0x0400199A RID: 6554
	public Grid.SceneLayer plantLayer = Grid.SceneLayer.BuildingBack;

	// Token: 0x0400199B RID: 6555
	private EntityPreview plantPreview;

	// Token: 0x0400199C RID: 6556
	[SerializeField]
	private bool accepts_fertilizer;

	// Token: 0x0400199D RID: 6557
	[SerializeField]
	private bool accepts_irrigation = true;

	// Token: 0x0400199E RID: 6558
	[SerializeField]
	public bool has_liquid_pipe_input;

	// Token: 0x0400199F RID: 6559
	private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>(delegate(PlantablePlot component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040019A0 RID: 6560
	private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>(delegate(PlantablePlot component, object data)
	{
		if (component.plantRef.Get() != null)
		{
			component.plantRef.Get().Trigger(144050788, data);
		}
	});
}

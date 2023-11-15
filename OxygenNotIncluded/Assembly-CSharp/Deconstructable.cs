using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
[AddComponentMenu("KMonoBehaviour/Workable/Deconstructable")]
public class Deconstructable : Workable
{
	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06001ABE RID: 6846 RVA: 0x0008F174 File Offset: 0x0008D374
	private CellOffset[] placementOffsets
	{
		get
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				CellOffset[] array = component.Def.PlacementOffsets;
				Rotatable component2 = component.GetComponent<Rotatable>();
				if (component2 != null)
				{
					array = new CellOffset[component.Def.PlacementOffsets.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = component2.GetRotatedCellOffset(component.Def.PlacementOffsets[i]);
					}
				}
				return array;
			}
			OccupyArea component3 = base.GetComponent<OccupyArea>();
			if (component3 != null)
			{
				return component3.OccupiedCellsOffsets;
			}
			if (this.looseEntityDeconstructable)
			{
				return new CellOffset[]
				{
					new CellOffset(0, 0)
				};
			}
			global::Debug.Assert(false, "Ack! We put a Deconstructable on something that's neither a Building nor OccupyArea!", this);
			return null;
		}
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x0008F238 File Offset: 0x0008D438
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			base.SetWorkTime(component.Def.ConstructionTime * 0.5f);
			return;
		}
		base.SetWorkTime(30f);
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x0008F32C File Offset: 0x0008D52C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		CellOffset[] filter = null;
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
			filter = component.Def.ConstructionOffsetFilter;
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(this.placementOffsets, table, filter);
		base.SetOffsetTable(offsetTable);
		base.Subscribe<Deconstructable>(493375141, Deconstructable.OnRefreshUserMenuDelegate);
		base.Subscribe<Deconstructable>(-111137758, Deconstructable.OnRefreshUserMenuDelegate);
		base.Subscribe<Deconstructable>(2127324410, Deconstructable.OnCancelDelegate);
		base.Subscribe<Deconstructable>(-790448070, Deconstructable.OnDeconstructDelegate);
		if (this.constructionElements == null || this.constructionElements.Length == 0)
		{
			this.constructionElements = new Tag[1];
			this.constructionElements[0] = base.GetComponent<PrimaryElement>().Element.tag;
		}
		if (this.isMarkedForDeconstruction)
		{
			this.QueueDeconstruction();
		}
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x0008F418 File Offset: 0x0008D618
	protected override void OnStartWork(Worker worker)
	{
		this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, false);
		base.Trigger(1830962028, this);
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x0008F468 File Offset: 0x0008D668
	protected override void OnCompleteWork(Worker worker)
	{
		Building component = base.GetComponent<Building>();
		SimCellOccupier component2 = base.GetComponent<SimCellOccupier>();
		if (DetailsScreen.Instance != null && DetailsScreen.Instance.CompareTargetWith(base.gameObject))
		{
			DetailsScreen.Instance.Show(false);
		}
		PrimaryElement component3 = base.GetComponent<PrimaryElement>();
		float temperature = component3.Temperature;
		byte disease_idx = component3.DiseaseIdx;
		int disease_count = component3.DiseaseCount;
		if (component2 != null)
		{
			if (component.Def.TileLayer != ObjectLayer.NumLayers)
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				if (Grid.Objects[num, (int)component.Def.TileLayer] == base.gameObject)
				{
					Grid.Objects[num, (int)component.Def.ObjectLayer] = null;
					Grid.Objects[num, (int)component.Def.TileLayer] = null;
					Grid.Foundation[num] = false;
					TileVisualizer.RefreshCell(num, component.Def.TileLayer, component.Def.ReplacementLayer);
				}
			}
			component2.DestroySelf(delegate
			{
				this.TriggerDestroy(temperature, disease_idx, disease_count, worker);
			});
		}
		else
		{
			this.TriggerDestroy(temperature, disease_idx, disease_count);
		}
		if (component == null || component.Def.PlayConstructionSounds)
		{
			string sound = GlobalAssets.GetSound("Finish_Deconstruction_" + ((!this.audioSize.IsNullOrWhiteSpace()) ? this.audioSize : component.Def.AudioSize), false);
			if (sound != null)
			{
				KMonoBehaviour.PlaySound3DAtLocation(sound, base.gameObject.transform.GetPosition());
			}
		}
		base.Trigger(-702296337, this);
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0008F636 File Offset: 0x0008D836
	public bool HasBeenDestroyed
	{
		get
		{
			return this.destroyed;
		}
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x0008F640 File Offset: 0x0008D840
	public List<GameObject> ForceDestroyAndGetMaterials()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float temperature = component.Temperature;
		byte diseaseIdx = component.DiseaseIdx;
		int diseaseCount = component.DiseaseCount;
		return this.TriggerDestroy(temperature, diseaseIdx, diseaseCount);
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x0008F670 File Offset: 0x0008D870
	private List<GameObject> TriggerDestroy(float temperature, byte disease_idx, int disease_count, Worker tile_worker)
	{
		if (this == null || this.destroyed)
		{
			return null;
		}
		List<GameObject> result = this.SpawnItemsFromConstruction(temperature, disease_idx, disease_count, tile_worker);
		this.destroyed = true;
		base.gameObject.DeleteObject();
		return result;
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x0008F6A2 File Offset: 0x0008D8A2
	private List<GameObject> TriggerDestroy(float temperature, byte disease_idx, int disease_count)
	{
		return this.TriggerDestroy(temperature, disease_idx, disease_count, base.worker);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x0008F6B4 File Offset: 0x0008D8B4
	private void QueueDeconstruction()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.chore == null)
		{
			BuildingComplete component = base.GetComponent<BuildingComplete>();
			if (component != null && component.Def.ReplacementLayer != ObjectLayer.NumLayers)
			{
				int cell = Grid.PosToCell(component);
				if (Grid.Objects[cell, (int)component.Def.ReplacementLayer] != null)
				{
					return;
				}
			}
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Deconstructable>(Db.Get().ChoreTypes.Deconstruct, this, null, true, null, null, null, true, null, false, false, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, this);
			this.isMarkedForDeconstruction = true;
			base.Trigger(2108245096, "Deconstruct");
		}
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x0008F78A File Offset: 0x0008D98A
	private void OnDeconstruct()
	{
		if (this.chore == null)
		{
			this.QueueDeconstruction();
			return;
		}
		this.CancelDeconstruction();
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x0008F7A1 File Offset: 0x0008D9A1
	public bool IsMarkedForDeconstruction()
	{
		return this.chore != null;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x0008F7AC File Offset: 0x0008D9AC
	public void SetAllowDeconstruction(bool allow)
	{
		this.allowDeconstruction = allow;
		if (!this.allowDeconstruction)
		{
			this.CancelDeconstruction();
		}
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x0008F7C4 File Offset: 0x0008D9C4
	public void SpawnItemsFromConstruction(Worker chore_worker)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float temperature = component.Temperature;
		byte diseaseIdx = component.DiseaseIdx;
		int diseaseCount = component.DiseaseCount;
		this.SpawnItemsFromConstruction(temperature, diseaseIdx, diseaseCount, chore_worker);
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x0008F7F8 File Offset: 0x0008D9F8
	private List<GameObject> SpawnItemsFromConstruction(float temperature, byte disease_idx, int disease_count, Worker construction_worker)
	{
		List<GameObject> list = new List<GameObject>();
		if (!this.allowDeconstruction)
		{
			return list;
		}
		Building component = base.GetComponent<Building>();
		float[] array;
		if (component != null)
		{
			array = component.Def.Mass;
		}
		else
		{
			array = new float[]
			{
				base.GetComponent<PrimaryElement>().Mass
			};
		}
		int num = 0;
		while (num < this.constructionElements.Length && array.Length > num)
		{
			GameObject gameObject = this.SpawnItem(base.transform.GetPosition(), this.constructionElements[num], array[num], temperature, disease_idx, disease_count, construction_worker);
			int num2 = Grid.PosToCell(gameObject.transform.GetPosition());
			int num3 = Grid.CellAbove(num2);
			Vector2 zero;
			if ((Grid.IsValidCell(num2) && Grid.Solid[num2]) || (Grid.IsValidCell(num3) && Grid.Solid[num3]))
			{
				zero = Vector2.zero;
			}
			else
			{
				zero = new Vector2(UnityEngine.Random.Range(-1f, 1f) * Deconstructable.INITIAL_VELOCITY_RANGE.x, Deconstructable.INITIAL_VELOCITY_RANGE.y);
			}
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, zero);
			list.Add(gameObject);
			num++;
		}
		return list;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x0008F944 File Offset: 0x0008DB44
	public GameObject SpawnItem(Vector3 position, Tag src_element, float src_mass, float src_temperature, byte disease_idx, int disease_count, Worker chore_worker)
	{
		GameObject gameObject = null;
		int cell = Grid.PosToCell(position);
		CellOffset[] placementOffsets = this.placementOffsets;
		Element element = ElementLoader.GetElement(src_element);
		if (element != null)
		{
			float num = src_mass;
			int num2 = 0;
			while ((float)num2 < src_mass / 400f)
			{
				int num3 = num2 % placementOffsets.Length;
				int cell2 = Grid.OffsetCell(cell, placementOffsets[num3]);
				float mass = num;
				if (num > 400f)
				{
					mass = 400f;
					num -= 400f;
				}
				gameObject = element.substance.SpawnResource(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), mass, src_temperature, disease_idx, disease_count, false, false, false);
				gameObject.Trigger(580035959, chore_worker);
				num2++;
			}
		}
		else
		{
			int num4 = 0;
			while ((float)num4 < src_mass)
			{
				int num5 = num4 % placementOffsets.Length;
				int cell3 = Grid.OffsetCell(cell, placementOffsets[num5]);
				gameObject = GameUtil.KInstantiate(Assets.GetPrefab(src_element), Grid.CellToPosCBC(cell3, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
				gameObject.SetActive(true);
				gameObject.Trigger(580035959, chore_worker);
				num4++;
			}
		}
		return gameObject;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x0008FA44 File Offset: 0x0008DC44
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowDeconstruction)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore == null) ? new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DECONSTRUCT.NAME, new System.Action(this.OnDeconstruct), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DECONSTRUCT.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DECONSTRUCT.NAME_OFF, new System.Action(this.OnDeconstruct), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DECONSTRUCT.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0f);
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x0008FAE8 File Offset: 0x0008DCE8
	public void CancelDeconstruction()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancelled deconstruction");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, false);
			base.ShowProgressBar(false);
			this.isMarkedForDeconstruction = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x0008FB49 File Offset: 0x0008DD49
	private void OnCancel(object data)
	{
		this.CancelDeconstruction();
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x0008FB51 File Offset: 0x0008DD51
	private void OnDeconstruct(object data)
	{
		if (this.allowDeconstruction || DebugHandler.InstantBuildMode)
		{
			this.QueueDeconstruction();
		}
	}

	// Token: 0x04000EDF RID: 3807
	public Chore chore;

	// Token: 0x04000EE0 RID: 3808
	public bool allowDeconstruction = true;

	// Token: 0x04000EE1 RID: 3809
	public string audioSize;

	// Token: 0x04000EE2 RID: 3810
	[Serialize]
	private bool isMarkedForDeconstruction;

	// Token: 0x04000EE3 RID: 3811
	[Serialize]
	public Tag[] constructionElements;

	// Token: 0x04000EE4 RID: 3812
	public bool looseEntityDeconstructable;

	// Token: 0x04000EE5 RID: 3813
	private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Deconstructable>(delegate(Deconstructable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04000EE6 RID: 3814
	private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Deconstructable>(delegate(Deconstructable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04000EE7 RID: 3815
	private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Deconstructable>(delegate(Deconstructable component, object data)
	{
		component.OnDeconstruct(data);
	});

	// Token: 0x04000EE8 RID: 3816
	private static readonly Vector2 INITIAL_VELOCITY_RANGE = new Vector2(0.5f, 4f);

	// Token: 0x04000EE9 RID: 3817
	private bool destroyed;
}

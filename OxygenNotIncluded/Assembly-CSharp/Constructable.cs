using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000492 RID: 1170
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Constructable")]
public class Constructable : Workable, ISaveLoadable
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0008BB52 File Offset: 0x00089D52
	public Recipe Recipe
	{
		get
		{
			return this.building.Def.CraftRecipe;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0008BB64 File Offset: 0x00089D64
	// (set) Token: 0x06001A5A RID: 6746 RVA: 0x0008BB6C File Offset: 0x00089D6C
	public IList<Tag> SelectedElementsTags
	{
		get
		{
			return this.selectedElementsTags;
		}
		set
		{
			if (this.selectedElementsTags == null || this.selectedElementsTags.Length != value.Count)
			{
				this.selectedElementsTags = new Tag[value.Count];
			}
			value.CopyTo(this.selectedElementsTags, 0);
		}
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x0008BBA4 File Offset: 0x00089DA4
	public override string GetConversationTopic()
	{
		return this.building.Def.PrefabID;
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x0008BBB8 File Offset: 0x00089DB8
	protected override void OnCompleteWork(Worker worker)
	{
		float num = 0f;
		float num2 = 0f;
		bool flag = true;
		foreach (GameObject gameObject in this.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null))
				{
					num += component.Mass;
					num2 += component.Temperature * component.Mass;
					flag = (flag && component.HasTag(GameTags.Liquifiable));
				}
			}
		}
		if (num <= 0f)
		{
			DebugUtil.LogWarningArgs(base.gameObject, new object[]
			{
				"uhhh this constructable is about to generate a nan",
				"Item Count: ",
				this.storage.items.Count
			});
			return;
		}
		if (flag)
		{
			this.initialTemperature = Mathf.Min(num2 / num, 318.15f);
		}
		else
		{
			this.initialTemperature = Mathf.Clamp(num2 / num, 288.15f, 318.15f);
		}
		KAnimGraphTileVisualizer component2 = base.GetComponent<KAnimGraphTileVisualizer>();
		UtilityConnections connections = (component2 == null) ? ((UtilityConnections)0) : component2.Connections;
		bool flag2 = true;
		if (this.IsReplacementTile)
		{
			int cell = Grid.PosToCell(base.transform.GetLocalPosition());
			GameObject replacementCandidate = this.building.Def.GetReplacementCandidate(cell);
			if (replacementCandidate != null)
			{
				flag2 = false;
				SimCellOccupier component3 = replacementCandidate.GetComponent<SimCellOccupier>();
				if (component3 != null)
				{
					component3.DestroySelf(delegate
					{
						if (this != null && this.gameObject != null)
						{
							this.FinishConstruction(connections, worker);
						}
					});
				}
				else
				{
					Conduit component4 = replacementCandidate.GetComponent<Conduit>();
					if (component4 != null)
					{
						component4.GetFlowManager().MarkForReplacement(cell);
					}
					BuildingComplete component5 = replacementCandidate.GetComponent<BuildingComplete>();
					if (component5 != null)
					{
						component5.Subscribe(-21016276, delegate(object data)
						{
							this.FinishConstruction(connections, worker);
						});
					}
					else
					{
						global::Debug.LogWarning("Why am I trying to replace a: " + replacementCandidate.name);
						this.FinishConstruction(connections, worker);
					}
				}
				KAnimGraphTileVisualizer component6 = replacementCandidate.GetComponent<KAnimGraphTileVisualizer>();
				if (component6 != null)
				{
					component6.skipCleanup = true;
				}
				Deconstructable component7 = replacementCandidate.GetComponent<Deconstructable>();
				if (component7 != null)
				{
					component7.SpawnItemsFromConstruction(worker);
				}
				replacementCandidate.Trigger(1606648047, this.building.Def.TileLayer);
				replacementCandidate.DeleteObject();
			}
		}
		if (flag2)
		{
			this.FinishConstruction(connections, worker);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, base.GetComponent<KSelectable>().GetName(), base.transform, 1.5f, false);
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x0008BE98 File Offset: 0x0008A098
	private void FinishConstruction(UtilityConnections connections, Worker workerForGameplayEvent)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		Orientation orientation = (component != null) ? component.GetOrientation() : Orientation.Neutral;
		int cell = Grid.PosToCell(base.transform.GetLocalPosition());
		this.UnmarkArea();
		GameObject gameObject = this.building.Def.Build(cell, orientation, this.storage, this.selectedElementsTags, this.initialTemperature, base.GetComponent<BuildingFacade>().CurrentFacade, true, GameClock.Instance.GetTime());
		BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
		gameplayEventData.building = gameObject.GetComponent<BuildingComplete>();
		gameplayEventData.workable = this;
		gameplayEventData.worker = workerForGameplayEvent;
		gameplayEventData.eventTrigger = GameHashes.NewBuilding;
		GameplayEventManager.Instance.Trigger(-1661515756, gameplayEventData);
		gameObject.transform.rotation = base.transform.rotation;
		Rotatable component2 = gameObject.GetComponent<Rotatable>();
		if (component2 != null)
		{
			component2.SetOrientation(orientation);
		}
		KAnimGraphTileVisualizer component3 = base.GetComponent<KAnimGraphTileVisualizer>();
		if (component3 != null)
		{
			gameObject.GetComponent<KAnimGraphTileVisualizer>().Connections = connections;
			component3.skipCleanup = true;
		}
		KSelectable component4 = base.GetComponent<KSelectable>();
		if (component4 != null && component4.IsSelected && gameObject.GetComponent<KSelectable>() != null)
		{
			component4.Unselect();
			if (PlayerController.Instance.ActiveTool.name == "SelectTool")
			{
				((SelectTool)PlayerController.Instance.ActiveTool).SelectNextFrame(gameObject.GetComponent<KSelectable>(), false);
			}
		}
		gameObject.Trigger(2121280625, this);
		this.storage.ConsumeAllIgnoringDisease();
		this.finished = true;
		this.DeleteObject();
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x0008C034 File Offset: 0x0008A234
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.invalidLocation = new Notification(MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.INVALIDCONSTRUCTIONLOCATION.TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		this.faceTargetWhenWorking = true;
		base.Subscribe<Constructable>(-1432940121, Constructable.OnReachableChangedDelegate);
		if (this.rotatable == null)
		{
			this.MarkArea();
		}
		if (Db.Get().TechItems.GetTechTierForItem(this.building.Def.PrefabID) > 2)
		{
			this.requireMinionToWork = true;
		}
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Building;
		this.workingStatusItem = null;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		Prioritizable.AddRef(base.gameObject);
		this.synchronizeAnims = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x0008C18C File Offset: 0x0008A38C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		if (this.building.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
		}
		CellOffset[] array = this.building.Def.PlacementOffsets;
		if (this.rotatable != null)
		{
			array = new CellOffset[this.building.Def.PlacementOffsets.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.rotatable.GetRotatedCellOffset(this.building.Def.PlacementOffsets[i]);
			}
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(array, table, this.building.Def.ConstructionOffsetFilter);
		base.SetOffsetTable(offsetTable);
		this.storage.SetOffsetTable(offsetTable);
		base.Subscribe<Constructable>(2127324410, Constructable.OnCancelDelegate);
		if (this.rotatable != null)
		{
			this.MarkArea();
		}
		this.fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.BuildFetch);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Element element = ElementLoader.GetElement(this.SelectedElementsTags[0]);
		global::Debug.Assert(element != null, "Missing primary element for Constructable");
		component.ElementID = element.id;
		component.Temperature = (component.Temperature = 293.15f);
		foreach (Recipe.Ingredient ingredient in this.Recipe.GetAllIngredients(this.selectedElementsTags))
		{
			this.fetchList.Add(ingredient.tag, null, ingredient.amount, Operational.State.None);
			MaterialNeeds.UpdateNeed(ingredient.tag, ingredient.amount, base.gameObject.GetMyWorldId());
		}
		if (!this.building.Def.IsTilePiece)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Construction");
		}
		this.building.RunOnArea(delegate(int offset_cell)
		{
			if (base.gameObject.GetComponent<ConduitBridge>() == null)
			{
				GameObject gameObject2 = Grid.Objects[offset_cell, 7];
				if (gameObject2 != null)
				{
					gameObject2.DeleteObject();
				}
			}
		});
		if (this.IsReplacementTile && this.building.Def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			GameObject x = Grid.Objects[cell, (int)this.building.Def.ReplacementLayer];
			if (x == null || x == base.gameObject)
			{
				Grid.Objects[cell, (int)this.building.Def.ReplacementLayer] = base.gameObject;
				if (base.gameObject.GetComponent<SimCellOccupier>() != null)
				{
					int renderLayer = LayerMask.NameToLayer("Overlay");
					World.Instance.blockTileRenderer.AddBlock(renderLayer, this.building.Def, this.IsReplacementTile, SimHashes.Void, cell);
				}
				TileVisualizer.RefreshCell(cell, this.building.Def.TileLayer, this.building.Def.ReplacementLayer);
			}
			else
			{
				global::Debug.LogError("multiple replacement tiles on the same cell!");
				Util.KDestroyGameObject(base.gameObject);
			}
			GameObject gameObject = Grid.Objects[cell, (int)this.building.Def.ObjectLayer];
			if (gameObject != null)
			{
				Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
				if (component2 != null)
				{
					component2.CancelDeconstruction();
				}
			}
		}
		bool flag = this.building.Def.BuildingComplete.GetComponent<Ladder>();
		this.waitForFetchesBeforeDigging = (flag || this.building.Def.BuildingComplete.GetComponent<SimCellOccupier>() || this.building.Def.BuildingComplete.GetComponent<Door>() || this.building.Def.BuildingComplete.GetComponent<LiquidPumpingStation>());
		if (flag)
		{
			int x2 = 0;
			int num = 0;
			Grid.CellToXY(Grid.PosToCell(this), out x2, out num);
			int y = num - 3;
			this.ladderDetectionExtents = new Extents(x2, y, 1, 5);
			this.ladderParititonerEntry = GameScenePartitioner.Instance.Add("Constructable.OnNearbyBuildingLayerChanged", base.gameObject, this.ladderDetectionExtents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnNearbyBuildingLayerChanged));
			this.OnNearbyBuildingLayerChanged(null);
		}
		this.fetchList.Submit(new System.Action(this.OnFetchListComplete), true);
		this.PlaceDiggables();
		new ReachabilityMonitor.Instance(this).StartSM();
		base.Subscribe<Constructable>(493375141, Constructable.OnRefreshUserMenuDelegate);
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		Prioritizable prioritizable = component3;
		prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		this.OnPriorityChanged(component3.GetMasterPriority());
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x0008C644 File Offset: 0x0008A844
	private void OnPriorityChanged(PrioritySetting priority)
	{
		this.building.RunOnArea(delegate(int cell)
		{
			Diggable diggable = Diggable.GetDiggable(cell);
			if (diggable != null)
			{
				diggable.GetComponent<Prioritizable>().SetMasterPriority(priority);
			}
		});
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x0008C678 File Offset: 0x0008A878
	private void MarkArea()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		BuildingDef def = this.building.Def;
		Orientation orientation = this.building.Orientation;
		ObjectLayer layer = this.IsReplacementTile ? def.ReplacementLayer : def.ObjectLayer;
		def.MarkArea(num, orientation, layer, base.gameObject);
		if (def.IsTilePiece)
		{
			if (Grid.Objects[num, (int)def.TileLayer] == null)
			{
				def.MarkArea(num, orientation, def.TileLayer, base.gameObject);
				def.RunOnArea(num, orientation, delegate(int c)
				{
					TileVisualizer.RefreshCell(c, def.TileLayer, def.ReplacementLayer);
				});
			}
			Grid.IsTileUnderConstruction[num] = true;
		}
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0008C75C File Offset: 0x0008A95C
	private void UnmarkArea()
	{
		if (this.unmarked)
		{
			return;
		}
		this.unmarked = true;
		int num = Grid.PosToCell(base.transform.GetPosition());
		BuildingDef def = this.building.Def;
		ObjectLayer layer = this.IsReplacementTile ? this.building.Def.ReplacementLayer : this.building.Def.ObjectLayer;
		def.UnmarkArea(num, this.building.Orientation, layer, base.gameObject);
		if (def.IsTilePiece)
		{
			Grid.IsTileUnderConstruction[num] = false;
		}
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x0008C7EC File Offset: 0x0008A9EC
	private void OnNearbyBuildingLayerChanged(object data)
	{
		this.hasLadderNearby = false;
		for (int i = this.ladderDetectionExtents.y; i < this.ladderDetectionExtents.y + this.ladderDetectionExtents.height; i++)
		{
			int num = Grid.OffsetCell(0, this.ladderDetectionExtents.x, i);
			if (Grid.IsValidCell(num))
			{
				GameObject gameObject = null;
				Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
				if (gameObject != null && gameObject.GetComponent<Ladder>() != null)
				{
					this.hasLadderNearby = true;
					return;
				}
			}
		}
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x0008C878 File Offset: 0x0008AA78
	private bool IsWire()
	{
		return this.building.Def.name.Contains("Wire");
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x0008C894 File Offset: 0x0008AA94
	public bool IconConnectionAnimation(float delay, int connectionCount, string defName, string soundName)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (this.building.Def.Name.Contains(defName))
		{
			Building building = null;
			GameObject gameObject = Grid.Objects[num, 1];
			if (gameObject != null)
			{
				building = gameObject.GetComponent<Building>();
			}
			if (building != null)
			{
				bool flag = this.IsWire();
				int num2 = flag ? building.GetPowerInputCell() : building.GetUtilityInputCell();
				int num3 = flag ? num2 : building.GetUtilityOutputCell();
				if (num == num2 || num == num3)
				{
					BuildingCellVisualizer component = building.gameObject.GetComponent<BuildingCellVisualizer>();
					if (component != null && (flag ? component.RequiresPower : component.RequiresUtilityConnection))
					{
						component.ConnectedEventWithDelay(delay, connectionCount, num, soundName);
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x0008C964 File Offset: 0x0008AB64
	protected override void OnCleanUp()
	{
		if (this.IsReplacementTile && this.building.Def.isKAnimTile)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			GameObject gameObject = Grid.Objects[cell, (int)this.building.Def.ReplacementLayer];
			if (gameObject == base.gameObject && gameObject.GetComponent<SimCellOccupier>() != null)
			{
				World.Instance.blockTileRenderer.RemoveBlock(this.building.Def, this.IsReplacementTile, SimHashes.Void, cell);
			}
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.digPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.ladderParititonerEntry);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (component != null)
		{
			SaveLoader.Instance.saveManager.Unregister(component);
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Constructable destroyed");
		}
		this.UnmarkArea();
		int[] placementCells = this.building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			Diggable diggable = Diggable.GetDiggable(placementCells[i]);
			if (diggable != null)
			{
				diggable.gameObject.DeleteObject();
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x0008CAB0 File Offset: 0x0008ACB0
	private void OnDiggableReachabilityChanged(object data)
	{
		if (!this.IsReplacementTile)
		{
			int diggable_count = 0;
			int unreachable_count = 0;
			this.building.RunOnArea(delegate(int offset_cell)
			{
				Diggable diggable = Diggable.GetDiggable(offset_cell);
				if (diggable != null)
				{
					int num = diggable_count + 1;
					diggable_count = num;
					if (!diggable.GetComponent<KPrefabID>().HasTag(GameTags.Reachable))
					{
						num = unreachable_count + 1;
						unreachable_count = num;
					}
				}
			});
			bool flag = unreachable_count > 0 && unreachable_count == diggable_count;
			if (flag != this.hasUnreachableDigs)
			{
				if (flag)
				{
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable, null);
				}
				else
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructableDigUnreachable, false);
				}
				this.hasUnreachableDigs = flag;
			}
		}
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x0008CB5C File Offset: 0x0008AD5C
	private void PlaceDiggables()
	{
		if (this.waitForFetchesBeforeDigging && this.fetchList != null && !this.hasLadderNearby)
		{
			this.OnDiggableReachabilityChanged(null);
			return;
		}
		bool digs_complete = true;
		if (!this.solidPartitionerEntry.IsValid())
		{
			Extents validPlacementExtents = this.building.GetValidPlacementExtents();
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnFetchListComplete", base.gameObject, validPlacementExtents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChangedOrDigDestroyed));
			this.digPartitionerEntry = GameScenePartitioner.Instance.Add("Constructable.OnFetchListComplete", base.gameObject, validPlacementExtents, GameScenePartitioner.Instance.digDestroyedLayer, new Action<object>(this.OnSolidChangedOrDigDestroyed));
		}
		if (!this.IsReplacementTile)
		{
			this.building.RunOnArea(delegate(int offset_cell)
			{
				PrioritySetting masterPriority = this.GetComponent<Prioritizable>().GetMasterPriority();
				if (Diggable.IsDiggable(offset_cell))
				{
					digs_complete = false;
					Diggable diggable = Diggable.GetDiggable(offset_cell);
					if (diggable == null)
					{
						diggable = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), Grid.SceneLayer.Move, null, 0).GetComponent<Diggable>();
						diggable.gameObject.SetActive(true);
						diggable.transform.SetPosition(Grid.CellToPosCBC(offset_cell, Grid.SceneLayer.Move));
						diggable.Subscribe(-1432940121, new Action<object>(this.OnDiggableReachabilityChanged));
						Grid.Objects[offset_cell, 7] = diggable.gameObject;
					}
					else
					{
						diggable.Unsubscribe(-1432940121, new Action<object>(this.OnDiggableReachabilityChanged));
						diggable.Subscribe(-1432940121, new Action<object>(this.OnDiggableReachabilityChanged));
					}
					diggable.choreTypeIdHash = Db.Get().ChoreTypes.BuildDig.IdHash;
					diggable.GetComponent<Prioritizable>().SetMasterPriority(masterPriority);
					RenderUtil.EnableRenderer(diggable.transform, false);
					SaveLoadRoot component = diggable.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
			});
			this.OnDiggableReachabilityChanged(null);
		}
		bool flag = this.building.Def.IsValidBuildLocation(base.gameObject, base.transform.GetPosition(), this.building.Orientation, this.IsReplacementTile);
		if (flag)
		{
			this.notifier.Remove(this.invalidLocation);
		}
		else
		{
			this.notifier.Add(this.invalidLocation, "");
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidBuildingLocation, !flag, this);
		bool flag2 = digs_complete && flag && this.fetchList == null;
		if (flag2 && this.buildChore == null)
		{
			this.buildChore = new WorkChore<Constructable>(Db.Get().ChoreTypes.Build, this, null, true, new Action<Chore>(this.UpdateBuildState), new Action<Chore>(this.UpdateBuildState), new Action<Chore>(this.UpdateBuildState), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.UpdateBuildState(this.buildChore);
			return;
		}
		if (!flag2 && this.buildChore != null)
		{
			this.buildChore.Cancel("Need to dig");
			this.buildChore = null;
		}
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x0008CD5D File Offset: 0x0008AF5D
	private void OnFetchListComplete()
	{
		this.fetchList = null;
		this.PlaceDiggables();
		this.ClearMaterialNeeds();
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x0008CD74 File Offset: 0x0008AF74
	private void ClearMaterialNeeds()
	{
		if (this.materialNeedsCleared)
		{
			return;
		}
		foreach (Recipe.Ingredient ingredient in this.Recipe.GetAllIngredients(this.SelectedElementsTags))
		{
			MaterialNeeds.UpdateNeed(ingredient.tag, -ingredient.amount, base.gameObject.GetMyWorldId());
		}
		this.materialNeedsCleared = true;
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x0008CDD2 File Offset: 0x0008AFD2
	private void OnSolidChangedOrDigDestroyed(object data)
	{
		if (this == null || this.finished)
		{
			return;
		}
		this.PlaceDiggables();
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x0008CDEC File Offset: 0x0008AFEC
	private void UpdateBuildState(Chore chore)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (chore.InProgress())
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.UnderConstruction, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.UnderConstructionNoWorker, null);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x0008CE58 File Offset: 0x0008B058
	[OnDeserialized]
	internal void OnDeserialized()
	{
		if (this.ids != null)
		{
			this.selectedElements = new Element[this.ids.Length];
			for (int i = 0; i < this.ids.Length; i++)
			{
				this.selectedElements[i] = ElementLoader.FindElementByHash((SimHashes)this.ids[i]);
			}
			if (this.selectedElementsTags == null)
			{
				this.selectedElementsTags = new Tag[this.ids.Length];
				for (int j = 0; j < this.ids.Length; j++)
				{
					this.selectedElementsTags[j] = ElementLoader.FindElementByHash((SimHashes)this.ids[j]).tag;
				}
			}
			global::Debug.Assert(this.selectedElements.Length == this.selectedElementsTags.Length);
			for (int k = 0; k < this.selectedElements.Length; k++)
			{
				global::Debug.Assert(this.selectedElements[k].tag == this.SelectedElementsTags[k]);
			}
		}
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x0008CF44 File Offset: 0x0008B144
	private void OnReachableChanged(object data)
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if ((bool)data)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable, false);
			if (component != null)
			{
				component.TintColour = Game.Instance.uiColours.Build.validLocation;
				return;
			}
		}
		else
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ConstructionUnreachable, this);
			if (component != null)
			{
				component.TintColour = Game.Instance.uiColours.Build.unreachable;
			}
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x0008CFEC File Offset: 0x0008B1EC
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_cancel", UI.USERMENUACTIONS.CANCELCONSTRUCTION.NAME, new System.Action(this.OnPressCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCONSTRUCTION.TOOLTIP, true), 1f);
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x0008D046 File Offset: 0x0008B246
	private void OnPressCancel()
	{
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x0008D059 File Offset: 0x0008B259
	private void OnCancel(object data = null)
	{
		DetailsScreen.Instance.Show(false);
		this.ClearMaterialNeeds();
	}

	// Token: 0x04000E96 RID: 3734
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04000E97 RID: 3735
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04000E98 RID: 3736
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04000E99 RID: 3737
	[MyCmpReq]
	private Building building;

	// Token: 0x04000E9A RID: 3738
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04000E9B RID: 3739
	private Notification invalidLocation;

	// Token: 0x04000E9C RID: 3740
	private float initialTemperature = -1f;

	// Token: 0x04000E9D RID: 3741
	[Serialize]
	private bool isPrioritized;

	// Token: 0x04000E9E RID: 3742
	private FetchList2 fetchList;

	// Token: 0x04000E9F RID: 3743
	private Chore buildChore;

	// Token: 0x04000EA0 RID: 3744
	private bool materialNeedsCleared;

	// Token: 0x04000EA1 RID: 3745
	private bool hasUnreachableDigs;

	// Token: 0x04000EA2 RID: 3746
	private bool finished;

	// Token: 0x04000EA3 RID: 3747
	private bool unmarked;

	// Token: 0x04000EA4 RID: 3748
	public bool isDiggingRequired = true;

	// Token: 0x04000EA5 RID: 3749
	private bool waitForFetchesBeforeDigging;

	// Token: 0x04000EA6 RID: 3750
	private bool hasLadderNearby;

	// Token: 0x04000EA7 RID: 3751
	private Extents ladderDetectionExtents;

	// Token: 0x04000EA8 RID: 3752
	[Serialize]
	public bool IsReplacementTile;

	// Token: 0x04000EA9 RID: 3753
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04000EAA RID: 3754
	private HandleVector<int>.Handle digPartitionerEntry;

	// Token: 0x04000EAB RID: 3755
	private HandleVector<int>.Handle ladderParititonerEntry;

	// Token: 0x04000EAC RID: 3756
	private LoggerFSS log = new LoggerFSS("Constructable", 35);

	// Token: 0x04000EAD RID: 3757
	[Serialize]
	private Tag[] selectedElementsTags;

	// Token: 0x04000EAE RID: 3758
	private Element[] selectedElements;

	// Token: 0x04000EAF RID: 3759
	[Serialize]
	private int[] ids;

	// Token: 0x04000EB0 RID: 3760
	private static readonly EventSystem.IntraObjectHandler<Constructable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04000EB1 RID: 3761
	private static readonly EventSystem.IntraObjectHandler<Constructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04000EB2 RID: 3762
	private static readonly EventSystem.IntraObjectHandler<Constructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Constructable>(delegate(Constructable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Klei;
using Klei.AI;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x0200074C RID: 1868
[Serializable]
public class BuildingDef : Def
{
	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x060033A4 RID: 13220 RVA: 0x0011313F File Offset: 0x0011133F
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".NAME");
		}
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x060033A5 RID: 13221 RVA: 0x00113165 File Offset: 0x00111365
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".DESC");
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x060033A6 RID: 13222 RVA: 0x0011318B File Offset: 0x0011138B
	public string Flavor
	{
		get
		{
			return "\"" + Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".FLAVOR") + "\"";
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060033A7 RID: 13223 RVA: 0x001131C0 File Offset: 0x001113C0
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060033A8 RID: 13224 RVA: 0x001131E6 File Offset: 0x001113E6
	public bool IsTilePiece
	{
		get
		{
			return this.TileLayer != ObjectLayer.NumLayers;
		}
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x001131F5 File Offset: 0x001113F5
	public bool CanReplace(GameObject go)
	{
		return this.ReplacementTags != null && go.GetComponent<KPrefabID>().HasAnyTags(this.ReplacementTags);
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x00113212 File Offset: 0x00111412
	public bool IsAvailable()
	{
		return !this.Deprecated && (!this.DebugOnly || Game.Instance.DebugOnlyBuildingsAllowed);
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x00113232 File Offset: 0x00111432
	public bool ShouldShowInBuildMenu()
	{
		return this.ShowInBuildMenu;
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0011323C File Offset: 0x0011143C
	public bool IsReplacementLayerOccupied(int cell)
	{
		if (Grid.Objects[cell, (int)this.ReplacementLayer] != null)
		{
			return true;
		}
		if (this.EquivalentReplacementLayers != null)
		{
			foreach (ObjectLayer layer in this.EquivalentReplacementLayers)
			{
				if (Grid.Objects[cell, (int)layer] != null)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x001132C8 File Offset: 0x001114C8
	public GameObject GetReplacementCandidate(int cell)
	{
		if (this.ReplacementCandidateLayers != null)
		{
			using (List<ObjectLayer>.Enumerator enumerator = this.ReplacementCandidateLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ObjectLayer objectLayer = enumerator.Current;
					if (Grid.ObjectLayers[(int)objectLayer].ContainsKey(cell))
					{
						GameObject gameObject = Grid.ObjectLayers[(int)objectLayer][cell];
						if (gameObject != null && gameObject.GetComponent<BuildingComplete>() != null)
						{
							return gameObject;
						}
					}
				}
				goto IL_96;
			}
		}
		if (Grid.ObjectLayers[(int)this.TileLayer].ContainsKey(cell))
		{
			return Grid.ObjectLayers[(int)this.TileLayer][cell];
		}
		IL_96:
		return null;
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x00113380 File Offset: 0x00111580
	public GameObject Create(Vector3 pos, Storage resource_storage, IList<Tag> selected_elements, Recipe recipe, float temperature, GameObject obj)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		if (resource_storage != null)
		{
			Recipe.Ingredient[] allIngredients = recipe.GetAllIngredients(selected_elements);
			if (allIngredients != null)
			{
				foreach (Recipe.Ingredient ingredient in allIngredients)
				{
					float num;
					SimUtil.DiseaseInfo b;
					float num2;
					resource_storage.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out b, out num2);
					diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
				}
			}
		}
		GameObject gameObject = GameUtil.KInstantiate(obj, pos, this.SceneLayer, null, 0);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.ElementID = element.id;
		component.Temperature = temperature;
		component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "BuildingDef.Create");
		gameObject.name = obj.name;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x00113450 File Offset: 0x00111650
	public List<Tag> DefaultElements()
	{
		List<Tag> list = new List<Tag>();
		string[] materialCategory = this.MaterialCategory;
		for (int i = 0; i < materialCategory.Length; i++)
		{
			List<Tag> validMaterials = MaterialSelector.GetValidMaterials(materialCategory[i], false);
			if (validMaterials.Count != 0)
			{
				list.Add(validMaterials[0]);
			}
		}
		return list;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x001134A0 File Offset: 0x001116A0
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, string facadeID, bool playsound = true, float timeBuilt = -1f)
	{
		GameObject gameObject = this.Build(cell, orientation, resource_storage, selected_elements, temperature, playsound, timeBuilt);
		if (facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
		}
		return gameObject;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x001134EC File Offset: 0x001116EC
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, bool playsound = true, float timeBuilt = -1f)
	{
		Vector3 pos = Grid.CellToPosCBC(cell, this.SceneLayer);
		GameObject gameObject = this.Create(pos, resource_storage, selected_elements, this.CraftRecipe, temperature, this.BuildingComplete);
		Rotatable component = gameObject.GetComponent<Rotatable>();
		if (component != null)
		{
			component.SetOrientation(orientation);
		}
		this.MarkArea(cell, orientation, this.ObjectLayer, gameObject);
		if (this.IsTilePiece)
		{
			this.MarkArea(cell, orientation, this.TileLayer, gameObject);
			this.RunOnArea(cell, orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.TileLayer, this.ReplacementLayer);
			});
		}
		if (this.PlayConstructionSounds)
		{
			string sound = GlobalAssets.GetSound("Finish_Building_" + this.AudioSize, false);
			if (playsound && sound != null)
			{
				Vector3 position = gameObject.transform.GetPosition();
				position.z = 0f;
				KFMOD.PlayOneShot(sound, position, 1f);
			}
		}
		Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[selected_elements.Count];
			for (int i = 0; i < selected_elements.Count; i++)
			{
				component2.constructionElements[i] = selected_elements[i];
			}
		}
		BuildingComplete component3 = gameObject.GetComponent<BuildingComplete>();
		if (component3)
		{
			component3.SetCreationTime(timeBuilt);
		}
		Game.Instance.Trigger(-1661515756, gameObject);
		gameObject.Trigger(-1661515756, gameObject);
		return gameObject;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x00113644 File Offset: 0x00111844
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		GameObject result = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, pos, orientation, false, out text))
		{
			result = this.Instantiate(pos, orientation, selected_elements, layer);
		}
		return result;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x00113670 File Offset: 0x00111870
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		GameObject gameObject = this.TryPlace(src_go, pos, orientation, selected_elements, layer);
		if (gameObject != null && facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
			gameObject.GetComponent<KBatchedAnimController>().Play("place", KAnim.PlayMode.Once, 1f, 0f);
		}
		return gameObject;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x001136E0 File Offset: 0x001118E0
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		GameObject result = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, pos, orientation, true, out text))
		{
			Constructable component = this.BuildingUnderConstruction.GetComponent<Constructable>();
			component.IsReplacementTile = true;
			result = this.Instantiate(pos, orientation, selected_elements, layer);
			component.IsReplacementTile = false;
		}
		return result;
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x00113724 File Offset: 0x00111924
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		GameObject gameObject = this.TryReplaceTile(src_go, pos, orientation, selected_elements, layer);
		if (gameObject != null && facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
		}
		return gameObject;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x00113774 File Offset: 0x00111974
	public GameObject Instantiate(Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		float num = -0.15f;
		pos.z += num;
		GameObject gameObject = GameUtil.KInstantiate(this.BuildingUnderConstruction, pos, Grid.SceneLayer.Front, null, layer);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null, "Missing primary element for BuildingDef");
		gameObject.GetComponent<PrimaryElement>().ElementID = element.id;
		gameObject.GetComponent<Constructable>().SelectedElementsTags = selected_elements;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060033B7 RID: 13239 RVA: 0x001137E8 File Offset: 0x001119E8
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, out string fail_reason)
	{
		bool flag = true;
		fail_reason = null;
		int i = 0;
		while (i < this.PlacementOffsets.Length)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if ((int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				flag = false;
				break;
			}
			bool flag2 = this.BuildLocationRule == BuildLocationRule.LogicBridge || this.BuildLocationRule == BuildLocationRule.Conduit || this.BuildLocationRule == BuildLocationRule.WireBridge;
			GameObject x = null;
			if (replace_tile)
			{
				x = this.GetReplacementCandidate(num);
			}
			if (!flag2)
			{
				GameObject gameObject = Grid.Objects[num, (int)layer];
				bool flag3 = false;
				if (gameObject != null)
				{
					Building component = gameObject.GetComponent<Building>();
					if (component != null)
					{
						flag3 = (component.Def.BuildLocationRule == BuildLocationRule.LogicBridge || component.Def.BuildLocationRule == BuildLocationRule.Conduit || component.Def.BuildLocationRule == BuildLocationRule.WireBridge);
					}
				}
				if (!flag3)
				{
					if (gameObject != null && gameObject != source_go && (x == null || x != gameObject) && (gameObject.GetComponent<Wire>() == null || this.BuildingComplete.GetComponent<Wire>() == null))
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
					if (tile_layer != ObjectLayer.NumLayers && (x == null || x == source_go) && Grid.Objects[num, (int)tile_layer] != null && Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == null)
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
				}
			}
			if (layer == ObjectLayer.Building && this.AttachmentSlotTag != GameTags.Rocket && Grid.Objects[num, 39] != null)
			{
				if (this.BuildingComplete.GetComponent<Wire>() == null)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
					flag = false;
					break;
				}
				break;
			}
			else
			{
				if (layer == ObjectLayer.Gantry)
				{
					bool flag4 = false;
					MakeBaseSolid.Def def = source_go.GetDef<MakeBaseSolid.Def>();
					for (int j = 0; j < def.solidOffsets.Length; j++)
					{
						CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(def.solidOffsets[j], orientation);
						flag4 |= (rotatedCellOffset2 == rotatedCellOffset);
					}
					if (flag4 && !this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
					GameObject gameObject2 = Grid.Objects[num, 1];
					if (gameObject2 != null && gameObject2.GetComponent<BuildingPreview>() == null)
					{
						Building component2 = gameObject2.GetComponent<Building>();
						if (flag4 || component2 == null || component2.Def.AttachmentSlotTag != GameTags.Rocket)
						{
							fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
							flag = false;
							break;
						}
					}
				}
				if (this.BuildLocationRule == BuildLocationRule.Tile)
				{
					if (!this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
				}
				else if (this.BuildLocationRule == BuildLocationRule.OnFloorOverSpace && global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) != SubWorld.ZoneType.Space)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
					flag = false;
					break;
				}
				i++;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (layer == ObjectLayer.LiquidConduit)
		{
			GameObject gameObject3 = Grid.Objects[cell, 19];
			if (gameObject3 != null)
			{
				Building component3 = gameObject3.GetComponent<Building>();
				if (component3 != null && component3.Def.BuildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin && component3.GetCell() == cell)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN;
					return false;
				}
			}
		}
		BuildLocationRule buildLocationRule = this.BuildLocationRule;
		switch (buildLocationRule)
		{
		case BuildLocationRule.NotInTiles:
		{
			GameObject x2 = Grid.Objects[cell, 9];
			if (!replace_tile && x2 != null && x2 != source_go)
			{
				flag = false;
			}
			else if (Grid.HasDoor[cell])
			{
				flag = false;
			}
			else
			{
				GameObject gameObject4 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject4 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						if (gameObject4 != source_go)
						{
							flag = false;
						}
					}
					else
					{
						Building component4 = gameObject4.GetComponent<Building>();
						if (component4 != null && component4.Def.ReplacementLayer != this.ReplacementLayer)
						{
							flag = false;
						}
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			}
			break;
		}
		case BuildLocationRule.Conduit:
		case BuildLocationRule.LogicBridge:
			break;
		case BuildLocationRule.WireBridge:
			return this.IsValidWireBridgeLocation(source_go, cell, orientation, out fail_reason);
		case BuildLocationRule.HighWattBridgeTile:
			flag = (this.IsValidTileLocation(source_go, cell, replace_tile, ref fail_reason) && this.IsValidHighWattBridgeLocation(source_go, cell, orientation, out fail_reason));
			break;
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num2 = 0;
			while (num2 < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int k = 0; k < Components.BuildingAttachPoints[num2].points.Length; k++)
				{
					if (Components.BuildingAttachPoints[num2].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num2++;
			}
			if (!flag)
			{
				fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			}
			break;
		}
		default:
			if (buildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin)
			{
				flag = (Grid.Objects[cell, 16] == null && (Grid.Objects[cell, 19] == null || Grid.Objects[cell, 19] == source_go));
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		flag = (flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x00113DF0 File Offset: 0x00111FF0
	private bool IsValidTileLocation(GameObject source_go, int cell, bool replacement_tile, ref string fail_reason)
	{
		GameObject gameObject = Grid.Objects[cell, 27];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.NotInTiles)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 29];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 2];
		if (gameObject != null && gameObject != source_go)
		{
			Building component = gameObject.GetComponent<Building>();
			if (!replacement_tile && component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL;
				return false;
			}
		}
		return true;
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x00113ED4 File Offset: 0x001120D4
	public void RunOnArea(int cell, Orientation orientation, Action<int> callback)
	{
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int obj = Grid.OffsetCell(cell, rotatedCellOffset);
			callback(obj);
		}
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x00113F18 File Offset: 0x00112118
	public void MarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (this.BuildLocationRule != BuildLocationRule.Conduit && this.BuildLocationRule != BuildLocationRule.WireBridge && this.BuildLocationRule != BuildLocationRule.LogicBridge)
		{
			for (int i = 0; i < this.PlacementOffsets.Length; i++)
			{
				CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
				int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
				Grid.Objects[cell2, (int)layer] = go;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell3, (int)objectLayerForConduitType], go);
			Grid.Objects[cell3, (int)objectLayerForConduitType] = go;
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell4, (int)objectLayerForConduitType2], go);
			Grid.Objects[cell4, (int)objectLayerForConduitType2] = go;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			this.MarkOverlappingPorts(Grid.Objects[cell5, 29], go);
			Grid.Objects[cell5, 29] = go;
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			this.MarkOverlappingPorts(Grid.Objects[cell6, 29], go);
			Grid.Objects[cell6, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.WireBridge || this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			this.MarkOverlappingPorts(Grid.Objects[cell7, 29], go);
			this.MarkOverlappingPorts(Grid.Objects[cell8, 29], go);
			Grid.Objects[cell7, 29] = go;
			Grid.Objects[cell8, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.LogicBridge)
		{
			LogicPorts component = go.GetComponent<LogicPorts>();
			if (component != null && component.inputPortInfo != null)
			{
				LogicPorts.Port[] inputPortInfo = component.inputPortInfo;
				for (int j = 0; j < inputPortInfo.Length; j++)
				{
					CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(inputPortInfo[j].cellOffset, orientation);
					int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
					this.MarkOverlappingLogicPorts(Grid.Objects[cell9, (int)layer], go, cell9);
					Grid.Objects[cell9, (int)layer] = go;
				}
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						this.MarkOverlappingPorts(Grid.Objects[cell10, (int)objectLayerForConduitType3], go);
						Grid.Objects[cell10, (int)objectLayerForConduitType3] = go;
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset8 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell11 = Grid.OffsetCell(cell, rotatedCellOffset8);
						this.MarkOverlappingPorts(Grid.Objects[cell11, (int)objectLayerForConduitType4], go);
						Grid.Objects[cell11, (int)objectLayerForConduitType4] = go;
					}
				}
			}
		}
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x001142EA File Offset: 0x001124EA
	public void MarkOverlappingPorts(GameObject existing, GameObject replaced)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			existing.AddTag(GameTags.HasInvalidPorts);
		}
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x00114320 File Offset: 0x00112520
	public void MarkOverlappingLogicPorts(GameObject existing, GameObject replaced, int cell)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			LogicGate component = existing.GetComponent<LogicGate>();
			LogicPorts component2 = existing.GetComponent<LogicPorts>();
			LogicPorts.Port port;
			bool flag;
			LogicGateBase.PortId portId;
			if ((component2 != null && component2.TryGetPortAtCell(cell, out port, out flag)) || (component != null && component.TryGetPortAtCell(cell, out portId)))
			{
				existing.AddTag(GameTags.HasInvalidPorts);
			}
		}
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x00114398 File Offset: 0x00112598
	public void UnmarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (cell == Grid.InvalidCell)
		{
			return;
		}
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (Grid.Objects[cell2, (int)layer] == go)
			{
				Grid.Objects[cell2, (int)layer] = null;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			if (Grid.Objects[cell3, (int)objectLayerForConduitType] == go)
			{
				Grid.Objects[cell3, (int)objectLayerForConduitType] = null;
			}
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			if (Grid.Objects[cell4, (int)objectLayerForConduitType2] == go)
			{
				Grid.Objects[cell4, (int)objectLayerForConduitType2] = null;
			}
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			if (Grid.Objects[cell5, 29] == go)
			{
				Grid.Objects[cell5, 29] = null;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			if (Grid.Objects[cell6, 29] == go)
			{
				Grid.Objects[cell6, 29] = null;
			}
		}
		if (this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			if (Grid.Objects[cell7, 29] == go)
			{
				Grid.Objects[cell7, 29] = null;
			}
			if (Grid.Objects[cell8, 29] == go)
			{
				Grid.Objects[cell8, 29] = null;
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
						if (Grid.Objects[cell9, (int)objectLayerForConduitType3] == go)
						{
							Grid.Objects[cell9, (int)objectLayerForConduitType3] = null;
						}
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						if (Grid.Objects[cell10, (int)objectLayerForConduitType4] == go)
						{
							Grid.Objects[cell10, (int)objectLayerForConduitType4] = null;
						}
					}
				}
			}
		}
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x001146D9 File Offset: 0x001128D9
	public int GetBuildingCell(int cell)
	{
		return cell + (this.WidthInCells - 1) / 2;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x001146E7 File Offset: 0x001128E7
	public Vector3 GetVisualizerOffset()
	{
		return Vector3.right * (0.5f * (float)((this.WidthInCells + 1) % 2));
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x00114704 File Offset: 0x00112904
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x00114724 File Offset: 0x00112924
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason);
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x00114745 File Offset: 0x00112945
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x00114754 File Offset: 0x00112954
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if ((int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (this.BuildLocationRule == BuildLocationRule.OnRocketEnvelope)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.OnWall)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.InCorner)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.WallFloor)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.BelowRocketCeiling)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
			if ((float)(Grid.CellToXY(cell).y + 35 + source_go.GetComponent<Building>().Def.HeightInCells) >= world.maximumBounds.y - (float)Grid.TopBorderHeight)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BELOWROCKETCEILING;
				return false;
			}
		}
		return this.IsAreaClear(source_go, cell, orientation, this.ObjectLayer, this.TileLayer, replace_tile, out fail_reason);
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x0011490C File Offset: 0x00112B0C
	public bool IsValidReplaceLocation(Vector3 pos, Orientation orientation, ObjectLayer replace_layer, ObjectLayer obj_layer)
	{
		if (replace_layer == ObjectLayer.NumLayers)
		{
			return false;
		}
		bool result = true;
		int cell = Grid.PosToCell(pos);
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(cell2))
			{
				return false;
			}
			if (Grid.Objects[cell2, (int)obj_layer] == null || Grid.Objects[cell2, (int)replace_layer] != null)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x00114994 File Offset: 0x00112B94
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile = false)
	{
		string text = "";
		return this.IsValidBuildLocation(source_go, pos, orientation, out text, replace_tile);
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x001149B4 File Offset: 0x00112BB4
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string reason, bool replace_tile = false)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidBuildLocation(source_go, cell, orientation, replace_tile, out reason);
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x001149D8 File Offset: 0x00112BD8
	public bool IsValidBuildLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (!this.IsAreaValid(cell, orientation, out fail_reason))
		{
			return false;
		}
		bool flag = true;
		fail_reason = null;
		switch (this.BuildLocationRule)
		{
		case BuildLocationRule.Anywhere:
		case BuildLocationRule.Conduit:
		case BuildLocationRule.OnFloorOrBuildingAttachPoint:
			flag = true;
			break;
		case BuildLocationRule.OnFloor:
		case BuildLocationRule.OnCeiling:
		case BuildLocationRule.OnFoundationRotatable:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			break;
		case BuildLocationRule.OnFloorOverSpace:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			else if (!BuildingDef.AreAllCellsValid(cell, orientation, this.WidthInCells, this.HeightInCells, (int check_cell) => global::World.Instance.zoneRenderData.GetSubWorldZoneType(check_cell) == SubWorld.ZoneType.Space))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
			}
			break;
		case BuildLocationRule.OnWall:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
			}
			break;
		case BuildLocationRule.InCorner:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
			}
			break;
		case BuildLocationRule.Tile:
		{
			flag = true;
			GameObject gameObject = Grid.Objects[cell, 27];
			if (gameObject != null)
			{
				Building component = gameObject.GetComponent<Building>();
				if (component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = false;
				}
			}
			gameObject = Grid.Objects[cell, 2];
			if (gameObject != null)
			{
				Building component2 = gameObject.GetComponent<Building>();
				if (component2 != null && component2.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = replace_tile;
				}
			}
			break;
		}
		case BuildLocationRule.NotInTiles:
		{
			GameObject x = Grid.Objects[cell, 9];
			flag = (replace_tile || x == null || x == source_go);
			flag = (flag && !Grid.HasDoor[cell]);
			if (flag)
			{
				GameObject gameObject2 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject2 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						flag = (flag && (gameObject2 == null || gameObject2 == source_go));
					}
					else
					{
						Building component3 = gameObject2.GetComponent<Building>();
						flag = (component3 == null || component3.Def.ReplacementLayer == this.ReplacementLayer);
					}
				}
			}
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			break;
		}
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num = 0;
			while (num < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int i = 0; i < Components.BuildingAttachPoints[num].points.Length; i++)
				{
					if (Components.BuildingAttachPoints[num].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num++;
			}
			fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			break;
		}
		case BuildLocationRule.OnRocketEnvelope:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
			}
			break;
		case BuildLocationRule.WallFloor:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason);
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x00114E0C File Offset: 0x0011300C
	private bool IsAreaValid(int cell, Orientation orientation, out string fail_reason)
	{
		bool result = true;
		fail_reason = null;
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x00114E98 File Offset: 0x00113098
	private bool ArePowerPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			GameObject x = Grid.Objects[cell2, 29];
			if (x != null && x != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			GameObject x2 = Grid.Objects[cell3, 29];
			if (x2 != null && x2 != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		return true;
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x00114F54 File Offset: 0x00113154
	private bool AreConduitPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		bool flag = true;
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
			flag = this.IsValidConduitConnection(source_go, this.InputConduitType, utility_cell, ref fail_reason);
		}
		if (flag && this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int utility_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
			flag = this.IsValidConduitConnection(source_go, this.OutputConduitType, utility_cell2, ref fail_reason);
		}
		Building component = source_go.GetComponent<Building>();
		if (flag && component)
		{
			ISecondaryInput[] components = component.Def.BuildingComplete.GetComponents<ISecondaryInput>();
			if (components != null)
			{
				foreach (ISecondaryInput secondaryInput in components)
				{
					for (int j = 0; j < 4; j++)
					{
						ConduitType conduitType = (ConduitType)j;
						if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
						{
							CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
							int utility_cell3 = Grid.OffsetCell(cell, rotatedCellOffset3);
							flag = this.IsValidConduitConnection(source_go, conduitType, utility_cell3, ref fail_reason);
						}
					}
				}
			}
		}
		if (flag)
		{
			ISecondaryOutput[] components2 = component.Def.BuildingComplete.GetComponents<ISecondaryOutput>();
			if (components2 != null)
			{
				foreach (ISecondaryOutput secondaryOutput in components2)
				{
					for (int k = 0; k < 4; k++)
					{
						ConduitType conduitType2 = (ConduitType)k;
						if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
						{
							CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
							int utility_cell4 = Grid.OffsetCell(cell, rotatedCellOffset4);
							flag = this.IsValidConduitConnection(source_go, conduitType2, utility_cell4, ref fail_reason);
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x001150F4 File Offset: 0x001132F4
	private bool IsValidWireBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			int cell2;
			int cell3;
			component.GetCells(out cell2, out cell3);
			if (Grid.Objects[cell2, 29] != null || Grid.Objects[cell3, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x0011515C File Offset: 0x0011335C
	private bool IsValidHighWattBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			if (!component.AreCellsValid(cell, orientation))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			int num;
			int num2;
			component.GetCells(out num, out num2);
			if (Grid.Objects[num, 29] != null || Grid.Objects[num2, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
			if (Grid.Objects[num, 9] != null || Grid.Objects[num2, 9] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			if (Grid.HasDoor[num] || Grid.HasDoor[num2])
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			GameObject gameObject = Grid.Objects[num, 1];
			GameObject gameObject2 = Grid.Objects[num2, 1];
			if (gameObject != null || gameObject2 != null)
			{
				BuildingUnderConstruction buildingUnderConstruction = gameObject ? gameObject.GetComponent<BuildingUnderConstruction>() : null;
				BuildingUnderConstruction buildingUnderConstruction2 = gameObject2 ? gameObject2.GetComponent<BuildingUnderConstruction>() : null;
				if ((buildingUnderConstruction && buildingUnderConstruction.Def.BuildingComplete.GetComponent<Door>()) || (buildingUnderConstruction2 && buildingUnderConstruction2.Def.BuildingComplete.GetComponent<Door>()))
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
					return false;
				}
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x001152E8 File Offset: 0x001134E8
	private bool AreLogicPortsInValidPositions(GameObject source_go, int cell, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		ReadOnlyCollection<ILogicUIElement> visElements = Game.Instance.logicCircuitManager.GetVisElements();
		LogicPorts component = source_go.GetComponent<LogicPorts>();
		if (component != null)
		{
			component.HackRefreshVisualizers();
			if (this.DoLogicPortsConflict(component.inputPorts, visElements) || this.DoLogicPortsConflict(component.outputPorts, visElements))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		else
		{
			LogicGateBase component2 = source_go.GetComponent<LogicGateBase>();
			if (component2 != null && (this.IsLogicPortObstructed(component2.InputCellOne, visElements) || this.IsLogicPortObstructed(component2.OutputCellOne, visElements) || ((component2.RequiresTwoInputs || component2.RequiresFourInputs) && this.IsLogicPortObstructed(component2.InputCellTwo, visElements)) || (component2.RequiresFourInputs && (this.IsLogicPortObstructed(component2.InputCellThree, visElements) || this.IsLogicPortObstructed(component2.InputCellFour, visElements))) || (component2.RequiresFourOutputs && (this.IsLogicPortObstructed(component2.OutputCellTwo, visElements) || this.IsLogicPortObstructed(component2.OutputCellThree, visElements) || this.IsLogicPortObstructed(component2.OutputCellFour, visElements)))))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		return true;
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x00115418 File Offset: 0x00113618
	private bool DoLogicPortsConflict(IList<ILogicUIElement> ports_a, IList<ILogicUIElement> ports_b)
	{
		if (ports_a == null || ports_b == null)
		{
			return false;
		}
		foreach (ILogicUIElement logicUIElement in ports_a)
		{
			int logicUICell = logicUIElement.GetLogicUICell();
			foreach (ILogicUIElement logicUIElement2 in ports_b)
			{
				if (logicUIElement != logicUIElement2 && logicUICell == logicUIElement2.GetLogicUICell())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x001154B4 File Offset: 0x001136B4
	private bool IsLogicPortObstructed(int cell, IList<ILogicUIElement> ports)
	{
		int num = 0;
		using (IEnumerator<ILogicUIElement> enumerator = ports.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetLogicUICell() == cell)
				{
					num++;
				}
			}
		}
		return num > 0;
	}

	// Token: 0x060033D0 RID: 13264 RVA: 0x00115508 File Offset: 0x00113708
	private bool IsValidConduitConnection(GameObject source_go, ConduitType conduit_type, int utility_cell, ref string fail_reason)
	{
		bool result = true;
		switch (conduit_type)
		{
		case ConduitType.Gas:
		{
			GameObject x = Grid.Objects[utility_cell, 15];
			if (x != null && x != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_GASPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Liquid:
		{
			GameObject x2 = Grid.Objects[utility_cell, 19];
			if (x2 != null && x2 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Solid:
		{
			GameObject x3 = Grid.Objects[utility_cell, 23];
			if (x3 != null && x3 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP;
			}
			break;
		}
		}
		return result;
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x001155C2 File Offset: 0x001137C2
	public static int GetXOffset(int width)
	{
		return -(width - 1) / 2;
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x001155CC File Offset: 0x001137CC
	public static bool CheckFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		if (location_rule == BuildLocationRule.OnWall)
		{
			return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.InCorner)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.WallFloor)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnFloor, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		return BuildingDef.CheckBaseFoundation(cell, orientation, location_rule, width, height, optionalFoundationRequiredTag);
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x00115648 File Offset: 0x00113848
	public static bool CheckBaseFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		for (int i = num; i <= num2; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset((location_rule == BuildLocationRule.OnCeiling) ? new CellOffset(i, height) : new CellOffset(i, -1), orientation);
			int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num3) || !Grid.Solid[num3])
			{
				return false;
			}
			if (optionalFoundationRequiredTag.IsValid && (!Grid.ObjectLayers[9].ContainsKey(num3) || !Grid.ObjectLayers[9][num3].HasTag(optionalFoundationRequiredTag)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x001156D8 File Offset: 0x001138D8
	public static bool CheckWallFoundation(int cell, int width, int height, bool leftWall)
	{
		for (int i = 0; i < height; i++)
		{
			CellOffset offset = new CellOffset(leftWall ? (-(width - 1) / 2 - 1) : (width / 2 + 1), i);
			int num = Grid.OffsetCell(cell, offset);
			GameObject gameObject = Grid.Objects[num, 1];
			bool flag = false;
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && component.Def.IsFoundation)
				{
					flag = true;
				}
			}
			if (!Grid.IsValidBuildingCell(num) || (!Grid.Solid[num] && !flag))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x00115774 File Offset: 0x00113974
	public static bool AreAllCellsValid(int base_cell, Orientation orientation, int width, int height, Func<int, bool> valid_cell_check)
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		if (orientation == Orientation.FlipH)
		{
			int num3 = num;
			num = -num2;
			num2 = -num3;
		}
		for (int i = 0; i < height; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				int arg = Grid.OffsetCell(base_cell, j, i);
				if (!valid_cell_check(arg))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x001157C6 File Offset: 0x001139C6
	public Sprite GetUISprite(string animName = "ui", bool centered = false)
	{
		return Def.GetUISpriteFromMultiObjectAnim(this.AnimFiles[0], animName, centered, "");
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x001157DC File Offset: 0x001139DC
	public void GenerateOffsets()
	{
		this.GenerateOffsets(this.WidthInCells, this.HeightInCells);
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x001157F0 File Offset: 0x001139F0
	public void GenerateOffsets(int width, int height)
	{
		if (!BuildingDef.placementOffsetsCache.TryGetValue(new CellOffset(width, height), out this.PlacementOffsets))
		{
			int num = width / 2 - width + 1;
			this.PlacementOffsets = new CellOffset[width * height];
			for (int num2 = 0; num2 != height; num2++)
			{
				int num3 = num2 * width;
				for (int num4 = 0; num4 != width; num4++)
				{
					int num5 = num3 + num4;
					this.PlacementOffsets[num5].x = num4 + num;
					this.PlacementOffsets[num5].y = num2;
				}
			}
			BuildingDef.placementOffsetsCache.Add(new CellOffset(width, height), this.PlacementOffsets);
		}
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x0011588C File Offset: 0x00113A8C
	public void PostProcess()
	{
		this.CraftRecipe = new Recipe(this.BuildingComplete.PrefabID().Name, 1f, (SimHashes)0, this.Name, null, 0);
		this.CraftRecipe.Icon = this.UISprite;
		for (int i = 0; i < this.MaterialCategory.Length; i++)
		{
			Recipe.Ingredient item = new Recipe.Ingredient(this.MaterialCategory[i], (float)((int)this.Mass[i]));
			this.CraftRecipe.Ingredients.Add(item);
		}
		if (this.DecorBlockTileInfo != null)
		{
			this.DecorBlockTileInfo.PostProcess();
		}
		if (this.DecorPlaceBlockTileInfo != null)
		{
			this.DecorPlaceBlockTileInfo.PostProcess();
		}
		if (!this.Deprecated)
		{
			Db.Get().TechItems.AddTechItem(this.PrefabID, this.Name, this.Effect, new Func<string, bool, Sprite>(this.GetUISprite), this.RequiredDlcIds);
		}
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x00115980 File Offset: 0x00113B80
	public bool MaterialsAvailable(IList<Tag> selected_elements, WorldContainer world)
	{
		bool result = true;
		foreach (Recipe.Ingredient ingredient in this.CraftRecipe.GetAllIngredients(selected_elements))
		{
			if (world.worldInventory.GetAmount(ingredient.tag, true) < ingredient.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x001159D0 File Offset: 0x00113BD0
	public bool CheckRequiresBuildingCellVisualizer()
	{
		return this.CheckRequiresPowerInput() || this.CheckRequiresPowerOutput() || this.CheckRequiresGasInput() || this.CheckRequiresGasOutput() || this.CheckRequiresLiquidInput() || this.CheckRequiresLiquidOutput() || this.CheckRequiresSolidInput() || this.CheckRequiresSolidOutput() || this.CheckRequiresHighEnergyParticleInput() || this.CheckRequiresHighEnergyParticleOutput() || this.DiseaseCellVisName != null;
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x00115A38 File Offset: 0x00113C38
	public bool CheckRequiresPowerInput()
	{
		return this.RequiresPowerInput;
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x00115A40 File Offset: 0x00113C40
	public bool CheckRequiresPowerOutput()
	{
		return this.RequiresPowerOutput;
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x00115A48 File Offset: 0x00113C48
	public bool CheckRequiresGasInput()
	{
		return this.InputConduitType == ConduitType.Gas;
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x00115A53 File Offset: 0x00113C53
	public bool CheckRequiresGasOutput()
	{
		return this.OutputConduitType == ConduitType.Gas;
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x00115A5E File Offset: 0x00113C5E
	public bool CheckRequiresLiquidInput()
	{
		return this.InputConduitType == ConduitType.Liquid;
	}

	// Token: 0x060033E1 RID: 13281 RVA: 0x00115A69 File Offset: 0x00113C69
	public bool CheckRequiresLiquidOutput()
	{
		return this.OutputConduitType == ConduitType.Liquid;
	}

	// Token: 0x060033E2 RID: 13282 RVA: 0x00115A74 File Offset: 0x00113C74
	public bool CheckRequiresSolidInput()
	{
		return this.InputConduitType == ConduitType.Solid;
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x00115A7F File Offset: 0x00113C7F
	public bool CheckRequiresSolidOutput()
	{
		return this.OutputConduitType == ConduitType.Solid;
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x00115A8A File Offset: 0x00113C8A
	public bool CheckRequiresHighEnergyParticleInput()
	{
		return this.UseHighEnergyParticleInputPort;
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x00115A92 File Offset: 0x00113C92
	public bool CheckRequiresHighEnergyParticleOutput()
	{
		return this.UseHighEnergyParticleOutputPort;
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x00115A9A File Offset: 0x00113C9A
	public void AddFacade(string db_facade_id)
	{
		if (this.AvailableFacades == null)
		{
			this.AvailableFacades = new List<string>();
		}
		if (!this.AvailableFacades.Contains(db_facade_id))
		{
			this.AvailableFacades.Add(db_facade_id);
		}
	}

	// Token: 0x04001F18 RID: 7960
	public string[] RequiredDlcIds;

	// Token: 0x04001F19 RID: 7961
	public float EnergyConsumptionWhenActive;

	// Token: 0x04001F1A RID: 7962
	public float GeneratorWattageRating;

	// Token: 0x04001F1B RID: 7963
	public float GeneratorBaseCapacity;

	// Token: 0x04001F1C RID: 7964
	public float MassForTemperatureModification;

	// Token: 0x04001F1D RID: 7965
	public float ExhaustKilowattsWhenActive;

	// Token: 0x04001F1E RID: 7966
	public float SelfHeatKilowattsWhenActive;

	// Token: 0x04001F1F RID: 7967
	public float BaseMeltingPoint;

	// Token: 0x04001F20 RID: 7968
	public float ConstructionTime;

	// Token: 0x04001F21 RID: 7969
	public float WorkTime;

	// Token: 0x04001F22 RID: 7970
	public float ThermalConductivity = 1f;

	// Token: 0x04001F23 RID: 7971
	public int WidthInCells;

	// Token: 0x04001F24 RID: 7972
	public int HeightInCells;

	// Token: 0x04001F25 RID: 7973
	public int HitPoints;

	// Token: 0x04001F26 RID: 7974
	public bool RequiresPowerInput;

	// Token: 0x04001F27 RID: 7975
	public bool AddLogicPowerPort = true;

	// Token: 0x04001F28 RID: 7976
	public bool RequiresPowerOutput;

	// Token: 0x04001F29 RID: 7977
	public bool UseWhitePowerOutputConnectorColour;

	// Token: 0x04001F2A RID: 7978
	public CellOffset ElectricalArrowOffset;

	// Token: 0x04001F2B RID: 7979
	public ConduitType InputConduitType;

	// Token: 0x04001F2C RID: 7980
	public ConduitType OutputConduitType;

	// Token: 0x04001F2D RID: 7981
	public bool ModifiesTemperature;

	// Token: 0x04001F2E RID: 7982
	public bool Floodable = true;

	// Token: 0x04001F2F RID: 7983
	public bool Disinfectable = true;

	// Token: 0x04001F30 RID: 7984
	public bool Entombable = true;

	// Token: 0x04001F31 RID: 7985
	public bool Replaceable = true;

	// Token: 0x04001F32 RID: 7986
	public bool Invincible;

	// Token: 0x04001F33 RID: 7987
	public bool Overheatable = true;

	// Token: 0x04001F34 RID: 7988
	public bool Repairable = true;

	// Token: 0x04001F35 RID: 7989
	public float OverheatTemperature = 348.15f;

	// Token: 0x04001F36 RID: 7990
	public float FatalHot = 533.15f;

	// Token: 0x04001F37 RID: 7991
	public bool Breakable;

	// Token: 0x04001F38 RID: 7992
	public bool ContinuouslyCheckFoundation;

	// Token: 0x04001F39 RID: 7993
	public bool IsFoundation;

	// Token: 0x04001F3A RID: 7994
	[Obsolete]
	public bool isSolidTile;

	// Token: 0x04001F3B RID: 7995
	public bool DragBuild;

	// Token: 0x04001F3C RID: 7996
	public bool UseStructureTemperature = true;

	// Token: 0x04001F3D RID: 7997
	public global::Action HotKey = global::Action.NumActions;

	// Token: 0x04001F3E RID: 7998
	public CellOffset attachablePosition = new CellOffset(0, 0);

	// Token: 0x04001F3F RID: 7999
	public bool CanMove;

	// Token: 0x04001F40 RID: 8000
	public bool Cancellable = true;

	// Token: 0x04001F41 RID: 8001
	public bool OnePerWorld;

	// Token: 0x04001F42 RID: 8002
	public bool PlayConstructionSounds = true;

	// Token: 0x04001F43 RID: 8003
	public List<Tag> ReplacementTags;

	// Token: 0x04001F44 RID: 8004
	public List<ObjectLayer> ReplacementCandidateLayers;

	// Token: 0x04001F45 RID: 8005
	public List<ObjectLayer> EquivalentReplacementLayers;

	// Token: 0x04001F46 RID: 8006
	[HashedEnum]
	[NonSerialized]
	public HashedString ViewMode = OverlayModes.None.ID;

	// Token: 0x04001F47 RID: 8007
	public BuildLocationRule BuildLocationRule;

	// Token: 0x04001F48 RID: 8008
	public ObjectLayer ObjectLayer = ObjectLayer.Building;

	// Token: 0x04001F49 RID: 8009
	public ObjectLayer TileLayer = ObjectLayer.NumLayers;

	// Token: 0x04001F4A RID: 8010
	public ObjectLayer ReplacementLayer = ObjectLayer.NumLayers;

	// Token: 0x04001F4B RID: 8011
	public string DiseaseCellVisName;

	// Token: 0x04001F4C RID: 8012
	public string[] MaterialCategory;

	// Token: 0x04001F4D RID: 8013
	public string AudioCategory = "Metal";

	// Token: 0x04001F4E RID: 8014
	public string AudioSize = "medium";

	// Token: 0x04001F4F RID: 8015
	public float[] Mass;

	// Token: 0x04001F50 RID: 8016
	public bool AlwaysOperational;

	// Token: 0x04001F51 RID: 8017
	public List<LogicPorts.Port> LogicInputPorts;

	// Token: 0x04001F52 RID: 8018
	public List<LogicPorts.Port> LogicOutputPorts;

	// Token: 0x04001F53 RID: 8019
	public bool Upgradeable;

	// Token: 0x04001F54 RID: 8020
	public float BaseTimeUntilRepair = 600f;

	// Token: 0x04001F55 RID: 8021
	public bool ShowInBuildMenu = true;

	// Token: 0x04001F56 RID: 8022
	public bool DebugOnly;

	// Token: 0x04001F57 RID: 8023
	public PermittedRotations PermittedRotations;

	// Token: 0x04001F58 RID: 8024
	public Orientation InitialOrientation;

	// Token: 0x04001F59 RID: 8025
	public bool Deprecated;

	// Token: 0x04001F5A RID: 8026
	public bool UseHighEnergyParticleInputPort;

	// Token: 0x04001F5B RID: 8027
	public bool UseHighEnergyParticleOutputPort;

	// Token: 0x04001F5C RID: 8028
	public CellOffset HighEnergyParticleInputOffset;

	// Token: 0x04001F5D RID: 8029
	public CellOffset HighEnergyParticleOutputOffset;

	// Token: 0x04001F5E RID: 8030
	public CellOffset PowerInputOffset;

	// Token: 0x04001F5F RID: 8031
	public CellOffset PowerOutputOffset;

	// Token: 0x04001F60 RID: 8032
	public CellOffset UtilityInputOffset = new CellOffset(0, 1);

	// Token: 0x04001F61 RID: 8033
	public CellOffset UtilityOutputOffset = new CellOffset(1, 0);

	// Token: 0x04001F62 RID: 8034
	public Grid.SceneLayer SceneLayer = Grid.SceneLayer.Building;

	// Token: 0x04001F63 RID: 8035
	public Grid.SceneLayer ForegroundLayer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04001F64 RID: 8036
	public string RequiredAttribute = "";

	// Token: 0x04001F65 RID: 8037
	public int RequiredAttributeLevel;

	// Token: 0x04001F66 RID: 8038
	public List<Descriptor> EffectDescription;

	// Token: 0x04001F67 RID: 8039
	public float MassTier;

	// Token: 0x04001F68 RID: 8040
	public float HeatTier;

	// Token: 0x04001F69 RID: 8041
	public float ConstructionTimeTier;

	// Token: 0x04001F6A RID: 8042
	public string PrimaryUse;

	// Token: 0x04001F6B RID: 8043
	public string SecondaryUse;

	// Token: 0x04001F6C RID: 8044
	public string PrimarySideEffect;

	// Token: 0x04001F6D RID: 8045
	public string SecondarySideEffect;

	// Token: 0x04001F6E RID: 8046
	public Recipe CraftRecipe;

	// Token: 0x04001F6F RID: 8047
	public Sprite UISprite;

	// Token: 0x04001F70 RID: 8048
	public bool isKAnimTile;

	// Token: 0x04001F71 RID: 8049
	public bool isUtility;

	// Token: 0x04001F72 RID: 8050
	public KAnimFile[] AnimFiles;

	// Token: 0x04001F73 RID: 8051
	public string DefaultAnimState = "off";

	// Token: 0x04001F74 RID: 8052
	public bool BlockTileIsTransparent;

	// Token: 0x04001F75 RID: 8053
	public TextureAtlas BlockTileAtlas;

	// Token: 0x04001F76 RID: 8054
	public TextureAtlas BlockTilePlaceAtlas;

	// Token: 0x04001F77 RID: 8055
	public TextureAtlas BlockTileShineAtlas;

	// Token: 0x04001F78 RID: 8056
	public Material BlockTileMaterial;

	// Token: 0x04001F79 RID: 8057
	public BlockTileDecorInfo DecorBlockTileInfo;

	// Token: 0x04001F7A RID: 8058
	public BlockTileDecorInfo DecorPlaceBlockTileInfo;

	// Token: 0x04001F7B RID: 8059
	public List<Klei.AI.Attribute> attributes = new List<Klei.AI.Attribute>();

	// Token: 0x04001F7C RID: 8060
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x04001F7D RID: 8061
	public Tag AttachmentSlotTag;

	// Token: 0x04001F7E RID: 8062
	public bool PreventIdleTraversalPastBuilding;

	// Token: 0x04001F7F RID: 8063
	public GameObject BuildingComplete;

	// Token: 0x04001F80 RID: 8064
	public GameObject BuildingPreview;

	// Token: 0x04001F81 RID: 8065
	public GameObject BuildingUnderConstruction;

	// Token: 0x04001F82 RID: 8066
	public CellOffset[] PlacementOffsets;

	// Token: 0x04001F83 RID: 8067
	public CellOffset[] ConstructionOffsetFilter;

	// Token: 0x04001F84 RID: 8068
	public static CellOffset[] ConstructionOffsetFilter_OneDown = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x04001F85 RID: 8069
	public float BaseDecor;

	// Token: 0x04001F86 RID: 8070
	public float BaseDecorRadius;

	// Token: 0x04001F87 RID: 8071
	public int BaseNoisePollution;

	// Token: 0x04001F88 RID: 8072
	public int BaseNoisePollutionRadius;

	// Token: 0x04001F89 RID: 8073
	public List<string> AvailableFacades = new List<string>();

	// Token: 0x04001F8A RID: 8074
	private static Dictionary<CellOffset, CellOffset[]> placementOffsetsCache = new Dictionary<CellOffset, CellOffset[]>();
}

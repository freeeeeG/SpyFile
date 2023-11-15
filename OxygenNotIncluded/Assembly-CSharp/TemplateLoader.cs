﻿using System;
using System.Collections.Generic;
using Klei.AI;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public static class TemplateLoader
{
	// Token: 0x06003CEC RID: 15596 RVA: 0x00151B9C File Offset: 0x0014FD9C
	public static void Stamp(TemplateContainer template, Vector2 rootLocation, System.Action on_complete_callback)
	{
		TemplateLoader.ActiveStamp item = new TemplateLoader.ActiveStamp(template, rootLocation, on_complete_callback);
		TemplateLoader.activeStamps.Add(item);
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x00151BBD File Offset: 0x0014FDBD
	private static void StampComplete(TemplateLoader.ActiveStamp stamp)
	{
		TemplateLoader.activeStamps.Remove(stamp);
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x00151BCC File Offset: 0x0014FDCC
	private static void BuildPhase1(int baseX, int baseY, TemplateContainer template, System.Action callback)
	{
		if (Grid.WidthInCells < 16)
		{
			return;
		}
		if (template.cells == null)
		{
			callback();
			return;
		}
		CellOffset[] array = new CellOffset[template.cells.Count];
		for (int i = 0; i < template.cells.Count; i++)
		{
			array[i] = new CellOffset(template.cells[i].location_x, template.cells[i].location_y);
		}
		TemplateLoader.ClearPickups(baseX, baseY, array);
		if (template.cells.Count > 0)
		{
			TemplateLoader.ApplyGridProperties(baseX, baseY, template);
			TemplateLoader.PlaceCells(baseX, baseY, template, callback);
			TemplateLoader.ClearEntities<Crop>(baseX, baseY, array);
			TemplateLoader.ClearEntities<Health>(baseX, baseY, array);
			TemplateLoader.ClearEntities<Geyser>(baseX, baseY, array);
			return;
		}
		callback();
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x00151C90 File Offset: 0x0014FE90
	private static void BuildPhase2(int baseX, int baseY, TemplateContainer template, System.Action callback)
	{
		int num = Grid.OffsetCell(0, baseX, baseY);
		if (template == null)
		{
			global::Debug.LogError("No stamp template");
		}
		if (template.buildings != null)
		{
			for (int i = 0; i < template.buildings.Count; i++)
			{
				TemplateLoader.PlaceBuilding(template.buildings[i], num);
			}
		}
		HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(callback, false));
		SimMessages.ReplaceElement(num, ElementLoader.elements[(int)Grid.ElementIdx[num]].id, CellEventLogger.Instance.TemplateLoader, Grid.Mass[num], Grid.Temperature[num], Grid.DiseaseIdx[num], Grid.DiseaseCount[num], handle.index);
		handle.index = -1;
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x00151D60 File Offset: 0x0014FF60
	public static GameObject PlaceBuilding(Prefab prefab, int root_cell)
	{
		if (prefab == null || prefab.id == "")
		{
			return null;
		}
		BuildingDef buildingDef = Assets.GetBuildingDef(prefab.id);
		if (buildingDef == null)
		{
			return null;
		}
		int num = prefab.location_x;
		int location_y = prefab.location_y;
		if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, num, location_y)))
		{
			return null;
		}
		num -= (buildingDef.WidthInCells - 1) / 2;
		GameObject gameObject = Scenario.PlaceBuilding(root_cell, num, location_y, prefab.id, prefab.element);
		if (gameObject == null)
		{
			global::Debug.LogWarning("Null prefab for " + prefab.id);
			return gameObject;
		}
		BuildingComplete component = gameObject.GetComponent<BuildingComplete>();
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TemplateBuilding, true);
		Components.TemplateBuildings.Add(component);
		Rotatable component2 = gameObject.GetComponent<Rotatable>();
		if (component2 != null)
		{
			component2.SetOrientation(prefab.rotationOrientation);
		}
		PrimaryElement component3 = component.GetComponent<PrimaryElement>();
		if (prefab.temperature > 0f)
		{
			component3.Temperature = prefab.temperature;
		}
		component3.AddDisease(Db.Get().Diseases.GetIndex(prefab.diseaseName), prefab.diseaseCount, "TemplateLoader.PlaceBuilding");
		if (prefab.id == "Door")
		{
			for (int i = 0; i < component.PlacementCells.Length; i++)
			{
				SimMessages.ReplaceElement(component.PlacementCells[i], SimHashes.Vacuum, CellEventLogger.Instance.TemplateLoader, 0f, 0f, byte.MaxValue, 0, -1);
			}
		}
		if (prefab.amounts != null)
		{
			foreach (Prefab.template_amount_value template_amount_value in prefab.amounts)
			{
				try
				{
					if (Db.Get().Amounts.Get(template_amount_value.id) != null)
					{
						gameObject.GetAmounts().SetValue(template_amount_value.id, template_amount_value.value);
					}
				}
				catch
				{
					global::Debug.LogWarning(string.Format("Building does not have amount with ID {0}", template_amount_value.id));
				}
			}
		}
		if (prefab.other_values != null)
		{
			Prefab.template_amount_value[] array = prefab.other_values;
			for (int j = 0; j < array.Length; j++)
			{
				Prefab.template_amount_value template_amount_value2 = array[j];
				string id = template_amount_value2.id;
				if (id != null)
				{
					if (!(id == "joulesAvailable"))
					{
						if (!(id == "sealedDoorDirection"))
						{
							if (id == "switchSetting")
							{
								LogicSwitch s = gameObject.GetComponent<LogicSwitch>();
								if (s && ((s.IsSwitchedOn && template_amount_value2.value == 0f) || (!s.IsSwitchedOn && template_amount_value2.value == 1f)))
								{
									s.SetFirstFrameCallback(delegate
									{
										s.HandleToggle();
									});
								}
							}
						}
						else
						{
							Unsealable component4 = gameObject.GetComponent<Unsealable>();
							if (component4)
							{
								component4.facingRight = (template_amount_value2.value != 0f);
							}
						}
					}
					else
					{
						Battery component5 = gameObject.GetComponent<Battery>();
						if (component5)
						{
							component5.AddEnergy(template_amount_value2.value);
						}
					}
				}
			}
		}
		if (prefab.storage != null && prefab.storage.Count > 0)
		{
			Storage component6 = component.gameObject.GetComponent<Storage>();
			if (component6 == null)
			{
				global::Debug.LogWarning("No storage component on stampTemplate building " + prefab.id + ". Saved storage contents will be ignored.");
			}
			int k = 0;
			while (k < prefab.storage.Count)
			{
				StorageItem storageItem = prefab.storage[k];
				string id2 = storageItem.id;
				GameObject gameObject2;
				if (storageItem.isOre)
				{
					gameObject2 = ElementLoader.FindElementByHash(storageItem.element).substance.SpawnResource(Vector3.zero, storageItem.units, storageItem.temperature, Db.Get().Diseases.GetIndex(storageItem.diseaseName), storageItem.diseaseCount, false, false, false);
					goto IL_49B;
				}
				gameObject2 = Scenario.SpawnPrefab(root_cell, 0, 0, id2, Grid.SceneLayer.Ore);
				if (gameObject2 == null)
				{
					global::Debug.LogWarning("Null prefab for " + id2);
				}
				else
				{
					gameObject2.SetActive(true);
					PrimaryElement component7 = gameObject2.GetComponent<PrimaryElement>();
					component7.Units = storageItem.units;
					component7.Temperature = storageItem.temperature;
					component7.AddDisease(Db.Get().Diseases.GetIndex(storageItem.diseaseName), storageItem.diseaseCount, "TemplateLoader.PlaceBuilding");
					global::Rottable.Instance smi = gameObject2.GetSMI<global::Rottable.Instance>();
					if (smi != null)
					{
						smi.RotValue = storageItem.rottable.rotAmount;
						goto IL_49B;
					}
					goto IL_49B;
				}
				IL_4C2:
				k++;
				continue;
				IL_49B:
				GameObject gameObject3 = component6.Store(gameObject2, true, true, true, false);
				if (gameObject3 != null)
				{
					gameObject3.GetComponent<Pickupable>().OnStore(component6);
					goto IL_4C2;
				}
				goto IL_4C2;
			}
		}
		if (prefab.connections != 0)
		{
			TemplateLoader.PlaceUtilityConnection(gameObject, prefab, root_cell);
		}
		return gameObject;
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x00152268 File Offset: 0x00150468
	public static void PlaceUtilityConnection(GameObject spawned, Prefab bc, int root_cell)
	{
		int cell = Grid.OffsetCell(root_cell, bc.location_x, bc.location_y);
		UtilityConnections connection = (UtilityConnections)bc.connections;
		string id = bc.id;
		if (id != null)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
			if (num <= 1938276536U)
			{
				if (num <= 609727380U)
				{
					if (num != 301047391U)
					{
						if (num != 379600269U)
						{
							if (num != 609727380U)
							{
								return;
							}
							if (!(id == "GasConduit"))
							{
								return;
							}
							goto IL_1DE;
						}
						else
						{
							if (!(id == "LiquidConduit"))
							{
								return;
							}
							goto IL_1FB;
						}
					}
					else if (!(id == "WireRefined"))
					{
						return;
					}
				}
				else if (num != 848332507U)
				{
					if (num != 1213766155U)
					{
						if (num != 1938276536U)
						{
							return;
						}
						if (!(id == "Wire"))
						{
							return;
						}
					}
					else
					{
						if (!(id == "TravelTube"))
						{
							return;
						}
						spawned.GetComponent<TravelTube>().SetFirstFrameCallback(delegate
						{
							Game.Instance.travelTubeSystem.SetConnections(connection, cell, true);
							KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
							if (component != null)
							{
								component.Refresh();
							}
						});
						return;
					}
				}
				else
				{
					if (!(id == "InsulatedGasConduit"))
					{
						return;
					}
					goto IL_1DE;
				}
			}
			else if (num <= 3711470516U)
			{
				if (num != 3228988836U)
				{
					if (num != 3324196971U)
					{
						if (num != 3711470516U)
						{
							return;
						}
						if (!(id == "InsulatedLiquidConduit"))
						{
							return;
						}
						goto IL_1FB;
					}
					else
					{
						if (!(id == "GasConduitRadiant"))
						{
							return;
						}
						goto IL_1DE;
					}
				}
				else
				{
					if (!(id == "LogicWire"))
					{
						return;
					}
					spawned.GetComponent<LogicWire>().SetFirstFrameCallback(delegate
					{
						Game.Instance.logicCircuitSystem.SetConnections(connection, cell, true);
						KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
						if (component != null)
						{
							component.Refresh();
						}
					});
					return;
				}
			}
			else if (num <= 3863001292U)
			{
				if (num != 3716494409U)
				{
					if (num != 3863001292U)
					{
						return;
					}
					if (!(id == "LiquidConduitRadiant"))
					{
						return;
					}
					goto IL_1FB;
				}
				else if (!(id == "HighWattageWire"))
				{
					return;
				}
			}
			else if (num != 4113070310U)
			{
				if (num != 4243975822U)
				{
					return;
				}
				if (!(id == "WireRefinedHighWattage"))
				{
					return;
				}
			}
			else
			{
				if (!(id == "SolidConduit"))
				{
					return;
				}
				spawned.GetComponent<SolidConduit>().SetFirstFrameCallback(delegate
				{
					Game.Instance.solidConduitSystem.SetConnections(connection, cell, true);
					KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
					if (component != null)
					{
						component.Refresh();
					}
				});
				return;
			}
			spawned.GetComponent<Wire>().SetFirstFrameCallback(delegate
			{
				Game.Instance.electricalConduitSystem.SetConnections(connection, cell, true);
				KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
				if (component != null)
				{
					component.Refresh();
				}
			});
			return;
			IL_1DE:
			spawned.GetComponent<Conduit>().SetFirstFrameCallback(delegate
			{
				Game.Instance.gasConduitSystem.SetConnections(connection, cell, true);
				KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
				if (component != null)
				{
					component.Refresh();
				}
			});
			return;
			IL_1FB:
			spawned.GetComponent<Conduit>().SetFirstFrameCallback(delegate
			{
				Game.Instance.liquidConduitSystem.SetConnections(connection, cell, true);
				KAnimGraphTileVisualizer component = spawned.GetComponent<KAnimGraphTileVisualizer>();
				if (component != null)
				{
					component.Refresh();
				}
			});
			return;
		}
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x001524E4 File Offset: 0x001506E4
	public static GameObject PlacePickupables(Prefab prefab, int root_cell)
	{
		int location_x = prefab.location_x;
		int location_y = prefab.location_y;
		if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, location_x, location_y)))
		{
			return null;
		}
		GameObject gameObject = Scenario.SpawnPrefab(root_cell, location_x, location_y, prefab.id, Grid.SceneLayer.Ore);
		if (gameObject == null)
		{
			global::Debug.LogWarning("Null prefab for " + prefab.id);
			return null;
		}
		gameObject.SetActive(true);
		if (prefab.units != 0f)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.Units = prefab.units;
			component.Temperature = ((prefab.temperature > 0f) ? prefab.temperature : component.Element.defaultValues.temperature);
			component.AddDisease(Db.Get().Diseases.GetIndex(prefab.diseaseName), prefab.diseaseCount, "TemplateLoader.PlacePickupables");
		}
		global::Rottable.Instance smi = gameObject.GetSMI<global::Rottable.Instance>();
		if (smi != null)
		{
			smi.RotValue = prefab.rottable.rotAmount;
		}
		return gameObject;
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x001525E0 File Offset: 0x001507E0
	public static GameObject PlaceOtherEntities(Prefab prefab, int root_cell)
	{
		int location_x = prefab.location_x;
		int location_y = prefab.location_y;
		if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, location_x, location_y)))
		{
			return null;
		}
		GameObject prefab2 = Assets.GetPrefab(new Tag(prefab.id));
		if (prefab2 == null)
		{
			return null;
		}
		Grid.SceneLayer scene_layer = Grid.SceneLayer.Front;
		KBatchedAnimController component = prefab2.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			scene_layer = component.sceneLayer;
		}
		GameObject gameObject = Scenario.SpawnPrefab(root_cell, location_x, location_y, prefab.id, scene_layer);
		Rotatable component2 = gameObject.GetComponent<Rotatable>();
		if (component2 != null)
		{
			component2.SetOrientation(prefab.rotationOrientation);
		}
		if (gameObject == null)
		{
			global::Debug.LogWarning("Null prefab for " + prefab.id);
			return null;
		}
		gameObject.SetActive(true);
		if (prefab.amounts != null)
		{
			foreach (Prefab.template_amount_value template_amount_value in prefab.amounts)
			{
				try
				{
					gameObject.GetAmounts().SetValue(template_amount_value.id, template_amount_value.value);
				}
				catch
				{
					global::Debug.LogWarning(string.Format("Entity {0} does not have amount with ID {1}", gameObject.GetProperName(), template_amount_value.id));
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x00152718 File Offset: 0x00150918
	public static GameObject PlaceElementalOres(Prefab prefab, int root_cell)
	{
		int location_x = prefab.location_x;
		int location_y = prefab.location_y;
		if (!Grid.IsValidCell(Grid.OffsetCell(root_cell, location_x, location_y)))
		{
			return null;
		}
		Substance substance = ElementLoader.FindElementByHash(prefab.element).substance;
		Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(root_cell, location_x, location_y), Grid.SceneLayer.Ore);
		byte index = Db.Get().Diseases.GetIndex(prefab.diseaseName);
		if (prefab.temperature <= 0f)
		{
			global::Debug.LogWarning("Template trying to spawn zero temperature substance!");
			prefab.temperature = 300f;
		}
		return substance.SpawnResource(position, prefab.units, prefab.temperature, index, prefab.diseaseCount, false, false, false);
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x001527C0 File Offset: 0x001509C0
	private static void BuildPhase3(int baseX, int baseY, TemplateContainer template, System.Action callback)
	{
		if (template != null)
		{
			int root_cell = Grid.OffsetCell(0, baseX, baseY);
			foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
			{
				KAnimGraphTileVisualizer component = buildingComplete.GetComponent<KAnimGraphTileVisualizer>();
				if (component != null)
				{
					component.Refresh();
				}
			}
			if (template.pickupables != null)
			{
				for (int i = 0; i < template.pickupables.Count; i++)
				{
					if (template.pickupables[i] != null && !(template.pickupables[i].id == ""))
					{
						TemplateLoader.PlacePickupables(template.pickupables[i], root_cell);
					}
				}
			}
			if (template.elementalOres != null)
			{
				for (int j = 0; j < template.elementalOres.Count; j++)
				{
					if (template.elementalOres[j] != null && !(template.elementalOres[j].id == ""))
					{
						TemplateLoader.PlaceElementalOres(template.elementalOres[j], root_cell);
					}
				}
			}
		}
		if (callback != null)
		{
			callback();
		}
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x001528FC File Offset: 0x00150AFC
	private static void BuildPhase4(int baseX, int baseY, TemplateContainer template, System.Action callback)
	{
		if (template != null)
		{
			int root_cell = Grid.OffsetCell(0, baseX, baseY);
			if (template.otherEntities != null)
			{
				for (int i = 0; i < template.otherEntities.Count; i++)
				{
					if (template.otherEntities[i] != null && !(template.otherEntities[i].id == ""))
					{
						TemplateLoader.PlaceOtherEntities(template.otherEntities[i], root_cell);
					}
				}
			}
			template = null;
		}
		if (callback != null)
		{
			callback();
		}
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x00152980 File Offset: 0x00150B80
	private static void ClearPickups(int baseX, int baseY, CellOffset[] template_as_offsets)
	{
		if (SaveGame.Instance.worldGenSpawner != null)
		{
			SaveGame.Instance.worldGenSpawner.ClearSpawnersInArea(new Vector2((float)baseX, (float)baseY), template_as_offsets);
		}
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			if (Grid.IsCellOffsetOf(Grid.XYToCell(baseX, baseY), pickupable.gameObject, template_as_offsets))
			{
				Util.KDestroyGameObject(pickupable.gameObject);
			}
		}
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x00152A1C File Offset: 0x00150C1C
	private static void ClearEntities<T>(int rootX, int rootY, CellOffset[] TemplateOffsets) where T : KMonoBehaviour
	{
		foreach (T t in (T[])UnityEngine.Object.FindObjectsOfType(typeof(T)))
		{
			if (Grid.IsCellOffsetOf(Grid.PosToCell(t.gameObject), Grid.XYToCell(rootX, rootY), TemplateOffsets))
			{
				Util.KDestroyGameObject(t.gameObject);
			}
		}
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x00152A84 File Offset: 0x00150C84
	private static void PlaceCells(int baseX, int baseY, TemplateContainer template, System.Action callback)
	{
		if (template == null)
		{
			global::Debug.LogError("Template Loader does not have template.");
		}
		if (template.cells == null)
		{
			callback();
			return;
		}
		HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(callback, false));
		for (int i = 0; i < template.cells.Count; i++)
		{
			int num = Grid.XYToCell(template.cells[i].location_x + baseX, template.cells[i].location_y + baseY);
			if (!Grid.IsValidCell(num))
			{
				global::Debug.LogError(string.Format("Trying to replace invalid cells cell{0} root{1}:{2} offset{3}:{4}", new object[]
				{
					num,
					baseX,
					baseY,
					template.cells[i].location_x,
					template.cells[i].location_y
				}));
			}
			SimHashes element = template.cells[i].element;
			float mass = template.cells[i].mass;
			float temperature = template.cells[i].temperature;
			byte index = Db.Get().Diseases.GetIndex(template.cells[i].diseaseName);
			int diseaseCount = template.cells[i].diseaseCount;
			SimMessages.ReplaceElement(num, element, CellEventLogger.Instance.TemplateLoader, mass, temperature, index, diseaseCount, handle.index);
			handle.index = -1;
		}
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x00152C10 File Offset: 0x00150E10
	public static void ApplyGridProperties(int baseX, int baseY, TemplateContainer template)
	{
		if (template.cells == null)
		{
			return;
		}
		for (int i = 0; i < template.cells.Count; i++)
		{
			int num = Grid.XYToCell(template.cells[i].location_x + baseX, template.cells[i].location_y + baseY);
			if (Grid.IsValidCell(num) && template.cells[i].preventFoWReveal)
			{
				Grid.PreventFogOfWarReveal[num] = true;
				Grid.Visible[num] = 0;
			}
		}
	}

	// Token: 0x040027CE RID: 10190
	private static List<TemplateLoader.ActiveStamp> activeStamps = new List<TemplateLoader.ActiveStamp>();

	// Token: 0x020015FC RID: 5628
	private class ActiveStamp
	{
		// Token: 0x060088FF RID: 35071 RVA: 0x00310437 File Offset: 0x0030E637
		public ActiveStamp(TemplateContainer template, Vector2 rootLocation, System.Action onCompleteCallback)
		{
			this.m_template = template;
			this.m_rootLocation = new Vector2I((int)rootLocation.x, (int)rootLocation.y);
			this.m_onCompleteCallback = onCompleteCallback;
			this.NextPhase();
		}

		// Token: 0x06008900 RID: 35072 RVA: 0x0031046C File Offset: 0x0030E66C
		private void NextPhase()
		{
			this.currentPhase++;
			switch (this.currentPhase)
			{
			case 1:
				TemplateLoader.BuildPhase1(this.m_rootLocation.x, this.m_rootLocation.y, this.m_template, new System.Action(this.NextPhase));
				return;
			case 2:
				TemplateLoader.BuildPhase2(this.m_rootLocation.x, this.m_rootLocation.y, this.m_template, new System.Action(this.NextPhase));
				return;
			case 3:
				TemplateLoader.BuildPhase3(this.m_rootLocation.x, this.m_rootLocation.y, this.m_template, new System.Action(this.NextPhase));
				return;
			case 4:
				TemplateLoader.BuildPhase4(this.m_rootLocation.x, this.m_rootLocation.y, this.m_template, new System.Action(this.NextPhase));
				return;
			case 5:
				this.m_onCompleteCallback();
				TemplateLoader.StampComplete(this);
				return;
			default:
				global::Debug.Assert(false, "How did we get here?? Something's wrong!");
				return;
			}
		}

		// Token: 0x04006A23 RID: 27171
		private TemplateContainer m_template;

		// Token: 0x04006A24 RID: 27172
		private Vector2I m_rootLocation;

		// Token: 0x04006A25 RID: 27173
		private System.Action m_onCompleteCallback;

		// Token: 0x04006A26 RID: 27174
		private int currentPhase;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000812 RID: 2066
public class DebugTool : DragTool
{
	// Token: 0x06003B6C RID: 15212 RVA: 0x0014A0DA File Offset: 0x001482DA
	public static void DestroyInstance()
	{
		DebugTool.Instance = null;
	}

	// Token: 0x06003B6D RID: 15213 RVA: 0x0014A0E2 File Offset: 0x001482E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugTool.Instance = this;
	}

	// Token: 0x06003B6E RID: 15214 RVA: 0x0014A0F0 File Offset: 0x001482F0
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003B6F RID: 15215 RVA: 0x0014A0FD File Offset: 0x001482FD
	public void Activate(DebugTool.Type type)
	{
		this.type = type;
		this.Activate();
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x0014A10C File Offset: 0x0014830C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		PlayerController.Instance.ToolDeactivated(this);
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x0014A120 File Offset: 0x00148320
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			switch (this.type)
			{
			case DebugTool.Type.ReplaceSubstance:
				this.DoReplaceSubstance(cell);
				return;
			case DebugTool.Type.FillReplaceSubstance:
			{
				GameUtil.FloodFillNext.Clear();
				GameUtil.FloodFillVisited.Clear();
				SimHashes elem_hash = Grid.Element[cell].id;
				GameUtil.FloodFillConditional(cell, delegate(int check_cell)
				{
					bool result = false;
					if (Grid.Element[check_cell].id == elem_hash)
					{
						result = true;
						this.DoReplaceSubstance(check_cell);
					}
					return result;
				}, GameUtil.FloodFillVisited, null);
				return;
			}
			case DebugTool.Type.Clear:
				this.ClearCell(cell);
				return;
			case DebugTool.Type.AddSelection:
				DebugBaseTemplateButton.Instance.AddToSelection(cell);
				return;
			case DebugTool.Type.RemoveSelection:
				DebugBaseTemplateButton.Instance.RemoveFromSelection(cell);
				return;
			case DebugTool.Type.Deconstruct:
				this.DeconstructCell(cell);
				return;
			case DebugTool.Type.Destroy:
				this.DestroyCell(cell);
				return;
			case DebugTool.Type.Sample:
				DebugPaintElementScreen.Instance.SampleCell(cell);
				return;
			case DebugTool.Type.StoreSubstance:
				this.DoStoreSubstance(cell);
				return;
			case DebugTool.Type.Dig:
				SimMessages.Dig(cell, -1, false);
				return;
			case DebugTool.Type.Heat:
				SimMessages.ModifyEnergy(cell, 10000f, 10000f, SimMessages.EnergySourceID.DebugHeat);
				return;
			case DebugTool.Type.Cool:
				SimMessages.ModifyEnergy(cell, -10000f, 10000f, SimMessages.EnergySourceID.DebugCool);
				return;
			case DebugTool.Type.AddPressure:
				SimMessages.ModifyMass(cell, 10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 293f, SimHashes.Oxygen);
				return;
			case DebugTool.Type.RemovePressure:
				SimMessages.ModifyMass(cell, -10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 0f, SimHashes.Oxygen);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x0014A298 File Offset: 0x00148498
	public void DoReplaceSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte b = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : Grid.Temperature[cell];
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : Grid.Mass[cell];
		int num3 = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : Grid.DiseaseCount[cell];
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		if (DebugPaintElementScreen.Instance.affectCells.isOn)
		{
			SimMessages.ReplaceElement(cell, element.id, CellEventLogger.Instance.DebugTool, num2, num, b, num3, -1);
			if (DebugPaintElementScreen.Instance.set_prevent_fow_reveal)
			{
				Grid.Visible[cell] = 0;
				Grid.PreventFogOfWarReveal[cell] = true;
			}
			else if (DebugPaintElementScreen.Instance.set_allow_fow_reveal && Grid.PreventFogOfWarReveal[cell])
			{
				Grid.PreventFogOfWarReveal[cell] = false;
			}
		}
		if (DebugPaintElementScreen.Instance.affectBuildings.isOn)
		{
			foreach (GameObject gameObject in new List<GameObject>
			{
				Grid.Objects[cell, 1],
				Grid.Objects[cell, 2],
				Grid.Objects[cell, 9],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 12],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 26]
			})
			{
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (num > 0f)
					{
						component.Temperature = num;
					}
					if (num3 > 0 && b != 255)
					{
						component.ModifyDiseaseCount(int.MinValue, "DebugTool.DoReplaceSubstance");
						component.AddDisease(b, num3, "DebugTool.DoReplaceSubstance");
					}
				}
			}
		}
	}

	// Token: 0x06003B73 RID: 15219 RVA: 0x0014A55C File Offset: 0x0014875C
	public void DeconstructCell(int cell)
	{
		bool instantBuildMode = DebugHandler.InstantBuildMode;
		DebugHandler.InstantBuildMode = true;
		DeconstructTool.Instance.DeconstructCell(cell);
		if (!instantBuildMode)
		{
			DebugHandler.InstantBuildMode = false;
		}
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x0014A57C File Offset: 0x0014877C
	public void DestroyCell(int cell)
	{
		foreach (GameObject gameObject in new List<GameObject>
		{
			Grid.Objects[cell, 2],
			Grid.Objects[cell, 1],
			Grid.Objects[cell, 12],
			Grid.Objects[cell, 16],
			Grid.Objects[cell, 0],
			Grid.Objects[cell, 26]
		})
		{
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.ClearCell(cell);
		if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
		{
			SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
			return;
		}
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x0014A6BC File Offset: 0x001488BC
	public void ClearCell(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		ListPool<ScenePartitionerEntry, DebugTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, DebugTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (pickupable != null && pickupable.GetComponent<MinionBrain>() == null)
			{
				Util.KDestroyGameObject(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x0014A744 File Offset: 0x00148944
	public void DoStoreSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return;
		}
		Storage component = gameObject.GetComponent<Storage>();
		if (component == null)
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte disease_idx = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : element.defaultValues.temperature;
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : element.defaultValues.mass;
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		int disease_count = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : 0;
		if (element.IsGas)
		{
			component.AddGasChunk(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsLiquid)
		{
			component.AddLiquid(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsSolid)
		{
			component.AddOre(element.id, num2, num, disease_idx, disease_count, false, true);
		}
	}

	// Token: 0x04002737 RID: 10039
	public static DebugTool Instance;

	// Token: 0x04002738 RID: 10040
	public DebugTool.Type type;

	// Token: 0x020015EA RID: 5610
	public enum Type
	{
		// Token: 0x040069EA RID: 27114
		ReplaceSubstance,
		// Token: 0x040069EB RID: 27115
		FillReplaceSubstance,
		// Token: 0x040069EC RID: 27116
		Clear,
		// Token: 0x040069ED RID: 27117
		AddSelection,
		// Token: 0x040069EE RID: 27118
		RemoveSelection,
		// Token: 0x040069EF RID: 27119
		Deconstruct,
		// Token: 0x040069F0 RID: 27120
		Destroy,
		// Token: 0x040069F1 RID: 27121
		Sample,
		// Token: 0x040069F2 RID: 27122
		StoreSubstance,
		// Token: 0x040069F3 RID: 27123
		Dig,
		// Token: 0x040069F4 RID: 27124
		Heat,
		// Token: 0x040069F5 RID: 27125
		Cool,
		// Token: 0x040069F6 RID: 27126
		AddPressure,
		// Token: 0x040069F7 RID: 27127
		RemovePressure,
		// Token: 0x040069F8 RID: 27128
		PaintPlant
	}
}

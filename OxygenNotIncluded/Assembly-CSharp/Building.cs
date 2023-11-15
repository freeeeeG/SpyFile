using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
[AddComponentMenu("KMonoBehaviour/scripts/Building")]
public class Building : KMonoBehaviour, IGameObjectEffectDescriptor, IUniformGridObject, IApproachable
{
	// Token: 0x1700019B RID: 411
	// (get) Token: 0x0600232B RID: 9003 RVA: 0x000C0A8B File Offset: 0x000BEC8B
	public Orientation Orientation
	{
		get
		{
			if (!(this.rotatable != null))
			{
				return Orientation.Neutral;
			}
			return this.rotatable.GetOrientation();
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x0600232C RID: 9004 RVA: 0x000C0AA8 File Offset: 0x000BECA8
	public int[] PlacementCells
	{
		get
		{
			if (this.placementCells == null)
			{
				this.RefreshCells();
			}
			return this.placementCells;
		}
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000C0ABE File Offset: 0x000BECBE
	public Extents GetExtents()
	{
		if (this.extents.width == 0 || this.extents.height == 0)
		{
			this.RefreshCells();
		}
		return this.extents;
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000C0AE8 File Offset: 0x000BECE8
	public Extents GetValidPlacementExtents()
	{
		Extents result = this.GetExtents();
		result.x--;
		result.y--;
		result.width += 2;
		result.height += 2;
		return result;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000C0B30 File Offset: 0x000BED30
	public bool PlacementCellsContainCell(int cell)
	{
		for (int i = 0; i < this.PlacementCells.Length; i++)
		{
			if (this.PlacementCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000C0B60 File Offset: 0x000BED60
	public void RefreshCells()
	{
		this.placementCells = new int[this.Def.PlacementOffsets.Length];
		int num = Grid.PosToCell(this);
		if (num < 0)
		{
			this.extents.x = -1;
			this.extents.y = -1;
			this.extents.width = this.Def.WidthInCells;
			this.extents.height = this.Def.HeightInCells;
			return;
		}
		Orientation orientation = this.Orientation;
		for (int i = 0; i < this.Def.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.Def.PlacementOffsets[i], orientation);
			int num2 = Grid.OffsetCell(num, rotatedCellOffset);
			this.placementCells[i] = num2;
		}
		int num3 = 0;
		int num4 = 0;
		Grid.CellToXY(this.placementCells[0], out num3, out num4);
		int num5 = num3;
		int num6 = num4;
		foreach (int cell in this.placementCells)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(cell, out val, out val2);
			num3 = Math.Min(num3, val);
			num4 = Math.Min(num4, val2);
			num5 = Math.Max(num5, val);
			num6 = Math.Max(num6, val2);
		}
		this.extents.x = num3;
		this.extents.y = num4;
		this.extents.width = num5 - num3 + 1;
		this.extents.height = num6 - num4 + 1;
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000C0CD0 File Offset: 0x000BEED0
	[OnDeserialized]
	internal void OnDeserialized()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component != null && component.Temperature == 0f)
		{
			if (component.Element == null)
			{
				DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + " primary element has no element.", base.gameObject);
				return;
			}
			if (!(this is BuildingUnderConstruction))
			{
				DeserializeWarnings.Instance.BuildingTemeperatureIsZeroKelvin.Warn(base.name + " is at zero degrees kelvin. Resetting temperature.", null);
				component.Temperature = component.Element.defaultValues.temperature;
			}
		}
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000C0D66 File Offset: 0x000BEF66
	public void SetDescription(string desc)
	{
		this.description = desc;
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06002333 RID: 9011 RVA: 0x000C0D6F File Offset: 0x000BEF6F
	public string Desc
	{
		get
		{
			if (!this.description.IsNullOrWhiteSpace())
			{
				return this.description;
			}
			return this.Def.Desc;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06002334 RID: 9012 RVA: 0x000C0D90 File Offset: 0x000BEF90
	public string DescFlavour
	{
		get
		{
			if (!this.descriptionFlavour.IsNullOrWhiteSpace())
			{
				return this.descriptionFlavour;
			}
			return this.Def.Effect;
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000C0DB1 File Offset: 0x000BEFB1
	public void SetDescriptionFlavour(string descriptionFlavour)
	{
		this.descriptionFlavour = descriptionFlavour;
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000C0DBC File Offset: 0x000BEFBC
	protected override void OnSpawn()
	{
		if (this.Def == null)
		{
			global::Debug.LogError("Missing building definition on object " + base.name);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetName(this.Def.Name);
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.35f, 0f));
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			component2.iconOffset.y = 0.3f;
		}
		if (base.GetComponent<KPrefabID>().HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
		{
			this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, base.gameObject, this.GetExtents(), GameScenePartitioner.Instance.industrialBuildings, null);
		}
		if (this.Def.Deprecated && base.GetComponent<KSelectable>() != null)
		{
			KSelectable component3 = base.GetComponent<KSelectable>();
			Building.deprecatedBuildingStatusItem = new StatusItem("BUILDING_DEPRECATED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			component3.AddStatusItem(Building.deprecatedBuildingStatusItem, null);
		}
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000C0EDB File Offset: 0x000BF0DB
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000C0EF3 File Offset: 0x000BF0F3
	public virtual void UpdatePosition()
	{
		this.RefreshCells();
		GameScenePartitioner.Instance.UpdatePosition(this.scenePartitionerEntry, this.GetExtents());
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000C0F14 File Offset: 0x000BF114
	protected void RegisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.AddBlock(base.gameObject.layer, this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000C0F9C File Offset: 0x000BF19C
	public CellOffset GetRotatedOffset(CellOffset offset)
	{
		if (!(this.rotatable != null))
		{
			return offset;
		}
		return this.rotatable.GetRotatedCellOffset(offset);
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000C0FBA File Offset: 0x000BF1BA
	private int GetBottomLeftCell()
	{
		return Grid.PosToCell(base.transform.GetPosition());
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000C0FCC File Offset: 0x000BF1CC
	public int GetPowerInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000C0FF8 File Offset: 0x000BF1F8
	public int GetPowerOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000C1024 File Offset: 0x000BF224
	public int GetUtilityInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000C1050 File Offset: 0x000BF250
	public int GetHighEnergyParticleInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000C107C File Offset: 0x000BF27C
	public int GetHighEnergyParticleOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000C10A8 File Offset: 0x000BF2A8
	public int GetUtilityOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000C10D3 File Offset: 0x000BF2D3
	public CellOffset GetUtilityInputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityInputOffset);
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000C10E6 File Offset: 0x000BF2E6
	public CellOffset GetUtilityOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityOutputOffset);
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000C10F9 File Offset: 0x000BF2F9
	public CellOffset GetHighEnergyParticleInputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000C110C File Offset: 0x000BF30C
	public CellOffset GetHighEnergyParticleOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000C1120 File Offset: 0x000BF320
	protected void UnregisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.RemoveBlock(this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000C119D File Offset: 0x000BF39D
	private SimHashes GetVisualizationElementID(PrimaryElement pe)
	{
		if (!(this is BuildingComplete))
		{
			return SimHashes.Void;
		}
		return pe.ElementID;
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000C11B3 File Offset: 0x000BF3B3
	public void RunOnArea(Action<int> callback)
	{
		this.Def.RunOnArea(Grid.PosToCell(this), this.Orientation, callback);
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000C11D0 File Offset: 0x000BF3D0
	public List<Descriptor> RequirementDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		BuildingComplete component = def.BuildingComplete.GetComponent<BuildingComplete>();
		if (def.RequiresPowerInput)
		{
			float wattsNeededWhenActive = component.GetComponent<IEnergyConsumer>().WattsNeededWhenActive;
			if (wattsNeededWhenActive > 0f)
			{
				string formattedWattage = GameUtil.GetFormattedWattage(wattsNeededWhenActive, GameUtil.WattageFormatterUnit.Automatic, true);
				Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESPOWER, formattedWattage), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWER, formattedWattage), Descriptor.DescriptorType.Requirement, false);
				list.Add(item);
			}
		}
		if (def.InputConduitType == ConduitType.Liquid)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item2);
		}
		else if (def.InputConduitType == ConduitType.Gas)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item3);
		}
		if (def.OutputConduitType == ConduitType.Liquid)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item4);
		}
		else if (def.OutputConduitType == ConduitType.Gas)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item5);
		}
		if (component.isManuallyOperated)
		{
			Descriptor item6 = default(Descriptor);
			item6.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESMANUALOPERATION, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESMANUALOPERATION, Descriptor.DescriptorType.Requirement);
			list.Add(item6);
		}
		if (component.isArtable)
		{
			Descriptor item7 = default(Descriptor);
			item7.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESCREATIVITY, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESCREATIVITY, Descriptor.DescriptorType.Requirement);
			list.Add(item7);
		}
		if (def.BuildingUnderConstruction != null)
		{
			Constructable component2 = def.BuildingUnderConstruction.GetComponent<Constructable>();
			if (component2 != null && component2.requiredSkillPerk != HashedString.Invalid)
			{
				StringBuilder stringBuilder = new StringBuilder();
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(component2.requiredSkillPerk);
				for (int i = 0; i < skillsWithPerk.Count; i++)
				{
					Skill skill = skillsWithPerk[i];
					stringBuilder.Append(skill.Name);
					if (i != skillsWithPerk.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				string replacement = stringBuilder.ToString();
				list.Add(new Descriptor(UI.BUILD_REQUIRES_SKILL.Replace("{Skill}", replacement), UI.BUILD_REQUIRES_SKILL_TOOLTIP.Replace("{Skill}", replacement), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000C1470 File Offset: 0x000BF670
	public List<Descriptor> EffectDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (def.EffectDescription != null)
		{
			list.AddRange(def.EffectDescription);
		}
		if (def.GeneratorWattageRating > 0f && base.GetComponent<Battery>() == null)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		if (def.ExhaustKilowattsWhenActive > 0f || def.SelfHeatKilowattsWhenActive > 0f)
		{
			Descriptor item2 = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy((def.ExhaustKilowattsWhenActive + def.SelfHeatKilowattsWhenActive) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item2.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000C1570 File Offset: 0x000BF770
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(this.Def))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors(this.Def))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000C1618 File Offset: 0x000BF818
	public override Vector2 PosMin()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)extents.x, (float)extents.y);
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000C1640 File Offset: 0x000BF840
	public override Vector2 PosMax()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)(extents.x + extents.width), (float)(extents.y + extents.height));
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000C1675 File Offset: 0x000BF875
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000C167C File Offset: 0x000BF87C
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001417 RID: 5143
	public BuildingDef Def;

	// Token: 0x04001418 RID: 5144
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001419 RID: 5145
	[MyCmpAdd]
	private StateMachineController stateMachineController;

	// Token: 0x0400141A RID: 5146
	private int[] placementCells;

	// Token: 0x0400141B RID: 5147
	private Extents extents;

	// Token: 0x0400141C RID: 5148
	private static StatusItem deprecatedBuildingStatusItem;

	// Token: 0x0400141D RID: 5149
	private string description;

	// Token: 0x0400141E RID: 5150
	private string descriptionFlavour;

	// Token: 0x0400141F RID: 5151
	private HandleVector<int>.Handle scenePartitionerEntry;
}

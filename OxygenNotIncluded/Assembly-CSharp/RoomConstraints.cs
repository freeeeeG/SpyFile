using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000946 RID: 2374
public static class RoomConstraints
{
	// Token: 0x0600452C RID: 17708 RVA: 0x00185438 File Offset: 0x00183638
	public static string RoomCriteriaString(Room room)
	{
		string text = "";
		RoomType roomType = room.roomType;
		if (roomType != Db.Get().RoomTypes.Neutral)
		{
			text = text + "<b>" + ROOMS.CRITERIA.HEADER + "</b>";
			text = text + "\n    • " + roomType.primary_constraint.name;
			if (roomType.additional_constraints != null)
			{
				foreach (RoomConstraints.Constraint constraint in roomType.additional_constraints)
				{
					if (constraint.isSatisfied(room))
					{
						text = text + "\n    • " + constraint.name;
					}
					else
					{
						text = text + "\n<color=#F44A47FF>    • " + constraint.name + "</color>";
					}
				}
			}
			return text;
		}
		RoomTypes.RoomTypeQueryResult[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
		text += ((possibleRoomTypes.Length > 1) ? ("<b>" + ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER + "</b>") : "");
		foreach (RoomTypes.RoomTypeQueryResult roomTypeQueryResult in possibleRoomTypes)
		{
			RoomType type = roomTypeQueryResult.Type;
			if (type != Db.Get().RoomTypes.Neutral)
			{
				if (text != "")
				{
					text += "\n";
				}
				text = string.Concat(new string[]
				{
					text,
					"<b><color=#BCBCBC>    • ",
					type.Name,
					"</b> (",
					type.primary_constraint.name,
					")</color>"
				});
				if (roomTypeQueryResult.SatisfactionRating == RoomType.RoomIdentificationResult.all_satisfied)
				{
					bool flag = false;
					RoomTypes.RoomTypeQueryResult[] array2 = possibleRoomTypes;
					for (int j = 0; j < array2.Length; j++)
					{
						RoomType type2 = array2[j].Type;
						if (type2 != type && type2 != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, type, type2))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						text += string.Format("\n<color=#F44A47FF>{0}{1}{2}</color>", "    ", "    • ", ROOMS.CRITERIA.NO_TYPE_CONFLICTS);
					}
				}
				else
				{
					foreach (RoomConstraints.Constraint constraint2 in type.additional_constraints)
					{
						if (!constraint2.isSatisfied(room))
						{
							string str = string.Empty;
							if (constraint2.building_criteria != null)
							{
								str = string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, constraint2.name);
							}
							else
							{
								str = string.Format(ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, constraint2.name);
							}
							text = text + "\n<color=#F44A47FF>        • " + str + "</color>";
						}
					}
				}
			}
		}
		return text;
	}

	// Token: 0x04002DB4 RID: 11700
	public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "4"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "4"), null);

	// Token: 0x04002DB5 RID: 11701
	public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 6, 1, string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.NAME, "6"), string.Format(ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, "6"), null);

	// Token: 0x04002DB6 RID: 11702
	public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 12, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "12"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "12"), null);

	// Token: 0x04002DB7 RID: 11703
	public static RoomConstraints.Constraint MINIMUM_SIZE_24 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 24, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "24"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "24"), null);

	// Token: 0x04002DB8 RID: 11704
	public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells >= 32, 1, string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.NAME, "32"), string.Format(ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, "32"), null);

	// Token: 0x04002DB9 RID: 11705
	public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 64, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "64"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "64"), null);

	// Token: 0x04002DBA RID: 11706
	public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 96, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "96"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "96"), null);

	// Token: 0x04002DBB RID: 11707
	public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint(null, (Room room) => room.cavity.numCells <= 120, 1, string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "120"), string.Format(ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "120"), null);

	// Token: 0x04002DBC RID: 11708
	public static RoomConstraints.Constraint NO_INDUSTRIAL_MACHINERY = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		using (List<KPrefabID>.Enumerator enumerator = room.buildings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
				{
					return false;
				}
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME, ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION, null);

	// Token: 0x04002DBD RID: 11709
	public static RoomConstraints.Constraint NO_COTS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (kprefabID.HasTag(RoomConstraints.ConstraintTags.BedType) && !kprefabID.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION, null);

	// Token: 0x04002DBE RID: 11710
	public static RoomConstraints.Constraint NO_LUXURY_BEDS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		using (List<KPrefabID>.Enumerator enumerator = room.buildings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
				{
					return false;
				}
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_COTS.NAME, ROOMS.CRITERIA.NO_COTS.DESCRIPTION, null);

	// Token: 0x04002DBF RID: 11711
	public static RoomConstraints.Constraint NO_OUTHOUSES = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (kprefabID.HasTag(RoomConstraints.ConstraintTags.ToiletType) && !kprefabID.HasTag(RoomConstraints.ConstraintTags.FlushToiletType))
			{
				return false;
			}
		}
		return true;
	}, 1, ROOMS.CRITERIA.NO_OUTHOUSES.NAME, ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION, null);

	// Token: 0x04002DC0 RID: 11712
	public static RoomConstraints.Constraint NO_MESS_STATION = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < room.buildings.Count)
		{
			flag = room.buildings[num].HasTag(RoomConstraints.ConstraintTags.MessTable);
			num++;
		}
		return !flag;
	}, 1, ROOMS.CRITERIA.NO_MESS_STATION.NAME, ROOMS.CRITERIA.NO_MESS_STATION.DESCRIPTION, null);

	// Token: 0x04002DC1 RID: 11713
	public static RoomConstraints.Constraint HAS_LUXURY_BED = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType), null, 1, ROOMS.CRITERIA.HAS_LUXURY_BED.NAME, ROOMS.CRITERIA.HAS_LUXURY_BED.DESCRIPTION, null);

	// Token: 0x04002DC2 RID: 11714
	public static RoomConstraints.Constraint HAS_BED = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.HAS_BED.NAME, ROOMS.CRITERIA.HAS_BED.DESCRIPTION, null);

	// Token: 0x04002DC3 RID: 11715
	public static RoomConstraints.Constraint SCIENCE_BUILDINGS = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.ScienceBuilding), null, 2, ROOMS.CRITERIA.SCIENCE_BUILDINGS.NAME, ROOMS.CRITERIA.SCIENCE_BUILDINGS.DESCRIPTION, null);

	// Token: 0x04002DC4 RID: 11716
	public static RoomConstraints.Constraint BED_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.BedType) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic), delegate(Room room)
	{
		short num = 0;
		int num2 = 0;
		while (num < 2 && num2 < room.buildings.Count)
		{
			if (room.buildings[num2].HasTag(RoomConstraints.ConstraintTags.BedType))
			{
				num += 1;
			}
			num2++;
		}
		return num == 1;
	}, 1, ROOMS.CRITERIA.BED_SINGLE.NAME, ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION, null);

	// Token: 0x04002DC5 RID: 11717
	public static RoomConstraints.Constraint LUXURY_BED_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBedType), delegate(Room room)
	{
		short num = 0;
		int num2 = 0;
		while (num <= 2 && num2 < room.buildings.Count)
		{
			if (room.buildings[num2].HasTag(RoomConstraints.ConstraintTags.LuxuryBedType))
			{
				num += 1;
			}
			num2++;
		}
		return num == 1;
	}, 1, ROOMS.CRITERIA.LUXURY_BED_SINGLE.NAME, ROOMS.CRITERIA.LUXURY_BED_SINGLE.DESCRIPTION, null);

	// Token: 0x04002DC6 RID: 11718
	public static RoomConstraints.Constraint BUILDING_DECOR_POSITIVE = new RoomConstraints.Constraint(delegate(KPrefabID bc)
	{
		DecorProvider component = bc.GetComponent<DecorProvider>();
		return component != null && component.baseDecor > 0f;
	}, null, 1, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME, ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION, null);

	// Token: 0x04002DC7 RID: 11719
	public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 1, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 1), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 1), null);

	// Token: 0x04002DC8 RID: 11720
	public static RoomConstraints.Constraint DECORATIVE_ITEM_2 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration), null, 2, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.NAME, 2), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION, 2), null);

	// Token: 0x04002DC9 RID: 11721
	public static RoomConstraints.Constraint DECORATIVE_ITEM_SCORE_20 = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(GameTags.Decoration) && bc.HasTag(RoomConstraints.ConstraintTags.Decor20), null, 1, string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM_N.NAME, "20"), string.Format(ROOMS.CRITERIA.DECORATIVE_ITEM_N.DESCRIPTION, "20"), null);

	// Token: 0x04002DCA RID: 11722
	public static RoomConstraints.Constraint POWER_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.PowerStation), null, 1, ROOMS.CRITERIA.POWER_STATION.NAME, ROOMS.CRITERIA.POWER_STATION.DESCRIPTION, null);

	// Token: 0x04002DCB RID: 11723
	public static RoomConstraints.Constraint FARM_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.FarmStationType), null, 1, ROOMS.CRITERIA.FARM_STATION.NAME, ROOMS.CRITERIA.FARM_STATION.DESCRIPTION, null);

	// Token: 0x04002DCC RID: 11724
	public static RoomConstraints.Constraint RANCH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.RanchStationType), null, 1, ROOMS.CRITERIA.RANCH_STATION.NAME, ROOMS.CRITERIA.RANCH_STATION.DESCRIPTION, null);

	// Token: 0x04002DCD RID: 11725
	public static RoomConstraints.Constraint SPICE_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.SpiceStation), null, 1, ROOMS.CRITERIA.SPICE_STATION.NAME, ROOMS.CRITERIA.SPICE_STATION.DESCRIPTION, null);

	// Token: 0x04002DCE RID: 11726
	public static RoomConstraints.Constraint COOK_TOP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.CookTop), null, 1, ROOMS.CRITERIA.COOK_TOP.NAME, ROOMS.CRITERIA.COOK_TOP.DESCRIPTION, null);

	// Token: 0x04002DCF RID: 11727
	public static RoomConstraints.Constraint REFRIGERATOR = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Refrigerator), null, 1, ROOMS.CRITERIA.REFRIGERATOR.NAME, ROOMS.CRITERIA.REFRIGERATOR.DESCRIPTION, null);

	// Token: 0x04002DD0 RID: 11728
	public static RoomConstraints.Constraint REC_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.RecBuilding), null, 1, ROOMS.CRITERIA.REC_BUILDING.NAME, ROOMS.CRITERIA.REC_BUILDING.DESCRIPTION, null);

	// Token: 0x04002DD1 RID: 11729
	public static RoomConstraints.Constraint MACHINE_SHOP = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.MachineShopType), null, 1, ROOMS.CRITERIA.MACHINE_SHOP.NAME, ROOMS.CRITERIA.MACHINE_SHOP.DESCRIPTION, null);

	// Token: 0x04002DD2 RID: 11730
	public static RoomConstraints.Constraint LIGHT = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		foreach (KPrefabID kprefabID in room.cavity.creatures)
		{
			if (kprefabID != null && kprefabID.GetComponent<Light2D>() != null)
			{
				return true;
			}
		}
		foreach (KPrefabID kprefabID2 in room.buildings)
		{
			if (!(kprefabID2 == null))
			{
				Light2D component = kprefabID2.GetComponent<Light2D>();
				if (component != null)
				{
					RequireInputs component2 = kprefabID2.GetComponent<RequireInputs>();
					if (component.enabled || (component2 != null && component2.RequirementsMet))
					{
						return true;
					}
				}
			}
		}
		return false;
	}, 1, ROOMS.CRITERIA.LIGHT.NAME, ROOMS.CRITERIA.LIGHT.DESCRIPTION, null);

	// Token: 0x04002DD3 RID: 11731
	public static RoomConstraints.Constraint DESTRESSING_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.DeStressingBuilding), null, 1, ROOMS.CRITERIA.DESTRESSING_BUILDING.NAME, ROOMS.CRITERIA.DESTRESSING_BUILDING.DESCRIPTION, null);

	// Token: 0x04002DD4 RID: 11732
	public static RoomConstraints.Constraint MASSAGE_TABLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.IsPrefabID(RoomConstraints.ConstraintTags.MassageTable), null, 1, ROOMS.CRITERIA.MASSAGE_TABLE.NAME, ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION, null);

	// Token: 0x04002DD5 RID: 11733
	public static RoomConstraints.Constraint MESS_STATION_SINGLE = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.MessTable), null, 1, ROOMS.CRITERIA.MESS_STATION_SINGLE.NAME, ROOMS.CRITERIA.MESS_STATION_SINGLE.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.REC_BUILDING
	});

	// Token: 0x04002DD6 RID: 11734
	public static RoomConstraints.Constraint TOILET = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.ToiletType), null, 1, ROOMS.CRITERIA.TOILET.NAME, ROOMS.CRITERIA.TOILET.DESCRIPTION, null);

	// Token: 0x04002DD7 RID: 11735
	public static RoomConstraints.Constraint FLUSH_TOILET = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.FlushToiletType), null, 1, ROOMS.CRITERIA.FLUSH_TOILET.NAME, ROOMS.CRITERIA.FLUSH_TOILET.DESCRIPTION, null);

	// Token: 0x04002DD8 RID: 11736
	public static RoomConstraints.Constraint WASH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.WashStation), null, 1, ROOMS.CRITERIA.WASH_STATION.NAME, ROOMS.CRITERIA.WASH_STATION.DESCRIPTION, null);

	// Token: 0x04002DD9 RID: 11737
	public static RoomConstraints.Constraint ADVANCED_WASH_STATION = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.AdvancedWashStation), null, 1, ROOMS.CRITERIA.ADVANCED_WASH_STATION.NAME, ROOMS.CRITERIA.ADVANCED_WASH_STATION.DESCRIPTION, null);

	// Token: 0x04002DDA RID: 11738
	public static RoomConstraints.Constraint CLINIC = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Clinic), null, 1, ROOMS.CRITERIA.CLINIC.NAME, ROOMS.CRITERIA.CLINIC.DESCRIPTION, new List<RoomConstraints.Constraint>
	{
		RoomConstraints.TOILET,
		RoomConstraints.FLUSH_TOILET,
		RoomConstraints.MESS_STATION_SINGLE
	});

	// Token: 0x04002DDB RID: 11739
	public static RoomConstraints.Constraint PARK_BUILDING = new RoomConstraints.Constraint((KPrefabID bc) => bc.HasTag(RoomConstraints.ConstraintTags.Park), null, 1, ROOMS.CRITERIA.PARK_BUILDING.NAME, ROOMS.CRITERIA.PARK_BUILDING.DESCRIPTION, null);

	// Token: 0x04002DDC RID: 11740
	public static RoomConstraints.Constraint ORIGINALTILES = new RoomConstraints.Constraint(null, (Room room) => 1 + room.cavity.maxY - room.cavity.minY >= 4, 1, "", "", null);

	// Token: 0x04002DDD RID: 11741
	public static RoomConstraints.Constraint IS_BACKWALLED = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		bool flag = true;
		int num = (room.cavity.maxX - room.cavity.minX + 1) / 2 + 1;
		int num2 = 0;
		while (flag && num2 < num)
		{
			int x = room.cavity.minX + num2;
			int x2 = room.cavity.maxX - num2;
			int num3 = room.cavity.minY;
			while (flag && num3 <= room.cavity.maxY)
			{
				int cell = Grid.XYToCell(x, num3);
				int cell2 = Grid.XYToCell(x2, num3);
				if (Game.Instance.roomProber.GetCavityForCell(cell) == room.cavity)
				{
					GameObject gameObject = Grid.Objects[cell, 2];
					flag &= (gameObject != null && !gameObject.HasTag(GameTags.UnderConstruction));
				}
				if (Game.Instance.roomProber.GetCavityForCell(cell2) == room.cavity)
				{
					GameObject gameObject2 = Grid.Objects[cell2, 2];
					flag &= (gameObject2 != null && !gameObject2.HasTag(GameTags.UnderConstruction));
				}
				if (!flag)
				{
					return false;
				}
				num3++;
			}
			num2++;
		}
		return flag;
	}, 1, ROOMS.CRITERIA.IS_BACKWALLED.NAME, ROOMS.CRITERIA.IS_BACKWALLED.DESCRIPTION, null);

	// Token: 0x04002DDE RID: 11742
	public static RoomConstraints.Constraint WILDANIMAL = new RoomConstraints.Constraint(null, (Room room) => room.cavity.creatures.Count + room.cavity.eggs.Count > 0, 1, ROOMS.CRITERIA.WILDANIMAL.NAME, ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION, null);

	// Token: 0x04002DDF RID: 11743
	public static RoomConstraints.Constraint WILDANIMALS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		using (List<KPrefabID>.Enumerator enumerator = room.cavity.creatures.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasTag(GameTags.Creatures.Wild))
				{
					num++;
				}
			}
		}
		return num >= 2;
	}, 1, ROOMS.CRITERIA.WILDANIMALS.NAME, ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION, null);

	// Token: 0x04002DE0 RID: 11744
	public static RoomConstraints.Constraint WILDPLANT = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.cavity.plants)
		{
			if (kprefabID != null)
			{
				BasicForagePlantPlanted component = kprefabID.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = kprefabID.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 2;
	}, 1, ROOMS.CRITERIA.WILDPLANT.NAME, ROOMS.CRITERIA.WILDPLANT.DESCRIPTION, null);

	// Token: 0x04002DE1 RID: 11745
	public static RoomConstraints.Constraint WILDPLANTS = new RoomConstraints.Constraint(null, delegate(Room room)
	{
		int num = 0;
		foreach (KPrefabID kprefabID in room.cavity.plants)
		{
			if (kprefabID != null)
			{
				BasicForagePlantPlanted component = kprefabID.GetComponent<BasicForagePlantPlanted>();
				ReceptacleMonitor component2 = kprefabID.GetComponent<ReceptacleMonitor>();
				if (component2 != null && !component2.Replanted)
				{
					num++;
				}
				else if (component != null)
				{
					num++;
				}
			}
		}
		return num >= 4;
	}, 1, ROOMS.CRITERIA.WILDPLANTS.NAME, ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION, null);

	// Token: 0x02001795 RID: 6037
	public static class ConstraintTags
	{
		// Token: 0x04006F37 RID: 28471
		public static Tag BedType = "BedType".ToTag();

		// Token: 0x04006F38 RID: 28472
		public static Tag LuxuryBedType = "LuxuryBedType".ToTag();

		// Token: 0x04006F39 RID: 28473
		public static Tag ToiletType = "ToiletType".ToTag();

		// Token: 0x04006F3A RID: 28474
		public static Tag FlushToiletType = "FlushToiletType".ToTag();

		// Token: 0x04006F3B RID: 28475
		public static Tag MessTable = "MessTable".ToTag();

		// Token: 0x04006F3C RID: 28476
		public static Tag Clinic = "Clinic".ToTag();

		// Token: 0x04006F3D RID: 28477
		public static Tag WashStation = "WashStation".ToTag();

		// Token: 0x04006F3E RID: 28478
		public static Tag AdvancedWashStation = "AdvancedWashStation".ToTag();

		// Token: 0x04006F3F RID: 28479
		public static Tag ScienceBuilding = "ScienceBuilding".ToTag();

		// Token: 0x04006F40 RID: 28480
		public static Tag LightSource = "LightSource".ToTag();

		// Token: 0x04006F41 RID: 28481
		public static Tag MassageTable = "MassageTable".ToTag();

		// Token: 0x04006F42 RID: 28482
		public static Tag DeStressingBuilding = "DeStressingBuilding".ToTag();

		// Token: 0x04006F43 RID: 28483
		public static Tag IndustrialMachinery = "IndustrialMachinery".ToTag();

		// Token: 0x04006F44 RID: 28484
		public static Tag PowerStation = "PowerStation".ToTag();

		// Token: 0x04006F45 RID: 28485
		public static Tag FarmStationType = "FarmStationType".ToTag();

		// Token: 0x04006F46 RID: 28486
		public static Tag CreatureRelocator = "CreatureRelocator".ToTag();

		// Token: 0x04006F47 RID: 28487
		public static Tag RanchStationType = "RanchStationType".ToTag();

		// Token: 0x04006F48 RID: 28488
		public static Tag SpiceStation = "SpiceStation".ToTag();

		// Token: 0x04006F49 RID: 28489
		public static Tag CookTop = "CookTop".ToTag();

		// Token: 0x04006F4A RID: 28490
		public static Tag Refrigerator = "Refrigerator".ToTag();

		// Token: 0x04006F4B RID: 28491
		public static Tag RecBuilding = "RecBuilding".ToTag();

		// Token: 0x04006F4C RID: 28492
		public static Tag MachineShopType = "MachineShopType".ToTag();

		// Token: 0x04006F4D RID: 28493
		public static Tag Park = "Park".ToTag();

		// Token: 0x04006F4E RID: 28494
		public static Tag NatureReserve = "NatureReserve".ToTag();

		// Token: 0x04006F4F RID: 28495
		public static Tag Decor20 = "Decor20".ToTag();

		// Token: 0x04006F50 RID: 28496
		public static Tag RocketInterior = "RocketInterior".ToTag();
	}

	// Token: 0x02001796 RID: 6038
	public class Constraint
	{
		// Token: 0x06008EB2 RID: 36530 RVA: 0x0032006B File Offset: 0x0031E26B
		public Constraint(Func<KPrefabID, bool> building_criteria, Func<Room, bool> room_criteria, int times_required = 1, string name = "", string description = "", List<RoomConstraints.Constraint> stomp_in_conflict = null)
		{
			this.room_criteria = room_criteria;
			this.building_criteria = building_criteria;
			this.times_required = times_required;
			this.name = name;
			this.description = description;
			this.stomp_in_conflict = stomp_in_conflict;
		}

		// Token: 0x06008EB3 RID: 36531 RVA: 0x003200A8 File Offset: 0x0031E2A8
		public bool isSatisfied(Room room)
		{
			int num = 0;
			if (this.room_criteria != null && !this.room_criteria(room))
			{
				return false;
			}
			if (this.building_criteria != null)
			{
				int num2 = 0;
				while (num < this.times_required && num2 < room.buildings.Count)
				{
					KPrefabID kprefabID = room.buildings[num2];
					if (!(kprefabID == null) && this.building_criteria(kprefabID))
					{
						num++;
					}
					num2++;
				}
				int num3 = 0;
				while (num < this.times_required && num3 < room.plants.Count)
				{
					KPrefabID kprefabID2 = room.plants[num3];
					if (!(kprefabID2 == null) && this.building_criteria(kprefabID2))
					{
						num++;
					}
					num3++;
				}
				return num >= this.times_required;
			}
			return true;
		}

		// Token: 0x04006F51 RID: 28497
		public string name;

		// Token: 0x04006F52 RID: 28498
		public string description;

		// Token: 0x04006F53 RID: 28499
		public int times_required = 1;

		// Token: 0x04006F54 RID: 28500
		public Func<Room, bool> room_criteria;

		// Token: 0x04006F55 RID: 28501
		public Func<KPrefabID, bool> building_criteria;

		// Token: 0x04006F56 RID: 28502
		public List<RoomConstraints.Constraint> stomp_in_conflict;
	}
}

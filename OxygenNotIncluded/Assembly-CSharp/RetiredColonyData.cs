using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000938 RID: 2360
public class RetiredColonyData
{
	// Token: 0x06004480 RID: 17536 RVA: 0x00180DF2 File Offset: 0x0017EFF2
	public RetiredColonyData()
	{
	}

	// Token: 0x06004481 RID: 17537 RVA: 0x00180DFC File Offset: 0x0017EFFC
	public RetiredColonyData(string colonyName, int cycleCount, string date, string[] achievements, MinionAssignablesProxy[] minions, BuildingComplete[] buildingCompletes, string startWorld, Dictionary<string, string> worldIdentities)
	{
		this.colonyName = colonyName;
		this.cycleCount = cycleCount;
		this.achievements = achievements;
		this.date = date;
		this.Duplicants = new RetiredColonyData.RetiredDuplicantData[(minions == null) ? 0 : minions.Length];
		int i = 0;
		while (i < this.Duplicants.Length)
		{
			this.Duplicants[i] = new RetiredColonyData.RetiredDuplicantData();
			this.Duplicants[i].name = minions[i].GetProperName();
			this.Duplicants[i].age = (int)Mathf.Floor((float)GameClock.Instance.GetCycle() - minions[i].GetArrivalTime());
			this.Duplicants[i].skillPointsGained = minions[i].GetTotalSkillpoints();
			this.Duplicants[i].accessories = new Dictionary<string, string>();
			if (minions[i].GetTargetGameObject().GetComponent<Accessorizer>() != null)
			{
				using (List<ResourceRef<Accessory>>.Enumerator enumerator = minions[i].GetTargetGameObject().GetComponent<Accessorizer>().GetAccessories().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResourceRef<Accessory> resourceRef = enumerator.Current;
						if (resourceRef.Get() != null)
						{
							this.Duplicants[i].accessories.Add(resourceRef.Get().slot.Id, resourceRef.Get().Id);
						}
					}
					goto IL_3AF;
				}
				goto IL_14E;
			}
			goto IL_14E;
			IL_3AF:
			i++;
			continue;
			IL_14E:
			StoredMinionIdentity component = minions[i].GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Eyes.Id, Db.Get().Accessories.Get(component.bodyData.eyes).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Arm.Id, Db.Get().Accessories.Get(component.bodyData.arms).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.ArmLower.Id, Db.Get().Accessories.Get(component.bodyData.armslower).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Body.Id, Db.Get().Accessories.Get(component.bodyData.body).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hair.Id, Db.Get().Accessories.Get(component.bodyData.hair).Id);
			if (component.bodyData.hat != HashedString.Invalid)
			{
				this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Hat.Id, Db.Get().Accessories.Get(component.bodyData.hat).Id);
			}
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.HeadShape.Id, Db.Get().Accessories.Get(component.bodyData.headShape).Id);
			this.Duplicants[i].accessories.Add(Db.Get().AccessorySlots.Mouth.Id, Db.Get().Accessories.Get(component.bodyData.mouth).Id);
			goto IL_3AF;
		}
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		if (buildingCompletes != null)
		{
			foreach (BuildingComplete cmp in buildingCompletes)
			{
				if (!dictionary.ContainsKey(cmp.PrefabID()))
				{
					dictionary[cmp.PrefabID()] = 0;
				}
				Dictionary<Tag, int> dictionary2 = dictionary;
				Tag key = cmp.PrefabID();
				int num = dictionary2[key];
				dictionary2[key] = num + 1;
			}
		}
		this.buildings = new List<global::Tuple<string, int>>();
		foreach (KeyValuePair<Tag, int> keyValuePair in dictionary)
		{
			this.buildings.Add(new global::Tuple<string, int>(keyValuePair.Key.ToString(), keyValuePair.Value));
		}
		this.Stats = null;
		if (ReportManager.Instance != null)
		{
			global::Tuple<float, float>[] array = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[k].day, ReportManager.Instance.reports[k].GetEntry(ReportManager.ReportType.OxygenCreated).accPositive);
			}
			global::Tuple<float, float>[] array2 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[l].day, ReportManager.Instance.reports[l].GetEntry(ReportManager.ReportType.OxygenCreated).accNegative * -1f);
			}
			global::Tuple<float, float>[] array3 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int m = 0; m < array3.Length; m++)
			{
				array3[m] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[m].day, ReportManager.Instance.reports[m].GetEntry(ReportManager.ReportType.CaloriesCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array4 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int n = 0; n < array4.Length; n++)
			{
				array4[n] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[n].day, ReportManager.Instance.reports[n].GetEntry(ReportManager.ReportType.CaloriesCreated).accNegative * 0.001f * -1f);
			}
			global::Tuple<float, float>[] array5 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num2 = 0; num2 < array5.Length; num2++)
			{
				array5[num2] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num2].day, ReportManager.Instance.reports[num2].GetEntry(ReportManager.ReportType.EnergyCreated).accPositive * 0.001f);
			}
			global::Tuple<float, float>[] array6 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num3 = 0; num3 < array6.Length; num3++)
			{
				array6[num3] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num3].day, ReportManager.Instance.reports[num3].GetEntry(ReportManager.ReportType.EnergyWasted).accNegative * -1f * 0.001f);
			}
			global::Tuple<float, float>[] array7 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num4 = 0; num4 < array7.Length; num4++)
			{
				array7[num4] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num4].day, ReportManager.Instance.reports[num4].GetEntry(ReportManager.ReportType.WorkTime).accPositive);
			}
			global::Tuple<float, float>[] array8 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num5 = 0; num5 < array7.Length; num5++)
			{
				int num6 = 0;
				float num7 = 0f;
				ReportManager.ReportEntry entry = ReportManager.Instance.reports[num5].GetEntry(ReportManager.ReportType.WorkTime);
				for (int num8 = 0; num8 < entry.contextEntries.Count; num8++)
				{
					num6++;
					num7 += entry.contextEntries[num8].accPositive;
				}
				num7 /= (float)num6;
				num7 /= 600f;
				num7 *= 100f;
				array8[num5] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num5].day, num7);
			}
			global::Tuple<float, float>[] array9 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num9 = 0; num9 < array9.Length; num9++)
			{
				array9[num9] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num9].day, ReportManager.Instance.reports[num9].GetEntry(ReportManager.ReportType.TravelTime).accPositive);
			}
			global::Tuple<float, float>[] array10 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num10 = 0; num10 < array9.Length; num10++)
			{
				int num11 = 0;
				float num12 = 0f;
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[num10].GetEntry(ReportManager.ReportType.TravelTime);
				for (int num13 = 0; num13 < entry2.contextEntries.Count; num13++)
				{
					num11++;
					num12 += entry2.contextEntries[num13].accPositive;
				}
				num12 /= (float)num11;
				num12 /= 600f;
				num12 *= 100f;
				array10[num10] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num10].day, num12);
			}
			global::Tuple<float, float>[] array11 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num14 = 0; num14 < array7.Length; num14++)
			{
				array11[num14] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num14].day, (float)ReportManager.Instance.reports[num14].GetEntry(ReportManager.ReportType.WorkTime).contextEntries.Count);
			}
			global::Tuple<float, float>[] array12 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num15 = 0; num15 < array12.Length; num15++)
			{
				int num16 = 0;
				float num17 = 0f;
				ReportManager.ReportEntry entry3 = ReportManager.Instance.reports[num15].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num18 = 0; num18 < entry3.contextEntries.Count; num18++)
				{
					num16++;
					num17 += entry3.contextEntries[num18].accPositive;
				}
				array12[num15] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num15].day, num17 / (float)num16);
			}
			global::Tuple<float, float>[] array13 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num19 = 0; num19 < array13.Length; num19++)
			{
				int num20 = 0;
				float num21 = 0f;
				ReportManager.ReportEntry entry4 = ReportManager.Instance.reports[num19].GetEntry(ReportManager.ReportType.StressDelta);
				for (int num22 = 0; num22 < entry4.contextEntries.Count; num22++)
				{
					num20++;
					num21 += entry4.contextEntries[num22].accNegative;
				}
				num21 *= -1f;
				array13[num19] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num19].day, num21 / (float)num20);
			}
			global::Tuple<float, float>[] array14 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num23 = 0; num23 < array14.Length; num23++)
			{
				array14[num23] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num23].day, ReportManager.Instance.reports[num23].GetEntry(ReportManager.ReportType.DomesticatedCritters).accPositive);
			}
			global::Tuple<float, float>[] array15 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num24 = 0; num24 < array15.Length; num24++)
			{
				array15[num24] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num24].day, ReportManager.Instance.reports[num24].GetEntry(ReportManager.ReportType.WildCritters).accPositive);
			}
			global::Tuple<float, float>[] array16 = new global::Tuple<float, float>[ReportManager.Instance.reports.Count];
			for (int num25 = 0; num25 < array16.Length; num25++)
			{
				array16[num25] = new global::Tuple<float, float>((float)ReportManager.Instance.reports[num25].day, ReportManager.Instance.reports[num25].GetEntry(ReportManager.ReportType.RocketsInFlight).accPositive);
			}
			this.Stats = new RetiredColonyData.RetiredColonyStatistic[]
			{
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenProduced, array, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.OxygenConsumed, array2, UI.RETIRED_COLONY_INFO_SCREEN.STATS.OXYGEN_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.MASS.KILOGRAM),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesProduced, array3, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.CaloriesRemoved, array4, UI.RETIRED_COLONY_INFO_SCREEN.STATS.CALORIES_CONSUMED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CALORIES.KILOCALORIE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerProduced, array5, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.PowerWasted, array6, UI.RETIRED_COLONY_INFO_SCREEN.STATS.POWER_WASTED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WorkTime, array7, UI.RETIRED_COLONY_INFO_SCREEN.STATS.WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageWorkTime, array8, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_WORK_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.TravelTime, array9, UI.RETIRED_COLONY_INFO_SCREEN.STATS.TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.SECONDS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageTravelTime, array10, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_TRAVEL_TIME, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.LiveDuplicants, array11, UI.RETIRED_COLONY_INFO_SCREEN.STATS.LIVE_DUPLICANTS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.DUPLICANTS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.RocketsInFlight, array16, UI.RETIRED_COLONY_INFO_SCREEN.STATS.ROCKET_MISSIONS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.ROCKET_MISSIONS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressCreated, array12, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_CREATED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.AverageStressRemoved, array13, UI.RETIRED_COLONY_INFO_SCREEN.STATS.AVERAGE_STRESS_REMOVED, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.PERCENT),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.DomesticatedCritters, array14, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_DOMESTICATED_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS),
				new RetiredColonyData.RetiredColonyStatistic(RetiredColonyData.DataIDs.WildCritters, array15, UI.RETIRED_COLONY_INFO_SCREEN.STATS.NUMBER_WILD_CRITTERS, UI.MATH_PICTURES.AXIS_LABELS.CYCLES, UI.UNITSUFFIXES.CRITTERS)
			};
			this.startWorld = startWorld;
			this.worldIdentities = worldIdentities;
		}
	}

	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06004482 RID: 17538 RVA: 0x00181DE4 File Offset: 0x0017FFE4
	// (set) Token: 0x06004483 RID: 17539 RVA: 0x00181DEC File Offset: 0x0017FFEC
	public string colonyName { get; set; }

	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06004484 RID: 17540 RVA: 0x00181DF5 File Offset: 0x0017FFF5
	// (set) Token: 0x06004485 RID: 17541 RVA: 0x00181DFD File Offset: 0x0017FFFD
	public int cycleCount { get; set; }

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06004486 RID: 17542 RVA: 0x00181E06 File Offset: 0x00180006
	// (set) Token: 0x06004487 RID: 17543 RVA: 0x00181E0E File Offset: 0x0018000E
	public string date { get; set; }

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06004488 RID: 17544 RVA: 0x00181E17 File Offset: 0x00180017
	// (set) Token: 0x06004489 RID: 17545 RVA: 0x00181E1F File Offset: 0x0018001F
	public string[] achievements { get; set; }

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x0600448A RID: 17546 RVA: 0x00181E28 File Offset: 0x00180028
	// (set) Token: 0x0600448B RID: 17547 RVA: 0x00181E30 File Offset: 0x00180030
	public RetiredColonyData.RetiredDuplicantData[] Duplicants { get; set; }

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x0600448C RID: 17548 RVA: 0x00181E39 File Offset: 0x00180039
	// (set) Token: 0x0600448D RID: 17549 RVA: 0x00181E41 File Offset: 0x00180041
	public List<global::Tuple<string, int>> buildings { get; set; }

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x0600448E RID: 17550 RVA: 0x00181E4A File Offset: 0x0018004A
	// (set) Token: 0x0600448F RID: 17551 RVA: 0x00181E52 File Offset: 0x00180052
	public RetiredColonyData.RetiredColonyStatistic[] Stats { get; set; }

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06004490 RID: 17552 RVA: 0x00181E5B File Offset: 0x0018005B
	// (set) Token: 0x06004491 RID: 17553 RVA: 0x00181E63 File Offset: 0x00180063
	public Dictionary<string, string> worldIdentities { get; set; }

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06004492 RID: 17554 RVA: 0x00181E6C File Offset: 0x0018006C
	// (set) Token: 0x06004493 RID: 17555 RVA: 0x00181E74 File Offset: 0x00180074
	public string startWorld { get; set; }

	// Token: 0x02001775 RID: 6005
	public static class DataIDs
	{
		// Token: 0x04006ECC RID: 28364
		public static string OxygenProduced = "oxygenProduced";

		// Token: 0x04006ECD RID: 28365
		public static string OxygenConsumed = "oxygenConsumed";

		// Token: 0x04006ECE RID: 28366
		public static string CaloriesProduced = "caloriesProduced";

		// Token: 0x04006ECF RID: 28367
		public static string CaloriesRemoved = "caloriesRemoved";

		// Token: 0x04006ED0 RID: 28368
		public static string PowerProduced = "powerProduced";

		// Token: 0x04006ED1 RID: 28369
		public static string PowerWasted = "powerWasted";

		// Token: 0x04006ED2 RID: 28370
		public static string WorkTime = "workTime";

		// Token: 0x04006ED3 RID: 28371
		public static string TravelTime = "travelTime";

		// Token: 0x04006ED4 RID: 28372
		public static string AverageWorkTime = "averageWorkTime";

		// Token: 0x04006ED5 RID: 28373
		public static string AverageTravelTime = "averageTravelTime";

		// Token: 0x04006ED6 RID: 28374
		public static string LiveDuplicants = "liveDuplicants";

		// Token: 0x04006ED7 RID: 28375
		public static string AverageStressCreated = "averageStressCreated";

		// Token: 0x04006ED8 RID: 28376
		public static string AverageStressRemoved = "averageStressRemoved";

		// Token: 0x04006ED9 RID: 28377
		public static string DomesticatedCritters = "domesticatedCritters";

		// Token: 0x04006EDA RID: 28378
		public static string WildCritters = "wildCritters";

		// Token: 0x04006EDB RID: 28379
		public static string AverageGerms = "averageGerms";

		// Token: 0x04006EDC RID: 28380
		public static string RocketsInFlight = "rocketsInFlight";
	}

	// Token: 0x02001776 RID: 6006
	public class RetiredColonyStatistic
	{
		// Token: 0x06008E5B RID: 36443 RVA: 0x0031F347 File Offset: 0x0031D547
		public RetiredColonyStatistic()
		{
		}

		// Token: 0x06008E5C RID: 36444 RVA: 0x0031F34F File Offset: 0x0031D54F
		public RetiredColonyStatistic(string id, global::Tuple<float, float>[] data, string name, string axisNameX, string axisNameY)
		{
			this.id = id;
			this.value = data;
			this.name = name;
			this.nameX = axisNameX;
			this.nameY = axisNameY;
		}

		// Token: 0x06008E5D RID: 36445 RVA: 0x0031F37C File Offset: 0x0031D57C
		public global::Tuple<float, float> GetByMaxValue()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].second > num2)
				{
					num2 = this.value[i].second;
					num = i;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			return this.value[num];
		}

		// Token: 0x06008E5E RID: 36446 RVA: 0x0031F3EC File Offset: 0x0031D5EC
		public global::Tuple<float, float> GetByMaxKey()
		{
			if (this.value.Length == 0)
			{
				return new global::Tuple<float, float>(0f, 0f);
			}
			int num = -1;
			float num2 = -1f;
			for (int i = 0; i < this.value.Length; i++)
			{
				if (this.value[i].first > num2)
				{
					num2 = this.value[i].first;
					num = i;
				}
			}
			return this.value[num];
		}

		// Token: 0x04006EDD RID: 28381
		public string id;

		// Token: 0x04006EDE RID: 28382
		public global::Tuple<float, float>[] value;

		// Token: 0x04006EDF RID: 28383
		public string name;

		// Token: 0x04006EE0 RID: 28384
		public string nameX;

		// Token: 0x04006EE1 RID: 28385
		public string nameY;
	}

	// Token: 0x02001777 RID: 6007
	public class RetiredDuplicantData
	{
		// Token: 0x04006EE2 RID: 28386
		public string name;

		// Token: 0x04006EE3 RID: 28387
		public int age;

		// Token: 0x04006EE4 RID: 28388
		public int skillPointsGained;

		// Token: 0x04006EE5 RID: 28389
		public Dictionary<string, string> accessories;
	}
}

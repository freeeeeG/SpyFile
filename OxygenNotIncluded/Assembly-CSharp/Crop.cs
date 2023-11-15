using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000714 RID: 1812
[AddComponentMenu("KMonoBehaviour/scripts/Crop")]
public class Crop : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x060031C8 RID: 12744 RVA: 0x00108781 File Offset: 0x00106981
	public string cropId
	{
		get
		{
			return this.cropVal.cropId;
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x060031C9 RID: 12745 RVA: 0x0010878E File Offset: 0x0010698E
	// (set) Token: 0x060031CA RID: 12746 RVA: 0x00108796 File Offset: 0x00106996
	public Storage PlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
		set
		{
			this.planterStorage = value;
		}
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x0010879F File Offset: 0x0010699F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Crops.Add(this);
		this.yield = this.GetAttributes().Add(Db.Get().PlantAttributes.YieldAmount);
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x001087D2 File Offset: 0x001069D2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Crop>(1272413801, Crop.OnHarvestDelegate);
	}

	// Token: 0x060031CD RID: 12749 RVA: 0x001087EB File Offset: 0x001069EB
	public void Configure(Crop.CropVal cropval)
	{
		this.cropVal = cropval;
	}

	// Token: 0x060031CE RID: 12750 RVA: 0x001087F4 File Offset: 0x001069F4
	public bool CanGrow()
	{
		return this.cropVal.renewable;
	}

	// Token: 0x060031CF RID: 12751 RVA: 0x00108804 File Offset: 0x00106A04
	public void SpawnConfiguredFruit(object callbackParam)
	{
		if (this == null)
		{
			return;
		}
		Crop.CropVal cropVal = this.cropVal;
		if (!string.IsNullOrEmpty(cropVal.cropId))
		{
			this.SpawnSomeFruit(cropVal.cropId, this.yield.GetTotalValue());
			base.Trigger(-1072826864, this);
		}
	}

	// Token: 0x060031D0 RID: 12752 RVA: 0x00108858 File Offset: 0x00106A58
	public void SpawnSomeFruit(Tag cropID, float amount)
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(cropID), base.transform.GetPosition() + new Vector3(0f, 0.75f, 0f), Grid.SceneLayer.Ore, null, 0);
		if (gameObject != null)
		{
			MutantPlant component = base.GetComponent<MutantPlant>();
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component != null && component.IsOriginal && component2 != null && base.GetComponent<SeedProducer>().RollForMutation())
			{
				component2.Mutate();
			}
			gameObject.SetActive(true);
			PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
			component3.Units = amount;
			component3.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			base.Trigger(35625290, gameObject);
			Edible component4 = gameObject.GetComponent<Edible>();
			if (component4)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component4.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.HARVESTED, "{0}", component4.GetProperName()), UI.ENDOFDAYREPORT.NOTES.HARVESTED_CONTEXT);
				return;
			}
		}
		else
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"tried to spawn an invalid crop prefab:",
				cropID
			});
		}
	}

	// Token: 0x060031D1 RID: 12753 RVA: 0x0010897C File Offset: 0x00106B7C
	protected override void OnCleanUp()
	{
		Components.Crops.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x0010898F File Offset: 0x00106B8F
	private void OnHarvest(object obj)
	{
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x00108991 File Offset: 0x00106B91
	public List<Descriptor> RequirementDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x00108998 File Offset: 0x00106B98
	public List<Descriptor> InformationDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Tag tag = new Tag(this.cropVal.cropId);
		GameObject prefab = Assets.GetPrefab(tag);
		Edible component = prefab.GetComponent<Edible>();
		Klei.AI.Attribute yieldAmount = Db.Get().PlantAttributes.YieldAmount;
		float preModifiedAttributeValue = go.GetComponent<Modifiers>().GetPreModifiedAttributeValue(yieldAmount);
		if (component != null)
		{
			DebugUtil.Assert(GameTags.DisplayAsCalories.Contains(tag), "Trying to display crop info for an edible fruit which isn't displayed as calories!", tag.ToString());
			float caloriesPerUnit = component.FoodInfo.CaloriesPerUnit;
			float calories = caloriesPerUnit * preModifiedAttributeValue;
			string text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
			Descriptor item = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD, "", GameUtil.GetFormattedCalories(caloriesPerUnit, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else
		{
			string text;
			if (GameTags.DisplayAsUnits.Contains(tag))
			{
				text = GameUtil.GetFormattedUnits((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, false, "");
			}
			else
			{
				text = GameUtil.GetFormattedMass((float)this.cropVal.numProduced, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			}
			Descriptor item2 = new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.YIELD_NONFOOD, prefab.GetProperName(), text), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.YIELD_NONFOOD, text), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x00108B08 File Offset: 0x00106D08
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(go))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.InformationDescriptors(go))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04001DCE RID: 7630
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001DCF RID: 7631
	public Crop.CropVal cropVal;

	// Token: 0x04001DD0 RID: 7632
	private AttributeInstance yield;

	// Token: 0x04001DD1 RID: 7633
	public string domesticatedDesc = "";

	// Token: 0x04001DD2 RID: 7634
	private Storage planterStorage;

	// Token: 0x04001DD3 RID: 7635
	private static readonly EventSystem.IntraObjectHandler<Crop> OnHarvestDelegate = new EventSystem.IntraObjectHandler<Crop>(delegate(Crop component, object data)
	{
		component.OnHarvest(data);
	});

	// Token: 0x02001477 RID: 5239
	[Serializable]
	public struct CropVal
	{
		// Token: 0x060084B6 RID: 33974 RVA: 0x003030F1 File Offset: 0x003012F1
		public CropVal(string crop_id, float crop_duration, int num_produced = 1, bool renewable = true)
		{
			this.cropId = crop_id;
			this.cropDuration = crop_duration;
			this.numProduced = num_produced;
			this.renewable = renewable;
		}

		// Token: 0x0400657E RID: 25982
		public string cropId;

		// Token: 0x0400657F RID: 25983
		public float cropDuration;

		// Token: 0x04006580 RID: 25984
		public int numProduced;

		// Token: 0x04006581 RID: 25985
		public bool renewable;
	}
}

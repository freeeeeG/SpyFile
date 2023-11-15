using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000902 RID: 2306
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/RationTracker")]
public class RationTracker : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060042D3 RID: 17107 RVA: 0x00175BD0 File Offset: 0x00173DD0
	public static void DestroyInstance()
	{
		RationTracker.instance = null;
	}

	// Token: 0x060042D4 RID: 17108 RVA: 0x00175BD8 File Offset: 0x00173DD8
	public static RationTracker Get()
	{
		return RationTracker.instance;
	}

	// Token: 0x060042D5 RID: 17109 RVA: 0x00175BDF File Offset: 0x00173DDF
	protected override void OnPrefabInit()
	{
		RationTracker.instance = this;
	}

	// Token: 0x060042D6 RID: 17110 RVA: 0x00175BE7 File Offset: 0x00173DE7
	protected override void OnSpawn()
	{
		base.Subscribe<RationTracker>(631075836, RationTracker.OnNewDayDelegate);
	}

	// Token: 0x060042D7 RID: 17111 RVA: 0x00175BFA File Offset: 0x00173DFA
	private void OnNewDay(object data)
	{
		this.previousFrame = this.currentFrame;
		this.currentFrame = default(RationTracker.Frame);
	}

	// Token: 0x060042D8 RID: 17112 RVA: 0x00175C14 File Offset: 0x00173E14
	public float CountRations(Dictionary<string, float> unitCountByFoodType, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(GameTags.Edible, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					Edible component = pickupable.GetComponent<Edible>();
					num += component.Calories;
					if (unitCountByFoodType != null)
					{
						if (!unitCountByFoodType.ContainsKey(component.FoodID))
						{
							unitCountByFoodType[component.FoodID] = 0f;
						}
						string foodID = component.FoodID;
						unitCountByFoodType[foodID] += component.Units;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x00175CE0 File Offset: 0x00173EE0
	public float CountRationsByFoodType(string foodID, WorldInventory inventory, bool excludeUnreachable = true)
	{
		float num = 0f;
		ICollection<Pickupable> pickupables = inventory.GetPickupables(GameTags.Edible, false);
		if (pickupables != null)
		{
			foreach (Pickupable pickupable in pickupables)
			{
				if (!pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
				{
					Edible component = pickupable.GetComponent<Edible>();
					if (component.FoodID == foodID)
					{
						num += component.Calories;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x00175D6C File Offset: 0x00173F6C
	public void RegisterCaloriesProduced(float calories)
	{
		this.currentFrame.caloriesProduced = this.currentFrame.caloriesProduced + calories;
	}

	// Token: 0x060042DB RID: 17115 RVA: 0x00175D80 File Offset: 0x00173F80
	public void RegisterRationsConsumed(Edible edible)
	{
		this.currentFrame.caloriesConsumed = this.currentFrame.caloriesConsumed + edible.caloriesConsumed;
		if (!this.caloriesConsumedByFood.ContainsKey(edible.FoodInfo.Id))
		{
			this.caloriesConsumedByFood.Add(edible.FoodInfo.Id, edible.caloriesConsumed);
			return;
		}
		Dictionary<string, float> dictionary = this.caloriesConsumedByFood;
		string id = edible.FoodInfo.Id;
		dictionary[id] += edible.caloriesConsumed;
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x00175E00 File Offset: 0x00174000
	public float GetCaloiresConsumedByFood(List<string> foodTypes)
	{
		float num = 0f;
		foreach (string key in foodTypes)
		{
			if (this.caloriesConsumedByFood.ContainsKey(key))
			{
				num += this.caloriesConsumedByFood[key];
			}
		}
		return num;
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x00175E6C File Offset: 0x0017406C
	public float GetCaloriesConsumed()
	{
		float num = 0f;
		foreach (KeyValuePair<string, float> keyValuePair in this.caloriesConsumedByFood)
		{
			num += keyValuePair.Value;
		}
		return num;
	}

	// Token: 0x04002B97 RID: 11159
	private static RationTracker instance;

	// Token: 0x04002B98 RID: 11160
	[Serialize]
	public RationTracker.Frame currentFrame;

	// Token: 0x04002B99 RID: 11161
	[Serialize]
	public RationTracker.Frame previousFrame;

	// Token: 0x04002B9A RID: 11162
	[Serialize]
	public Dictionary<string, float> caloriesConsumedByFood = new Dictionary<string, float>();

	// Token: 0x04002B9B RID: 11163
	private static readonly EventSystem.IntraObjectHandler<RationTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<RationTracker>(delegate(RationTracker component, object data)
	{
		component.OnNewDay(data);
	});

	// Token: 0x02001756 RID: 5974
	public struct Frame
	{
		// Token: 0x04006E6C RID: 28268
		public float caloriesProduced;

		// Token: 0x04006E6D RID: 28269
		public float caloriesConsumed;
	}
}

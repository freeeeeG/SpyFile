using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200077B RID: 1915
[AddComponentMenu("KMonoBehaviour/scripts/EdiblesManager")]
public class EdiblesManager : KMonoBehaviour
{
	// Token: 0x06003508 RID: 13576 RVA: 0x0011FAB8 File Offset: 0x0011DCB8
	public static List<EdiblesManager.FoodInfo> GetAllFoodTypes()
	{
		return (from x in EdiblesManager.s_allFoodTypes
		where DlcManager.IsContentActive(x.DlcId)
		select x).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x0011FAE8 File Offset: 0x0011DCE8
	public static EdiblesManager.FoodInfo GetFoodInfo(string foodID)
	{
		string key = foodID.Replace("Compost", "");
		EdiblesManager.FoodInfo result = null;
		EdiblesManager.s_allFoodMap.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x0011FB17 File Offset: 0x0011DD17
	public static bool TryGetFoodInfo(string foodID, out EdiblesManager.FoodInfo info)
	{
		info = null;
		if (string.IsNullOrEmpty(foodID))
		{
			return false;
		}
		info = EdiblesManager.GetFoodInfo(foodID);
		return info != null;
	}

	// Token: 0x0400201B RID: 8219
	private static List<EdiblesManager.FoodInfo> s_allFoodTypes = new List<EdiblesManager.FoodInfo>();

	// Token: 0x0400201C RID: 8220
	private static Dictionary<string, EdiblesManager.FoodInfo> s_allFoodMap = new Dictionary<string, EdiblesManager.FoodInfo>();

	// Token: 0x02001504 RID: 5380
	public class FoodInfo : IConsumableUIItem
	{
		// Token: 0x060086A4 RID: 34468 RVA: 0x00308FA0 File Offset: 0x003071A0
		public FoodInfo(string id, string dlcId, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot)
		{
			this.Id = id;
			this.DlcId = dlcId;
			this.CaloriesPerUnit = caloriesPerUnit;
			this.Quality = quality;
			this.PreserveTemperature = preserveTemperatue;
			this.RotTemperature = rotTemperature;
			this.StaleTime = spoilTime / 2f;
			this.SpoilTime = spoilTime;
			this.CanRot = can_rot;
			this.Name = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".NAME");
			this.Description = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".DESC");
			this.Effects = new List<string>();
			EdiblesManager.s_allFoodTypes.Add(this);
			EdiblesManager.s_allFoodMap[this.Id] = this;
		}

		// Token: 0x060086A5 RID: 34469 RVA: 0x0030906F File Offset: 0x0030726F
		public EdiblesManager.FoodInfo AddEffects(List<string> effects, string[] dlcIds)
		{
			if (DlcManager.IsDlcListValidForCurrentContent(dlcIds))
			{
				this.Effects.AddRange(effects);
			}
			return this;
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060086A6 RID: 34470 RVA: 0x00309086 File Offset: 0x00307286
		public string ConsumableId
		{
			get
			{
				return this.Id;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060086A7 RID: 34471 RVA: 0x0030908E File Offset: 0x0030728E
		public string ConsumableName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060086A8 RID: 34472 RVA: 0x00309096 File Offset: 0x00307296
		public int MajorOrder
		{
			get
			{
				return this.Quality;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060086A9 RID: 34473 RVA: 0x0030909E File Offset: 0x0030729E
		public int MinorOrder
		{
			get
			{
				return (int)this.CaloriesPerUnit;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060086AA RID: 34474 RVA: 0x003090A7 File Offset: 0x003072A7
		public bool Display
		{
			get
			{
				return this.CaloriesPerUnit != 0f;
			}
		}

		// Token: 0x04006705 RID: 26373
		public string Id;

		// Token: 0x04006706 RID: 26374
		public string DlcId;

		// Token: 0x04006707 RID: 26375
		public string Name;

		// Token: 0x04006708 RID: 26376
		public string Description;

		// Token: 0x04006709 RID: 26377
		public float CaloriesPerUnit;

		// Token: 0x0400670A RID: 26378
		public float PreserveTemperature;

		// Token: 0x0400670B RID: 26379
		public float RotTemperature;

		// Token: 0x0400670C RID: 26380
		public float StaleTime;

		// Token: 0x0400670D RID: 26381
		public float SpoilTime;

		// Token: 0x0400670E RID: 26382
		public bool CanRot;

		// Token: 0x0400670F RID: 26383
		public int Quality;

		// Token: 0x04006710 RID: 26384
		public List<string> Effects;
	}
}

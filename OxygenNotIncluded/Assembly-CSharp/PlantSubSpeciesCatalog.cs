using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008E6 RID: 2278
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantSubSpeciesCatalog : KMonoBehaviour
{
	// Token: 0x060041D4 RID: 16852 RVA: 0x00170316 File Offset: 0x0016E516
	public static void DestroyInstance()
	{
		PlantSubSpeciesCatalog.Instance = null;
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x060041D5 RID: 16853 RVA: 0x00170320 File Offset: 0x0016E520
	public bool AnyNonOriginalDiscovered
	{
		get
		{
			foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
			{
				if (keyValuePair.Value.Find((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !ss.IsOriginal).IsValid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x001703A8 File Offset: 0x0016E5A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PlantSubSpeciesCatalog.Instance = this;
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x001703B6 File Offset: 0x0016E5B6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.EnsureOriginalSubSpecies();
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x001703C4 File Offset: 0x0016E5C4
	public List<Tag> GetAllDiscoveredSpecies()
	{
		return this.discoveredSubspeciesBySpecies.Keys.ToList<Tag>();
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x001703D8 File Offset: 0x0016E5D8
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllSubSpeciesForSpecies(Tag speciesID)
	{
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> result;
		if (this.discoveredSubspeciesBySpecies.TryGetValue(speciesID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x001703F8 File Offset: 0x0016E5F8
	public bool GetOriginalSubSpecies(Tag speciesID, out PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo)
	{
		if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
		{
			subSpeciesInfo = default(PlantSubSpeciesCatalog.SubSpeciesInfo);
			return false;
		}
		subSpeciesInfo = this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.IsOriginal);
		return true;
	}

	// Token: 0x060041DB RID: 16859 RVA: 0x00170454 File Offset: 0x0016E654
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpecies(Tag speciesID, Tag subSpeciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID);
	}

	// Token: 0x060041DC RID: 16860 RVA: 0x0017048C File Offset: 0x0016E68C
	public PlantSubSpeciesCatalog.SubSpeciesInfo FindSubSpecies(Tag subSpeciesID)
	{
		Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> <>9__0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
		{
			List<PlantSubSpeciesCatalog.SubSpeciesInfo> value = keyValuePair.Value;
			Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> match;
			if ((match = <>9__0) == null)
			{
				match = (<>9__0 = ((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID));
			}
			PlantSubSpeciesCatalog.SubSpeciesInfo result = value.Find(match);
			if (result.ID.IsValid)
			{
				return result;
			}
		}
		return default(PlantSubSpeciesCatalog.SubSpeciesInfo);
	}

	// Token: 0x060041DD RID: 16861 RVA: 0x00170534 File Offset: 0x0016E734
	public void DiscoverSubSpecies(PlantSubSpeciesCatalog.SubSpeciesInfo newSubSpeciesInfo, MutantPlant source)
	{
		if (!this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Contains(newSubSpeciesInfo))
		{
			this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Add(newSubSpeciesInfo);
			Notification notification = new Notification(MISC.NOTIFICATIONS.NEWMUTANTSEED.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(this.NewSubspeciesTooltipCB), newSubSpeciesInfo, true, 0f, null, null, source.transform, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x060041DE RID: 16862 RVA: 0x001705BC File Offset: 0x0016E7BC
	private string NewSubspeciesTooltipCB(List<Notification> notifications, object data)
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = (PlantSubSpeciesCatalog.SubSpeciesInfo)data;
		return MISC.NOTIFICATIONS.NEWMUTANTSEED.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x060041DF RID: 16863 RVA: 0x001705EC File Offset: 0x0016E7EC
	public void IdentifySubSpecies(Tag subSpeciesID)
	{
		if (this.identifiedSubSpecies.Add(subSpeciesID))
		{
			this.FindSubSpecies(subSpeciesID);
			foreach (object obj in Components.MutantPlants)
			{
				MutantPlant mutantPlant = (MutantPlant)obj;
				if (mutantPlant.HasTag(subSpeciesID))
				{
					mutantPlant.UpdateNameAndTags();
				}
			}
			GeneticAnalysisCompleteMessage message = new GeneticAnalysisCompleteMessage(subSpeciesID);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x060041E0 RID: 16864 RVA: 0x00170674 File Offset: 0x0016E874
	public bool IsSubSpeciesIdentified(Tag subSpeciesID)
	{
		return this.identifiedSubSpecies.Contains(subSpeciesID);
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x00170682 File Offset: 0x0016E882
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllUnidentifiedSubSpecies(Tag speciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].FindAll((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !this.IsSubSpeciesIdentified(ss.ID));
	}

	// Token: 0x060041E2 RID: 16866 RVA: 0x001706A4 File Offset: 0x0016E8A4
	public bool IsValidPlantableSeed(Tag seedID, Tag subspeciesID)
	{
		if (!seedID.IsValid)
		{
			return false;
		}
		MutantPlant component = Assets.GetPrefab(seedID).GetComponent<MutantPlant>();
		if (component == null)
		{
			return !subspeciesID.IsValid;
		}
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(component.SpeciesID);
		return allSubSpeciesForSpecies != null && allSubSpeciesForSpecies.FindIndex((PlantSubSpeciesCatalog.SubSpeciesInfo s) => s.ID == subspeciesID) != -1 && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subspeciesID);
	}

	// Token: 0x060041E3 RID: 16867 RVA: 0x00170728 File Offset: 0x0016E928
	private void EnsureOriginalSubSpecies()
	{
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<MutantPlant>())
		{
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			Tag speciesID = component.SpeciesID;
			if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
			{
				this.discoveredSubspeciesBySpecies[speciesID] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>();
				this.discoveredSubspeciesBySpecies[speciesID].Add(component.GetSubSpeciesInfo());
			}
			this.identifiedSubSpecies.Add(component.SubSpeciesID);
		}
	}

	// Token: 0x04002AFE RID: 11006
	public static PlantSubSpeciesCatalog Instance;

	// Token: 0x04002AFF RID: 11007
	[Serialize]
	private Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> discoveredSubspeciesBySpecies = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();

	// Token: 0x04002B00 RID: 11008
	[Serialize]
	private HashSet<Tag> identifiedSubSpecies = new HashSet<Tag>();

	// Token: 0x0200172D RID: 5933
	[Serializable]
	public struct SubSpeciesInfo : IEquatable<PlantSubSpeciesCatalog.SubSpeciesInfo>
	{
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06008DB2 RID: 36274 RVA: 0x0031D3F2 File Offset: 0x0031B5F2
		public bool IsValid
		{
			get
			{
				return this.ID.IsValid;
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06008DB3 RID: 36275 RVA: 0x0031D3FF File Offset: 0x0031B5FF
		public bool IsOriginal
		{
			get
			{
				return this.mutationIDs == null || this.mutationIDs.Count == 0;
			}
		}

		// Token: 0x06008DB4 RID: 36276 RVA: 0x0031D419 File Offset: 0x0031B619
		public SubSpeciesInfo(Tag speciesID, List<string> mutationIDs)
		{
			this.speciesID = speciesID;
			this.mutationIDs = ((mutationIDs != null) ? new List<string>(mutationIDs) : new List<string>());
			this.ID = PlantSubSpeciesCatalog.SubSpeciesInfo.SubSpeciesIDFromMutations(speciesID, mutationIDs);
		}

		// Token: 0x06008DB5 RID: 36277 RVA: 0x0031D448 File Offset: 0x0031B648
		public static Tag SubSpeciesIDFromMutations(Tag speciesID, List<string> mutationIDs)
		{
			if (mutationIDs == null || mutationIDs.Count == 0)
			{
				Tag tag = speciesID;
				return tag.ToString() + "_Original";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(speciesID);
			foreach (string value in mutationIDs)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString().ToTag();
		}

		// Token: 0x06008DB6 RID: 36278 RVA: 0x0031D4EC File Offset: 0x0031B6EC
		public string GetMutationsNames()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.PLANT_MUTATIONS.NONE.NAME;
			}
			return string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs));
		}

		// Token: 0x06008DB7 RID: 36279 RVA: 0x0031D538 File Offset: 0x0031B738
		public string GetNameWithMutations(string properName, bool identified, bool cleanOriginal)
		{
			string result;
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				if (cleanOriginal)
				{
					result = properName;
				}
				else
				{
					result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.NONE.NAME);
				}
			}
			else if (!identified)
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.UNIDENTIFIED);
			}
			else
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs)));
			}
			return result;
		}

		// Token: 0x06008DB8 RID: 36280 RVA: 0x0031D5F0 File Offset: 0x0031B7F0
		public static bool operator ==(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return obj1.Equals(obj2);
		}

		// Token: 0x06008DB9 RID: 36281 RVA: 0x0031D5FA File Offset: 0x0031B7FA
		public static bool operator !=(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return !(obj1 == obj2);
		}

		// Token: 0x06008DBA RID: 36282 RVA: 0x0031D606 File Offset: 0x0031B806
		public override bool Equals(object other)
		{
			return other is PlantSubSpeciesCatalog.SubSpeciesInfo && this == (PlantSubSpeciesCatalog.SubSpeciesInfo)other;
		}

		// Token: 0x06008DBB RID: 36283 RVA: 0x0031D623 File Offset: 0x0031B823
		public bool Equals(PlantSubSpeciesCatalog.SubSpeciesInfo other)
		{
			return this.ID == other.ID;
		}

		// Token: 0x06008DBC RID: 36284 RVA: 0x0031D636 File Offset: 0x0031B836
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x06008DBD RID: 36285 RVA: 0x0031D64C File Offset: 0x0031B84C
		public string GetMutationsTooltip()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.STATUSITEMS.ORIGINALPLANTMUTATION.TOOLTIP;
			}
			if (!PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.ID))
			{
				return CREATURES.STATUSITEMS.UNKNOWNMUTATION.TOOLTIP;
			}
			string id = this.mutationIDs[0];
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			return CREATURES.STATUSITEMS.SPECIFICPLANTMUTATION.TOOLTIP.Replace("{MutationName}", plantMutation.Name) + "\n" + plantMutation.GetTooltip();
		}

		// Token: 0x04006DD7 RID: 28119
		public Tag speciesID;

		// Token: 0x04006DD8 RID: 28120
		public Tag ID;

		// Token: 0x04006DD9 RID: 28121
		public List<string> mutationIDs;

		// Token: 0x04006DDA RID: 28122
		private const string ORIGINAL_SUFFIX = "_Original";
	}
}

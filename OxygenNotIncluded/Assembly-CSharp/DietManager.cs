using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000769 RID: 1897
[AddComponentMenu("KMonoBehaviour/scripts/DietManager")]
public class DietManager : KMonoBehaviour
{
	// Token: 0x06003461 RID: 13409 RVA: 0x001194C6 File Offset: 0x001176C6
	public static void DestroyInstance()
	{
		DietManager.Instance = null;
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x001194CE File Offset: 0x001176CE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.diets = DietManager.CollectDiets(null);
		DietManager.Instance = this;
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x001194E8 File Offset: 0x001176E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscovered())
		{
			this.Discover(tag);
		}
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			Diet.Info[] infos = keyValuePair.Value.infos;
			for (int i = 0; i < infos.Length; i++)
			{
				foreach (Tag tag2 in infos[i].consumedTags)
				{
					if (Assets.GetPrefab(tag2) == null)
					{
						global::Debug.LogError(string.Format("Could not find prefab {0}, required by diet for {1}", tag2, keyValuePair.Key));
					}
				}
			}
		}
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x00119630 File Offset: 0x00117830
	private void Discover(Tag tag)
	{
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			if (keyValuePair.Value.GetDietInfo(tag) != null)
			{
				DiscoveredResources.Instance.Discover(tag, keyValuePair.Key);
			}
		}
	}

	// Token: 0x06003465 RID: 13413 RVA: 0x001196A0 File Offset: 0x001178A0
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		this.Discover(tag);
	}

	// Token: 0x06003466 RID: 13414 RVA: 0x001196AC File Offset: 0x001178AC
	public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = diet;
			}
		}
		return dictionary;
	}

	// Token: 0x04001FA7 RID: 8103
	private Dictionary<Tag, Diet> diets;

	// Token: 0x04001FA8 RID: 8104
	public static DietManager Instance;
}

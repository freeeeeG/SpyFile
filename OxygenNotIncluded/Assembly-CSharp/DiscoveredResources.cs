using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200076A RID: 1898
[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
	// Token: 0x06003468 RID: 13416 RVA: 0x0011975C File Offset: 0x0011795C
	public static void DestroyInstance()
	{
		DiscoveredResources.Instance = null;
	}

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06003469 RID: 13417 RVA: 0x00119764 File Offset: 0x00117964
	// (remove) Token: 0x0600346A RID: 13418 RVA: 0x0011979C File Offset: 0x0011799C
	public event Action<Tag, Tag> OnDiscover;

	// Token: 0x0600346B RID: 13419 RVA: 0x001197D4 File Offset: 0x001179D4
	public void Discover(Tag tag, Tag categoryTag)
	{
		bool flag = this.Discovered.Add(tag);
		this.DiscoverCategory(categoryTag, tag);
		if (flag)
		{
			if (this.OnDiscover != null)
			{
				this.OnDiscover(categoryTag, tag);
			}
			if (!this.newDiscoveries.ContainsKey(tag))
			{
				this.newDiscoveries.Add(tag, (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
			}
		}
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x0011983C File Offset: 0x00117A3C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DiscoveredResources.Instance = this;
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x0011984A File Offset: 0x00117A4A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FilterDisabledContent();
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x00119858 File Offset: 0x00117A58
	private void FilterDisabledContent()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		foreach (Tag tag in this.Discovered)
		{
			Element element = ElementLoader.GetElement(tag);
			if (element != null && element.disabled)
			{
				hashSet.Add(tag);
			}
			else
			{
				GameObject gameObject = Assets.TryGetPrefab(tag);
				if (gameObject != null && gameObject.HasTag(GameTags.DeprecatedContent))
				{
					hashSet.Add(tag);
				}
				else if (gameObject == null)
				{
					hashSet.Add(tag);
				}
			}
		}
		foreach (Tag item in hashSet)
		{
			this.Discovered.Remove(item);
		}
		foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in this.DiscoveredCategories)
		{
			foreach (Tag item2 in hashSet)
			{
				if (keyValuePair.Value.Contains(item2))
				{
					keyValuePair.Value.Remove(item2);
				}
			}
		}
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x001199DC File Offset: 0x00117BDC
	public bool CheckAllDiscoveredAreNew()
	{
		foreach (Tag key in this.Discovered)
		{
			if (!this.newDiscoveries.ContainsKey(key))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x00119A40 File Offset: 0x00117C40
	private void DiscoverCategory(Tag category_tag, Tag item_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.DiscoveredCategories.TryGetValue(category_tag, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.DiscoveredCategories[category_tag] = hashSet;
		}
		hashSet.Add(item_tag);
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x00119A78 File Offset: 0x00117C78
	public HashSet<Tag> GetDiscovered()
	{
		return this.Discovered;
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x00119A80 File Offset: 0x00117C80
	public bool IsDiscovered(Tag tag)
	{
		return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x00119AA0 File Offset: 0x00117CA0
	public bool AnyDiscovered(ICollection<Tag> tags)
	{
		foreach (Tag tag in tags)
		{
			if (this.IsDiscovered(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003474 RID: 13428 RVA: 0x00119AF4 File Offset: 0x00117CF4
	public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return this.DiscoveredCategories.TryGetValue(tag, out resources);
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x00119B04 File Offset: 0x00117D04
	public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		HashSet<Tag> result;
		if (this.DiscoveredCategories.TryGetValue(tag, out result))
		{
			return result;
		}
		return new HashSet<Tag>();
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x00119B28 File Offset: 0x00117D28
	public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag key in tagSet)
		{
			HashSet<Tag> value;
			if (this.DiscoveredCategories.TryGetValue(key, out value))
			{
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x00119B88 File Offset: 0x00117D88
	public static Tag GetCategoryForTags(HashSet<Tag> tags)
	{
		Tag result = Tag.Invalid;
		foreach (Tag tag in tags)
		{
			if (GameTags.AllCategories.Contains(tag) || GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				result = tag;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x00119BF4 File Offset: 0x00117DF4
	public static Tag GetCategoryForEntity(KPrefabID entity)
	{
		ElementChunk component = entity.GetComponent<ElementChunk>();
		if (component != null)
		{
			return component.GetComponent<PrimaryElement>().Element.materialCategory;
		}
		return DiscoveredResources.GetCategoryForTags(entity.Tags);
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x00119C30 File Offset: 0x00117E30
	public void Sim4000ms(float dt)
	{
		float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, float> keyValuePair in this.newDiscoveries)
		{
			if (num - keyValuePair.Value > 3f)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.newDiscoveries.Remove(key);
		}
	}

	// Token: 0x04001FA9 RID: 8105
	public static DiscoveredResources Instance;

	// Token: 0x04001FAA RID: 8106
	[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

	// Token: 0x04001FAB RID: 8107
	[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

	// Token: 0x04001FAD RID: 8109
	[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();
}

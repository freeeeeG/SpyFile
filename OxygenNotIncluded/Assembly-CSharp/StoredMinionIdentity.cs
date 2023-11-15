using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200050F RID: 1295
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StoredMinionIdentity")]
public class StoredMinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, IPersonalPriorityManager
{
	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x000A5897 File Offset: 0x000A3A97
	// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x000A589F File Offset: 0x000A3A9F
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06001EF4 RID: 7924 RVA: 0x000A58A8 File Offset: 0x000A3AA8
	// (set) Token: 0x06001EF5 RID: 7925 RVA: 0x000A58B0 File Offset: 0x000A3AB0
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06001EF6 RID: 7926 RVA: 0x000A58B9 File Offset: 0x000A3AB9
	// (set) Token: 0x06001EF7 RID: 7927 RVA: 0x000A58C1 File Offset: 0x000A3AC1
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x06001EF8 RID: 7928 RVA: 0x000A58CC File Offset: 0x000A3ACC
	[OnDeserialized]
	[Obsolete]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					num++;
				}
			}
			this.TotalExperienceGained = MinionResume.CalculatePreviousExperienceBar(num);
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.forbiddenTagSet = new HashSet<Tag>(this.forbiddenTags);
			this.forbiddenTags = null;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 30))
		{
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
		}
		List<ResourceRef<Accessory>> list = this.accessories.FindAll((ResourceRef<Accessory> acc) => acc.Get() == null);
		if (list.Count > 0)
		{
			List<ClothingItemResource> list2 = new List<ClothingItemResource>();
			foreach (ResourceRef<Accessory> resourceRef in list)
			{
				ClothingItemResource clothingItemResource = Db.Get().Permits.ClothingItems.TryResolveAccessoryResource(resourceRef.Guid);
				if (clothingItemResource != null && !list2.Contains(clothingItemResource))
				{
					list2.Add(clothingItemResource);
					this.customClothingItems[ClothingOutfitUtility.OutfitType.Clothing].Add(new ResourceRef<ClothingItemResource>(clothingItemResource));
				}
			}
			this.bodyData = Accessorizer.UpdateAccessorySlots(this.nameStringKey, ref this.accessories);
		}
		this.OnDeserializeModifiers();
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000A5B24 File Offset: 0x000A3D24
	public bool HasPerk(SkillPerk perk)
	{
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).perks.Contains(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000A5BA4 File Offset: 0x000A3DA4
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x000A5BC2 File Offset: 0x000A3DC2
	protected override void OnPrefabInit()
	{
		this.assignableProxy = new Ref<MinionAssignablesProxy>();
		this.minionModifiers = base.GetComponent<MinionModifiers>();
		this.savedAttributeValues = new Dictionary<string, float>();
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x000A5BE8 File Offset: 0x000A3DE8
	[OnSerializing]
	private void OnSerialize()
	{
		this.savedAttributeValues.Clear();
		foreach (AttributeInstance attributeInstance in this.minionModifiers.attributes)
		{
			this.savedAttributeValues.Add(attributeInstance.Attribute.Id, attributeInstance.GetTotalValue());
		}
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x000A5C5C File Offset: 0x000A3E5C
	protected override void OnSpawn()
	{
		MinionConfig.AddMinionAmounts(this.minionModifiers);
		MinionConfig.AddMinionTraits(DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME, this.minionModifiers);
		this.ValidateProxy();
		this.CleanupLimboMinions();
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x000A5C8A File Offset: 0x000A3E8A
	public void OnHardDelete()
	{
		if (this.assignableProxy.Get() != null)
		{
			Util.KDestroyGameObject(this.assignableProxy.Get().gameObject);
		}
		ScheduleManager.Instance.OnStoredDupeDestroyed(this);
		Components.StoredMinionIdentities.Remove(this);
	}

	// Token: 0x06001EFF RID: 7935 RVA: 0x000A5CCC File Offset: 0x000A3ECC
	private void OnDeserializeModifiers()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.savedAttributeValues)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(keyValuePair.Key);
			if (attribute == null)
			{
				attribute = Db.Get().BuildingAttributes.TryGet(keyValuePair.Key);
			}
			if (attribute != null)
			{
				if (this.minionModifiers.attributes.Get(attribute.Id) != null)
				{
					this.minionModifiers.attributes.Get(attribute.Id).Modifiers.Clear();
					this.minionModifiers.attributes.Get(attribute.Id).ClearModifiers();
				}
				else
				{
					this.minionModifiers.attributes.Add(attribute);
				}
				this.minionModifiers.attributes.Add(new AttributeModifier(attribute.Id, keyValuePair.Value, () => DUPLICANTS.ATTRIBUTES.STORED_VALUE, false, false));
			}
		}
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x000A5E00 File Offset: 0x000A4000
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x000A5E14 File Offset: 0x000A4014
	public string[] GetClothingItemIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customClothingItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customClothingItems[outfitType].Count];
			for (int i = 0; i < this.customClothingItems[outfitType].Count; i++)
			{
				array[i] = this.customClothingItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return null;
	}

	// Token: 0x06001F02 RID: 7938 RVA: 0x000A5E84 File Offset: 0x000A4084
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		bool flag = false;
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Stored minion with an invalid kpid! Attempting to recover...",
				this.storedName
			});
			flag = true;
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.storedName
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
		bool flag2 = false;
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
			for (int i = 0; i < storedMinionInfo.Count; i++)
			{
				MinionStorage.Info info = storedMinionInfo[i];
				if (flag && info.serializedMinion != null && info.serializedMinion.GetId() == -1 && info.name == this.storedName)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Found a minion storage with an invalid ref, rebinding.",
						component.InstanceID,
						this.storedName,
						minionStorage.gameObject.name
					});
					info = new MinionStorage.Info(this.storedName, new Ref<KPrefabID>(component));
					storedMinionInfo[i] = info;
					minionStorage.GetComponent<Assignable>().Assign(this);
					flag2 = true;
					break;
				}
				if (info.serializedMinion != null && info.serializedMinion.Get() == component)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		if (!flag2)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Found a stored minion that wasn't in any minion storage. Respawning them at the portal.",
				component.InstanceID,
				this.storedName
			});
			GameObject activeTelepad = GameUtil.GetActiveTelepad();
			if (activeTelepad != null)
			{
				MinionStorage.DeserializeMinion(component.gameObject, activeTelepad.transform.GetPosition());
			}
		}
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x000A6164 File Offset: 0x000A4364
	public string GetProperName()
	{
		return this.storedName;
	}

	// Token: 0x06001F04 RID: 7940 RVA: 0x000A616C File Offset: 0x000A436C
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x06001F05 RID: 7941 RVA: 0x000A617E File Offset: 0x000A437E
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x000A6190 File Offset: 0x000A4390
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x000A61A3 File Offset: 0x000A43A3
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x000A61B0 File Offset: 0x000A43B0
	public Accessory GetAccessory(AccessorySlot slot)
	{
		for (int i = 0; i < this.accessories.Count; i++)
		{
			if (this.accessories[i].Get() != null && this.accessories[i].Get().slot == slot)
			{
				return this.accessories[i].Get();
			}
		}
		return null;
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x000A6212 File Offset: 0x000A4412
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x000A621C File Offset: 0x000A441C
	public string GetStorageReason()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			using (List<MinionStorage.Info>.Enumerator enumerator2 = minionStorage.GetStoredMinionInfo().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.serializedMinion.Get() == component)
					{
						return minionStorage.GetProperName();
					}
				}
			}
		}
		return "";
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x000A62D4 File Offset: 0x000A44D4
	public bool IsPermittedToConsume(string consumable)
	{
		return !this.forbiddenTagSet.Contains(consumable);
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000A62EC File Offset: 0x000A44EC
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		foreach (string id in this.traitIDs)
		{
			if (Db.Get().traits.Exists(id))
			{
				Trait trait = Db.Get().traits.Get(id);
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == chore_group.IdHash)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000A639C File Offset: 0x000A459C
	public int GetPersonalPriority(ChoreGroup chore_group)
	{
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(chore_group.IdHash, out priorityInfo))
		{
			return priorityInfo.priority;
		}
		return 0;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x000A63C6 File Offset: 0x000A45C6
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return 0;
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x000A63C9 File Offset: 0x000A45C9
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000A63CB File Offset: 0x000A45CB
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x0400115D RID: 4445
	[Serialize]
	public string storedName;

	// Token: 0x0400115E RID: 4446
	[Serialize]
	public string gender;

	// Token: 0x04001162 RID: 4450
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x04001163 RID: 4451
	[Serialize]
	public int voiceIdx;

	// Token: 0x04001164 RID: 4452
	[Serialize]
	public KCompBuilder.BodyData bodyData;

	// Token: 0x04001165 RID: 4453
	[Serialize]
	public List<Ref<KPrefabID>> assignedItems;

	// Token: 0x04001166 RID: 4454
	[Serialize]
	public List<Ref<KPrefabID>> equippedItems;

	// Token: 0x04001167 RID: 4455
	[Serialize]
	public List<string> traitIDs;

	// Token: 0x04001168 RID: 4456
	[Serialize]
	public List<ResourceRef<Accessory>> accessories;

	// Token: 0x04001169 RID: 4457
	[Obsolete("Deprecated, use customClothingItems")]
	[Serialize]
	public List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x0400116A RID: 4458
	[Serialize]
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customClothingItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x0400116B RID: 4459
	[Serialize]
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x0400116C RID: 4460
	[Obsolete("Deprecated, use forbiddenTagSet")]
	[Serialize]
	public List<Tag> forbiddenTags;

	// Token: 0x0400116D RID: 4461
	[Serialize]
	public HashSet<Tag> forbiddenTagSet;

	// Token: 0x0400116E RID: 4462
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x0400116F RID: 4463
	[Serialize]
	public List<Effects.SaveLoadEffect> saveLoadEffects;

	// Token: 0x04001170 RID: 4464
	[Serialize]
	public List<Effects.SaveLoadImmunities> saveLoadImmunities;

	// Token: 0x04001171 RID: 4465
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x04001172 RID: 4466
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x04001173 RID: 4467
	[Serialize]
	public List<string> grantedSkillIDs = new List<string>();

	// Token: 0x04001174 RID: 4468
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x04001175 RID: 4469
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x04001176 RID: 4470
	[Serialize]
	public float TotalExperienceGained;

	// Token: 0x04001177 RID: 4471
	[Serialize]
	public string currentHat;

	// Token: 0x04001178 RID: 4472
	[Serialize]
	public string targetHat;

	// Token: 0x04001179 RID: 4473
	[Serialize]
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x0400117A RID: 4474
	[Serialize]
	public List<AttributeLevels.LevelSaveLoad> attributeLevels;

	// Token: 0x0400117B RID: 4475
	[Serialize]
	public Dictionary<string, float> savedAttributeValues;

	// Token: 0x0400117C RID: 4476
	public MinionModifiers minionModifiers;
}

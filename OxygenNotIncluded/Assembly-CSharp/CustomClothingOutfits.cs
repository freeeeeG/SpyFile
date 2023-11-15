using System;
using System.Collections.Generic;

// Token: 0x020006D1 RID: 1745
public class CustomClothingOutfits
{
	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000FB68A File Offset: 0x000F988A
	public static CustomClothingOutfits Instance
	{
		get
		{
			if (CustomClothingOutfits._instance == null)
			{
				CustomClothingOutfits._instance = new CustomClothingOutfits();
			}
			return CustomClothingOutfits._instance;
		}
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000FB6A2 File Offset: 0x000F98A2
	public SerializableOutfitData.Version2 Internal_GetOutfitData()
	{
		return this.serializableOutfitData;
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x000FB6AA File Offset: 0x000F98AA
	public void Internal_SetOutfitData(SerializableOutfitData.Version2 data)
	{
		this.serializableOutfitData = data;
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x000FB6B4 File Offset: 0x000F98B4
	public void Internal_EditOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name, string[] outfit_items)
	{
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfit_name, out customTemplateOutfitEntry))
		{
			customTemplateOutfitEntry = new SerializableOutfitData.Version2.CustomTemplateOutfitEntry();
			customTemplateOutfitEntry.outfitType = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			customTemplateOutfitEntry.itemIds = outfit_items;
			this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[outfit_name] = customTemplateOutfitEntry;
		}
		else
		{
			ClothingOutfitUtility.OutfitType outfitType;
			if (!Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				throw new NotSupportedException(string.Concat(new string[]
				{
					"Cannot edit outfit \"",
					outfit_name,
					"\" of unknown outfit type \"",
					customTemplateOutfitEntry.outfitType,
					"\""
				}));
			}
			if (outfitType != outfit_type)
			{
				throw new NotSupportedException(string.Format("Cannot edit outfit \"{0}\" of outfit type \"{1}\" to be an outfit of type \"{2}\"", outfit_name, customTemplateOutfitEntry.outfitType, outfit_type));
			}
			customTemplateOutfitEntry.itemIds = outfit_items;
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x000FB788 File Offset: 0x000F9988
	public void Internal_RenameOutfit(ClothingOutfitUtility.OutfitType outfit_type, string old_outfit_name, string new_outfit_name)
	{
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(old_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": missing \"",
				old_outfit_name,
				"\" entry"
			}));
		}
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(new_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": entry \"",
				new_outfit_name,
				"\" already exists"
			}));
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Add(new_outfit_name, this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[old_outfit_name]);
		foreach (KeyValuePair<string, Dictionary<string, string>> self in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
		{
			string text;
			Dictionary<string, string> dictionary;
			self.Deconstruct(out text, out dictionary);
			string text2 = text;
			Dictionary<string, string> dictionary2 = dictionary;
			if (dictionary2 != null)
			{
				using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
				{
					foreach (KeyValuePair<string, string> self2 in dictionary2)
					{
						string a;
						self2.Deconstruct(out text, out a);
						string item = text;
						if (a == old_outfit_name)
						{
							pooledList.Add(item);
						}
					}
					foreach (string text3 in pooledList)
					{
						dictionary2[text3] = new_outfit_name;
						Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(text2);
						ClothingOutfitUtility.OutfitType outfitType;
						if (personalityFromNameStringKey.IsNullOrDestroyed())
						{
							DebugUtil.DevAssert(false, string.Concat(new string[]
							{
								"<Renaming Outfit Error> Couldn't find personality \"",
								text2,
								"\" to switch their outfit preference from \"",
								old_outfit_name,
								"\" to \"",
								new_outfit_name,
								"\""
							}), null);
						}
						else if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(text3, true, out outfitType))
						{
							personalityFromNameStringKey.Internal_SetSelectedTemplateOutfitId(outfitType, new_outfit_name);
						}
					}
				}
			}
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(old_outfit_name);
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x000FBA30 File Offset: 0x000F9C30
	public void Internal_RemoveOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name)
	{
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(outfit_name))
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> self in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
			{
				string text;
				Dictionary<string, string> dictionary;
				self.Deconstruct(out text, out dictionary);
				string text2 = text;
				Dictionary<string, string> dictionary2 = dictionary;
				if (dictionary2 != null)
				{
					using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
					{
						foreach (KeyValuePair<string, string> self2 in dictionary2)
						{
							string a;
							self2.Deconstruct(out text, out a);
							string item = text;
							if (a == outfit_name)
							{
								pooledList.Add(item);
							}
						}
						foreach (string text3 in pooledList)
						{
							dictionary2.Remove(text3);
							Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(text2);
							ClothingOutfitUtility.OutfitType outfitType;
							if (personalityFromNameStringKey.IsNullOrDestroyed())
							{
								DebugUtil.DevAssert(false, "<Deleting Outfit Error> Couldn't find personality \"" + text2 + "\" to clear their outfit preference", null);
							}
							else if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(text3, true, out outfitType))
							{
								personalityFromNameStringKey.Internal_SetSelectedTemplateOutfitId(outfitType, Option.None);
							}
						}
					}
				}
			}
			ClothingOutfitUtility.SaveClothingOutfitData();
		}
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x000FBBF0 File Offset: 0x000F9DF0
	public void Internal_SetDuplicantPersonalityOutfit(string personalityId, Option<string> outfit_id, ClothingOutfitUtility.OutfitType outfit_type)
	{
		string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
		Dictionary<string, string> dictionary;
		if (outfit_id.HasValue)
		{
			if (!this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Add(personalityId, new Dictionary<string, string>());
			}
			this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name] = outfit_id.Value;
		}
		else if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.TryGetValue(personalityId, out dictionary))
		{
			dictionary.Remove(name);
			if (dictionary.Count == 0)
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Remove(personalityId);
			}
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x04001C41 RID: 7233
	private static CustomClothingOutfits _instance;

	// Token: 0x04001C42 RID: 7234
	private SerializableOutfitData.Version2 serializableOutfitData = new SerializableOutfitData.Version2();
}

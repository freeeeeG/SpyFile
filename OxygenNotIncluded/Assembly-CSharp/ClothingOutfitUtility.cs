using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Database;
using Newtonsoft.Json.Linq;
using STRINGS;

// Token: 0x020006D2 RID: 1746
public static class ClothingOutfitUtility
{
	// Token: 0x06002FA8 RID: 12200 RVA: 0x000FBCB8 File Offset: 0x000F9EB8
	public static string GetName(this ClothingOutfitUtility.OutfitType self)
	{
		switch (self)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_CLOTHING;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_JOY_RESPONSE;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return UI.MINION_BROWSER_SCREEN.OUTFIT_TYPE_ATMOSUIT;
		default:
			DebugUtil.DevAssert(false, string.Format("Couldn't find name for outfit type: {0}", self), null);
			return self.ToString();
		}
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x000FBD20 File Offset: 0x000F9F20
	public static bool SaveClothingOutfitData()
	{
		if (!Directory.Exists(Util.RootFolder()))
		{
			Directory.CreateDirectory(Util.RootFolder());
		}
		string text = Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string path = Path.Combine(text, ClothingOutfitUtility.OutfitFile_U47_to_Present);
		string data = SerializableOutfitData.ToJsonString(SerializableOutfitData.ToJson(CustomClothingOutfits.Instance.Internal_GetOutfitData()));
		return ClothingOutfitUtility.TryWriteTo(path, data);
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x000FBD8C File Offset: 0x000F9F8C
	public static void LoadClothingOutfitData(ClothingOutfits dbClothingOutfits)
	{
		string pathToJsonFile = ClothingOutfitUtility.GetPathToJsonFile(ClothingOutfitUtility.OutfitFile_U47_to_Present);
		if (!File.Exists(pathToJsonFile))
		{
			pathToJsonFile = ClothingOutfitUtility.GetPathToJsonFile(ClothingOutfitUtility.OutfitFile_U44_to_U46);
			if (!File.Exists(pathToJsonFile))
			{
				return;
			}
		}
		string json;
		if (!ClothingOutfitUtility.TryReadFrom(pathToJsonFile, out json))
		{
			return;
		}
		SerializableOutfitData.Version2 version = null;
		try
		{
			version = SerializableOutfitData.FromJson(JObject.Parse(json));
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(false, "ClothingOutfitData Parse failed: " + ex.ToString(), null);
		}
		if (version == null)
		{
			return;
		}
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> self in version.OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			self.Deconstruct(out text, out customTemplateOutfitEntry);
			string text2 = text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry2 = customTemplateOutfitEntry;
			ClothingOutfitResource clothingOutfitResource = dbClothingOutfits.TryGet(text2);
			if (clothingOutfitResource != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("UserAuthored outfit with id \"{0}\" of type {1} conflicts with DatabaseAuthored outfit with id \"{2}\" of type {3}. This may result in weird behaviour with outfits.", new object[]
					{
						text2,
						customTemplateOutfitEntry2.outfitType,
						clothingOutfitResource.Id,
						clothingOutfitResource.outfitType
					})
				});
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Dictionary<string, string>> self2 in version.PersonalityIdToAssignedOutfits)
		{
			string text;
			Dictionary<string, string> dictionary;
			self2.Deconstruct(out text, out dictionary);
			string text3 = text;
			Dictionary<string, string> dictionary2 = dictionary;
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(text3);
			if (personalityFromNameStringKey.IsNullOrDestroyed())
			{
				DebugUtil.DevAssert(false, "<Loadings Outfit Error> Couldn't find personality \"" + text3 + "\" to apply outfit preferences", null);
			}
			else
			{
				foreach (KeyValuePair<string, string> self3 in dictionary2)
				{
					string text4;
					self3.Deconstruct(out text, out text4);
					string value = text;
					string value2 = text4;
					ClothingOutfitUtility.OutfitType outfitType;
					if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(value, true, out outfitType))
					{
						personalityFromNameStringKey.Internal_SetSelectedTemplateOutfitId(outfitType, value2);
					}
				}
				if (text3 != personalityFromNameStringKey.Id)
				{
					list.Add(text3);
				}
			}
		}
		foreach (string text5 in list)
		{
			Personality personalityFromNameStringKey2 = Db.Get().Personalities.GetPersonalityFromNameStringKey(text5);
			if (!personalityFromNameStringKey2.IsNullOrDestroyed() && version.PersonalityIdToAssignedOutfits.ContainsKey(text5))
			{
				string id = personalityFromNameStringKey2.Id;
				Dictionary<string, string> dictionary3 = version.PersonalityIdToAssignedOutfits[text5];
				version.PersonalityIdToAssignedOutfits.Remove(text5);
				Dictionary<string, string> dictionary4;
				if (version.PersonalityIdToAssignedOutfits.TryGetValue(id, out dictionary4))
				{
					using (Dictionary<string, string>.Enumerator enumerator3 = dictionary3.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<string, string> self4 = enumerator3.Current;
							string text;
							string text4;
							self4.Deconstruct(out text4, out text);
							string key = text4;
							string value3 = text;
							if (!dictionary4.ContainsKey(key))
							{
								dictionary4[key] = value3;
							}
						}
						continue;
					}
				}
				version.PersonalityIdToAssignedOutfits.Add(id, dictionary3);
			}
		}
		CustomClothingOutfits.Instance.Internal_SetOutfitData(version);
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x000FC0D0 File Offset: 0x000FA2D0
	public static string GetPathToJsonFile(string jsonFileName)
	{
		return Path.Combine(Util.RootFolder(), Util.GetKleiItemUserDataFolderName(), jsonFileName);
	}

	// Token: 0x06002FAC RID: 12204 RVA: 0x000FC0E4 File Offset: 0x000FA2E4
	public static bool TryWriteTo(string path, string data)
	{
		bool result = false;
		try
		{
			using (FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(data);
				fileStream.Write(bytes, 0, bytes.Length);
				result = true;
			}
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(false, "ClothingOutfitData Write failed: " + ex.ToString(), null);
		}
		return result;
	}

	// Token: 0x06002FAD RID: 12205 RVA: 0x000FC15C File Offset: 0x000FA35C
	public static bool TryReadFrom(string path, out string data)
	{
		data = null;
		bool result = false;
		try
		{
			using (FileStream fileStream = File.Open(path, FileMode.Open))
			{
				using (StreamReader streamReader = new StreamReader(fileStream, new UTF8Encoding(false, true)))
				{
					data = streamReader.ReadToEnd();
					result = true;
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.DevAssert(false, "ClothingOutfitData Load failed: " + ex.ToString(), null);
		}
		return result;
	}

	// Token: 0x04001C43 RID: 7235
	public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_CLOTHING = new PermitCategory[]
	{
		PermitCategory.DupeTops,
		PermitCategory.DupeGloves,
		PermitCategory.DupeBottoms,
		PermitCategory.DupeShoes
	};

	// Token: 0x04001C44 RID: 7236
	public static readonly PermitCategory[] PERMIT_CATEGORIES_FOR_ATMO_SUITS = new PermitCategory[]
	{
		PermitCategory.AtmoSuitHelmet,
		PermitCategory.AtmoSuitBody,
		PermitCategory.AtmoSuitGloves,
		PermitCategory.AtmoSuitBelt,
		PermitCategory.AtmoSuitShoes
	};

	// Token: 0x04001C45 RID: 7237
	private static string OutfitFile_U44_to_U46 = "OutfitUserData.json";

	// Token: 0x04001C46 RID: 7238
	private static string OutfitFile_U47_to_Present = "OutfitUserData2.json";

	// Token: 0x02001410 RID: 5136
	public enum OutfitType
	{
		// Token: 0x0400643B RID: 25659
		Clothing,
		// Token: 0x0400643C RID: 25660
		JoyResponse,
		// Token: 0x0400643D RID: 25661
		AtmoSuit,
		// Token: 0x0400643E RID: 25662
		LENGTH
	}
}

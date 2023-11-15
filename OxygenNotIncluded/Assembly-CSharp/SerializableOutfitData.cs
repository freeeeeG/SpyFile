using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// Token: 0x02000969 RID: 2409
public static class SerializableOutfitData
{
	// Token: 0x060046A7 RID: 18087 RVA: 0x0018F2A8 File Offset: 0x0018D4A8
	public static int GetVersionFrom(JObject jsonData)
	{
		int result;
		if (jsonData["Version"] == null)
		{
			result = 1;
		}
		else
		{
			result = jsonData.Value<int>("Version");
			jsonData.Remove("Version");
		}
		return result;
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x0018F2E0 File Offset: 0x0018D4E0
	public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
	{
		int versionFrom = SerializableOutfitData.GetVersionFrom(jsonData);
		if (versionFrom == 1)
		{
			return SerializableOutfitData.Version2.FromVersion1(SerializableOutfitData.Version1.FromJson(jsonData));
		}
		if (versionFrom != 2)
		{
			DebugUtil.DevAssert(false, string.Format("Version {0} of OutfitData is not supported", versionFrom), null);
			return new SerializableOutfitData.Version2();
		}
		return SerializableOutfitData.Version2.FromJson(jsonData);
	}

	// Token: 0x060046A9 RID: 18089 RVA: 0x0018F32D File Offset: 0x0018D52D
	public static JObject ToJson(SerializableOutfitData.Version2 data)
	{
		return SerializableOutfitData.Version2.ToJson(data);
	}

	// Token: 0x060046AA RID: 18090 RVA: 0x0018F338 File Offset: 0x0018D538
	public static string ToJsonString(JObject data)
	{
		string result;
		using (StringWriter stringWriter = new StringWriter())
		{
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
				result = stringWriter.ToString();
			}
		}
		return result;
	}

	// Token: 0x060046AB RID: 18091 RVA: 0x0018F398 File Offset: 0x0018D598
	public static void ToJsonString(JObject data, TextWriter textWriter)
	{
		using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
		{
			data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
		}
	}

	// Token: 0x04002EE1 RID: 12001
	public const string VERSION_KEY = "Version";

	// Token: 0x020017C9 RID: 6089
	public class Version2
	{
		// Token: 0x06008F75 RID: 36725 RVA: 0x00322638 File Offset: 0x00320838
		public static SerializableOutfitData.Version2 FromVersion1(SerializableOutfitData.Version1 data)
		{
			Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> dictionary = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
			foreach (KeyValuePair<string, string[]> self in data.CustomOutfits)
			{
				string text;
				string[] array;
				self.Deconstruct(out text, out array);
				string key = text;
				string[] itemIds = array;
				dictionary.Add(key, new SerializableOutfitData.Version2.CustomTemplateOutfitEntry
				{
					outfitType = "Clothing",
					itemIds = itemIds
				});
			}
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> self2 in data.DuplicantOutfits)
			{
				string text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3;
				self2.Deconstruct(out text, out dictionary3);
				string key2 = text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary4 = dictionary3;
				Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
				dictionary2[key2] = dictionary5;
				foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> self3 in dictionary4)
				{
					ClothingOutfitUtility.OutfitType outfitType;
					self3.Deconstruct(out outfitType, out text);
					ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
					string value = text;
					dictionary5.Add(Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfitType2), value);
				}
			}
			return new SerializableOutfitData.Version2
			{
				PersonalityIdToAssignedOutfits = dictionary2,
				OutfitIdToUserAuthoredTemplateOutfit = dictionary
			};
		}

		// Token: 0x06008F76 RID: 36726 RVA: 0x00322798 File Offset: 0x00320998
		public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
		{
			return jsonData.ToObject<SerializableOutfitData.Version2>(SerializableOutfitData.Version2.GetSerializer());
		}

		// Token: 0x06008F77 RID: 36727 RVA: 0x003227A5 File Offset: 0x003209A5
		public static JObject ToJson(SerializableOutfitData.Version2 data)
		{
			JObject jobject = JObject.FromObject(data, SerializableOutfitData.Version2.GetSerializer());
			jobject.AddFirst(new JProperty("Version", 2));
			return jobject;
		}

		// Token: 0x06008F78 RID: 36728 RVA: 0x003227C8 File Offset: 0x003209C8
		public static JsonSerializer GetSerializer()
		{
			if (SerializableOutfitData.Version2.s_serializer != null)
			{
				return SerializableOutfitData.Version2.s_serializer;
			}
			SerializableOutfitData.Version2.s_serializer = JsonSerializer.CreateDefault();
			SerializableOutfitData.Version2.s_serializer.Converters.Add(new StringEnumConverter());
			return SerializableOutfitData.Version2.s_serializer;
		}

		// Token: 0x04006FE8 RID: 28648
		public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x04006FE9 RID: 28649
		public Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();

		// Token: 0x04006FEA RID: 28650
		private static JsonSerializer s_serializer;

		// Token: 0x020021DF RID: 8671
		public class CustomTemplateOutfitEntry
		{
			// Token: 0x040097AD RID: 38829
			public string outfitType;

			// Token: 0x040097AE RID: 38830
			public string[] itemIds;
		}
	}

	// Token: 0x020017CA RID: 6090
	public class Version1
	{
		// Token: 0x06008F7A RID: 36730 RVA: 0x00322818 File Offset: 0x00320A18
		public static JObject ToJson(SerializableOutfitData.Version1 data)
		{
			return JObject.FromObject(data);
		}

		// Token: 0x06008F7B RID: 36731 RVA: 0x00322820 File Offset: 0x00320A20
		public static SerializableOutfitData.Version1 FromJson(JObject jsonData)
		{
			SerializableOutfitData.Version1 version = new SerializableOutfitData.Version1();
			SerializableOutfitData.Version1 result;
			using (JsonReader jsonReader = jsonData.CreateReader())
			{
				string a = null;
				string b = "DuplicantOutfits";
				string b2 = "CustomOutfits";
				while (jsonReader.Read())
				{
					JsonToken tokenType = jsonReader.TokenType;
					if (tokenType == JsonToken.PropertyName)
					{
						a = jsonReader.Value.ToString();
					}
					if (tokenType == JsonToken.StartObject && a == b)
					{
						ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								string key = jsonReader.Value.ToString();
								while (jsonReader.Read())
								{
									tokenType = jsonReader.TokenType;
									if (tokenType == JsonToken.EndObject)
									{
										break;
									}
									if (tokenType == JsonToken.PropertyName)
									{
										Enum.TryParse<ClothingOutfitUtility.OutfitType>(jsonReader.Value.ToString(), out outfitType);
										while (jsonReader.Read())
										{
											tokenType = jsonReader.TokenType;
											if (tokenType == JsonToken.String)
											{
												string value = jsonReader.Value.ToString();
												if (outfitType != ClothingOutfitUtility.OutfitType.LENGTH)
												{
													if (!version.DuplicantOutfits.ContainsKey(key))
													{
														version.DuplicantOutfits.Add(key, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
													}
													version.DuplicantOutfits[key][outfitType] = value;
													break;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
					else if (a == b2)
					{
						string text = null;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								text = jsonReader.Value.ToString();
							}
							if (tokenType == JsonToken.StartArray)
							{
								JArray jarray = JArray.Load(jsonReader);
								if (jarray != null)
								{
									string[] array = new string[jarray.Count];
									for (int i = 0; i < jarray.Count; i++)
									{
										array[i] = jarray[i].ToString();
									}
									if (text != null)
									{
										version.CustomOutfits[text] = array;
									}
								}
							}
						}
					}
				}
				result = version;
			}
			return result;
		}

		// Token: 0x04006FEB RID: 28651
		public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();

		// Token: 0x04006FEC RID: 28652
		public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();
	}
}

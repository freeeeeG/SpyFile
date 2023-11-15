﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x02000937 RID: 2359
public static class RetireColonyUtility
{
	// Token: 0x06004476 RID: 17526 RVA: 0x0017FDF4 File Offset: 0x0017DFF4
	public static bool SaveColonySummaryData()
	{
		if (!Directory.Exists(Util.RootFolder()))
		{
			Directory.CreateDirectory(Util.RootFolder());
		}
		string text = Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = RetireColonyUtility.StripInvalidCharacters(SaveGame.Instance.BaseName);
		string text3 = Path.Combine(text, text2);
		if (!Directory.Exists(text3))
		{
			Directory.CreateDirectory(text3);
		}
		string path = Path.Combine(text3, text2 + ".json");
		string s = JsonConvert.SerializeObject(RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		if (DlcManager.IsExpansion1Active())
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
				{
					string name = worldContainer.GetComponent<ClusterGridEntity>().Name;
					string text4 = Path.Combine(text3, name);
					string text5 = Path.Combine(text3, worldContainer.id.ToString("D5"));
					if (Directory.Exists(text4))
					{
						bool flag = Directory.GetFiles(text4).Length != 0;
						if (!Directory.Exists(text5))
						{
							Directory.CreateDirectory(text5);
						}
						foreach (string text6 in Directory.GetFiles(text4))
						{
							try
							{
								File.Copy(text6, text6.Replace(text4, text5), true);
								File.Delete(text6);
							}
							catch (Exception obj)
							{
								flag = false;
								global::Debug.LogWarning("Error occurred trying to migrate screenshot: " + text6);
								global::Debug.LogWarning(obj);
							}
						}
						if (flag)
						{
							Directory.Delete(text4);
						}
					}
				}
			}
		}
		bool flag2 = false;
		int num = 0;
		while (!flag2 && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				{
					flag2 = true;
					byte[] bytes = Encoding.UTF8.GetBytes(s);
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("SaveColonySummaryData failed attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
		return flag2;
	}

	// Token: 0x06004477 RID: 17527 RVA: 0x00180054 File Offset: 0x0017E254
	public static RetiredColonyData GetCurrentColonyRetiredColonyData()
	{
		List<MinionAssignablesProxy> list = new List<MinionAssignablesProxy>();
		for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
		{
			if (Components.MinionAssignablesProxy[i] != null)
			{
				list.Add(Components.MinionAssignablesProxy[i]);
			}
		}
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in SaveGame.Instance.GetComponent<ColonyAchievementTracker>().achievements)
		{
			if (keyValuePair.Value.success)
			{
				list2.Add(keyValuePair.Key);
			}
		}
		BuildingComplete[] array = new BuildingComplete[Components.BuildingCompletes.Count];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = Components.BuildingCompletes[j];
		}
		string startWorld = null;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered && !worldContainer.IsModuleInterior)
			{
				dictionary.Add(worldContainer.id.ToString("D5"), worldContainer.worldName);
				if (worldContainer.IsStartWorld)
				{
					startWorld = worldContainer.id.ToString("D5");
				}
			}
		}
		return new RetiredColonyData(SaveGame.Instance.BaseName, GameClock.Instance.GetCycle(), System.DateTime.Now.ToShortDateString(), list2.ToArray(), list.ToArray(), array, startWorld, dictionary);
	}

	// Token: 0x06004478 RID: 17528 RVA: 0x00180210 File Offset: 0x0017E410
	private static RetiredColonyData LoadRetiredColony(string file, bool skipStats, Encoding enc)
	{
		RetiredColonyData retiredColonyData = new RetiredColonyData();
		using (FileStream fileStream = File.Open(file, FileMode.Open))
		{
			using (StreamReader streamReader = new StreamReader(fileStream, enc))
			{
				using (JsonReader jsonReader = new JsonTextReader(streamReader))
				{
					string a = string.Empty;
					List<string> list = new List<string>();
					List<global::Tuple<string, int>> list2 = new List<global::Tuple<string, int>>();
					List<RetiredColonyData.RetiredDuplicantData> list3 = new List<RetiredColonyData.RetiredDuplicantData>();
					List<RetiredColonyData.RetiredColonyStatistic> list4 = new List<RetiredColonyData.RetiredColonyStatistic>();
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					while (jsonReader.Read())
					{
						JsonToken tokenType = jsonReader.TokenType;
						if (tokenType == JsonToken.PropertyName)
						{
							a = jsonReader.Value.ToString();
						}
						if (tokenType == JsonToken.String && a == "colonyName")
						{
							retiredColonyData.colonyName = jsonReader.Value.ToString();
						}
						if (tokenType == JsonToken.String && a == "date")
						{
							retiredColonyData.date = jsonReader.Value.ToString();
						}
						if (tokenType == JsonToken.Integer && a == "cycleCount")
						{
							retiredColonyData.cycleCount = int.Parse(jsonReader.Value.ToString());
						}
						if (tokenType == JsonToken.String && a == "achievements")
						{
							list.Add(jsonReader.Value.ToString());
						}
						if (tokenType == JsonToken.StartObject && a == "Duplicants")
						{
							string a2 = null;
							RetiredColonyData.RetiredDuplicantData retiredDuplicantData = new RetiredColonyData.RetiredDuplicantData();
							retiredDuplicantData.accessories = new Dictionary<string, string>();
							while (jsonReader.Read())
							{
								tokenType = jsonReader.TokenType;
								if (tokenType == JsonToken.EndObject)
								{
									break;
								}
								if (tokenType == JsonToken.PropertyName)
								{
									a2 = jsonReader.Value.ToString();
								}
								if (a2 == "name" && tokenType == JsonToken.String)
								{
									retiredDuplicantData.name = jsonReader.Value.ToString();
								}
								if (a2 == "age" && tokenType == JsonToken.Integer)
								{
									retiredDuplicantData.age = int.Parse(jsonReader.Value.ToString());
								}
								if (a2 == "skillPointsGained" && tokenType == JsonToken.Integer)
								{
									retiredDuplicantData.skillPointsGained = int.Parse(jsonReader.Value.ToString());
								}
								if (a2 == "accessories")
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
										if (text != null && jsonReader.Value != null && tokenType == JsonToken.String)
										{
											string value = jsonReader.Value.ToString();
											retiredDuplicantData.accessories.Add(text, value);
										}
									}
								}
							}
							list3.Add(retiredDuplicantData);
						}
						if (tokenType == JsonToken.StartObject && a == "buildings")
						{
							string a3 = null;
							string a4 = null;
							int b = 0;
							while (jsonReader.Read())
							{
								tokenType = jsonReader.TokenType;
								if (tokenType == JsonToken.EndObject)
								{
									break;
								}
								if (tokenType == JsonToken.PropertyName)
								{
									a3 = jsonReader.Value.ToString();
								}
								if (a3 == "first" && tokenType == JsonToken.String)
								{
									a4 = jsonReader.Value.ToString();
								}
								if (a3 == "second" && tokenType == JsonToken.Integer)
								{
									b = int.Parse(jsonReader.Value.ToString());
								}
							}
							global::Tuple<string, int> item = new global::Tuple<string, int>(a4, b);
							list2.Add(item);
						}
						if (tokenType == JsonToken.StartObject && a == "Stats")
						{
							if (skipStats)
							{
								break;
							}
							string a5 = null;
							RetiredColonyData.RetiredColonyStatistic retiredColonyStatistic = new RetiredColonyData.RetiredColonyStatistic();
							List<global::Tuple<float, float>> list5 = new List<global::Tuple<float, float>>();
							while (jsonReader.Read())
							{
								tokenType = jsonReader.TokenType;
								if (tokenType == JsonToken.EndObject)
								{
									break;
								}
								if (tokenType == JsonToken.PropertyName)
								{
									a5 = jsonReader.Value.ToString();
								}
								if (a5 == "id" && tokenType == JsonToken.String)
								{
									retiredColonyStatistic.id = jsonReader.Value.ToString();
								}
								if (a5 == "name" && tokenType == JsonToken.String)
								{
									retiredColonyStatistic.name = jsonReader.Value.ToString();
								}
								if (a5 == "nameX" && tokenType == JsonToken.String)
								{
									retiredColonyStatistic.nameX = jsonReader.Value.ToString();
								}
								if (a5 == "nameY" && tokenType == JsonToken.String)
								{
									retiredColonyStatistic.nameY = jsonReader.Value.ToString();
								}
								if (a5 == "value" && tokenType == JsonToken.StartObject)
								{
									string a6 = null;
									float a7 = 0f;
									float b2 = 0f;
									while (jsonReader.Read())
									{
										tokenType = jsonReader.TokenType;
										if (tokenType == JsonToken.EndObject)
										{
											break;
										}
										if (tokenType == JsonToken.PropertyName)
										{
											a6 = jsonReader.Value.ToString();
										}
										if (a6 == "first" && (tokenType == JsonToken.Float || tokenType == JsonToken.Integer))
										{
											a7 = float.Parse(jsonReader.Value.ToString());
										}
										if (a6 == "second" && (tokenType == JsonToken.Float || tokenType == JsonToken.Integer))
										{
											b2 = float.Parse(jsonReader.Value.ToString());
										}
									}
									global::Tuple<float, float> item2 = new global::Tuple<float, float>(a7, b2);
									list5.Add(item2);
								}
							}
							retiredColonyStatistic.value = list5.ToArray();
							list4.Add(retiredColonyStatistic);
						}
						if (tokenType == JsonToken.StartObject && a == "worldIdentities")
						{
							string text2 = null;
							while (jsonReader.Read())
							{
								tokenType = jsonReader.TokenType;
								if (tokenType == JsonToken.EndObject)
								{
									break;
								}
								if (tokenType == JsonToken.PropertyName)
								{
									text2 = jsonReader.Value.ToString();
								}
								if (text2 != null && jsonReader.Value != null && tokenType == JsonToken.String)
								{
									string value2 = jsonReader.Value.ToString();
									dictionary.Add(text2, value2);
								}
							}
						}
						if (tokenType == JsonToken.String && a == "startWorld")
						{
							retiredColonyData.startWorld = jsonReader.Value.ToString();
						}
					}
					retiredColonyData.Duplicants = list3.ToArray();
					retiredColonyData.Stats = list4.ToArray();
					retiredColonyData.achievements = list.ToArray();
					retiredColonyData.buildings = list2;
					retiredColonyData.worldIdentities = dictionary;
				}
			}
		}
		return retiredColonyData;
	}

	// Token: 0x06004479 RID: 17529 RVA: 0x00180820 File Offset: 0x0017EA20
	public static RetiredColonyData[] LoadRetiredColonies(bool skipStats = false)
	{
		List<RetiredColonyData> list = new List<RetiredColonyData>();
		if (!Directory.Exists(Util.RootFolder()))
		{
			Directory.CreateDirectory(Util.RootFolder());
		}
		string path = Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		path = Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName());
		string[] directories = Directory.GetDirectories(path);
		for (int i = 0; i < directories.Length; i++)
		{
			foreach (string text in Directory.GetFiles(directories[i]))
			{
				if (text.EndsWith(".json"))
				{
					for (int k = 0; k < RetireColonyUtility.attempt_encodings.Length; k++)
					{
						Encoding encoding = RetireColonyUtility.attempt_encodings[k];
						try
						{
							RetiredColonyData retiredColonyData = RetireColonyUtility.LoadRetiredColony(text, skipStats, encoding);
							if (retiredColonyData != null)
							{
								if (retiredColonyData.colonyName == null)
								{
									throw new Exception("data.colonyName was null");
								}
								list.Add(retiredColonyData);
							}
							break;
						}
						catch (Exception ex)
						{
							global::Debug.LogWarningFormat("LoadRetiredColonies failed load {0} [{1}]: {2}", new object[]
							{
								encoding,
								text,
								ex.ToString()
							});
						}
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x0600447A RID: 17530 RVA: 0x00180958 File Offset: 0x0017EB58
	public static string[] LoadColonySlideshowFiles(string colonyName, string world_name)
	{
		string path = RetireColonyUtility.StripInvalidCharacters(colonyName);
		string text = Path.Combine(Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName()), path);
		if (!world_name.IsNullOrWhiteSpace())
		{
			text = Path.Combine(text, world_name);
		}
		List<string> list = new List<string>();
		if (Directory.Exists(text))
		{
			foreach (string text2 in Directory.GetFiles(text))
			{
				if (text2.EndsWith(".png"))
				{
					list.Add(text2);
				}
			}
		}
		else
		{
			global::Debug.LogWarningFormat("LoadColonySlideshow path does not exist or is not directory [{0}]", new object[]
			{
				text
			});
		}
		return list.ToArray();
	}

	// Token: 0x0600447B RID: 17531 RVA: 0x001809F4 File Offset: 0x0017EBF4
	public static Sprite[] LoadColonySlideshow(string colonyName)
	{
		string path = RetireColonyUtility.StripInvalidCharacters(colonyName);
		string text = Path.Combine(Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName()), path);
		List<Sprite> list = new List<Sprite>();
		if (Directory.Exists(text))
		{
			foreach (string text2 in Directory.GetFiles(text))
			{
				if (text2.EndsWith(".png"))
				{
					Texture2D texture2D = new Texture2D(512, 768);
					texture2D.filterMode = FilterMode.Point;
					texture2D.LoadImage(File.ReadAllBytes(text2));
					list.Add(Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect));
				}
			}
		}
		else
		{
			global::Debug.LogWarningFormat("LoadColonySlideshow path does not exist or is not directory [{0}]", new object[]
			{
				text
			});
		}
		return list.ToArray();
	}

	// Token: 0x0600447C RID: 17532 RVA: 0x00180AE8 File Offset: 0x0017ECE8
	public static Sprite LoadRetiredColonyPreview(string colonyName, string startName = null)
	{
		try
		{
			string path = RetireColonyUtility.StripInvalidCharacters(colonyName);
			string text = Path.Combine(Path.Combine(Util.RootFolder(), Util.GetRetiredColoniesFolderName()), path);
			if (!startName.IsNullOrWhiteSpace())
			{
				text = Path.Combine(text, startName);
			}
			List<string> list = new List<string>();
			if (Directory.Exists(text))
			{
				foreach (string text2 in Directory.GetFiles(text))
				{
					if (text2.EndsWith(".png"))
					{
						list.Add(text2);
					}
				}
			}
			if (list.Count > 0)
			{
				Texture2D texture2D = new Texture2D(512, 768);
				string path2 = list[list.Count - 1];
				if (!texture2D.LoadImage(File.ReadAllBytes(path2)))
				{
					return null;
				}
				if (texture2D.width > SystemInfo.maxTextureSize || texture2D.height > SystemInfo.maxTextureSize)
				{
					return null;
				}
				if (texture2D.width == 0 || texture2D.height == 0)
				{
					return null;
				}
				return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
			}
		}
		catch (Exception ex)
		{
			global::Debug.Log("Loading timelapse preview failed! reason: " + ex.Message);
		}
		return null;
	}

	// Token: 0x0600447D RID: 17533 RVA: 0x00180C60 File Offset: 0x0017EE60
	public static Sprite LoadColonyPreview(string savePath, string colonyName, bool fallbackToTimelapse = false)
	{
		string path = Path.ChangeExtension(savePath, ".png");
		if (File.Exists(path))
		{
			try
			{
				Texture2D texture2D = new Texture2D(512, 768);
				if (!texture2D.LoadImage(File.ReadAllBytes(path)))
				{
					return null;
				}
				if (texture2D.width > SystemInfo.maxTextureSize || texture2D.height > SystemInfo.maxTextureSize)
				{
					return null;
				}
				if (texture2D.width == 0 || texture2D.height == 0)
				{
					return null;
				}
				return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
			}
			catch (Exception ex)
			{
				string str = "failed to load preview image!? ";
				Exception ex2 = ex;
				global::Debug.Log(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}
		if (!fallbackToTimelapse)
		{
			return null;
		}
		try
		{
			return RetireColonyUtility.LoadRetiredColonyPreview(colonyName, null);
		}
		catch (Exception arg)
		{
			global::Debug.Log(string.Format("failed to load fallback timelapse image!? {0}", arg));
		}
		return null;
	}

	// Token: 0x0600447E RID: 17534 RVA: 0x00180D80 File Offset: 0x0017EF80
	public static string StripInvalidCharacters(string source)
	{
		foreach (char oldChar in RetireColonyUtility.invalidCharacters)
		{
			source = source.Replace(oldChar, '_');
		}
		source = source.Trim();
		return source;
	}

	// Token: 0x04002D5E RID: 11614
	private const int FILE_IO_RETRY_ATTEMPTS = 5;

	// Token: 0x04002D5F RID: 11615
	private static char[] invalidCharacters = "<>:\"\\/|?*.".ToCharArray();

	// Token: 0x04002D60 RID: 11616
	private static Encoding[] attempt_encodings = new Encoding[]
	{
		new UTF8Encoding(false, true),
		new UnicodeEncoding(false, true, true),
		Encoding.ASCII
	};
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000D72 RID: 3442
	internal static class DLLLoader
	{
		// Token: 0x06006B54 RID: 27476 RVA: 0x002A009C File Offset: 0x0029E29C
		public static bool LoadUserModLoaderDLL()
		{
			try
			{
				string path = Path.Combine(Path.Combine(Application.dataPath, "Managed"), "ModLoader.dll");
				if (!File.Exists(path))
				{
					return false;
				}
				Assembly assembly = Assembly.LoadFile(path);
				if (assembly == null)
				{
					return false;
				}
				Type type = assembly.GetType("ModLoader.ModLoader");
				if (type == null)
				{
					return false;
				}
				MethodInfo method = type.GetMethod("Start");
				if (method == null)
				{
					return false;
				}
				method.Invoke(null, null);
				global::Debug.Log("Successfully started ModLoader.dll");
				return true;
			}
			catch (Exception ex)
			{
				global::Debug.Log(ex.ToString());
			}
			return false;
		}

		// Token: 0x06006B55 RID: 27477 RVA: 0x002A0154 File Offset: 0x0029E354
		public static LoadedModData LoadDLLs(Mod ownerMod, string harmonyId, string path, bool isDev)
		{
			LoadedModData loadedModData = new LoadedModData();
			LoadedModData result;
			try
			{
				if (Testing.dll_loading == Testing.DLLLoading.Fail)
				{
					result = null;
				}
				else if (Testing.dll_loading == Testing.DLLLoading.UseModLoaderDLLExclusively)
				{
					result = null;
				}
				else
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					if (!directoryInfo.Exists)
					{
						result = null;
					}
					else
					{
						List<Assembly> list = new List<Assembly>();
						foreach (FileInfo fileInfo in directoryInfo.GetFiles())
						{
							if (fileInfo.Name.ToLower().EndsWith(".dll"))
							{
								global::Debug.Log(string.Format("Loading MOD dll: {0}", fileInfo.Name));
								Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);
								if (assembly != null)
								{
									list.Add(assembly);
								}
							}
						}
						if (list.Count == 0)
						{
							result = null;
						}
						else
						{
							loadedModData.dlls = new HashSet<Assembly>();
							loadedModData.userMod2Instances = new Dictionary<Assembly, UserMod2>();
							foreach (Assembly assembly2 in list)
							{
								loadedModData.dlls.Add(assembly2);
								UserMod2 userMod = null;
								foreach (Type type in assembly2.GetTypes())
								{
									if (!(type == null) && typeof(UserMod2).IsAssignableFrom(type))
									{
										if (userMod != null)
										{
											global::Debug.LogError("Found more than one class inheriting `UserMod2` in " + assembly2.FullName + ", only one per assembly is allowed. Aborting load.");
											return null;
										}
										userMod = (Activator.CreateInstance(type) as UserMod2);
									}
								}
								if (userMod == null)
								{
									if (isDev)
									{
										global::Debug.LogWarning(string.Format("{0} at {1} has no classes inheriting from UserMod, creating one...", assembly2.GetName(), path));
									}
									userMod = new UserMod2();
								}
								userMod.assembly = assembly2;
								userMod.path = path;
								userMod.mod = ownerMod;
								loadedModData.userMod2Instances[assembly2] = userMod;
							}
							loadedModData.harmony = new Harmony(harmonyId);
							if (loadedModData.harmony != null)
							{
								foreach (KeyValuePair<Assembly, UserMod2> keyValuePair in loadedModData.userMod2Instances)
								{
									keyValuePair.Value.OnLoad(loadedModData.harmony);
								}
							}
							loadedModData.patched_methods = (from method in loadedModData.harmony.GetPatchedMethods()
							where Harmony.GetPatchInfo(method).Owners.Contains(harmonyId)
							select method).ToList<MethodBase>();
							result = loadedModData;
						}
					}
				}
			}
			catch (Exception e)
			{
				DebugUtil.LogException(null, string.Concat(new string[]
				{
					"Exception while loading mod ",
					harmonyId,
					" at ",
					path,
					"."
				}), e);
				result = null;
			}
			return result;
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x002A0460 File Offset: 0x0029E660
		public static void PostLoadDLLs(string harmonyId, LoadedModData modData, IReadOnlyList<Mod> mods)
		{
			try
			{
				foreach (KeyValuePair<Assembly, UserMod2> keyValuePair in modData.userMod2Instances)
				{
					keyValuePair.Value.OnAllModsLoaded(modData.harmony, mods);
				}
			}
			catch (Exception e)
			{
				DebugUtil.LogException(null, "Exception while postLoading mod " + harmonyId + ".", e);
			}
		}

		// Token: 0x04004EA8 RID: 20136
		private const string managed_path = "Managed";
	}
}

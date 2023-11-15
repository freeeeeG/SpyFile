using System;
using System.Collections.Generic;
using System.Reflection;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class LegacyModMain
{
	// Token: 0x06000A14 RID: 2580 RVA: 0x0003A1A8 File Offset: 0x000383A8
	public static void Load()
	{
		List<Type> list = new List<Type>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			if (types != null)
			{
				list.AddRange(types);
			}
		}
		EntityTemplates.CreateTemplates();
		EntityTemplates.CreateBaseOreTemplates();
		LegacyModMain.LoadOre(list);
		LegacyModMain.LoadBuildings(list);
		LegacyModMain.ConfigElements();
		LegacyModMain.LoadEntities(list);
		LegacyModMain.LoadEquipment(list);
		EntityTemplates.DestroyBaseOreTemplates();
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0003A214 File Offset: 0x00038414
	private static void Test()
	{
		Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Component));
		for (int i = 0; i < array.Length; i++)
		{
			Type type = ((Component)array[i]).GetType();
			int num = 0;
			dictionary.TryGetValue(type, out num);
			dictionary[type] = num + 1;
		}
		List<LegacyModMain.Entry> list = new List<LegacyModMain.Entry>();
		foreach (KeyValuePair<Type, int> keyValuePair in dictionary)
		{
			if (keyValuePair.Key.GetMethod("Update", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy) != null)
			{
				list.Add(new LegacyModMain.Entry
				{
					count = keyValuePair.Value,
					type = keyValuePair.Key
				});
			}
		}
		list.Sort((LegacyModMain.Entry x, LegacyModMain.Entry y) => y.count.CompareTo(x.count));
		string text = "";
		foreach (LegacyModMain.Entry entry in list)
		{
			string[] array2 = new string[5];
			array2[0] = text;
			array2[1] = entry.type.Name;
			array2[2] = ": ";
			int num2 = 3;
			int i = entry.count;
			array2[num2] = i.ToString();
			array2[4] = "\n";
			text = string.Concat(array2);
		}
		global::Debug.Log(text);
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0003A3A8 File Offset: 0x000385A8
	private static void ListUnusedTypes()
	{
		HashSet<Type> hashSet = new HashSet<Type>();
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Component component in ((GameObject)array[i]).GetComponents<Component>())
			{
				if (!(component == null))
				{
					Type type = component.GetType();
					while (type != typeof(Component))
					{
						hashSet.Add(type);
						type = type.BaseType;
					}
				}
			}
		}
		HashSet<Type> hashSet2 = new HashSet<Type>();
		foreach (Type type2 in App.GetCurrentDomainTypes())
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(type2) && !hashSet.Contains(type2))
			{
				hashSet2.Add(type2);
			}
		}
		List<Type> list = new List<Type>(hashSet2);
		list.Sort((Type x, Type y) => x.FullName.CompareTo(y.FullName));
		string text = "Unused types:";
		foreach (Type type3 in list)
		{
			text = text + "\n" + type3.FullName;
		}
		global::Debug.Log(text);
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0003A530 File Offset: 0x00038730
	private static void DebugSelected()
	{
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0003A532 File Offset: 0x00038732
	private static void DebugSelected(GameObject go)
	{
		object component = go.GetComponent<Constructable>();
		int num = 0 + 1;
		global::Debug.Log(component);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0003A543 File Offset: 0x00038743
	private static void LoadOre(List<Type> types)
	{
		GeneratedOre.LoadGeneratedOre(types);
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0003A54C File Offset: 0x0003874C
	private static void LoadBuildings(List<Type> types)
	{
		LocString.CreateLocStringKeys(typeof(BUILDINGS.PREFABS), "STRINGS.BUILDINGS.");
		LocString.CreateLocStringKeys(typeof(BUILDINGS.DAMAGESOURCES), "STRINGS.BUILDINGS.DAMAGESOURCES");
		LocString.CreateLocStringKeys(typeof(BUILDINGS.REPAIRABLE), "STRINGS.BUILDINGS.REPAIRABLE");
		LocString.CreateLocStringKeys(typeof(BUILDINGS.DISINFECTABLE), "STRINGS.BUILDINGS.DISINFECTABLE");
		GeneratedBuildings.LoadGeneratedBuildings(types);
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0003A5AF File Offset: 0x000387AF
	private static void LoadEntities(List<Type> types)
	{
		EntityConfigManager.Instance.LoadGeneratedEntities(types);
		BuildingConfigManager.Instance.ConfigurePost();
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0003A5C6 File Offset: 0x000387C6
	private static void LoadEquipment(List<Type> types)
	{
		LocString.CreateLocStringKeys(typeof(EQUIPMENT.PREFABS), "STRINGS.EQUIPMENT.");
		GeneratedEquipment.LoadGeneratedEquipment(types);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0003A5E4 File Offset: 0x000387E4
	private static void ConfigElements()
	{
		foreach (LegacyModMain.ElementInfo elementInfo in new LegacyModMain.ElementInfo[]
		{
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Katairite,
				overheatMod = 200f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Cuprite,
				decor = 0.1f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Copper,
				decor = 0.2f,
				overheatMod = 50f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Gold,
				decor = 0.5f,
				overheatMod = 50f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Lead,
				overheatMod = -20f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Granite,
				decor = 0.2f,
				overheatMod = 15f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.SandStone,
				decor = 0.1f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.ToxicSand,
				overheatMod = -10f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Dirt,
				overheatMod = -10f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.IgneousRock,
				overheatMod = 15f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Obsidian,
				overheatMod = 15f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Ceramic,
				overheatMod = 200f,
				decor = 0.2f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.RefinedCarbon,
				overheatMod = 900f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Iron,
				overheatMod = 50f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Tungsten,
				overheatMod = 50f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Steel,
				overheatMod = 200f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.GoldAmalgam,
				overheatMod = 50f,
				decor = 0.1f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Diamond,
				overheatMod = 200f,
				decor = 1f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.Niobium,
				decor = 0.5f,
				overheatMod = 500f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.TempConductorSolid,
				overheatMod = 900f
			},
			new LegacyModMain.ElementInfo
			{
				id = SimHashes.HardPolypropylene,
				overheatMod = 900f
			}
		})
		{
			Element element = ElementLoader.FindElementByHash(elementInfo.id);
			if (elementInfo.decor != 0f)
			{
				AttributeModifier item = new AttributeModifier("Decor", elementInfo.decor, element.name, true, false, true);
				element.attributeModifiers.Add(item);
			}
			if (elementInfo.overheatMod != 0f)
			{
				AttributeModifier item2 = new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, elementInfo.overheatMod, element.name, false, false, true);
				element.attributeModifiers.Add(item2);
			}
		}
	}

	// Token: 0x02000F56 RID: 3926
	private struct Entry
	{
		// Token: 0x0400559E RID: 21918
		public int count;

		// Token: 0x0400559F RID: 21919
		public Type type;
	}

	// Token: 0x02000F57 RID: 3927
	private struct ElementInfo
	{
		// Token: 0x040055A0 RID: 21920
		public SimHashes id;

		// Token: 0x040055A1 RID: 21921
		public float decor;

		// Token: 0x040055A2 RID: 21922
		public float overheatMod;
	}
}

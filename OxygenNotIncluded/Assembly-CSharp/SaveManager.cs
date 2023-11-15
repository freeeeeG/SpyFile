using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x020004FE RID: 1278
[AddComponentMenu("KMonoBehaviour/scripts/SaveManager")]
public class SaveManager : KMonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06001E11 RID: 7697 RVA: 0x000A0744 File Offset: 0x0009E944
	// (remove) Token: 0x06001E12 RID: 7698 RVA: 0x000A077C File Offset: 0x0009E97C
	public event Action<SaveLoadRoot> onRegister;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06001E13 RID: 7699 RVA: 0x000A07B4 File Offset: 0x0009E9B4
	// (remove) Token: 0x06001E14 RID: 7700 RVA: 0x000A07EC File Offset: 0x0009E9EC
	public event Action<SaveLoadRoot> onUnregister;

	// Token: 0x06001E15 RID: 7701 RVA: 0x000A0821 File Offset: 0x0009EA21
	protected override void OnPrefabInit()
	{
		Assets.RegisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x000A0834 File Offset: 0x0009EA34
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Assets.UnregisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x000A0850 File Offset: 0x0009EA50
	private void OnAddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		Tag saveLoadTag = prefab.GetSaveLoadTag();
		this.prefabMap[saveLoadTag] = prefab.gameObject;
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000A0880 File Offset: 0x0009EA80
	public Dictionary<Tag, List<SaveLoadRoot>> GetLists()
	{
		return this.sceneObjects;
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000A0888 File Offset: 0x0009EA88
	private List<SaveLoadRoot> GetSaveLoadRootList(SaveLoadRoot saver)
	{
		KPrefabID component = saver.GetComponent<KPrefabID>();
		if (component == null)
		{
			DebugUtil.LogErrorArgs(saver.gameObject, new object[]
			{
				"All savers must also have a KPrefabID on them but",
				saver.gameObject.name,
				"does not have one."
			});
			return null;
		}
		List<SaveLoadRoot> list;
		if (!this.sceneObjects.TryGetValue(component.GetSaveLoadTag(), out list))
		{
			list = new List<SaveLoadRoot>();
			this.sceneObjects[component.GetSaveLoadTag()] = list;
		}
		return list;
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000A0904 File Offset: 0x0009EB04
	public void Register(SaveLoadRoot root)
	{
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Add(root);
		if (this.onRegister != null)
		{
			this.onRegister(root);
		}
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000A0938 File Offset: 0x0009EB38
	public void Unregister(SaveLoadRoot root)
	{
		if (this.onRegister != null)
		{
			this.onUnregister(root);
		}
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Remove(root);
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x000A0970 File Offset: 0x0009EB70
	public GameObject GetPrefab(Tag tag)
	{
		GameObject result = null;
		if (this.prefabMap.TryGetValue(tag, out result))
		{
			return result;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Item not found in prefabMap",
			"[" + tag.Name + "]"
		});
		return null;
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000A09C0 File Offset: 0x0009EBC0
	public void Save(BinaryWriter writer)
	{
		writer.Write(SaveManager.SAVE_HEADER);
		writer.Write(7);
		writer.Write(32);
		int num = 0;
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			if (keyValuePair.Value.Count > 0)
			{
				num++;
			}
		}
		writer.Write(num);
		this.orderedKeys.Clear();
		this.orderedKeys.AddRange(this.sceneObjects.Keys);
		this.orderedKeys.Remove(SaveGame.Instance.PrefabID());
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name == "StickerBomb"
		select a).ToList<Tag>();
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name.Contains("UnderConstruction")
		select a).ToList<Tag>();
		this.Write(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot>(new SaveLoadRoot[]
		{
			SaveGame.Instance.GetComponent<SaveLoadRoot>()
		}), writer);
		foreach (Tag key in this.orderedKeys)
		{
			List<SaveLoadRoot> list = this.sceneObjects[key];
			if (list.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot in list)
				{
					if (!(saveLoadRoot == null) && saveLoadRoot.GetComponent<SimCellOccupier>() != null)
					{
						this.Write(key, list, writer);
						break;
					}
				}
			}
		}
		foreach (Tag key2 in this.orderedKeys)
		{
			List<SaveLoadRoot> list2 = this.sceneObjects[key2];
			if (list2.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot2 in list2)
				{
					if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<SimCellOccupier>() == null)
					{
						this.Write(key2, list2, writer);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000A0C74 File Offset: 0x0009EE74
	private void Write(Tag key, List<SaveLoadRoot> value, BinaryWriter writer)
	{
		int count = value.Count;
		Tag tag = key;
		writer.WriteKleiString(tag.Name);
		writer.Write(count);
		long position = writer.BaseStream.Position;
		int value2 = -1;
		writer.Write(value2);
		long position2 = writer.BaseStream.Position;
		foreach (SaveLoadRoot saveLoadRoot in value)
		{
			if (saveLoadRoot != null)
			{
				saveLoadRoot.Save(writer);
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Null game object when saving"
				});
			}
		}
		long position3 = writer.BaseStream.Position;
		long num = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000A0D5C File Offset: 0x0009EF5C
	public bool Load(IReader reader)
	{
		char[] array = reader.ReadChars(SaveManager.SAVE_HEADER.Length);
		if (array == null || array.Length != SaveManager.SAVE_HEADER.Length)
		{
			return false;
		}
		for (int i = 0; i < SaveManager.SAVE_HEADER.Length; i++)
		{
			if (array[i] != SaveManager.SAVE_HEADER[i])
			{
				return false;
			}
		}
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		if (num != 7 || num2 > 32)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("SAVE FILE VERSION MISMATCH! Expected {0}.{1} but got {2}.{3}", new object[]
				{
					7,
					32,
					num,
					num2
				})
			});
			return false;
		}
		this.ClearScene();
		try
		{
			int num3 = reader.ReadInt32();
			for (int j = 0; j < num3; j++)
			{
				string text = reader.ReadKleiString();
				int num4 = reader.ReadInt32();
				int length = reader.ReadInt32();
				Tag key = TagManager.Create(text);
				GameObject prefab;
				if (!this.prefabMap.TryGetValue(key, out prefab))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not find prefab '" + text + "'"
					});
					reader.SkipBytes(length);
				}
				else
				{
					List<SaveLoadRoot> value = new List<SaveLoadRoot>(num4);
					this.sceneObjects[key] = value;
					for (int k = 0; k < num4; k++)
					{
						SaveLoadRoot x = SaveLoadRoot.Load(prefab, reader);
						if (SaveManager.DEBUG_OnlyLoadThisCellsObjects == -1 && x == null)
						{
							global::Debug.LogError("Error loading data [" + text + "]");
							return false;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Error deserializing prefabs\n\n",
				ex.ToString()
			});
			throw ex;
		}
		return true;
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x000A0F20 File Offset: 0x0009F120
	private void ClearScene()
	{
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			foreach (SaveLoadRoot saveLoadRoot in keyValuePair.Value)
			{
				UnityEngine.Object.Destroy(saveLoadRoot.gameObject);
			}
		}
		this.sceneObjects.Clear();
	}

	// Token: 0x040010BE RID: 4286
	public const int SAVE_MAJOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x040010BF RID: 4287
	public const int SAVE_MAJOR_VERSION = 7;

	// Token: 0x040010C0 RID: 4288
	public const int SAVE_MINOR_VERSION_EXPLICIT_VALUE_TYPES = 4;

	// Token: 0x040010C1 RID: 4289
	public const int SAVE_MINOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x040010C2 RID: 4290
	public const int SAVE_MINOR_VERSION_MOD_IDENTIFIER = 8;

	// Token: 0x040010C3 RID: 4291
	public const int SAVE_MINOR_VERSION_FINITE_SPACE_RESOURCES = 9;

	// Token: 0x040010C4 RID: 4292
	public const int SAVE_MINOR_VERSION_COLONY_REQ_ACHIEVEMENTS = 10;

	// Token: 0x040010C5 RID: 4293
	public const int SAVE_MINOR_VERSION_TRACK_NAV_DISTANCE = 11;

	// Token: 0x040010C6 RID: 4294
	public const int SAVE_MINOR_VERSION_EXPANDED_WORLD_INFO = 12;

	// Token: 0x040010C7 RID: 4295
	public const int SAVE_MINOR_VERSION_BASIC_COMFORTS_FIX = 13;

	// Token: 0x040010C8 RID: 4296
	public const int SAVE_MINOR_VERSION_PLATFORM_TRAIT_NAMES = 14;

	// Token: 0x040010C9 RID: 4297
	public const int SAVE_MINOR_VERSION_ADD_JOY_REACTIONS = 15;

	// Token: 0x040010CA RID: 4298
	public const int SAVE_MINOR_VERSION_NEW_AUTOMATION_WARNING = 16;

	// Token: 0x040010CB RID: 4299
	public const int SAVE_MINOR_VERSION_ADD_GUID_TO_HEADER = 17;

	// Token: 0x040010CC RID: 4300
	public const int SAVE_MINOR_VERSION_EXPANSION_1_INTRODUCED = 20;

	// Token: 0x040010CD RID: 4301
	public const int SAVE_MINOR_VERSION_CONTENT_SETTINGS = 21;

	// Token: 0x040010CE RID: 4302
	public const int SAVE_MINOR_VERSION_COLONY_REQ_REMOVE_SERIALIZATION = 22;

	// Token: 0x040010CF RID: 4303
	public const int SAVE_MINOR_VERSION_ROTTABLE_TUNING = 23;

	// Token: 0x040010D0 RID: 4304
	public const int SAVE_MINOR_VERSION_LAUNCH_PAD_SOLIDITY = 24;

	// Token: 0x040010D1 RID: 4305
	public const int SAVE_MINOR_VERSION_BASE_GAME_MERGEDOWN = 25;

	// Token: 0x040010D2 RID: 4306
	public const int SAVE_MINOR_VERSION_FALLING_WATER_WORLDIDX_SERIALIZATION = 26;

	// Token: 0x040010D3 RID: 4307
	public const int SAVE_MINOR_VERSION_ROCKET_RANGE_REBALANCE = 27;

	// Token: 0x040010D4 RID: 4308
	public const int SAVE_MINOR_VERSION_ENTITIES_WRONG_LAYER = 28;

	// Token: 0x040010D5 RID: 4309
	public const int SAVE_MINOR_VERSION_TAGBITS_REWORK = 29;

	// Token: 0x040010D6 RID: 4310
	public const int SAVE_MINOR_VERSION_ACCESSORY_SLOT_UPGRADE = 30;

	// Token: 0x040010D7 RID: 4311
	public const int SAVE_MINOR_VERSION_GEYSER_CAN_BE_RENAMED = 31;

	// Token: 0x040010D8 RID: 4312
	public const int SAVE_MINOR_VERSION_SPACE_SCANNERS_TELESCOPES = 32;

	// Token: 0x040010D9 RID: 4313
	public const int SAVE_MINOR_VERSION = 32;

	// Token: 0x040010DA RID: 4314
	private Dictionary<Tag, GameObject> prefabMap = new Dictionary<Tag, GameObject>();

	// Token: 0x040010DB RID: 4315
	private Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = new Dictionary<Tag, List<SaveLoadRoot>>();

	// Token: 0x040010DE RID: 4318
	public static int DEBUG_OnlyLoadThisCellsObjects = -1;

	// Token: 0x040010DF RID: 4319
	private static readonly char[] SAVE_HEADER = new char[]
	{
		'K',
		'S',
		'A',
		'V'
	};

	// Token: 0x040010E0 RID: 4320
	private List<Tag> orderedKeys = new List<Tag>();

	// Token: 0x020011AA RID: 4522
	private enum BoundaryTag : uint
	{
		// Token: 0x04005D28 RID: 23848
		Component = 3735928559U,
		// Token: 0x04005D29 RID: 23849
		Prefab = 3131961357U,
		// Token: 0x04005D2A RID: 23850
		Complete = 3735929054U
	}
}

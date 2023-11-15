using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using KMod;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000591 RID: 1425
[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06002279 RID: 8825 RVA: 0x000BD494 File Offset: 0x000BB694
	protected override void OnPrefabInit()
	{
		Assets.instance = this;
		if (KPlayerPrefs.HasKey("TemperatureUnit"))
		{
			GameUtil.temperatureUnit = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt("TemperatureUnit");
		}
		if (KPlayerPrefs.HasKey("MassUnit"))
		{
			GameUtil.massUnit = (GameUtil.MassUnit)KPlayerPrefs.GetInt("MassUnit");
		}
		RecipeManager.DestroyInstance();
		RecipeManager.Get();
		Assets.AnimMaterial = this.AnimMaterialAsset;
		Assets.Prefabs = new List<KPrefabID>(from x in this.PrefabAssets
		where x != null
		select x);
		Assets.PrefabsByTag.Clear();
		Assets.PrefabsByAdditionalTags.Clear();
		Assets.CountableTags.Clear();
		Assets.Sprites = new Dictionary<HashedString, Sprite>();
		foreach (Sprite sprite in this.SpriteAssets)
		{
			if (!(sprite == null))
			{
				HashedString key = new HashedString(sprite.name);
				Assets.Sprites.Add(key, sprite);
			}
		}
		Assets.TintedSprites = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		Assets.Materials = (from x in this.MaterialAssets
		where x != null
		select x).ToList<Material>();
		Assets.Textures = (from x in this.TextureAssets
		where x != null
		select x).ToList<Texture2D>();
		Assets.TextureAtlases = (from x in this.TextureAtlasAssets
		where x != null
		select x).ToList<TextureAtlas>();
		Assets.BlockTileDecorInfos = (from x in this.BlockTileDecorInfoAssets
		where x != null
		select x).ToList<BlockTileDecorInfo>();
		this.LoadAnims();
		Assets.UIPrefabs = this.UIPrefabAssets;
		Assets.DebugFont = this.DebugFontAsset;
		AsyncLoadManager<IGlobalAsyncLoader>.Run();
		GameAudioSheets.Get().Initialize();
		this.SubstanceListHookup();
		this.CreatePrefabs();
	}

	// Token: 0x0600227A RID: 8826 RVA: 0x000BD6EC File Offset: 0x000BB8EC
	private void CreatePrefabs()
	{
		Db.Get();
		Assets.BuildingDefs = new List<BuildingDef>();
		foreach (KPrefabID kprefabID in this.PrefabAssets)
		{
			if (!(kprefabID == null))
			{
				kprefabID.InitializeTags(true);
				Assets.AddPrefab(kprefabID);
			}
		}
		LegacyModMain.Load();
		Db.Get().PostProcess();
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x000BD770 File Offset: 0x000BB970
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Db.Get();
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x000BD780 File Offset: 0x000BB980
	private static void TryAddCountableTag(KPrefabID prefab)
	{
		foreach (Tag tag in GameTags.DisplayAsUnits)
		{
			if (prefab.HasTag(tag))
			{
				Assets.AddCountableTag(prefab.PrefabTag);
				break;
			}
		}
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x000BD7DC File Offset: 0x000BB9DC
	public static void AddCountableTag(Tag tag)
	{
		Assets.CountableTags.Add(tag);
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000BD7EA File Offset: 0x000BB9EA
	public static bool IsTagCountable(Tag tag)
	{
		return Assets.CountableTags.Contains(tag);
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000BD7F7 File Offset: 0x000BB9F7
	private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab)
	{
		if (prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
		{
			Assets.SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
		}
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000BD817 File Offset: 0x000BBA17
	public static bool IsTagSolidTransferArmConveyable(Tag tag)
	{
		return Assets.SolidTransferArmConeyableTags.Contains(tag);
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x000BD824 File Offset: 0x000BBA24
	private void LoadAnims()
	{
		KAnimBatchManager.DestroyInstance();
		KAnimGroupFile.DestroyInstance();
		KGlobalAnimParser.DestroyInstance();
		KAnimBatchManager.CreateInstance();
		KGlobalAnimParser.CreateInstance();
		KAnimGroupFile.LoadGroupResourceFile();
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			this.AnimAssets.AddRange(BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);
		}
		Assets.Anims = (from x in this.AnimAssets
		where x != null
		select x).ToList<KAnimFile>();
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		Assets.AnimTable.Clear();
		foreach (KAnimFile kanimFile in Assets.Anims)
		{
			if (kanimFile != null)
			{
				HashedString key = kanimFile.name;
				Assets.AnimTable[key] = kanimFile;
			}
		}
		KAnimGroupFile.MapNamesToAnimFiles(Assets.AnimTable);
		Global.Instance.modManager.Load(Content.Animation);
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		foreach (KAnimFile kanimFile2 in Assets.ModLoadedKAnims)
		{
			if (kanimFile2 != null)
			{
				HashedString key2 = kanimFile2.name;
				Assets.AnimTable[key2] = kanimFile2;
			}
		}
		global::Debug.Assert(Assets.AnimTable.Count > 0, "Anim Assets not yet loaded");
		KAnimGroupFile.LoadAll();
		foreach (KAnimFile kanimFile3 in Assets.Anims)
		{
			kanimFile3.FinalizeLoading();
		}
		KAnimBatchManager.Instance().CompleteInit();
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000BDA14 File Offset: 0x000BBC14
	private void SubstanceListHookup()
	{
		Dictionary<string, SubstanceTable> dictionary = new Dictionary<string, SubstanceTable>
		{
			{
				"",
				this.substanceTable
			}
		};
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			dictionary["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;
		}
		Hashtable hashtable = new Hashtable();
		ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
		ElementLoader.Load(ref hashtable, dictionary);
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000BDA86 File Offset: 0x000BBC86
	public static string GetSimpleSoundEventName(EventReference event_ref)
	{
		return Assets.GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x000BDA94 File Offset: 0x000BBC94
	public static string GetSimpleSoundEventName(string path)
	{
		string text = null;
		if (!Assets.simpleSoundEventNames.TryGetValue(path, out text))
		{
			int num = path.LastIndexOf('/');
			text = ((num != -1) ? path.Substring(num + 1) : path);
			Assets.simpleSoundEventNames[path] = text;
		}
		return text;
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000BDADC File Offset: 0x000BBCDC
	private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
	{
		int count = defs.Count;
		for (int i = 0; i < count; i++)
		{
			if (defs[i].PrefabID == prefab_id)
			{
				return defs[i];
			}
		}
		return null;
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000BDB19 File Offset: 0x000BBD19
	public static BuildingDef GetBuildingDef(string prefab_id)
	{
		return Assets.GetDef(Assets.BuildingDefs, prefab_id);
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000BDB28 File Offset: 0x000BBD28
	public static TintedSprite GetTintedSprite(string name)
	{
		TintedSprite result = null;
		if (Assets.TintedSprites != null)
		{
			for (int i = 0; i < Assets.TintedSprites.Count; i++)
			{
				if (Assets.TintedSprites[i].sprite.name == name)
				{
					result = Assets.TintedSprites[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x000BDB80 File Offset: 0x000BBD80
	public static Sprite GetSprite(HashedString name)
	{
		Sprite result = null;
		if (Assets.Sprites != null)
		{
			Assets.Sprites.TryGetValue(name, out result);
		}
		return result;
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x000BDBA5 File Offset: 0x000BBDA5
	public static VideoClip GetVideo(string name)
	{
		return Resources.Load<VideoClip>("video_webm/" + name);
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000BDBB8 File Offset: 0x000BBDB8
	public static Texture2D GetTexture(string name)
	{
		Texture2D result = null;
		if (Assets.Textures != null)
		{
			for (int i = 0; i < Assets.Textures.Count; i++)
			{
				if (Assets.Textures[i].name == name)
				{
					result = Assets.Textures[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000BDC0C File Offset: 0x000BBE0C
	public static ComicData GetComic(string id)
	{
		foreach (ComicData comicData in Assets.instance.comics)
		{
			if (comicData.name == id)
			{
				return comicData;
			}
		}
		return null;
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x000BDC48 File Offset: 0x000BBE48
	public static void AddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		prefab.InitializeTags(true);
		prefab.UpdateSaveLoadTag();
		if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
		{
			string str = "Tried loading prefab with duplicate tag, ignoring: ";
			Tag prefabTag = prefab.PrefabTag;
			global::Debug.LogWarning(str + prefabTag.ToString());
			return;
		}
		Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
		foreach (Tag key in prefab.Tags)
		{
			if (!Assets.PrefabsByAdditionalTags.ContainsKey(key))
			{
				Assets.PrefabsByAdditionalTags[key] = new List<KPrefabID>();
			}
			Assets.PrefabsByAdditionalTags[key].Add(prefab);
		}
		Assets.Prefabs.Add(prefab);
		Assets.TryAddCountableTag(prefab);
		Assets.TryAddSolidTransferArmConveyableTag(prefab);
		if (Assets.OnAddPrefab != null)
		{
			Assets.OnAddPrefab(prefab);
		}
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x000BDD4C File Offset: 0x000BBF4C
	public static void RegisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Combine(Assets.OnAddPrefab, on_add);
		foreach (KPrefabID obj in Assets.Prefabs)
		{
			on_add(obj);
		}
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000BDDB4 File Offset: 0x000BBFB4
	public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Remove(Assets.OnAddPrefab, on_add);
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x000BDDCB File Offset: 0x000BBFCB
	public static void ClearOnAddPrefab()
	{
		Assets.OnAddPrefab = null;
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x000BDDD4 File Offset: 0x000BBFD4
	public static GameObject GetPrefab(Tag tag)
	{
		GameObject gameObject = Assets.TryGetPrefab(tag);
		if (gameObject == null)
		{
			string str = "Missing prefab: ";
			Tag tag2 = tag;
			global::Debug.LogWarning(str + tag2.ToString());
		}
		return gameObject;
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x000BDE10 File Offset: 0x000BC010
	public static GameObject TryGetPrefab(Tag tag)
	{
		KPrefabID kprefabID = null;
		Assets.PrefabsByTag.TryGetValue(tag, out kprefabID);
		if (!(kprefabID != null))
		{
			return null;
		}
		return kprefabID.gameObject;
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x000BDE40 File Offset: 0x000BC040
	public static List<GameObject> GetPrefabsWithTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
		{
			for (int i = 0; i < Assets.PrefabsByAdditionalTags[tag].Count; i++)
			{
				list.Add(Assets.PrefabsByAdditionalTags[tag][i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000BDE98 File Offset: 0x000BC098
	public static List<GameObject> GetPrefabsWithComponent<Type>()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000BDEEE File Offset: 0x000BC0EE
	public static GameObject GetPrefabWithComponent<Type>()
	{
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Type>();
		global::Debug.Assert(prefabsWithComponent.Count > 0, "There are no prefabs of type " + typeof(Type).Name);
		return prefabsWithComponent[0];
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000BDF24 File Offset: 0x000BC124
	public static List<Tag> GetPrefabTagsWithComponent<Type>()
	{
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].PrefabID());
			}
		}
		return list;
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000BDF7C File Offset: 0x000BC17C
	public static Assets GetInstanceEditorOnly()
	{
		Assets[] array = (Assets[])Resources.FindObjectsOfTypeAll(typeof(Assets));
		if (array != null)
		{
			int num = array.Length;
		}
		return array[0];
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000BDFA8 File Offset: 0x000BC1A8
	public static TextureAtlas GetTextureAtlas(string name)
	{
		foreach (TextureAtlas textureAtlas in Assets.TextureAtlases)
		{
			if (textureAtlas.name == name)
			{
				return textureAtlas;
			}
		}
		return null;
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x000BE008 File Offset: 0x000BC208
	public static Material GetMaterial(string name)
	{
		foreach (Material material in Assets.Materials)
		{
			if (material.name == name)
			{
				return material;
			}
		}
		return null;
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x000BE068 File Offset: 0x000BC268
	public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
	{
		foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
		{
			if (blockTileDecorInfo.name == name)
			{
				return blockTileDecorInfo;
			}
		}
		global::Debug.LogError("Could not find BlockTileDecorInfo named [" + name + "]");
		return null;
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000BE0E0 File Offset: 0x000BC2E0
	public static KAnimFile GetAnim(HashedString name)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			return null;
		}
		KAnimFile kanimFile = null;
		Assets.AnimTable.TryGetValue(name, out kanimFile);
		if (kanimFile == null)
		{
			global::Debug.LogWarning("Missing Anim: [" + name.ToString() + "]. You may have to run Collect Anim on the Assets prefab");
		}
		return kanimFile;
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000BE13D File Offset: 0x000BC33D
	public static bool TryGetAnim(HashedString name, out KAnimFile anim)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			anim = null;
			return false;
		}
		Assets.AnimTable.TryGetValue(name, out anim);
		return anim != null;
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000BE16C File Offset: 0x000BC36C
	public void OnAfterDeserialize()
	{
		this.TintedSpriteAssets = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		this.TintedSpriteAssets.Sort((TintedSprite a, TintedSprite b) => a.name.CompareTo(b.name));
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000BE1D8 File Offset: 0x000BC3D8
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000BE1DC File Offset: 0x000BC3DC
	public static void AddBuildingDef(BuildingDef def)
	{
		Assets.BuildingDefs = (from x in Assets.BuildingDefs
		where x.PrefabID != def.PrefabID
		select x).ToList<BuildingDef>();
		Assets.BuildingDefs.Add(def);
	}

	// Token: 0x040013B0 RID: 5040
	public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();

	// Token: 0x040013B1 RID: 5041
	private static Action<KPrefabID> OnAddPrefab;

	// Token: 0x040013B2 RID: 5042
	public static List<BuildingDef> BuildingDefs;

	// Token: 0x040013B3 RID: 5043
	public List<KPrefabID> PrefabAssets = new List<KPrefabID>();

	// Token: 0x040013B4 RID: 5044
	public static List<KPrefabID> Prefabs = new List<KPrefabID>();

	// Token: 0x040013B5 RID: 5045
	private static HashSet<Tag> CountableTags = new HashSet<Tag>();

	// Token: 0x040013B6 RID: 5046
	private static HashSet<Tag> SolidTransferArmConeyableTags = new HashSet<Tag>();

	// Token: 0x040013B7 RID: 5047
	public List<Sprite> SpriteAssets;

	// Token: 0x040013B8 RID: 5048
	public static Dictionary<HashedString, Sprite> Sprites;

	// Token: 0x040013B9 RID: 5049
	public List<string> videoClipNames;

	// Token: 0x040013BA RID: 5050
	private const string VIDEO_ASSET_PATH = "video_webm";

	// Token: 0x040013BB RID: 5051
	public List<TintedSprite> TintedSpriteAssets;

	// Token: 0x040013BC RID: 5052
	public static List<TintedSprite> TintedSprites;

	// Token: 0x040013BD RID: 5053
	public List<Texture2D> TextureAssets;

	// Token: 0x040013BE RID: 5054
	public static List<Texture2D> Textures;

	// Token: 0x040013BF RID: 5055
	public static List<TextureAtlas> TextureAtlases;

	// Token: 0x040013C0 RID: 5056
	public List<TextureAtlas> TextureAtlasAssets;

	// Token: 0x040013C1 RID: 5057
	public static List<Material> Materials;

	// Token: 0x040013C2 RID: 5058
	public List<Material> MaterialAssets;

	// Token: 0x040013C3 RID: 5059
	public static List<Shader> Shaders;

	// Token: 0x040013C4 RID: 5060
	public List<Shader> ShaderAssets;

	// Token: 0x040013C5 RID: 5061
	public static List<BlockTileDecorInfo> BlockTileDecorInfos;

	// Token: 0x040013C6 RID: 5062
	public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;

	// Token: 0x040013C7 RID: 5063
	public Material AnimMaterialAsset;

	// Token: 0x040013C8 RID: 5064
	public static Material AnimMaterial;

	// Token: 0x040013C9 RID: 5065
	public DiseaseVisualization DiseaseVisualization;

	// Token: 0x040013CA RID: 5066
	public Sprite LegendColourBox;

	// Token: 0x040013CB RID: 5067
	public Texture2D invalidAreaTex;

	// Token: 0x040013CC RID: 5068
	public Assets.UIPrefabData UIPrefabAssets;

	// Token: 0x040013CD RID: 5069
	public static Assets.UIPrefabData UIPrefabs;

	// Token: 0x040013CE RID: 5070
	private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();

	// Token: 0x040013CF RID: 5071
	private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();

	// Token: 0x040013D0 RID: 5072
	public List<KAnimFile> AnimAssets;

	// Token: 0x040013D1 RID: 5073
	public static List<KAnimFile> Anims;

	// Token: 0x040013D2 RID: 5074
	private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();

	// Token: 0x040013D3 RID: 5075
	public Font DebugFontAsset;

	// Token: 0x040013D4 RID: 5076
	public static Font DebugFont;

	// Token: 0x040013D5 RID: 5077
	public SubstanceTable substanceTable;

	// Token: 0x040013D6 RID: 5078
	[SerializeField]
	public TextAsset elementAudio;

	// Token: 0x040013D7 RID: 5079
	[SerializeField]
	public TextAsset personalitiesFile;

	// Token: 0x040013D8 RID: 5080
	public LogicModeUI logicModeUIData;

	// Token: 0x040013D9 RID: 5081
	public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;

	// Token: 0x040013DA RID: 5082
	public DigPlacerConfig.DigPlacerAssets digPlacerAssets;

	// Token: 0x040013DB RID: 5083
	public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;

	// Token: 0x040013DC RID: 5084
	public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;

	// Token: 0x040013DD RID: 5085
	public ComicData[] comics;

	// Token: 0x040013DE RID: 5086
	public static Assets instance;

	// Token: 0x040013DF RID: 5087
	private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();

	// Token: 0x02001222 RID: 4642
	[Serializable]
	public struct UIPrefabData
	{
		// Token: 0x04005E7F RID: 24191
		public ProgressBar ProgressBar;

		// Token: 0x04005E80 RID: 24192
		public HealthBar HealthBar;

		// Token: 0x04005E81 RID: 24193
		public GameObject ResourceVisualizer;

		// Token: 0x04005E82 RID: 24194
		public Image RegionCellBlocked;

		// Token: 0x04005E83 RID: 24195
		public RectTransform PriorityOverlayIcon;

		// Token: 0x04005E84 RID: 24196
		public RectTransform HarvestWhenReadyOverlayIcon;

		// Token: 0x04005E85 RID: 24197
		public Assets.TableScreenAssets TableScreenWidgets;
	}

	// Token: 0x02001223 RID: 4643
	[Serializable]
	public struct TableScreenAssets
	{
		// Token: 0x04005E86 RID: 24198
		public Material DefaultUIMaterial;

		// Token: 0x04005E87 RID: 24199
		public Material DesaturatedUIMaterial;

		// Token: 0x04005E88 RID: 24200
		public GameObject MinionPortrait;

		// Token: 0x04005E89 RID: 24201
		public GameObject GenericPortrait;

		// Token: 0x04005E8A RID: 24202
		public GameObject TogglePortrait;

		// Token: 0x04005E8B RID: 24203
		public GameObject ButtonLabel;

		// Token: 0x04005E8C RID: 24204
		public GameObject ButtonLabelWhite;

		// Token: 0x04005E8D RID: 24205
		public GameObject Label;

		// Token: 0x04005E8E RID: 24206
		public GameObject LabelHeader;

		// Token: 0x04005E8F RID: 24207
		public GameObject Checkbox;

		// Token: 0x04005E90 RID: 24208
		public GameObject BlankCell;

		// Token: 0x04005E91 RID: 24209
		public GameObject SuperCheckbox_Horizontal;

		// Token: 0x04005E92 RID: 24210
		public GameObject SuperCheckbox_Vertical;

		// Token: 0x04005E93 RID: 24211
		public GameObject Spacer;

		// Token: 0x04005E94 RID: 24212
		public GameObject NumericDropDown;

		// Token: 0x04005E95 RID: 24213
		public GameObject DropDownHeader;

		// Token: 0x04005E96 RID: 24214
		public GameObject PriorityGroupSelector;

		// Token: 0x04005E97 RID: 24215
		public GameObject PriorityGroupSelectorHeader;

		// Token: 0x04005E98 RID: 24216
		public GameObject PrioritizeRowWidget;

		// Token: 0x04005E99 RID: 24217
		public GameObject PrioritizeRowHeaderWidget;
	}
}

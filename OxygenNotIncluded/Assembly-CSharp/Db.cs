using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class Db : EntityModifierSet
{
	// Token: 0x06002087 RID: 8327 RVA: 0x000AEBD8 File Offset: 0x000ACDD8
	public static string GetPath(string dlcId, string folder)
	{
		string result;
		if (dlcId == "")
		{
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, folder));
		}
		else
		{
			string contentDirectoryName = DlcManager.GetContentDirectoryName(dlcId);
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, "dlc", contentDirectoryName, folder));
		}
		return result;
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x000AEC24 File Offset: 0x000ACE24
	public static Db Get()
	{
		if (Db._Instance == null)
		{
			Db._Instance = Resources.Load<Db>("Db");
			Db._Instance.Initialize();
		}
		return Db._Instance;
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x000AEC51 File Offset: 0x000ACE51
	public static BuildingFacades GetBuildingFacades()
	{
		return Db.Get().Permits.BuildingFacades;
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000AEC62 File Offset: 0x000ACE62
	public static ArtableStages GetArtableStages()
	{
		return Db.Get().Permits.ArtableStages;
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x000AEC73 File Offset: 0x000ACE73
	public static EquippableFacades GetEquippableFacades()
	{
		return Db.Get().Permits.EquippableFacades;
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x000AEC84 File Offset: 0x000ACE84
	public static StickerBombs GetStickerBombs()
	{
		return Db.Get().Permits.StickerBombs;
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x000AEC95 File Offset: 0x000ACE95
	public static MonumentParts GetMonumentParts()
	{
		return Db.Get().Permits.MonumentParts;
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x000AECA8 File Offset: 0x000ACEA8
	public override void Initialize()
	{
		base.Initialize();
		this.Urges = new Urges();
		this.AssignableSlots = new AssignableSlots();
		this.StateMachineCategories = new StateMachineCategories();
		this.Personalities = new Personalities();
		this.Faces = new Faces();
		this.Shirts = new Shirts();
		this.Expressions = new Expressions(this.Root);
		this.Emotes = new Emotes(this.Root);
		this.Thoughts = new Thoughts(this.Root);
		this.Dreams = new Dreams(this.Root);
		this.Deaths = new Deaths(this.Root);
		this.StatusItemCategories = new StatusItemCategories(this.Root);
		this.TechTreeTitles = new TechTreeTitles(this.Root);
		this.TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.Techs = new Techs(this.Root);
		this.TechItems = new TechItems(this.Root);
		this.Techs.Init();
		this.Techs.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.TechItems.Init();
		this.Accessories = new Accessories(this.Root);
		this.AccessorySlots = new AccessorySlots(this.Root);
		this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
		this.ScheduleGroups = new ScheduleGroups(this.Root);
		this.RoomTypeCategories = new RoomTypeCategories(this.Root);
		this.RoomTypes = new RoomTypes(this.Root);
		this.ArtifactDropRates = new ArtifactDropRates(this.Root);
		this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
		this.Diseases = new Diseases(this.Root, false);
		this.Sicknesses = new Database.Sicknesses(this.Root);
		this.SkillPerks = new SkillPerks(this.Root);
		this.SkillGroups = new SkillGroups(this.Root);
		this.Skills = new Skills(this.Root);
		this.ColonyAchievements = new ColonyAchievements(this.Root);
		this.MiscStatusItems = new MiscStatusItems(this.Root);
		this.CreatureStatusItems = new CreatureStatusItems(this.Root);
		this.BuildingStatusItems = new BuildingStatusItems(this.Root);
		this.RobotStatusItems = new RobotStatusItems(this.Root);
		this.ChoreTypes = new ChoreTypes(this.Root);
		this.Quests = new Quests(this.Root);
		this.GameplayEvents = new GameplayEvents(this.Root);
		this.GameplaySeasons = new GameplaySeasons(this.Root);
		this.Stories = new Stories(this.Root);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			this.PlantMutations = new PlantMutations(this.Root);
		}
		this.OrbitalTypeCategories = new OrbitalTypeCategories(this.Root);
		this.ArtableStatuses = new ArtableStatuses(this.Root);
		this.Permits = new PermitResources(this.Root);
		Effect effect = new Effect("CenterOfAttention", DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier("StressDelta", -0.008333334f, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, false, false, true));
		this.effects.Add(effect);
		this.Spices = new Spices(this.Root);
		this.CollectResources(this.Root, this.ResourceTable);
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000AF049 File Offset: 0x000AD249
	public void PostProcess()
	{
		this.Techs.PostProcess();
		this.Permits.PostProcess();
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x000AF064 File Offset: 0x000AD264
	private void CollectResources(Resource resource, List<Resource> resource_table)
	{
		if (resource.Guid != null)
		{
			resource_table.Add(resource);
		}
		ResourceSet resourceSet = resource as ResourceSet;
		if (resourceSet != null)
		{
			for (int i = 0; i < resourceSet.Count; i++)
			{
				this.CollectResources(resourceSet.GetResource(i), resource_table);
			}
		}
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x000AF0B0 File Offset: 0x000AD2B0
	public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
	{
		Resource resource = this.ResourceTable.FirstOrDefault((Resource s) => s.Guid == guid);
		if (resource == null)
		{
			string str = "Could not find resource: ";
			ResourceGuid guid2 = guid;
			global::Debug.LogWarning(str + ((guid2 != null) ? guid2.ToString() : null));
			return default(ResourceType);
		}
		ResourceType resourceType = (ResourceType)((object)resource);
		if (resourceType == null)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Resource type mismatch for resource: ",
				resource.Id,
				"\nExpecting Type: ",
				typeof(ResourceType).Name,
				"\nGot Type: ",
				resource.GetType().Name
			}));
			return default(ResourceType);
		}
		return resourceType;
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000AF17B File Offset: 0x000AD37B
	public void ResetProblematicDbs()
	{
		this.Emotes.ResetProblematicReferences();
	}

	// Token: 0x04001230 RID: 4656
	private static Db _Instance;

	// Token: 0x04001231 RID: 4657
	public TextAsset researchTreeFileVanilla;

	// Token: 0x04001232 RID: 4658
	public TextAsset researchTreeFileExpansion1;

	// Token: 0x04001233 RID: 4659
	public Diseases Diseases;

	// Token: 0x04001234 RID: 4660
	public Database.Sicknesses Sicknesses;

	// Token: 0x04001235 RID: 4661
	public Urges Urges;

	// Token: 0x04001236 RID: 4662
	public AssignableSlots AssignableSlots;

	// Token: 0x04001237 RID: 4663
	public StateMachineCategories StateMachineCategories;

	// Token: 0x04001238 RID: 4664
	public Personalities Personalities;

	// Token: 0x04001239 RID: 4665
	public Faces Faces;

	// Token: 0x0400123A RID: 4666
	public Shirts Shirts;

	// Token: 0x0400123B RID: 4667
	public Expressions Expressions;

	// Token: 0x0400123C RID: 4668
	public Emotes Emotes;

	// Token: 0x0400123D RID: 4669
	public Thoughts Thoughts;

	// Token: 0x0400123E RID: 4670
	public Dreams Dreams;

	// Token: 0x0400123F RID: 4671
	public BuildingStatusItems BuildingStatusItems;

	// Token: 0x04001240 RID: 4672
	public MiscStatusItems MiscStatusItems;

	// Token: 0x04001241 RID: 4673
	public CreatureStatusItems CreatureStatusItems;

	// Token: 0x04001242 RID: 4674
	public RobotStatusItems RobotStatusItems;

	// Token: 0x04001243 RID: 4675
	public StatusItemCategories StatusItemCategories;

	// Token: 0x04001244 RID: 4676
	public Deaths Deaths;

	// Token: 0x04001245 RID: 4677
	public ChoreTypes ChoreTypes;

	// Token: 0x04001246 RID: 4678
	public TechItems TechItems;

	// Token: 0x04001247 RID: 4679
	public AccessorySlots AccessorySlots;

	// Token: 0x04001248 RID: 4680
	public Accessories Accessories;

	// Token: 0x04001249 RID: 4681
	public ScheduleBlockTypes ScheduleBlockTypes;

	// Token: 0x0400124A RID: 4682
	public ScheduleGroups ScheduleGroups;

	// Token: 0x0400124B RID: 4683
	public RoomTypeCategories RoomTypeCategories;

	// Token: 0x0400124C RID: 4684
	public RoomTypes RoomTypes;

	// Token: 0x0400124D RID: 4685
	public ArtifactDropRates ArtifactDropRates;

	// Token: 0x0400124E RID: 4686
	public SpaceDestinationTypes SpaceDestinationTypes;

	// Token: 0x0400124F RID: 4687
	public SkillPerks SkillPerks;

	// Token: 0x04001250 RID: 4688
	public SkillGroups SkillGroups;

	// Token: 0x04001251 RID: 4689
	public Skills Skills;

	// Token: 0x04001252 RID: 4690
	public ColonyAchievements ColonyAchievements;

	// Token: 0x04001253 RID: 4691
	public Quests Quests;

	// Token: 0x04001254 RID: 4692
	public GameplayEvents GameplayEvents;

	// Token: 0x04001255 RID: 4693
	public GameplaySeasons GameplaySeasons;

	// Token: 0x04001256 RID: 4694
	public PlantMutations PlantMutations;

	// Token: 0x04001257 RID: 4695
	public Spices Spices;

	// Token: 0x04001258 RID: 4696
	public Techs Techs;

	// Token: 0x04001259 RID: 4697
	public TechTreeTitles TechTreeTitles;

	// Token: 0x0400125A RID: 4698
	public OrbitalTypeCategories OrbitalTypeCategories;

	// Token: 0x0400125B RID: 4699
	public PermitResources Permits;

	// Token: 0x0400125C RID: 4700
	public ArtableStatuses ArtableStatuses;

	// Token: 0x0400125D RID: 4701
	public Stories Stories;

	// Token: 0x020011E5 RID: 4581
	[Serializable]
	public class SlotInfo : Resource
	{
	}
}

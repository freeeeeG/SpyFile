using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000889 RID: 2185
[SerializationConfig(MemberSerialization.OptIn)]
public class MutantPlant : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06003F86 RID: 16262 RVA: 0x00162F6A File Offset: 0x0016116A
	public List<string> MutationIDs
	{
		get
		{
			return this.mutationIDs;
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06003F87 RID: 16263 RVA: 0x00162F72 File Offset: 0x00161172
	public bool IsOriginal
	{
		get
		{
			return this.mutationIDs == null || this.mutationIDs.Count == 0;
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06003F88 RID: 16264 RVA: 0x00162F8C File Offset: 0x0016118C
	public bool IsIdentified
	{
		get
		{
			return this.analyzed && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.SubSpeciesID);
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06003F89 RID: 16265 RVA: 0x00162FA8 File Offset: 0x001611A8
	// (set) Token: 0x06003F8A RID: 16266 RVA: 0x00162FCB File Offset: 0x001611CB
	public Tag SpeciesID
	{
		get
		{
			global::Debug.Assert(this.speciesID != null, "Ack, forgot to configure the species ID for this mutantPlant!");
			return this.speciesID;
		}
		set
		{
			this.speciesID = value;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06003F8B RID: 16267 RVA: 0x00162FD4 File Offset: 0x001611D4
	public Tag SubSpeciesID
	{
		get
		{
			if (this.cachedSubspeciesID == null)
			{
				this.cachedSubspeciesID = this.GetSubSpeciesInfo().ID;
			}
			return this.cachedSubspeciesID;
		}
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x00163000 File Offset: 0x00161200
	protected override void OnPrefabInit()
	{
		base.Subscribe<MutantPlant>(-2064133523, MutantPlant.OnAbsorbDelegate);
		base.Subscribe<MutantPlant>(1335436905, MutantPlant.OnSplitFromChunkDelegate);
	}

	// Token: 0x06003F8D RID: 16269 RVA: 0x00163024 File Offset: 0x00161224
	protected override void OnSpawn()
	{
		if (this.IsOriginal || this.HasTag(GameTags.Plant))
		{
			this.analyzed = true;
		}
		if (!this.IsOriginal)
		{
			this.AddTag(GameTags.MutatedSeed);
		}
		this.AddTag(this.SubSpeciesID);
		Components.MutantPlants.Add(this);
		base.OnSpawn();
		this.ApplyMutations();
		this.UpdateNameAndTags();
		PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(this.GetSubSpeciesInfo(), this);
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x0016309A File Offset: 0x0016129A
	protected override void OnCleanUp()
	{
		Components.MutantPlants.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003F8F RID: 16271 RVA: 0x001630B0 File Offset: 0x001612B0
	private void OnAbsorb(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		global::Debug.Assert(component != null && this.SubSpeciesID == component.SubSpeciesID, "Two seeds of different subspecies just absorbed!");
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x001630F0 File Offset: 0x001612F0
	private void OnSplitFromChunk(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		if (component != null)
		{
			component.CopyMutationsTo(this);
		}
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x0016311C File Offset: 0x0016131C
	public void Mutate()
	{
		List<string> list = (this.mutationIDs != null) ? new List<string>(this.mutationIDs) : new List<string>();
		while (list.Count >= 1 && list.Count > 0)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		list.Add(Db.Get().PlantMutations.GetRandomMutation(this.PrefabID().Name).Id);
		this.SetSubSpecies(list);
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00163199 File Offset: 0x00161399
	public void Analyze()
	{
		this.analyzed = true;
		this.UpdateNameAndTags();
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x001631A8 File Offset: 0x001613A8
	public void ApplyMutations()
	{
		if (this.IsOriginal)
		{
			return;
		}
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).ApplyTo(this);
		}
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x00163214 File Offset: 0x00161414
	public void DummySetSubspecies(List<string> mutations)
	{
		this.mutationIDs = mutations;
	}

	// Token: 0x06003F95 RID: 16277 RVA: 0x0016321D File Offset: 0x0016141D
	public void SetSubSpecies(List<string> mutations)
	{
		if (base.gameObject.HasTag(this.SubSpeciesID))
		{
			base.gameObject.RemoveTag(this.SubSpeciesID);
		}
		this.cachedSubspeciesID = Tag.Invalid;
		this.mutationIDs = mutations;
		this.UpdateNameAndTags();
	}

	// Token: 0x06003F96 RID: 16278 RVA: 0x0016325B File Offset: 0x0016145B
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpeciesInfo()
	{
		return new PlantSubSpeciesCatalog.SubSpeciesInfo(this.SpeciesID, this.mutationIDs);
	}

	// Token: 0x06003F97 RID: 16279 RVA: 0x0016326E File Offset: 0x0016146E
	public void CopyMutationsTo(MutantPlant target)
	{
		target.SetSubSpecies(this.mutationIDs);
		target.analyzed = this.analyzed;
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x00163288 File Offset: 0x00161488
	public void UpdateNameAndTags()
	{
		bool flag = !base.IsInitialized() || this.IsIdentified;
		bool flag2 = PlantSubSpeciesCatalog.Instance == null || PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(this.SpeciesID).Count == 1;
		KPrefabID component = base.GetComponent<KPrefabID>();
		component.AddTag(this.SubSpeciesID, false);
		component.SetTag(GameTags.UnidentifiedSeed, !flag);
		base.gameObject.name = component.PrefabTag.ToString() + " (" + this.SubSpeciesID.ToString() + ")";
		base.GetComponent<KSelectable>().SetName(this.GetSubSpeciesInfo().GetNameWithMutations(component.PrefabTag.ProperName(), flag, flag2));
		KSelectable component2 = base.GetComponent<KSelectable>();
		foreach (Guid guid in this.statusItemHandles)
		{
			component2.RemoveStatusItem(guid, false);
		}
		this.statusItemHandles.Clear();
		if (!flag2)
		{
			if (this.IsOriginal)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.OriginalPlantMutation, null));
				return;
			}
			if (!flag)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.UnknownMutation, null));
				return;
			}
			foreach (string id in this.mutationIDs)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.SpecificPlantMutation, Db.Get().PlantMutations.Get(id)));
			}
		}
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x0016347C File Offset: 0x0016167C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.IsOriginal)
		{
			return null;
		}
		List<Descriptor> result = new List<Descriptor>();
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).GetDescriptors(ref result, go);
		}
		return result;
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x001634F4 File Offset: 0x001616F4
	public List<string> GetSoundEvents()
	{
		List<string> list = new List<string>();
		if (this.mutationIDs != null)
		{
			foreach (string id in this.mutationIDs)
			{
				PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
				list.AddRange(plantMutation.AdditionalSoundEvents);
			}
		}
		return list;
	}

	// Token: 0x04002942 RID: 10562
	[Serialize]
	private bool analyzed;

	// Token: 0x04002943 RID: 10563
	[Serialize]
	private List<string> mutationIDs;

	// Token: 0x04002944 RID: 10564
	private List<Guid> statusItemHandles = new List<Guid>();

	// Token: 0x04002945 RID: 10565
	private const int MAX_MUTATIONS = 1;

	// Token: 0x04002946 RID: 10566
	[SerializeField]
	private Tag speciesID;

	// Token: 0x04002947 RID: 10567
	private Tag cachedSubspeciesID;

	// Token: 0x04002948 RID: 10568
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04002949 RID: 10569
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnSplitFromChunk(data);
	});
}

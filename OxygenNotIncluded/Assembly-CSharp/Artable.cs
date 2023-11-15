using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200047E RID: 1150
[AddComponentMenu("KMonoBehaviour/Workable/Artable")]
public class Artable : Workable
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x0600193D RID: 6461 RVA: 0x000840F0 File Offset: 0x000822F0
	public string CurrentStage
	{
		get
		{
			return this.currentStage;
		}
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000840F8 File Offset: 0x000822F8
	protected Artable()
	{
		this.faceTargetWhenWorking = true;
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x00084114 File Offset: 0x00082314
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanArt.Id;
		base.SetWorkTime(80f);
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x000841A8 File Offset: 0x000823A8
	protected override void OnSpawn()
	{
		base.GetComponent<KPrefabID>().PrefabID();
		if (string.IsNullOrEmpty(this.currentStage) || this.currentStage == this.defaultArtworkId)
		{
			this.SetDefault();
		}
		else
		{
			this.SetStage(this.currentStage, true);
		}
		this.shouldShowSkillPerkStatusItem = false;
		base.OnSpawn();
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x00084204 File Offset: 0x00082404
	[OnDeserialized]
	public void OnDeserialized()
	{
		if (Db.GetArtableStages().TryGet(this.currentStage) == null && this.currentStage != this.defaultArtworkId)
		{
			string id = string.Format("{0}_{1}", base.GetComponent<KPrefabID>().PrefabID().ToString(), this.currentStage);
			if (Db.GetArtableStages().TryGet(id) == null)
			{
				global::Debug.LogError("Failed up to update " + this.currentStage + " to ArtableStages");
				return;
			}
			this.currentStage = id;
		}
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x00084290 File Offset: 0x00082490
	protected override void OnCompleteWork(Worker worker)
	{
		if (string.IsNullOrEmpty(this.userChosenTargetStage))
		{
			Db db = Db.Get();
			Tag prefab_id = base.GetComponent<KPrefabID>().PrefabID();
			List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(prefab_id);
			ArtableStatusItem artist_skill = db.ArtableStatuses.LookingUgly;
			MinionResume component = worker.GetComponent<MinionResume>();
			if (component != null)
			{
				if (component.HasPerk(db.SkillPerks.CanArtGreat.Id))
				{
					artist_skill = db.ArtableStatuses.LookingGreat;
				}
				else if (component.HasPerk(db.SkillPerks.CanArtOkay.Id))
				{
					artist_skill = db.ArtableStatuses.LookingOkay;
				}
			}
			prefabStages.RemoveAll((ArtableStage stage) => stage.statusItem.StatusType > artist_skill.StatusType || stage.statusItem.StatusType == ArtableStatuses.ArtableStatusType.AwaitingArting);
			prefabStages.Sort((ArtableStage x, ArtableStage y) => y.statusItem.StatusType.CompareTo(x.statusItem.StatusType));
			ArtableStatuses.ArtableStatusType highest_type = prefabStages[0].statusItem.StatusType;
			prefabStages.RemoveAll((ArtableStage stage) => stage.statusItem.StatusType < highest_type);
			prefabStages.RemoveAll((ArtableStage stage) => !stage.IsUnlocked());
			prefabStages.Shuffle<ArtableStage>();
			this.SetStage(prefabStages[0].id, false);
			if (prefabStages[0].cheerOnComplete)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
			}
			else
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Disappointed, 1, null);
			}
		}
		else
		{
			this.SetStage(this.userChosenTargetStage, false);
			this.userChosenTargetStage = null;
		}
		this.shouldShowSkillPerkStatusItem = false;
		this.UpdateStatusItem(null);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x00084488 File Offset: 0x00082688
	public void SetDefault()
	{
		this.currentStage = this.defaultArtworkId;
		base.GetComponent<KBatchedAnimController>().SwapAnims(base.GetComponent<Building>().Def.AnimFiles);
		base.GetComponent<KAnimControllerBase>().Play(this.defaultAnimName, KAnim.PlayMode.Once, 1f, 0f);
		KSelectable component = base.GetComponent<KSelectable>();
		BuildingDef def = base.GetComponent<Building>().Def;
		component.SetName(def.Name);
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().ArtableStatuses.AwaitingArting, this);
		this.GetAttributes().Remove(this.artQualityDecorModifier);
		this.shouldShowSkillPerkStatusItem = false;
		this.UpdateStatusItem(null);
		if (this.currentStage == this.defaultArtworkId)
		{
			this.shouldShowSkillPerkStatusItem = true;
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Artable>(Db.Get().ChoreTypes.Art, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
		}
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000845AC File Offset: 0x000827AC
	public virtual void SetStage(string stage_id, bool skip_effect)
	{
		ArtableStage artableStage = Db.GetArtableStages().Get(stage_id);
		if (artableStage == null)
		{
			global::Debug.LogError("Missing stage: " + stage_id);
			return;
		}
		this.currentStage = artableStage.id;
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artableStage.animFile)
		});
		base.GetComponent<KAnimControllerBase>().Play(artableStage.anim, KAnim.PlayMode.Once, 1f, 0f);
		this.GetAttributes().Remove(this.artQualityDecorModifier);
		if (artableStage.decor != 0)
		{
			this.artQualityDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, (float)artableStage.decor, "Art Quality", false, false, true);
			this.GetAttributes().Add(this.artQualityDecorModifier);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		component.SetName(artableStage.Name);
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, artableStage.statusItem, this);
		this.shouldShowSkillPerkStatusItem = false;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x000846BE File Offset: 0x000828BE
	public void SetUserChosenTargetState(string stageID)
	{
		this.SetDefault();
		this.userChosenTargetStage = stageID;
	}

	// Token: 0x04000DEB RID: 3563
	[Serialize]
	private string currentStage;

	// Token: 0x04000DEC RID: 3564
	[Serialize]
	private string userChosenTargetStage;

	// Token: 0x04000DED RID: 3565
	private AttributeModifier artQualityDecorModifier;

	// Token: 0x04000DEE RID: 3566
	private string defaultArtworkId = "Default";

	// Token: 0x04000DEF RID: 3567
	public string defaultAnimName;

	// Token: 0x04000DF0 RID: 3568
	private WorkChore<Artable> chore;
}

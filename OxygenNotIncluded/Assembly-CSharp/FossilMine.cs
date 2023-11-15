﻿using System;
using STRINGS;
using TUNING;

// Token: 0x0200019D RID: 413
public class FossilMine : ComplexFabricator
{
	// Token: 0x06000828 RID: 2088 RVA: 0x0002FE00 File Offset: 0x0002E000
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.fabricatorSM.idleAnimationName = "idle";
		this.fabricatorSM.idleQueue_StatusItem = Db.Get().BuildingStatusItems.FossilMineIdle;
		this.fabricatorSM.waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FossilMineEmpty;
		this.fabricatorSM.waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.FossilMinePendingWork;
		this.SideScreenSubtitleLabel = CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_LIST_TITLE;
		this.SideScreenRecipeScreenTitle = CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FABRICATOR_RECIPE_SCREEN_TITLE;
		this.choreType = Db.Get().ChoreTypes.Art;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0002FEA8 File Offset: 0x0002E0A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Digging;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fossil_dig_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0002FF70 File Offset: 0x0002E170
	public void SetActiveState(bool active)
	{
		if (active)
		{
			this.inStorage.showInUI = true;
			this.buildStorage.showInUI = true;
			this.outStorage.showInUI = true;
			this.fabricatorSM.Activate();
			if (this.workable is FossilMineWorkable)
			{
				(this.workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(true);
			}
			base.enabled = active;
			return;
		}
		base.OnDisable();
		this.fabricatorSM.Deactivate();
		this.inStorage.showInUI = false;
		this.buildStorage.showInUI = false;
		this.outStorage.showInUI = false;
		if (this.workable is FossilMineWorkable)
		{
			(this.workable as FossilMineWorkable).SetShouldShowSkillPerkStatusItem(false);
		}
		base.enabled = false;
	}

	// Token: 0x04000543 RID: 1347
	[MyCmpAdd]
	protected new FossilMineSM fabricatorSM;
}

using System;
using TUNING;

// Token: 0x0200093A RID: 2362
public class ArmTrapWorkable : Workable
{
	// Token: 0x060044A8 RID: 17576 RVA: 0x00182454 File Offset: 0x00180654
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.CanBeArmedAtLongDistance)
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			this.faceTargetWhenWorking = true;
			this.multitoolContext = "build";
			this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		}
		if (this.initialOffsets != null && this.initialOffsets.Length != 0)
		{
			base.SetOffsets(this.initialOffsets);
		}
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.ArmingTrap);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(5f);
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x00182531 File Offset: 0x00180731
	public override void OnPendingCompleteWork(Worker worker)
	{
		base.OnPendingCompleteWork(worker);
		this.WorkInPstAnimation = true;
		base.gameObject.Trigger(-2025798095, null);
	}

	// Token: 0x060044AA RID: 17578 RVA: 0x00182552 File Offset: 0x00180752
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.WorkInPstAnimation = false;
	}

	// Token: 0x04002D73 RID: 11635
	public bool WorkInPstAnimation;

	// Token: 0x04002D74 RID: 11636
	public bool CanBeArmedAtLongDistance;

	// Token: 0x04002D75 RID: 11637
	public CellOffset[] initialOffsets;
}

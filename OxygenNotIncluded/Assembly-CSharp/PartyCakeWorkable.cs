using System;
using TUNING;

// Token: 0x0200066D RID: 1645
public class PartyCakeWorkable : Workable
{
	// Token: 0x06002B82 RID: 11138 RVA: 0x000E748C File Offset: 0x000E568C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = PartyCakeWorkable.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x000E7542 File Offset: 0x000E5742
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		base.GetComponent<KBatchedAnimController>().SetPositionPercent(this.GetPercentComplete());
		return false;
	}

	// Token: 0x04001981 RID: 6529
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04001982 RID: 6530
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}

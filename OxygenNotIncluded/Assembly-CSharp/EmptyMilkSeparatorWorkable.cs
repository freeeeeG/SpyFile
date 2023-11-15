using System;
using TUNING;

// Token: 0x02000655 RID: 1621
public class EmptyMilkSeparatorWorkable : Workable
{
	// Token: 0x06002ABF RID: 10943 RVA: 0x000E3CC4 File Offset: 0x000E1EC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workLayer = Grid.SceneLayer.BuildingFront;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_milk_separator_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(15f);
		this.synchronizeAnims = true;
	}

	// Token: 0x06002AC0 RID: 10944 RVA: 0x000E3D64 File Offset: 0x000E1F64
	public override void OnPendingCompleteWork(Worker worker)
	{
		System.Action onWork_PST_Begins = this.OnWork_PST_Begins;
		if (onWork_PST_Begins != null)
		{
			onWork_PST_Begins();
		}
		base.OnPendingCompleteWork(worker);
	}

	// Token: 0x040018F3 RID: 6387
	public System.Action OnWork_PST_Begins;
}

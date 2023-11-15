using System;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000694 RID: 1684
public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
	// Token: 0x06002D27 RID: 11559 RVA: 0x000EFAE4 File Offset: 0x000EDCE4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_spice_grinder_kanim")
		};
		base.SetWorkTime(5f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x000EFBA4 File Offset: 0x000EDDA4
	protected override void OnStartWork(Worker worker)
	{
		if (this.Grinder.CurrentFood != null)
		{
			float num = this.Grinder.CurrentFood.Calories * 0.001f / 1000f;
			base.SetWorkTime(num * 5f);
		}
		else
		{
			global::Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
			base.StopWork(worker, true);
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x000EFC0D File Offset: 0x000EDE0D
	protected override void OnAbortWork(Worker worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x000EFC2E File Offset: 0x000EDE2E
	protected override void OnCompleteWork(Worker worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.SpiceFood();
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x000EFC50 File Offset: 0x000EDE50
	public IConfigurableConsumerOption[] GetSettingOptions()
	{
		return SpiceGrinder.SettingOptions.Values.ToArray<SpiceGrinder.Option>();
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x000EFC6E File Offset: 0x000EDE6E
	public IConfigurableConsumerOption GetSelectedOption()
	{
		return this.Grinder.SelectedOption;
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x000EFC7B File Offset: 0x000EDE7B
	public void SetSelectedOption(IConfigurableConsumerOption option)
	{
		this.Grinder.OnOptionSelected(option as SpiceGrinder.Option);
	}

	// Token: 0x04001AA5 RID: 6821
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001AA6 RID: 6822
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04001AA7 RID: 6823
	public SpiceGrinder.StatesInstance Grinder;
}

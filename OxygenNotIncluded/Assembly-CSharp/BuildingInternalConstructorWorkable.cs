using System;
using TUNING;

// Token: 0x020005AE RID: 1454
public class BuildingInternalConstructorWorkable : Workable
{
	// Token: 0x060023A0 RID: 9120 RVA: 0x000C3138 File Offset: 0x000C1338
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.resetProgressOnStop = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000C31DB File Offset: 0x000C13DB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.constructorInstance = this.GetSMI<BuildingInternalConstructor.Instance>();
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000C31EF File Offset: 0x000C13EF
	protected override void OnCompleteWork(Worker worker)
	{
		this.constructorInstance.ConstructionComplete(false);
	}

	// Token: 0x04001458 RID: 5208
	private BuildingInternalConstructor.Instance constructorInstance;
}

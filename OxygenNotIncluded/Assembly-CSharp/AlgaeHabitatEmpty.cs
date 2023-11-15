using System;
using TUNING;
using UnityEngine;

// Token: 0x020005B6 RID: 1462
[AddComponentMenu("KMonoBehaviour/Workable/AlgaeHabitatEmpty")]
public class AlgaeHabitatEmpty : Workable
{
	// Token: 0x060023CB RID: 9163 RVA: 0x000C3E8C File Offset: 0x000C208C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.workAnims = AlgaeHabitatEmpty.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x04001480 RID: 5248
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"sponge_pre",
		"sponge_loop"
	};

	// Token: 0x04001481 RID: 5249
	private static readonly HashedString PST_ANIM = new HashedString("sponge_pst");
}

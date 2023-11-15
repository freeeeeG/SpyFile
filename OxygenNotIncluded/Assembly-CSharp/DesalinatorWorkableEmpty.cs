using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
[AddComponentMenu("KMonoBehaviour/Workable/DesalinatorWorkableEmpty")]
public class DesalinatorWorkableEmpty : Workable
{
	// Token: 0x060025AF RID: 9647 RVA: 0x000CCB9C File Offset: 0x000CAD9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = DesalinatorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x000CCC59 File Offset: 0x000CAE59
	protected override void OnCompleteWork(Worker worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x0400158A RID: 5514
	[Serialize]
	public int timesCleaned;

	// Token: 0x0400158B RID: 5515
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x0400158C RID: 5516
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}

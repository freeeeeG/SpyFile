using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
[AddComponentMenu("KMonoBehaviour/Workable/Disinfectable")]
public class Disinfectable : Workable
{
	// Token: 0x06001B03 RID: 6915 RVA: 0x00090F8C File Offset: 0x0008F18C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		base.Subscribe<Disinfectable>(2127324410, Disinfectable.OnCancelDelegate);
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x00091043 File Offset: 0x0008F243
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDisinfect)
		{
			this.MarkForDisinfect(true);
		}
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x0009106C File Offset: 0x0008F26C
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x0009108D File Offset: 0x0008F28D
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x000910C0 File Offset: 0x0008F2C0
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.isMarkedForDisinfect = false;
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x00091142 File Offset: 0x0008F342
	private void ToggleMarkForDisinfect()
	{
		if (this.isMarkedForDisinfect)
		{
			this.CancelDisinfection();
			return;
		}
		base.SetWorkTime(10f);
		this.MarkForDisinfect(false);
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x00091168 File Offset: 0x0008F368
	private void CancelDisinfection()
	{
		if (this.isMarkedForDisinfect)
		{
			Prioritizable.RemoveRef(base.gameObject);
			base.ShowProgressBar(false);
			this.isMarkedForDisinfect = false;
			this.chore.Cancel("disinfection cancelled");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000911D0 File Offset: 0x0008F3D0
	public void MarkForDisinfect(bool force = false)
	{
		if (!this.isMarkedForDisinfect || force)
		{
			this.isMarkedForDisinfect = true;
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Disinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x00091244 File Offset: 0x0008F444
	private void OnCancel(object data)
	{
		this.CancelDisinfection();
	}

	// Token: 0x04000F01 RID: 3841
	private Chore chore;

	// Token: 0x04000F02 RID: 3842
	[Serialize]
	private bool isMarkedForDisinfect;

	// Token: 0x04000F03 RID: 3843
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x04000F04 RID: 3844
	private float diseasePerSecond;

	// Token: 0x04000F05 RID: 3845
	private static readonly EventSystem.IntraObjectHandler<Disinfectable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Disinfectable>(delegate(Disinfectable component, object data)
	{
		component.OnCancel(data);
	});
}

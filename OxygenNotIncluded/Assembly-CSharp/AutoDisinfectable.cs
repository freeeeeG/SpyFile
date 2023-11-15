using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000480 RID: 1152
[AddComponentMenu("KMonoBehaviour/Workable/AutoDisinfectable")]
public class AutoDisinfectable : Workable
{
	// Token: 0x0600194E RID: 6478 RVA: 0x0008493C File Offset: 0x00082B3C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.resetProgressOnStop = true;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000849A4 File Offset: 0x00082BA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<AutoDisinfectable>(493375141, AutoDisinfectable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x00084A1F File Offset: 0x00082C1F
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("AutoDisinfectable.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x00084A40 File Offset: 0x00082C40
	public void RefreshChore()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.enableAutoDisinfect || !SaveGame.Instance.enableAutoDisinfect)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Autodisinfect Disabled");
				this.chore = null;
				return;
			}
		}
		else if (this.chore == null || !(this.chore.driver != null))
		{
			int diseaseCount = this.primaryElement.DiseaseCount;
			if (this.chore == null && diseaseCount > SaveGame.Instance.minGermCountForDisinfect)
			{
				this.chore = new WorkChore<AutoDisinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
				return;
			}
			if (diseaseCount < SaveGame.Instance.minGermCountForDisinfect && this.chore != null)
			{
				this.chore.Cancel("AutoDisinfectable.Update");
				this.chore = null;
			}
		}
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x00084B21 File Offset: 0x00082D21
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x00084B42 File Offset: 0x00082D42
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x00084B74 File Offset: 0x00082D74
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x00084BE4 File Offset: 0x00082DE4
	private void EnableAutoDisinfect()
	{
		this.enableAutoDisinfect = true;
		this.RefreshChore();
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x00084BF3 File Offset: 0x00082DF3
	private void DisableAutoDisinfect()
	{
		this.enableAutoDisinfect = false;
		this.RefreshChore();
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x00084C04 File Offset: 0x00082E04
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.enableAutoDisinfect)
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.NAME, new System.Action(this.EnableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.NAME, new System.Action(this.DisableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 10f);
	}

	// Token: 0x04000DF6 RID: 3574
	private Chore chore;

	// Token: 0x04000DF7 RID: 3575
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x04000DF8 RID: 3576
	private float diseasePerSecond;

	// Token: 0x04000DF9 RID: 3577
	[MyCmpGet]
	private PrimaryElement primaryElement;

	// Token: 0x04000DFA RID: 3578
	[Serialize]
	private bool enableAutoDisinfect = true;

	// Token: 0x04000DFB RID: 3579
	private static readonly EventSystem.IntraObjectHandler<AutoDisinfectable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<AutoDisinfectable>(delegate(AutoDisinfectable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}

using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000511 RID: 1297
[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000A6619 File Offset: 0x000A4819
	public bool Studied
	{
		get
		{
			return this.studied;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06001F1C RID: 7964 RVA: 0x000A6621 File Offset: 0x000A4821
	public bool Studying
	{
		get
		{
			return this.chore != null && this.chore.InProgress();
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06001F1D RID: 7965 RVA: 0x000A6638 File Offset: 0x000A4838
	public string SidescreenTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06001F1E RID: 7966 RVA: 0x000A663F File Offset: 0x000A483F
	public string SidescreenStatusMessage
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06001F1F RID: 7967 RVA: 0x000A6671 File Offset: 0x000A4871
	public string SidescreenButtonText
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06001F20 RID: 7968 RVA: 0x000A66A3 File Offset: 0x000A48A3
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x000A66D5 File Offset: 0x000A48D5
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x000A66D8 File Offset: 0x000A48D8
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x000A66DF File Offset: 0x000A48DF
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x000A66E2 File Offset: 0x000A48E2
	public bool SidescreenButtonInteractable()
	{
		return !this.studied;
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x000A66F0 File Offset: 0x000A48F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
		this.resetProgressOnStop = false;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		base.SetWorkTime(3600f);
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x000A67B8 File Offset: 0x000A49B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.studiedIndicator = new MeterController(base.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			this.meterTrackerSymbol
		});
		this.studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
		this.Refresh();
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x000A6816 File Offset: 0x000A4A16
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Studyable.CancelChore");
			this.chore = null;
			base.Trigger(1488501379, null);
		}
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x000A6844 File Offset: 0x000A4A44
	public void Refresh()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.studied)
		{
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied, null);
			this.studiedIndicator.gameObject.SetActive(true);
			this.studiedIndicator.meterController.Play(this.meterAnim, KAnim.PlayMode.Loop, 1f, 0f);
			this.requiredSkillPerk = null;
			this.UpdateStatusItem(null);
			return;
		}
		if (this.markedForStudy)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy, null);
		}
		else
		{
			this.CancelChore();
			this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
		}
		this.studiedIndicator.gameObject.SetActive(false);
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x000A695C File Offset: 0x000A4B5C
	private void ToggleStudyChore()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.studied = true;
			if (this.chore != null)
			{
				this.chore.Cancel("debug");
				this.chore = null;
			}
			base.Trigger(-1436775550, null);
		}
		else
		{
			this.markedForStudy = !this.markedForStudy;
		}
		this.Refresh();
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x000A69B9 File Offset: 0x000A4BB9
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.studied = true;
		this.chore = null;
		this.Refresh();
		base.Trigger(-1436775550, null);
		if (DlcManager.IsExpansion1Active())
		{
			this.DropDatabanks();
		}
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x000A69F0 File Offset: 0x000A4BF0
	private void DropDatabanks()
	{
		int num = UnityEngine.Random.Range(7, 13);
		for (int i = 0; i <= num; i++)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), base.transform.position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
			gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x000A6A64 File Offset: 0x000A4C64
	public void OnSidescreenButtonPressed()
	{
		this.ToggleStudyChore();
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x000A6A6C File Offset: 0x000A4C6C
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04001184 RID: 4484
	public string meterTrackerSymbol;

	// Token: 0x04001185 RID: 4485
	public string meterAnim;

	// Token: 0x04001186 RID: 4486
	private Chore chore;

	// Token: 0x04001187 RID: 4487
	private const float STUDY_WORK_TIME = 3600f;

	// Token: 0x04001188 RID: 4488
	[Serialize]
	private bool studied;

	// Token: 0x04001189 RID: 4489
	[Serialize]
	private bool markedForStudy;

	// Token: 0x0400118A RID: 4490
	private Guid statusItemGuid;

	// Token: 0x0400118B RID: 4491
	private Guid additionalStatusItemGuid;

	// Token: 0x0400118C RID: 4492
	public MeterController studiedIndicator;
}

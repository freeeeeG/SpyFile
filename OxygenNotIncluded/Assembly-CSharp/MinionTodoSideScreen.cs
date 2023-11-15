using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C2D RID: 3117
public class MinionTodoSideScreen : SideScreenContent
{
	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x0600629A RID: 25242 RVA: 0x0024674C File Offset: 0x0024494C
	public static List<JobsTableScreen.PriorityInfo> priorityInfo
	{
		get
		{
			if (MinionTodoSideScreen._priorityInfo == null)
			{
				MinionTodoSideScreen._priorityInfo = new List<JobsTableScreen.PriorityInfo>
				{
					new JobsTableScreen.PriorityInfo(4, Assets.GetSprite("ic_dupe"), UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY),
					new JobsTableScreen.PriorityInfo(3, Assets.GetSprite("notification_exclamation"), UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY),
					new JobsTableScreen.PriorityInfo(2, Assets.GetSprite("status_item_room_required"), UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS),
					new JobsTableScreen.PriorityInfo(1, Assets.GetSprite("status_item_prioritized"), UI.JOBSSCREEN.PRIORITY_CLASS.HIGH),
					new JobsTableScreen.PriorityInfo(0, null, UI.JOBSSCREEN.PRIORITY_CLASS.BASIC),
					new JobsTableScreen.PriorityInfo(-1, Assets.GetSprite("icon_gear"), UI.JOBSSCREEN.PRIORITY_CLASS.IDLE)
				};
			}
			return MinionTodoSideScreen._priorityInfo;
		}
	}

	// Token: 0x0600629B RID: 25243 RVA: 0x00246824 File Offset: 0x00244A24
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.priorityGroups.Count != 0)
		{
			return;
		}
		foreach (JobsTableScreen.PriorityInfo priorityInfo in MinionTodoSideScreen.priorityInfo)
		{
			PriorityScreen.PriorityClass priority = (PriorityScreen.PriorityClass)priorityInfo.priority;
			if (priority == PriorityScreen.PriorityClass.basic)
			{
				for (int i = 5; i >= 0; i--)
				{
					global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple = new global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, i, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
					tuple.third.name = "PriorityGroup_" + priorityInfo.name + "_" + i.ToString();
					tuple.third.gameObject.SetActive(true);
					JobsTableScreen.PriorityInfo priorityInfo2 = JobsTableScreen.priorityInfo[i];
					tuple.third.GetReference<LocText>("Title").text = priorityInfo2.name.text.ToUpper();
					tuple.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo2.sprite;
					this.priorityGroups.Add(tuple);
				}
			}
			else
			{
				global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple2 = new global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, 3, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
				tuple2.third.name = "PriorityGroup_" + priorityInfo.name;
				tuple2.third.gameObject.SetActive(true);
				tuple2.third.GetReference<LocText>("Title").text = priorityInfo.name.text.ToUpper();
				tuple2.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo.sprite;
				this.priorityGroups.Add(tuple2);
			}
		}
	}

	// Token: 0x0600629C RID: 25244 RVA: 0x00246A10 File Offset: 0x00244C10
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>() != null && !target.HasTag(GameTags.Dead);
	}

	// Token: 0x0600629D RID: 25245 RVA: 0x00246A30 File Offset: 0x00244C30
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.refreshHandle.ClearScheduler();
	}

	// Token: 0x0600629E RID: 25246 RVA: 0x00246A43 File Offset: 0x00244C43
	public override void SetTarget(GameObject target)
	{
		this.refreshHandle.ClearScheduler();
		if (this.priorityGroups.Count == 0)
		{
			this.OnPrefabInit();
		}
		base.SetTarget(target);
	}

	// Token: 0x0600629F RID: 25247 RVA: 0x00246A6A File Offset: 0x00244C6A
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.PopulateElements(null);
	}

	// Token: 0x060062A0 RID: 25248 RVA: 0x00246A7C File Offset: 0x00244C7C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.refreshHandle.ClearScheduler();
		if (!show)
		{
			if (this.useOffscreenIndicators)
			{
				foreach (GameObject target in this.choreTargets)
				{
					OffscreenIndicator.Instance.DeactivateIndicator(target);
				}
			}
			return;
		}
		if (DetailsScreen.Instance.target == null)
		{
			return;
		}
		this.choreConsumer = DetailsScreen.Instance.target.GetComponent<ChoreConsumer>();
		this.PopulateElements(null);
	}

	// Token: 0x060062A1 RID: 25249 RVA: 0x00246B20 File Offset: 0x00244D20
	private void PopulateElements(object data = null)
	{
		this.refreshHandle.ClearScheduler();
		this.refreshHandle = UIScheduler.Instance.Schedule("RefreshToDoList", 0.1f, new Action<object>(this.PopulateElements), null, null);
		ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
		ChoreConsumer.PreconditionSnapshot lastPreconditionSnapshot = this.choreConsumer.GetLastPreconditionSnapshot();
		if (lastPreconditionSnapshot.doFailedContextsNeedSorting)
		{
			lastPreconditionSnapshot.failedContexts.Sort();
			lastPreconditionSnapshot.doFailedContextsNeedSorting = false;
		}
		pooledList.AddRange(lastPreconditionSnapshot.failedContexts);
		pooledList.AddRange(lastPreconditionSnapshot.succeededContexts);
		Chore.Precondition.Context choreB = default(Chore.Precondition.Context);
		MinionTodoChoreEntry minionTodoChoreEntry = null;
		int num = 0;
		Schedulable component = DetailsScreen.Instance.target.GetComponent<Schedulable>();
		string arg = "";
		Schedule schedule = component.GetSchedule();
		if (schedule != null)
		{
			arg = schedule.GetBlock(Schedule.GetBlockIdx()).name;
		}
		this.currentScheduleBlockLabel.SetText(string.Format(UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CURRENT_SCHEDULE_BLOCK, arg));
		this.choreTargets.Clear();
		bool flag = false;
		this.activeChoreEntries = 0;
		for (int i = pooledList.Count - 1; i >= 0; i--)
		{
			if (pooledList[i].chore != null && !pooledList[i].chore.target.isNull && !(pooledList[i].chore.target.gameObject == null) && pooledList[i].IsPotentialSuccess())
			{
				if (pooledList[i].chore.driver == this.choreConsumer.choreDriver)
				{
					this.currentTask.Apply(pooledList[i]);
					minionTodoChoreEntry = this.currentTask;
					choreB = pooledList[i];
					num = 0;
					flag = true;
				}
				else if (!flag && this.activeChoreEntries != 0 && GameUtil.AreChoresUIMergeable(pooledList[i], choreB))
				{
					num++;
					minionTodoChoreEntry.SetMoreAmount(num);
				}
				else
				{
					HierarchyReferences hierarchyReferences = this.PriorityGroupForPriority(this.choreConsumer, pooledList[i].chore);
					if (hierarchyReferences == null)
					{
						DebugUtil.DevLogError(string.Format("Priority group was null for {0} with priority class {1} and personaly priority {2}", pooledList[i].chore.GetReportName(null), pooledList[i].chore.masterPriority.priority_class, this.choreConsumer.GetPersonalPriority(pooledList[i].chore.choreType)));
					}
					else
					{
						MinionTodoChoreEntry choreEntry = this.GetChoreEntry(hierarchyReferences.GetReference<RectTransform>("EntriesContainer"));
						choreEntry.Apply(pooledList[i]);
						minionTodoChoreEntry = choreEntry;
						choreB = pooledList[i];
						num = 0;
						flag = false;
					}
				}
			}
		}
		pooledList.Recycle();
		for (int j = this.choreEntries.Count - 1; j >= this.activeChoreEntries; j--)
		{
			this.choreEntries[j].gameObject.SetActive(false);
		}
		foreach (global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple in this.priorityGroups)
		{
			RectTransform reference = tuple.third.GetReference<RectTransform>("EntriesContainer");
			tuple.third.gameObject.SetActive(reference.childCount > 0);
		}
	}

	// Token: 0x060062A2 RID: 25250 RVA: 0x00246E7C File Offset: 0x0024507C
	private MinionTodoChoreEntry GetChoreEntry(RectTransform parent)
	{
		MinionTodoChoreEntry minionTodoChoreEntry;
		if (this.activeChoreEntries >= this.choreEntries.Count - 1)
		{
			minionTodoChoreEntry = Util.KInstantiateUI<MinionTodoChoreEntry>(this.taskEntryPrefab.gameObject, parent.gameObject, false);
			this.choreEntries.Add(minionTodoChoreEntry);
		}
		else
		{
			minionTodoChoreEntry = this.choreEntries[this.activeChoreEntries];
			minionTodoChoreEntry.transform.SetParent(parent);
			minionTodoChoreEntry.transform.SetAsLastSibling();
		}
		this.activeChoreEntries++;
		minionTodoChoreEntry.gameObject.SetActive(true);
		return minionTodoChoreEntry;
	}

	// Token: 0x060062A3 RID: 25251 RVA: 0x00246F08 File Offset: 0x00245108
	private HierarchyReferences PriorityGroupForPriority(ChoreConsumer choreConsumer, Chore chore)
	{
		foreach (global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple in this.priorityGroups)
		{
			if (tuple.first == chore.masterPriority.priority_class)
			{
				if (chore.masterPriority.priority_class != PriorityScreen.PriorityClass.basic)
				{
					return tuple.third;
				}
				if (tuple.second == choreConsumer.GetPersonalPriority(chore.choreType))
				{
					return tuple.third;
				}
			}
		}
		return null;
	}

	// Token: 0x060062A4 RID: 25252 RVA: 0x00246FA0 File Offset: 0x002451A0
	private void Button_onPointerEnter()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0400433A RID: 17210
	private bool useOffscreenIndicators;

	// Token: 0x0400433B RID: 17211
	public MinionTodoChoreEntry taskEntryPrefab;

	// Token: 0x0400433C RID: 17212
	public GameObject priorityGroupPrefab;

	// Token: 0x0400433D RID: 17213
	public GameObject taskEntryContainer;

	// Token: 0x0400433E RID: 17214
	public MinionTodoChoreEntry currentTask;

	// Token: 0x0400433F RID: 17215
	public LocText currentScheduleBlockLabel;

	// Token: 0x04004340 RID: 17216
	private List<global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>> priorityGroups = new List<global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>>();

	// Token: 0x04004341 RID: 17217
	private List<MinionTodoChoreEntry> choreEntries = new List<MinionTodoChoreEntry>();

	// Token: 0x04004342 RID: 17218
	private List<GameObject> choreTargets = new List<GameObject>();

	// Token: 0x04004343 RID: 17219
	private SchedulerHandle refreshHandle;

	// Token: 0x04004344 RID: 17220
	private ChoreConsumer choreConsumer;

	// Token: 0x04004345 RID: 17221
	[SerializeField]
	private ColorStyleSetting buttonColorSettingCurrent;

	// Token: 0x04004346 RID: 17222
	[SerializeField]
	private ColorStyleSetting buttonColorSettingStandard;

	// Token: 0x04004347 RID: 17223
	private static List<JobsTableScreen.PriorityInfo> _priorityInfo;

	// Token: 0x04004348 RID: 17224
	private int activeChoreEntries;
}

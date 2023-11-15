using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C09 RID: 3081
public class CommandModuleSideScreen : SideScreenContent
{
	// Token: 0x0600618F RID: 24975 RVA: 0x0024013C File Offset: 0x0023E33C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ScheduleUpdate();
		MultiToggle multiToggle = this.debugVictoryButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SpaceDestination destination = SpacecraftManager.instance.destinations.Find((SpaceDestination match) => match.GetDestinationType() == Db.Get().SpaceDestinationTypes.Wormhole);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			this.target.Launch(destination);
		}));
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x0024019C File Offset: 0x0023E39C
	private bool CheckHydrogenRocket()
	{
		RocketModule rocketModule = this.target.rocketModules.Find((RocketModule match) => match.GetComponent<RocketEngine>());
		return rocketModule != null && rocketModule.GetComponent<RocketEngine>().fuelTag == ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
	}

	// Token: 0x06006191 RID: 24977 RVA: 0x00240203 File Offset: 0x0023E403
	private void ScheduleUpdate()
	{
		this.updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen", 1f, delegate(object o)
		{
			this.RefreshConditions();
			this.ScheduleUpdate();
		}, null, null);
	}

	// Token: 0x06006192 RID: 24978 RVA: 0x0024022D File Offset: 0x0023E42D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchConditionManager>() != null;
	}

	// Token: 0x06006193 RID: 24979 RVA: 0x0024023C File Offset: 0x0023E43C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<LaunchConditionManager>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchConditionManager component");
			return;
		}
		this.ClearConditions();
		this.ConfigureConditions();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x06006194 RID: 24980 RVA: 0x002402B0 File Offset: 0x0023E4B0
	private void ClearConditions()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.conditionTable.Clear();
	}

	// Token: 0x06006195 RID: 24981 RVA: 0x00240314 File Offset: 0x0023E514
	private void ConfigureConditions()
	{
		foreach (ProcessCondition key in this.target.GetLaunchConditionList())
		{
			GameObject value = Util.KInstantiateUI(this.prefabConditionLineItem, this.conditionListContainer, true);
			this.conditionTable.Add(key, value);
		}
		this.RefreshConditions();
	}

	// Token: 0x06006196 RID: 24982 RVA: 0x0024038C File Offset: 0x0023E58C
	public void RefreshConditions()
	{
		bool flag = false;
		List<ProcessCondition> launchConditionList = this.target.GetLaunchConditionList();
		foreach (ProcessCondition processCondition in launchConditionList)
		{
			if (!this.conditionTable.ContainsKey(processCondition))
			{
				flag = true;
				break;
			}
			GameObject gameObject = this.conditionTable[processCondition];
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			if (processCondition.GetParentCondition() != null && processCondition.GetParentCondition().EvaluateCondition() == ProcessCondition.Status.Failure)
			{
				gameObject.SetActive(false);
			}
			else if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			ProcessCondition.Status status = processCondition.EvaluateCondition();
			bool flag2 = status == ProcessCondition.Status.Ready;
			component.GetReference<LocText>("Label").text = processCondition.GetStatusMessage(status);
			component.GetReference<LocText>("Label").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Box").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Check").gameObject.SetActive(flag2);
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(processCondition.GetStatusTooltip(status));
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			if (!launchConditionList.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.ClearConditions();
			this.ConfigureConditions();
		}
		this.destinationButton.gameObject.SetActive(ManagementMenu.StarmapAvailable());
		this.destinationButton.onClick = delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
	}

	// Token: 0x06006197 RID: 24983 RVA: 0x0024058C File Offset: 0x0023E78C
	protected override void OnCleanUp()
	{
		this.updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x0400426C RID: 17004
	private LaunchConditionManager target;

	// Token: 0x0400426D RID: 17005
	public GameObject conditionListContainer;

	// Token: 0x0400426E RID: 17006
	public GameObject prefabConditionLineItem;

	// Token: 0x0400426F RID: 17007
	public MultiToggle destinationButton;

	// Token: 0x04004270 RID: 17008
	public MultiToggle debugVictoryButton;

	// Token: 0x04004271 RID: 17009
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public List<Color> statusColors;

	// Token: 0x04004272 RID: 17010
	private Dictionary<ProcessCondition, GameObject> conditionTable = new Dictionary<ProcessCondition, GameObject>();

	// Token: 0x04004273 RID: 17011
	private SchedulerHandle updateHandle;
}

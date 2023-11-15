using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C52 RID: 3154
public class TelepadSideScreen : SideScreenContent
{
	// Token: 0x060063F1 RID: 25585 RVA: 0x0024EF9C File Offset: 0x0024D19C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.viewImmigrantsBtn.onClick += delegate()
		{
			ImmigrantScreen.InitializeImmigrantScreen(this.targetTelepad);
			Game.Instance.Trigger(288942073, null);
		};
		this.viewColonySummaryBtn.onClick += delegate()
		{
			this.newAchievementsEarned.gameObject.SetActive(false);
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		};
		this.openRolesScreenButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleSkills();
		};
		this.BuildVictoryConditions();
	}

	// Token: 0x060063F2 RID: 25586 RVA: 0x0024F00D File Offset: 0x0024D20D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telepad>() != null;
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x0024F01C File Offset: 0x0024D21C
	public override void SetTarget(GameObject target)
	{
		Telepad component = target.GetComponent<Telepad>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a telepad associated with it.");
			return;
		}
		this.targetTelepad = component;
		if (this.targetTelepad != null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x0024F074 File Offset: 0x0024D274
	private void Update()
	{
		if (this.targetTelepad != null)
		{
			if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver())
			{
				base.gameObject.SetActive(false);
				this.timeLabel.text = UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
				this.SetContentState(true);
			}
			else
			{
				if (this.targetTelepad.GetComponent<Operational>().IsOperational)
				{
					this.timeLabel.text = string.Format(UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION, GameUtil.GetFormattedCycles(this.targetTelepad.GetTimeRemaining(), "F1", false));
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				this.SetContentState(!Immigration.Instance.ImmigrantsAvailable);
			}
			this.UpdateVictoryConditions();
			this.UpdateAchievementsUnlocked();
			this.UpdateSkills();
		}
	}

	// Token: 0x060063F5 RID: 25589 RVA: 0x0024F14C File Offset: 0x0024D34C
	private void SetContentState(bool isLabel)
	{
		if (this.timeLabel.gameObject.activeInHierarchy != isLabel)
		{
			this.timeLabel.gameObject.SetActive(isLabel);
		}
		if (this.viewImmigrantsBtn.gameObject.activeInHierarchy == isLabel)
		{
			this.viewImmigrantsBtn.gameObject.SetActive(!isLabel);
		}
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x0024F1A4 File Offset: 0x0024D3A4
	private void BuildVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled)
			{
				Dictionary<ColonyAchievementRequirement, GameObject> dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
				this.victoryAchievementWidgets.Add(colonyAchievement, dictionary);
				GameObject gameObject = Util.KInstantiateUI(this.conditionContainerTemplate, this.victoryConditionsContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(colonyAchievement.Name);
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					VictoryColonyAchievementRequirement victoryColonyAchievementRequirement = colonyAchievementRequirement as VictoryColonyAchievementRequirement;
					if (victoryColonyAchievementRequirement != null)
					{
						GameObject gameObject2 = Util.KInstantiateUI(this.checkboxLinePrefab, gameObject, true);
						gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(victoryColonyAchievementRequirement.Name());
						gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(victoryColonyAchievementRequirement.Description());
						dictionary.Add(colonyAchievementRequirement, gameObject2);
					}
					else
					{
						global::Debug.LogWarning(string.Format("Colony achievement {0} is not a victory requirement but it is attached to a victory achievement {1}.", colonyAchievementRequirement.GetType().ToString(), colonyAchievement.Name));
					}
				}
				this.entries.Add(colonyAchievement.Id, dictionary);
			}
		}
	}

	// Token: 0x060063F7 RID: 25591 RVA: 0x0024F33C File Offset: 0x0024D53C
	private void UpdateVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled)
			{
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					this.entries[colonyAchievement.Id][colonyAchievementRequirement].GetComponent<HierarchyReferences>().GetReference<Image>("Check").enabled = colonyAchievementRequirement.Success();
				}
			}
		}
		foreach (KeyValuePair<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> keyValuePair in this.victoryAchievementWidgets)
		{
			foreach (KeyValuePair<ColonyAchievementRequirement, GameObject> keyValuePair2 in keyValuePair.Value)
			{
				keyValuePair2.Value.GetComponent<ToolTip>().SetSimpleTooltip(keyValuePair2.Key.GetProgress(keyValuePair2.Key.Success()));
			}
		}
	}

	// Token: 0x060063F8 RID: 25592 RVA: 0x0024F4B4 File Offset: 0x0024D6B4
	private void UpdateAchievementsUnlocked()
	{
		if (SaveGame.Instance.GetComponent<ColonyAchievementTracker>().achievementsToDisplay.Count > 0)
		{
			this.newAchievementsEarned.gameObject.SetActive(true);
		}
	}

	// Token: 0x060063F9 RID: 25593 RVA: 0x0024F4E0 File Offset: 0x0024D6E0
	private void UpdateSkills()
	{
		bool active = false;
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				active = true;
				break;
			}
		}
		this.skillPointsAvailable.gameObject.SetActive(active);
	}

	// Token: 0x04004431 RID: 17457
	[SerializeField]
	private LocText timeLabel;

	// Token: 0x04004432 RID: 17458
	[SerializeField]
	private KButton viewImmigrantsBtn;

	// Token: 0x04004433 RID: 17459
	[SerializeField]
	private Telepad targetTelepad;

	// Token: 0x04004434 RID: 17460
	[SerializeField]
	private KButton viewColonySummaryBtn;

	// Token: 0x04004435 RID: 17461
	[SerializeField]
	private Image newAchievementsEarned;

	// Token: 0x04004436 RID: 17462
	[SerializeField]
	private KButton openRolesScreenButton;

	// Token: 0x04004437 RID: 17463
	[SerializeField]
	private Image skillPointsAvailable;

	// Token: 0x04004438 RID: 17464
	[SerializeField]
	private GameObject victoryConditionsContainer;

	// Token: 0x04004439 RID: 17465
	[SerializeField]
	private GameObject conditionContainerTemplate;

	// Token: 0x0400443A RID: 17466
	[SerializeField]
	private GameObject checkboxLinePrefab;

	// Token: 0x0400443B RID: 17467
	private Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();

	// Token: 0x0400443C RID: 17468
	private Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> victoryAchievementWidgets = new Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>();
}

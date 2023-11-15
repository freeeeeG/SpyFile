using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02000A8A RID: 2698
[AddComponentMenu("KMonoBehaviour/scripts/AchievementWidget")]
public class AchievementWidget : KMonoBehaviour
{
	// Token: 0x0600525F RID: 21087 RVA: 0x001D78E9 File Offset: 0x001D5AE9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle component = base.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.ExpandAchievement();
		}));
	}

	// Token: 0x06005260 RID: 21088 RVA: 0x001D7918 File Offset: 0x001D5B18
	private void Update()
	{
	}

	// Token: 0x06005261 RID: 21089 RVA: 0x001D791A File Offset: 0x001D5B1A
	private void ExpandAchievement()
	{
		if (SaveGame.Instance != null)
		{
			this.progressParent.gameObject.SetActive(!this.progressParent.gameObject.activeSelf);
		}
	}

	// Token: 0x06005262 RID: 21090 RVA: 0x001D794C File Offset: 0x001D5B4C
	public void ActivateNewlyAchievedFlourish(float delay = 1f)
	{
		base.StartCoroutine(this.Flourish(delay));
	}

	// Token: 0x06005263 RID: 21091 RVA: 0x001D795C File Offset: 0x001D5B5C
	private IEnumerator Flourish(float startDelay)
	{
		this.SetNeverAchieved();
		Canvas canvas = base.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		yield return SequenceUtil.WaitForSecondsRealtime(startDelay);
		KScrollRect component = base.transform.parent.parent.GetComponent<KScrollRect>();
		float num = 1.1f;
		float smoothAutoScrollTarget = 1f + base.transform.localPosition.y * num / component.content.rect.height;
		component.SetSmoothAutoScrollTarget(smoothAutoScrollTarget);
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		canvas.overrideSorting = true;
		canvas.sortingOrder = 30;
		GameObject icon = base.GetComponent<HierarchyReferences>().GetReference<Image>("icon").transform.parent.gameObject;
		foreach (KBatchedAnimController kbatchedAnimController in this.sparks)
		{
			if (kbatchedAnimController.transform.parent != icon.transform.parent)
			{
				kbatchedAnimController.GetComponent<KBatchedAnimController>().TintColour = new Color(1f, 0.86f, 0.56f, 1f);
				kbatchedAnimController.transform.SetParent(icon.transform.parent);
				kbatchedAnimController.transform.SetSiblingIndex(icon.transform.GetSiblingIndex());
				kbatchedAnimController.GetComponent<KBatchedAnimCanvasRenderer>().compare = CompareFunction.Always;
			}
		}
		HierarchyReferences component2 = base.GetComponent<HierarchyReferences>();
		component2.GetReference<Image>("iconBG").color = this.color_dark_red;
		component2.GetReference<Image>("iconBorder").color = this.color_gold;
		component2.GetReference<Image>("icon").color = this.color_gold;
		bool colorChanged = false;
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("AchievementUnlocked", false), Vector3.zero, 1f);
		int num2 = Mathf.RoundToInt(MathUtil.Clamp(1f, 7f, startDelay - startDelay % 1f / 1f)) - 1;
		instance.setParameterByName("num_achievements", (float)num2, false);
		KFMOD.EndOneShot(instance);
		for (float i = 0f; i < 1.2f; i += Time.unscaledDeltaTime)
		{
			icon.transform.localScale = Vector3.one * this.flourish_iconScaleCurve.Evaluate(i);
			this.sheenTransform.anchoredPosition = new Vector2(this.flourish_sheenPositionCurve.Evaluate(i), this.sheenTransform.anchoredPosition.y);
			if (i > 1f && !colorChanged)
			{
				colorChanged = true;
				KBatchedAnimController[] array = this.sparks;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Play("spark", KAnim.PlayMode.Once, 1f, 0f);
				}
				this.SetAchievedNow();
			}
			yield return SequenceUtil.WaitForNextFrame;
		}
		icon.transform.localScale = Vector3.one;
		this.CompleteFlourish();
		for (float i = 0f; i < 0.6f; i += Time.unscaledDeltaTime)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		base.transform.localScale = Vector3.one;
		yield break;
	}

	// Token: 0x06005264 RID: 21092 RVA: 0x001D7974 File Offset: 0x001D5B74
	public void CompleteFlourish()
	{
		Canvas canvas = base.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		canvas.overrideSorting = false;
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x001D79A4 File Offset: 0x001D5BA4
	public void SetAchievedNow()
	{
		base.GetComponent<MultiToggle>().ChangeState(1);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_red;
		component.GetReference<Image>("iconBorder").color = this.color_gold;
		component.GetReference<Image>("icon").color = this.color_gold;
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_THIS_COLONY_TOOLTIP);
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x001D7A3C File Offset: 0x001D5C3C
	public void SetAchievedBefore()
	{
		base.GetComponent<MultiToggle>().ChangeState(1);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_red;
		component.GetReference<Image>("iconBorder").color = this.color_gold;
		component.GetReference<Image>("icon").color = this.color_gold;
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_OTHER_COLONY_TOOLTIP);
	}

	// Token: 0x06005267 RID: 21095 RVA: 0x001D7AD4 File Offset: 0x001D5CD4
	public void SetNeverAchieved()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("icon").color = this.color_grey;
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_EVER);
	}

	// Token: 0x06005268 RID: 21096 RVA: 0x001D7B94 File Offset: 0x001D5D94
	public void SetNotAchieved()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("icon").color = this.color_grey;
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_THIS_COLONY);
	}

	// Token: 0x06005269 RID: 21097 RVA: 0x001D7C54 File Offset: 0x001D5E54
	public void SetFailed()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBG").SetAlpha(0.5f);
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("iconBorder").SetAlpha(0.5f);
		component.GetReference<Image>("icon").color = this.color_grey;
		component.GetReference<Image>("icon").SetAlpha(0.5f);
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.25f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.FAILED_THIS_COLONY);
	}

	// Token: 0x0600526A RID: 21098 RVA: 0x001D7D54 File Offset: 0x001D5F54
	private void ConfigureToolTip(ToolTip tooltip, string status)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(status, null);
		if (SaveGame.Instance != null && !this.progressParent.gameObject.activeSelf)
		{
			tooltip.AddMultiStringTooltip(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXPAND_TOOLTIP, null);
		}
		if (this.dlcAchievement)
		{
			tooltip.AddMultiStringTooltip(COLONY_ACHIEVEMENTS.DLC.EXPANSION1, null);
		}
	}

	// Token: 0x0600526B RID: 21099 RVA: 0x001D7DB8 File Offset: 0x001D5FB8
	public void ShowProgress(ColonyAchievementStatus achievement)
	{
		if (this.progressParent == null)
		{
			return;
		}
		this.numRequirementsDisplayed = 0;
		for (int i = 0; i < achievement.Requirements.Count; i++)
		{
			ColonyAchievementRequirement colonyAchievementRequirement = achievement.Requirements[i];
			if (colonyAchievementRequirement is CritterTypesWithTraits)
			{
				this.ShowCritterChecklist(colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is DupesCompleteChoreInExoSuitForCycles)
			{
				this.ShowDupesInExoSuitsRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is DupesVsSolidTransferArmFetch)
			{
				this.ShowArmsOutPeformingDupesRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is ProduceXEngeryWithoutUsingYList)
			{
				this.ShowEngeryWithoutUsing(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is MinimumMorale)
			{
				this.ShowMinimumMoraleRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is SurviveARocketWithMinimumMorale)
			{
				this.ShowRocketMoraleRequirement(achievement.success, colonyAchievementRequirement);
			}
			else
			{
				this.ShowRequirement(achievement.success, colonyAchievementRequirement);
			}
		}
	}

	// Token: 0x0600526C RID: 21100 RVA: 0x001D7E98 File Offset: 0x001D6098
	private HierarchyReferences GetNextRequirementWidget()
	{
		GameObject gameObject;
		if (this.progressParent.childCount <= this.numRequirementsDisplayed)
		{
			gameObject = global::Util.KInstantiateUI(this.requirementPrefab, this.progressParent.gameObject, true);
		}
		else
		{
			gameObject = this.progressParent.GetChild(this.numRequirementsDisplayed).gameObject;
			gameObject.SetActive(true);
		}
		this.numRequirementsDisplayed++;
		return gameObject.GetComponent<HierarchyReferences>();
	}

	// Token: 0x0600526D RID: 21101 RVA: 0x001D7F04 File Offset: 0x001D6104
	private void SetDescription(string str, HierarchyReferences refs)
	{
		refs.GetReference<LocText>("Desc").SetText(str);
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x001D7F17 File Offset: 0x001D6117
	private void SetIcon(Sprite sprite, Color color, HierarchyReferences refs)
	{
		Image reference = refs.GetReference<Image>("Icon");
		reference.sprite = sprite;
		reference.color = color;
		reference.gameObject.SetActive(true);
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x001D7F3D File Offset: 0x001D613D
	private void ShowIcon(bool show, HierarchyReferences refs)
	{
		refs.GetReference<Image>("Icon").gameObject.SetActive(show);
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x001D7F58 File Offset: 0x001D6158
	private void ShowRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		bool flag = req.Success() || succeed;
		bool flag2 = req.Fail();
		if (flag && !flag2)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else if (flag2)
		{
			this.SetIcon(this.statusFailureIcon, Color.red, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		this.SetDescription(req.GetProgress(flag), nextRequirementWidget);
	}

	// Token: 0x06005271 RID: 21105 RVA: 0x001D7FC4 File Offset: 0x001D61C4
	private void ShowCritterChecklist(ColonyAchievementRequirement req)
	{
		CritterTypesWithTraits critterTypesWithTraits = req as CritterTypesWithTraits;
		if (req == null)
		{
			return;
		}
		foreach (KeyValuePair<Tag, bool> keyValuePair in critterTypesWithTraits.critterTypesToCheck)
		{
			HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
			if (keyValuePair.Value)
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			else
			{
				this.ShowIcon(false, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TAME_A_CRITTER, keyValuePair.Key.Name.ProperName()), nextRequirementWidget);
		}
	}

	// Token: 0x06005272 RID: 21106 RVA: 0x001D8078 File Offset: 0x001D6278
	private void ShowArmsOutPeformingDupesRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		DupesVsSolidTransferArmFetch dupesVsSolidTransferArmFetch = req as DupesVsSolidTransferArmFetch;
		if (dupesVsSolidTransferArmFetch == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_PERFORMANCE, succeed ? dupesVsSolidTransferArmFetch.numCycles : dupesVsSolidTransferArmFetch.currentCycleCount, dupesVsSolidTransferArmFetch.numCycles), nextRequirementWidget);
		if (!succeed)
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.GetComponent<ColonyAchievementTracker>().fetchAutomatedChoreDeliveries;
			int num = 0;
			fetchDupeChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num);
			int num2 = 0;
			fetchAutomatedChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num2);
			nextRequirementWidget = this.GetNextRequirementWidget();
			if ((float)num < (float)num2 * dupesVsSolidTransferArmFetch.percentage)
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			else
			{
				this.ShowIcon(false, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_VS_DUPE_FETCHES, "SolidTransferArm", num2, num), nextRequirementWidget);
		}
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x001D818C File Offset: 0x001D638C
	private void ShowDupesInExoSuitsRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		DupesCompleteChoreInExoSuitForCycles dupesCompleteChoreInExoSuitForCycles = req as DupesCompleteChoreInExoSuitForCycles;
		if (dupesCompleteChoreInExoSuitForCycles == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_CYCLES, succeed ? dupesCompleteChoreInExoSuitForCycles.numCycles : dupesCompleteChoreInExoSuitForCycles.currentCycleStreak, dupesCompleteChoreInExoSuitForCycles.numCycles), nextRequirementWidget);
		if (!succeed)
		{
			nextRequirementWidget = this.GetNextRequirementWidget();
			int num = dupesCompleteChoreInExoSuitForCycles.GetNumberOfDupesForCycle(GameClock.Instance.GetCycle());
			if (num >= Components.LiveMinionIdentities.Count)
			{
				num = Components.LiveMinionIdentities.Count;
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_THIS_CYCLE, num, Components.LiveMinionIdentities.Count), nextRequirementWidget);
		}
	}

	// Token: 0x06005274 RID: 21108 RVA: 0x001D8274 File Offset: 0x001D6474
	private void ShowEngeryWithoutUsing(bool succeed, ColonyAchievementRequirement req)
	{
		ProduceXEngeryWithoutUsingYList produceXEngeryWithoutUsingYList = req as ProduceXEngeryWithoutUsingYList;
		if (req == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		float productionAmount = produceXEngeryWithoutUsingYList.GetProductionAmount(succeed);
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.GENERATE_POWER, GameUtil.GetFormattedRoundedJoules(productionAmount), GameUtil.GetFormattedRoundedJoules(produceXEngeryWithoutUsingYList.amountToProduce * 1000f)), nextRequirementWidget);
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		foreach (Tag key in produceXEngeryWithoutUsingYList.disallowedBuildings)
		{
			nextRequirementWidget = this.GetNextRequirementWidget();
			if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
			{
				this.SetIcon(this.statusFailureIcon, Color.red, nextRequirementWidget);
			}
			else
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			BuildingDef buildingDef = Assets.GetBuildingDef(key.Name);
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_BUILDING, buildingDef.Name), nextRequirementWidget);
		}
	}

	// Token: 0x06005275 RID: 21109 RVA: 0x001D839C File Offset: 0x001D659C
	private void ShowMinimumMoraleRequirement(bool success, ColonyAchievementRequirement req)
	{
		MinimumMorale minimumMorale = req as MinimumMorale;
		if (minimumMorale == null)
		{
			return;
		}
		if (success)
		{
			this.ShowRequirement(success, req);
			return;
		}
		foreach (object obj in Components.MinionAssignablesProxy)
		{
			GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
			if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
				if (attributeInstance != null)
				{
					HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
					if (attributeInstance.GetTotalValue() >= (float)minimumMorale.minimumMorale)
					{
						this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
					}
					else
					{
						this.ShowIcon(false, nextRequirementWidget);
					}
					this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MORALE, targetGameObject.GetProperName(), attributeInstance.GetTotalDisplayValue()), nextRequirementWidget);
				}
			}
		}
	}

	// Token: 0x06005276 RID: 21110 RVA: 0x001D84A8 File Offset: 0x001D66A8
	private void ShowRocketMoraleRequirement(bool success, ColonyAchievementRequirement req)
	{
		SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = req as SurviveARocketWithMinimumMorale;
		if (surviveARocketWithMinimumMorale == null)
		{
			return;
		}
		if (success)
		{
			this.ShowRequirement(success, req);
			return;
		}
		foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.GetComponent<ColonyAchievementTracker>().cyclesRocketDupeMoraleAboveRequirement)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
			if (world != null)
			{
				HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
				this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE, new object[]
				{
					surviveARocketWithMinimumMorale.minimumMorale,
					keyValuePair.Value,
					surviveARocketWithMinimumMorale.numberOfCycles,
					world.GetProperName()
				}), nextRequirementWidget);
			}
		}
	}

	// Token: 0x0400370E RID: 14094
	private Color color_dark_red = new Color(0.28235295f, 0.16078432f, 0.14901961f);

	// Token: 0x0400370F RID: 14095
	private Color color_gold = new Color(1f, 0.63529414f, 0.28627452f);

	// Token: 0x04003710 RID: 14096
	private Color color_dark_grey = new Color(0.21568628f, 0.21568628f, 0.21568628f);

	// Token: 0x04003711 RID: 14097
	private Color color_grey = new Color(0.6901961f, 0.6901961f, 0.6901961f);

	// Token: 0x04003712 RID: 14098
	[SerializeField]
	private RectTransform sheenTransform;

	// Token: 0x04003713 RID: 14099
	public AnimationCurve flourish_iconScaleCurve;

	// Token: 0x04003714 RID: 14100
	public AnimationCurve flourish_sheenPositionCurve;

	// Token: 0x04003715 RID: 14101
	public KBatchedAnimController[] sparks;

	// Token: 0x04003716 RID: 14102
	[SerializeField]
	private RectTransform progressParent;

	// Token: 0x04003717 RID: 14103
	[SerializeField]
	private GameObject requirementPrefab;

	// Token: 0x04003718 RID: 14104
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x04003719 RID: 14105
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x0400371A RID: 14106
	private int numRequirementsDisplayed;

	// Token: 0x0400371B RID: 14107
	public bool dlcAchievement;
}

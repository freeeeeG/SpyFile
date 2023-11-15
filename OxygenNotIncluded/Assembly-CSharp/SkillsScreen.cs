using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C65 RID: 3173
public class SkillsScreen : KModalScreen
{
	// Token: 0x060064F1 RID: 25841 RVA: 0x00256E9F File Offset: 0x0025509F
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x060064F2 RID: 25842 RVA: 0x00256EB4 File Offset: 0x002550B4
	// (set) Token: 0x060064F3 RID: 25843 RVA: 0x00256ED3 File Offset: 0x002550D3
	public IAssignableIdentity CurrentlySelectedMinion
	{
		get
		{
			if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
			{
				return null;
			}
			return this.currentlySelectedMinion;
		}
		set
		{
			this.currentlySelectedMinion = value;
			if (base.IsActive())
			{
				this.RefreshSelectedMinion();
				this.RefreshSkillWidgets();
			}
		}
	}

	// Token: 0x060064F4 RID: 25844 RVA: 0x00256EF0 File Offset: 0x002550F0
	protected override void OnSpawn()
	{
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.WorldRemoved));
	}

	// Token: 0x060064F5 RID: 25845 RVA: 0x00256F10 File Offset: 0x00255110
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = true;
		base.OnActivate();
		this.BuildMinions();
		this.RefreshAll();
		this.SortRows((this.active_sort_method == null) ? this.compareByMinion : this.active_sort_method);
		Components.LiveMinionIdentities.OnAdd += this.OnAddMinionIdentity;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveMinionIdentity;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		MultiToggle multiToggle = this.dupeSortingToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SortRows(this.compareByMinion);
		}));
		MultiToggle multiToggle2 = this.moraleSortingToggle;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SortRows(this.compareByMorale);
		}));
		MultiToggle multiToggle3 = this.experienceSortingToggle;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			this.SortRows(this.compareByExperience);
		}));
	}

	// Token: 0x060064F6 RID: 25846 RVA: 0x00257020 File Offset: 0x00255220
	protected override void OnShow(bool show)
	{
		if (show)
		{
			if (this.CurrentlySelectedMinion == null && Components.LiveMinionIdentities.Count > 0)
			{
				this.CurrentlySelectedMinion = Components.LiveMinionIdentities.Items[0];
			}
			this.BuildMinions();
			this.RefreshAll();
			this.SortRows((this.active_sort_method == null) ? this.compareByMinion : this.active_sort_method);
		}
		base.OnShow(show);
	}

	// Token: 0x060064F7 RID: 25847 RVA: 0x0025708A File Offset: 0x0025528A
	public void RefreshAll()
	{
		this.dirty = false;
		this.RefreshSkillWidgets();
		this.RefreshSelectedMinion();
		this.linesPending = true;
	}

	// Token: 0x060064F8 RID: 25848 RVA: 0x002570A6 File Offset: 0x002552A6
	private void RefreshSelectedMinion()
	{
		this.minionAnimWidget.SetPortraitAnimator(this.currentlySelectedMinion);
		this.RefreshProgressBars();
		this.RefreshHat();
	}

	// Token: 0x060064F9 RID: 25849 RVA: 0x002570C5 File Offset: 0x002552C5
	public void GetMinionIdentity(IAssignableIdentity assignableIdentity, out MinionIdentity minionIdentity, out StoredMinionIdentity storedMinionIdentity)
	{
		if (assignableIdentity is MinionAssignablesProxy)
		{
			minionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<MinionIdentity>();
			storedMinionIdentity = ((MinionAssignablesProxy)assignableIdentity).GetTargetGameObject().GetComponent<StoredMinionIdentity>();
			return;
		}
		minionIdentity = (assignableIdentity as MinionIdentity);
		storedMinionIdentity = (assignableIdentity as StoredMinionIdentity);
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x00257104 File Offset: 0x00255304
	private void RefreshProgressBars()
	{
		if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
		{
			return;
		}
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		HierarchyReferences component = this.expectationsTooltip.GetComponent<HierarchyReferences>();
		component.GetReference("Labels").gameObject.SetActive(minionIdentity != null);
		component.GetReference("MoraleBar").gameObject.SetActive(minionIdentity != null);
		component.GetReference("ExpectationBar").gameObject.SetActive(minionIdentity != null);
		component.GetReference("StoredMinion").gameObject.SetActive(minionIdentity == null);
		this.experienceProgressFill.gameObject.SetActive(minionIdentity != null);
		if (minionIdentity == null)
		{
			this.expectationsTooltip.SetSimpleTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), this.currentlySelectedMinion.GetProperName()));
			this.experienceBarTooltip.SetSimpleTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), this.currentlySelectedMinion.GetProperName()));
			this.EXPCount.text = "";
			this.duplicantLevelIndicator.text = UI.TABLESCREENS.NA;
			return;
		}
		MinionResume component2 = minionIdentity.GetComponent<MinionResume>();
		float num = MinionResume.CalculatePreviousExperienceBar(component2.TotalSkillPointsGained);
		float num2 = MinionResume.CalculateNextExperienceBar(component2.TotalSkillPointsGained);
		float fillAmount = (component2.TotalExperienceGained - num) / (num2 - num);
		this.EXPCount.text = Mathf.RoundToInt(component2.TotalExperienceGained - num).ToString() + " / " + Mathf.RoundToInt(num2 - num).ToString();
		this.duplicantLevelIndicator.text = component2.AvailableSkillpoints.ToString();
		this.experienceProgressFill.fillAmount = fillAmount;
		this.experienceBarTooltip.SetSimpleTooltip(string.Format(UI.SKILLS_SCREEN.EXPERIENCE_TOOLTIP, Mathf.RoundToInt(num2 - num) - Mathf.RoundToInt(component2.TotalExperienceGained - num)));
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component2);
		AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component2);
		float num3 = 0f;
		float num4 = 0f;
		if (!string.IsNullOrEmpty(this.hoveredSkillID) && !component2.HasMasteredSkill(this.hoveredSkillID))
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			list.Add(this.hoveredSkillID);
			while (list.Count > 0)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (!component2.HasMasteredSkill(list[i]))
					{
						num3 += (float)(Db.Get().Skills.Get(list[i]).tier + 1);
						if (component2.AptitudeBySkillGroup.ContainsKey(Db.Get().Skills.Get(list[i]).skillGroup) && component2.AptitudeBySkillGroup[Db.Get().Skills.Get(list[i]).skillGroup] > 0f)
						{
							num4 += 1f;
						}
						foreach (string item in Db.Get().Skills.Get(list[i]).priorSkills)
						{
							list2.Add(item);
						}
					}
				}
				list.Clear();
				list.AddRange(list2);
				list2.Clear();
			}
		}
		float num5 = attributeInstance.GetTotalValue() + num4 / (attributeInstance2.GetTotalValue() + num3);
		float f = Mathf.Max(attributeInstance.GetTotalValue() + num4, attributeInstance2.GetTotalValue() + num3);
		while (this.moraleNotches.Count < Mathf.RoundToInt(f))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.moraleNotch, this.moraleNotch.transform.parent);
			gameObject.SetActive(true);
			this.moraleNotches.Add(gameObject);
		}
		while (this.moraleNotches.Count > Mathf.RoundToInt(f))
		{
			GameObject gameObject2 = this.moraleNotches[this.moraleNotches.Count - 1];
			this.moraleNotches.Remove(gameObject2);
			UnityEngine.Object.Destroy(gameObject2);
		}
		for (int j = 0; j < this.moraleNotches.Count; j++)
		{
			if ((float)j < attributeInstance.GetTotalValue() + num4)
			{
				this.moraleNotches[j].GetComponentsInChildren<Image>()[1].color = this.moraleNotchColor;
			}
			else
			{
				this.moraleNotches[j].GetComponentsInChildren<Image>()[1].color = Color.clear;
			}
		}
		this.moraleProgressLabel.text = UI.SKILLS_SCREEN.MORALE + ": " + attributeInstance.GetTotalValue().ToString();
		if (num4 > 0f)
		{
			LocText locText = this.moraleProgressLabel;
			locText.text = locText.text + " + " + GameUtil.ApplyBoldString(GameUtil.ColourizeString(this.moraleNotchColor, num4.ToString()));
		}
		while (this.expectationNotches.Count < Mathf.RoundToInt(f))
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.expectationNotch, this.expectationNotch.transform.parent);
			gameObject3.SetActive(true);
			this.expectationNotches.Add(gameObject3);
		}
		while (this.expectationNotches.Count > Mathf.RoundToInt(f))
		{
			GameObject gameObject4 = this.expectationNotches[this.expectationNotches.Count - 1];
			this.expectationNotches.Remove(gameObject4);
			UnityEngine.Object.Destroy(gameObject4);
		}
		for (int k = 0; k < this.expectationNotches.Count; k++)
		{
			if ((float)k < attributeInstance2.GetTotalValue() + num3)
			{
				if ((float)k < attributeInstance2.GetTotalValue())
				{
					this.expectationNotches[k].GetComponentsInChildren<Image>()[1].color = this.expectationNotchColor;
				}
				else
				{
					this.expectationNotches[k].GetComponentsInChildren<Image>()[1].color = this.expectationNotchProspectColor;
				}
			}
			else
			{
				this.expectationNotches[k].GetComponentsInChildren<Image>()[1].color = Color.clear;
			}
		}
		this.expectationsProgressLabel.text = UI.SKILLS_SCREEN.MORALE_EXPECTATION + ": " + attributeInstance2.GetTotalValue().ToString();
		if (num3 > 0f)
		{
			LocText locText2 = this.expectationsProgressLabel;
			locText2.text = locText2.text + " + " + GameUtil.ApplyBoldString(GameUtil.ColourizeString(this.expectationNotchColor, num3.ToString()));
		}
		if (num5 < 1f)
		{
			this.expectationWarning.SetActive(true);
			this.moraleWarning.SetActive(false);
		}
		else
		{
			this.expectationWarning.SetActive(false);
			this.moraleWarning.SetActive(true);
		}
		string text = "";
		Dictionary<string, float> dictionary = new Dictionary<string, float>();
		text = string.Concat(new string[]
		{
			text,
			GameUtil.ApplyBoldString(UI.SKILLS_SCREEN.MORALE),
			": ",
			attributeInstance.GetTotalValue().ToString(),
			"\n"
		});
		for (int l = 0; l < attributeInstance.Modifiers.Count; l++)
		{
			dictionary.Add(attributeInstance.Modifiers[l].GetDescription(), attributeInstance.Modifiers[l].Value);
		}
		List<KeyValuePair<string, float>> list3 = dictionary.ToList<KeyValuePair<string, float>>();
		list3.Sort((KeyValuePair<string, float> pair1, KeyValuePair<string, float> pair2) => pair2.Value.CompareTo(pair1.Value));
		foreach (KeyValuePair<string, float> keyValuePair in list3)
		{
			text = string.Concat(new string[]
			{
				text,
				"    • ",
				keyValuePair.Key,
				": ",
				(keyValuePair.Value > 0f) ? UIConstants.ColorPrefixGreen : UIConstants.ColorPrefixRed,
				keyValuePair.Value.ToString(),
				UIConstants.ColorSuffix,
				"\n"
			});
		}
		text += "\n";
		text = string.Concat(new string[]
		{
			text,
			GameUtil.ApplyBoldString(UI.SKILLS_SCREEN.MORALE_EXPECTATION),
			": ",
			attributeInstance2.GetTotalValue().ToString(),
			"\n"
		});
		for (int m = 0; m < attributeInstance2.Modifiers.Count; m++)
		{
			text = string.Concat(new string[]
			{
				text,
				"    • ",
				attributeInstance2.Modifiers[m].GetDescription(),
				": ",
				(attributeInstance2.Modifiers[m].Value > 0f) ? UIConstants.ColorPrefixRed : UIConstants.ColorPrefixGreen,
				attributeInstance2.Modifiers[m].GetFormattedString(),
				UIConstants.ColorSuffix,
				"\n"
			});
		}
		this.expectationsTooltip.SetSimpleTooltip(text);
	}

	// Token: 0x060064FB RID: 25851 RVA: 0x00257ABC File Offset: 0x00255CBC
	private void RefreshHat()
	{
		if (this.currentlySelectedMinion == null || this.currentlySelectedMinion.IsNull())
		{
			return;
		}
		List<IListableOption> list = new List<IListableOption>();
		string text = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			text = (string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat);
			foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					list.Add(new SkillListable(keyValuePair.Key));
				}
			}
			this.hatDropDown.Initialize(list, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, this.currentlySelectedMinion);
		}
		else
		{
			text = (string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat);
		}
		this.hatDropDown.openButton.enabled = (minionIdentity != null);
		this.selectedHat.transform.Find("Arrow").gameObject.SetActive(minionIdentity != null);
		this.selectedHat.sprite = Assets.GetSprite(string.IsNullOrEmpty(text) ? "hat_role_none" : text);
	}

	// Token: 0x060064FC RID: 25852 RVA: 0x00257C44 File Offset: 0x00255E44
	private void OnHatDropEntryClick(IListableOption skill, object data)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.GetMinionIdentity(this.currentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return;
		}
		MinionResume component = minionIdentity.GetComponent<MinionResume>();
		string text = "hat_role_none";
		if (skill != null)
		{
			this.selectedHat.sprite = Assets.GetSprite((skill as SkillListable).skillHat);
			if (component != null)
			{
				text = (skill as SkillListable).skillHat;
				component.SetHats(component.CurrentHat, text);
				if (component.OwnsHat(text))
				{
					new PutOnHatChore(component, Db.Get().ChoreTypes.SwitchHat);
				}
			}
		}
		else
		{
			this.selectedHat.sprite = Assets.GetSprite(text);
			if (component != null)
			{
				component.SetHats(component.CurrentHat, null);
				component.ApplyTargetHat();
			}
		}
		IAssignableIdentity assignableIdentity = minionIdentity.assignableProxy.Get();
		foreach (SkillMinionWidget skillMinionWidget in this.sortableRows)
		{
			if (skillMinionWidget.assignableIdentity == assignableIdentity)
			{
				skillMinionWidget.RefreshHat(component.TargetHat);
			}
		}
	}

	// Token: 0x060064FD RID: 25853 RVA: 0x00257D78 File Offset: 0x00255F78
	private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillListable skillListable = entry.entryData as SkillListable;
			entry.image.sprite = Assets.GetSprite(skillListable.skillHat);
		}
	}

	// Token: 0x060064FE RID: 25854 RVA: 0x00257DB4 File Offset: 0x00255FB4
	private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x060064FF RID: 25855 RVA: 0x00257DB8 File Offset: 0x00255FB8
	private void Update()
	{
		if (this.dirty)
		{
			this.RefreshAll();
		}
		if (this.linesPending)
		{
			foreach (GameObject gameObject in this.skillWidgets.Values)
			{
				gameObject.GetComponent<SkillWidget>().RefreshLines();
			}
			this.linesPending = false;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.scrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x06006500 RID: 25856 RVA: 0x00257E58 File Offset: 0x00256058
	private void RefreshSkillWidgets()
	{
		int num = 1;
		foreach (SkillGroup skillGroup in Db.Get().SkillGroups.resources)
		{
			List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(skillGroup.Id);
			if (skillsBySkillGroup.Count > 0)
			{
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				for (int i = 0; i < skillsBySkillGroup.Count; i++)
				{
					Skill skill = skillsBySkillGroup[i];
					if (!skill.deprecated)
					{
						if (!this.skillWidgets.ContainsKey(skill.Id))
						{
							while (skill.tier >= this.skillColumns.Count)
							{
								GameObject gameObject = Util.KInstantiateUI(this.Prefab_skillColumn, this.Prefab_tableLayout, true);
								this.skillColumns.Add(gameObject);
								HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
								if (this.skillColumns.Count % 2 == 0)
								{
									component.GetReference("BG").gameObject.SetActive(false);
								}
							}
							int num2 = 0;
							dictionary.TryGetValue(skill.tier, out num2);
							dictionary[skill.tier] = num2 + 1;
							GameObject value = Util.KInstantiateUI(this.Prefab_skillWidget, this.skillColumns[skill.tier], true);
							this.skillWidgets.Add(skill.Id, value);
						}
						this.skillWidgets[skill.Id].GetComponent<SkillWidget>().Refresh(skill.Id);
					}
				}
				if (!this.skillGroupRow.ContainsKey(skillGroup.Id))
				{
					int num3 = 1;
					foreach (KeyValuePair<int, int> keyValuePair in dictionary)
					{
						num3 = Mathf.Max(num3, keyValuePair.Value);
					}
					this.skillGroupRow.Add(skillGroup.Id, num);
					num += num3;
				}
			}
		}
		foreach (SkillMinionWidget skillMinionWidget in this.sortableRows)
		{
			skillMinionWidget.Refresh();
		}
		this.RefreshWidgetPositions();
	}

	// Token: 0x06006501 RID: 25857 RVA: 0x002580E0 File Offset: 0x002562E0
	public void HoverSkill(string skillID)
	{
		this.hoveredSkillID = skillID;
		if (this.delayRefreshRoutine != null)
		{
			base.StopCoroutine(this.delayRefreshRoutine);
			this.delayRefreshRoutine = null;
		}
		if (string.IsNullOrEmpty(this.hoveredSkillID))
		{
			this.delayRefreshRoutine = base.StartCoroutine(this.DelayRefreshProgressBars());
			return;
		}
		this.RefreshProgressBars();
	}

	// Token: 0x06006502 RID: 25858 RVA: 0x00258135 File Offset: 0x00256335
	private IEnumerator DelayRefreshProgressBars()
	{
		yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
		this.RefreshProgressBars();
		yield break;
	}

	// Token: 0x06006503 RID: 25859 RVA: 0x00258144 File Offset: 0x00256344
	public void RefreshWidgetPositions()
	{
		float num = 0f;
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.skillWidgets)
		{
			float rowPosition = this.GetRowPosition(keyValuePair.Key);
			num = Mathf.Max(rowPosition, num);
			keyValuePair.Value.rectTransform().anchoredPosition = Vector2.down * rowPosition;
		}
		num = Mathf.Max(num, (float)this.layoutRowHeight);
		float num2 = (float)this.layoutRowHeight;
		foreach (GameObject gameObject in this.skillColumns)
		{
			gameObject.GetComponent<LayoutElement>().minHeight = num + num2;
		}
		this.linesPending = true;
	}

	// Token: 0x06006504 RID: 25860 RVA: 0x00258230 File Offset: 0x00256430
	public float GetRowPosition(string skillID)
	{
		Skill skill = Db.Get().Skills.Get(skillID);
		int num = this.skillGroupRow[skill.skillGroup];
		List<Skill> skillsBySkillGroup = this.GetSkillsBySkillGroup(skill.skillGroup);
		int num2 = 0;
		foreach (Skill skill2 in skillsBySkillGroup)
		{
			if (skill2 == skill)
			{
				break;
			}
			if (skill2.tier == skill.tier)
			{
				num2++;
			}
		}
		return (float)(this.layoutRowHeight * (num2 + num - 1));
	}

	// Token: 0x06006505 RID: 25861 RVA: 0x002582D0 File Offset: 0x002564D0
	private void OnAddMinionIdentity(MinionIdentity add)
	{
		this.BuildMinions();
		this.RefreshAll();
	}

	// Token: 0x06006506 RID: 25862 RVA: 0x002582DE File Offset: 0x002564DE
	private void OnRemoveMinionIdentity(MinionIdentity remove)
	{
		if (this.CurrentlySelectedMinion == remove)
		{
			this.CurrentlySelectedMinion = null;
		}
		this.BuildMinions();
		this.RefreshAll();
	}

	// Token: 0x06006507 RID: 25863 RVA: 0x002582FC File Offset: 0x002564FC
	private void BuildMinions()
	{
		for (int i = this.sortableRows.Count - 1; i >= 0; i--)
		{
			this.sortableRows[i].DeleteObject();
		}
		this.sortableRows.Clear();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			GameObject gameObject = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
			gameObject.GetComponent<SkillMinionWidget>().SetMinon(minionIdentity.assignableProxy.Get());
			this.sortableRows.Add(gameObject.GetComponent<SkillMinionWidget>());
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity storedMinionIdentity = info.serializedMinion.Get<StoredMinionIdentity>();
					GameObject gameObject2 = Util.KInstantiateUI(this.Prefab_minion, this.Prefab_minionLayout, true);
					gameObject2.GetComponent<SkillMinionWidget>().SetMinon(storedMinionIdentity.assignableProxy.Get());
					this.sortableRows.Add(gameObject2.GetComponent<SkillMinionWidget>());
				}
			}
		}
		foreach (int num in ClusterManager.Instance.GetWorldIDsSorted())
		{
			if (ClusterManager.Instance.GetWorld(num).IsDiscovered)
			{
				this.AddWorldDivider(num);
			}
		}
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.worldDividers)
		{
			keyValuePair.Value.SetActive(ClusterManager.Instance.GetWorld(keyValuePair.Key).IsDiscovered && DlcManager.FeatureClusterSpaceEnabled());
			Component reference = keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference("NobodyRow");
			reference.gameObject.SetActive(true);
			using (IEnumerator enumerator6 = Components.MinionAssignablesProxy.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					if (((MinionAssignablesProxy)enumerator6.Current).GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id == keyValuePair.Key)
					{
						reference.gameObject.SetActive(false);
						break;
					}
				}
			}
		}
		if (this.CurrentlySelectedMinion == null && Components.LiveMinionIdentities.Count > 0)
		{
			this.CurrentlySelectedMinion = Components.LiveMinionIdentities.Items[0];
		}
	}

	// Token: 0x06006508 RID: 25864 RVA: 0x00258620 File Offset: 0x00256820
	protected void AddWorldDivider(int worldId)
	{
		if (!this.worldDividers.ContainsKey(worldId))
		{
			GameObject gameObject = Util.KInstantiateUI(this.Prefab_worldDivider, this.Prefab_minionLayout, true);
			gameObject.GetComponentInChildren<Image>().color = ClusterManager.worldColors[worldId % ClusterManager.worldColors.Length];
			ClusterGridEntity component = ClusterManager.Instance.GetWorld(worldId).GetComponent<ClusterGridEntity>();
			gameObject.GetComponentInChildren<LocText>().SetText(component.Name);
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = component.GetUISprite();
			this.worldDividers.Add(worldId, gameObject);
		}
	}

	// Token: 0x06006509 RID: 25865 RVA: 0x002586B8 File Offset: 0x002568B8
	private void WorldRemoved(object worldId)
	{
		int key = (int)worldId;
		GameObject obj;
		if (this.worldDividers.TryGetValue(key, out obj))
		{
			UnityEngine.Object.Destroy(obj);
			this.worldDividers.Remove(key);
		}
	}

	// Token: 0x0600650A RID: 25866 RVA: 0x002586EF File Offset: 0x002568EF
	public Vector2 GetSkillWidgetLineTargetPosition(string skillID)
	{
		return this.skillWidgets[skillID].GetComponent<SkillWidget>().lines_right.GetPosition();
	}

	// Token: 0x0600650B RID: 25867 RVA: 0x00258711 File Offset: 0x00256911
	public SkillWidget GetSkillWidget(string skill)
	{
		return this.skillWidgets[skill].GetComponent<SkillWidget>();
	}

	// Token: 0x0600650C RID: 25868 RVA: 0x00258724 File Offset: 0x00256924
	public List<Skill> GetSkillsBySkillGroup(string skillGrp)
	{
		List<Skill> list = new List<Skill>();
		foreach (Skill skill in Db.Get().Skills.resources)
		{
			if (skill.skillGroup == skillGrp && !skill.deprecated)
			{
				list.Add(skill);
			}
		}
		return list;
	}

	// Token: 0x0600650D RID: 25869 RVA: 0x002587A0 File Offset: 0x002569A0
	private void SelectSortToggle(MultiToggle toggle)
	{
		this.dupeSortingToggle.ChangeState(0);
		this.experienceSortingToggle.ChangeState(0);
		this.moraleSortingToggle.ChangeState(0);
		if (toggle != null)
		{
			if (this.activeSortToggle == toggle)
			{
				this.sortReversed = !this.sortReversed;
			}
			this.activeSortToggle = toggle;
		}
		this.activeSortToggle.ChangeState(this.sortReversed ? 2 : 1);
	}

	// Token: 0x0600650E RID: 25870 RVA: 0x00258818 File Offset: 0x00256A18
	private void SortRows(Comparison<IAssignableIdentity> comparison)
	{
		this.active_sort_method = comparison;
		Dictionary<IAssignableIdentity, SkillMinionWidget> dictionary = new Dictionary<IAssignableIdentity, SkillMinionWidget>();
		foreach (SkillMinionWidget skillMinionWidget in this.sortableRows)
		{
			dictionary.Add(skillMinionWidget.assignableIdentity, skillMinionWidget);
		}
		Dictionary<int, List<IAssignableIdentity>> minionsByWorld = ClusterManager.Instance.MinionsByWorld;
		this.sortableRows.Clear();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<int, List<IAssignableIdentity>> keyValuePair in minionsByWorld)
		{
			dictionary2.Add(keyValuePair.Key, num);
			num++;
			List<IAssignableIdentity> list = new List<IAssignableIdentity>();
			foreach (IAssignableIdentity item in keyValuePair.Value)
			{
				list.Add(item);
			}
			if (comparison != null)
			{
				list.Sort(comparison);
				if (this.sortReversed)
				{
					list.Reverse();
				}
			}
			num += list.Count;
			num2 += list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				IAssignableIdentity key = list[i];
				SkillMinionWidget item2 = dictionary[key];
				this.sortableRows.Add(item2);
			}
		}
		for (int j = 0; j < this.sortableRows.Count; j++)
		{
			this.sortableRows[j].gameObject.transform.SetSiblingIndex(j);
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in dictionary2)
		{
			this.worldDividers[keyValuePair2.Key].transform.SetSiblingIndex(keyValuePair2.Value);
		}
	}

	// Token: 0x0400451B RID: 17691
	[SerializeField]
	private KButton CloseButton;

	// Token: 0x0400451C RID: 17692
	[Header("Prefabs")]
	[SerializeField]
	private GameObject Prefab_skillWidget;

	// Token: 0x0400451D RID: 17693
	[SerializeField]
	private GameObject Prefab_skillColumn;

	// Token: 0x0400451E RID: 17694
	[SerializeField]
	private GameObject Prefab_minion;

	// Token: 0x0400451F RID: 17695
	[SerializeField]
	private GameObject Prefab_minionLayout;

	// Token: 0x04004520 RID: 17696
	[SerializeField]
	private GameObject Prefab_tableLayout;

	// Token: 0x04004521 RID: 17697
	[SerializeField]
	private GameObject Prefab_worldDivider;

	// Token: 0x04004522 RID: 17698
	[Header("Sort Toggles")]
	[SerializeField]
	private MultiToggle dupeSortingToggle;

	// Token: 0x04004523 RID: 17699
	[SerializeField]
	private MultiToggle experienceSortingToggle;

	// Token: 0x04004524 RID: 17700
	[SerializeField]
	private MultiToggle moraleSortingToggle;

	// Token: 0x04004525 RID: 17701
	private MultiToggle activeSortToggle;

	// Token: 0x04004526 RID: 17702
	private bool sortReversed;

	// Token: 0x04004527 RID: 17703
	private Comparison<IAssignableIdentity> active_sort_method;

	// Token: 0x04004528 RID: 17704
	[Header("Duplicant Animation")]
	[SerializeField]
	private FullBodyUIMinionWidget minionAnimWidget;

	// Token: 0x04004529 RID: 17705
	[Header("Progress Bars")]
	[SerializeField]
	private ToolTip expectationsTooltip;

	// Token: 0x0400452A RID: 17706
	[SerializeField]
	private LocText moraleProgressLabel;

	// Token: 0x0400452B RID: 17707
	[SerializeField]
	private GameObject moraleWarning;

	// Token: 0x0400452C RID: 17708
	[SerializeField]
	private GameObject moraleNotch;

	// Token: 0x0400452D RID: 17709
	[SerializeField]
	private Color moraleNotchColor;

	// Token: 0x0400452E RID: 17710
	private List<GameObject> moraleNotches = new List<GameObject>();

	// Token: 0x0400452F RID: 17711
	[SerializeField]
	private LocText expectationsProgressLabel;

	// Token: 0x04004530 RID: 17712
	[SerializeField]
	private GameObject expectationWarning;

	// Token: 0x04004531 RID: 17713
	[SerializeField]
	private GameObject expectationNotch;

	// Token: 0x04004532 RID: 17714
	[SerializeField]
	private Color expectationNotchColor;

	// Token: 0x04004533 RID: 17715
	[SerializeField]
	private Color expectationNotchProspectColor;

	// Token: 0x04004534 RID: 17716
	private List<GameObject> expectationNotches = new List<GameObject>();

	// Token: 0x04004535 RID: 17717
	[SerializeField]
	private ToolTip experienceBarTooltip;

	// Token: 0x04004536 RID: 17718
	[SerializeField]
	private Image experienceProgressFill;

	// Token: 0x04004537 RID: 17719
	[SerializeField]
	private LocText EXPCount;

	// Token: 0x04004538 RID: 17720
	[SerializeField]
	private LocText duplicantLevelIndicator;

	// Token: 0x04004539 RID: 17721
	[SerializeField]
	private KScrollRect scrollRect;

	// Token: 0x0400453A RID: 17722
	[SerializeField]
	private float scrollSpeed = 7f;

	// Token: 0x0400453B RID: 17723
	[SerializeField]
	private DropDown hatDropDown;

	// Token: 0x0400453C RID: 17724
	[SerializeField]
	public Image selectedHat;

	// Token: 0x0400453D RID: 17725
	private IAssignableIdentity currentlySelectedMinion;

	// Token: 0x0400453E RID: 17726
	private List<GameObject> rows = new List<GameObject>();

	// Token: 0x0400453F RID: 17727
	private List<SkillMinionWidget> sortableRows = new List<SkillMinionWidget>();

	// Token: 0x04004540 RID: 17728
	private Dictionary<int, GameObject> worldDividers = new Dictionary<int, GameObject>();

	// Token: 0x04004541 RID: 17729
	private string hoveredSkillID = "";

	// Token: 0x04004542 RID: 17730
	private Dictionary<string, GameObject> skillWidgets = new Dictionary<string, GameObject>();

	// Token: 0x04004543 RID: 17731
	private Dictionary<string, int> skillGroupRow = new Dictionary<string, int>();

	// Token: 0x04004544 RID: 17732
	private List<GameObject> skillColumns = new List<GameObject>();

	// Token: 0x04004545 RID: 17733
	private bool dirty;

	// Token: 0x04004546 RID: 17734
	private bool linesPending;

	// Token: 0x04004547 RID: 17735
	private int layoutRowHeight = 80;

	// Token: 0x04004548 RID: 17736
	private Coroutine delayRefreshRoutine;

	// Token: 0x04004549 RID: 17737
	protected Comparison<IAssignableIdentity> compareByExperience = delegate(IAssignableIdentity a, IAssignableIdentity b)
	{
		GameObject targetGameObject = ((MinionAssignablesProxy)a).GetTargetGameObject();
		GameObject targetGameObject2 = ((MinionAssignablesProxy)b).GetTargetGameObject();
		if (targetGameObject == null && targetGameObject2 == null)
		{
			return 0;
		}
		if (targetGameObject == null)
		{
			return -1;
		}
		if (targetGameObject2 == null)
		{
			return 1;
		}
		MinionResume component = targetGameObject.GetComponent<MinionResume>();
		MinionResume component2 = targetGameObject2.GetComponent<MinionResume>();
		if (component == null && component2 == null)
		{
			return 0;
		}
		if (component == null)
		{
			return -1;
		}
		if (component2 == null)
		{
			return 1;
		}
		float num = (float)component.AvailableSkillpoints;
		float value = (float)component2.AvailableSkillpoints;
		return num.CompareTo(value);
	};

	// Token: 0x0400454A RID: 17738
	protected Comparison<IAssignableIdentity> compareByMinion = (IAssignableIdentity a, IAssignableIdentity b) => a.GetProperName().CompareTo(b.GetProperName());

	// Token: 0x0400454B RID: 17739
	protected Comparison<IAssignableIdentity> compareByMorale = delegate(IAssignableIdentity a, IAssignableIdentity b)
	{
		GameObject targetGameObject = ((MinionAssignablesProxy)a).GetTargetGameObject();
		GameObject targetGameObject2 = ((MinionAssignablesProxy)b).GetTargetGameObject();
		if (targetGameObject == null && targetGameObject2 == null)
		{
			return 0;
		}
		if (targetGameObject == null)
		{
			return -1;
		}
		if (targetGameObject2 == null)
		{
			return 1;
		}
		MinionResume component = targetGameObject.GetComponent<MinionResume>();
		MinionResume component2 = targetGameObject2.GetComponent<MinionResume>();
		if (component == null && component2 == null)
		{
			return 0;
		}
		if (component == null)
		{
			return -1;
		}
		if (component2 == null)
		{
			return 1;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component);
		Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component);
		AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLife.Lookup(component2);
		Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component2);
		float totalValue = attributeInstance.GetTotalValue();
		float totalValue2 = attributeInstance2.GetTotalValue();
		return totalValue.CompareTo(totalValue2);
	};
}

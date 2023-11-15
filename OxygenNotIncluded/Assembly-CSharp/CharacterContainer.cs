using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000A57 RID: 2647
public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x06004FE3 RID: 20451 RVA: 0x001C457A File Offset: 0x001C277A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06004FE4 RID: 20452 RVA: 0x001C4582 File Offset: 0x001C2782
	public MinionStartingStats Stats
	{
		get
		{
			return this.stats;
		}
	}

	// Token: 0x06004FE5 RID: 20453 RVA: 0x001C458C File Offset: 0x001C278C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		this.characterNameTitle.OnStartedEditing += this.OnStartedEditing;
		this.characterNameTitle.OnNameChanged += this.OnNameChanged;
		this.reshuffleButton.onClick += delegate()
		{
			this.Reshuffle(true);
		};
		List<IListableOption> list = new List<IListableOption>();
		foreach (SkillGroup item in new List<SkillGroup>(Db.Get().SkillGroups.resources))
		{
			list.Add(item);
		}
		this.archetypeDropDown.Initialize(list, new Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false, null);
		this.archetypeDropDown.CustomizeEmptyRow(Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x06004FE6 RID: 20454 RVA: 0x001C46A4 File Offset: 0x001C28A4
	public void ForceStopEditingTitle()
	{
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x06004FE7 RID: 20455 RVA: 0x001C46B1 File Offset: 0x001C28B1
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06004FE8 RID: 20456 RVA: 0x001C46B8 File Offset: 0x001C28B8
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.GenerateCharacter(this.controller.IsStarterMinion, null);
		yield break;
	}

	// Token: 0x06004FE9 RID: 20457 RVA: 0x001C46C7 File Offset: 0x001C28C7
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x06004FEA RID: 20458 RVA: 0x001C46F4 File Offset: 0x001C28F4
	protected override void OnForcedCleanUp()
	{
		CharacterContainer.containers.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x06004FEB RID: 20459 RVA: 0x001C4708 File Offset: 0x001C2908
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x06004FEC RID: 20460 RVA: 0x001C47A0 File Offset: 0x001C29A0
	private void Initialize()
	{
		this.iconGroups = new List<GameObject>();
		this.traitEntries = new List<GameObject>();
		this.expectationLabels = new List<LocText>();
		this.aptitudeEntries = new List<GameObject>();
		if (CharacterContainer.containers == null)
		{
			CharacterContainer.containers = new List<CharacterContainer>();
		}
		CharacterContainer.containers.Add(this);
	}

	// Token: 0x06004FED RID: 20461 RVA: 0x001C47F5 File Offset: 0x001C29F5
	private void OnNameChanged(string newName)
	{
		this.stats.Name = newName;
		this.stats.personality.Name = newName;
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06004FEE RID: 20462 RVA: 0x001C482F File Offset: 0x001C2A2F
	private void OnStartedEditing()
	{
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x001C483C File Offset: 0x001C2A3C
	private void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
	{
		int num = 0;
		do
		{
			this.stats = new MinionStartingStats(is_starter, guaranteedAptitudeID, null, false);
			num++;
		}
		while (this.IsCharacterRedundant() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001C48E4 File Offset: 0x001C2AE4
	private void SetAnimator()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("MinionSelectPreview")), this.contentBody.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = this.baseCharacterScale;
		}
		MinionConfig.ConfigureSymbols(this.animController.gameObject, true);
		this.stats.ApplyTraits(this.animController.gameObject);
		this.stats.ApplyRace(this.animController.gameObject);
		this.stats.ApplyAccessories(this.animController.gameObject);
		this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyJoyResponseOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyExperience(this.animController.gameObject);
		HashedString idleAnim = this.GetIdleAnim(this.stats);
		this.idle_anim = Assets.GetAnim(idleAnim);
		if (this.idle_anim != null)
		{
			this.animController.AddAnimOverrides(this.idle_anim, 0f);
		}
		KAnimFile anim = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
		if (anim != null)
		{
			this.animController.AddAnimOverrides(anim, 0f);
		}
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001C4A80 File Offset: 0x001C2C80
	private HashedString GetIdleAnim(MinionStartingStats minionStartingStats)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, string[]> keyValuePair in CharacterContainer.traitIdleAnims)
		{
			foreach (Trait trait in minionStartingStats.Traits)
			{
				if (keyValuePair.Value.Contains(trait.Id))
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (keyValuePair.Value.Contains(minionStartingStats.joyTrait.Id) || keyValuePair.Value.Contains(minionStartingStats.stressTrait.Id))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray()[UnityEngine.Random.Range(0, list.Count)];
		}
		return CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)];
	}

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001C4BAC File Offset: 0x001C2DAC
	private void SetInfoText()
	{
		this.traitEntries.ForEach(delegate(GameObject tl)
		{
			UnityEngine.Object.Destroy(tl.gameObject);
		});
		this.traitEntries.Clear();
		this.characterNameTitle.SetTitle(this.stats.Name);
		for (int i = 1; i < this.stats.Traits.Count; i++)
		{
			Trait trait = this.stats.Traits[i];
			LocText locText = trait.PositiveTrait ? this.goodTrait : this.badTrait;
			LocText locText2 = Util.KInstantiateUI<LocText>(locText.gameObject, locText.transform.parent.gameObject, false);
			locText2.gameObject.SetActive(true);
			locText2.text = this.stats.Traits[i].Name;
			locText2.color = (trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR);
			locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.GetTooltip());
			for (int j = 0; j < trait.SelfModifiers.Count; j++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				string format = (trait.SelfModifiers[j].Value > 0f) ? UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED : UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED;
				componentInChildren.text = string.Format(format, Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"));
				trait.SelfModifiers[j].AttributeId == "GermResistance";
				Klei.AI.Attribute attribute = Db.Get().Attributes.Get(trait.SelfModifiers[j].AttributeId);
				string text = attribute.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"),
					": ",
					trait.SelfModifiers[j].GetFormattedString()
				});
				List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attribute);
				for (int k = 0; k < convertersForAttribute.Count; k++)
				{
					string text2 = convertersForAttribute[k].DescriptionFromAttribute(convertersForAttribute[k].multiplier * trait.SelfModifiers[j].Value, null);
					if (text2 != "")
					{
						text = text + "\n    • " + text2;
					}
				}
				componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(text);
				this.traitEntries.Add(gameObject);
			}
			if (trait.disabledChoreGroups != null)
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject2.SetActive(true);
				LocText componentInChildren2 = gameObject2.GetComponentInChildren<LocText>();
				componentInChildren2.text = trait.GetDisabledChoresString(false);
				string text3 = "";
				string text4 = "";
				for (int l = 0; l < trait.disabledChoreGroups.Length; l++)
				{
					if (l > 0)
					{
						text3 += ", ";
						text4 += "\n";
					}
					text3 += trait.disabledChoreGroups[l].Name;
					text4 += trait.disabledChoreGroups[l].description;
				}
				componentInChildren2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, text3, text4));
				this.traitEntries.Add(gameObject2);
			}
			if (trait.ignoredEffects != null && trait.ignoredEffects.Length != 0)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject3.SetActive(true);
				LocText componentInChildren3 = gameObject3.GetComponentInChildren<LocText>();
				componentInChildren3.text = trait.GetIgnoredEffectsString(false);
				string text5 = "";
				string text6 = "";
				for (int m = 0; m < trait.ignoredEffects.Length; m++)
				{
					if (m > 0)
					{
						text5 += ", ";
						text6 += "\n";
					}
					text5 += Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".NAME");
					text6 += Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".CAUSE");
				}
				componentInChildren3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, text5, text6));
				this.traitEntries.Add(gameObject3);
			}
			StringEntry stringEntry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC", out stringEntry))
			{
				GameObject gameObject4 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject4.SetActive(true);
				LocText componentInChildren4 = gameObject4.GetComponentInChildren<LocText>();
				componentInChildren4.text = stringEntry.String;
				componentInChildren4.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP"));
				this.traitEntries.Add(gameObject4);
			}
			this.traitEntries.Add(locText2.gameObject);
		}
		this.aptitudeEntries.ForEach(delegate(GameObject al)
		{
			UnityEngine.Object.Destroy(al.gameObject);
		});
		this.aptitudeEntries.Clear();
		this.expectationLabels.ForEach(delegate(LocText el)
		{
			UnityEngine.Object.Destroy(el.gameObject);
		});
		this.expectationLabels.Clear();
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.stats.skillAptitudes)
		{
			if (keyValuePair.Value != 0f)
			{
				SkillGroup skillGroup = Db.Get().SkillGroups.Get(keyValuePair.Key.IdHash);
				if (skillGroup == null)
				{
					global::Debug.LogWarningFormat("Role group not found for aptitude: {0}", new object[]
					{
						keyValuePair.Key
					});
				}
				else
				{
					GameObject gameObject5 = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeEntry.transform.parent.gameObject, false);
					LocText locText3 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, gameObject5, false);
					locText3.gameObject.SetActive(true);
					locText3.text = skillGroup.Name;
					string simpleTooltip;
					if (skillGroup.choreGroupID != "")
					{
						ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
						simpleTooltip = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS, choreGroup.description);
					}
					else
					{
						simpleTooltip = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS);
					}
					locText3.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
					float num = (float)this.stats.StartingLevels[keyValuePair.Key.relevantAttributes[0].Id];
					LocText locText4 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, gameObject5, false);
					locText4.gameObject.SetActive(true);
					locText4.text = "+" + num.ToString() + " " + keyValuePair.Key.relevantAttributes[0].Name;
					string text7 = keyValuePair.Key.relevantAttributes[0].Description;
					text7 = string.Concat(new string[]
					{
						text7,
						"\n\n",
						keyValuePair.Key.relevantAttributes[0].Name,
						": +",
						num.ToString()
					});
					List<AttributeConverter> convertersForAttribute2 = Db.Get().AttributeConverters.GetConvertersForAttribute(keyValuePair.Key.relevantAttributes[0]);
					for (int n = 0; n < convertersForAttribute2.Count; n++)
					{
						text7 = text7 + "\n    • " + convertersForAttribute2[n].DescriptionFromAttribute(convertersForAttribute2[n].multiplier * num, null);
					}
					locText4.GetComponent<ToolTip>().SetSimpleTooltip(text7);
					gameObject5.gameObject.SetActive(true);
					this.aptitudeEntries.Add(gameObject5);
				}
			}
		}
		if (this.stats.stressTrait != null)
		{
			LocText locText5 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText5.gameObject.SetActive(true);
			locText5.text = string.Format(UI.CHARACTERCONTAINER_STRESSTRAIT, this.stats.stressTrait.Name);
			locText5.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
			this.expectationLabels.Add(locText5);
		}
		if (this.stats.joyTrait != null)
		{
			LocText locText6 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText6.gameObject.SetActive(true);
			locText6.text = string.Format(UI.CHARACTERCONTAINER_JOYTRAIT, this.stats.joyTrait.Name);
			locText6.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.joyTrait.GetTooltip());
			this.expectationLabels.Add(locText6);
		}
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001C5630 File Offset: 0x001C3830
	private IEnumerator SetAttributes()
	{
		yield return null;
		this.iconGroups.ForEach(delegate(GameObject icg)
		{
			UnityEngine.Object.Destroy(icg);
		});
		this.iconGroups.Clear();
		List<AttributeInstance> list = new List<AttributeInstance>(this.animController.gameObject.GetAttributes().AttributeTable);
		list.RemoveAll((AttributeInstance at) => at.Attribute.ShowInUI != Klei.AI.Attribute.Display.Skill);
		list = (from at in list
		orderby at.Name
		select at).ToList<AttributeInstance>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.iconGroup.gameObject, this.iconGroup.transform.parent.gameObject, false);
			LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
			gameObject.SetActive(true);
			float totalValue = list[i].GetTotalValue();
			if (totalValue > 0f)
			{
				componentInChildren.color = Constants.POSITIVE_COLOR;
			}
			else if (totalValue == 0f)
			{
				componentInChildren.color = Constants.NEUTRAL_COLOR;
			}
			else
			{
				componentInChildren.color = Constants.NEGATIVE_COLOR;
			}
			componentInChildren.text = string.Format(UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f), list[i].Name);
			AttributeInstance attributeInstance = list[i];
			string text = attributeInstance.Description;
			if (attributeInstance.Attribute.converters.Count > 0)
			{
				text += "\n";
				foreach (AttributeConverter attributeConverter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance converter = this.animController.gameObject.GetComponent<Klei.AI.AttributeConverters>().GetConverter(attributeConverter.Id);
					string text2 = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
					if (text2 != null)
					{
						text = text + "\n" + text2;
					}
				}
			}
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(text);
			this.iconGroups.Add(gameObject);
		}
		yield break;
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001C5640 File Offset: 0x001C3840
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.stats);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Play("cheer_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001C5728 File Offset: 0x001C3928
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.stats);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001C57EE File Offset: 0x001C39EE
	private void OnReplacedEvent(ITelepadDeliverable deliverable)
	{
		if (deliverable == this.stats)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001C5800 File Offset: 0x001C3A00
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001C5876 File Offset: 0x001C3A76
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06004FF9 RID: 20473 RVA: 0x001C5888 File Offset: 0x001C3A88
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x001C58AC File Offset: 0x001C3AAC
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x06004FFB RID: 20475 RVA: 0x001C58FD File Offset: 0x001C3AFD
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
		this.archetypeDropDown.gameObject.SetActive(enable);
	}

	// Token: 0x06004FFC RID: 20476 RVA: 0x001C5924 File Offset: 0x001C3B24
	private void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		if (this.fxAnim != null)
		{
			this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
	}

	// Token: 0x06004FFD RID: 20477 RVA: 0x001C5994 File Offset: 0x001C3B94
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x06004FFE RID: 20478 RVA: 0x001C5A54 File Offset: 0x001C3C54
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x06004FFF RID: 20479 RVA: 0x001C5AB1 File Offset: 0x001C3CB1
	private bool IsCharacterRedundant()
	{
		return CharacterContainer.containers.Find((CharacterContainer c) => c != null && c.stats != null && c != this && c.stats.Name == this.stats.Name && c.stats.IsValid) != null || Components.LiveMinionIdentities.Items.Any((MinionIdentity id) => id.GetProperName() == this.stats.Name);
	}

	// Token: 0x06005000 RID: 20480 RVA: 0x001C5AEE File Offset: 0x001C3CEE
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x06005001 RID: 20481 RVA: 0x001C5AFE File Offset: 0x001C3CFE
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = true;
		base.OnPointerEnter(eventData);
	}

	// Token: 0x06005002 RID: 20482 RVA: 0x001C5B13 File Offset: 0x001C3D13
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = false;
		base.OnPointerExit(eventData);
	}

	// Token: 0x06005003 RID: 20483 RVA: 0x001C5B28 File Offset: 0x001C3D28
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape) || e.IsAction(global::Action.MouseRight))
		{
			this.characterNameTitle.ForceStopEditing();
			this.controller.OnPressBack();
			this.archetypeDropDown.scrollRect.gameObject.SetActive(false);
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyDown(e);
			return;
		}
		this.scroll_rect.OnKeyDown(e);
	}

	// Token: 0x06005004 RID: 20484 RVA: 0x001C5BF8 File Offset: 0x001C3DF8
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyUp(e);
			return;
		}
		this.scroll_rect.OnKeyUp(e);
	}

	// Token: 0x06005005 RID: 20485 RVA: 0x001C5C87 File Offset: 0x001C3E87
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.stats == null)
		{
			return;
		}
		this.SetAnimator();
	}

	// Token: 0x06005006 RID: 20486 RVA: 0x001C5C9E File Offset: 0x001C3E9E
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x06005007 RID: 20487 RVA: 0x001C5CB4 File Offset: 0x001C3EB4
	private void OnArchetypeEntryClick(IListableOption skill, object data)
	{
		if (skill != null)
		{
			SkillGroup skillGroup = skill as SkillGroup;
			this.guaranteedAptitudeID = skillGroup.Id;
			this.selectedArchetypeIcon.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
			this.Reshuffle(true);
			return;
		}
		this.guaranteedAptitudeID = null;
		this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
		this.Reshuffle(true);
	}

	// Token: 0x06005008 RID: 20488 RVA: 0x001C5D19 File Offset: 0x001C3F19
	private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		if (b.Equals("Random"))
		{
			return -1;
		}
		return b.GetProperName().CompareTo(a.GetProperName());
	}

	// Token: 0x06005009 RID: 20489 RVA: 0x001C5D3C File Offset: 0x001C3F3C
	private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillGroup skillGroup = entry.entryData as SkillGroup;
			entry.image.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
		}
	}

	// Token: 0x0400343B RID: 13371
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x0400343C RID: 13372
	[SerializeField]
	private LocText characterName;

	// Token: 0x0400343D RID: 13373
	[SerializeField]
	private EditableTitleBar characterNameTitle;

	// Token: 0x0400343E RID: 13374
	[SerializeField]
	private LocText characterJob;

	// Token: 0x0400343F RID: 13375
	public GameObject selectedBorder;

	// Token: 0x04003440 RID: 13376
	[SerializeField]
	private Image titleBar;

	// Token: 0x04003441 RID: 13377
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04003442 RID: 13378
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04003443 RID: 13379
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04003444 RID: 13380
	private KBatchedAnimController animController;

	// Token: 0x04003445 RID: 13381
	[SerializeField]
	private GameObject iconGroup;

	// Token: 0x04003446 RID: 13382
	private List<GameObject> iconGroups;

	// Token: 0x04003447 RID: 13383
	[SerializeField]
	private LocText goodTrait;

	// Token: 0x04003448 RID: 13384
	[SerializeField]
	private LocText badTrait;

	// Token: 0x04003449 RID: 13385
	[SerializeField]
	private GameObject aptitudeEntry;

	// Token: 0x0400344A RID: 13386
	[SerializeField]
	private Transform aptitudeLabel;

	// Token: 0x0400344B RID: 13387
	[SerializeField]
	private Transform attributeLabelAptitude;

	// Token: 0x0400344C RID: 13388
	[SerializeField]
	private Transform attributeLabelTrait;

	// Token: 0x0400344D RID: 13389
	[SerializeField]
	private LocText expectationRight;

	// Token: 0x0400344E RID: 13390
	private List<LocText> expectationLabels;

	// Token: 0x0400344F RID: 13391
	[SerializeField]
	private DropDown archetypeDropDown;

	// Token: 0x04003450 RID: 13392
	[SerializeField]
	private Image selectedArchetypeIcon;

	// Token: 0x04003451 RID: 13393
	[SerializeField]
	private Sprite noArchetypeIcon;

	// Token: 0x04003452 RID: 13394
	[SerializeField]
	private Sprite dropdownArrowIcon;

	// Token: 0x04003453 RID: 13395
	private string guaranteedAptitudeID;

	// Token: 0x04003454 RID: 13396
	private List<GameObject> aptitudeEntries;

	// Token: 0x04003455 RID: 13397
	private List<GameObject> traitEntries;

	// Token: 0x04003456 RID: 13398
	[SerializeField]
	private LocText description;

	// Token: 0x04003457 RID: 13399
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04003458 RID: 13400
	[SerializeField]
	private KBatchedAnimController fxAnim;

	// Token: 0x04003459 RID: 13401
	private MinionStartingStats stats;

	// Token: 0x0400345A RID: 13402
	private CharacterSelectionController controller;

	// Token: 0x0400345B RID: 13403
	private static List<CharacterContainer> containers;

	// Token: 0x0400345C RID: 13404
	private KAnimFile idle_anim;

	// Token: 0x0400345D RID: 13405
	[HideInInspector]
	public bool addMinionToIdentityList = true;

	// Token: 0x0400345E RID: 13406
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x0400345F RID: 13407
	[SerializeField]
	private KScrollRect scroll_rect;

	// Token: 0x04003460 RID: 13408
	private static readonly Dictionary<HashedString, string[]> traitIdleAnims = new Dictionary<HashedString, string[]>
	{
		{
			"anim_idle_food_kanim",
			new string[]
			{
				"Foodie"
			}
		},
		{
			"anim_idle_animal_lover_kanim",
			new string[]
			{
				"RanchingUp"
			}
		},
		{
			"anim_idle_loner_kanim",
			new string[]
			{
				"Loner"
			}
		},
		{
			"anim_idle_mole_hands_kanim",
			new string[]
			{
				"MoleHands"
			}
		},
		{
			"anim_idle_buff_kanim",
			new string[]
			{
				"StrongArm"
			}
		},
		{
			"anim_idle_distracted_kanim",
			new string[]
			{
				"CantResearch",
				"CantBuild",
				"CantCook",
				"CantDig"
			}
		},
		{
			"anim_idle_coaster_kanim",
			new string[]
			{
				"HappySinger"
			}
		}
	};

	// Token: 0x04003461 RID: 13409
	private static readonly HashedString[] idleAnims = new HashedString[]
	{
		"anim_idle_healthy_kanim",
		"anim_idle_susceptible_kanim",
		"anim_idle_keener_kanim",
		"anim_idle_fastfeet_kanim",
		"anim_idle_breatherdeep_kanim",
		"anim_idle_breathershallow_kanim"
	};

	// Token: 0x04003462 RID: 13410
	public float baseCharacterScale = 0.38f;

	// Token: 0x020018EB RID: 6379
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x0400735B RID: 29531
		public string professionName;

		// Token: 0x0400735C RID: 29532
		public Sprite iconImg;
	}
}

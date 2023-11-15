using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B57 RID: 2903
public class CrewJobsEntry : CrewListEntry
{
	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x060059BC RID: 22972 RVA: 0x0020D2A5 File Offset: 0x0020B4A5
	// (set) Token: 0x060059BD RID: 22973 RVA: 0x0020D2AD File Offset: 0x0020B4AD
	public ChoreConsumer consumer { get; private set; }

	// Token: 0x060059BE RID: 22974 RVA: 0x0020D2B8 File Offset: 0x0020B4B8
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.consumer = _identity.GetComponent<ChoreConsumer>();
		ChoreConsumer consumer = this.consumer;
		consumer.choreRulesChanged = (System.Action)Delegate.Combine(consumer.choreRulesChanged, new System.Action(this.Dirty));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.CreateChoreButton(chore_group);
		}
		this.CreateAllTaskButton();
		this.dirty = true;
	}

	// Token: 0x060059BF RID: 22975 RVA: 0x0020D35C File Offset: 0x0020B55C
	private void CreateChoreButton(ChoreGroup chore_group)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButton, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = chore_group.Id;
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = chore_group.Name;
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.choreGroup = chore_group;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		priorityButton.tooltip.OnToolTip = (() => this.OnPriorityButtonTooltip(priorityButton));
		priorityButton.button.onClick.AddListener(delegate()
		{
			this.OnPriorityPress(chore_group);
		});
		this.PriorityButtons.Add(priorityButton);
	}

	// Token: 0x060059C0 RID: 22976 RVA: 0x0020D4D8 File Offset: 0x0020B6D8
	private void CreateAllTaskButton()
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButtonAllTasks, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = "AllTasks";
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = "";
		Button b = gameObject.GetComponent<Button>();
		b.onClick.AddListener(delegate()
		{
			this.ToggleTasksAll(b);
		});
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		this.AllTasksButton = priorityButton;
	}

	// Token: 0x060059C1 RID: 22977 RVA: 0x0020D5E8 File Offset: 0x0020B7E8
	private void ToggleTasksAll(Button button)
	{
		bool flag = this.rowToggleState != CrewJobsScreen.everyoneToggleState.on;
		string name = "HUD_Click_Deselect";
		if (flag)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.consumer.SetPermittedByUser(chore_group, flag);
		}
	}

	// Token: 0x060059C2 RID: 22978 RVA: 0x0020D674 File Offset: 0x0020B874
	private void OnPriorityPress(ChoreGroup chore_group)
	{
		bool flag = this.consumer.IsPermittedByUser(chore_group);
		string name = "HUD_Click";
		if (flag)
		{
			name = "HUD_Click_Deselect";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		this.consumer.SetPermittedByUser(chore_group, !this.consumer.IsPermittedByUser(chore_group));
	}

	// Token: 0x060059C3 RID: 22979 RVA: 0x0020D6C8 File Offset: 0x0020B8C8
	private void Refresh(object data = null)
	{
		if (this.identity == null)
		{
			this.dirty = false;
			return;
		}
		if (this.dirty)
		{
			Attributes attributes = this.identity.GetAttributes();
			foreach (CrewJobsEntry.PriorityButton priorityButton in this.PriorityButtons)
			{
				bool flag = this.consumer.IsPermittedByUser(priorityButton.choreGroup);
				if (priorityButton.ToggleIcon.activeSelf != flag)
				{
					priorityButton.ToggleIcon.SetActive(flag);
				}
				float t = Mathf.Min(attributes.Get(priorityButton.choreGroup.attribute).GetTotalValue() / 10f, 1f);
				Color baseBorderColor = priorityButton.baseBorderColor;
				baseBorderColor.r = Mathf.Lerp(priorityButton.baseBorderColor.r, 0.72156864f, t);
				baseBorderColor.g = Mathf.Lerp(priorityButton.baseBorderColor.g, 0.44313726f, t);
				baseBorderColor.b = Mathf.Lerp(priorityButton.baseBorderColor.b, 0.5803922f, t);
				if (priorityButton.border.color != baseBorderColor)
				{
					priorityButton.border.color = baseBorderColor;
				}
				Color color = priorityButton.baseBackgroundColor;
				color.a = Mathf.Lerp(0f, 1f, t);
				bool flag2 = this.consumer.IsPermittedByTraits(priorityButton.choreGroup);
				if (!flag2)
				{
					color = Color.clear;
					priorityButton.border.color = Color.clear;
					priorityButton.ToggleIcon.SetActive(false);
				}
				priorityButton.button.interactable = flag2;
				if (priorityButton.background.color != color)
				{
					priorityButton.background.color = color;
				}
			}
			int num = 0;
			int num2 = 0;
			foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
			{
				if (this.consumer.IsPermittedByTraits(chore_group))
				{
					num2++;
					if (this.consumer.IsPermittedByUser(chore_group))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.off;
			}
			else if (num < num2)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.mixed;
			}
			else
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.on;
			}
			ImageToggleState component = this.AllTasksButton.ToggleIcon.GetComponent<ImageToggleState>();
			switch (this.rowToggleState)
			{
			case CrewJobsScreen.everyoneToggleState.off:
				component.SetDisabled();
				break;
			case CrewJobsScreen.everyoneToggleState.mixed:
				component.SetInactive();
				break;
			case CrewJobsScreen.everyoneToggleState.on:
				component.SetActive();
				break;
			}
			this.dirty = false;
		}
	}

	// Token: 0x060059C4 RID: 22980 RVA: 0x0020D9B8 File Offset: 0x0020BBB8
	private string OnPriorityButtonTooltip(CrewJobsEntry.PriorityButton b)
	{
		b.tooltip.ClearMultiStringTooltip();
		if (this.identity != null)
		{
			Attributes attributes = this.identity.GetAttributes();
			if (attributes != null)
			{
				if (!this.consumer.IsPermittedByTraits(b.choreGroup))
				{
					string newString = string.Format(UI.TOOLTIPS.JOBSSCREEN_CANNOTPERFORMTASK, this.consumer.GetComponent<MinionIdentity>().GetProperName());
					b.tooltip.AddMultiStringTooltip(newString, this.TooltipTextStyle_AbilityNegativeModifier);
					return "";
				}
				b.tooltip.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_RELEVANT_ATTRIBUTES, this.TooltipTextStyle_Ability);
				Klei.AI.Attribute attribute = b.choreGroup.attribute;
				AttributeInstance attributeInstance = attributes.Get(attribute);
				float totalValue = attributeInstance.GetTotalValue();
				TextStyleSetting styleSetting = this.TooltipTextStyle_Ability;
				if (totalValue > 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityPositiveModifier;
				}
				else if (totalValue < 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityNegativeModifier;
				}
				b.tooltip.AddMultiStringTooltip(attribute.Name + " " + attributeInstance.GetTotalValue().ToString(), styleSetting);
			}
		}
		return "";
	}

	// Token: 0x060059C5 RID: 22981 RVA: 0x0020DAD1 File Offset: 0x0020BCD1
	private void LateUpdate()
	{
		this.Refresh(null);
	}

	// Token: 0x060059C6 RID: 22982 RVA: 0x0020DADA File Offset: 0x0020BCDA
	private void OnLevelUp(object data)
	{
		this.Dirty();
	}

	// Token: 0x060059C7 RID: 22983 RVA: 0x0020DAE2 File Offset: 0x0020BCE2
	private void Dirty()
	{
		this.dirty = true;
		CrewJobsScreen.Instance.Dirty(null);
	}

	// Token: 0x060059C8 RID: 22984 RVA: 0x0020DAF6 File Offset: 0x0020BCF6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.consumer != null)
		{
			ChoreConsumer consumer = this.consumer;
			consumer.choreRulesChanged = (System.Action)Delegate.Remove(consumer.choreRulesChanged, new System.Action(this.Dirty));
		}
	}

	// Token: 0x04003CC6 RID: 15558
	public GameObject Prefab_JobPriorityButton;

	// Token: 0x04003CC7 RID: 15559
	public GameObject Prefab_JobPriorityButtonAllTasks;

	// Token: 0x04003CC8 RID: 15560
	private List<CrewJobsEntry.PriorityButton> PriorityButtons = new List<CrewJobsEntry.PriorityButton>();

	// Token: 0x04003CC9 RID: 15561
	private CrewJobsEntry.PriorityButton AllTasksButton;

	// Token: 0x04003CCA RID: 15562
	public TextStyleSetting TooltipTextStyle_Title;

	// Token: 0x04003CCB RID: 15563
	public TextStyleSetting TooltipTextStyle_Ability;

	// Token: 0x04003CCC RID: 15564
	public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;

	// Token: 0x04003CCD RID: 15565
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04003CCE RID: 15566
	private bool dirty;

	// Token: 0x04003CD0 RID: 15568
	private CrewJobsScreen.everyoneToggleState rowToggleState;

	// Token: 0x02001A6B RID: 6763
	[Serializable]
	public struct PriorityButton
	{
		// Token: 0x0400797B RID: 31099
		public Button button;

		// Token: 0x0400797C RID: 31100
		public GameObject ToggleIcon;

		// Token: 0x0400797D RID: 31101
		public ChoreGroup choreGroup;

		// Token: 0x0400797E RID: 31102
		public ToolTip tooltip;

		// Token: 0x0400797F RID: 31103
		public Image border;

		// Token: 0x04007980 RID: 31104
		public Image background;

		// Token: 0x04007981 RID: 31105
		public Color baseBorderColor;

		// Token: 0x04007982 RID: 31106
		public Color baseBackgroundColor;
	}
}

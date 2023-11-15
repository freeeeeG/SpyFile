using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000C63 RID: 3171
[AddComponentMenu("KMonoBehaviour/scripts/SkillWidget")]
public class SkillWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x060064DD RID: 25821 RVA: 0x00255FBB File Offset: 0x002541BB
	// (set) Token: 0x060064DE RID: 25822 RVA: 0x00255FC3 File Offset: 0x002541C3
	public string skillID { get; private set; }

	// Token: 0x060064DF RID: 25823 RVA: 0x00255FCC File Offset: 0x002541CC
	public void Refresh(string skillID)
	{
		Skill skill = Db.Get().Skills.Get(skillID);
		if (skill == null)
		{
			global::Debug.LogWarning("DbSkills is missing skillId " + skillID);
			return;
		}
		this.Name.text = skill.Name;
		LocText name = this.Name;
		name.text = name.text + "\n(" + Db.Get().SkillGroups.Get(skill.skillGroup).Name + ")";
		this.skillID = skillID;
		this.tooltip.SetSimpleTooltip(this.SkillTooltip(skill));
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(skillID);
			bool flag = minionResume.CanMasterSkill(skillMasteryConditions);
			if (!(minionResume == null) && (minionResume.HasMasteredSkill(skillID) || flag))
			{
				this.TitleBarBG.color = (minionResume.HasMasteredSkill(skillID) ? this.header_color_has_skill : this.header_color_can_assign);
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.HasMasteredSkill(skillID))
			{
				this.TitleBarBG.color = this.header_color_has_skill;
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		this.hatImage.sprite = Assets.GetSprite(skill.badge);
		bool active = false;
		bool flag2 = false;
		if (minionResume != null)
		{
			flag2 = minionResume.HasBeenGrantedSkill(skill);
			float num;
			minionResume.AptitudeBySkillGroup.TryGetValue(skill.skillGroup, out num);
			active = (num > 0f && !flag2);
		}
		this.aptitudeBox.SetActive(active);
		this.grantedBox.SetActive(flag2);
		this.traitDisabledIcon.SetActive(minionResume != null && !minionResume.IsAbleToLearnSkill(skill.Id));
		string text = "";
		List<string> list = new List<string>();
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			MinionResume component = minionIdentity2.GetComponent<MinionResume>();
			if (component != null && component.HasMasteredSkill(skillID))
			{
				list.Add(component.GetProperName());
			}
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity storedMinionIdentity2 = info.serializedMinion.Get<StoredMinionIdentity>();
					if (storedMinionIdentity2 != null && storedMinionIdentity2.HasMasteredSkill(skillID))
					{
						list.Add(storedMinionIdentity2.GetProperName());
					}
				}
			}
		}
		this.masteryCount.gameObject.SetActive(list.Count > 0);
		foreach (string str in list)
		{
			text = text + "\n    • " + str;
		}
		this.masteryCount.SetSimpleTooltip((list.Count > 0) ? string.Format(UI.ROLES_SCREEN.WIDGET.NUMBER_OF_MASTERS_TOOLTIP, text) : UI.ROLES_SCREEN.WIDGET.NO_MASTERS_TOOLTIP.text);
		this.masteryCount.GetComponentInChildren<LocText>().text = list.Count.ToString();
	}

	// Token: 0x060064E0 RID: 25824 RVA: 0x002563F8 File Offset: 0x002545F8
	public void RefreshLines()
	{
		this.prerequisiteSkillWidgets.Clear();
		List<Vector2> list = new List<Vector2>();
		foreach (string text in Db.Get().Skills.Get(this.skillID).priorSkills)
		{
			list.Add(this.skillsScreen.GetSkillWidgetLineTargetPosition(text));
			this.prerequisiteSkillWidgets.Add(this.skillsScreen.GetSkillWidget(text));
		}
		if (this.lines != null)
		{
			for (int i = this.lines.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.lines[i].gameObject);
			}
		}
		this.linePoints.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			float num = this.lines_left.GetPosition().x - list[j].x - 12f;
			float y = 0f;
			this.linePoints.Add(new Vector2(0f, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-(this.lines_left.GetPosition().x - list[j].x), -(this.lines_left.GetPosition().y - list[j].y)));
		}
		this.lines = new UILineRenderer[this.linePoints.Count / 2];
		int num2 = 0;
		for (int k = 0; k < this.linePoints.Count; k += 2)
		{
			GameObject gameObject = new GameObject("Line");
			gameObject.AddComponent<RectTransform>();
			gameObject.transform.SetParent(this.lines_left.transform);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			this.lines[num2] = gameObject.AddComponent<UILineRenderer>();
			this.lines[num2].color = new Color(0.6509804f, 0.6509804f, 0.6509804f, 1f);
			this.lines[num2].Points = new Vector2[]
			{
				this.linePoints[k],
				this.linePoints[k + 1]
			};
			num2++;
		}
	}

	// Token: 0x060064E1 RID: 25825 RVA: 0x0025670C File Offset: 0x0025490C
	public void ToggleBorderHighlight(bool on)
	{
		this.borderHighlight.SetActive(on);
		if (this.lines != null)
		{
			foreach (UILineRenderer uilineRenderer in this.lines)
			{
				uilineRenderer.color = (on ? this.line_color_active : this.line_color_default);
				uilineRenderer.LineThickness = (float)(on ? 4 : 2);
				uilineRenderer.SetAllDirty();
			}
		}
		for (int j = 0; j < this.prerequisiteSkillWidgets.Count; j++)
		{
			this.prerequisiteSkillWidgets[j].ToggleBorderHighlight(on);
		}
	}

	// Token: 0x060064E2 RID: 25826 RVA: 0x00256797 File Offset: 0x00254997
	public string SkillTooltip(Skill skill)
	{
		return "" + SkillWidget.SkillPerksString(skill) + "\n" + this.DuplicantSkillString(skill);
	}

	// Token: 0x060064E3 RID: 25827 RVA: 0x002567BC File Offset: 0x002549BC
	public static string SkillPerksString(Skill skill)
	{
		string text = "";
		foreach (SkillPerk skillPerk in skill.perks)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			text = text + "• " + skillPerk.Name;
		}
		return text;
	}

	// Token: 0x060064E4 RID: 25828 RVA: 0x00256838 File Offset: 0x00254A38
	public string CriteriaString(Skill skill)
	{
		bool flag = false;
		string text = "";
		text = text + "<b>" + UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.TITLE + "</b>\n";
		SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
		if (skillGroup != null && skillGroup.relevantAttributes != null)
		{
			foreach (Klei.AI.Attribute attribute in skillGroup.relevantAttributes)
			{
				if (attribute != null)
				{
					text = text + "    • " + string.Format(UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.SKILLGROUP_ENABLED.DESCRIPTION, attribute.Name) + "\n";
					flag = true;
				}
			}
		}
		if (skill.priorSkills.Count > 0)
		{
			flag = true;
			for (int i = 0; i < skill.priorSkills.Count; i++)
			{
				text = text + "    • " + string.Format("{0}", Db.Get().Skills.Get(skill.priorSkills[i]).Name);
				text += "</color>";
				if (i != skill.priorSkills.Count - 1)
				{
					text += "\n";
				}
			}
		}
		if (!flag)
		{
			text = text + "    • " + string.Format(UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.NONE, skill.Name);
		}
		return text;
	}

	// Token: 0x060064E5 RID: 25829 RVA: 0x002569A8 File Offset: 0x00254BA8
	public string DuplicantSkillString(Skill skill)
	{
		string text = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (component == null)
			{
				return "";
			}
			LocString loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CAN_MASTER;
			if (component.HasMasteredSkill(skill.Id))
			{
				if (component.HasBeenGrantedSkill(skill))
				{
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_GRANTED;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
				}
			}
			else
			{
				MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(skill.Id);
				if (!component.CanMasterSkill(skillMasteryConditions))
				{
					bool flag = false;
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CANNOT_MASTER;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn))
					{
						flag = true;
						string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
						Trait trait;
						minionIdentity.GetComponent<Traits>().IsChoreGroupDisabled(choreGroupID, out trait);
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.PREVENTED_BY_TRAIT;
						text += string.Format(loc_string, trait.Name);
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.MissingPreviousSkill))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_PREVIOUS_SKILLS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.NeedsSkillPoints))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_MORE_SKILL_POINTS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
				}
				else
				{
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.StressWarning))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.STRESS_WARNING_MESSAGE;
						text += string.Format(loc_string, skill.Name, minionIdentity.GetProperName());
					}
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.SkillAptitude))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_APTITUDE;
						text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x060064E6 RID: 25830 RVA: 0x00256C96 File Offset: 0x00254E96
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(true);
		this.skillsScreen.HoverSkill(this.skillID);
		this.soundPlayer.Play(1);
	}

	// Token: 0x060064E7 RID: 25831 RVA: 0x00256CBC File Offset: 0x00254EBC
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(false);
		this.skillsScreen.HoverSkill(null);
	}

	// Token: 0x060064E8 RID: 25832 RVA: 0x00256CD4 File Offset: 0x00254ED4
	public void OnPointerClick(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
			{
				component.ForceAddSkillPoint();
			}
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(this.skillID);
			bool flag = component.CanMasterSkill(skillMasteryConditions);
			if (component != null && !component.HasMasteredSkill(this.skillID) && flag)
			{
				component.MasterSkill(this.skillID);
				this.skillsScreen.RefreshAll();
			}
		}
	}

	// Token: 0x060064E9 RID: 25833 RVA: 0x00256D70 File Offset: 0x00254F70
	public void OnPointerDown(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		bool flag = false;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(this.skillID);
			flag = minionResume.CanMasterSkill(skillMasteryConditions);
		}
		if (minionResume != null && !minionResume.HasMasteredSkill(this.skillID) && flag)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x040044FD RID: 17661
	[SerializeField]
	private LocText Name;

	// Token: 0x040044FE RID: 17662
	[SerializeField]
	private LocText Description;

	// Token: 0x040044FF RID: 17663
	[SerializeField]
	private Image TitleBarBG;

	// Token: 0x04004500 RID: 17664
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x04004501 RID: 17665
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04004502 RID: 17666
	[SerializeField]
	private RectTransform lines_left;

	// Token: 0x04004503 RID: 17667
	[SerializeField]
	public RectTransform lines_right;

	// Token: 0x04004504 RID: 17668
	[SerializeField]
	private Color header_color_has_skill;

	// Token: 0x04004505 RID: 17669
	[SerializeField]
	private Color header_color_can_assign;

	// Token: 0x04004506 RID: 17670
	[SerializeField]
	private Color header_color_disabled;

	// Token: 0x04004507 RID: 17671
	[SerializeField]
	private Color line_color_default;

	// Token: 0x04004508 RID: 17672
	[SerializeField]
	private Color line_color_active;

	// Token: 0x04004509 RID: 17673
	[SerializeField]
	private Image hatImage;

	// Token: 0x0400450A RID: 17674
	[SerializeField]
	private GameObject borderHighlight;

	// Token: 0x0400450B RID: 17675
	[SerializeField]
	private ToolTip masteryCount;

	// Token: 0x0400450C RID: 17676
	[SerializeField]
	private GameObject aptitudeBox;

	// Token: 0x0400450D RID: 17677
	[SerializeField]
	private GameObject grantedBox;

	// Token: 0x0400450E RID: 17678
	[SerializeField]
	private GameObject traitDisabledIcon;

	// Token: 0x0400450F RID: 17679
	public TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x04004510 RID: 17680
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04004511 RID: 17681
	private List<SkillWidget> prerequisiteSkillWidgets = new List<SkillWidget>();

	// Token: 0x04004512 RID: 17682
	private UILineRenderer[] lines;

	// Token: 0x04004513 RID: 17683
	private List<Vector2> linePoints = new List<Vector2>();

	// Token: 0x04004514 RID: 17684
	public Material defaultMaterial;

	// Token: 0x04004515 RID: 17685
	public Material desaturatedMaterial;

	// Token: 0x04004516 RID: 17686
	public ButtonSoundPlayer soundPlayer;
}

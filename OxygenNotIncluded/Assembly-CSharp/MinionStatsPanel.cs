using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B94 RID: 2964
public class MinionStatsPanel : TargetScreen
{
	// Token: 0x06005C2A RID: 23594 RVA: 0x0021BE36 File Offset: 0x0021A036
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>();
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x0021BE44 File Offset: 0x0021A044
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resumePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		this.attributesPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		this.resumeDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.resumePanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
		this.attributesDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.attributesPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x0021BEDB File Offset: 0x0021A0DB
	protected override void OnCleanUp()
	{
		this.updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005C2D RID: 23597 RVA: 0x0021BEEE File Offset: 0x0021A0EE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Refresh();
		this.ScheduleUpdate();
	}

	// Token: 0x06005C2E RID: 23598 RVA: 0x0021BF02 File Offset: 0x0021A102
	public override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		this.Refresh();
	}

	// Token: 0x06005C2F RID: 23599 RVA: 0x0021BF11 File Offset: 0x0021A111
	private void ScheduleUpdate()
	{
		this.updateHandle = UIScheduler.Instance.Schedule("RefreshMinionStatsPanel", 1f, delegate(object o)
		{
			this.Refresh();
			this.ScheduleUpdate();
		}, null, null);
	}

	// Token: 0x06005C30 RID: 23600 RVA: 0x0021BF3C File Offset: 0x0021A13C
	private GameObject AddOrGetLabel(Dictionary<string, GameObject> labels, GameObject panel, string id)
	{
		GameObject gameObject;
		if (labels.ContainsKey(id))
		{
			gameObject = labels[id];
		}
		else
		{
			gameObject = Util.KInstantiate(this.attributesLabelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, null);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			labels[id] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005C31 RID: 23601 RVA: 0x0021BFAA File Offset: 0x0021A1AA
	private void Refresh()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (this.selectedTarget == null || this.selectedTarget.GetComponent<MinionIdentity>() == null)
		{
			return;
		}
		this.RefreshResume();
		this.RefreshAttributes();
	}

	// Token: 0x06005C32 RID: 23602 RVA: 0x0021BFE8 File Offset: 0x0021A1E8
	private void RefreshAttributes()
	{
		if (!this.selectedTarget.GetComponent<MinionIdentity>())
		{
			this.attributesPanel.SetActive(false);
			return;
		}
		this.attributesPanel.SetActive(true);
		this.attributesPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = UI.DETAILTABS.STATS.GROUPNAME_ATTRIBUTES;
		List<AttributeInstance> list = new List<AttributeInstance>(this.selectedTarget.GetAttributes().AttributeTable).FindAll((AttributeInstance a) => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill);
		this.attributesDrawer.BeginDrawing();
		if (list.Count > 0)
		{
			foreach (AttributeInstance attributeInstance in list)
			{
				this.attributesDrawer.NewLabel(string.Format("{0}: {1}", attributeInstance.Name, attributeInstance.GetFormattedValue())).Tooltip(attributeInstance.GetAttributeValueTooltip());
			}
		}
		this.attributesDrawer.EndDrawing();
	}

	// Token: 0x06005C33 RID: 23603 RVA: 0x0021C104 File Offset: 0x0021A304
	private void RefreshResume()
	{
		MinionResume component = this.selectedTarget.GetComponent<MinionResume>();
		if (!component)
		{
			this.resumePanel.SetActive(false);
			return;
		}
		this.resumePanel.SetActive(true);
		this.resumePanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = string.Format(UI.DETAILTABS.PERSONALITY.GROUPNAME_RESUME, this.selectedTarget.name.ToUpper());
		this.resumeDrawer.BeginDrawing();
		List<Skill> list = new List<Skill>();
		foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				Skill item = Db.Get().Skills.Get(keyValuePair.Key);
				list.Add(item);
			}
		}
		this.resumeDrawer.NewLabel(UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS).Tooltip(UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS_TOOLTIP);
		if (list.Count == 0)
		{
			this.resumeDrawer.NewLabel("  • " + UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.NAME).Tooltip(string.Format(UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.TOOLTIP, this.selectedTarget.name));
		}
		else
		{
			foreach (Skill skill in list)
			{
				string text = "";
				foreach (SkillPerk skillPerk in skill.perks)
				{
					text = text + "  • " + skillPerk.Name + "\n";
				}
				this.resumeDrawer.NewLabel("  • " + skill.Name).Tooltip(skill.description + "\n" + text);
			}
		}
		this.resumeDrawer.EndDrawing();
	}

	// Token: 0x04003E17 RID: 15895
	public GameObject attributesLabelTemplate;

	// Token: 0x04003E18 RID: 15896
	private GameObject resumePanel;

	// Token: 0x04003E19 RID: 15897
	private GameObject attributesPanel;

	// Token: 0x04003E1A RID: 15898
	private DetailsPanelDrawer resumeDrawer;

	// Token: 0x04003E1B RID: 15899
	private DetailsPanelDrawer attributesDrawer;

	// Token: 0x04003E1C RID: 15900
	private SchedulerHandle updateHandle;
}

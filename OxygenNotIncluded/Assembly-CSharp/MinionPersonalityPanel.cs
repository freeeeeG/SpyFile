using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B93 RID: 2963
public class MinionPersonalityPanel : TargetScreen
{
	// Token: 0x06005C1B RID: 23579 RVA: 0x0021B888 File Offset: 0x00219A88
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>() != null;
	}

	// Token: 0x06005C1C RID: 23580 RVA: 0x0021B896 File Offset: 0x00219A96
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
	}

	// Token: 0x06005C1D RID: 23581 RVA: 0x0021B89F File Offset: 0x00219A9F
	public override void OnSelectTarget(GameObject target)
	{
		this.panel.SetSelectedMinion(target);
		this.panel.Refresh(null);
		base.OnSelectTarget(target);
		this.Refresh();
	}

	// Token: 0x06005C1E RID: 23582 RVA: 0x0021B8C6 File Offset: 0x00219AC6
	public override void OnDeselectTarget(GameObject target)
	{
	}

	// Token: 0x06005C1F RID: 23583 RVA: 0x0021B8C8 File Offset: 0x00219AC8
	protected override void OnActivate()
	{
		base.OnActivate();
		if (this.panel == null)
		{
			this.panel = base.GetComponent<MinionEquipmentPanel>();
		}
	}

	// Token: 0x06005C20 RID: 23584 RVA: 0x0021B8EC File Offset: 0x00219AEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.bioPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		this.traitsPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		this.bioDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.bioPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
		this.traitsDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.traitsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
	}

	// Token: 0x06005C21 RID: 23585 RVA: 0x0021B983 File Offset: 0x00219B83
	protected override void OnCleanUp()
	{
		this.updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005C22 RID: 23586 RVA: 0x0021B996 File Offset: 0x00219B96
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.panel == null)
		{
			this.panel = base.GetComponent<MinionEquipmentPanel>();
		}
		this.Refresh();
		this.ScheduleUpdate();
	}

	// Token: 0x06005C23 RID: 23587 RVA: 0x0021B9C4 File Offset: 0x00219BC4
	private void ScheduleUpdate()
	{
		this.updateHandle = UIScheduler.Instance.Schedule("RefreshMinionPersonalityPanel", 1f, delegate(object o)
		{
			this.Refresh();
			this.ScheduleUpdate();
		}, null, null);
	}

	// Token: 0x06005C24 RID: 23588 RVA: 0x0021B9F0 File Offset: 0x00219BF0
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

	// Token: 0x06005C25 RID: 23589 RVA: 0x0021BA5E File Offset: 0x00219C5E
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
		this.RefreshBio();
		this.RefreshTraits();
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x0021BA9C File Offset: 0x00219C9C
	private void RefreshBio()
	{
		MinionIdentity component = this.selectedTarget.GetComponent<MinionIdentity>();
		if (!component)
		{
			this.bioPanel.SetActive(false);
			return;
		}
		this.bioPanel.SetActive(true);
		this.bioPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = UI.DETAILTABS.PERSONALITY.GROUPNAME_BIO;
		this.bioDrawer.BeginDrawing().NewLabel(DUPLICANTS.NAMETITLE + component.name).NewLabel(DUPLICANTS.ARRIVALTIME + GameUtil.GetFormattedCycles(((float)GameClock.Instance.GetCycle() - component.arrivalTime) * 600f, "F0", true)).Tooltip(string.Format(DUPLICANTS.ARRIVALTIME_TOOLTIP, component.arrivalTime + 1f, component.name)).NewLabel(DUPLICANTS.GENDERTITLE + string.Format(Strings.Get(string.Format("STRINGS.DUPLICANTS.GENDER.{0}.NAME", component.genderStringKey.ToUpper())), component.gender)).NewLabel(string.Format(Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", component.nameStringKey.ToUpper())), component.name)).Tooltip(string.Format(Strings.Get(string.Format("STRINGS.DUPLICANTS.DESC_TOOLTIP", component.nameStringKey.ToUpper())), component.name));
		MinionResume component2 = this.selectedTarget.GetComponent<MinionResume>();
		if (component2 != null && component2.AptitudeBySkillGroup.Count > 0)
		{
			this.bioDrawer.NewLabel(UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.NAME + "\n").Tooltip(string.Format(UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.TOOLTIP, this.selectedTarget.name));
			foreach (KeyValuePair<HashedString, float> keyValuePair in component2.AptitudeBySkillGroup)
			{
				if (keyValuePair.Value != 0f)
				{
					SkillGroup skillGroup = Db.Get().SkillGroups.TryGet(keyValuePair.Key);
					if (skillGroup != null)
					{
						this.bioDrawer.NewLabel("  • " + skillGroup.Name).Tooltip(string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, keyValuePair.Value));
					}
				}
			}
		}
		this.bioDrawer.EndDrawing();
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x0021BD3C File Offset: 0x00219F3C
	private void RefreshTraits()
	{
		if (!this.selectedTarget.GetComponent<MinionIdentity>())
		{
			this.traitsPanel.SetActive(false);
			return;
		}
		this.traitsPanel.SetActive(true);
		this.traitsPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = UI.DETAILTABS.STATS.GROUPNAME_TRAITS;
		this.traitsDrawer.BeginDrawing();
		foreach (Trait trait in this.selectedTarget.GetComponent<Traits>().TraitList)
		{
			if (!string.IsNullOrEmpty(trait.Name))
			{
				this.traitsDrawer.NewLabel(trait.Name).Tooltip(trait.GetTooltip());
			}
		}
		this.traitsDrawer.EndDrawing();
	}

	// Token: 0x04003E10 RID: 15888
	public GameObject attributesLabelTemplate;

	// Token: 0x04003E11 RID: 15889
	private GameObject bioPanel;

	// Token: 0x04003E12 RID: 15890
	private GameObject traitsPanel;

	// Token: 0x04003E13 RID: 15891
	private DetailsPanelDrawer bioDrawer;

	// Token: 0x04003E14 RID: 15892
	private DetailsPanelDrawer traitsDrawer;

	// Token: 0x04003E15 RID: 15893
	public MinionEquipmentPanel panel;

	// Token: 0x04003E16 RID: 15894
	private SchedulerHandle updateHandle;
}

using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
[AddComponentMenu("KMonoBehaviour/scripts/MinionEquipmentPanel")]
public class MinionEquipmentPanel : KMonoBehaviour
{
	// Token: 0x06005C12 RID: 23570 RVA: 0x0021B2D9 File Offset: 0x002194D9
	public MinionEquipmentPanel()
	{
		this.refreshDelegate = new Action<object>(this.Refresh);
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x0021B300 File Offset: 0x00219500
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.roomPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		this.roomPanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_ROOMS;
		this.roomPanel.SetActive(true);
		this.ownablePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		this.ownablePanel.GetComponent<CollapsibleDetailContentPanel>().HeaderLabel.text = UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_OWNABLE;
		this.ownablePanel.SetActive(true);
	}

	// Token: 0x06005C14 RID: 23572 RVA: 0x0021B3A4 File Offset: 0x002195A4
	public void SetSelectedMinion(GameObject minion)
	{
		if (this.SelectedMinion != null)
		{
			this.SelectedMinion.Unsubscribe(-448952673, this.refreshDelegate);
			this.SelectedMinion.Unsubscribe(-1285462312, this.refreshDelegate);
			this.SelectedMinion.Unsubscribe(-1585839766, this.refreshDelegate);
		}
		this.SelectedMinion = minion;
		this.SelectedMinion.Subscribe(-448952673, this.refreshDelegate);
		this.SelectedMinion.Subscribe(-1285462312, this.refreshDelegate);
		this.SelectedMinion.Subscribe(-1585839766, this.refreshDelegate);
		this.Refresh(null);
	}

	// Token: 0x06005C15 RID: 23573 RVA: 0x0021B454 File Offset: 0x00219654
	public void Refresh(object data = null)
	{
		if (this.SelectedMinion == null)
		{
			return;
		}
		this.Build();
	}

	// Token: 0x06005C16 RID: 23574 RVA: 0x0021B46C File Offset: 0x0021966C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.SelectedMinion != null)
		{
			this.SelectedMinion.Unsubscribe(-448952673, this.refreshDelegate);
			this.SelectedMinion.Unsubscribe(-1285462312, this.refreshDelegate);
			this.SelectedMinion.Unsubscribe(-1585839766, this.refreshDelegate);
		}
	}

	// Token: 0x06005C17 RID: 23575 RVA: 0x0021B4D0 File Offset: 0x002196D0
	private GameObject AddOrGetLabel(Dictionary<string, GameObject> labels, GameObject panel, string id)
	{
		GameObject gameObject;
		if (labels.ContainsKey(id))
		{
			gameObject = labels[id];
		}
		else
		{
			gameObject = Util.KInstantiate(this.labelTemplate, panel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject, null);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			labels[id] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005C18 RID: 23576 RVA: 0x0021B53E File Offset: 0x0021973E
	private void Build()
	{
		this.ShowAssignables(this.SelectedMinion.GetComponent<MinionIdentity>().GetSoleOwner(), this.roomPanel);
		this.ShowAssignables(this.SelectedMinion.GetComponent<MinionIdentity>().GetEquipment(), this.ownablePanel);
	}

	// Token: 0x06005C19 RID: 23577 RVA: 0x0021B578 File Offset: 0x00219778
	private void ShowAssignables(Assignables assignables, GameObject panel)
	{
		bool flag = false;
		foreach (AssignableSlotInstance assignableSlotInstance in assignables.Slots)
		{
			if (assignableSlotInstance.slot.showInUI)
			{
				GameObject gameObject = this.AddOrGetLabel(this.labels, panel, assignableSlotInstance.slot.Name);
				if (assignableSlotInstance.IsAssigned())
				{
					gameObject.SetActive(true);
					flag = true;
					string text = assignableSlotInstance.IsAssigned() ? assignableSlotInstance.assignable.GetComponent<KSelectable>().GetName() : UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES.text;
					gameObject.GetComponent<LocText>().text = string.Format("{0}: {1}", assignableSlotInstance.slot.Name, text);
					gameObject.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.ASSIGNED_TOOLTIP, text, this.GetAssignedEffectsString(assignableSlotInstance), this.SelectedMinion.name);
				}
				else
				{
					gameObject.SetActive(false);
					gameObject.GetComponent<LocText>().text = UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES;
					gameObject.GetComponent<ToolTip>().toolTip = UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES_TOOLTIP;
				}
			}
		}
		if (assignables is Ownables)
		{
			if (!flag)
			{
				GameObject gameObject2 = this.AddOrGetLabel(this.labels, panel, "NothingAssigned");
				this.labels["NothingAssigned"].SetActive(true);
				gameObject2.GetComponent<LocText>().text = UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES;
				gameObject2.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES_TOOLTIP, this.SelectedMinion.name);
			}
			else if (this.labels.ContainsKey("NothingAssigned"))
			{
				this.labels["NothingAssigned"].SetActive(false);
			}
		}
		if (assignables is Equipment)
		{
			if (!flag)
			{
				GameObject gameObject3 = this.AddOrGetLabel(this.labels, panel, "NoSuitAssigned");
				this.labels["NoSuitAssigned"].SetActive(true);
				gameObject3.GetComponent<LocText>().text = UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT;
				gameObject3.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT_TOOLTIP, this.SelectedMinion.name);
				return;
			}
			if (this.labels.ContainsKey("NoSuitAssigned"))
			{
				this.labels["NoSuitAssigned"].SetActive(false);
			}
		}
	}

	// Token: 0x06005C1A RID: 23578 RVA: 0x0021B7F0 File Offset: 0x002199F0
	private string GetAssignedEffectsString(AssignableSlotInstance slot)
	{
		string text = "";
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(GameUtil.GetGameObjectEffects(slot.assignable.gameObject, false));
		if (list.Count > 0)
		{
			text += "\n";
			foreach (Descriptor descriptor in list)
			{
				text = text + "  • " + descriptor.IndentedText() + "\n";
			}
		}
		return text;
	}

	// Token: 0x04003E09 RID: 15881
	public GameObject SelectedMinion;

	// Token: 0x04003E0A RID: 15882
	public GameObject labelTemplate;

	// Token: 0x04003E0B RID: 15883
	private GameObject roomPanel;

	// Token: 0x04003E0C RID: 15884
	private GameObject ownablePanel;

	// Token: 0x04003E0D RID: 15885
	private Storage storage;

	// Token: 0x04003E0E RID: 15886
	private Dictionary<string, GameObject> labels = new Dictionary<string, GameObject>();

	// Token: 0x04003E0F RID: 15887
	private Action<object> refreshDelegate;
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C00 RID: 3072
public class ButtonMenuSideScreen : SideScreenContent
{
	// Token: 0x06006136 RID: 24886 RVA: 0x0023E490 File Offset: 0x0023C690
	public override bool IsValidForTarget(GameObject target)
	{
		ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>();
		if (sidescreenButtonControl == null)
		{
			sidescreenButtonControl = target.GetSMI<ISidescreenButtonControl>();
		}
		return sidescreenButtonControl != null && sidescreenButtonControl.SidescreenEnabled();
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x0023E4B9 File Offset: 0x0023C6B9
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].ButtonSideScreenSortOrder();
	}

	// Token: 0x06006138 RID: 24888 RVA: 0x0023E4D7 File Offset: 0x0023C6D7
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = new_target.GetAllSMI<ISidescreenButtonControl>();
		this.targets.AddRange(new_target.GetComponents<ISidescreenButtonControl>());
		this.Refresh();
	}

	// Token: 0x06006139 RID: 24889 RVA: 0x0023E510 File Offset: 0x0023C710
	public GameObject GetHorizontalGroup(int id)
	{
		if (!this.horizontalGroups.ContainsKey(id))
		{
			this.horizontalGroups.Add(id, Util.KInstantiateUI(this.horizontalGroupPrefab, this.buttonContainer.gameObject, true));
		}
		return this.horizontalGroups[id];
	}

	// Token: 0x0600613A RID: 24890 RVA: 0x0023E550 File Offset: 0x0023C750
	public void CopyLayoutSettings(LayoutElement to, LayoutElement from)
	{
		to.ignoreLayout = from.ignoreLayout;
		to.minWidth = from.minWidth;
		to.minHeight = from.minHeight;
		to.preferredWidth = from.preferredWidth;
		to.preferredHeight = from.preferredHeight;
		to.flexibleWidth = from.flexibleWidth;
		to.flexibleHeight = from.flexibleHeight;
		to.layoutPriority = from.layoutPriority;
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x0023E5C0 File Offset: 0x0023C7C0
	private void Refresh()
	{
		while (this.liveButtons.Count < this.targets.Count)
		{
			this.liveButtons.Add(Util.KInstantiateUI(this.buttonPrefab.gameObject, this.buttonContainer.gameObject, true));
		}
		foreach (int key in this.horizontalGroups.Keys)
		{
			this.horizontalGroups[key].SetActive(false);
		}
		for (int i = 0; i < this.liveButtons.Count; i++)
		{
			if (i >= this.targets.Count)
			{
				this.liveButtons[i].SetActive(false);
			}
			else
			{
				if (!this.liveButtons[i].activeSelf)
				{
					this.liveButtons[i].SetActive(true);
				}
				int num = this.targets[i].HorizontalGroupID();
				LayoutElement component = this.liveButtons[i].GetComponent<LayoutElement>();
				KButton componentInChildren = this.liveButtons[i].GetComponentInChildren<KButton>();
				ToolTip componentInChildren2 = this.liveButtons[i].GetComponentInChildren<ToolTip>();
				LocText componentInChildren3 = this.liveButtons[i].GetComponentInChildren<LocText>();
				if (num >= 0)
				{
					GameObject horizontalGroup = this.GetHorizontalGroup(num);
					horizontalGroup.SetActive(true);
					this.liveButtons[i].transform.SetParent(horizontalGroup.transform, false);
					this.CopyLayoutSettings(component, this.horizontalButtonPrefab);
				}
				else
				{
					this.liveButtons[i].transform.SetParent(this.buttonContainer, false);
					this.CopyLayoutSettings(component, this.buttonPrefab);
				}
				componentInChildren.isInteractable = this.targets[i].SidescreenButtonInteractable();
				componentInChildren.ClearOnClick();
				componentInChildren.onClick += this.targets[i].OnSidescreenButtonPressed;
				componentInChildren.onClick += this.Refresh;
				componentInChildren3.SetText(this.targets[i].SidescreenButtonText);
				componentInChildren2.SetSimpleTooltip(this.targets[i].SidescreenButtonTooltip);
			}
		}
	}

	// Token: 0x04004239 RID: 16953
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	// Token: 0x0400423A RID: 16954
	public LayoutElement buttonPrefab;

	// Token: 0x0400423B RID: 16955
	public LayoutElement horizontalButtonPrefab;

	// Token: 0x0400423C RID: 16956
	public GameObject horizontalGroupPrefab;

	// Token: 0x0400423D RID: 16957
	public RectTransform buttonContainer;

	// Token: 0x0400423E RID: 16958
	private List<GameObject> liveButtons = new List<GameObject>();

	// Token: 0x0400423F RID: 16959
	private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();

	// Token: 0x04004240 RID: 16960
	private List<ISidescreenButtonControl> targets;
}

using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C5B RID: 3163
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x0600647D RID: 25725 RVA: 0x00251EE1 File Offset: 0x002500E1
	// (set) Token: 0x0600647E RID: 25726 RVA: 0x00251EE9 File Offset: 0x002500E9
	public bool ArrowExpanded { get; private set; }

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x0600647F RID: 25727 RVA: 0x00251EF2 File Offset: 0x002500F2
	// (set) Token: 0x06006480 RID: 25728 RVA: 0x00251EFA File Offset: 0x002500FA
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x00251F04 File Offset: 0x00250104
	public TreeFilterableSideScreenRow.State GetState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			if (this.parent.GetElementTagAcceptedState(treeFilterableSideScreenElement.GetElementTag()))
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (this.rowElements.Count <= 0)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		return TreeFilterableSideScreenRow.State.On;
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x00251F98 File Offset: 0x00250198
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.checkBoxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (this.parent.CurrentSearchValue == "")
			{
				TreeFilterableSideScreenRow.State state = this.GetState();
				if (state > TreeFilterableSideScreenRow.State.Mixed)
				{
					if (state == TreeFilterableSideScreenRow.State.On)
					{
						this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
						return;
					}
				}
				else
				{
					this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
				}
			}
		}));
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x00251FC7 File Offset: 0x002501C7
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SetArrowToggleState(this.GetState() > TreeFilterableSideScreenRow.State.Off);
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x00251FDE File Offset: 0x002501DE
	protected override void OnCmpDisable()
	{
		this.SetArrowToggleState(false);
		this.rowElements.ForEach(delegate(TreeFilterableSideScreenElement row)
		{
			row.OnSelectionChanged -= this.OnElementSelectionChanged;
		});
		base.OnCmpDisable();
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x00252004 File Offset: 0x00250204
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006486 RID: 25734 RVA: 0x0025200C File Offset: 0x0025020C
	public void UpdateCheckBoxVisualState()
	{
		this.checkBoxToggle.ChangeState((int)this.GetState());
		this.visualDirty = false;
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x00252028 File Offset: 0x00250228
	public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			for (int i = 0; i < this.rowElements.Count; i++)
			{
				this.rowElements[i].SetCheckBox(false);
			}
			break;
		case TreeFilterableSideScreenRow.State.On:
			for (int j = 0; j < this.rowElements.Count; j++)
			{
				this.rowElements[j].SetCheckBox(true);
			}
			break;
		}
		this.visualDirty = true;
	}

	// Token: 0x06006488 RID: 25736 RVA: 0x002520A2 File Offset: 0x002502A2
	private void ArrowToggleClicked()
	{
		this.SetArrowToggleState(!this.ArrowExpanded);
		this.RefreshArrowToggleState();
	}

	// Token: 0x06006489 RID: 25737 RVA: 0x002520B9 File Offset: 0x002502B9
	public void SetArrowToggleState(bool state)
	{
		this.ArrowExpanded = state;
		this.RefreshArrowToggleState();
	}

	// Token: 0x0600648A RID: 25738 RVA: 0x002520C8 File Offset: 0x002502C8
	private void RefreshArrowToggleState()
	{
		this.arrowToggle.ChangeState(this.ArrowExpanded ? 1 : 0);
		this.elementGroup.SetActive(this.ArrowExpanded);
		this.bgImg.enabled = this.ArrowExpanded;
	}

	// Token: 0x0600648B RID: 25739 RVA: 0x00252103 File Offset: 0x00250303
	private void ArrowToggleDisabledClick()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x0600648C RID: 25740 RVA: 0x00252115 File Offset: 0x00250315
	public void ShowToggleBox(bool show)
	{
		this.checkBoxToggle.gameObject.SetActive(show);
	}

	// Token: 0x0600648D RID: 25741 RVA: 0x00252128 File Offset: 0x00250328
	private void OnElementSelectionChanged(Tag t, bool state)
	{
		if (state)
		{
			this.parent.AddTag(t);
		}
		else
		{
			this.parent.RemoveTag(t);
		}
		this.visualDirty = true;
	}

	// Token: 0x0600648E RID: 25742 RVA: 0x00252150 File Offset: 0x00250350
	public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
	{
		this.subTags.Clear();
		this.rowElements.Clear();
		this.elementName.text = mainElementTag.ProperName();
		this.bgImg.enabled = false;
		string simpleTooltip = string.Format(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, mainElementTag.ProperName());
		this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		if (filterMap.Count == 0)
		{
			if (this.elementGroup.activeInHierarchy)
			{
				this.elementGroup.SetActive(false);
			}
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleDisabledClick);
			this.arrowToggle.ChangeState(0);
		}
		else
		{
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleClicked);
			this.arrowToggle.ChangeState(0);
			foreach (KeyValuePair<Tag, bool> keyValuePair in filterMap)
			{
				TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
				freeElement.Parent = this.parent;
				freeElement.SetTag(keyValuePair.Key);
				freeElement.SetCheckBox(keyValuePair.Value);
				freeElement.OnSelectionChanged += this.OnElementSelectionChanged;
				freeElement.SetCheckBox(this.parent.IsTagAllowed(keyValuePair.Key));
				this.rowElements.Add(freeElement);
				this.subTags.Add(keyValuePair.Key);
			}
		}
		this.UpdateCheckBoxVisualState();
	}

	// Token: 0x0600648F RID: 25743 RVA: 0x002522F0 File Offset: 0x002504F0
	public void FilterAgainstSearch(Tag thisCategoryTag, string search)
	{
		bool flag = false;
		bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		search = search.ToUpper();
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			bool flag3 = flag2 || treeFilterableSideScreenElement.GetElementTag().ProperNameStripLink().ToUpper().Contains(search.ToUpper());
			treeFilterableSideScreenElement.gameObject.SetActive(flag3);
			flag = (flag || flag3);
		}
		base.gameObject.SetActive(flag);
		if (search != "" && flag && this.arrowToggle.CurrentState == 0)
		{
			this.SetArrowToggleState(true);
		}
	}

	// Token: 0x04004499 RID: 17561
	public bool visualDirty;

	// Token: 0x0400449A RID: 17562
	[SerializeField]
	private LocText elementName;

	// Token: 0x0400449B RID: 17563
	[SerializeField]
	private GameObject elementGroup;

	// Token: 0x0400449C RID: 17564
	[SerializeField]
	private MultiToggle checkBoxToggle;

	// Token: 0x0400449D RID: 17565
	[SerializeField]
	private MultiToggle arrowToggle;

	// Token: 0x0400449E RID: 17566
	[SerializeField]
	private KImage bgImg;

	// Token: 0x0400449F RID: 17567
	private List<Tag> subTags = new List<Tag>();

	// Token: 0x040044A0 RID: 17568
	private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();

	// Token: 0x040044A1 RID: 17569
	private TreeFilterableSideScreen parent;

	// Token: 0x02001B90 RID: 7056
	public enum State
	{
		// Token: 0x04007D17 RID: 32023
		Off,
		// Token: 0x04007D18 RID: 32024
		Mixed,
		// Token: 0x04007D19 RID: 32025
		On
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C34 RID: 3124
public class NToggleSideScreen : SideScreenContent
{
	// Token: 0x060062D4 RID: 25300 RVA: 0x00247B5E File Offset: 0x00245D5E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x00247B66 File Offset: 0x00245D66
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<INToggleSideScreenControl>() != null;
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x00247B74 File Offset: 0x00245D74
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<INToggleSideScreenControl>();
		if (this.target == null)
		{
			return;
		}
		this.titleKey = this.target.SidescreenTitleKey;
		base.gameObject.SetActive(true);
		this.Refresh();
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x00247BC0 File Offset: 0x00245DC0
	private void Refresh()
	{
		for (int i = 0; i < Mathf.Max(this.target.Options.Count, this.buttonList.Count); i++)
		{
			if (i >= this.target.Options.Count)
			{
				this.buttonList[i].gameObject.SetActive(false);
			}
			else
			{
				if (i >= this.buttonList.Count)
				{
					KToggle ktoggle = Util.KInstantiateUI<KToggle>(this.buttonPrefab.gameObject, this.ContentContainer, false);
					int idx = i;
					ktoggle.onClick += delegate()
					{
						this.target.QueueSelectedOption(idx);
						this.Refresh();
					};
					this.buttonList.Add(ktoggle);
				}
				this.buttonList[i].GetComponentInChildren<LocText>().text = this.target.Options[i];
				this.buttonList[i].GetComponentInChildren<ToolTip>().toolTip = this.target.Tooltips[i];
				if (this.target.SelectedOption == i && this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				else if (this.target.QueuedOption == i)
				{
					this.buttonList[i].isOn = true;
					ImageToggleState[] componentsInChildren = this.buttonList[i].GetComponentsInChildren<ImageToggleState>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].SetActive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = true;
				}
				else
				{
					this.buttonList[i].isOn = false;
					foreach (ImageToggleState imageToggleState in this.buttonList[i].GetComponentsInChildren<ImageToggleState>())
					{
						imageToggleState.SetInactive();
						imageToggleState.SetInactive();
					}
					this.buttonList[i].GetComponent<ImageToggleStateThrobber>().enabled = false;
				}
				this.buttonList[i].gameObject.SetActive(true);
			}
		}
		this.description.text = this.target.Description;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(this.target.Description));
	}

	// Token: 0x0400435B RID: 17243
	[SerializeField]
	private KToggle buttonPrefab;

	// Token: 0x0400435C RID: 17244
	[SerializeField]
	private LocText description;

	// Token: 0x0400435D RID: 17245
	private INToggleSideScreenControl target;

	// Token: 0x0400435E RID: 17246
	private List<KToggle> buttonList = new List<KToggle>();
}

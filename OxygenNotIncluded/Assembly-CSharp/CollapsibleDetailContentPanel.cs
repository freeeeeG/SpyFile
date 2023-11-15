using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEF RID: 2799
[AddComponentMenu("KMonoBehaviour/scripts/CollapsibleDetailContentPanel")]
public class CollapsibleDetailContentPanel : KMonoBehaviour
{
	// Token: 0x06005645 RID: 22085 RVA: 0x001F6B14 File Offset: 0x001F4D14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.collapseButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ToggleOpen));
		this.ArrowIcon.SetActive();
		this.log = new LoggerFSS("detailpanel", 35);
		this.labels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>>();
		this.buttonLabels = new Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>>();
		this.Commit();
	}

	// Token: 0x06005646 RID: 22086 RVA: 0x001F6B87 File Offset: 0x001F4D87
	public void SetTitle(string title)
	{
		this.HeaderLabel.text = title;
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x001F6B98 File Offset: 0x001F4D98
	public void Commit()
	{
		int num = 0;
		foreach (CollapsibleDetailContentPanel.Label<DetailLabel> label in this.labels.Values)
		{
			if (label.used)
			{
				num++;
				if (!label.obj.gameObject.activeSelf)
				{
					label.obj.gameObject.SetActive(true);
				}
			}
			else if (!label.used && label.obj.gameObject.activeSelf)
			{
				label.obj.gameObject.SetActive(false);
			}
			label.used = false;
		}
		foreach (CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label2 in this.buttonLabels.Values)
		{
			if (label2.used)
			{
				num++;
				if (!label2.obj.gameObject.activeSelf)
				{
					label2.obj.gameObject.SetActive(true);
				}
			}
			else if (!label2.used && label2.obj.gameObject.activeSelf)
			{
				label2.obj.gameObject.SetActive(false);
			}
			label2.used = false;
		}
		if (base.gameObject.activeSelf && num == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!base.gameObject.activeSelf && num > 0)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06005648 RID: 22088 RVA: 0x001F6D34 File Offset: 0x001F4F34
	public void SetLabel(string id, string text, string tooltip)
	{
		CollapsibleDetailContentPanel.Label<DetailLabel> label;
		if (!this.labels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabel>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabel>()
			};
			label.obj.gameObject.name = id;
			this.labels[id] = label;
		}
		label.obj.label.AllowLinks = true;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.used = true;
	}

	// Token: 0x06005649 RID: 22089 RVA: 0x001F6DE0 File Offset: 0x001F4FE0
	public void SetLabelWithButton(string id, string text, string tooltip, string buttonText, string buttonTooltip, System.Action buttonCb)
	{
		CollapsibleDetailContentPanel.Label<DetailLabelWithButton> label;
		if (!this.buttonLabels.TryGetValue(id, out label))
		{
			label = new CollapsibleDetailContentPanel.Label<DetailLabelWithButton>
			{
				used = true,
				obj = Util.KInstantiateUI(this.labelWithActionButtonTemplate.gameObject, this.Content.gameObject, false).GetComponent<DetailLabelWithButton>()
			};
			label.obj.gameObject.name = id;
			this.buttonLabels[id] = label;
		}
		label.obj.label.AllowLinks = true;
		label.obj.label.text = text;
		label.obj.toolTip.toolTip = tooltip;
		label.obj.buttonLabel.text = buttonText;
		label.obj.buttonToolTip.toolTip = buttonTooltip;
		label.obj.button.ClearOnClick();
		label.obj.button.onClick += buttonCb;
		label.used = true;
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x001F6ED0 File Offset: 0x001F50D0
	private void ToggleOpen()
	{
		bool flag = this.scalerMask.gameObject.activeSelf;
		flag = !flag;
		this.scalerMask.gameObject.SetActive(flag);
		if (flag)
		{
			this.ArrowIcon.SetActive();
			this.ForceLocTextsMeshRebuild();
			return;
		}
		this.ArrowIcon.SetInactive();
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x001F6F24 File Offset: 0x001F5124
	public void ForceLocTextsMeshRebuild()
	{
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ForceMeshUpdate();
		}
	}

	// Token: 0x04003A02 RID: 14850
	public ImageToggleState ArrowIcon;

	// Token: 0x04003A03 RID: 14851
	public LocText HeaderLabel;

	// Token: 0x04003A04 RID: 14852
	public MultiToggle collapseButton;

	// Token: 0x04003A05 RID: 14853
	public Transform Content;

	// Token: 0x04003A06 RID: 14854
	public ScalerMask scalerMask;

	// Token: 0x04003A07 RID: 14855
	[Space(10f)]
	public DetailLabel labelTemplate;

	// Token: 0x04003A08 RID: 14856
	public DetailLabelWithButton labelWithActionButtonTemplate;

	// Token: 0x04003A09 RID: 14857
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabel>> labels;

	// Token: 0x04003A0A RID: 14858
	private Dictionary<string, CollapsibleDetailContentPanel.Label<DetailLabelWithButton>> buttonLabels;

	// Token: 0x04003A0B RID: 14859
	private LoggerFSS log;

	// Token: 0x02001A02 RID: 6658
	private class Label<T>
	{
		// Token: 0x04007801 RID: 30721
		public T obj;

		// Token: 0x04007802 RID: 30722
		public bool used;
	}
}

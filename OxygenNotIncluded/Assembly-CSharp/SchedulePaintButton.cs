using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BE4 RID: 3044
[AddComponentMenu("KMonoBehaviour/scripts/SchedulePaintButton")]
public class SchedulePaintButton : KMonoBehaviour
{
	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x06006055 RID: 24661 RVA: 0x00239B87 File Offset: 0x00237D87
	// (set) Token: 0x06006056 RID: 24662 RVA: 0x00239B8F File Offset: 0x00237D8F
	public ScheduleGroup group { get; private set; }

	// Token: 0x06006057 RID: 24663 RVA: 0x00239B98 File Offset: 0x00237D98
	public void SetGroup(ScheduleGroup group, Dictionary<string, ColorStyleSetting> styles, Action<SchedulePaintButton> onClick)
	{
		this.group = group;
		if (styles.ContainsKey(group.Id))
		{
			this.toggleState.SetColorStyle(styles[group.Id]);
		}
		this.label.text = group.Name;
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			onClick(this);
		}));
		this.toolTip.SetSimpleTooltip(group.GetTooltip());
		base.gameObject.name = "PaintButton_" + group.Id;
	}

	// Token: 0x06006058 RID: 24664 RVA: 0x00239C49 File Offset: 0x00237E49
	public void SetToggle(bool on)
	{
		this.toggle.ChangeState(on ? 1 : 0);
	}

	// Token: 0x04004193 RID: 16787
	[SerializeField]
	private LocText label;

	// Token: 0x04004194 RID: 16788
	[SerializeField]
	private ImageToggleState toggleState;

	// Token: 0x04004195 RID: 16789
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04004196 RID: 16790
	[SerializeField]
	private ToolTip toolTip;
}

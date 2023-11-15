using System;
using UnityEngine;

// Token: 0x02000BC6 RID: 3014
[AddComponentMenu("KMonoBehaviour/scripts/PriorityButton")]
public class PriorityButton : KMonoBehaviour
{
	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06005E93 RID: 24211 RVA: 0x0022B730 File Offset: 0x00229930
	// (set) Token: 0x06005E94 RID: 24212 RVA: 0x0022B738 File Offset: 0x00229938
	public PrioritySetting priority
	{
		get
		{
			return this._priority;
		}
		set
		{
			this._priority = value;
			if (this.its != null)
			{
				if (this.priority.priority_class == PriorityScreen.PriorityClass.high)
				{
					this.its.colorStyleSetting = this.highStyle;
				}
				else
				{
					this.its.colorStyleSetting = this.normalStyle;
				}
				this.its.RefreshColorStyle();
				this.its.ResetColor();
			}
		}
	}

	// Token: 0x06005E95 RID: 24213 RVA: 0x0022B7A2 File Offset: 0x002299A2
	protected override void OnPrefabInit()
	{
		this.toggle.onClick += this.OnClick;
	}

	// Token: 0x06005E96 RID: 24214 RVA: 0x0022B7BB File Offset: 0x002299BB
	private void OnClick()
	{
		if (this.playSelectionSound)
		{
			PriorityScreen.PlayPriorityConfirmSound(this.priority);
		}
		if (this.onClick != null)
		{
			this.onClick(this.priority);
		}
	}

	// Token: 0x04003FD3 RID: 16339
	public KToggle toggle;

	// Token: 0x04003FD4 RID: 16340
	public LocText text;

	// Token: 0x04003FD5 RID: 16341
	public ToolTip tooltip;

	// Token: 0x04003FD6 RID: 16342
	[MyCmpGet]
	private ImageToggleState its;

	// Token: 0x04003FD7 RID: 16343
	public ColorStyleSetting normalStyle;

	// Token: 0x04003FD8 RID: 16344
	public ColorStyleSetting highStyle;

	// Token: 0x04003FD9 RID: 16345
	public bool playSelectionSound = true;

	// Token: 0x04003FDA RID: 16346
	public Action<PrioritySetting> onClick;

	// Token: 0x04003FDB RID: 16347
	private PrioritySetting _priority;
}

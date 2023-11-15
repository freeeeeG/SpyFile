using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BEC RID: 3052
public class SelectableTextStyler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x06006086 RID: 24710 RVA: 0x0023AC79 File Offset: 0x00238E79
	private void Start()
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x0023AC88 File Offset: 0x00238E88
	private void SetState(SelectableTextStyler.State state, SelectableTextStyler.HoverState hover_state)
	{
		if (state == SelectableTextStyler.State.Normal)
		{
			if (hover_state != SelectableTextStyler.HoverState.Normal)
			{
				if (hover_state == SelectableTextStyler.HoverState.Hovered)
				{
					this.target.textStyleSetting = this.normalHovered;
				}
			}
			else
			{
				this.target.textStyleSetting = this.normalNormal;
			}
		}
		this.target.ApplySettings();
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x0023ACC5 File Offset: 0x00238EC5
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Hovered);
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x0023ACD4 File Offset: 0x00238ED4
	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x0023ACE3 File Offset: 0x00238EE3
	public void OnPointerClick(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x040041BF RID: 16831
	[SerializeField]
	private LocText target;

	// Token: 0x040041C0 RID: 16832
	[SerializeField]
	private SelectableTextStyler.State state;

	// Token: 0x040041C1 RID: 16833
	[SerializeField]
	private TextStyleSetting normalNormal;

	// Token: 0x040041C2 RID: 16834
	[SerializeField]
	private TextStyleSetting normalHovered;

	// Token: 0x02001B47 RID: 6983
	public enum State
	{
		// Token: 0x04007C58 RID: 31832
		Normal
	}

	// Token: 0x02001B48 RID: 6984
	public enum HoverState
	{
		// Token: 0x04007C5A RID: 31834
		Normal,
		// Token: 0x04007C5B RID: 31835
		Hovered
	}
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public class UI_CursorToolTip : AUISituational
{
	// Token: 0x06000909 RID: 2313 RVA: 0x000222AC File Offset: 0x000204AC
	private void OnEnable()
	{
		EventMgr.Register<bool>(eGameEvents.UI_ToggleMouseTooltip, new Action<bool>(this.OnToggleMouseTooltip));
		EventMgr.Register<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, new Action<UI_CursorToolTip.eTargetType, Transform, Vector3>(this.OnSetMouseTooltipTarget));
		EventMgr.Register<string, string>(eGameEvents.UI_SetMouseTooltipContent, new Action<string, string>(this.OnSetMouseTooltipContent));
		this.currentFormatType = UI_CursorToolTip.eFormatType.NONE;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00022314 File Offset: 0x00020514
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eGameEvents.UI_ToggleMouseTooltip, new Action<bool>(this.OnToggleMouseTooltip));
		EventMgr.Remove<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, new Action<UI_CursorToolTip.eTargetType, Transform, Vector3>(this.OnSetMouseTooltipTarget));
		EventMgr.Remove<string, string>(eGameEvents.UI_SetMouseTooltipContent, new Action<string, string>(this.OnSetMouseTooltipContent));
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00022372 File Offset: 0x00020572
	private void OnToggleMouseTooltip(bool isOn)
	{
		if (this.isUIOn == isOn)
		{
			return;
		}
		base.Toggle(isOn);
		this.isUIOn = isOn;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0002238C File Offset: 0x0002058C
	private void OnSetMouseTooltipTarget(UI_CursorToolTip.eTargetType targetType, Transform target, Vector3 targetOffset)
	{
		this.curTargetType = targetType;
		this.curTrackingTarget = target;
		this.curTargetOffset = target.TransformVector(targetOffset);
		this.UpdatePosition();
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x000223AF File Offset: 0x000205AF
	private void OnSetMouseTooltipContent(string title, string content)
	{
		this.msg_Title = title;
		this.msg_Content = content;
		if (this.currentFormat != null)
		{
			this.SetTitle(this.msg_Title);
			this.SetContent(this.msg_Content);
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x000223DF File Offset: 0x000205DF
	private void Update()
	{
		if (!this.isUIOn)
		{
			return;
		}
		this.UpdatePosition();
		this.UpdateFormat();
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x000223F8 File Offset: 0x000205F8
	private void UpdateFormat()
	{
		Vector3 vector = Singleton<CameraManager>.Instance.CalculateViewportPos(this.format_ArrowDown.text_Title.transform.position);
		if (vector.y < 0.9f)
		{
			if (this.currentFormatType != UI_CursorToolTip.eFormatType.ARROW_DOWN)
			{
				this.SwitchFormat(UI_CursorToolTip.eFormatType.ARROW_DOWN);
				return;
			}
		}
		else if (vector.x < 0.75f)
		{
			if (this.currentFormatType != UI_CursorToolTip.eFormatType.ARROW_LEFT)
			{
				this.SwitchFormat(UI_CursorToolTip.eFormatType.ARROW_LEFT);
				return;
			}
		}
		else if (this.currentFormatType != UI_CursorToolTip.eFormatType.ARROW_RIGHT)
		{
			this.SwitchFormat(UI_CursorToolTip.eFormatType.ARROW_RIGHT);
		}
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00022474 File Offset: 0x00020674
	private void SwitchFormat(UI_CursorToolTip.eFormatType layout)
	{
		if (this.currentFormatType == layout)
		{
			return;
		}
		this.currentFormatType = layout;
		switch (layout)
		{
		case UI_CursorToolTip.eFormatType.ARROW_DOWN:
			this.currentFormat = this.format_ArrowDown;
			this.SetTitle(this.msg_Title);
			this.SetContent(this.msg_Content);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.format_ArrowLeft.verticalLayoutGroup.transform as RectTransform);
			this.format_ArrowDown.node_Format.SetActive(true);
			this.format_ArrowLeft.node_Format.SetActive(false);
			this.format_ArrowRight.node_Format.SetActive(false);
			return;
		case UI_CursorToolTip.eFormatType.ARROW_LEFT:
			this.currentFormat = this.format_ArrowLeft;
			this.SetTitle(this.msg_Title);
			this.SetContent(this.msg_Content);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.format_ArrowDown.verticalLayoutGroup.transform as RectTransform);
			this.format_ArrowLeft.node_Format.SetActive(true);
			this.format_ArrowDown.node_Format.SetActive(false);
			this.format_ArrowRight.node_Format.SetActive(false);
			return;
		case UI_CursorToolTip.eFormatType.ARROW_RIGHT:
			this.currentFormat = this.format_ArrowRight;
			this.SetTitle(this.msg_Title);
			this.SetContent(this.msg_Content);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.format_ArrowDown.verticalLayoutGroup.transform as RectTransform);
			this.format_ArrowLeft.node_Format.SetActive(false);
			this.format_ArrowRight.node_Format.SetActive(true);
			this.format_ArrowDown.node_Format.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x000225FC File Offset: 0x000207FC
	private void UpdatePosition()
	{
		if (this.curTrackingTarget != null)
		{
			if (this.curTargetType == UI_CursorToolTip.eTargetType._2D)
			{
				base.transform.position = this.curTrackingTarget.position + this.curTargetOffset;
				base.transform.localPosition += this.uiOffset;
				return;
			}
			Vector3 position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.curTrackingTarget.position + this.curTargetOffset);
			base.transform.position = position;
			base.transform.localPosition += this.uiOffset;
		}
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x000226AC File Offset: 0x000208AC
	public void SetTitle(string msg)
	{
		if (msg == "")
		{
			this.currentFormat.text_Title.gameObject.SetActive(false);
			this.currentFormat.image_Deco.gameObject.SetActive(false);
			return;
		}
		this.currentFormat.text_Title.gameObject.SetActive(true);
		this.currentFormat.image_Deco.gameObject.SetActive(true);
		this.currentFormat.text_Title.text = msg;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00022730 File Offset: 0x00020930
	public void SetContent(string msg)
	{
		this.currentFormat.text_Content.text = msg;
	}

	// Token: 0x04000723 RID: 1827
	[SerializeField]
	private VerticalLayoutGroup verticalLayoutGroup;

	// Token: 0x04000724 RID: 1828
	[SerializeField]
	private Vector3 uiOffset;

	// Token: 0x04000725 RID: 1829
	[SerializeField]
	private UI_CursorToolTip.FormatContent format_ArrowDown;

	// Token: 0x04000726 RID: 1830
	[SerializeField]
	private UI_CursorToolTip.FormatContent format_ArrowLeft;

	// Token: 0x04000727 RID: 1831
	[SerializeField]
	private UI_CursorToolTip.FormatContent format_ArrowRight;

	// Token: 0x04000728 RID: 1832
	[SerializeField]
	private float screenBorderThreshold = 100f;

	// Token: 0x04000729 RID: 1833
	private bool isUIOn;

	// Token: 0x0400072A RID: 1834
	private Transform curTrackingTarget;

	// Token: 0x0400072B RID: 1835
	private Vector3 curTargetOffset;

	// Token: 0x0400072C RID: 1836
	private UI_CursorToolTip.eTargetType curTargetType;

	// Token: 0x0400072D RID: 1837
	private UI_CursorToolTip.FormatContent currentFormat;

	// Token: 0x0400072E RID: 1838
	private UI_CursorToolTip.eFormatType currentFormatType;

	// Token: 0x0400072F RID: 1839
	private string msg_Title = "";

	// Token: 0x04000730 RID: 1840
	private string msg_Content = "";

	// Token: 0x0200028A RID: 650
	[Serializable]
	public class FormatContent
	{
		// Token: 0x04000C0C RID: 3084
		public GameObject node_Format;

		// Token: 0x04000C0D RID: 3085
		public GameObject node_Title;

		// Token: 0x04000C0E RID: 3086
		public Image image_Deco;

		// Token: 0x04000C0F RID: 3087
		public TMP_Text text_Title;

		// Token: 0x04000C10 RID: 3088
		public TMP_Text text_Content;

		// Token: 0x04000C11 RID: 3089
		public VerticalLayoutGroup verticalLayoutGroup;
	}

	// Token: 0x0200028B RID: 651
	public enum eFormatType
	{
		// Token: 0x04000C13 RID: 3091
		NONE,
		// Token: 0x04000C14 RID: 3092
		ARROW_DOWN,
		// Token: 0x04000C15 RID: 3093
		ARROW_LEFT,
		// Token: 0x04000C16 RID: 3094
		ARROW_RIGHT
	}

	// Token: 0x0200028C RID: 652
	public enum eTargetType
	{
		// Token: 0x04000C18 RID: 3096
		_2D,
		// Token: 0x04000C19 RID: 3097
		_3D
	}
}

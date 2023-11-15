using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000B8C RID: 2956
public class TutorialMessageDialog : MessageDialog
{
	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06005BBD RID: 23485 RVA: 0x00219ABE File Offset: 0x00217CBE
	public override bool CanDontShowAgain
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x00219AC1 File Offset: 0x00217CC1
	public override bool CanDisplay(Message message)
	{
		return typeof(TutorialMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x00219AD8 File Offset: 0x00217CD8
	public override void SetMessage(Message base_message)
	{
		this.message = (base_message as TutorialMessage);
		this.description.text = this.message.GetMessageBody();
		if (!string.IsNullOrEmpty(this.message.videoClipId))
		{
			VideoClip video = Assets.GetVideo(this.message.videoClipId);
			this.SetVideo(video, this.message.videoOverlayName, this.message.videoTitleText);
		}
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x00219B48 File Offset: 0x00217D48
	public void SetVideo(VideoClip clip, string overlayName, string titleText)
	{
		if (this.videoWidget == null)
		{
			this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, base.transform.gameObject, true).GetComponent<VideoWidget>();
			this.videoWidget.transform.SetAsFirstSibling();
		}
		this.videoWidget.SetClip(clip, overlayName, new List<string>
		{
			titleText,
			VIDEOS.TUTORIAL_HEADER
		});
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x00219BBE File Offset: 0x00217DBE
	public override void OnClickAction()
	{
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x00219BC0 File Offset: 0x00217DC0
	public override void OnDontShowAgain()
	{
		Tutorial.Instance.HideTutorialMessage(this.message.messageId);
	}

	// Token: 0x04003DCC RID: 15820
	[SerializeField]
	private LocText description;

	// Token: 0x04003DCD RID: 15821
	private TutorialMessage message;

	// Token: 0x04003DCE RID: 15822
	[SerializeField]
	private GameObject videoWidgetPrefab;

	// Token: 0x04003DCF RID: 15823
	private VideoWidget videoWidget;
}

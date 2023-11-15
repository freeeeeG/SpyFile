using System;
using KSerialization;

// Token: 0x02000B8B RID: 2955
public class TutorialMessage : GenericMessage
{
	// Token: 0x06005BBB RID: 23483 RVA: 0x00219A52 File Offset: 0x00217C52
	public TutorialMessage()
	{
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x00219A68 File Offset: 0x00217C68
	public TutorialMessage(Tutorial.TutorialMessages messageId, string title, string body, string tooltip, string videoClipId = null, string videoOverlayName = null, string videoTitleText = null, string icon = "", string[] overrideDLCIDs = null) : base(title, body, tooltip, null)
	{
		this.messageId = messageId;
		this.videoClipId = videoClipId;
		this.videoOverlayName = videoOverlayName;
		this.videoTitleText = videoTitleText;
		this.icon = icon;
		if (overrideDLCIDs != null)
		{
			this.DLCIDs = overrideDLCIDs;
		}
	}

	// Token: 0x04003DC6 RID: 15814
	[Serialize]
	public Tutorial.TutorialMessages messageId;

	// Token: 0x04003DC7 RID: 15815
	public string videoClipId;

	// Token: 0x04003DC8 RID: 15816
	public string videoOverlayName;

	// Token: 0x04003DC9 RID: 15817
	public string videoTitleText;

	// Token: 0x04003DCA RID: 15818
	public string icon;

	// Token: 0x04003DCB RID: 15819
	public string[] DLCIDs = DlcManager.AVAILABLE_ALL_VERSIONS;
}

using System;
using UnityEngine;

// Token: 0x02000C89 RID: 3209
public class URLOpenFunction : MonoBehaviour
{
	// Token: 0x0600664A RID: 26186 RVA: 0x00262866 File Offset: 0x00260A66
	private void Start()
	{
		if (this.triggerButton != null)
		{
			this.triggerButton.ClearOnClick();
			this.triggerButton.onClick += delegate()
			{
				this.OpenUrl(this.fixedURL);
			};
		}
	}

	// Token: 0x0600664B RID: 26187 RVA: 0x00262898 File Offset: 0x00260A98
	public void OpenUrl(string url)
	{
		App.OpenWebURL(url);
	}

	// Token: 0x04004670 RID: 18032
	[SerializeField]
	private KButton triggerButton;

	// Token: 0x04004671 RID: 18033
	[SerializeField]
	private string fixedURL;
}

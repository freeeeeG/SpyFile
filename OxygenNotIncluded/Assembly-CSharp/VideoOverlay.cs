using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C8D RID: 3213
[AddComponentMenu("KMonoBehaviour/scripts/VideoOverlay")]
public class VideoOverlay : KMonoBehaviour
{
	// Token: 0x0600665E RID: 26206 RVA: 0x00262E3C File Offset: 0x0026103C
	public void SetText(List<string> strings)
	{
		if (strings.Count != this.textFields.Count)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				base.name,
				"expects",
				this.textFields.Count,
				"strings passed to it, got",
				strings.Count
			});
		}
		for (int i = 0; i < this.textFields.Count; i++)
		{
			this.textFields[i].text = strings[i];
		}
	}

	// Token: 0x04004683 RID: 18051
	public List<LocText> textFields;
}

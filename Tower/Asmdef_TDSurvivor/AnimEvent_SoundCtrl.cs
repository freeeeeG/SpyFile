using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class AnimEvent_SoundCtrl : MonoBehaviour
{
	// Token: 0x06000AE9 RID: 2793 RVA: 0x00029050 File Offset: 0x00027250
	public void Anim_PlaySound(string fullString)
	{
		if (Application.isPlaying)
		{
			int num = fullString.IndexOf(':');
			string assetKeyName = fullString.Substring(0, num);
			string sndName = fullString.Substring(num + 1, fullString.Length - num - 1);
			SoundManager.PlaySound(assetKeyName, sndName, -1f, -1f, -1f);
		}
	}

	// Token: 0x04000853 RID: 2131
	[TextArea(5, 10)]
	public string note;
}

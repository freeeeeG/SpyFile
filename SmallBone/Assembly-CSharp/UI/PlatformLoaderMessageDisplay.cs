using System;
using Platforms;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x020003C3 RID: 963
	public class PlatformLoaderMessageDisplay : MonoBehaviour
	{
		// Token: 0x060011EA RID: 4586 RVA: 0x00034E13 File Offset: 0x00033013
		private void Update()
		{
			if (this._platformLoader == null)
			{
				UnityEngine.Object.Destroy(this);
				return;
			}
			this._text.text = this._platformLoader.message;
		}

		// Token: 0x04000ED2 RID: 3794
		[SerializeField]
		private PlatformLoader _platformLoader;

		// Token: 0x04000ED3 RID: 3795
		[SerializeField]
		private TMP_Text _text;
	}
}

using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000235 RID: 565
	[RequireComponent(typeof(TMP_Text))]
	public class TextLocalizerUI : MonoBehaviour
	{
		// Token: 0x06000C69 RID: 3177 RVA: 0x0002D81C File Offset: 0x0002BA1C
		private void OnLanguageChanged(object sender, object args)
		{
			this.tmp.text = LocalizationSystem.GetLocalizedValue(this.localizedString.key);
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0002D839 File Offset: 0x0002BA39
		private void Start()
		{
			this.tmp = base.GetComponent<TMP_Text>();
			this.tmp.text = LocalizationSystem.GetLocalizedValue(this.localizedString.key);
			this.AddObserver(new Action<object, object>(this.OnLanguageChanged), LanguageSetter.ChangedEvent);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0002D879 File Offset: 0x0002BA79
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnLanguageChanged), LanguageSetter.ChangedEvent);
		}

		// Token: 0x040008AD RID: 2221
		private TMP_Text tmp;

		// Token: 0x040008AE RID: 2222
		[SerializeField]
		private LocalizedString localizedString;
	}
}

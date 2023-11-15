using System;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class LocalisedObject : MonoBehaviour
{
	// Token: 0x060005BD RID: 1469 RVA: 0x0002AC4C File Offset: 0x0002904C
	protected void Awake()
	{
		SupportedLanguages language = Localization.GetLanguage();
		bool flag = MaskUtils.HasFlag<SupportedLanguages>(this.m_languageMask, language);
		bool flag2 = this.m_flagBehaviour == LocalisedObject.FlagBehaviour.Activator;
		bool active = !(flag ^ flag2);
		base.gameObject.SetActive(active);
	}

	// Token: 0x040004BD RID: 1213
	[SerializeField]
	[Mask(typeof(SupportedLanguages))]
	private int m_languageMask;

	// Token: 0x040004BE RID: 1214
	[SerializeField]
	private LocalisedObject.FlagBehaviour m_flagBehaviour;

	// Token: 0x02000141 RID: 321
	private enum FlagBehaviour
	{
		// Token: 0x040004C0 RID: 1216
		Activator,
		// Token: 0x040004C1 RID: 1217
		Deactivator
	}
}

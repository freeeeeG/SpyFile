using System;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class PerformanceDependantActivation : MonoBehaviour
{
	// Token: 0x060007E6 RID: 2022 RVA: 0x00030EE6 File Offset: 0x0002F2E6
	public static string GetArrayName(int _index)
	{
		return QualitySettings.names.TryAtIndex(_index);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00030EF4 File Offset: 0x0002F2F4
	private void Awake()
	{
		int qualityLevel = QualitySettings.GetQualityLevel();
		bool active = this.IsActive.TryAtIndex(qualityLevel, false);
		base.gameObject.SetActive(active);
	}

	// Token: 0x0400064A RID: 1610
	[ArrayNames("GetArrayName")]
	public bool[] IsActive;
}

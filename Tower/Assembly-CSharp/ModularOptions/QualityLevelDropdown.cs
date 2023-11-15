using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000093 RID: 147
	[AddComponentMenu("Modular Options/Display/Quality Level Dropdown")]
	[RequireComponent(typeof(TMP_Dropdown))]
	public sealed class QualityLevelDropdown : MonoBehaviour
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00008A60 File Offset: 0x00006C60
		private void Awake()
		{
			this.dropdown = base.GetComponent<TMP_Dropdown>();
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(new List<string>(QualitySettings.names));
			this.dropdown.value = QualitySettings.GetQualityLevel();
			this.dropdown.RefreshShownValue();
			this.dropdown.onValueChanged.AddListener(delegate(int _)
			{
				this.OnValueChange(_);
			});
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008AD0 File Offset: 0x00006CD0
		private void OnValueChange(int _value)
		{
			QualitySettings.SetQualityLevel(_value, true);
		}

		// Token: 0x040001CE RID: 462
		private TMP_Dropdown dropdown;
	}
}

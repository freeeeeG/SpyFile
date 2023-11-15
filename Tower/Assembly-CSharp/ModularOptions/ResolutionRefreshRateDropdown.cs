using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000095 RID: 149
	[AddComponentMenu("Modular Options/Display/Resolution & RefreshRate Dropdown")]
	[RequireComponent(typeof(TMP_Dropdown))]
	public sealed class ResolutionRefreshRateDropdown : MonoBehaviour
	{
		// Token: 0x06000213 RID: 531 RVA: 0x00008C71 File Offset: 0x00006E71
		private void Awake()
		{
			this.dropdown = base.GetComponent<TMP_Dropdown>();
			this.UpdateResolutions();
			this.dropdown.onValueChanged.AddListener(delegate(int _)
			{
				this.OnValueChange(_);
			});
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008CA4 File Offset: 0x00006EA4
		private void UpdateResolutions()
		{
			this.resolutions = Screen.resolutions;
			List<string> list = new List<string>();
			int value = 0;
			Resolution currentResolution = Screen.currentResolution;
			currentResolution.width = Screen.width;
			currentResolution.height = Screen.height;
			int i = 0;
			int num = this.resolutions.Length;
			while (i < num)
			{
				list.Add(this.resolutions[i].ToString());
				if (this.resolutions[i].Equals(currentResolution))
				{
					value = i;
				}
				i++;
			}
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(list);
			this.dropdown.value = value;
			this.dropdown.RefreshShownValue();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008D68 File Offset: 0x00006F68
		private void OnValueChange(int _resolutionIndex)
		{
			Resolution resolution = this.resolutions[_resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);
		}

		// Token: 0x040001D2 RID: 466
		private Resolution[] resolutions;

		// Token: 0x040001D3 RID: 467
		private TMP_Dropdown dropdown;
	}
}

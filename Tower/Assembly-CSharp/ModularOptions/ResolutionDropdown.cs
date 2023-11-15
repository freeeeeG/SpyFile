using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000094 RID: 148
	[AddComponentMenu("Modular Options/Display/Resolution Dropdown")]
	[RequireComponent(typeof(TMP_Dropdown))]
	public sealed class ResolutionDropdown : MonoBehaviour
	{
		// Token: 0x0600020E RID: 526 RVA: 0x00008AEA File Offset: 0x00006CEA
		private void Awake()
		{
			this.dropdown = base.GetComponent<TMP_Dropdown>();
			this.UpdateResolutions();
			this.dropdown.onValueChanged.AddListener(delegate(int _)
			{
				this.OnValueChange(_);
			});
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00008B1C File Offset: 0x00006D1C
		private void UpdateResolutions()
		{
			Resolution[] array = Screen.resolutions;
			List<string> list = new List<string>();
			int value = 0;
			Vector2Int other = new Vector2Int(Screen.width, Screen.height);
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				Vector2Int item = new Vector2Int(array[i].width, array[i].height);
				if (!this.resolutions.Contains(item))
				{
					this.resolutions.Add(item);
					list.Add(item.x.ToString() + this.separator + item.y.ToString());
					if (item.Equals(other))
					{
						value = this.resolutions.Count - 1;
					}
				}
				i++;
			}
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(list);
			this.dropdown.value = value;
			this.dropdown.RefreshShownValue();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008C18 File Offset: 0x00006E18
		private void OnValueChange(int _resolutionIndex)
		{
			Vector2Int vector2Int = this.resolutions[_resolutionIndex];
			Screen.SetResolution(vector2Int.x, vector2Int.y, Screen.fullScreenMode);
		}

		// Token: 0x040001CF RID: 463
		[Tooltip("Text separating Horizontal from Vertical Resolution.")]
		public string separator = "x";

		// Token: 0x040001D0 RID: 464
		private List<Vector2Int> resolutions = new List<Vector2Int>();

		// Token: 0x040001D1 RID: 465
		private TMP_Dropdown dropdown;
	}
}

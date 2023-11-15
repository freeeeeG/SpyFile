using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200021A RID: 538
	public class MapDescription : DataUIBinding<MapData>
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x0002C79B File Offset: 0x0002A99B
		public override void Refresh()
		{
			this.descriptionTMP.text = base.data.description;
			this.acensionModeUI.SetActive(!base.data.darknessDisabled);
		}

		// Token: 0x04000860 RID: 2144
		[SerializeField]
		private TMP_Text descriptionTMP;

		// Token: 0x04000861 RID: 2145
		[SerializeField]
		private GameObject acensionModeUI;
	}
}

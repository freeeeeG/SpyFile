using System;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200021B RID: 539
	public class MapEntry : DataUIBinding<MapData>
	{
		// Token: 0x06000C04 RID: 3076 RVA: 0x0002C7D4 File Offset: 0x0002A9D4
		public override void Refresh()
		{
			if (this.labelTMP != null)
			{
				this.labelTMP.text = base.data.nameString;
			}
		}

		// Token: 0x04000862 RID: 2146
		[SerializeField]
		private TMP_Text labelTMP;
	}
}

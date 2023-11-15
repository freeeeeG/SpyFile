using System;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200021D RID: 541
	public class PowerupIcon : DataUIBinding<Powerup>
	{
		// Token: 0x06000C08 RID: 3080 RVA: 0x0002C8BB File Offset: 0x0002AABB
		public override void Refresh()
		{
			this.iconImage.sprite = base.data.icon;
			if (this.tooltipText != null)
			{
				this.tooltipText.tooltip = base.data.description;
			}
		}

		// Token: 0x04000867 RID: 2151
		[SerializeField]
		private Image iconImage;

		// Token: 0x04000868 RID: 2152
		[SerializeField]
		private ToolTipText tooltipText;
	}
}

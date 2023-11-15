using System;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000205 RID: 517
	public class ColorTintOnToggle : MonoBehaviour
	{
		// Token: 0x06000BAA RID: 2986 RVA: 0x0002B77B File Offset: 0x0002997B
		private void Awake()
		{
			this.toggle.onValueChanged.AddListener(delegate(bool <p0>)
			{
				this.ToggleValueChanged(this.toggle);
			});
			this._originalColor = this.targetGraphic.color;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0002B7AA File Offset: 0x000299AA
		private void ToggleValueChanged(Toggle change)
		{
			if (this.toggle.isOn)
			{
				this.targetGraphic.color = this.toggleOnColor;
				return;
			}
			this.targetGraphic.color = this._originalColor;
		}

		// Token: 0x04000804 RID: 2052
		[SerializeField]
		private Toggle toggle;

		// Token: 0x04000805 RID: 2053
		[SerializeField]
		private Graphic targetGraphic;

		// Token: 0x04000806 RID: 2054
		[SerializeField]
		private Color toggleOnColor = Color.white;

		// Token: 0x04000807 RID: 2055
		private Color _originalColor;
	}
}

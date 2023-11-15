using System;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x0200023D RID: 573
	public abstract class Widget<T> : MonoBehaviour where T : IUIProperties
	{
		// Token: 0x06000C97 RID: 3223 RVA: 0x0002E05B File Offset: 0x0002C25B
		private void Start()
		{
			this.panel = base.GetComponent<Panel>();
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0002E069 File Offset: 0x0002C269
		public virtual void Show()
		{
			if (this.panel != null)
			{
				this.panel.Show();
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0002E084 File Offset: 0x0002C284
		public virtual void Hide()
		{
			if (this.panel != null)
			{
				this.panel.Hide();
			}
		}

		// Token: 0x06000C9A RID: 3226
		public abstract void SetProperties(T properties);

		// Token: 0x040008D1 RID: 2257
		private Panel panel;
	}
}

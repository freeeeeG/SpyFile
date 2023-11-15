using System;
using Level;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000355 RID: 853
	public class PropDestroyed : Trigger
	{
		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002FC3D File Offset: 0x0002DE3D
		private void Awake()
		{
			if (this._prop == null)
			{
				return;
			}
			this._prop.onDestroy += delegate()
			{
				this._destroyed = true;
			};
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002FC65 File Offset: 0x0002DE65
		protected override bool Check()
		{
			return this._destroyed;
		}

		// Token: 0x04000D0D RID: 3341
		[SerializeField]
		private Prop _prop;

		// Token: 0x04000D0E RID: 3342
		private bool _destroyed;
	}
}

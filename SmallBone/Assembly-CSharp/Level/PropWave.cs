using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200053F RID: 1343
	public class PropWave : Wave
	{
		// Token: 0x06001A6A RID: 6762 RVA: 0x00052DB2 File Offset: 0x00050FB2
		public override void Initialize()
		{
			base.state = Wave.State.Spawned;
			this._prop.onDestroy += this.Clear;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00051D0C File Offset: 0x0004FF0C
		private void Clear()
		{
			base.state = Wave.State.Cleared;
			Action onClear = this._onClear;
			if (onClear == null)
			{
				return;
			}
			onClear();
		}

		// Token: 0x04001702 RID: 5890
		[SerializeField]
		private Wave _formerWave;

		// Token: 0x04001703 RID: 5891
		[SerializeField]
		private Prop _prop;
	}
}

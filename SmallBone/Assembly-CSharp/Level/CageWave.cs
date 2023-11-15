using System;
using UnityEngine;

namespace Level
{
	// Token: 0x02000532 RID: 1330
	public class CageWave : Wave
	{
		// Token: 0x06001A1B RID: 6683 RVA: 0x00051CA0 File Offset: 0x0004FEA0
		public override void Initialize()
		{
			base.state = Wave.State.Spawned;
			this._cage.onDestroyed += this.Clear;
			if (this._formerWave == null)
			{
				this._cage.Activate();
				return;
			}
			this._formerWave.onClear += this._cage.Activate;
			this._cage.Deactivate();
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x00051D0C File Offset: 0x0004FF0C
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

		// Token: 0x040016D7 RID: 5847
		[SerializeField]
		private Wave _formerWave;

		// Token: 0x040016D8 RID: 5848
		[SerializeField]
		private Cage _cage;
	}
}

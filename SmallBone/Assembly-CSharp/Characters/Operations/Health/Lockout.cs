using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E89 RID: 3721
	public sealed class Lockout : CharacterOperation
	{
		// Token: 0x060049AD RID: 18861 RVA: 0x000D732B File Offset: 0x000D552B
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._runReference.Stop();
			this._runReference = this.StartCoroutineWithReference(this.CRun(owner));
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x000D7352 File Offset: 0x000D5552
		private IEnumerator CRun(Character character)
		{
			if (this._duration == 0f)
			{
				this._duration = 2.1474836E+09f;
			}
			character.status.unstoppable.Attach(this);
			yield return character.chronometer.master.WaitForSeconds(this._duration);
			character.status.unstoppable.Detach(this);
			yield break;
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x000D7368 File Offset: 0x000D5568
		public override void Stop()
		{
			if (this._owner != null)
			{
				this._owner.status.unstoppable.Detach(this);
			}
		}

		// Token: 0x040038DF RID: 14559
		[SerializeField]
		[FrameTime]
		private float _duration;

		// Token: 0x040038E0 RID: 14560
		private CoroutineReference _runReference;

		// Token: 0x040038E1 RID: 14561
		private Character _owner;
	}
}

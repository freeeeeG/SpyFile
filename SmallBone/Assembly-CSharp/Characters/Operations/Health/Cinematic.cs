using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E80 RID: 3712
	public sealed class Cinematic : CharacterOperation
	{
		// Token: 0x06004989 RID: 18825 RVA: 0x000D6F42 File Offset: 0x000D5142
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._runReference.Stop();
			this._runReference = owner.StartCoroutineWithReference(this.CRun(owner));
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x000D6F69 File Offset: 0x000D5169
		private IEnumerator CRun(Character character)
		{
			if (this._duration == 0f)
			{
				this._duration = 2.1474836E+09f;
			}
			character.cinematic.Attach(this);
			yield return character.chronometer.master.WaitForSeconds(this._duration);
			character.cinematic.Detach(this);
			yield break;
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x000D6F7F File Offset: 0x000D517F
		public override void Stop()
		{
			if (this._owner != null)
			{
				this._owner.cinematic.Detach(this);
			}
		}

		// Token: 0x040038C4 RID: 14532
		[FrameTime]
		[SerializeField]
		private float _duration;

		// Token: 0x040038C5 RID: 14533
		private CoroutineReference _runReference;

		// Token: 0x040038C6 RID: 14534
		private Character _owner;
	}
}

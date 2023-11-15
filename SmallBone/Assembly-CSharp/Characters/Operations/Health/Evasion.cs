using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E82 RID: 3714
	public sealed class Evasion : CharacterOperation
	{
		// Token: 0x06004993 RID: 18835 RVA: 0x000D7050 File Offset: 0x000D5250
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._runReference.Stop();
			this._runReference = this.StartCoroutineWithReference(this.CRun(owner));
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x000D7077 File Offset: 0x000D5277
		private IEnumerator CRun(Character character)
		{
			if (this._duration == 0f)
			{
				this._duration = 2.1474836E+09f;
			}
			character.evasion.Attach(this);
			yield return character.chronometer.master.WaitForSeconds(this._duration);
			character.evasion.Detach(this);
			yield break;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x000D708D File Offset: 0x000D528D
		public override void Stop()
		{
			if (this._owner != null)
			{
				this._owner.evasion.Detach(this);
			}
		}

		// Token: 0x040038CB RID: 14539
		[FrameTime]
		[SerializeField]
		private float _duration;

		// Token: 0x040038CC RID: 14540
		private CoroutineReference _runReference;

		// Token: 0x040038CD RID: 14541
		private Character _owner;
	}
}

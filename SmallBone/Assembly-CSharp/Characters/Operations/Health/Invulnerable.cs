using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Health
{
	// Token: 0x02000E86 RID: 3718
	public class Invulnerable : CharacterOperation
	{
		// Token: 0x060049A0 RID: 18848 RVA: 0x000D7206 File Offset: 0x000D5406
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._runReference.Stop();
			this._runReference = this.StartCoroutineWithReference(this.CRun(owner));
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x000D722D File Offset: 0x000D542D
		private IEnumerator CRun(Character character)
		{
			if (this._duration == 0f)
			{
				this._duration = 2.1474836E+09f;
			}
			character.invulnerable.Attach(this);
			yield return character.chronometer.master.WaitForSeconds(this._duration);
			character.invulnerable.Detach(this);
			yield break;
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x000D7243 File Offset: 0x000D5443
		public override void Stop()
		{
			if (this._owner != null)
			{
				this._owner.invulnerable.Detach(this);
			}
		}

		// Token: 0x040038D8 RID: 14552
		[SerializeField]
		[FrameTime]
		private float _duration;

		// Token: 0x040038D9 RID: 14553
		private CoroutineReference _runReference;

		// Token: 0x040038DA RID: 14554
		private Character _owner;
	}
}

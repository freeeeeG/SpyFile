using System;
using Runnables;
using UnityEngine;

namespace CutScenes
{
	// Token: 0x020001AF RID: 431
	public class CharacterStealth : State
	{
		// Token: 0x0600092E RID: 2350 RVA: 0x0001A492 File Offset: 0x00018692
		public override void Attach()
		{
			this._target.character.stealth.Attach(State.key);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001A4AE File Offset: 0x000186AE
		public override void Detach()
		{
			if (this._target == null)
			{
				return;
			}
			if (this._target.character == null)
			{
				return;
			}
			this._target.character.stealth.Detach(State.key);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDisable()
		{
			this.Detach();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDestroy()
		{
			this.Detach();
		}

		// Token: 0x04000791 RID: 1937
		[SerializeField]
		private Target _target;
	}
}

using System;
using Runnables;
using UnityEngine;

namespace CutScenes
{
	// Token: 0x020001AE RID: 430
	public class CharacterInvulnerable : State
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x0001A404 File Offset: 0x00018604
		public override void Attach()
		{
			this._target.character.cinematic.Attach(State.key);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001A420 File Offset: 0x00018620
		public override void Detach()
		{
			if (this._target != null && this._target.character != null)
			{
				this._target.character.cinematic.Detach(State.key);
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001A387 File Offset: 0x00018587
		private void OnDisable()
		{
			this.Detach();
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001A458 File Offset: 0x00018658
		private void OnDestroy()
		{
			if (this._target == null)
			{
				return;
			}
			if (this._target.character == null)
			{
				return;
			}
			this._target.character.cinematic.Detach(State.key);
		}

		// Token: 0x04000790 RID: 1936
		[SerializeField]
		private Target _target;
	}
}

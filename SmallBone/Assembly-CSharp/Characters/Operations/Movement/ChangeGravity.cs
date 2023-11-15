using System;
using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E4F RID: 3663
	public sealed class ChangeGravity : CharacterOperation
	{
		// Token: 0x060048CF RID: 18639 RVA: 0x000D44E5 File Offset: 0x000D26E5
		public override void Run(Character owner)
		{
			this.character = owner;
			this.Attach();
			this._coroutineReference.Stop();
			this._coroutineReference = owner.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x000D4511 File Offset: 0x000D2711
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				yield return null;
				if (!this._originalConfig.Equals(this.character.movement.config))
				{
					this.Attach();
				}
			}
			yield break;
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x000D4520 File Offset: 0x000D2720
		private void Attach()
		{
			if (this._originalConfig != null)
			{
				this._originalConfig.gravity = this._originalGravity;
			}
			this._originalConfig = this.character.movement.config;
			this._originalGravity = this._originalConfig.gravity;
			this._originalConfig.gravity = this._gravirty;
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x000D457E File Offset: 0x000D277E
		public override void Stop()
		{
			this._coroutineReference.Stop();
			if (this.character != null && this._originalConfig != null)
			{
				this._originalConfig.gravity = this._originalGravity;
			}
		}

		// Token: 0x040037DB RID: 14299
		[SerializeField]
		private float _gravirty;

		// Token: 0x040037DC RID: 14300
		private Character character;

		// Token: 0x040037DD RID: 14301
		private Movement.Config _originalConfig;

		// Token: 0x040037DE RID: 14302
		private float _originalGravity;

		// Token: 0x040037DF RID: 14303
		private CoroutineReference _coroutineReference;
	}
}

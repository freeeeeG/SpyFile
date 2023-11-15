using System;
using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E64 RID: 3684
	public class OverrideMovementConfig : CharacterOperation
	{
		// Token: 0x0600491F RID: 18719 RVA: 0x000D55C0 File Offset: 0x000D37C0
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._owner.movement.configs.Add(this._priority, this._config);
			if (this._config.keepMove)
			{
				this._owner.movement.MoveHorizontal((this._owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
			}
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CRun());
			}
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x000D5645 File Offset: 0x000D3845
		private IEnumerator CRun()
		{
			yield return this._owner.chronometer.master.WaitForSeconds(this._duration);
			this.Remove();
			yield break;
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x000D5654 File Offset: 0x000D3854
		private void Remove()
		{
			if (this._owner == null)
			{
				return;
			}
			this._owner.movement.configs.Remove(this._config);
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x000D5681 File Offset: 0x000D3881
		public override void Stop()
		{
			base.Stop();
			this.Remove();
		}

		// Token: 0x04003844 RID: 14404
		[SerializeField]
		private Movement.Config _config;

		// Token: 0x04003845 RID: 14405
		[SerializeField]
		private int _priority;

		// Token: 0x04003846 RID: 14406
		[SerializeField]
		private float _duration;

		// Token: 0x04003847 RID: 14407
		private Character _owner;
	}
}

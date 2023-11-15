using System;
using System.Collections;
using Characters;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A4 RID: 5284
	[TaskDescription("Chronometer로 체크하는 쿨다운")]
	public sealed class ChronometerCoolDown : Conditional
	{
		// Token: 0x0600670A RID: 26378 RVA: 0x0012A170 File Offset: 0x00128370
		public override void OnAwake()
		{
			base.OnAwake();
			if (this.preCoolDown.Value)
			{
				this._owner = this.chronomaterOwner.Value;
				this._cooldownReference = this._owner.StartCoroutineWithReference(this.CCooldown(this._owner.chronometer.master));
				return;
			}
			this._canUse = true;
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x0012A1D0 File Offset: 0x001283D0
		public override TaskStatus OnUpdate()
		{
			if (this.duration.Value == 0f)
			{
				return TaskStatus.Success;
			}
			if (!this._canUse)
			{
				return TaskStatus.Failure;
			}
			this._owner = this.chronomaterOwner.Value;
			this._cooldownReference = this._owner.StartCoroutineWithReference(this.CCooldown(this._owner.chronometer.master));
			return TaskStatus.Success;
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x0012A234 File Offset: 0x00128434
		private IEnumerator CCooldown(Chronometer chronometer)
		{
			this._canUse = false;
			float elapsed = 0f;
			float durationValue = this.duration.Value;
			while (elapsed < durationValue)
			{
				elapsed += chronometer.deltaTime;
				yield return null;
			}
			this._canUse = true;
			yield break;
		}

		// Token: 0x040052F2 RID: 21234
		public SharedCharacter chronomaterOwner;

		// Token: 0x040052F3 RID: 21235
		public SharedFloat duration = 2f;

		// Token: 0x040052F4 RID: 21236
		public SharedBool preCoolDown;

		// Token: 0x040052F5 RID: 21237
		private bool _canUse;

		// Token: 0x040052F6 RID: 21238
		private Character _owner;

		// Token: 0x040052F7 RID: 21239
		private CoroutineReference _cooldownReference;
	}
}

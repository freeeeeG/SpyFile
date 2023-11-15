using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200092B RID: 2347
	public class ComboAction : Action
	{
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003270 RID: 12912 RVA: 0x000960EA File Offset: 0x000942EA
		internal ComboAction.ActionInfo current
		{
			get
			{
				return this._actionInfo.values[this._current];
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003271 RID: 12913 RVA: 0x000960FE File Offset: 0x000942FE
		public override Motion[] motions
		{
			get
			{
				return (from m in this._actionInfo.values
				select m.motion).ToArray<Motion>();
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003272 RID: 12914 RVA: 0x00096134 File Offset: 0x00094334
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.current.motion);
			}
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x00096163 File Offset: 0x00094363
		private IEnumerator CReservedAttack()
		{
			while (this._cancelReserved && this._owner.runningMotion == this.current.motion)
			{
				if (MMMaths.Range(this.current.motion.time, this.current.cancel) && (this._cancelReserved || MMMaths.Range(this.current.motion.time, this.current.input)))
				{
					this._cancelReserved = false;
					int num = this._current + 1;
					if (num >= this._actionInfo.values.Length)
					{
						num = this._cycleOffset;
					}
					this._current = num;
					base.DoAction(this._actionInfo.values[num].motion);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x00096174 File Offset: 0x00094374
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this._actionInfo.values.Length; i++)
			{
				this._actionInfo.values[i].motion.Initialize(this);
			}
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000961B8 File Offset: 0x000943B8
		public override bool TryStart()
		{
			if (!this.canUse)
			{
				return false;
			}
			if (this._owner.runningMotion != null && this._owner.runningMotion.action == this)
			{
				if (this._owner.runningMotion != this.current.motion)
				{
					return false;
				}
				if (this._cancelReserved)
				{
					return false;
				}
				if (this.current.input.x == this.current.input.y)
				{
					return false;
				}
				if (!MMMaths.Range(this._owner.runningMotion.time, this.current.input))
				{
					return false;
				}
				this._cancelReserved = true;
				base.StartCoroutine(this.CReservedAttack());
				return true;
			}
			else
			{
				this._current = 0;
				this._cancelReserved = false;
				if (!base.ConsumeCooldownIfNeeded())
				{
					return false;
				}
				base.DoAction(this.current.motion);
				return true;
			}
		}

		// Token: 0x04002935 RID: 10549
		[SerializeField]
		private ComboAction.ActionInfo.Reorderable _actionInfo;

		// Token: 0x04002936 RID: 10550
		[SerializeField]
		private int _cycleOffset;

		// Token: 0x04002937 RID: 10551
		protected bool _cancelReserved;

		// Token: 0x04002938 RID: 10552
		protected int _current;

		// Token: 0x0200092C RID: 2348
		[Serializable]
		internal class ActionInfo
		{
			// Token: 0x17000ADD RID: 2781
			// (get) Token: 0x06003277 RID: 12919 RVA: 0x000962B1 File Offset: 0x000944B1
			internal Vector2 input
			{
				get
				{
					return this._input;
				}
			}

			// Token: 0x17000ADE RID: 2782
			// (get) Token: 0x06003278 RID: 12920 RVA: 0x000962B9 File Offset: 0x000944B9
			internal Vector2 cancel
			{
				get
				{
					return this._cancel;
				}
			}

			// Token: 0x17000ADF RID: 2783
			// (get) Token: 0x06003279 RID: 12921 RVA: 0x000962C1 File Offset: 0x000944C1
			internal Motion motion
			{
				get
				{
					return this._motion;
				}
			}

			// Token: 0x04002939 RID: 10553
			[SerializeField]
			private Vector2 _input;

			// Token: 0x0400293A RID: 10554
			[SerializeField]
			private Vector2 _cancel;

			// Token: 0x0400293B RID: 10555
			[SerializeField]
			private Motion _motion;

			// Token: 0x0200092D RID: 2349
			[Serializable]
			internal class Reorderable : ReorderableArray<ComboAction.ActionInfo>
			{
			}
		}
	}
}

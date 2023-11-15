using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000933 RID: 2355
	public class EnhanceableComboAction : Action
	{
		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x000965F6 File Offset: 0x000947F6
		private EnhanceableComboAction.ActionInfo current
		{
			get
			{
				return this._currentActionInfo[this._current];
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x00096605 File Offset: 0x00094805
		public override Motion[] motions
		{
			get
			{
				return (from m in this._actionInfo.values
				select m.motion).ToArray<Motion>();
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003293 RID: 12947 RVA: 0x0009663B File Offset: 0x0009483B
		private EnhanceableComboAction.ActionInfo[] _currentActionInfo
		{
			get
			{
				if (!this.enhanced)
				{
					return this._actionInfo.values;
				}
				return this._enhancedActionInfo.values;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003294 RID: 12948 RVA: 0x0009665C File Offset: 0x0009485C
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.current.motion);
			}
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x0009668B File Offset: 0x0009488B
		private IEnumerator CReservedAttack()
		{
			while (this._cancelReserved)
			{
				Motion motion = this._actionInfo.values[this._current].motion;
				Motion motion2 = this._enhancedActionInfo.values[this._current].motion;
				EnhanceableComboAction.ActionInfo actionInfo;
				if (this._owner.runningMotion == motion)
				{
					actionInfo = this._actionInfo.values[this._current];
				}
				else
				{
					if (!(this._owner.runningMotion == motion2))
					{
						break;
					}
					actionInfo = this._enhancedActionInfo.values[this._current];
				}
				if (MMMaths.Range(actionInfo.motion.time, actionInfo.cancel) && (this._cancelReserved || MMMaths.Range(actionInfo.motion.time, actionInfo.input)))
				{
					this._cancelReserved = false;
					int num = this._current + 1;
					if (num >= this._currentActionInfo.Length)
					{
						num = this._cycleOffset;
					}
					this._current = num;
					base.DoAction(this._currentActionInfo[num].motion);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x0009669C File Offset: 0x0009489C
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this._actionInfo.values.Length; i++)
			{
				this._actionInfo.values[i].motion.Initialize(this);
			}
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000966E0 File Offset: 0x000948E0
		public override bool TryStart()
		{
			if (!this.canUse)
			{
				return false;
			}
			if (this._owner.runningMotion != null && this._owner.runningMotion.action == this)
			{
				if (this._cancelReserved)
				{
					return false;
				}
				Motion motion = this._actionInfo.values[this._current].motion;
				Motion motion2 = this._enhancedActionInfo.values[this._current].motion;
				EnhanceableComboAction.ActionInfo actionInfo;
				if (this._owner.runningMotion == motion)
				{
					actionInfo = this._actionInfo.values[this._current];
				}
				else
				{
					if (!(this._owner.runningMotion == motion2))
					{
						return false;
					}
					actionInfo = this._enhancedActionInfo.values[this._current];
				}
				if (actionInfo.input.x == actionInfo.input.y)
				{
					return false;
				}
				if (!MMMaths.Range(actionInfo.motion.time, actionInfo.input))
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

		// Token: 0x0400294B RID: 10571
		[Header("Actions")]
		[SerializeField]
		private EnhanceableComboAction.ActionInfo.Reorderable _actionInfo;

		// Token: 0x0400294C RID: 10572
		[SerializeField]
		[Header("Enhanced Actions")]
		private EnhanceableComboAction.ActionInfo.Reorderable _enhancedActionInfo;

		// Token: 0x0400294D RID: 10573
		[SerializeField]
		private int _cycleOffset;

		// Token: 0x0400294E RID: 10574
		protected bool _cancelReserved;

		// Token: 0x0400294F RID: 10575
		protected int _current;

		// Token: 0x04002950 RID: 10576
		[NonSerialized]
		public bool enhanced;

		// Token: 0x02000934 RID: 2356
		[Serializable]
		internal class ActionInfo
		{
			// Token: 0x17000AE8 RID: 2792
			// (get) Token: 0x06003299 RID: 12953 RVA: 0x00096828 File Offset: 0x00094A28
			internal Vector2 input
			{
				get
				{
					return this._input;
				}
			}

			// Token: 0x17000AE9 RID: 2793
			// (get) Token: 0x0600329A RID: 12954 RVA: 0x00096830 File Offset: 0x00094A30
			internal Vector2 cancel
			{
				get
				{
					return this._cancel;
				}
			}

			// Token: 0x17000AEA RID: 2794
			// (get) Token: 0x0600329B RID: 12955 RVA: 0x00096838 File Offset: 0x00094A38
			internal Motion motion
			{
				get
				{
					return this._motion;
				}
			}

			// Token: 0x04002951 RID: 10577
			[SerializeField]
			private Vector2 _input;

			// Token: 0x04002952 RID: 10578
			[SerializeField]
			private Vector2 _cancel;

			// Token: 0x04002953 RID: 10579
			[SerializeField]
			private Motion _motion;

			// Token: 0x02000935 RID: 2357
			[Serializable]
			internal class Reorderable : ReorderableArray<EnhanceableComboAction.ActionInfo>
			{
			}
		}
	}
}

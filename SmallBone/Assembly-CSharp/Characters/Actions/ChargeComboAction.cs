using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000924 RID: 2340
	public class ChargeComboAction : Action
	{
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06003250 RID: 12880 RVA: 0x000957D4 File Offset: 0x000939D4
		internal ChargeComboAction.ActionInfo current
		{
			get
			{
				return this._actionInfo.values[this._current];
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x000957E8 File Offset: 0x000939E8
		public override Motion[] motions
		{
			get
			{
				if (this._motions == null)
				{
					this._motions = this._actionInfo.values.SelectMany((ChargeComboAction.ActionInfo m) => m.motions).ToArray<Motion>();
				}
				return this._motions;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003252 RID: 12882 RVA: 0x0009583D File Offset: 0x00093A3D
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.current.anticipation);
			}
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x0009586C File Offset: 0x00093A6C
		private IEnumerator CReservedAttack()
		{
			while (this._cancelReserved)
			{
				Vector2 range;
				Vector2 range2;
				if (this._owner.runningMotion == this.current.earlyFinish)
				{
					range = this.current.earlyInput;
					range2 = this.current.earlyCancel;
				}
				else
				{
					if (!(this._owner.runningMotion == this.current.finish))
					{
						yield break;
					}
					range = this.current.input;
					range2 = this.current.cancel;
				}
				if (MMMaths.Range(this._owner.runningMotion.normalizedTime, range2) && (this._cancelReserved || MMMaths.Range(this._owner.runningMotion.normalizedTime, range)))
				{
					this._cancelReserved = false;
					int num = this._current + 1;
					if (num >= this._actionInfo.values.Length)
					{
						num = this._cycleOffset;
					}
					ChargeComboAction.ActionInfo actionInfo = this._actionInfo.values[num];
					if (this._endReserved)
					{
						this._endReserved = false;
						actionInfo.ReserveEarlyFinish();
					}
					this._current = num;
					base.DoAction(actionInfo.anticipation);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x0009587C File Offset: 0x00093A7C
		protected override void Awake()
		{
			for (int i = 0; i < this._actionInfo.values.Length; i++)
			{
				this._actionInfo.values[i].InitializeMotions(this);
			}
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000958B4 File Offset: 0x00093AB4
		private void InvokeStartCharging()
		{
			Action<Action> onStartCharging = this._owner.onStartCharging;
			if (onStartCharging == null)
			{
				return;
			}
			onStartCharging(this);
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x0009531A File Offset: 0x0009351A
		private void InvokeEndCharging()
		{
			Action<Action> onStopCharging = this._owner.onStopCharging;
			if (onStopCharging == null)
			{
				return;
			}
			onStopCharging(this);
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x00095332 File Offset: 0x00093532
		private void InvokeCancelCharging()
		{
			Action<Action> onCancelCharging = this._owner.onCancelCharging;
			if (onCancelCharging == null)
			{
				return;
			}
			onCancelCharging(this);
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000958CC File Offset: 0x00093ACC
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			Motion runningMotion = this._owner.runningMotion;
			if (!(runningMotion != null) || !(runningMotion.action == this))
			{
				this._current = 0;
				this._cancelReserved = false;
				this._endReserved = false;
				base.DoAction(this.current.anticipation);
				return true;
			}
			if (this._cancelReserved)
			{
				return false;
			}
			Vector2 vector;
			if (this._owner.runningMotion == this.current.earlyFinish)
			{
				vector = this.current.earlyInput;
			}
			else
			{
				if (!(this._owner.runningMotion == this.current.finish))
				{
					return false;
				}
				vector = this.current.input;
			}
			if (vector.x == vector.y)
			{
				return false;
			}
			if (!MMMaths.Range(runningMotion.normalizedTime, vector))
			{
				return false;
			}
			this._cancelReserved = true;
			base.StartCoroutine(this.CReservedAttack());
			return true;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000959E0 File Offset: 0x00093BE0
		public override bool TryEnd()
		{
			Motion runningMotion = this._owner.runningMotion;
			if (runningMotion == null || runningMotion.action != this)
			{
				return false;
			}
			if (runningMotion == this.current.earlyFinish || runningMotion == this.current.finish)
			{
				if (this._cancelReserved)
				{
					this._endReserved = true;
				}
				return false;
			}
			if (this.current.charged != null && runningMotion == this.current.charged)
			{
				base.DoMotion(this.current.finish);
				return true;
			}
			if (!(this.current.earlyFinish != null) || !(runningMotion != this.current.earlyFinish) || !(runningMotion != this.current.finish))
			{
				base.owner.CancelAction();
				return false;
			}
			if (base.owner.motion == this.current.anticipation || base.owner.motion == this.current.prepare)
			{
				this.current.ReserveEarlyFinish();
				return false;
			}
			base.DoMotion(this.current.earlyFinish);
			return true;
		}

		// Token: 0x04002918 RID: 10520
		[SerializeField]
		private ChargeComboAction.ActionInfo.Reorderable _actionInfo;

		// Token: 0x04002919 RID: 10521
		[SerializeField]
		private int _cycleOffset;

		// Token: 0x0400291A RID: 10522
		protected bool _cancelReserved;

		// Token: 0x0400291B RID: 10523
		protected bool _endReserved;

		// Token: 0x0400291C RID: 10524
		protected int _current;

		// Token: 0x0400291D RID: 10525
		private Character.LookingDirection _lookingDirection;

		// Token: 0x0400291E RID: 10526
		private Motion[] _motions;

		// Token: 0x02000925 RID: 2341
		[Serializable]
		internal class ActionInfo
		{
			// Token: 0x17000AD7 RID: 2775
			// (get) Token: 0x0600325B RID: 12891 RVA: 0x00095B24 File Offset: 0x00093D24
			public Motion[] motions
			{
				get
				{
					Motion[] result;
					if ((result = this._motions) == null)
					{
						result = (this._motions = new Motion[]
						{
							this.anticipation,
							this.prepare,
							this.charging,
							this.charged,
							this.earlyFinish,
							this.finish
						});
					}
					return result;
				}
			}

			// Token: 0x0600325C RID: 12892 RVA: 0x00095B80 File Offset: 0x00093D80
			public void InitializeMotions(ChargeComboAction action)
			{
				ChargeComboAction.ActionInfo.<>c__DisplayClass16_0 CS$<>8__locals1 = new ChargeComboAction.ActionInfo.<>c__DisplayClass16_0();
				CS$<>8__locals1.action = action;
				CS$<>8__locals1.<>4__this = this;
				this._action = CS$<>8__locals1.action;
				List<Motion> list = new List<Motion>(6);
				list.Add(this.anticipation);
				if (this.prepare != null)
				{
					list.Add(this.prepare);
				}
				list.Add(this.charging);
				if (this.charged != null)
				{
					list.Add(this.charged);
				}
				list.Add(this.finish);
				for (int i = 0; i < list.Count - 1; i++)
				{
					Motion nextMotion = list[i + 1];
					list[i].onEnd += delegate()
					{
						CS$<>8__locals1.action.DoMotion(nextMotion);
					};
				}
				if (this.earlyFinish != null)
				{
					list.Add(this.earlyFinish);
				}
				this._motions = list.ToArray();
				Motion[] motions = this._motions;
				for (int j = 0; j < motions.Length; j++)
				{
					motions[j].Initialize(CS$<>8__locals1.action);
				}
				if (this.anticipation.blockLook)
				{
					this.anticipation.onStart += delegate()
					{
						CS$<>8__locals1.action._lookingDirection = CS$<>8__locals1.action._owner.lookingDirection;
					};
					if (this.prepare != null)
					{
						this.prepare.onStart += CS$<>8__locals1.<InitializeMotions>g__RepositLookingDirection|1;
					}
					this.charging.onStart += CS$<>8__locals1.<InitializeMotions>g__RepositLookingDirection|1;
					if (this.charged != null)
					{
						this.charged.onStart += CS$<>8__locals1.<InitializeMotions>g__RepositLookingDirection|1;
					}
					if (this.earlyFinish != null)
					{
						this.earlyFinish.onStart += CS$<>8__locals1.<InitializeMotions>g__RepositLookingDirection|1;
					}
					this.finish.onStart += CS$<>8__locals1.<InitializeMotions>g__RepositLookingDirection|1;
				}
				this.charging.onStart += delegate()
				{
					if (CS$<>8__locals1.<>4__this._earlyFinishReserved)
					{
						return;
					}
					CS$<>8__locals1.action.InvokeStartCharging();
				};
				this.charging.onCancel += CS$<>8__locals1.action.InvokeCancelCharging;
				if (this.charged == null)
				{
					this.charging.onEnd += CS$<>8__locals1.action.InvokeEndCharging;
					return;
				}
				this.charged.onEnd += CS$<>8__locals1.action.InvokeEndCharging;
				this.charged.onCancel += CS$<>8__locals1.action.InvokeCancelCharging;
			}

			// Token: 0x0600325D RID: 12893 RVA: 0x00095E00 File Offset: 0x00094000
			private void EarlyFinish()
			{
				this._earlyFinishReserved = false;
				this._action.DoMotion(this.earlyFinish);
				this.anticipation.onEnd -= this.EarlyFinish;
				if (this.prepare != null)
				{
					this.prepare.onEnd -= this.EarlyFinish;
				}
			}

			// Token: 0x0600325E RID: 12894 RVA: 0x00095E64 File Offset: 0x00094064
			public void ReserveEarlyFinish()
			{
				this._earlyFinishReserved = true;
				this.anticipation.onEnd -= this.EarlyFinish;
				this.anticipation.onEnd += this.EarlyFinish;
				if (this.prepare != null)
				{
					this.prepare.onEnd -= this.EarlyFinish;
					this.prepare.onEnd += this.EarlyFinish;
				}
			}

			// Token: 0x0400291F RID: 10527
			[MinMaxSlider(0f, 1f)]
			[Tooltip("fnish 모션을 캔슬하기 위한 입력 구간, 사용자의 입력이 input 범위 내이지만 cancel 보다 빠를 경우 선입력으로 예약됨")]
			public Vector2 input;

			// Token: 0x04002920 RID: 10528
			[Tooltip("finish 모션이 캔슬될 수 있는 구간")]
			[MinMaxSlider(0f, 1f)]
			public Vector2 cancel;

			// Token: 0x04002921 RID: 10529
			[MinMaxSlider(0f, 1f)]
			[Tooltip("earlyFinish 모션을 캔슬하기 위한 입력 구간, 사용자의 입력이 input 범위 내이지만 earlyCancel 보다 빠를 경우 선입력으로 예약됨")]
			public Vector2 earlyInput;

			// Token: 0x04002922 RID: 10530
			[Tooltip("earlyFinish 모션이 캔슬될 수 있는 구간")]
			[MinMaxSlider(0f, 1f)]
			public Vector2 earlyCancel;

			// Token: 0x04002923 RID: 10531
			[Space]
			[Subcomponent(typeof(Motion))]
			public Motion anticipation;

			// Token: 0x04002924 RID: 10532
			[Subcomponent(true, typeof(Motion))]
			public Motion prepare;

			// Token: 0x04002925 RID: 10533
			[Subcomponent(typeof(Motion))]
			public Motion charging;

			// Token: 0x04002926 RID: 10534
			[Subcomponent(true, typeof(Motion))]
			public Motion charged;

			// Token: 0x04002927 RID: 10535
			[Subcomponent(true, typeof(Motion))]
			public Motion earlyFinish;

			// Token: 0x04002928 RID: 10536
			[Subcomponent(typeof(Motion))]
			public Motion finish;

			// Token: 0x04002929 RID: 10537
			private bool _earlyFinishReserved;

			// Token: 0x0400292A RID: 10538
			private ChargeComboAction _action;

			// Token: 0x0400292B RID: 10539
			private Motion[] _motions;

			// Token: 0x02000926 RID: 2342
			[Serializable]
			internal class Reorderable : ReorderableArray<ChargeComboAction.ActionInfo>
			{
			}
		}
	}
}

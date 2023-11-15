using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000949 RID: 2377
	public class MultiChargeAction : Action
	{
		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x0600332C RID: 13100 RVA: 0x00097D37 File Offset: 0x00095F37
		public override Motion[] motions
		{
			get
			{
				return this._motions;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x0600332D RID: 13101 RVA: 0x00097D3F File Offset: 0x00095F3F
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._anticipation);
			}
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00097D69 File Offset: 0x00095F69
		protected override void Awake()
		{
			base.Awake();
			this.InitializeMotions();
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x00097D77 File Offset: 0x00095F77
		private void InvokeStartCharging()
		{
			if (this._earlyFinishReserved)
			{
				return;
			}
			Action<Action> onStartCharging = this._owner.onStartCharging;
			if (onStartCharging == null)
			{
				return;
			}
			onStartCharging(this);
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x0009531A File Offset: 0x0009351A
		private void InvokeEndCharging()
		{
			Action<Action> onStopCharging = this._owner.onStopCharging;
			if (onStopCharging == null)
			{
				return;
			}
			onStopCharging(this);
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x00095332 File Offset: 0x00093532
		private void InvokeCancelCharging()
		{
			Action<Action> onCancelCharging = this._owner.onCancelCharging;
			if (onCancelCharging == null)
			{
				return;
			}
			onCancelCharging(this);
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x00097D98 File Offset: 0x00095F98
		private void InitializeMotions()
		{
			List<Motion> list = new List<Motion>(8);
			list.Add(this._anticipation);
			if (this._prepare != null)
			{
				list.Add(this._prepare);
			}
			foreach (MultiChargeAction.ChargeMotions chargeMotions in this._chargeMotions.values)
			{
				list.Add(chargeMotions.charging);
				if (chargeMotions.charged != null)
				{
					list.Add(chargeMotions.charged);
				}
			}
			list.Add(this._chargeMotions.values.Last<MultiChargeAction.ChargeMotions>().finish);
			for (int j = 0; j < list.Count - 1; j++)
			{
				Motion nextMotion = list[j + 1];
				list[j].onEnd += delegate()
				{
					this.DoMotion(nextMotion);
				};
			}
			if (this._earlyFinish != null)
			{
				list.Add(this._earlyFinish);
			}
			foreach (Motion motion in list)
			{
				motion.Initialize(this);
			}
			if (this._anticipation.blockLook)
			{
				this._anticipation.onStart += delegate()
				{
					this._lookingDirection = this._owner.lookingDirection;
				};
				if (this._prepare != null)
				{
					this._prepare.onStart += this.<InitializeMotions>g__RepositLookingDirection|16_1;
				}
				if (this._earlyFinish != null)
				{
					this._earlyFinish.onStart += this.<InitializeMotions>g__RepositLookingDirection|16_1;
				}
				foreach (MultiChargeAction.ChargeMotions chargeMotions2 in this._chargeMotions.values)
				{
					chargeMotions2.charging.onStart += this.<InitializeMotions>g__RepositLookingDirection|16_1;
					if (chargeMotions2.charged != null)
					{
						chargeMotions2.charged.onStart += this.<InitializeMotions>g__RepositLookingDirection|16_1;
					}
					chargeMotions2.finish.onStart += this.<InitializeMotions>g__RepositLookingDirection|16_1;
					list.Add(chargeMotions2.finish);
					chargeMotions2.finish.Initialize(this);
				}
			}
			for (int k = 0; k < this._chargeMotions.values.Length; k++)
			{
				MultiChargeAction.ChargeMotions chargeMotions3 = this._chargeMotions.values[k];
				Motion x = null;
				if (k + 1 < this._chargeMotions.values.Length)
				{
					x = this._chargeMotions.values[k + 1].charging;
				}
				if (k == 0)
				{
					chargeMotions3.charging.onStart += this.InvokeStartCharging;
				}
				chargeMotions3.charging.onCancel += this.InvokeCancelCharging;
				if (chargeMotions3.charged == null)
				{
					if (x == null)
					{
						chargeMotions3.charging.onEnd += this.InvokeEndCharging;
					}
				}
				else
				{
					chargeMotions3.charged.onEnd += this.InvokeEndCharging;
					chargeMotions3.charged.onCancel += this.InvokeCancelCharging;
				}
			}
			this._motions = list.ToArray();
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000980E0 File Offset: 0x000962E0
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._anticipation);
			return true;
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x00098110 File Offset: 0x00096310
		private void EarlyFinish()
		{
			this._earlyFinishReserved = false;
			base.DoMotion(this._earlyFinish);
			this._anticipation.onEnd -= this.EarlyFinish;
			if (this._prepare != null)
			{
				this._prepare.onEnd -= this.EarlyFinish;
			}
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x0009816C File Offset: 0x0009636C
		public void ReserveEarlyFinish()
		{
			this._earlyFinishReserved = true;
			this._anticipation.onEnd -= this.EarlyFinish;
			this._anticipation.onEnd += this.EarlyFinish;
			if (this._prepare != null)
			{
				this._prepare.onEnd -= this.EarlyFinish;
				this._prepare.onEnd += this.EarlyFinish;
			}
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000981EC File Offset: 0x000963EC
		public override bool TryEnd()
		{
			Motion runningMotion = base.owner.runningMotion;
			if (runningMotion == null || runningMotion.action != this)
			{
				return false;
			}
			if (runningMotion == this._earlyFinish)
			{
				return false;
			}
			if (this._earlyFinish != null && (runningMotion == this._anticipation || runningMotion == this._prepare))
			{
				this.ReserveEarlyFinish();
				return false;
			}
			for (int i = 0; i < this._chargeMotions.values.Length; i++)
			{
				MultiChargeAction.ChargeMotions chargeMotions = this._chargeMotions.values[i];
				if (runningMotion == chargeMotions.finish)
				{
					return false;
				}
				if (runningMotion == chargeMotions.charging)
				{
					if (i == 0)
					{
						if (this._earlyFinish == null)
						{
							return false;
						}
						base.DoMotion(this._earlyFinish);
					}
					else
					{
						base.DoMotion(this._chargeMotions.values[i - 1].finish);
					}
					return true;
				}
				if (runningMotion == chargeMotions.charged)
				{
					base.DoMotion(chargeMotions.finish);
					return true;
				}
			}
			base.owner.CancelAction();
			return false;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x00098322 File Offset: 0x00096522
		[CompilerGenerated]
		private void <InitializeMotions>g__RepositLookingDirection|16_1()
		{
			this._owner.ForceToLookAt(this._lookingDirection);
		}

		// Token: 0x040029AD RID: 10669
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _anticipation;

		// Token: 0x040029AE RID: 10670
		[SerializeField]
		[Subcomponent(true, typeof(Motion))]
		protected Motion _prepare;

		// Token: 0x040029AF RID: 10671
		[SerializeField]
		[Subcomponent(true, typeof(Motion))]
		protected Motion _earlyFinish;

		// Token: 0x040029B0 RID: 10672
		[Space]
		[SerializeField]
		protected MultiChargeAction.ChargeMotions.Reorderable _chargeMotions;

		// Token: 0x040029B1 RID: 10673
		private Character.LookingDirection _lookingDirection;

		// Token: 0x040029B2 RID: 10674
		protected Motion[] _motions;

		// Token: 0x040029B3 RID: 10675
		private bool _earlyFinishReserved;

		// Token: 0x0200094A RID: 2378
		[Serializable]
		protected class ChargeMotions
		{
			// Token: 0x040029B4 RID: 10676
			[Space]
			[Subcomponent(typeof(Motion))]
			public Motion charging;

			// Token: 0x040029B5 RID: 10677
			[Subcomponent(true, typeof(Motion))]
			public Motion charged;

			// Token: 0x040029B6 RID: 10678
			[Subcomponent(typeof(Motion))]
			public Motion finish;

			// Token: 0x0200094B RID: 2379
			[Serializable]
			public class Reorderable : ReorderableArray<MultiChargeAction.ChargeMotions>
			{
			}
		}
	}
}

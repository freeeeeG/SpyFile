using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000922 RID: 2338
	public class ChargeAction : Action
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x00095256 File Offset: 0x00093456
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._anticipation,
					this._prepare,
					this._charging,
					this._charged,
					this._earlyFinish,
					this._finish
				};
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x00095294 File Offset: 0x00093494
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._anticipation);
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x000952BE File Offset: 0x000934BE
		public float chargedPercent
		{
			get
			{
				return this._charging.normalizedTime;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x000952CB File Offset: 0x000934CB
		public float chargingPercent
		{
			get
			{
				if (!this._charging.running)
				{
					return 0f;
				}
				return this._charging.normalizedTime;
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000952EB File Offset: 0x000934EB
		protected override void Awake()
		{
			base.Awake();
			this.InitializeMotions();
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000952F9 File Offset: 0x000934F9
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

		// Token: 0x06003244 RID: 12868 RVA: 0x0009531A File Offset: 0x0009351A
		private void InvokeEndCharging()
		{
			Action<Action> onStopCharging = this._owner.onStopCharging;
			if (onStopCharging == null)
			{
				return;
			}
			onStopCharging(this);
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x00095332 File Offset: 0x00093532
		private void InvokeCancelCharging()
		{
			Action<Action> onCancelCharging = this._owner.onCancelCharging;
			if (onCancelCharging == null)
			{
				return;
			}
			onCancelCharging(this);
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0009534C File Offset: 0x0009354C
		private void InitializeMotions()
		{
			List<Motion> list = new List<Motion>(6);
			list.Add(this._anticipation);
			if (this._prepare != null)
			{
				list.Add(this._prepare);
			}
			list.Add(this._charging);
			if (this._charged != null)
			{
				list.Add(this._charged);
			}
			list.Add(this._finish);
			for (int i = 0; i < list.Count - 1; i++)
			{
				Motion nextMotion = list[i + 1];
				list[i].onEnd += delegate()
				{
					this.DoMotion(nextMotion);
				};
			}
			if (this._earlyFinish != null)
			{
				list.Add(this._earlyFinish);
			}
			this._motions = list.ToArray();
			Motion[] motions = this._motions;
			for (int j = 0; j < motions.Length; j++)
			{
				motions[j].Initialize(this);
			}
			if (this._anticipation.blockLook)
			{
				this._anticipation.onStart += delegate()
				{
					this._lookingDirection = this._owner.lookingDirection;
				};
				if (this._prepare != null)
				{
					this._prepare.onStart += this.<InitializeMotions>g__RepositLookingDirection|22_1;
				}
				this._charging.onStart += this.<InitializeMotions>g__RepositLookingDirection|22_1;
				if (this._charged != null)
				{
					this._charged.onStart += this.<InitializeMotions>g__RepositLookingDirection|22_1;
				}
				if (this._earlyFinish != null)
				{
					this._earlyFinish.onStart += this.<InitializeMotions>g__RepositLookingDirection|22_1;
				}
				this._finish.onStart += this.<InitializeMotions>g__RepositLookingDirection|22_1;
			}
			this._charging.onStart += this.InvokeStartCharging;
			this._charging.onCancel += this.InvokeCancelCharging;
			if (this._charged == null)
			{
				this._charging.onEnd += this.InvokeEndCharging;
				return;
			}
			this._charged.onEnd += this.InvokeEndCharging;
			this._charged.onCancel += this.InvokeCancelCharging;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x0009558E File Offset: 0x0009378E
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._anticipation);
			return true;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000955BC File Offset: 0x000937BC
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

		// Token: 0x06003249 RID: 12873 RVA: 0x00095618 File Offset: 0x00093818
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

		// Token: 0x0600324A RID: 12874 RVA: 0x00095698 File Offset: 0x00093898
		public override bool TryEnd()
		{
			if (base.owner.motion == this._finish || base.owner.motion == this._earlyFinish)
			{
				return false;
			}
			if (this._charged != null && base.owner.motion == this._charged)
			{
				base.DoMotion(this._finish);
				return true;
			}
			if (!(this._earlyFinish != null) || !(base.owner.motion != this._earlyFinish) || !(base.owner.motion != this._finish))
			{
				base.owner.CancelAction();
				return false;
			}
			if (base.owner.motion == this._anticipation || base.owner.motion == this._prepare)
			{
				this.ReserveEarlyFinish();
				return false;
			}
			base.DoMotion(this._earlyFinish);
			return true;
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000957AE File Offset: 0x000939AE
		[CompilerGenerated]
		private void <InitializeMotions>g__RepositLookingDirection|22_1()
		{
			this._owner.ForceToLookAt(this._lookingDirection);
		}

		// Token: 0x0400290C RID: 10508
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion _anticipation;

		// Token: 0x0400290D RID: 10509
		[Subcomponent(true, typeof(Motion))]
		[SerializeField]
		protected Motion _prepare;

		// Token: 0x0400290E RID: 10510
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _charging;

		// Token: 0x0400290F RID: 10511
		[SerializeField]
		[Subcomponent(true, typeof(Motion))]
		protected Motion _charged;

		// Token: 0x04002910 RID: 10512
		[Subcomponent(true, typeof(Motion))]
		[SerializeField]
		protected Motion _earlyFinish;

		// Token: 0x04002911 RID: 10513
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _finish;

		// Token: 0x04002912 RID: 10514
		private Character.LookingDirection _lookingDirection;

		// Token: 0x04002913 RID: 10515
		protected Motion[] _motions;

		// Token: 0x04002914 RID: 10516
		private bool _earlyFinishReserved;

		// Token: 0x04002915 RID: 10517
		private bool _isCharging;
	}
}

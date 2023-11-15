using System;
using Characters.Gear.Weapons;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000939 RID: 2361
	public class GhoulHookAction : Action
	{
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x060032B2 RID: 12978 RVA: 0x00096AED File Offset: 0x00094CED
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._fire,
					this._pull,
					this._fly
				};
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x00096B10 File Offset: 0x00094D10
		public Motion motion
		{
			get
			{
				return this._fire;
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x060032B4 RID: 12980 RVA: 0x00096B18 File Offset: 0x00094D18
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._fire);
			}
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00096B44 File Offset: 0x00094D44
		protected override void Awake()
		{
			base.Awake();
			this._hook.onTerrainHit += this.OnHookTerrainHit;
			this._hook.onExpired += this.OnHookExpired;
			this._hook.onPullEnd += this.OnHookPullEnd;
			this._hook.onFlyEnd += this.OnHookFlyEnd;
			if (this._fire.blockLook)
			{
				this._fire.onStart += delegate()
				{
					this._lookingDirection = base.owner.lookingDirection;
				};
				this._pull.onStart += delegate()
				{
					base.owner.lookingDirection = this._lookingDirection;
				};
				if (this._consume != null)
				{
					this._hook.onPullEnd += delegate()
					{
						this._consume.TryStart();
					};
					this._consume.onStart += delegate()
					{
						base.owner.lookingDirection = this._lookingDirection;
					};
				}
				this._fly.onStart += delegate()
				{
					base.owner.lookingDirection = this._lookingDirection;
				};
			}
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00096C44 File Offset: 0x00094E44
		private void OnHookExpired()
		{
			base.DoMotion(this._pull);
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00096C52 File Offset: 0x00094E52
		private void OnHookTerrainHit()
		{
			base.DoMotion(this._fly);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x00096C60 File Offset: 0x00094E60
		private void OnHookPullEnd()
		{
			base.owner.CancelAction();
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x00096C60 File Offset: 0x00094E60
		private void OnHookFlyEnd()
		{
			base.owner.CancelAction();
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00096C6D File Offset: 0x00094E6D
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._fire.Initialize(this);
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x00096C82 File Offset: 0x00094E82
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._fire);
			return true;
		}

		// Token: 0x0400295C RID: 10588
		[SerializeField]
		private GhoulHook _hook;

		// Token: 0x0400295D RID: 10589
		[Header("Motion")]
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		private Motion _fire;

		// Token: 0x0400295E RID: 10590
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		private Motion _pull;

		// Token: 0x0400295F RID: 10591
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		private Motion _fly;

		// Token: 0x04002960 RID: 10592
		[SerializeField]
		private Action _consume;

		// Token: 0x04002961 RID: 10593
		private Character.LookingDirection _lookingDirection;
	}
}

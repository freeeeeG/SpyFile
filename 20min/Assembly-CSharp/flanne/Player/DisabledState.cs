using System;

namespace flanne.Player
{
	// Token: 0x02000160 RID: 352
	public class DisabledState : PlayerState
	{
		// Token: 0x060008F3 RID: 2291 RVA: 0x000254F8 File Offset: 0x000236F8
		private void OnDisableToggleChange(object sender, bool isDisabled)
		{
			if (!isDisabled)
			{
				if (base.ammo.outOfAmmo)
				{
					this.owner.ChangeState<ReloadState>();
					return;
				}
				if (base.playerInput.actions["Fire"].ReadValue<float>() != 0f)
				{
					this.owner.ChangeState<ShootingState>();
					return;
				}
				this.owner.ChangeState<IdleState>();
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002555C File Offset: 0x0002375C
		public override void Enter()
		{
			base.gun.StopShooting();
			this.owner.disableAction.ToggleEvent += this.OnDisableToggleChange;
			this.owner.moveSpeedMultiplier = 1f;
			this.owner.faceMouse = false;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000255AC File Offset: 0x000237AC
		public override void Exit()
		{
			this.owner.disableAction.ToggleEvent -= this.OnDisableToggleChange;
		}
	}
}

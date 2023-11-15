using System;
using System.Collections;
using flanne.Core;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.Player
{
	// Token: 0x02000161 RID: 353
	public class IdleState : PlayerState
	{
		// Token: 0x060008F7 RID: 2295 RVA: 0x000255D2 File Offset: 0x000237D2
		private void OnFireAction(InputAction.CallbackContext obj)
		{
			if (PauseController.isPaused)
			{
				return;
			}
			if (base.ammo.outOfAmmo)
			{
				this.owner.ChangeState<ReloadState>();
				return;
			}
			this.owner.ChangeState<ShootingState>();
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00025600 File Offset: 0x00023800
		private void OnReloadAction(InputAction.CallbackContext obj)
		{
			if (PauseController.isPaused)
			{
				return;
			}
			if (!base.ammo.fullOnAmmo && !base.gun.gunData.disableManualReload)
			{
				this.owner.ChangeState<ReloadState>();
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00025634 File Offset: 0x00023834
		private void OnAmmoChange(int amountChanged)
		{
			if (base.ammo.outOfAmmo)
			{
				this.owner.ChangeState<ReloadState>();
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002564E File Offset: 0x0002384E
		private void OnDisableToggleChange(object sender, bool isDisabled)
		{
			if (isDisabled)
			{
				this.owner.ChangeState<DisabledState>();
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00025660 File Offset: 0x00023860
		public override void Enter()
		{
			base.gun.StopShooting();
			base.playerInput.actions["Fire"].performed += this.OnFireAction;
			base.playerInput.actions["Reload"].started += this.OnReloadAction;
			base.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChange));
			this.owner.disableAction.ToggleEvent += this.OnDisableToggleChange;
			this.owner.moveSpeedMultiplier = 1f;
			this.owner.faceMouse = false;
			if (base.ammo.outOfAmmo && base.gun.gunData != null)
			{
				base.StartCoroutine(this.WaitToExitToReloadStateCR());
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00025748 File Offset: 0x00023948
		public override void Exit()
		{
			base.playerInput.actions["Fire"].performed -= this.OnFireAction;
			base.playerInput.actions["Reload"].started -= this.OnReloadAction;
			base.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChange));
			this.owner.disableAction.ToggleEvent -= this.OnDisableToggleChange;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000257D9 File Offset: 0x000239D9
		private IEnumerator WaitToExitToReloadStateCR()
		{
			yield return null;
			this.owner.ChangeState<ReloadState>();
			yield break;
		}
	}
}

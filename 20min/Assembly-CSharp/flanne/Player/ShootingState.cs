using System;
using System.Collections;
using flanne.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.Player
{
	// Token: 0x02000164 RID: 356
	public class ShootingState : PlayerState
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x00025ADE File Offset: 0x00023CDE
		private void OnAmmoChanged(int amount)
		{
			if (amount == 0 && this._changeStateCoroutine == null)
			{
				this._changeStateCoroutine = this.WaitToChangeState<ReloadState>();
				base.StartCoroutine(this._changeStateCoroutine);
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00025B04 File Offset: 0x00023D04
		private void OnReloadAction(InputAction.CallbackContext obj)
		{
			if (!base.gun.gunData.disableManualReload)
			{
				this.owner.ChangeState<ReloadState>();
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00025B24 File Offset: 0x00023D24
		private void OnCancelFire(InputAction.CallbackContext obj)
		{
			if (base.ammo.outOfAmmo || (OptionsSetter.AutoReloadEnabled && !base.gun.gunData.disableManualReload))
			{
				this._changeStateCoroutine = this.WaitToChangeState<ReloadState>();
			}
			else
			{
				this._changeStateCoroutine = this.WaitToChangeState<IdleState>();
			}
			base.StartCoroutine(this._changeStateCoroutine);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00025B7E File Offset: 0x00023D7E
		private void OnDisableToggleChange(object sender, bool isDisabled)
		{
			if (isDisabled)
			{
				if (this._changeStateCoroutine != null)
				{
					base.StopCoroutine(this._changeStateCoroutine);
					this._changeStateCoroutine = null;
				}
				this.owner.ChangeState<DisabledState>();
			}
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00025BAC File Offset: 0x00023DAC
		public override void Enter()
		{
			base.playerInput.actions["Fire"].canceled += this.OnCancelFire;
			base.playerInput.actions["Reload"].started += this.OnReloadAction;
			base.playerInput.actions["Aim"].canceled += this.OnCancelFire;
			base.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
			this.owner.disableAction.ToggleEvent += this.OnDisableToggleChange;
			base.gun.StartShooting();
			this.owner.moveSpeedMultiplier = Mathf.Min(1f, base.stats[StatType.WalkSpeed].Modify(0.35f));
			this.owner.faceMouse = true;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00025CA8 File Offset: 0x00023EA8
		public override void Exit()
		{
			base.playerInput.actions["Fire"].canceled -= this.OnCancelFire;
			base.playerInput.actions["Reload"].started -= this.OnReloadAction;
			base.playerInput.actions["Aim"].canceled -= this.OnCancelFire;
			base.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
			this.owner.disableAction.ToggleEvent -= this.OnDisableToggleChange;
			base.gun.StopShooting();
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00025D6A File Offset: 0x00023F6A
		private IEnumerator WaitToChangeState<T>() where T : PlayerState
		{
			base.playerInput.actions["Fire"].canceled -= this.OnCancelFire;
			base.playerInput.actions["Reload"].started -= this.OnReloadAction;
			base.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
			while (!base.gun.shotReady)
			{
				yield return null;
			}
			this.owner.ChangeState<T>();
			this._changeStateCoroutine = null;
			yield break;
		}

		// Token: 0x040006A8 RID: 1704
		private IEnumerator _changeStateCoroutine;
	}
}

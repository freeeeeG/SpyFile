using System;
using System.Collections;
using flanne.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.Player
{
	// Token: 0x02000163 RID: 355
	public class ReloadState : PlayerState
	{
		// Token: 0x06000908 RID: 2312 RVA: 0x0002585C File Offset: 0x00023A5C
		private void OnFireAction(InputAction.CallbackContext obj)
		{
			if (!base.ammo.outOfAmmo && !PauseController.isPaused)
			{
				this.owner.ChangeState<ShootingState>();
				if (this.reloadCoroutine != null)
				{
					base.StopCoroutine(this.reloadCoroutine);
					this.reloadCoroutine = null;
				}
				base.reloadBar.transform.parent.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000258C0 File Offset: 0x00023AC0
		private void OnDisableToggleChange(object sender, bool isDisabled)
		{
			if (isDisabled)
			{
				this.owner.ChangeState<DisabledState>();
				if (this.reloadCoroutine != null)
				{
					base.StopCoroutine(this.reloadCoroutine);
					this.reloadCoroutine = null;
				}
				base.reloadBar.transform.parent.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00025911 File Offset: 0x00023B11
		private void OnAmmoChanged(int currentAmmo)
		{
			if (currentAmmo > 0 && base.playerInput.actions["Fire"].ReadValue<float>() == 1f)
			{
				this.owner.ChangeState<ShootingState>();
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00025944 File Offset: 0x00023B44
		public override void Enter()
		{
			base.gun.StopShooting();
			base.playerInput.actions["Fire"].started += this.OnFireAction;
			this.owner.disableAction.ToggleEvent += this.OnDisableToggleChange;
			base.ammo.OnAmmoChanged.AddListener(new UnityAction<int>(this.OnAmmoChanged));
			this.owner.moveSpeedMultiplier = 1f;
			this.owner.faceMouse = false;
			if (this.reloadCoroutine == null)
			{
				this.reloadCoroutine = this.ReloadCR();
				base.StartCoroutine(this.reloadCoroutine);
			}
			base.gun.SetAnimationTrigger("Reload");
			if (this.owner.gun.gunData.reloadSFXOverride == null)
			{
				this.owner.reloadStartSFX.Play(null);
				return;
			}
			this.owner.gun.gunData.reloadSFXOverride.Play(null);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00025A54 File Offset: 0x00023C54
		public override void Exit()
		{
			base.playerInput.actions["Fire"].started -= this.OnFireAction;
			this.owner.disableAction.ToggleEvent -= this.OnDisableToggleChange;
			base.ammo.OnAmmoChanged.RemoveListener(new UnityAction<int>(this.OnAmmoChanged));
			base.gun.SetAnimationTrigger("Still");
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00025ACF File Offset: 0x00023CCF
		private IEnumerator ReloadCR()
		{
			base.reloadBar.transform.parent.gameObject.SetActive(true);
			float timer = 0f;
			float reloadDuration = base.gun.reloadDuration;
			while (timer < reloadDuration)
			{
				base.reloadBar.value = timer / reloadDuration;
				yield return null;
				timer += Time.deltaTime;
			}
			base.ammo.Reload();
			this.owner.reloadEndSFX.Play(null);
			if (base.playerInput.actions["Fire"].ReadValue<float>() != 0f)
			{
				this.owner.ChangeState<ShootingState>();
			}
			else
			{
				this.owner.ChangeState<IdleState>();
			}
			base.reloadBar.transform.parent.gameObject.SetActive(false);
			this.reloadCoroutine = null;
			yield break;
		}

		// Token: 0x040006A7 RID: 1703
		private IEnumerator reloadCoroutine;
	}
}

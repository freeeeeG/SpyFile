using System;
using flanne.Pickups;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace flanne.Core
{
	// Token: 0x020001EF RID: 495
	public class CombatState : GameState
	{
		// Token: 0x06000B25 RID: 2853 RVA: 0x00029FA2 File Offset: 0x000281A2
		private void OnLevelUP(int level)
		{
			if (level == 20)
			{
				this.owner.ChangeState<GunEvoMenuState>();
				return;
			}
			base.levelUpSFX.Play(null);
			this.owner.ChangeState<PowerupMenuState>();
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00029FCD File Offset: 0x000281CD
		private void OnDeath()
		{
			this.owner.ChangeState<PlayerDeadState>();
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00029FDA File Offset: 0x000281DA
		private void OnChestPickup(object sender, object args)
		{
			this.owner.ChangeState<ChestState>();
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00029FE7 File Offset: 0x000281E7
		private void OnDevilDealPickup(object sender, object args)
		{
			this.owner.ChangeState<DevilDealState>();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00029FF4 File Offset: 0x000281F4
		private void OnHaloPickup(object sender, object args)
		{
			this.owner.ChangeState<ShanaHaloState>();
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0002A001 File Offset: 0x00028201
		private void OnTimerReached(object sender, object args)
		{
			this.owner.ChangeState<KillEnemiesState>();
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0002A00E File Offset: 0x0002820E
		private void OnPauseAction(InputAction.CallbackContext obj)
		{
			base.pauseController.Pause();
			this.owner.ChangeState<PauseState>();
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0002A028 File Offset: 0x00028228
		public override void Enter()
		{
			base.playerXP.OnLevelChanged.AddListener(new UnityAction<int>(this.OnLevelUP));
			base.playerHealth.onDeath.AddListener(new UnityAction(this.OnDeath));
			this.AddObserver(new Action<object, object>(this.OnChestPickup), ChestPickup.ChestPickupEvent);
			this.AddObserver(new Action<object, object>(this.OnDevilDealPickup), DevilDealPickup.DevilDealPickupEvent);
			this.AddObserver(new Action<object, object>(this.OnHaloPickup), HaloPickup.HaloPickupEvent);
			this.AddObserver(new Action<object, object>(this.OnTimerReached), GameTimer.TimeLimitNotification);
			base.playerInput.actions["Pause"].started += this.OnPauseAction;
			base.mouseAmmoUI.Show();
			base.shootingCursor.EnableGamepadCusor();
			EventSystem.current.SetSelectedGameObject(null);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0002A110 File Offset: 0x00028310
		public override void Exit()
		{
			base.playerXP.OnLevelChanged.RemoveListener(new UnityAction<int>(this.OnLevelUP));
			base.playerHealth.onDeath.RemoveListener(new UnityAction(this.OnDeath));
			this.RemoveObserver(new Action<object, object>(this.OnChestPickup), ChestPickup.ChestPickupEvent);
			this.RemoveObserver(new Action<object, object>(this.OnDevilDealPickup), DevilDealPickup.DevilDealPickupEvent);
			this.RemoveObserver(new Action<object, object>(this.OnHaloPickup), HaloPickup.HaloPickupEvent);
			this.RemoveObserver(new Action<object, object>(this.OnTimerReached), GameTimer.TimeLimitNotification);
			base.playerInput.actions["Pause"].started -= this.OnPauseAction;
			base.mouseAmmoUI.Hide();
			base.shootingCursor.DisableGamepadCursor();
		}
	}
}

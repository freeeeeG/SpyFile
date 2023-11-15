using System;
using flanne.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne
{
	// Token: 0x0200010F RID: 271
	public class ShootDetector : MonoBehaviour
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00020E96 File Offset: 0x0001F096
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00020E9E File Offset: 0x0001F09E
		public bool usedShooting { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00020EA7 File Offset: 0x0001F0A7
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00020EAF File Offset: 0x0001F0AF
		public bool usedManualShooting { get; private set; }

		// Token: 0x0600079C RID: 1948 RVA: 0x00020EB8 File Offset: 0x0001F0B8
		private void OnShoot()
		{
			this.usedShooting = true;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00020EC1 File Offset: 0x0001F0C1
		private void OnManualShoot(InputAction.CallbackContext context)
		{
			if (!PauseController.isPaused)
			{
				this.usedManualShooting = true;
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00020ED4 File Offset: 0x0001F0D4
		private void Start()
		{
			this.usedShooting = false;
			this.usedManualShooting = false;
			this.playerGun.OnShoot.AddListener(new UnityAction(this.OnShoot));
			this._shootAction = this.inputs.FindActionMap("PlayerMap", false).FindAction("Fire", false);
			this._shootAction.performed += this.OnManualShoot;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00020F44 File Offset: 0x0001F144
		private void OnDestroy()
		{
			this.playerGun.OnShoot.RemoveListener(new UnityAction(this.OnShoot));
			this._shootAction.performed -= this.OnManualShoot;
		}

		// Token: 0x04000578 RID: 1400
		[SerializeField]
		private Gun playerGun;

		// Token: 0x04000579 RID: 1401
		[SerializeField]
		private InputActionAsset inputs;

		// Token: 0x0400057A RID: 1402
		private InputAction _shootAction;
	}
}

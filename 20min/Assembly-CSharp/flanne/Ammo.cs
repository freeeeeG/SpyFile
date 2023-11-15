using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000B7 RID: 183
	public class Ammo : MonoBehaviour
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001B54D File Offset: 0x0001974D
		public bool outOfAmmo
		{
			get
			{
				return this.amount == 0;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001B558 File Offset: 0x00019758
		public bool fullOnAmmo
		{
			get
			{
				return this.amount == this.gun.maxAmmo;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001B56D File Offset: 0x0001976D
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001B575 File Offset: 0x00019775
		public int amount { get; private set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001B57E File Offset: 0x0001977E
		public int max
		{
			get
			{
				return this.gun.maxAmmo;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001B58C File Offset: 0x0001978C
		private void Start()
		{
			this.infiniteAmmo = new BoolToggle(false);
			this.Reload();
			this.OnAmmoChanged.Invoke(this.amount);
			this.OnMaxAmmoChanged.Invoke(this.gun.maxAmmo);
			this.gun.stats[StatType.MaxAmmo].ChangedEvent += this.AmmoModChanged;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001B5F4 File Offset: 0x000197F4
		private void OnDestroy()
		{
			this.gun.stats[StatType.MaxAmmo].ChangedEvent -= this.AmmoModChanged;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001B618 File Offset: 0x00019818
		public void Reload()
		{
			this.amount = this.gun.maxAmmo;
			this.OnAmmoChanged.Invoke(this.amount);
			this.OnReload.Invoke();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001B648 File Offset: 0x00019848
		public void UseAmmo(int a = 1)
		{
			if (this.infiniteAmmo.value)
			{
				return;
			}
			BaseException ex = new BaseException(true);
			this.PostNotification(Ammo.ShouldConsumeAmmoCheck, ex);
			if (!ex.toggle)
			{
				return;
			}
			this.amount -= a;
			this.amount = Mathf.Clamp(this.amount, 0, this.gun.maxAmmo);
			this.OnAmmoChanged.Invoke(this.amount);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0001B6BC File Offset: 0x000198BC
		public void GainAmmo(int value = 1)
		{
			this.amount += value;
			this.amount = Mathf.Clamp(this.amount, 0, this.gun.maxAmmo);
			this.OnAmmoChanged.Invoke(this.amount);
			this.OnAmmoGained.Invoke();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001B710 File Offset: 0x00019910
		public void AmmoModChanged(object sender, EventArgs e)
		{
			this.OnMaxAmmoChanged.Invoke(this.gun.maxAmmo);
		}

		// Token: 0x040003B5 RID: 949
		public static string ShouldConsumeAmmoCheck = "Ammo.ShouldConsumeAmmo";

		// Token: 0x040003B6 RID: 950
		[SerializeField]
		private Gun gun;

		// Token: 0x040003B7 RID: 951
		public UnityEvent OnReload;

		// Token: 0x040003B8 RID: 952
		public UnityEvent OnAmmoGained;

		// Token: 0x040003B9 RID: 953
		public UnityIntEvent OnAmmoChanged;

		// Token: 0x040003BA RID: 954
		public UnityIntEvent OnMaxAmmoChanged;

		// Token: 0x040003BC RID: 956
		public BoolToggle infiniteAmmo;
	}
}

using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using GameResources;
using Services;
using Singletons;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001DA RID: 474
	public sealed class GetWeapon : Sequence
	{
		// Token: 0x060009D5 RID: 2517 RVA: 0x0001B978 File Offset: 0x00019B78
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			WeaponReference weaponByName = Singleton<Service>.Instance.gearManager.GetWeaponByName("ChiefGuard");
			this._request = weaponByName.LoadAsync();
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0001B9BB File Offset: 0x00019BBB
		public override IEnumerator CRun()
		{
			WeaponInventory inventory = this._player.GetComponent<WeaponInventory>();
			Skul skul = inventory.polymorphOrCurrent.GetComponent<Skul>();
			skul.getSkul.TryStart();
			yield return null;
			base.StartCoroutine(this.CGet(skul, inventory));
			yield break;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001B9CA File Offset: 0x00019BCA
		private IEnumerator CGet(Skul skul, WeaponInventory inventory)
		{
			while (skul.getSkul.running)
			{
				yield return null;
			}
			while (this._request == null || !this._request.isDone)
			{
				yield return null;
			}
			Weapon asset = this._request.asset;
			inventory.ForceEquip(asset.Instantiate());
			yield break;
		}

		// Token: 0x04000802 RID: 2050
		private Character _player;

		// Token: 0x04000803 RID: 2051
		private const string weaponName = "ChiefGuard";

		// Token: 0x04000804 RID: 2052
		private WeaponRequest _request;
	}
}

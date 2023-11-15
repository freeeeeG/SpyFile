using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001D5 RID: 469
	public class GetBone : Sequence
	{
		// Token: 0x060009BC RID: 2492 RVA: 0x0001B726 File Offset: 0x00019926
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0001B73D File Offset: 0x0001993D
		public override IEnumerator CRun()
		{
			this._inventory = this._player.GetComponent<WeaponInventory>();
			this._tutorialSkul = this._inventory.polymorphOrCurrent.GetComponent<TutorialSkul>();
			this._tutorialSkul.getBone.TryStart();
			yield return Chronometer.global.WaitForSeconds(0.5f);
			base.StartCoroutine(this.CGetWeapon());
			yield break;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0001B74C File Offset: 0x0001994C
		private IEnumerator CGetWeapon()
		{
			while (this._tutorialSkul.getBone.running)
			{
				yield return null;
			}
			Singleton<Service>.Instance.levelManager.player.GetComponent<WeaponInventory>().ForceEquipAt(this._skul.Instantiate(), 0);
			this._inventory.polymorphOrCurrent.RemoveSkill(1);
			yield break;
		}

		// Token: 0x040007F3 RID: 2035
		[SerializeField]
		private Weapon _skul;

		// Token: 0x040007F4 RID: 2036
		private TutorialSkul _tutorialSkul;

		// Token: 0x040007F5 RID: 2037
		private WeaponInventory _inventory;

		// Token: 0x040007F6 RID: 2038
		private Character _player;
	}
}

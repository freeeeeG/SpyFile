using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001D8 RID: 472
	public class GetScroll : Sequence
	{
		// Token: 0x060009CC RID: 2508 RVA: 0x0001B8B1 File Offset: 0x00019AB1
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0001B8C8 File Offset: 0x00019AC8
		public override IEnumerator CRun()
		{
			WeaponInventory component = this._player.GetComponent<WeaponInventory>();
			Skul skul = component.polymorphOrCurrent.GetComponent<Skul>();
			skul.getScroll.TryStart();
			while (skul.getScroll.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x040007FD RID: 2045
		private Character _player;
	}
}

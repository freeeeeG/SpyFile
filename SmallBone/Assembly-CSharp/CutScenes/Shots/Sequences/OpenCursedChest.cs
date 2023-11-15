using System;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001DD RID: 477
	public sealed class OpenCursedChest : Event
	{
		// Token: 0x060009E5 RID: 2533 RVA: 0x0001BB59 File Offset: 0x00019D59
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0001BB70 File Offset: 0x00019D70
		public override void Run()
		{
			Prisoner2 component = this._player.GetComponent<WeaponInventory>().polymorphOrCurrent.GetComponent<Prisoner2>();
			if (component == null)
			{
				return;
			}
			base.StartCoroutine(component.COpenCursedChest());
		}

		// Token: 0x0400080F RID: 2063
		private Character _player;
	}
}

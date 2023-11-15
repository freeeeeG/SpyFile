using System;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;

namespace CutScenes.Shots.Sequences
{
	// Token: 0x020001E0 RID: 480
	public sealed class OpenPrisonerChest : Event
	{
		// Token: 0x060009F2 RID: 2546 RVA: 0x0001BD2C File Offset: 0x00019F2C
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001BD44 File Offset: 0x00019F44
		public override void Run()
		{
			Prisoner2 component = this._player.GetComponent<WeaponInventory>().polymorphOrCurrent.GetComponent<Prisoner2>();
			if (component == null)
			{
				return;
			}
			base.StartCoroutine(component.COpenChest());
		}

		// Token: 0x0400081C RID: 2076
		private Character _player;
	}
}

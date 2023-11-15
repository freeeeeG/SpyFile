using System;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x02000205 RID: 517
	public class PlayTutorialSkulAnimation : Event
	{
		// Token: 0x06000A78 RID: 2680 RVA: 0x0001CB52 File Offset: 0x0001AD52
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0001CB6C File Offset: 0x0001AD6C
		public override void Run()
		{
			this._inventory = this._player.GetComponent<WeaponInventory>();
			this._skul = this._inventory.polymorphOrCurrent.GetComponent<TutorialSkul>();
			switch (this._type)
			{
			case PlayTutorialSkulAnimation.Type.Idle:
				this._skul.idle.TryStart();
				return;
			case PlayTutorialSkulAnimation.Type.OpenEyes:
				this._skul.openEyes.TryStart();
				return;
			case PlayTutorialSkulAnimation.Type.EquipHead:
				this._skul.equipHead.TryStart();
				return;
			case PlayTutorialSkulAnimation.Type.ScratchHead:
				this._skul.scratchHead.TryStart();
				return;
			case PlayTutorialSkulAnimation.Type.Blink:
				this._skul.blink.TryStart();
				return;
			default:
				return;
			}
		}

		// Token: 0x0400088C RID: 2188
		[SerializeField]
		private PlayTutorialSkulAnimation.Type _type;

		// Token: 0x0400088D RID: 2189
		private TutorialSkul _skul;

		// Token: 0x0400088E RID: 2190
		private WeaponInventory _inventory;

		// Token: 0x0400088F RID: 2191
		private Character _player;

		// Token: 0x02000206 RID: 518
		private enum Type
		{
			// Token: 0x04000891 RID: 2193
			Idle,
			// Token: 0x04000892 RID: 2194
			OpenEyes,
			// Token: 0x04000893 RID: 2195
			EquipHead,
			// Token: 0x04000894 RID: 2196
			ScratchHead,
			// Token: 0x04000895 RID: 2197
			Blink
		}
	}
}

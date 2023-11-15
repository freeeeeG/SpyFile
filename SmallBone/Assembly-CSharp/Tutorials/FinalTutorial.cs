using System;
using System.Collections;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using Scenes;

namespace Tutorials
{
	// Token: 0x020000DA RID: 218
	public class FinalTutorial : Tutorial
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0000E39C File Offset: 0x0000C59C
		private bool active
		{
			get
			{
				return !GameData.Generic.playedTutorialDuringEA && GameData.Generic.tutorial.isPlaying();
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000E3B1 File Offset: 0x0000C5B1
		private void Awake()
		{
			if (!this.active)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000E3C7 File Offset: 0x0000C5C7
		public override void Activate()
		{
			if (!this.active)
			{
				return;
			}
			base.Activate();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000E3D8 File Offset: 0x0000C5D8
		protected override IEnumerator Process()
		{
			yield return Chronometer.global.WaitForSeconds(2f);
			for (int i = 3; i < 12; i++)
			{
			}
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			WeaponInventory weapon = this._player.playerComponents.inventory.weapon;
			Skul skul = weapon.polymorphOrCurrent.GetComponent<Skul>();
			skul.getScroll.TryStart();
			while (skul.getScroll.running)
			{
				yield return null;
			}
			for (int j = 12; j < 15; j++)
			{
				this.Deactivate();
			}
			GameData.Generic.tutorial.End();
			yield break;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000E3E7 File Offset: 0x0000C5E7
		protected override void OnDisable()
		{
			base.OnDisable();
		}
	}
}

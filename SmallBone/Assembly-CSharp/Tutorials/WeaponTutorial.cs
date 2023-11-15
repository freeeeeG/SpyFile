using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using Scenes;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000F0 RID: 240
	public class WeaponTutorial : Tutorial
	{
		// Token: 0x060004A2 RID: 1186 RVA: 0x0000F09C File Offset: 0x0000D29C
		protected override IEnumerator Process()
		{
			yield return base.MoveTo(this._conversationPoint.position);
			this._player.lookingDirection = Character.LookingDirection.Right;
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			WeaponInventory inventory = this._player.playerComponents.inventory.weapon;
			Skul skul = inventory.polymorphOrCurrent.GetComponent<Skul>();
			this._head.SetActive(false);
			skul.getSkul.TryStart();
			while (skul.getSkul.running)
			{
				yield return null;
			}
			inventory.ForceEquip(this._skeletonBossWeapon.Instantiate());
			this.Deactivate();
			yield break;
		}

		// Token: 0x0400038E RID: 910
		[SerializeField]
		private Weapon _skeletonBossWeapon;

		// Token: 0x0400038F RID: 911
		[SerializeField]
		private Transform _conversationPoint;

		// Token: 0x04000390 RID: 912
		[SerializeField]
		private GameObject _head;
	}
}

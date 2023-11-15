using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Characters.Player;
using FX;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorials
{
	// Token: 0x020000D0 RID: 208
	public class AttackTutorial : Tutorial
	{
		// Token: 0x060003F8 RID: 1016 RVA: 0x0000DA00 File Offset: 0x0000BC00
		protected override IEnumerator Process()
		{
			this._player.CancelAction();
			yield return base.MoveTo(this._conversationPoint.position);
			this._player.lookingDirection = Character.LookingDirection.Right;
			for (int i = 0; i < 5; i++)
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._giveBoneSoundInfo, base.transform.position);
			}
			this._skulAnimator.Play("GiveWeapon");
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			WeaponInventory inventory = this._player.playerComponents.inventory.weapon;
			TutorialSkul tutorialSkul = inventory.polymorphOrCurrent.GetComponent<TutorialSkul>();
			tutorialSkul.getBone.TryStart();
			yield return Chronometer.global.WaitForSeconds(0.5f);
			this._skulAnimator.Play("Dead");
			while (tutorialSkul.getBone.running)
			{
				yield return null;
			}
			this._skulAnimator.Play("DeadStop");
			inventory.LoseAll();
			Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.ForceEquipAt(this._skul.Instantiate(), 0);
			inventory.polymorphOrCurrent.RemoveSkill(1);
			this.Deactivate();
			yield return Chronometer.global.WaitForSeconds(1.7f);
			yield break;
		}

		// Token: 0x0400031B RID: 795
		[SerializeField]
		private Weapon _skul;

		// Token: 0x0400031C RID: 796
		[SerializeField]
		private Animator _skulAnimator;

		// Token: 0x0400031D RID: 797
		[SerializeField]
		private Transform _conversationPoint;

		// Token: 0x0400031E RID: 798
		[SerializeField]
		private SoundInfo _giveBoneSoundInfo;
	}
}

using System;
using Characters;
using Characters.Abilities;
using FX;
using Singletons;
using UnityEngine;

namespace Level.Objects
{
	// Token: 0x02000570 RID: 1392
	public class CleansingObject : InteractiveObject
	{
		// Token: 0x06001B52 RID: 6994 RVA: 0x00054DEC File Offset: 0x00052FEC
		public override void InteractWith(Character character)
		{
			character.playerComponents.savableAbilityManager.Remove(SavableAbilityManager.Name.Curse);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._effectinfo.Spawn(this._spawnPosition.position, 0f, 1f);
			this._animator.Play("Deactivate");
			base.Deactivate();
		}

		// Token: 0x0400177F RID: 6015
		[SerializeField]
		[GetComponent]
		private Animator _animator;

		// Token: 0x04001780 RID: 6016
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04001781 RID: 6017
		[SerializeField]
		private EffectInfo _effectinfo;

		// Token: 0x04001782 RID: 6018
		private const string deactivateClipCode = "Deactivate";
	}
}

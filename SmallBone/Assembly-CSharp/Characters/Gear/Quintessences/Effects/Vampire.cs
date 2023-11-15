using System;
using System.Collections;
using Characters.Actions;
using PhysicsUtils;
using Services;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008E6 RID: 2278
	public sealed class Vampire : QuintessenceEffect
	{
		// Token: 0x060030AE RID: 12462 RVA: 0x00091D42 File Offset: 0x0008FF42
		protected override void OnInvoke(Quintessence quintessence)
		{
			this.ActivateCharacter();
			this._sync.Synchronize(this._character, quintessence.owner);
			base.StartCoroutine(this.CRitual());
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x00091D6E File Offset: 0x0008FF6E
		private IEnumerator CRitual()
		{
			this._ritual.TryStart();
			bool success = false;
			while (this._ritual.running)
			{
				yield return null;
				if (!this.CheckPlayerInRange())
				{
					yield return this.CFailInRitual();
					success = false;
				}
			}
			if (success)
			{
				yield return this.CSucceedInRitual();
			}
			this.DeactivateCharacter();
			yield break;
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x00091D7D File Offset: 0x0008FF7D
		private void ActivateCharacter()
		{
			this._character.gameObject.SetActive(true);
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x00091D90 File Offset: 0x0008FF90
		private void DeactivateCharacter()
		{
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x00091DA4 File Offset: 0x0008FFA4
		private bool CheckPlayerInRange()
		{
			Vampire._sharedOverlapper.contactFilter.SetLayerMask(512);
			foreach (Target target in Vampire._sharedOverlapper.OverlapCollider(this._area).GetComponents<Target>(true))
			{
				if (!(target.character == null) && target.character.type == Character.Type.Player)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x00091E3C File Offset: 0x0009003C
		private IEnumerator CSucceedInRitual()
		{
			this._success.TryStart();
			while (this._success.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x00091E4B File Offset: 0x0009004B
		private IEnumerator CFailInRitual()
		{
			this._fail.TryStart();
			while (this._fail.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x00091E5A File Offset: 0x0009005A
		private void OnDestroy()
		{
			if (!Service.quitting)
			{
				UnityEngine.Object.Destroy(this._character.gameObject);
			}
		}

		// Token: 0x0400282C RID: 10284
		[SerializeField]
		private Collider2D _area;

		// Token: 0x0400282D RID: 10285
		[SerializeField]
		private Character _character;

		// Token: 0x0400282E RID: 10286
		[SerializeField]
		private Characters.Actions.Action _ritual;

		// Token: 0x0400282F RID: 10287
		[SerializeField]
		private Characters.Actions.Action _fail;

		// Token: 0x04002830 RID: 10288
		[SerializeField]
		private Characters.Actions.Action _success;

		// Token: 0x04002831 RID: 10289
		[SerializeField]
		private CharacterSynchronization _sync;

		// Token: 0x04002832 RID: 10290
		private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(1);
	}
}

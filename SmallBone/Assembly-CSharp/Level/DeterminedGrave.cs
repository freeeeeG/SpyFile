using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x0200054A RID: 1354
	public class DeterminedGrave : InteractiveObject
	{
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0005401F File Offset: 0x0005221F
		// (set) Token: 0x06001AE4 RID: 6884 RVA: 0x00054027 File Offset: 0x00052227
		public Weapon droppedWeapon { get; private set; }

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00054030 File Offset: 0x00052230
		public override void OnActivate()
		{
			this._animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00054042 File Offset: 0x00052242
		public override void OnDeactivate()
		{
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00054054 File Offset: 0x00052254
		public override void InteractWith(Character character)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			base.StartCoroutine(this.<InteractWith>g__CDelayedDrop|8_0());
			base.Deactivate();
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00054085 File Offset: 0x00052285
		[CompilerGenerated]
		private IEnumerator <InteractWith>g__CDelayedDrop|8_0()
		{
			yield return Chronometer.global.WaitForSeconds(0.4f);
			this.droppedWeapon = Singleton<Service>.Instance.levelManager.DropWeapon(this._weapon, base.transform.position);
			yield break;
		}

		// Token: 0x0400172C RID: 5932
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x0400172D RID: 5933
		[SerializeField]
		private Weapon _weapon;
	}
}

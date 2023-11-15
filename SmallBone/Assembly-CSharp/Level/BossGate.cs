using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x02000497 RID: 1175
	public class BossGate : InteractiveObject
	{
		// Token: 0x0600165F RID: 5727 RVA: 0x00046517 File Offset: 0x00044717
		public override void OnActivate()
		{
			base.OnActivate();
			Animator animator = this._animator;
			if (animator == null)
			{
				return;
			}
			animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00046534 File Offset: 0x00044734
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			Animator animator = this._animator;
			if (animator == null)
			{
				return;
			}
			animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00046551 File Offset: 0x00044751
		public override void InteractWith(Character character)
		{
			if (this._used)
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			this._used = true;
			Singleton<Service>.Instance.levelManager.LoadNextMap(NodeIndex.Node1);
		}

		// Token: 0x040013A3 RID: 5027
		[SerializeField]
		[GetComponent]
		private Animator _animator;

		// Token: 0x040013A4 RID: 5028
		private bool _used;
	}
}

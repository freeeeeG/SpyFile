using System;
using UnityEngine;

namespace Level.BlackMarket
{
	// Token: 0x0200061D RID: 1565
	public abstract class Npc : MonoBehaviour
	{
		// Token: 0x06001F5B RID: 8027 RVA: 0x0005F6F4 File Offset: 0x0005D8F4
		public void Activate()
		{
			if (MMMaths.RandomBool() && this._animator.HasState(0, Npc._activate2Hash))
			{
				this._animator.Play(Npc._activate2Hash);
			}
			else
			{
				this._animator.Play(Npc._activateHash);
			}
			this._minimapAgent.SetActive(true);
			this.OnActivate();
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0005F74F File Offset: 0x0005D94F
		public void Deactivate()
		{
			this._animator.Play(Npc._deactivateHash);
			this._minimapAgent.SetActive(false);
			this.OnDeactivate();
		}

		// Token: 0x06001F5D RID: 8029
		protected abstract void OnActivate();

		// Token: 0x06001F5E RID: 8030
		protected abstract void OnDeactivate();

		// Token: 0x04001A84 RID: 6788
		protected static readonly int _activateHash = Animator.StringToHash("Activate");

		// Token: 0x04001A85 RID: 6789
		protected static readonly int _activate2Hash = Animator.StringToHash("Activate2");

		// Token: 0x04001A86 RID: 6790
		protected static readonly int _deactivateHash = Animator.StringToHash("Deactivate");

		// Token: 0x04001A87 RID: 6791
		[SerializeField]
		protected Animator _animator;

		// Token: 0x04001A88 RID: 6792
		[SerializeField]
		protected GameObject _minimapAgent;
	}
}

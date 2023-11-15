using System;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005AD RID: 1453
	public class NpcAnimation : MonoBehaviour
	{
		// Token: 0x06001CD0 RID: 7376 RVA: 0x0005894E File Offset: 0x00056B4E
		private void Awake()
		{
			this._animator = base.GetComponent<Animator>();
			this.Play(this._animation);
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x00058968 File Offset: 0x00056B68
		public void Play(NpcAnimation.Animation animation)
		{
			this._animator.Play(NpcAnimation._hashes[animation]);
		}

		// Token: 0x04001872 RID: 6258
		private static readonly int _idleHash = Animator.StringToHash("Idle");

		// Token: 0x04001873 RID: 6259
		private static readonly int _cageHash = Animator.StringToHash("Idle_Cage");

		// Token: 0x04001874 RID: 6260
		private static readonly int _castleHash = Animator.StringToHash("Idle_Castle");

		// Token: 0x04001875 RID: 6261
		private static readonly EnumArray<NpcAnimation.Animation, int> _hashes = new EnumArray<NpcAnimation.Animation, int>(new int[]
		{
			NpcAnimation._idleHash,
			NpcAnimation._cageHash,
			NpcAnimation._castleHash
		});

		// Token: 0x04001876 RID: 6262
		[SerializeField]
		private NpcAnimation.Animation _animation;

		// Token: 0x04001877 RID: 6263
		private Animator _animator;

		// Token: 0x020005AE RID: 1454
		public enum Animation
		{
			// Token: 0x04001879 RID: 6265
			Idle,
			// Token: 0x0400187A RID: 6266
			Cage,
			// Token: 0x0400187B RID: 6267
			Castle
		}
	}
}

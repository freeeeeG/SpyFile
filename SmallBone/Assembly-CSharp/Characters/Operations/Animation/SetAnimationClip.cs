using System;
using UnityEngine;

namespace Characters.Operations.Animation
{
	// Token: 0x02000FA7 RID: 4007
	public class SetAnimationClip : Operation
	{
		// Token: 0x06004DCD RID: 19917 RVA: 0x000E8CC0 File Offset: 0x000E6EC0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._idleClip = null;
			this._walkClip = null;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x000E8CD8 File Offset: 0x000E6ED8
		public override void Run()
		{
			if (this._characterAnimation == null)
			{
				return;
			}
			if (this._idleClip != null)
			{
				this._characterAnimation.SetIdle(this._idleClip);
			}
			if (this._walkClip != null)
			{
				this._characterAnimation.SetWalk(this._walkClip);
			}
		}

		// Token: 0x04003DBB RID: 15803
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x04003DBC RID: 15804
		[SerializeField]
		private AnimationClip _idleClip;

		// Token: 0x04003DBD RID: 15805
		[SerializeField]
		private AnimationClip _walkClip;
	}
}

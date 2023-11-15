using System;
using Characters;
using Characters.Abilities.Blessing;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs
{
	// Token: 0x02000219 RID: 537
	public sealed class ApplyBlessing : Event
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001CE94 File Offset: 0x0001B094
		public override void Run()
		{
			Character character = this._target.character;
			Blessing blessing = UnityEngine.Object.Instantiate<Blessing>(this._blessings.Random<Blessing>());
			blessing.Apply(character);
			this._holyGrailAnimator.Play(blessing.clip.name);
			this._nameKeyCache.key = blessing.activatedNameKey;
			this._chatKeyCache.key = blessing.activatedChatKey;
		}

		// Token: 0x040008AA RID: 2218
		[SerializeField]
		private TextKeyCache _nameKeyCache;

		// Token: 0x040008AB RID: 2219
		[SerializeField]
		private TextKeyCache _chatKeyCache;

		// Token: 0x040008AC RID: 2220
		[SerializeField]
		private Runnables.Target _target;

		// Token: 0x040008AD RID: 2221
		[SerializeField]
		private Blessing[] _blessings;

		// Token: 0x040008AE RID: 2222
		[SerializeField]
		private Animator _holyGrailAnimator;
	}
}

using System;
using Characters;
using Characters.Movements;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Events
{
	// Token: 0x0200020C RID: 524
	public sealed class Knockback : Event
	{
		// Token: 0x06000A85 RID: 2693 RVA: 0x0001CCBC File Offset: 0x0001AEBC
		public override void Run()
		{
			Character character = this._taker.character;
			Character character2 = this._giver.character;
			character.movement.push.ApplyKnockback(character2, this._pushInfo);
		}

		// Token: 0x0400089A RID: 2202
		[SerializeField]
		private Runnables.Target _giver;

		// Token: 0x0400089B RID: 2203
		[SerializeField]
		private Runnables.Target _taker;

		// Token: 0x0400089C RID: 2204
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);
	}
}

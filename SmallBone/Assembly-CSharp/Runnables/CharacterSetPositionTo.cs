using System;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002D2 RID: 722
	public sealed class CharacterSetPositionTo : Runnable
	{
		// Token: 0x06000E87 RID: 3719 RVA: 0x0002D66E File Offset: 0x0002B86E
		public override void Run()
		{
			this._target.character.transform.position = this._point.transform.position;
		}

		// Token: 0x04000C0E RID: 3086
		[SerializeField]
		private Target _target;

		// Token: 0x04000C0F RID: 3087
		[SerializeField]
		private Transform _point;
	}
}

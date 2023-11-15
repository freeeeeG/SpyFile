using System;
using Characters;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200034A RID: 842
	public class ByPosition : Trigger
	{
		// Token: 0x06000FDC RID: 4060 RVA: 0x0002F9F0 File Offset: 0x0002DBF0
		protected override bool Check()
		{
			Character character = this._target.character;
			return !(character == null) && ((this._direction == ByPosition.Direction.Left && character.transform.position.x < this._base.position.x) || (this._direction == ByPosition.Direction.Right && character.transform.position.x > this._base.position.x));
		}

		// Token: 0x04000CFD RID: 3325
		[SerializeField]
		private Target _target;

		// Token: 0x04000CFE RID: 3326
		[SerializeField]
		private ByPosition.Direction _direction;

		// Token: 0x04000CFF RID: 3327
		[SerializeField]
		private Transform _base;

		// Token: 0x0200034B RID: 843
		private enum Direction
		{
			// Token: 0x04000D01 RID: 3329
			Left,
			// Token: 0x04000D02 RID: 3330
			Right
		}
	}
}

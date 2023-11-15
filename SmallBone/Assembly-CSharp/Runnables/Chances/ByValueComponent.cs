using System;
using UnityEngine;

namespace Runnables.Chances
{
	// Token: 0x02000346 RID: 838
	public sealed class ByValueComponent : Chance
	{
		// Token: 0x06000FD4 RID: 4052 RVA: 0x0002F9A7 File Offset: 0x0002DBA7
		public override bool IsTrue()
		{
			return MMMaths.Chance(this._component.value);
		}

		// Token: 0x04000CF9 RID: 3321
		[SerializeField]
		private FloatComponent _component;
	}
}

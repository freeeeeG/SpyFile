using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF5 RID: 3829
	public class ToRandomPoint : Policy
	{
		// Token: 0x06004B06 RID: 19206 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x000DCA0B File Offset: 0x000DAC0B
		public override Vector2 GetPosition()
		{
			return this._transforms.Random<Transform>().position;
		}

		// Token: 0x04003A3C RID: 14908
		[SerializeField]
		private Transform[] _transforms;
	}
}

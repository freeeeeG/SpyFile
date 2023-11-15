using System;
using Level;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF8 RID: 3832
	public class ToSavedPosition : Policy
	{
		// Token: 0x06004B13 RID: 19219 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x000DCF4A File Offset: 0x000DB14A
		public override Vector2 GetPosition()
		{
			return this._repo.Load();
		}

		// Token: 0x04003A4D RID: 14925
		[SerializeField]
		private PositionCache _repo;
	}
}

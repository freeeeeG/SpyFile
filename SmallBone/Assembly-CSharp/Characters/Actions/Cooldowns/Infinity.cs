using System;
using UnityEngine;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x02000969 RID: 2409
	public class Infinity : Cooldown
	{
		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x060033F3 RID: 13299 RVA: 0x00099F90 File Offset: 0x00098190
		internal static Infinity singleton
		{
			get
			{
				if (Infinity._singleton == null)
				{
					Infinity._singleton = new GameObject("Infinity")
					{
						hideFlags = HideFlags.HideAndDontSave
					}.AddComponent<Infinity>();
				}
				return Infinity._singleton;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060033F4 RID: 13300 RVA: 0x00071719 File Offset: 0x0006F919
		public override float remainPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x000076D4 File Offset: 0x000058D4
		public override bool canUse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x000076D4 File Offset: 0x000058D4
		internal override bool Consume()
		{
			return true;
		}

		// Token: 0x04002A12 RID: 10770
		protected static Infinity _singleton;
	}
}

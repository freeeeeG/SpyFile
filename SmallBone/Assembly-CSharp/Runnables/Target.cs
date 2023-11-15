using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002DF RID: 735
	[Serializable]
	public class Target
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0002D911 File Offset: 0x0002BB11
		public Target.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0002D919 File Offset: 0x0002BB19
		public Character character
		{
			get
			{
				if (Singleton<Service>.Instance.levelManager == null)
				{
					return null;
				}
				if (this.type == Target.Type.Player)
				{
					return Singleton<Service>.Instance.levelManager.player;
				}
				return this._character;
			}
		}

		// Token: 0x04000C25 RID: 3109
		[SerializeField]
		private Target.Type _type;

		// Token: 0x04000C26 RID: 3110
		[SerializeField]
		private Character _character;

		// Token: 0x020002E0 RID: 736
		public enum Type
		{
			// Token: 0x04000C28 RID: 3112
			Player,
			// Token: 0x04000C29 RID: 3113
			Character
		}
	}
}

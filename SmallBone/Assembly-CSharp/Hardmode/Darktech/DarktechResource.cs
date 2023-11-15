using System;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x0200015C RID: 348
	[CreateAssetMenu]
	public sealed class DarktechResource : ScriptableObject
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001402D File Offset: 0x0001222D
		public Sprite smallCloverIcon
		{
			get
			{
				return this._smallCloverIcon;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00014035 File Offset: 0x00012235
		public Sprite bigCloverIcon
		{
			get
			{
				return this._bigCloverIcon;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001403D File Offset: 0x0001223D
		public Sprite brutalityBuffIcon
		{
			get
			{
				return this._brutalityBuffIcon;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00014045 File Offset: 0x00012245
		public Sprite rageBuffIcon
		{
			get
			{
				return this._rageBuffIcon;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001404D File Offset: 0x0001224D
		public Sprite fortitudeBuffIcon
		{
			get
			{
				return this._fortitudeBuffIcon;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00014055 File Offset: 0x00012255
		public RuntimeAnimatorController omenChestSpawnEffect
		{
			get
			{
				return this._omenChestSpawnEffect;
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00014060 File Offset: 0x00012260
		public ValueTuple<Sprite, string> Find(DarktechData.Type type)
		{
			foreach (DarktechResource.Info info in this._infos)
			{
				if (info._type == type)
				{
					return new ValueTuple<Sprite, string>(info._icon, info._displayName);
				}
			}
			return new ValueTuple<Sprite, string>(null, string.Empty);
		}

		// Token: 0x04000511 RID: 1297
		[Header("품목 순환 장치")]
		[SerializeField]
		private Sprite _smallCloverIcon;

		// Token: 0x04000512 RID: 1298
		[SerializeField]
		private Sprite _bigCloverIcon;

		// Token: 0x04000513 RID: 1299
		[SerializeField]
		private Sprite _brutalityBuffIcon;

		// Token: 0x04000514 RID: 1300
		[SerializeField]
		private Sprite _rageBuffIcon;

		// Token: 0x04000515 RID: 1301
		[SerializeField]
		private Sprite _fortitudeBuffIcon;

		// Token: 0x04000516 RID: 1302
		[SerializeField]
		private RuntimeAnimatorController _omenChestSpawnEffect;

		// Token: 0x04000517 RID: 1303
		[SerializeField]
		private DarktechResource.Info[] _infos;

		// Token: 0x0200015D RID: 349
		[Serializable]
		private class Info
		{
			// Token: 0x04000518 RID: 1304
			[SerializeField]
			internal DarktechData.Type _type;

			// Token: 0x04000519 RID: 1305
			[SerializeField]
			internal int _level;

			// Token: 0x0400051A RID: 1306
			[SerializeField]
			internal Sprite _icon;

			// Token: 0x0400051B RID: 1307
			[SerializeField]
			internal string _displayName;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Actions;
using GameResources;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x02000829 RID: 2089
	public class SkillInfo : MonoBehaviour
	{
		// Token: 0x06002B28 RID: 11048 RVA: 0x00084DD8 File Offset: 0x00082FD8
		public static SkillInfo WeightedRandomPop(List<SkillInfo> from)
		{
			int maxExclusive = from.Sum((SkillInfo s) => s.weight);
			int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
			for (int i = 0; i < from.Count; i++)
			{
				SkillInfo skillInfo = from[i];
				num -= skillInfo.weight;
				if (num <= 0)
				{
					from.RemoveAt(i);
					return skillInfo;
				}
			}
			return from[0];
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002B29 RID: 11049 RVA: 0x00084E4B File Offset: 0x0008304B
		public string key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x00084E53 File Offset: 0x00083053
		public bool hasAlways
		{
			get
			{
				return this._hasAlways;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002B2B RID: 11051 RVA: 0x00084E5B File Offset: 0x0008305B
		public int weight
		{
			get
			{
				return this._weight;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002B2C RID: 11052 RVA: 0x00084E63 File Offset: 0x00083063
		// (set) Token: 0x06002B2D RID: 11053 RVA: 0x00084E6B File Offset: 0x0008306B
		public Sprite cachedIcon { get; private set; }

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x00084E74 File Offset: 0x00083074
		public string displayName
		{
			get
			{
				return Localization.GetLocalizedString("skill/" + this._key + "/name");
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002B2F RID: 11055 RVA: 0x00084E90 File Offset: 0x00083090
		public string description
		{
			get
			{
				return Localization.GetLocalizedString("skill/" + this._key + "/desc");
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002B30 RID: 11056 RVA: 0x00084EAC File Offset: 0x000830AC
		// (set) Token: 0x06002B31 RID: 11057 RVA: 0x00084EB4 File Offset: 0x000830B4
		public Characters.Actions.Action action { get; private set; }

		// Token: 0x06002B32 RID: 11058 RVA: 0x00084EC0 File Offset: 0x000830C0
		public void Initialize()
		{
			this.action = base.GetComponent<Characters.Actions.Action>();
			this.cachedIcon = GearResource.instance.GetSkillIcon(this._key);
			if (this.cachedIcon == null)
			{
				Debug.LogError(string.Format("Couldn't find a skill icon file: {0}.png", this.cachedIcon));
			}
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x00084F12 File Offset: 0x00083112
		public Sprite GetIcon()
		{
			return GearResource.instance.GetSkillIcon(this._key);
		}

		// Token: 0x040024BE RID: 9406
		[SerializeField]
		private string _key;

		// Token: 0x040024BF RID: 9407
		[SerializeField]
		private bool _hasAlways;

		// Token: 0x040024C0 RID: 9408
		[Range(0f, 100f)]
		[SerializeField]
		private int _weight = 1;
	}
}

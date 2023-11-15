using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords
{
	// Token: 0x02001284 RID: 4740
	public class LightSwordPool : MonoBehaviour
	{
		// Token: 0x06005E01 RID: 24065 RVA: 0x001148B7 File Offset: 0x00112AB7
		private void Awake()
		{
			this.Create(this._count);
		}

		// Token: 0x06005E02 RID: 24066 RVA: 0x001148C8 File Offset: 0x00112AC8
		private void Create(int count)
		{
			this._swords = new List<LightSword>(this._count);
			for (int i = 0; i < count; i++)
			{
				LightSword lightSword = UnityEngine.Object.Instantiate<LightSword>(this._lightSwordPrefab, Vector2.zero, Quaternion.identity, Map.Instance.transform);
				lightSword.Initialzie(this._owner);
				this._swords.Add(lightSword);
			}
		}

		// Token: 0x06005E03 RID: 24067 RVA: 0x0011492F File Offset: 0x00112B2F
		public List<LightSword> Get()
		{
			return this._swords;
		}

		// Token: 0x06005E04 RID: 24068 RVA: 0x00114937 File Offset: 0x00112B37
		public List<LightSword> Take(int count)
		{
			if (this._swords.Count < count)
			{
				return null;
			}
			return this._swords.Take(count).ToList<LightSword>();
		}

		// Token: 0x04004B84 RID: 19332
		[SerializeField]
		private Character _owner;

		// Token: 0x04004B85 RID: 19333
		[SerializeField]
		private LightSword _lightSwordPrefab;

		// Token: 0x04004B86 RID: 19334
		[SerializeField]
		private int _count;

		// Token: 0x04004B87 RID: 19335
		private List<LightSword> _swords;
	}
}

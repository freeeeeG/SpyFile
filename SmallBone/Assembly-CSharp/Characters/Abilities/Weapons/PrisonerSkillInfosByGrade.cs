using System;
using System.Collections.Generic;
using Characters.Gear.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BF4 RID: 3060
	public class PrisonerSkillInfosByGrade : MonoBehaviour
	{
		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x000B6A82 File Offset: 0x000B4C82
		public string key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003EDC RID: 16092 RVA: 0x000B6A8A File Offset: 0x000B4C8A
		public IReadOnlyList<SkillInfo> skillInfos
		{
			get
			{
				return this._skillInfos;
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x000B6A94 File Offset: 0x000B4C94
		private void Awake()
		{
			for (int i = 0; i < this._skillInfos.Length; i++)
			{
				PrisonerSkill.AddComponent(this._skillInfos[i].gameObject, this, i);
			}
		}

		// Token: 0x04003083 RID: 12419
		[SerializeField]
		private string _key;

		// Token: 0x04003084 RID: 12420
		[FormerlySerializedAs("_skills")]
		[SerializeField]
		private SkillInfo[] _skillInfos;
	}
}

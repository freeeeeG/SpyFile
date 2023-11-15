using System;
using UnityEngine;

namespace Characters.Abilities.Weapons
{
	// Token: 0x02000BF3 RID: 3059
	public class PrisonerSkill : MonoBehaviour
	{
		// Token: 0x06003ED9 RID: 16089 RVA: 0x000B6A6C File Offset: 0x000B4C6C
		public static PrisonerSkill AddComponent(GameObject gameObject, PrisonerSkillInfosByGrade parent, int level)
		{
			PrisonerSkill prisonerSkill = gameObject.AddComponent<PrisonerSkill>();
			prisonerSkill.parent = parent;
			prisonerSkill.level = level;
			return prisonerSkill;
		}

		// Token: 0x04003081 RID: 12417
		public PrisonerSkillInfosByGrade parent;

		// Token: 0x04003082 RID: 12418
		public int level;
	}
}

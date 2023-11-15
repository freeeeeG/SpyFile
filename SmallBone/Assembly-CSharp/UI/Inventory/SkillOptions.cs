using System;
using Characters.Gear.Weapons;
using UnityEngine;

namespace UI.Inventory
{
	// Token: 0x02000446 RID: 1094
	public class SkillOptions : MonoBehaviour
	{
		// Token: 0x060014C8 RID: 5320 RVA: 0x00040974 File Offset: 0x0003EB74
		public void Set(Weapon weapon)
		{
			if (weapon.currentSkills.Count == 1)
			{
				this.optionContainer.SetActive(true);
				this.option2Container.SetActive(false);
				this.option.Set(weapon.currentSkills[0]);
				return;
			}
			this.optionContainer.SetActive(false);
			this.option2Container.SetActive(true);
			this.optionLeft.Set(weapon.currentSkills[0]);
			this.optionRight.Set(weapon.currentSkills[1]);
		}

		// Token: 0x040011E4 RID: 4580
		[SerializeField]
		private GameObject optionContainer;

		// Token: 0x040011E5 RID: 4581
		[SerializeField]
		private SkillOption option;

		// Token: 0x040011E6 RID: 4582
		[SerializeField]
		private GameObject option2Container;

		// Token: 0x040011E7 RID: 4583
		[SerializeField]
		private SkillOption optionLeft;

		// Token: 0x040011E8 RID: 4584
		[SerializeField]
		private SkillOption optionRight;
	}
}

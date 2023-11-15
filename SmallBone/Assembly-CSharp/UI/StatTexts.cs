using System;
using System.Collections.Generic;
using Characters.Gear;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using UnityEngine;

namespace UI
{
	// Token: 0x020003D2 RID: 978
	public class StatTexts : MonoBehaviour
	{
		// Token: 0x0600122D RID: 4653 RVA: 0x000358B6 File Offset: 0x00033AB6
		private void Awake()
		{
			this._gear = base.GetComponentInParent<Gear>();
			this.SetSkillDescription();
			this.SetQuintessenceDescription();
			this.SetStatText(this._gear.stat.strings);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000358E8 File Offset: 0x00033AE8
		private void SetSkillDescription()
		{
			Weapon weapon = this._gear as Weapon;
			if (weapon == null)
			{
				return;
			}
			for (int i = 0; i < weapon.currentSkills.Count; i++)
			{
				UnityEngine.Object.Instantiate<SkillDesc>(this._skillDescription, base.transform, false).info = weapon.currentSkills[i];
			}
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00035944 File Offset: 0x00033B44
		private void SetQuintessenceDescription()
		{
			Quintessence quintessence = this._gear as Quintessence;
			if (quintessence == null)
			{
				return;
			}
			UnityEngine.Object.Instantiate<QuintessenceDesc>(this._quintessenceDescription, base.transform, false).text = quintessence.description;
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00035984 File Offset: 0x00033B84
		private void SetStatText(IList<string> _texts)
		{
			for (int i = 0; i < _texts.Count; i++)
			{
				UnityEngine.Object.Instantiate<TextLayoutElement>(this._statText, base.transform, false).text = _texts[i];
			}
		}

		// Token: 0x04000F14 RID: 3860
		[SerializeField]
		private TextLayoutElement _statText;

		// Token: 0x04000F15 RID: 3861
		[SerializeField]
		private QuintessenceDesc _quintessenceDescription;

		// Token: 0x04000F16 RID: 3862
		[SerializeField]
		private SkillDesc _skillDescription;

		// Token: 0x04000F17 RID: 3863
		private Gear _gear;
	}
}

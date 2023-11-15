using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace UI.Hud
{
	// Token: 0x02000464 RID: 1124
	public class AbilityIconDisplay : MonoBehaviour
	{
		// Token: 0x0600156A RID: 5482 RVA: 0x00043504 File Offset: 0x00041704
		private void Awake()
		{
			this._icons = new AbilityIcon[27];
			bool activeSelf = this._abilityIconPrefab.gameObject.activeSelf;
			this._abilityIconPrefab.gameObject.SetActive(false);
			for (int i = 0; i < 27; i++)
			{
				this._icons[i] = UnityEngine.Object.Instantiate<AbilityIcon>(this._abilityIconPrefab, base.transform);
			}
			this._abilityIconPrefab.gameObject.SetActive(activeSelf);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00043577 File Offset: 0x00041777
		public void Initialize(Character character)
		{
			this._character = character;
			base.StopAllCoroutines();
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00043588 File Offset: 0x00041788
		private IAbilityInstance FindNextAbilityWithIcon(ref int index)
		{
			while (index < this._character.ability.Count)
			{
				IAbilityInstance abilityInstance = this._character.ability[index];
				if (!(abilityInstance.icon == null))
				{
					index++;
					return abilityInstance;
				}
				index++;
			}
			return null;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000435DC File Offset: 0x000417DC
		private void Update()
		{
			if (this._character == null)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 27; i++)
			{
				AbilityIcon abilityIcon = this._icons[i];
				IAbilityInstance abilityInstance = this.FindNextAbilityWithIcon(ref num);
				if (abilityInstance == null)
				{
					abilityIcon.gameObject.SetActive(false);
				}
				else
				{
					abilityIcon.gameObject.SetActive(true);
					abilityIcon.icon = abilityInstance.icon;
					abilityIcon.fillAmount = abilityInstance.iconFillAmount;
					if (abilityInstance.iconFillInversed)
					{
						abilityIcon.fillAmount = 1f - abilityIcon.fillAmount;
					}
					abilityIcon.clockwise = !abilityInstance.iconFillFlipped;
					abilityIcon.stacks = abilityInstance.iconStacks;
				}
			}
		}

		// Token: 0x040012BA RID: 4794
		private const int _maxIcons = 27;

		// Token: 0x040012BB RID: 4795
		[SerializeField]
		private AbilityIcon _abilityIconPrefab;

		// Token: 0x040012BC RID: 4796
		private AbilityIcon[] _icons;

		// Token: 0x040012BD RID: 4797
		private Character _character;
	}
}

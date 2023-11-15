using System;
using System.Collections;
using Characters.Abilities;
using Characters.Abilities.CharacterStat;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200088B RID: 2187
	public sealed class Fortress : InscriptionInstance
	{
		// Token: 0x06002E1E RID: 11806 RVA: 0x0008B6F4 File Offset: 0x000898F4
		protected override void Initialize()
		{
			this._statBonus.Initialize();
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x0008B704 File Offset: 0x00089904
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			this._shield.amount = this._shieldByStep[this.keyword.step];
			if (this.keyword.step < this.keyword.steps.Count - 1)
			{
				base.character.ability.Remove(this._statBonus);
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0008B764 File Offset: 0x00089964
		public override void Attach()
		{
			base.StartCoroutine("CApplyStatBonus");
			base.StartCoroutine("CRefreshShield");
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x0008B780 File Offset: 0x00089980
		public override void Detach()
		{
			base.StopCoroutine("CApplyStatBonus");
			base.StopCoroutine("CRefreshShield");
			base.character.ability.Remove(this._shield);
			base.character.ability.Remove(this._cooldownAbility);
			base.character.ability.Remove(this._statBonus);
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x0008B7E8 File Offset: 0x000899E8
		private IEnumerator CRefreshShield()
		{
			yield return null;
			for (;;)
			{
				if (this.keyword.step < 1)
				{
					yield return null;
				}
				else
				{
					base.character.ability.Add(this._shield);
					base.character.ability.Add(this._cooldownAbility);
					yield return base.character.chronometer.master.WaitForSeconds(this._cooldownAbility.duration);
				}
			}
			yield break;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x0008B7F7 File Offset: 0x000899F7
		private IEnumerator CApplyStatBonus()
		{
			yield return null;
			for (;;)
			{
				if (this.keyword.step < this.keyword.steps.Count - 1)
				{
					yield return null;
				}
				else
				{
					if (base.character.health.shield.hasAny)
					{
						base.character.ability.Add(this._statBonus);
					}
					else
					{
						base.character.ability.Remove(this._statBonus);
					}
					yield return base.character.chronometer.master.WaitForSeconds(0.1f);
				}
			}
			yield break;
		}

		// Token: 0x04002665 RID: 9829
		private const float _checkInterval = 0.1f;

		// Token: 0x04002666 RID: 9830
		[SerializeField]
		[Header("2세트 효과")]
		private float[] _shieldByStep;

		// Token: 0x04002667 RID: 9831
		[SerializeField]
		private int _refreshInterval;

		// Token: 0x04002668 RID: 9832
		[Information("duration은 _refreshInterval + 1로 설정하는 것을 권장", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Shield _shield;

		// Token: 0x04002669 RID: 9833
		[Header("4세트 효과")]
		[SerializeField]
		private StatBonus _statBonus;

		// Token: 0x0400266A RID: 9834
		[SerializeField]
		private Nothing _cooldownAbility;
	}
}

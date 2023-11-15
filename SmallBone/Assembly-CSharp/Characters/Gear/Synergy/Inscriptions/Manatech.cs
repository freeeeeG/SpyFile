using System;
using Characters.Actions;
using Level;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200089B RID: 2203
	public sealed class Manatech : SimpleStatBonusKeyword
	{
		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x0008C859 File Offset: 0x0008AA59
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusBystep;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x00088DBC File Offset: 0x00086FBC
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.PercentPoint;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x0008C861 File Offset: 0x0008AA61
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.SkillAttackSpeed;
			}
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x0008C7EC File Offset: 0x0008A9EC
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x0008C868 File Offset: 0x0008AA68
		public override void Attach()
		{
			base.Attach();
			base.character.onStartAction += this.OnStartAction;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x0008C887 File Offset: 0x0008AA87
		public override void Detach()
		{
			base.Detach();
			base.character.onStartAction -= this.OnStartAction;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x0008C8A8 File Offset: 0x0008AAA8
		private void OnStartAction(Characters.Actions.Action action)
		{
			if (action.type != Characters.Actions.Action.Type.Skill || action.cooldown.usedByStreak)
			{
				return;
			}
			int num = 0;
			while ((double)num < this._countBystep[this.keyword.step])
			{
				Vector3 position = base.transform.position;
				position.y += 0.5f;
				this._manatechPart.poolObject.Spawn(position, true).GetComponent<DroppedManatechPart>().cooldownReducingAmount = this._cooldownReducingAmount;
				num++;
			}
		}

		// Token: 0x040026CF RID: 9935
		[SerializeField]
		private float _cooldownReducingAmount;

		// Token: 0x040026D0 RID: 9936
		[SerializeField]
		private DroppedManatechPart _manatechPart;

		// Token: 0x040026D1 RID: 9937
		[SerializeField]
		private double[] _countBystep;

		// Token: 0x040026D2 RID: 9938
		[SerializeField]
		private double[] _statBonusBystep;
	}
}

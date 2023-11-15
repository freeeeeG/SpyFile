using System;
using System.Collections;
using Characters.Abilities;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x0200086F RID: 2159
	public sealed class Bone : InscriptionInstance
	{
		// Token: 0x06002D44 RID: 11588 RVA: 0x00089B50 File Offset: 0x00087D50
		protected override void Initialize()
		{
			this._onAttach.Initialize();
			this._buff = new Bone.Buff
			{
				defaultIcon = this._icon,
				buffDuration = this._duration,
				cycle = this._cycle,
				onAttach = this._onAttach
			};
			this._buff.Initialize();
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x00002191 File Offset: 0x00000391
		public override void Attach()
		{
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x00089BAE File Offset: 0x00087DAE
		public override void Detach()
		{
			base.character.ability.Remove(this._buff);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x00089BC7 File Offset: 0x00087DC7
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			if (this.keyword.isMaxStep)
			{
				base.character.ability.Add(this._buff);
				return;
			}
			base.character.ability.Remove(this._buff);
		}

		// Token: 0x040025F3 RID: 9715
		[SerializeField]
		[Header("2세트 효과")]
		private int _cycle;

		// Token: 0x040025F4 RID: 9716
		[SerializeField]
		private Sprite _icon;

		// Token: 0x040025F5 RID: 9717
		[SerializeField]
		private float _duration;

		// Token: 0x040025F6 RID: 9718
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onAttach;

		// Token: 0x040025F7 RID: 9719
		private Bone.Buff _buff;

		// Token: 0x02000870 RID: 2160
		private class Buff : Ability
		{
			// Token: 0x1700098B RID: 2443
			// (get) Token: 0x06002D49 RID: 11593 RVA: 0x00089C0D File Offset: 0x00087E0D
			// (set) Token: 0x06002D4A RID: 11594 RVA: 0x00089C15 File Offset: 0x00087E15
			public OperationInfo.Subcomponents onAttach { get; set; }

			// Token: 0x1700098C RID: 2444
			// (get) Token: 0x06002D4B RID: 11595 RVA: 0x00089C1E File Offset: 0x00087E1E
			// (set) Token: 0x06002D4C RID: 11596 RVA: 0x00089C26 File Offset: 0x00087E26
			public int cycle { get; set; }

			// Token: 0x1700098D RID: 2445
			// (get) Token: 0x06002D4D RID: 11597 RVA: 0x00089C2F File Offset: 0x00087E2F
			// (set) Token: 0x06002D4E RID: 11598 RVA: 0x00089C37 File Offset: 0x00087E37
			public float buffDuration { get; set; }

			// Token: 0x06002D4F RID: 11599 RVA: 0x00089C40 File Offset: 0x00087E40
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new Bone.Buff.Instance(owner, this);
			}

			// Token: 0x02000871 RID: 2161
			public class Instance : AbilityInstance<Bone.Buff>
			{
				// Token: 0x1700098E RID: 2446
				// (get) Token: 0x06002D51 RID: 11601 RVA: 0x00089C51 File Offset: 0x00087E51
				public override Sprite icon
				{
					get
					{
						if (this._count != 0)
						{
							return this.ability.defaultIcon;
						}
						return null;
					}
				}

				// Token: 0x1700098F RID: 2447
				// (get) Token: 0x06002D52 RID: 11602 RVA: 0x00089C68 File Offset: 0x00087E68
				public override int iconStacks
				{
					get
					{
						return this._count;
					}
				}

				// Token: 0x17000990 RID: 2448
				// (get) Token: 0x06002D53 RID: 11603 RVA: 0x00089C70 File Offset: 0x00087E70
				public override float iconFillAmount
				{
					get
					{
						return (float)((this._count == this.ability.cycle - 1) ? 0 : 1);
					}
				}

				// Token: 0x06002D54 RID: 11604 RVA: 0x00089C8C File Offset: 0x00087E8C
				public Instance(Character owner, Bone.Buff ability) : base(owner, ability)
				{
				}

				// Token: 0x06002D55 RID: 11605 RVA: 0x00089C96 File Offset: 0x00087E96
				protected override void OnAttach()
				{
					this.owner.onStartAction += this.OnStartAction;
				}

				// Token: 0x06002D56 RID: 11606 RVA: 0x00089CAF File Offset: 0x00087EAF
				protected override void OnDetach()
				{
					this.owner.onStartAction -= this.OnStartAction;
					this.owner.evasion.Detach(this);
				}

				// Token: 0x06002D57 RID: 11607 RVA: 0x00089CDA File Offset: 0x00087EDA
				public override void UpdateTime(float deltaTime)
				{
					this._remainBuffTime -= deltaTime;
					if (this._attached && this._remainBuffTime < 0f)
					{
						this._attached = false;
						this.DetachEvasion();
					}
				}

				// Token: 0x06002D58 RID: 11608 RVA: 0x00089D0C File Offset: 0x00087F0C
				private void OnStartAction(Characters.Actions.Action action)
				{
					if (action.type != Characters.Actions.Action.Type.Swap)
					{
						return;
					}
					this._count++;
					if (this._count >= this.ability.cycle)
					{
						this._count -= this.ability.cycle;
						this.Apply();
					}
				}

				// Token: 0x06002D59 RID: 11609 RVA: 0x00089D62 File Offset: 0x00087F62
				private void AttachEvasion()
				{
					this.owner.evasion.Attach(this);
				}

				// Token: 0x06002D5A RID: 11610 RVA: 0x00089D75 File Offset: 0x00087F75
				private void DetachEvasion()
				{
					this.owner.evasion.Detach(this);
				}

				// Token: 0x06002D5B RID: 11611 RVA: 0x00089D8C File Offset: 0x00087F8C
				private void Apply()
				{
					this._attached = true;
					this._remainBuffTime = this.ability.buffDuration;
					this.owner.StartCoroutine(this.CApply());
					this.AttachEvasion();
					this.owner.StartCoroutine(this.ability.onAttach.CRun(this.owner));
				}

				// Token: 0x06002D5C RID: 11612 RVA: 0x00089DEB File Offset: 0x00087FEB
				private IEnumerator CApply()
				{
					yield return null;
					this.owner.playerComponents.inventory.weapon.ResetSwapCooldown();
					yield break;
				}

				// Token: 0x040025FB RID: 9723
				private int _count;

				// Token: 0x040025FC RID: 9724
				private float _remainBuffTime;

				// Token: 0x040025FD RID: 9725
				private bool _attached;
			}
		}
	}
}

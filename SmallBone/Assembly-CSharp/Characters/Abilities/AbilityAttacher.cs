using System;
using Characters.Abilities.Customs;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009D7 RID: 2519
	public abstract class AbilityAttacher : MonoBehaviour
	{
		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x0009F0CE File Offset: 0x0009D2CE
		// (set) Token: 0x0600358C RID: 13708 RVA: 0x0009F0D6 File Offset: 0x0009D2D6
		public Character owner { get; private set; }

		// Token: 0x0600358D RID: 13709 RVA: 0x0009F0DF File Offset: 0x0009D2DF
		public void Initialize(Character character)
		{
			this.owner = character;
			this.OnIntialize();
		}

		// Token: 0x0600358E RID: 13710
		public abstract void StartAttach();

		// Token: 0x0600358F RID: 13711
		public abstract void StopAttach();

		// Token: 0x06003590 RID: 13712
		public abstract void OnIntialize();

		// Token: 0x020009D8 RID: 2520
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06003592 RID: 13714 RVA: 0x0009F0F0 File Offset: 0x0009D2F0
			static SubcomponentAttribute()
			{
				int length = typeof(AbilityAttacher).Namespace.Length;
				AbilityAttacher.SubcomponentAttribute.names = new string[AbilityAttacher.SubcomponentAttribute.types.Length];
				for (int i = 0; i < AbilityAttacher.SubcomponentAttribute.names.Length; i++)
				{
					Type type = AbilityAttacher.SubcomponentAttribute.types[i];
					if (type == null)
					{
						string text = AbilityAttacher.SubcomponentAttribute.names[i - 1];
						int num = text.LastIndexOf('/');
						if (num == -1)
						{
							AbilityAttacher.SubcomponentAttribute.names[i] = string.Empty;
						}
						else
						{
							AbilityAttacher.SubcomponentAttribute.names[i] = text.Substring(0, num + 1);
						}
					}
					else
					{
						AbilityAttacher.SubcomponentAttribute.names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
					}
				}
			}

			// Token: 0x06003593 RID: 13715 RVA: 0x0009F304 File Offset: 0x0009D504
			public SubcomponentAttribute() : base(true, AbilityAttacher.SubcomponentAttribute.types, AbilityAttacher.SubcomponentAttribute.names)
			{
			}

			// Token: 0x04002B23 RID: 11043
			public new static readonly Type[] types = new Type[]
			{
				typeof(AlwaysAbilityAttacher),
				typeof(TriggerAbilityAttacher),
				typeof(RandomTriggerAbilityAttacher),
				typeof(DuringCombatAbilityAttacher),
				typeof(DuringChargingAbilityAttacher),
				typeof(DuringRunningActionAbilityAttacher),
				typeof(HealthAttacher),
				typeof(ShieldAttacher),
				typeof(EnemyWithinRangeAttacher),
				typeof(EffectZoneAbilityAttacher),
				typeof(AirAndGroundAbility),
				typeof(InMapAbilityAttacher),
				typeof(OwnItemAbilityAttacher),
				typeof(CurrencyAbilityAttacher),
				typeof(FirstAttackAbilityAttacher),
				null,
				typeof(HeadCategoryAttacher),
				typeof(CurrentHeadCategoryAttacher),
				typeof(CurrentHeadTagAttacher),
				null,
				typeof(MaxStatStackAbilityAttacher),
				typeof(CharacterAliveAbilityAttacher),
				typeof(GhoulConsumeStatAttacher),
				typeof(LivingArmor2PassiveAttacher),
				typeof(EntColliderAbilityAttacher),
				typeof(OnUseReassembleAttacher)
			};

			// Token: 0x04002B24 RID: 11044
			public new static readonly string[] names;
		}

		// Token: 0x020009D9 RID: 2521
		[Serializable]
		internal class Subcomponents : SubcomponentArray<AbilityAttacher>
		{
			// Token: 0x06003594 RID: 13716 RVA: 0x0009F318 File Offset: 0x0009D518
			public void Initialize(Character character)
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].Initialize(character);
				}
			}

			// Token: 0x06003595 RID: 13717 RVA: 0x0009F348 File Offset: 0x0009D548
			public void StartAttach()
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].StartAttach();
				}
			}

			// Token: 0x06003596 RID: 13718 RVA: 0x0009F378 File Offset: 0x0009D578
			public void StopAttach()
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].StopAttach();
				}
			}
		}
	}
}

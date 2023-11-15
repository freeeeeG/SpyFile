using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Characters.Abilities;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DAC RID: 3500
	public class AttachAbilityToMinions : CharacterOperation
	{
		// Token: 0x06004677 RID: 18039 RVA: 0x000CB838 File Offset: 0x000C9A38
		public override void Initialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x000CB848 File Offset: 0x000C9A48
		public override void Run(Character target)
		{
			this._target = target;
			if (this._target.playerComponents == null)
			{
				return;
			}
			if (this._targetMinions != null && this._targetMinions.Length != 0)
			{
				this.ApplyToMinions();
				return;
			}
			IEnumerable<Minion> minionEnumerable;
			if (this._targetMinion == null)
			{
				minionEnumerable = this._target.playerComponents.minionLeader.GetMinionEnumerable();
			}
			else
			{
				minionEnumerable = this._target.playerComponents.minionLeader.GetMinionEnumerable(this._targetMinion);
			}
			if (this._maxCount == 0)
			{
				using (IEnumerator<Minion> enumerator = minionEnumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Minion minion = enumerator.Current;
						if (!this.AddAbility(minion))
						{
							break;
						}
					}
					return;
				}
			}
			switch (this._priorityPolicy)
			{
			case AttachAbilityToMinions.PriorityPolicy.NearToFar:
				this.AddabilityByDistance(minionEnumerable, target.transform.position, ([TupleElementNames(new string[]
				{
					"minion",
					"distance"
				})] ValueTuple<Minion, float> a, [TupleElementNames(new string[]
				{
					"minion",
					"distance"
				})] ValueTuple<Minion, float> b) => a.Item2.CompareTo(b.Item2));
				return;
			case AttachAbilityToMinions.PriorityPolicy.FarToNear:
				this.AddabilityByDistance(minionEnumerable, target.transform.position, ([TupleElementNames(new string[]
				{
					"minion",
					"distance"
				})] ValueTuple<Minion, float> a, [TupleElementNames(new string[]
				{
					"minion",
					"distance"
				})] ValueTuple<Minion, float> b) => b.Item2.CompareTo(a.Item2));
				return;
			case AttachAbilityToMinions.PriorityPolicy.Random:
				this.AddAbilityRandom(minionEnumerable);
				break;
			default:
				return;
			}
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x000CB998 File Offset: 0x000C9B98
		private void ApplyToMinions()
		{
			if (this._targetMinions == null || this._targetMinions.Length == 0)
			{
				return;
			}
			Minion[] targetMinions = this._targetMinions;
			int i = 0;
			while (i < targetMinions.Length)
			{
				Minion targetMinion = targetMinions[i];
				IEnumerable<Minion> minionEnumerable = this._target.playerComponents.minionLeader.GetMinionEnumerable(targetMinion);
				if (this._maxCount == 0)
				{
					using (IEnumerator<Minion> enumerator = minionEnumerable.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Minion minion = enumerator.Current;
							if (!this.AddAbility(minion))
							{
								return;
							}
						}
						goto IL_113;
					}
					goto IL_7F;
				}
				goto IL_7F;
				IL_113:
				i++;
				continue;
				IL_7F:
				switch (this._priorityPolicy)
				{
				case AttachAbilityToMinions.PriorityPolicy.NearToFar:
					this.AddabilityByDistance(minionEnumerable, this._target.transform.position, ([TupleElementNames(new string[]
					{
						"minion",
						"distance"
					})] ValueTuple<Minion, float> a, [TupleElementNames(new string[]
					{
						"minion",
						"distance"
					})] ValueTuple<Minion, float> b) => a.Item2.CompareTo(b.Item2));
					goto IL_113;
				case AttachAbilityToMinions.PriorityPolicy.FarToNear:
					this.AddabilityByDistance(minionEnumerable, this._target.transform.position, ([TupleElementNames(new string[]
					{
						"minion",
						"distance"
					})] ValueTuple<Minion, float> a, [TupleElementNames(new string[]
					{
						"minion",
						"distance"
					})] ValueTuple<Minion, float> b) => b.Item2.CompareTo(a.Item2));
					goto IL_113;
				case AttachAbilityToMinions.PriorityPolicy.Random:
					this.AddAbilityRandom(minionEnumerable);
					goto IL_113;
				default:
					goto IL_113;
				}
			}
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x000CBAD8 File Offset: 0x000C9CD8
		private void AddabilityByDistance(IEnumerable<Minion> enumerable, Vector3 origin, [TupleElementNames(new string[]
		{
			"minion",
			"distance"
		})] Comparison<ValueTuple<Minion, float>> comparison)
		{
			List<ValueTuple<Minion, float>> list = new List<ValueTuple<Minion, float>>();
			foreach (Minion minion in enumerable)
			{
				list.Add(new ValueTuple<Minion, float>(minion, math.distancesq(origin, minion.transform.position)));
			}
			list.Sort(comparison);
			int num = 0;
			foreach (ValueTuple<Minion, float> valueTuple in list)
			{
				Minion item = valueTuple.Item1;
				if (!this.AddAbility(item))
				{
					break;
				}
				num++;
				if (num == this._maxCount)
				{
					break;
				}
			}
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x000CBBA8 File Offset: 0x000C9DA8
		private void AddAbilityRandom(IEnumerable<Minion> enumerable)
		{
			int num = 0;
			Minion[] array = enumerable.ToArray<Minion>();
			array.Shuffle<Minion>();
			enumerable = array;
			foreach (Minion minion in enumerable)
			{
				if (!this.AddAbility(minion))
				{
					break;
				}
				num++;
				if (num == this._maxCount)
				{
					break;
				}
			}
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x000CBC14 File Offset: 0x000C9E14
		private bool AddAbility(Minion minion)
		{
			Character character = minion.character;
			if (this._range != null && !this._range.OverlapPoint(character.transform.position))
			{
				return false;
			}
			character.ability.Add(this._abilityComponent.ability);
			minion.onUnsummon -= this.RemoveAbilityOnUnsummon;
			minion.onUnsummon += this.RemoveAbilityOnUnsummon;
			return true;
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x000CBC91 File Offset: 0x000C9E91
		private void RemoveAbilityOnUnsummon(Character owner, Character summoned)
		{
			summoned.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x000CBCAC File Offset: 0x000C9EAC
		public override void Stop()
		{
			if (this._target == null)
			{
				return;
			}
			if (this._target.playerComponents == null)
			{
				return;
			}
			IEnumerable<Minion> minionEnumerable;
			if (this._targetMinion == null)
			{
				minionEnumerable = this._target.playerComponents.minionLeader.GetMinionEnumerable();
			}
			else
			{
				minionEnumerable = this._target.playerComponents.minionLeader.GetMinionEnumerable(this._targetMinion);
			}
			foreach (Minion minion in minionEnumerable)
			{
				Character character = minion.character;
				if (!(this._range != null) || this._range.OverlapPoint(character.transform.position))
				{
					character.ability.Remove(this._abilityComponent.ability);
					minion.onUnsummon -= this.RemoveAbilityOnUnsummon;
				}
			}
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04003556 RID: 13654
		[SerializeField]
		[Tooltip("적용할 미니언 프리팹, 비워두면 모든 미니언에 적용됨")]
		private Minion _targetMinion;

		// Token: 0x04003557 RID: 13655
		[SerializeField]
		private Minion[] _targetMinions;

		// Token: 0x04003558 RID: 13656
		[Tooltip("적용할 범위, 비워두면 모든 범위에 적용됨")]
		[SerializeField]
		[Space]
		private Collider2D _range;

		// Token: 0x04003559 RID: 13657
		[SerializeField]
		[Tooltip("최대 적용 가능한 미니언 수, 0이면 무제한")]
		private int _maxCount;

		// Token: 0x0400355A RID: 13658
		[SerializeField]
		private AttachAbilityToMinions.PriorityPolicy _priorityPolicy;

		// Token: 0x0400355B RID: 13659
		[Space]
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x0400355C RID: 13660
		private Character _target;

		// Token: 0x02000DAD RID: 3501
		private enum PriorityPolicy
		{
			// Token: 0x0400355E RID: 13662
			NearToFar,
			// Token: 0x0400355F RID: 13663
			FarToNear,
			// Token: 0x04003560 RID: 13664
			Random
		}
	}
}

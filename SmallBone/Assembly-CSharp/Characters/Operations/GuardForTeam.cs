using System;
using System.Collections;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E12 RID: 3602
	public class GuardForTeam : CharacterOperation
	{
		// Token: 0x060047E4 RID: 18404 RVA: 0x000D15C3 File Offset: 0x000CF7C3
		static GuardForTeam()
		{
			GuardForTeam._teamOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x000D15EC File Offset: 0x000CF7EC
		private bool Block(ref Damage damage)
		{
			Attacker attacker = damage.attacker;
			if (damage.attackType == Damage.AttackType.Additional)
			{
				return false;
			}
			if (damage.attackType == Damage.AttackType.Ranged)
			{
				return false;
			}
			Vector3 position = base.transform.position;
			Vector3 position2 = damage.attacker.character.transform.position;
			if (this._owner.lookingDirection == Character.LookingDirection.Right)
			{
				if (this._guardRange.bounds.max.x < position2.x)
				{
					this.GiveGuardEffect(ref damage, attacker.character);
					return true;
				}
			}
			else if (this._guardRange.bounds.min.x > position2.x)
			{
				this.GiveGuardEffect(ref damage, attacker.character);
				return true;
			}
			return false;
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x000D16A4 File Offset: 0x000CF8A4
		public override void Run(Character owner)
		{
			this._owner = owner;
			this._running = true;
			this._teamCached = new List<Character>();
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CExpire());
			}
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x000D16D9 File Offset: 0x000CF8D9
		private void Update()
		{
			if (this._running)
			{
				this.GiveGuardBuff();
			}
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x000D16EC File Offset: 0x000CF8EC
		private void GiveGuardEffect(ref Damage damage, Character attacker)
		{
			damage.stoppingPower = 0f;
			if (damage.attackType == Damage.AttackType.Melee)
			{
				this._onHitToOwnerChronoInfo.ApplyGlobe();
				if (this._onHitToOwner.components.Length != 0)
				{
					for (int i = 0; i < this._onHitToOwner.components.Length; i++)
					{
						this._onHitToOwner.components[i].Run(this._owner);
					}
				}
				if (this._onHitToTarget.components.Length != 0)
				{
					for (int j = 0; j < this._onHitToTarget.components.Length; j++)
					{
						this._onHitToTarget.components[j].Run(attacker);
					}
					return;
				}
			}
			else if ((damage.attackType == Damage.AttackType.Ranged || damage.attackType == Damage.AttackType.Projectile) && this._onHitToOwnerFromRangeAttack.components.Length != 0)
			{
				for (int k = 0; k < this._onHitToOwnerFromRangeAttack.components.Length; k++)
				{
					this._onHitToOwnerFromRangeAttack.components[k].Run(this._owner);
				}
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x000D17E3 File Offset: 0x000CF9E3
		private IEnumerator CExpire()
		{
			this._running = true;
			yield return this._owner.chronometer.master.WaitForSeconds(this._duration);
			this._running = false;
			this.Stop();
			yield break;
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x000D17F4 File Offset: 0x000CF9F4
		public override void Stop()
		{
			this._running = false;
			if (this._teamCached != null)
			{
				foreach (Character character in this._teamCached)
				{
					character.health.onTakeDamage.Remove(new TakeDamageDelegate(this.Block));
				}
			}
			Character owner = this._owner;
			if (owner == null)
			{
				return;
			}
			owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.Block));
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x000D1894 File Offset: 0x000CFA94
		private List<Character> FindTeamBody(Collider2D collider)
		{
			collider.enabled = true;
			List<Character> components = GuardForTeam._teamOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
			if (components.Count == 0)
			{
				collider.enabled = false;
				return null;
			}
			return components;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x000D18CC File Offset: 0x000CFACC
		private void GiveGuardBuff()
		{
			List<Character> list = this.FindTeamBody(this._guardBuffRange);
			if (list == null)
			{
				foreach (Character character in this._teamCached)
				{
					character.health.onTakeDamage.Remove(new TakeDamageDelegate(this.Block));
				}
			}
			foreach (Character character2 in this._teamCached)
			{
				if (!list.Contains(character2))
				{
					character2.health.onTakeDamage.Remove(new TakeDamageDelegate(this.Block));
				}
			}
			foreach (Character character3 in list)
			{
				if (!character3.health.onTakeDamage.Contains(new TakeDamageDelegate(this.Block)))
				{
					character3.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.Block));
				}
			}
			this._teamCached.Clear();
			this._teamCached.AddRange(list);
		}

		// Token: 0x0400370D RID: 14093
		[SerializeField]
		private Collider2D _guardRange;

		// Token: 0x0400370E RID: 14094
		[SerializeField]
		private Collider2D _guardBuffRange;

		// Token: 0x0400370F RID: 14095
		[SerializeField]
		private float _duration;

		// Token: 0x04003710 RID: 14096
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _onHitToOwner;

		// Token: 0x04003711 RID: 14097
		[SerializeField]
		private ChronoInfo _onHitToOwnerChronoInfo;

		// Token: 0x04003712 RID: 14098
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onHitToOwnerFromRangeAttack;

		// Token: 0x04003713 RID: 14099
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onHitToTarget;

		// Token: 0x04003714 RID: 14100
		private Character _owner;

		// Token: 0x04003715 RID: 14101
		private static readonly NonAllocOverlapper _teamOverlapper = new NonAllocOverlapper(6);

		// Token: 0x04003716 RID: 14102
		private bool _running;

		// Token: 0x04003717 RID: 14103
		private List<Character> _teamCached;
	}
}

using System;
using System.Collections;
using Characters.AI;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FD6 RID: 4054
	public class EnergyCorps : CharacterOperation
	{
		// Token: 0x06004E64 RID: 20068 RVA: 0x000EACAB File Offset: 0x000E8EAB
		public override void Initialize()
		{
			base.Initialize();
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x000EACBF File Offset: 0x000E8EBF
		public override void Run(Character owner)
		{
			this._hitHistoryManager = (this._fireEnergyCrops.group ? new HitHistoryManager(15) : null);
			this._cReference = base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x000EACF1 File Offset: 0x000E8EF1
		private IEnumerator CRun(Character owner)
		{
			Character target = this._controller.target;
			foreach (object obj in this._energyCorpsContainer)
			{
				Transform transform = (Transform)obj;
				float y;
				if (this._fireEnergyCrops.platformTarget)
				{
					y = target.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
				}
				else
				{
					y = target.transform.position.y + target.collider.bounds.extents.y;
				}
				Vector3 vector = new Vector3(target.transform.position.x, y) - transform.transform.position;
				float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				transform.rotation = Quaternion.Euler(0f, 0f, z);
				transform.gameObject.SetActive(false);
				this.FireProjectile(owner, transform);
				yield return owner.chronometer.master.WaitForSeconds(this._interval);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x000EAD08 File Offset: 0x000E8F08
		private void FireProjectile(Character owner, Transform firePosition)
		{
			CustomAngle[] values = this._fireEnergyCrops.directions.values;
			if (this._fireEnergyCrops.directionType == EnergyCorps.FireEnergyCrops.DirectionType.RotationOfFirePosition)
			{
				for (int i = 0; i < values.Length; i++)
				{
					this._fireEnergyCrops.projectile.reusable.Spawn(firePosition.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, firePosition.localRotation.eulerAngles.z + values[i].value, firePosition.lossyScale.x < 0f, false, 1f, null, 0f);
				}
				return;
			}
			if (this._fireEnergyCrops.directionType == EnergyCorps.FireEnergyCrops.DirectionType.OwnerDirection)
			{
				for (int j = 0; j < values.Length; j++)
				{
					this._fireEnergyCrops.projectile.reusable.Spawn(firePosition.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, values[j].value, owner.lookingDirection == Character.LookingDirection.Left, false, 1f, null, 0f);
				}
				return;
			}
			for (int k = 0; k < values.Length; k++)
			{
				this._fireEnergyCrops.projectile.reusable.Spawn(firePosition.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, values[k].value, false, false, 1f, this._fireEnergyCrops.group ? this._hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x000EAE89 File Offset: 0x000E9089
		public override void Stop()
		{
			base.Stop();
			if (this._cReference != null)
			{
				base.StopCoroutine(this._cReference);
			}
		}

		// Token: 0x04003E76 RID: 15990
		[SerializeField]
		private float _interval;

		// Token: 0x04003E77 RID: 15991
		[SerializeField]
		private AIController _controller;

		// Token: 0x04003E78 RID: 15992
		[SerializeField]
		private Transform _energyCorpsContainer;

		// Token: 0x04003E79 RID: 15993
		[SerializeField]
		[Header("Fire Projectile")]
		private EnergyCorps.FireEnergyCrops _fireEnergyCrops;

		// Token: 0x04003E7A RID: 15994
		private HitHistoryManager _hitHistoryManager;

		// Token: 0x04003E7B RID: 15995
		private IAttackDamage _attackDamage;

		// Token: 0x04003E7C RID: 15996
		private Coroutine _cReference;

		// Token: 0x02000FD7 RID: 4055
		[Serializable]
		private class FireEnergyCrops
		{
			// Token: 0x04003E7D RID: 15997
			[SerializeField]
			internal Projectile projectile;

			// Token: 0x04003E7E RID: 15998
			[SerializeField]
			internal bool group;

			// Token: 0x04003E7F RID: 15999
			[SerializeField]
			internal bool platformTarget;

			// Token: 0x04003E80 RID: 16000
			[SerializeField]
			internal EnergyCorps.FireEnergyCrops.DirectionType directionType;

			// Token: 0x04003E81 RID: 16001
			[SerializeField]
			internal CustomAngle.Reorderable directions;

			// Token: 0x02000FD8 RID: 4056
			internal enum DirectionType
			{
				// Token: 0x04003E83 RID: 16003
				RotationOfFirePosition,
				// Token: 0x04003E84 RID: 16004
				OwnerDirection,
				// Token: 0x04003E85 RID: 16005
				Constant
			}
		}
	}
}

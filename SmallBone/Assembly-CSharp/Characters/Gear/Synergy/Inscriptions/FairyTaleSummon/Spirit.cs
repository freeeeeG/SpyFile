using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Abilities.Constraints;
using Characters.Operations;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions.FairyTaleSummon
{
	// Token: 0x020008D4 RID: 2260
	public class Spirit : MonoBehaviour
	{
		// Token: 0x06003025 RID: 12325 RVA: 0x000906DF File Offset: 0x0008E8DF
		private void Awake()
		{
			this._detectRange.enabled = false;
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x000906ED File Offset: 0x0008E8ED
		private void OnEnable()
		{
			base.StartCoroutine(this.CRun());
			this.ResetPosition();
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.ResetPosition;
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x0009071D File Offset: 0x0008E91D
		private void OnDisable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.ResetPosition;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x0009073A File Offset: 0x0008E93A
		private void ResetPosition()
		{
			base.transform.position = this._slot.transform.position;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x00090757 File Offset: 0x0008E957
		public void Initialize(Character owner)
		{
			this._owner = owner;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x00090760 File Offset: 0x0008E960
		public void Set(int attackCooldown, RuntimeAnimatorController graphic)
		{
			this._attackCooldown = (float)attackCooldown;
			this._animator.runtimeAnimatorController = graphic;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x00090778 File Offset: 0x0008E978
		private bool FindTarget(out Target target)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(this._owner.gameObject));
			this._detectRange.enabled = true;
			this._overlapper.OverlapCollider(this._detectRange);
			this._detectRange.enabled = false;
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				target = null;
				return false;
			}
			target = components[0];
			return true;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000907FE File Offset: 0x0008E9FE
		private void Move(float deltaTime)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this._slot.transform.position, deltaTime * this._trackSpeed);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x00090833 File Offset: 0x0008EA33
		private IEnumerator CRun()
		{
			float time = 0f;
			for (;;)
			{
				if (time >= this._attackCooldown)
				{
					Target target;
					if (!this._constraints.components.Pass())
					{
						time -= this._owner.chronometer.master.deltaTime;
						yield return null;
					}
					else if (!this.FindTarget(out target))
					{
						time = this._attackCooldown - 0.25f;
					}
					else
					{
						time = 0f;
						yield return this.CSpawnOperationRunner(target);
					}
				}
				else
				{
					yield return null;
					float deltaTime = this._owner.chronometer.master.deltaTime;
					time += deltaTime;
					this.Move(deltaTime);
				}
			}
			yield break;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x00090842 File Offset: 0x0008EA42
		private IEnumerator CSpawnOperationRunner(Target target)
		{
			Vector3 vector = target.collider.bounds.center - base.transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			OperationInfos spawnedOperationInfos = this._operationRunner.Spawn().operationInfos;
			spawnedOperationInfos.transform.SetPositionAndRotation(base.transform.position, Quaternion.Euler(0f, 0f, z));
			spawnedOperationInfos.Run(this._owner);
			while (spawnedOperationInfos.gameObject.activeSelf)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x040027E0 RID: 10208
		private Character _owner;

		// Token: 0x040027E1 RID: 10209
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint.Subcomponents _constraints;

		// Token: 0x040027E2 RID: 10210
		[SerializeField]
		private Transform _slot;

		// Token: 0x040027E3 RID: 10211
		[SerializeField]
		private float _trackSpeed = 2.5f;

		// Token: 0x040027E4 RID: 10212
		[SerializeField]
		[Space]
		private Animator _animator;

		// Token: 0x040027E5 RID: 10213
		[Space]
		[SerializeField]
		private Collider2D _detectRange;

		// Token: 0x040027E6 RID: 10214
		[Space]
		[Tooltip("오퍼레이션 프리팹")]
		[SerializeField]
		private OperationRunner _operationRunner;

		// Token: 0x040027E7 RID: 10215
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040027E8 RID: 10216
		private NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

		// Token: 0x040027E9 RID: 10217
		private float _attackCooldown;
	}
}

using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Weapons.GrimReaper
{
	// Token: 0x02000C0E RID: 3086
	public sealed class GrimReaperSoul : MonoBehaviour
	{
		// Token: 0x06003F70 RID: 16240 RVA: 0x000B7F65 File Offset: 0x000B6165
		public void Spawn(Vector3 postion, GrimReaperHarvestPassive grimReaperHarvestPassive)
		{
			GrimReaperSoul component = this._poolObject.Spawn(postion, true).GetComponent<GrimReaperSoul>();
			component._grimReperPassive = null;
			component._grimReaperHarvestPassive = grimReaperHarvestPassive;
			component.MoveToGrimReaper();
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x000B7F8C File Offset: 0x000B618C
		public void Spawn(Vector3 postion, GrimReaperPassive grimReaperPassive)
		{
			GrimReaperSoul component = this._poolObject.Spawn(postion, true).GetComponent<GrimReaperSoul>();
			component._grimReaperHarvestPassive = null;
			component._grimReperPassive = grimReaperPassive;
			component.MoveToGrimReaper();
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x000B7FB3 File Offset: 0x000B61B3
		private void MoveToGrimReaper()
		{
			this._onGain.Initialize();
			base.StartCoroutine(this.CMoveToGrimReaper());
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x000B7FCD File Offset: 0x000B61CD
		private IEnumerator CMoveToGrimReaper()
		{
			this._target = ((this._grimReperPassive == null) ? this._grimReaperHarvestPassive.owner : this._grimReperPassive.owner);
			this._directionVector = (base.transform.position - this._target.transform.position).normalized;
			this._directionVector = Quaternion.AngleAxis(UnityEngine.Random.Range(-this._startRotationRange, this._startRotationRange), Vector3.forward) * this._directionVector;
			this._direction = Mathf.Atan2(this._directionVector.y, this._directionVector.x) * 57.29578f;
			this._rotation = Quaternion.Euler(0f, 0f, this._direction);
			this._currentRotateSpeed = this._rotateSpeed;
			this._elapsed = 0f;
			for (;;)
			{
				yield return null;
				if (this._target == null)
				{
					this.Despawn();
				}
				float deltaTime = Chronometer.global.deltaTime;
				this._elapsed += deltaTime;
				if (this._elapsed >= this._delay)
				{
					this.UpdateDirection(deltaTime);
					this._currentRotateSpeed += deltaTime * 4f;
					if (Vector2.SqrMagnitude(base.transform.position - this._target.collider.bounds.center) < 1f)
					{
						break;
					}
				}
				float d = this._startSpeed + this._speedIncreasing.Evaluate(this._elapsed) * deltaTime;
				base.transform.Translate(this._directionVector * d, Space.World);
			}
			this.PickedUp();
			yield break;
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x000B7FDC File Offset: 0x000B61DC
		private void PickedUp()
		{
			if (this._grimReperPassive != null)
			{
				this._grimReperPassive.owner.StartCoroutine(this._onGain.CRun(this._grimReperPassive.owner));
				this._grimReperPassive.AddStack();
			}
			else
			{
				this._grimReaperHarvestPassive.owner.StartCoroutine(this._onGain.CRun(this._grimReaperHarvestPassive.owner));
				this._grimReaperHarvestPassive.AddStack();
			}
			this.Despawn();
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x000B805D File Offset: 0x000B625D
		private void Despawn()
		{
			this._poolObject.Despawn();
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x000B806C File Offset: 0x000B626C
		private void UpdateDirection(float deltaTime)
		{
			if (this._target == null)
			{
				return;
			}
			Vector3 vector = this._target.collider.bounds.center - base.transform.position;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			switch (this._rotateMethod)
			{
			case GrimReaperSoul.RotateMethod.Constant:
				this._rotation = Quaternion.RotateTowards(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._currentRotateSpeed * 100f * deltaTime);
				break;
			case GrimReaperSoul.RotateMethod.Lerp:
				this._rotation = Quaternion.Lerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._currentRotateSpeed * deltaTime);
				break;
			case GrimReaperSoul.RotateMethod.Slerp:
				this._rotation = Quaternion.Slerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), this._currentRotateSpeed * deltaTime);
				break;
			}
			this._direction = this._rotation.eulerAngles.z;
			this._directionVector = this._rotation * Vector3.right;
		}

		// Token: 0x040030CB RID: 12491
		private GrimReaperHarvestPassive _grimReaperHarvestPassive;

		// Token: 0x040030CC RID: 12492
		private GrimReaperPassive _grimReperPassive;

		// Token: 0x040030CD RID: 12493
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onGain;

		// Token: 0x040030CE RID: 12494
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x040030CF RID: 12495
		[SerializeField]
		private float _delay;

		// Token: 0x040030D0 RID: 12496
		[SerializeField]
		private GrimReaperSoul.RotateMethod _rotateMethod;

		// Token: 0x040030D1 RID: 12497
		[SerializeField]
		private float _rotateSpeed = 2f;

		// Token: 0x040030D2 RID: 12498
		[SerializeField]
		private float _startRotationRange = 45f;

		// Token: 0x040030D3 RID: 12499
		[SerializeField]
		private float _startSpeed;

		// Token: 0x040030D4 RID: 12500
		[SerializeField]
		private Curve _speedIncreasing;

		// Token: 0x040030D5 RID: 12501
		private Character _target;

		// Token: 0x040030D6 RID: 12502
		private Vector2 _directionVector;

		// Token: 0x040030D7 RID: 12503
		private float _direction;

		// Token: 0x040030D8 RID: 12504
		private Quaternion _rotation;

		// Token: 0x040030D9 RID: 12505
		private float _currentRotateSpeed;

		// Token: 0x040030DA RID: 12506
		private float _elapsed;

		// Token: 0x02000C0F RID: 3087
		public enum RotateMethod
		{
			// Token: 0x040030DC RID: 12508
			Constant,
			// Token: 0x040030DD RID: 12509
			Lerp,
			// Token: 0x040030DE RID: 12510
			Slerp
		}
	}
}

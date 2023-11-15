using System;
using System.Collections;
using System.Collections.Generic;
using FX;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FCB RID: 4043
	public class MultiShadowBunshin : CharacterOperation
	{
		// Token: 0x06004E43 RID: 20035 RVA: 0x000EA3A8 File Offset: 0x000E85A8
		private void Awake()
		{
			this._originIndics = new HashSet<int>();
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x000EA3B5 File Offset: 0x000E85B5
		public override void Run(Character owner)
		{
			this.UpdateOriginIndics();
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x000EA3CB File Offset: 0x000E85CB
		private IEnumerator CRun(Character owner)
		{
			Bounds platform = owner.movement.controller.collisionState.lastStandingCollider.bounds;
			int num;
			for (int i = 0; i < this._totalCount; i = num + 1)
			{
				Vector3 vector = Vector2.zero;
				bool flag = i >= this._totalCount / 2;
				Vector2 v;
				if (flag)
				{
					v = new Vector2(UnityEngine.Random.Range(platform.center.x, platform.max.x), platform.max.y);
					vector.z = (180f - vector.z) % 360f;
				}
				else
				{
					v = new Vector2(UnityEngine.Random.Range(platform.min.x, platform.center.x), platform.max.y);
				}
				OperationRunner operationRunner;
				if (this._originIndics.Contains(i))
				{
					operationRunner = this._origin;
				}
				else
				{
					operationRunner = this._fake;
				}
				OperationInfos operationInfos = operationRunner.Spawn().operationInfos;
				operationInfos.transform.SetPositionAndRotation(v, Quaternion.Euler(vector));
				if (flag)
				{
					operationInfos.transform.localScale = new Vector3(1f, -1f, 1f);
				}
				else
				{
					operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				this._spawnEffect.Spawn(v, 0f, 1f);
				operationInfos.Run(owner);
				yield return owner.chronometer.master.WaitForSeconds(this._delay);
				num = i;
			}
			yield break;
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x000EA3E4 File Offset: 0x000E85E4
		private void UpdateOriginIndics()
		{
			this._originIndics.Clear();
			for (int i = 0; i < this._originCount; i++)
			{
				int item = UnityEngine.Random.Range(0, this._totalCount);
				while (this._originIndics.Contains(item))
				{
					item = UnityEngine.Random.Range(0, this._totalCount);
				}
				this._originIndics.Add(item);
			}
		}

		// Token: 0x04003E49 RID: 15945
		[SerializeField]
		private EffectInfo _spawnEffect;

		// Token: 0x04003E4A RID: 15946
		[SerializeField]
		private OperationRunner _origin;

		// Token: 0x04003E4B RID: 15947
		[SerializeField]
		private OperationRunner _fake;

		// Token: 0x04003E4C RID: 15948
		[SerializeField]
		private int _totalCount;

		// Token: 0x04003E4D RID: 15949
		[SerializeField]
		private int _originCount;

		// Token: 0x04003E4E RID: 15950
		[SerializeField]
		private float _delay;

		// Token: 0x04003E4F RID: 15951
		private HashSet<int> _originIndics;
	}
}

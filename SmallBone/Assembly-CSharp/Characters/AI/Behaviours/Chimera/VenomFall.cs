using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using Data;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001380 RID: 4992
	public class VenomFall : Behaviour
	{
		// Token: 0x0600626D RID: 25197 RVA: 0x0011EEE8 File Offset: 0x0011D0E8
		private void Awake()
		{
			for (int i = 0; i < 4; i++)
			{
				this._operations[i].Initialize();
			}
			this._readyOperations.Initialize();
			this._roarOperations.Initialize();
			this._endOperationsInHardmode.Initialize();
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x0011EF30 File Offset: 0x0011D130
		public void ShuffleOrder()
		{
			int[] array = new int[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = i;
			}
			array.Shuffle<int>();
			this._order = new Queue<int>(4);
			for (int j = 0; j < 4; j++)
			{
				this._order.Enqueue(array[j]);
			}
			this.SetPoints();
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x0011EF86 File Offset: 0x0011D186
		public void Ready(Character character)
		{
			this.ShuffleOrder();
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
			base.StartCoroutine(this.CoolDown(character.chronometer.animation));
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x0011EFC3 File Offset: 0x0011D1C3
		public void Roar(Character character)
		{
			this._roarOperations.gameObject.SetActive(true);
			this._roarOperations.Run(character);
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x00002191 File Offset: 0x00000391
		public void EndRoar(Character character)
		{
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x0011EFE2 File Offset: 0x0011D1E2
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character character = controller.character;
			if (!this._running)
			{
				base.StartCoroutine(this.CStartFall(character));
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06006273 RID: 25203 RVA: 0x0011EFF8 File Offset: 0x0011D1F8
		private IEnumerator CStartFall(Character owner)
		{
			float term = 0.8f;
			float elapsed = 0f;
			this._running = true;
			while (this._order.Count > 0)
			{
				yield return null;
				elapsed += Chronometer.global.deltaTime;
				if (elapsed > term)
				{
					elapsed -= term;
					this.Next(owner);
				}
			}
			this._running = false;
			yield break;
		}

		// Token: 0x06006274 RID: 25204 RVA: 0x0011F010 File Offset: 0x0011D210
		private void Next(Character owner)
		{
			int num = this._order.Dequeue();
			this._operations[num].gameObject.SetActive(true);
			this._operations[num].Run(owner);
			if (this._order.Count == 1)
			{
				int num2 = this._order.Dequeue();
				this._operations[num2].gameObject.SetActive(true);
				this._operations[num2].Run(owner);
				if (GameData.HardmodeProgress.hardmode)
				{
					base.StartCoroutine(this.CStartEndFall(owner));
				}
			}
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x0011F09A File Offset: 0x0011D29A
		private IEnumerator CStartEndFall(Character owner)
		{
			yield return Chronometer.global.WaitForSeconds(this._endOperationTimingFromStart);
			if (owner.health.dead)
			{
				yield break;
			}
			this._endOperationsInHardmode.gameObject.SetActive(true);
			this._endOperationsInHardmode.Run(owner);
			yield break;
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x0011F0B0 File Offset: 0x0011D2B0
		private void SetPoints()
		{
			for (int i = 0; i < this.points.childCount; i++)
			{
				float num = UnityEngine.Random.Range(0f, this._range);
				this.points.GetChild(i).position = new Vector2(this._startPoint.transform.position.x + num + (this._range + this._term) * (float)i, this._startPoint.transform.position.y);
			}
		}

		// Token: 0x06006277 RID: 25207 RVA: 0x0011F13C File Offset: 0x0011D33C
		private IEnumerator CoolDown(Chronometer chronometer)
		{
			this._coolDown = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this._coolDown = true;
			yield break;
		}

		// Token: 0x06006278 RID: 25208 RVA: 0x0011F152 File Offset: 0x0011D352
		public bool CanUse()
		{
			return this._coolDown;
		}

		// Token: 0x04004F5F RID: 20319
		[SerializeField]
		private float _coolTime;

		// Token: 0x04004F60 RID: 20320
		[Header("Ready")]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04004F61 RID: 20321
		[Header("Roar")]
		[SerializeField]
		private OperationInfos _roarOperations;

		// Token: 0x04004F62 RID: 20322
		[SerializeField]
		[Header("Fire")]
		private OperationInfos[] _operations;

		// Token: 0x04004F63 RID: 20323
		[SerializeField]
		[Header("Hardmode End")]
		private float _endOperationTimingFromStart = 3.55f;

		// Token: 0x04004F64 RID: 20324
		[SerializeField]
		private OperationInfos _endOperationsInHardmode;

		// Token: 0x04004F65 RID: 20325
		[SerializeField]
		private float _term = 2f;

		// Token: 0x04004F66 RID: 20326
		[SerializeField]
		[Range(1f, 10f)]
		private float _range;

		// Token: 0x04004F67 RID: 20327
		[SerializeField]
		private Transform points;

		// Token: 0x04004F68 RID: 20328
		[SerializeField]
		private Transform _startPoint;

		// Token: 0x04004F69 RID: 20329
		private const int _maxOrder = 4;

		// Token: 0x04004F6A RID: 20330
		private Queue<int> _order;

		// Token: 0x04004F6B RID: 20331
		private bool _coolDown = true;

		// Token: 0x04004F6C RID: 20332
		private bool _running;
	}
}

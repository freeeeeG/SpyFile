using System;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;

namespace Characters.Gear.Weapons.Rockstar
{
	// Token: 0x02000840 RID: 2112
	public class Amp : MonoBehaviour
	{
		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06002BEC RID: 11244 RVA: 0x00086A14 File Offset: 0x00084C14
		// (set) Token: 0x06002BED RID: 11245 RVA: 0x00086A1C File Offset: 0x00084C1C
		public int beat
		{
			get
			{
				return this._beat;
			}
			private set
			{
				this._beat = value;
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06002BEE RID: 11246 RVA: 0x00086A28 File Offset: 0x00084C28
		// (remove) Token: 0x06002BEF RID: 11247 RVA: 0x00086A60 File Offset: 0x00084C60
		public event Action onInstantiate;

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x00086A95 File Offset: 0x00084C95
		public bool ampExists
		{
			get
			{
				return this._ampObjects.Count > 0;
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x00086AA8 File Offset: 0x00084CA8
		private void Awake()
		{
			List<OperationInfos> list = new List<OperationInfos>();
			foreach (Amp.Timing timing in this._timings)
			{
				if (!list.Contains(timing.operation))
				{
					list.Add(timing.operation);
					timing.operation.Initialize();
				}
			}
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x00086AFC File Offset: 0x00084CFC
		public void InstantiateAmp(Vector3 position, bool flipX)
		{
			OperationRunner operationRunner = this._ampOriginal.Spawn();
			operationRunner.transform.SetPositionAndRotation(position, Quaternion.identity);
			int num = (this._weapon.owner.lookingDirection == Character.LookingDirection.Left ^ flipX) ? -1 : 1;
			operationRunner.transform.localScale = new Vector3((float)num, 1f, 1f);
			operationRunner.GetComponent<SpriteRenderer>().flipX = flipX;
			operationRunner.operationInfos.Run(this._weapon.owner);
			this._ampObjects.Add(operationRunner);
			Action action = this.onInstantiate;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002BF3 RID: 11251 RVA: 0x00086B9C File Offset: 0x00084D9C
		public void PlayAmpBeat(int index)
		{
			for (int i = this._ampObjects.Count - 1; i >= 0; i--)
			{
				OperationRunner operationRunner = this._ampObjects[i];
				if (operationRunner == null || !operationRunner.gameObject.activeSelf)
				{
					this._ampObjects.Remove(operationRunner);
				}
				else
				{
					base.transform.position = operationRunner.transform.position;
					float num = (this._weapon.owner.lookingDirection == Character.LookingDirection.Right) ? 1f : -1f;
					num *= operationRunner.transform.localScale.x;
					base.transform.localScale = new Vector3(num, 1f, 1f);
					this._timings[index].Run(this._weapon.owner);
				}
			}
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00086C74 File Offset: 0x00084E74
		public float[] GetTimings()
		{
			float[] array = new float[this._timings.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this._timings[i].timming / (float)this.beat;
			}
			return array;
		}

		// Token: 0x04002525 RID: 9509
		[GetComponentInParent(false)]
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04002526 RID: 9510
		[SerializeField]
		[Tooltip("앰프 프리팹")]
		private OperationRunner _ampOriginal;

		// Token: 0x04002527 RID: 9511
		[SerializeField]
		[Tooltip("몇 박자마다 반복 할 것인지 기입")]
		private int _beat = 1;

		// Token: 0x04002528 RID: 9512
		[SerializeField]
		[Tooltip("Beat에서 지정 한 박자 내에서 아래 지정한 백분률 지점마다 정해준 OperationInfos를 실행하게 됨\n예를 들어 Beat가 2인 상태로 0, 0.5 두 지점에서 동일한 OpeartionInfos를 실행하면 한 박자에 한 번씩 실행 됨")]
		private Amp.Timing[] _timings;

		// Token: 0x04002529 RID: 9513
		private List<OperationRunner> _ampObjects = new List<OperationRunner>();

		// Token: 0x02000841 RID: 2113
		[Serializable]
		private class Timing
		{
			// Token: 0x1700091B RID: 2331
			// (get) Token: 0x06002BF6 RID: 11254 RVA: 0x00086CD0 File Offset: 0x00084ED0
			public OperationInfos operation
			{
				get
				{
					return this._operation;
				}
			}

			// Token: 0x1700091C RID: 2332
			// (get) Token: 0x06002BF7 RID: 11255 RVA: 0x00086CD8 File Offset: 0x00084ED8
			public float timming
			{
				get
				{
					return this._timing;
				}
			}

			// Token: 0x06002BF8 RID: 11256 RVA: 0x00086CE0 File Offset: 0x00084EE0
			public void Initialize()
			{
				this._operation.Initialize();
			}

			// Token: 0x06002BF9 RID: 11257 RVA: 0x00086CED File Offset: 0x00084EED
			public void Run(Character owner)
			{
				this._operation.gameObject.SetActive(true);
				this._operation.Run(owner);
			}

			// Token: 0x0400252B RID: 9515
			[SerializeField]
			private OperationInfos _operation;

			// Token: 0x0400252C RID: 9516
			[SerializeField]
			private float _timing;
		}
	}
}

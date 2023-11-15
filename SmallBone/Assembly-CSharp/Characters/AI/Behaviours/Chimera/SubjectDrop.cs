using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001377 RID: 4983
	public class SubjectDrop : Behaviour
	{
		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06006237 RID: 25143 RVA: 0x0011E8B1 File Offset: 0x0011CAB1
		// (set) Token: 0x06006238 RID: 25144 RVA: 0x0011E8B9 File Offset: 0x0011CAB9
		public bool canUse { get; set; } = true;

		// Token: 0x06006239 RID: 25145 RVA: 0x0011E8C4 File Offset: 0x0011CAC4
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._roarOperations.Initialize();
			this._endOperations.Initialize();
			for (int i = 0; i < this._fireOperationInfos.Length; i++)
			{
				this._fireOperationInfos[i].Initialize();
			}
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x0011E912 File Offset: 0x0011CB12
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x0011E931 File Offset: 0x0011CB31
		public void Roar(Character character)
		{
			this._roarOperations.gameObject.SetActive(true);
			this._roarOperations.Run(character);
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x0011E950 File Offset: 0x0011CB50
		public void End(Character character)
		{
			this._endOperations.gameObject.SetActive(true);
			this._endOperations.Run(character);
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x0011E96F File Offset: 0x0011CB6F
		public void Run(AIController controller)
		{
			if (this._coolDown != null)
			{
				base.StopCoroutine(this._coolDown);
			}
			base.StartCoroutine(this.CRun(controller));
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x0011E993 File Offset: 0x0011CB93
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._coolDown = base.StartCoroutine(this.CoolDown(controller.character.chronometer.animation));
			this.SetPoints();
			this._fireOperationInfos.Shuffle<OperationInfos>();
			int num;
			for (int i = 0; i < this._fireOperationInfos.Length; i = num + 1)
			{
				this._fireOperationInfos[i].gameObject.SetActive(true);
				this._fireOperationInfos[i].Run(controller.character);
				yield return Chronometer.global.WaitForSeconds(0.5f);
				num = i;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x0011E9AC File Offset: 0x0011CBAC
		private void SetPoints()
		{
			for (int i = 0; i < this.points.childCount; i++)
			{
				float num = UnityEngine.Random.Range(0f, this._range);
				this.points.GetChild(i).position = new Vector2(this._startPoint.transform.position.x + num + (this._range + this._term) * (float)i, this._height);
			}
		}

		// Token: 0x06006240 RID: 25152 RVA: 0x0011EA29 File Offset: 0x0011CC29
		private IEnumerator CoolDown(Chronometer chronometer)
		{
			this.canUse = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this.canUse = true;
			yield break;
		}

		// Token: 0x04004F34 RID: 20276
		[SerializeField]
		[Header("Ready")]
		private OperationInfos _readyOperations;

		// Token: 0x04004F35 RID: 20277
		[SerializeField]
		[Header("Roar")]
		private OperationInfos _roarOperations;

		// Token: 0x04004F36 RID: 20278
		[SerializeField]
		[Header("Fire")]
		private SequentialAction _fireSequencialAction;

		// Token: 0x04004F37 RID: 20279
		[SerializeField]
		private OperationInfos[] _fireOperationInfos;

		// Token: 0x04004F38 RID: 20280
		[SerializeField]
		[Header("End")]
		private OperationInfos _endOperations;

		// Token: 0x04004F39 RID: 20281
		[SerializeField]
		private float _coolTime;

		// Token: 0x04004F3A RID: 20282
		[SerializeField]
		private float _term = 2f;

		// Token: 0x04004F3B RID: 20283
		[Range(1f, 10f)]
		[SerializeField]
		private float _range;

		// Token: 0x04004F3C RID: 20284
		[SerializeField]
		private float _height;

		// Token: 0x04004F3D RID: 20285
		[SerializeField]
		private Transform points;

		// Token: 0x04004F3E RID: 20286
		[SerializeField]
		private Transform _startPoint;

		// Token: 0x04004F3F RID: 20287
		private Coroutine _coolDown;
	}
}

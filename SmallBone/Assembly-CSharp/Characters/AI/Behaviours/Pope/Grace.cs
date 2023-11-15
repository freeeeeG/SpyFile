using System;
using System.Collections;
using Characters.Actions;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001353 RID: 4947
	public sealed class Grace : Behaviour
	{
		// Token: 0x06006183 RID: 24963 RVA: 0x0011D311 File Offset: 0x0011B511
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			this._ready.TryStart();
			while (this._ready.running)
			{
				yield return null;
			}
			int num;
			for (int i = 0; i < 3; i = num + 1)
			{
				this.SetNoneTarget();
				this._attack.TryStart();
				while (this._attack.running)
				{
					yield return null;
				}
				num = i;
			}
			this._end.TryStart();
			while (this._end.running)
			{
				yield return null;
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x0011D328 File Offset: 0x0011B528
		public void SetNoneTarget()
		{
			int num = this._lineEffects.Length;
			int num2 = Mathf.FloorToInt((float)((this._lineEffects.Length - 1) / 2));
			float delta = (float)(180 / num);
			float margin = 30f;
			float num3 = 15f;
			this.SetAngle(num, num2, delta, margin, num3);
			if (num % 2 != 0)
			{
				float num4 = UnityEngine.Random.Range(0f, num3);
				float num5 = (float)(MMMaths.RandomBool() ? 1 : -1);
				this._lineEffects[num2].transform.rotation = Quaternion.AngleAxis(270f + num5 * num4, Vector3.forward);
			}
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x0011D3BC File Offset: 0x0011B5BC
		private void SetAngle(int n, int mid, float delta, float margin, float padding)
		{
			float num = 180f + margin;
			float num2 = UnityEngine.Random.Range(0f, padding);
			this._lineEffects[0].transform.rotation = Quaternion.AngleAxis(num + num2, Vector3.forward);
			num += delta;
			for (int i = 1; i <= mid; i++)
			{
				num2 = UnityEngine.Random.Range(-padding, delta - padding);
				this._lineEffects[i].transform.rotation = Quaternion.AngleAxis(num + num2, Vector3.forward);
				num += delta;
			}
			num = 360f - margin;
			num2 = UnityEngine.Random.Range(0f, padding);
			this._lineEffects[mid + 1].transform.rotation = Quaternion.AngleAxis(num - num2, Vector3.forward);
			num -= delta;
			for (int j = mid + 2; j < n; j++)
			{
				num2 = UnityEngine.Random.Range(-padding, delta - padding);
				this._lineEffects[j].transform.rotation = Quaternion.AngleAxis(num - num2, Vector3.forward);
				num -= delta;
			}
		}

		// Token: 0x04004EA2 RID: 20130
		[SerializeField]
		private Characters.Actions.Action _ready;

		// Token: 0x04004EA3 RID: 20131
		[SerializeField]
		private Characters.Actions.Action _attack;

		// Token: 0x04004EA4 RID: 20132
		[SerializeField]
		private Characters.Actions.Action _end;

		// Token: 0x04004EA5 RID: 20133
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		private MoveHandler _moveHandler;

		// Token: 0x04004EA6 RID: 20134
		[MinMaxSlider(0f, 180f)]
		[SerializeField]
		private Vector2 _radianRangeFromPlayerTarget;

		// Token: 0x04004EA7 RID: 20135
		[SerializeField]
		private LineEffect[] _lineEffects;

		// Token: 0x04004EA8 RID: 20136
		private const int _count = 3;
	}
}

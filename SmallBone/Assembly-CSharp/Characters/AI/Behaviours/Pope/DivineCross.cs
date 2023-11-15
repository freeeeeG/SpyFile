using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using Level.Chapter4;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200134A RID: 4938
	public sealed class DivineCross : Behaviour
	{
		// Token: 0x06006160 RID: 24928 RVA: 0x0011CEA5 File Offset: 0x0011B0A5
		private void Awake()
		{
			this._platforms = new Platform[this._points.Length + 1];
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x0011CEBC File Offset: 0x0011B0BC
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._moveHandler.CMove(controller);
			this.SettleOnDestination();
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x0011CED4 File Offset: 0x0011B0D4
		private void SettleOnDestination()
		{
			this._platformContainer.RandomTakeTo(this._platforms);
			for (int i = 0; i < this._points.Length; i++)
			{
				this._points[i].attackPoint.position = this._platforms[i].transform.position;
				this._points[i].firePoint.position = this._platforms[i].GetFirePosition();
			}
			this._crossPoint.attackPoint.position = this._platformContainer.center.transform.position;
			this._crossPoint.firePoint.position = this._platformContainer.center.GetFirePosition();
		}

		// Token: 0x04004E81 RID: 20097
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004E82 RID: 20098
		[UnityEditor.Subcomponent(typeof(MoveHandler))]
		[SerializeField]
		private MoveHandler _moveHandler;

		// Token: 0x04004E83 RID: 20099
		[SerializeField]
		private PlatformContainer _platformContainer;

		// Token: 0x04004E84 RID: 20100
		private Platform[] _platforms;

		// Token: 0x04004E85 RID: 20101
		[Header("Points")]
		[SerializeField]
		private DivineCross.Point _crossPoint;

		// Token: 0x04004E86 RID: 20102
		[SerializeField]
		private DivineCross.Point[] _points;

		// Token: 0x0200134B RID: 4939
		[Serializable]
		private class Point
		{
			// Token: 0x04004E87 RID: 20103
			[SerializeField]
			internal Transform attackPoint;

			// Token: 0x04004E88 RID: 20104
			[SerializeField]
			internal Transform firePoint;
		}
	}
}

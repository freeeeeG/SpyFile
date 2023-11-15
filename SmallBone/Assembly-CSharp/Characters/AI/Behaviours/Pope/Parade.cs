using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations;
using Level.Chapter4;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x02001346 RID: 4934
	public sealed class Parade : Behaviour
	{
		// Token: 0x0600614E RID: 24910 RVA: 0x0011CD05 File Offset: 0x0011AF05
		private void Awake()
		{
			this._scenario.OnPhase1End += delegate()
			{
				this._operations.StopAll();
			};
			this._operations.Initialize();
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x0011CD29 File Offset: 0x0011AF29
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			if (controller.target.transform.position.x > controller.transform.position.x)
			{
				this._toSpawnPoint.position = this._rightSpawnPoint.position;
			}
			else
			{
				this._toSpawnPoint.position = this._leftSpawnPoint.position;
			}
			base.StartCoroutine(this._operations.CRun(controller.character));
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x04004E72 RID: 20082
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004E73 RID: 20083
		[SerializeField]
		private Transform _toSpawnPoint;

		// Token: 0x04004E74 RID: 20084
		[SerializeField]
		private Transform _leftSpawnPoint;

		// Token: 0x04004E75 RID: 20085
		[SerializeField]
		private Transform _rightSpawnPoint;

		// Token: 0x04004E76 RID: 20086
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04004E77 RID: 20087
		[SerializeField]
		private Scenario _scenario;
	}
}

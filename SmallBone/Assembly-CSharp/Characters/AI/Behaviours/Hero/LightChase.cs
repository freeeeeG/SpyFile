using System;
using System.Collections;
using Characters.AI.Hero.LightSwords;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x02001397 RID: 5015
	public class LightChase : Behaviour
	{
		// Token: 0x060062F8 RID: 25336 RVA: 0x00120352 File Offset: 0x0011E552
		private void Awake()
		{
			this._readyGhost.Initialize();
			this._attackGhost.Initialize();
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x0012036A File Offset: 0x0011E56A
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			LightSword sword = this._helper.GetFarthestFromHero();
			if (sword == null)
			{
				base.result = Behaviour.Result.Success;
				yield break;
			}
			this._destination.position = sword.GetStuckPosition();
			sword.Sign();
			yield return this._ready.CRun(controller);
			sword.Despawn();
			this.RunOperation(this._readyGhost, controller.character);
			yield return this._attack.CRun(controller);
			int num;
			for (int i = 0; i < this._consecutiveTimes - 1; i = num + 1)
			{
				sword = this._helper.GetFarthestFromHero();
				if (sword == null)
				{
					break;
				}
				this._destination.position = sword.GetStuckPosition();
				sword.Sign();
				yield return this._shortReady.CRun(controller);
				sword.Despawn();
				this.RunOperation(this._attackGhost, controller.character);
				yield return this._attack.CRun(controller);
				num = i;
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x00120380 File Offset: 0x0011E580
		private void RunOperation(OperationInfos operation, Character owner)
		{
			operation.gameObject.SetActive(true);
			operation.Run(owner);
		}

		// Token: 0x04004FCB RID: 20427
		[SerializeField]
		private int _consecutiveTimes = 3;

		// Token: 0x04004FCC RID: 20428
		[SerializeField]
		private float _attackDelay;

		// Token: 0x04004FCD RID: 20429
		[SerializeField]
		private LightSwordFieldHelper _helper;

		// Token: 0x04004FCE RID: 20430
		[SerializeField]
		private Transform _destination;

		// Token: 0x04004FCF RID: 20431
		[SerializeField]
		private Behaviour _ready;

		// Token: 0x04004FD0 RID: 20432
		[SerializeField]
		private Behaviour _shortReady;

		// Token: 0x04004FD1 RID: 20433
		[SerializeField]
		private Behaviour _attack;

		// Token: 0x04004FD2 RID: 20434
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfos))]
		private OperationInfos _readyGhost;

		// Token: 0x04004FD3 RID: 20435
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfos))]
		private OperationInfos _attackGhost;
	}
}

using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera
{
	// Token: 0x02001374 RID: 4980
	public class Stomp : Behaviour
	{
		// Token: 0x06006223 RID: 25123 RVA: 0x0011E6BB File Offset: 0x0011C8BB
		private void Awake()
		{
			this._readyOperations.Initialize();
			this._attackOperations.Initialize();
			this._endOperations.Initialize();
			this._terrainHitOperations.Initialize();
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x0011E6E9 File Offset: 0x0011C8E9
		public void Ready(Character character)
		{
			this._readyOperations.gameObject.SetActive(true);
			this._readyOperations.Run(character);
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x0011E708 File Offset: 0x0011C908
		public void Hit(Character character)
		{
			this._terrainHitOperations.gameObject.SetActive(true);
			this._terrainHitOperations.Run(character);
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x0011E727 File Offset: 0x0011C927
		public void End(Character character)
		{
			this._endOperations.gameObject.SetActive(true);
			this._endOperations.Run(character);
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x0011E746 File Offset: 0x0011C946
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(this.CoolDown(controller.character.chronometer.master));
			this._attackOperations.gameObject.SetActive(true);
			this._attackOperations.Run(controller.character);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x0011E75C File Offset: 0x0011C95C
		private IEnumerator CoolDown(Chronometer chronometer)
		{
			this._coolDown = false;
			yield return chronometer.WaitForSeconds(this._coolTime);
			this._coolDown = true;
			yield break;
		}

		// Token: 0x06006229 RID: 25129 RVA: 0x0011E772 File Offset: 0x0011C972
		public bool CanUse(AIController controller)
		{
			return this._coolDown && controller.FindClosestPlayerBody(this._trigger) != null;
		}

		// Token: 0x04004F25 RID: 20261
		[SerializeField]
		private float _coolTime;

		// Token: 0x04004F26 RID: 20262
		[SerializeField]
		private Collider2D _trigger;

		// Token: 0x04004F27 RID: 20263
		[Header("Ready")]
		[SerializeField]
		private OperationInfos _readyOperations;

		// Token: 0x04004F28 RID: 20264
		[Header("Attack")]
		[SerializeField]
		private OperationInfos _attackOperations;

		// Token: 0x04004F29 RID: 20265
		[SerializeField]
		[Header("Hit")]
		private OperationInfos _terrainHitOperations;

		// Token: 0x04004F2A RID: 20266
		[SerializeField]
		[Header("End")]
		private OperationInfos _endOperations;

		// Token: 0x04004F2B RID: 20267
		private bool _coolDown = true;
	}
}

using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001268 RID: 4712
	public class PillarOfLight : MonoBehaviour
	{
		// Token: 0x06005D6C RID: 23916 RVA: 0x00112F06 File Offset: 0x00111106
		private void Awake()
		{
			this._attackOperations.Initialize();
		}

		// Token: 0x06005D6D RID: 23917 RVA: 0x00112F13 File Offset: 0x00111113
		public void Sign(Character owner)
		{
			this._sign.SetActive(true);
			if (!this._registered)
			{
				owner.health.onDied += delegate()
				{
					this._sign.SetActive(false);
				};
			}
			this._registered = true;
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x00112F47 File Offset: 0x00111147
		public void Attack(Character owner)
		{
			this._sign.SetActive(false);
			this._attackOperations.gameObject.SetActive(true);
			this._attackOperations.Run(owner);
		}

		// Token: 0x04004AFE RID: 19198
		[SerializeField]
		private GameObject _sign;

		// Token: 0x04004AFF RID: 19199
		[SerializeField]
		private GameObject _attack;

		// Token: 0x04004B00 RID: 19200
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _attackOperations;

		// Token: 0x04004B01 RID: 19201
		private bool _registered;
	}
}

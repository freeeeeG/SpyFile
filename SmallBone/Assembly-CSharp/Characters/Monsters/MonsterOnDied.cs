using System;
using Characters.Operations;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Monsters
{
	// Token: 0x02000815 RID: 2069
	public sealed class MonsterOnDied : MonoBehaviour
	{
		// Token: 0x06002A91 RID: 10897 RVA: 0x0008330C File Offset: 0x0008150C
		private void Awake()
		{
			this._operations.Initialize();
			this._monster.character.health.onDied += delegate()
			{
				this._monster.character.health.Revive();
				this._monster.Despawn();
			};
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x0008333A File Offset: 0x0008153A
		private void HandleOnDied()
		{
			Map.Instance.StartCoroutine(this._operations.CRun(this._monster.character));
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x0008335D File Offset: 0x0008155D
		public void DetachOnDiedEvents()
		{
			this._monster.character.health.onDied -= this.HandleOnDied;
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x00083380 File Offset: 0x00081580
		private void OnEnable()
		{
			this._monster.character.health.onDied -= this.HandleOnDied;
			this._monster.character.health.onDied += this.HandleOnDied;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000833CF File Offset: 0x000815CF
		private void OnDisable()
		{
			this.DetachOnDiedEvents();
		}

		// Token: 0x04002441 RID: 9281
		[SerializeField]
		private Monster _monster;

		// Token: 0x04002442 RID: 9282
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;
	}
}

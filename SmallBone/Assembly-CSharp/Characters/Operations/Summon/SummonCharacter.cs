using System;
using System.Collections;
using Characters.Actions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F39 RID: 3897
	public class SummonCharacter : CharacterOperation
	{
		// Token: 0x06004BCE RID: 19406 RVA: 0x000DEFEC File Offset: 0x000DD1EC
		private void Awake()
		{
			this._characterToSummon.gameObject.SetActive(false);
			this._characterToSummon.transform.parent = null;
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x000DF010 File Offset: 0x000DD210
		public override void Run(Character owner)
		{
			this._owner = owner;
			if (!this._characterToSummon.gameObject.activeSelf)
			{
				base.StartCoroutine(this.CUse());
			}
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x000DF038 File Offset: 0x000DD238
		public override void Stop()
		{
			this._characterToSummon.gameObject.SetActive(false);
		}

		// Token: 0x06004BD1 RID: 19409 RVA: 0x000DF04B File Offset: 0x000DD24B
		private void OnEnable()
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += this.Stop;
		}

		// Token: 0x06004BD2 RID: 19410 RVA: 0x000DF069 File Offset: 0x000DD269
		private void OnDisable()
		{
			this.Stop();
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.Stop;
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x000DF08D File Offset: 0x000DD28D
		private IEnumerator CUse()
		{
			this._characterToSummon.lookingDirection = this._owner.desiringLookingDirection;
			this._characterToSummon.transform.position = base.transform.position;
			this._characterToSummon.gameObject.SetActive(true);
			Characters.Actions.Action action = this._action;
			if (action != null)
			{
				action.TryStart();
			}
			while (this._action.running)
			{
				yield return null;
			}
			this._characterToSummon.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06004BD4 RID: 19412 RVA: 0x000DF09C File Offset: 0x000DD29C
		protected override void OnDestroy()
		{
			if (!Service.quitting)
			{
				UnityEngine.Object.Destroy(this._characterToSummon.gameObject);
			}
		}

		// Token: 0x04003B03 RID: 15107
		[SerializeField]
		private Character _characterToSummon;

		// Token: 0x04003B04 RID: 15108
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04003B05 RID: 15109
		private Character _owner;
	}
}

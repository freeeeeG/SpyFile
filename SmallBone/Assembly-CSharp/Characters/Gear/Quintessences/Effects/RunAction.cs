using System;
using System.Collections;
using Characters.Actions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008EC RID: 2284
	public sealed class RunAction : QuintessenceEffect
	{
		// Token: 0x060030D3 RID: 12499 RVA: 0x00092231 File Offset: 0x00090431
		private void Awake()
		{
			this.Initialize();
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x0009223C File Offset: 0x0009043C
		protected override void OnInvoke(Quintessence quintessence)
		{
			Character owner = quintessence.owner;
			if (owner == null)
			{
				return;
			}
			this.ActivateCharacter();
			this._synchronization.Synchronize(this._character, owner);
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x0009227F File Offset: 0x0009047F
		private void Initialize()
		{
			this.DeactivateCharacter();
			this._character.transform.parent = null;
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.DeactivateCharacter;
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000922B3 File Offset: 0x000904B3
		private void ActivateCharacter()
		{
			this._character.gameObject.SetActive(true);
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000922C6 File Offset: 0x000904C6
		private void DeactivateCharacter()
		{
			this._character.gameObject.SetActive(false);
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000922D9 File Offset: 0x000904D9
		private void Dispose()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.DeactivateCharacter;
			UnityEngine.Object.Destroy(this._character.gameObject);
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x00092306 File Offset: 0x00090506
		private IEnumerator CRun()
		{
			this._action.TryStart();
			while (this._action.running)
			{
				yield return null;
			}
			this.DeactivateCharacter();
			yield break;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x00092315 File Offset: 0x00090515
		private void OnDestroy()
		{
			if (!Service.quitting)
			{
				this.Dispose();
			}
		}

		// Token: 0x04002844 RID: 10308
		[SerializeField]
		private Character _character;

		// Token: 0x04002845 RID: 10309
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04002846 RID: 10310
		[SerializeField]
		private CharacterSynchronization _synchronization;
	}
}

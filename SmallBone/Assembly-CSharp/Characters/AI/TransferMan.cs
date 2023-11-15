using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Operations;
using Level;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010FC RID: 4348
	public sealed class TransferMan : AIController
	{
		// Token: 0x06005483 RID: 21635 RVA: 0x000FCBAE File Offset: 0x000FADAE
		private void Awake()
		{
			this._onTransferStart.Initialize();
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x000FCBBB File Offset: 0x000FADBB
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x000FCBD0 File Offset: 0x000FADD0
		protected override IEnumerator CProcess()
		{
			yield return null;
			yield return null;
			while (!base.dead)
			{
				if (base.stuned)
				{
					yield return null;
				}
				else
				{
					if (this.TryToSetTeleportDest())
					{
						yield return this._teleportToEnemy.CRun(this);
						yield return Chronometer.global.WaitForSeconds(1f);
						yield return this._transferToPlayerReady.CRun(this);
						this.Transfer();
						yield return this._transferToPlayer.CRun(this);
					}
					yield return Chronometer.global.WaitForSeconds(2f);
				}
			}
			yield break;
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x000FCBE0 File Offset: 0x000FADE0
		private bool TryToSetTeleportDest()
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			if (allEnemies.Count == 0)
			{
				return false;
			}
			Character player = Singleton<Service>.Instance.levelManager.player;
			Collider2D lastStandingCollider = player.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				player.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
			}
			allEnemies.Shuffle<Character>();
			foreach (Character character in allEnemies)
			{
				Collider2D lastStandingCollider2 = character.movement.controller.collisionState.lastStandingCollider;
				if (!(lastStandingCollider2 == null) && lastStandingCollider2 != lastStandingCollider)
				{
					this._teleportDest.position = character.transform.position;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x000FCCDC File Offset: 0x000FAEDC
		private void Transfer()
		{
			this._onTransferStart.gameObject.SetActive(true);
			this._onTransferStart.Run(this.character);
			TransferMan._overlapper.contactFilter.SetLayerMask(1024);
			ReadonlyBoundedList<Collider2D> results = TransferMan._overlapper.OverlapCollider(this._transferRange).results;
			Debug.Log(results.Count);
			Bounds bounds = this._transferRange.bounds;
			foreach (Collider2D collider2D in results)
			{
				if (!(collider2D.GetComponent<Character>() == null))
				{
					float x = collider2D.transform.position.x - bounds.center.x;
					float y = collider2D.transform.position.y - bounds.min.y;
					collider2D.transform.position = this._transferDest.position + new Vector3(x, y);
				}
			}
		}

		// Token: 0x040043D5 RID: 17365
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _teleportToEnemy;

		// Token: 0x040043D6 RID: 17366
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _transferToPlayer;

		// Token: 0x040043D7 RID: 17367
		[SerializeField]
		private Characters.AI.Behaviours.Behaviour _transferToPlayerReady;

		// Token: 0x040043D8 RID: 17368
		[SerializeField]
		private Transform _teleportDest;

		// Token: 0x040043D9 RID: 17369
		[SerializeField]
		private Collider2D _transferRange;

		// Token: 0x040043DA RID: 17370
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onTransferStart;

		// Token: 0x040043DB RID: 17371
		[SerializeField]
		private Transform _transferDest;

		// Token: 0x040043DC RID: 17372
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(64);
	}
}

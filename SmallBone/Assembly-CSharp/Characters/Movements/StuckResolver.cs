using System;
using System.Collections;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Movements
{
	// Token: 0x02000810 RID: 2064
	[RequireComponent(typeof(Movement))]
	public class StuckResolver : MonoBehaviour
	{
		// Token: 0x06002A73 RID: 10867 RVA: 0x00082FD0 File Offset: 0x000811D0
		private void Awake()
		{
			this._controller = base.GetComponent<CharacterController2D>();
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += this.StartCheck;
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.StopCheck;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x0008301F File Offset: 0x0008121F
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.StartCheck;
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.StopCheck;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x0008305F File Offset: 0x0008125F
		private void StartCheck()
		{
			this._lastValidPosition = Map.Instance.playerOrigin;
			base.StartCoroutine(this.CCheck());
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x00048973 File Offset: 0x00046B73
		private void StopCheck()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x0008307E File Offset: 0x0008127E
		private IEnumerator CCheck()
		{
			for (;;)
			{
				yield return StuckResolver._waitForCheck;
				if (!this._controller.IsInTerrain())
				{
					this._lastValidPosition = base.transform.position;
				}
				else
				{
					bool stuck = true;
					int num;
					for (int i = 0; i < 10; i = num + 1)
					{
						yield return StuckResolver._waitForStuckCheck;
						if (this.stop.value)
						{
							stuck = false;
							break;
						}
						if (!this._controller.IsInTerrain())
						{
							stuck = false;
							break;
						}
						num = i;
					}
					if (stuck)
					{
						Debug.Log("The character " + base.name + " is stucked in the terrain. It was moved to last valid position.");
						base.transform.position = this._lastValidPosition;
						this._controller.ResetBounds();
						Physics2D.SyncTransforms();
					}
				}
			}
			yield break;
		}

		// Token: 0x04002430 RID: 9264
		private const float _checkInterval = 3f;

		// Token: 0x04002431 RID: 9265
		private static readonly WaitForSeconds _waitForCheck = new WaitForSeconds(3f);

		// Token: 0x04002432 RID: 9266
		private const float _stuckCheckInterval = 0.1f;

		// Token: 0x04002433 RID: 9267
		private static readonly WaitForSeconds _waitForStuckCheck = new WaitForSeconds(0.1f);

		// Token: 0x04002434 RID: 9268
		private const int _stuckCheckCount = 10;

		// Token: 0x04002435 RID: 9269
		private CharacterController2D _controller;

		// Token: 0x04002436 RID: 9270
		private Vector3 _lastValidPosition;

		// Token: 0x04002437 RID: 9271
		public readonly TrueOnlyLogicalSumList stop = new TrueOnlyLogicalSumList(false);
	}
}

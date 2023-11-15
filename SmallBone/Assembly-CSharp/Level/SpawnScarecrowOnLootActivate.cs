using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.AI;
using UnityEngine;

namespace Level
{
	// Token: 0x02000523 RID: 1315
	public class SpawnScarecrowOnLootActivate : MonoBehaviour
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x000510D4 File Offset: 0x0004F2D4
		private void Start()
		{
			this._scarCraws = new List<ScareCrawAI>(base.transform.childCount);
			ScareCrawAI[] componentsInChildren = base.GetComponentsInChildren<ScareCrawAI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ScareCrawAI scareCraw = componentsInChildren[i];
				scareCraw.character.health.onDied += delegate()
				{
					this.StartCoroutine(this.Revive(scareCraw));
				};
				scareCraw.character.gameObject.SetActive(false);
				this._scarCraws.Add(scareCraw);
			}
			this._mapReward.onLoot += delegate()
			{
				foreach (ScareCrawAI scareCrawAI in this._scarCraws)
				{
					scareCrawAI.character.gameObject.SetActive(true);
					scareCrawAI.Appear();
				}
			};
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00051181 File Offset: 0x0004F381
		private IEnumerator Revive(ScareCrawAI scareCraw)
		{
			yield return Chronometer.global.WaitForSeconds(3f);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._scareCrawPrefab, scareCraw.character.transform.position, Quaternion.identity, base.transform);
			Character component = gameObject.GetComponent<Character>();
			ScareCrawAI scareCrawAI = gameObject.GetComponentInChildren<ScareCrawAI>();
			component.ForceToLookAt(scareCraw.character.lookingDirection);
			component.gameObject.SetActive(true);
			component.health.onDied += delegate()
			{
				this.StartCoroutine(this.Revive(scareCrawAI));
			};
			scareCrawAI.Appear();
			UnityEngine.Object.Destroy(scareCraw.gameObject);
			yield break;
		}

		// Token: 0x040016A3 RID: 5795
		[SerializeField]
		private MapReward _mapReward;

		// Token: 0x040016A4 RID: 5796
		[SerializeField]
		private GameObject _scareCrawPrefab;

		// Token: 0x040016A5 RID: 5797
		private List<ScareCrawAI> _scarCraws;
	}
}

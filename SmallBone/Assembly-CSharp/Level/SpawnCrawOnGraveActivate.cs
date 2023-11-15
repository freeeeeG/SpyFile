using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.AI;
using UnityEngine;

namespace Level
{
	// Token: 0x0200051F RID: 1311
	public class SpawnCrawOnGraveActivate : MonoBehaviour
	{
		// Token: 0x060019D0 RID: 6608 RVA: 0x00050E44 File Offset: 0x0004F044
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
			this._grave.onLoot += delegate()
			{
				foreach (ScareCrawAI scareCrawAI in this._scarCraws)
				{
					scareCrawAI.character.gameObject.SetActive(true);
					scareCrawAI.Appear();
				}
			};
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00050EF1 File Offset: 0x0004F0F1
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

		// Token: 0x04001697 RID: 5783
		[SerializeField]
		private Grave _grave;

		// Token: 0x04001698 RID: 5784
		[SerializeField]
		private GameObject _scareCrawPrefab;

		// Token: 0x04001699 RID: 5785
		private List<ScareCrawAI> _scarCraws;
	}
}

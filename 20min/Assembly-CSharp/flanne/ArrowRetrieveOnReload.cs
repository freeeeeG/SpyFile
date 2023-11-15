using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000D0 RID: 208
	public class ArrowRetrieveOnReload : MonoBehaviour
	{
		// Token: 0x0600066C RID: 1644 RVA: 0x0001D3AC File Offset: 0x0001B5AC
		private void OnReload()
		{
			for (int i = 0; i < ArrowRetrieveOnReload.retrievalPoints.Count; i++)
			{
				if (ArrowRetrieveOnReload.retrievalPoints[i].gameObject.activeSelf)
				{
					GameObject pooledObject = this.OP.GetPooledObject(this.retrieveArrowPrefab.name);
					pooledObject.transform.position = ArrowRetrieveOnReload.retrievalPoints[i].transform.position;
					pooledObject.SetActive(true);
					Harmful component = pooledObject.GetComponent<Harmful>();
					component.damageAmount = Mathf.FloorToInt(this.gun.damage * this.damageMulti);
					if (!this.dontRetrieve)
					{
						component.StartCoroutine(this.RetrieveCR(pooledObject));
					}
					ArrowRetrieveOnReload.retrievalPoints[i].gameObject.SetActive(false);
				}
			}
			SoundEffectSO soundEffectSO = this.retrieveSFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001D490 File Offset: 0x0001B690
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.retrieveArrowPrefab.name, this.retrieveArrowPrefab, 100, true);
			this.player = base.GetComponentInParent<PlayerController>();
			this.gun = this.player.gun;
			this.ammo = this.player.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001D511 File Offset: 0x0001B711
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001D52F File Offset: 0x0001B72F
		public static void RegisterRetrievalPoint(ArrowRetrievalPoint r)
		{
			if (ArrowRetrieveOnReload.retrievalPoints == null)
			{
				ArrowRetrieveOnReload.retrievalPoints = new List<ArrowRetrievalPoint>();
			}
			ArrowRetrieveOnReload.retrievalPoints.Add(r);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001D54D File Offset: 0x0001B74D
		public static void RemoveRetrievalPoint(ArrowRetrievalPoint r)
		{
			ArrowRetrieveOnReload.retrievalPoints.Remove(r);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001D55B File Offset: 0x0001B75B
		private IEnumerator RetrieveCR(GameObject obj)
		{
			obj.transform.SetParent(this.player.transform);
			while (obj.transform.localPosition != Vector3.zero)
			{
				yield return null;
				float maxDistanceDelta = this.retrieveSpeed * Time.deltaTime;
				Vector3 localPosition = Vector3.MoveTowards(obj.transform.localPosition, Vector3.zero, maxDistanceDelta);
				obj.transform.localPosition = localPosition;
			}
			obj.transform.SetParent(ObjectPooler.SharedInstance.transform);
			obj.transform.localPosition = Vector3.zero;
			obj.SetActive(false);
			yield break;
		}

		// Token: 0x0400043D RID: 1085
		private static List<ArrowRetrievalPoint> retrievalPoints;

		// Token: 0x0400043E RID: 1086
		[SerializeField]
		private GameObject retrieveArrowPrefab;

		// Token: 0x0400043F RID: 1087
		[SerializeField]
		private float retrieveSpeed;

		// Token: 0x04000440 RID: 1088
		[SerializeField]
		private float damageMulti = 1f;

		// Token: 0x04000441 RID: 1089
		[SerializeField]
		private bool dontRetrieve;

		// Token: 0x04000442 RID: 1090
		[SerializeField]
		private SoundEffectSO retrieveSFX;

		// Token: 0x04000443 RID: 1091
		private ObjectPooler OP;

		// Token: 0x04000444 RID: 1092
		private PlayerController player;

		// Token: 0x04000445 RID: 1093
		private Gun gun;

		// Token: 0x04000446 RID: 1094
		private Ammo ammo;
	}
}

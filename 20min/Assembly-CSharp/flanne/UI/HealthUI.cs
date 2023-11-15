using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.UI
{
	// Token: 0x0200020A RID: 522
	public class HealthUI : MonoBehaviour
	{
		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002BD84 File Offset: 0x00029F84
		private void Start()
		{
			this.playerHealth.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHpChanged));
			this.playerHealth.onMaxHPChangedTo.AddListener(new UnityAction<int>(this.OnMaxHPChanged));
			this.playerHealth.onSoulHPChangedTo.AddListener(new UnityAction<int>(this.OnSoulHpChanged));
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002BDE8 File Offset: 0x00029FE8
		private void OnDestroy()
		{
			this.playerHealth.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHpChanged));
			this.playerHealth.onMaxHPChangedTo.RemoveListener(new UnityAction<int>(this.OnMaxHPChanged));
			this.playerHealth.onSoulHPChangedTo.RemoveListener(new UnityAction<int>(this.OnSoulHpChanged));
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002BE49 File Offset: 0x0002A049
		public void OnHpChanged(int value)
		{
			this.hp = value;
			this.Refresh();
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002BE58 File Offset: 0x0002A058
		public void OnMaxHPChanged(int value)
		{
			this.mhp = value;
			this.Refresh();
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002BE67 File Offset: 0x0002A067
		public void OnSoulHpChanged(int value)
		{
			this.shp = value;
			this.Refresh();
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002BE78 File Offset: 0x0002A078
		private void Refresh()
		{
			if (this.hearts == null)
			{
				this.hearts = new List<GameObject>();
			}
			foreach (GameObject obj in this.hearts)
			{
				Object.Destroy(obj);
			}
			this.hearts.Clear();
			int i;
			for (i = 0; i < this.hp; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.heartPrefab);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.hearts.Add(gameObject);
			}
			while (i < this.mhp)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.emptyHeartPrefab);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				this.hearts.Add(gameObject2);
				i++;
			}
			for (int j = 0; j < this.shp; j++)
			{
				GameObject gameObject3 = Object.Instantiate<GameObject>(this.soulHeartPrefab);
				gameObject3.transform.SetParent(base.transform);
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				this.hearts.Add(gameObject3);
			}
		}

		// Token: 0x04000836 RID: 2102
		[SerializeField]
		private PlayerHealth playerHealth;

		// Token: 0x04000837 RID: 2103
		[SerializeField]
		private GameObject heartPrefab;

		// Token: 0x04000838 RID: 2104
		[SerializeField]
		private GameObject emptyHeartPrefab;

		// Token: 0x04000839 RID: 2105
		[SerializeField]
		private GameObject soulHeartPrefab;

		// Token: 0x0400083A RID: 2106
		private List<GameObject> hearts;

		// Token: 0x0400083B RID: 2107
		private int hp;

		// Token: 0x0400083C RID: 2108
		private int mhp;

		// Token: 0x0400083D RID: 2109
		private int shp;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000258 RID: 600
	public class ThunderOverTime : MonoBehaviour
	{
		// Token: 0x06000D04 RID: 3332 RVA: 0x0002F6FD File Offset: 0x0002D8FD
		private void Start()
		{
			this.TGen = ThunderGenerator.SharedInstance;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0002F70C File Offset: 0x0002D90C
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer > this.cooldown)
			{
				this._timer -= this.cooldown;
				for (int i = 0; i < this.thundersPerWave; i++)
				{
					GameObject newTarget = this.GetNewTarget();
					if (newTarget != null)
					{
						this.TGen.GenerateAt(newTarget, this.baseDamage);
					}
				}
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0002F780 File Offset: 0x0002D980
		private GameObject GetNewTarget()
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Enemy");
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in array)
			{
				if (this.IsWithinRange(gameObject.transform.position))
				{
					list.Add(gameObject);
				}
			}
			if (list.Count > 0)
			{
				return array[Random.Range(0, array.Length)];
			}
			return null;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0002F7EC File Offset: 0x0002D9EC
		private bool IsWithinRange(Vector2 pos)
		{
			return this.player != null && Mathf.Abs(this.player.position.x - pos.x) < this.rangeX && Mathf.Abs(this.player.position.y - pos.y) < this.rangeY;
		}

		// Token: 0x04000952 RID: 2386
		[SerializeField]
		private int baseDamage;

		// Token: 0x04000953 RID: 2387
		[SerializeField]
		private float cooldown;

		// Token: 0x04000954 RID: 2388
		[SerializeField]
		private int thundersPerWave;

		// Token: 0x04000955 RID: 2389
		[SerializeField]
		private float rangeX;

		// Token: 0x04000956 RID: 2390
		[SerializeField]
		private float rangeY;

		// Token: 0x04000957 RID: 2391
		private ThunderGenerator TGen;

		// Token: 0x04000958 RID: 2392
		[SerializeField]
		private Transform player;

		// Token: 0x04000959 RID: 2393
		private float _timer;
	}
}

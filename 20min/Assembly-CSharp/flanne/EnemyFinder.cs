using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000140 RID: 320
	public static class EnemyFinder
	{
		// Token: 0x0600085C RID: 2140 RVA: 0x00023278 File Offset: 0x00021478
		public static GameObject GetRandomEnemy(Vector2 center, Vector2 range)
		{
			List<GameObject> list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
			list.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyChampion")));
			GameObject gameObject = null;
			while (list.Count > 0 && gameObject == null)
			{
				GameObject gameObject2 = list[Random.Range(0, list.Count)];
				if (Mathf.Abs(gameObject2.transform.position.x - center.x) < range.x && Mathf.Abs(gameObject2.transform.position.y - center.y) < range.y)
				{
					gameObject = gameObject2;
				}
				else
				{
					list.Remove(gameObject2);
				}
			}
			return gameObject;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00023328 File Offset: 0x00021528
		public static GameObject GetClosestEnemy(Vector2 center)
		{
			return EnemyFinder.GetClosestEnemy(center, null);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00023334 File Offset: 0x00021534
		public static GameObject GetClosestEnemy(Vector2 center, AIComponent exclude)
		{
			List<AIComponent> enemies = AIController.SharedInstance.enemies;
			GameObject result = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < enemies.Count; i++)
			{
				if (!(enemies[i] == exclude) && enemies[i].gameObject.activeInHierarchy)
				{
					Vector2 b = new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y);
					float sqrMagnitude = (center - b).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = enemies[i].gameObject;
					}
				}
			}
			return result;
		}
	}
}

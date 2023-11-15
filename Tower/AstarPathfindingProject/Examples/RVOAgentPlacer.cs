using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000E3 RID: 227
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_r_v_o_agent_placer.php")]
	public class RVOAgentPlacer : MonoBehaviour
	{
		// Token: 0x060009C1 RID: 2497 RVA: 0x00040293 File Offset: 0x0003E493
		private IEnumerator Start()
		{
			yield return null;
			for (int i = 0; i < this.agents; i++)
			{
				float num = (float)i / (float)this.agents * 3.1415927f * 2f;
				Vector3 vector = new Vector3((float)Math.Cos((double)num), 0f, (float)Math.Sin((double)num)) * this.ringSize;
				Vector3 target = -vector + this.goalOffset;
				GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, Vector3.zero, Quaternion.Euler(0f, num + 180f, 0f));
				RVOExampleAgent component = gameObject.GetComponent<RVOExampleAgent>();
				if (component == null)
				{
					Debug.LogError("Prefab does not have an RVOExampleAgent component attached");
					yield break;
				}
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = vector;
				component.repathRate = this.repathRate;
				component.SetTarget(target);
				component.SetColor(this.GetColor(num));
			}
			yield break;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000402A2 File Offset: 0x0003E4A2
		public Color GetColor(float angle)
		{
			return AstarMath.HSVToRGB(angle * 57.295776f, 0.8f, 0.6f);
		}

		// Token: 0x040005DC RID: 1500
		public int agents = 100;

		// Token: 0x040005DD RID: 1501
		public float ringSize = 100f;

		// Token: 0x040005DE RID: 1502
		public LayerMask mask;

		// Token: 0x040005DF RID: 1503
		public GameObject prefab;

		// Token: 0x040005E0 RID: 1504
		public Vector3 goalOffset;

		// Token: 0x040005E1 RID: 1505
		public float repathRate = 1f;

		// Token: 0x040005E2 RID: 1506
		private const float rad2Deg = 57.295776f;
	}
}

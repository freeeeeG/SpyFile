using System;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000058 RID: 88
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_target_mover.php")]
	public class TargetMover : MonoBehaviour
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x000150C5 File Offset: 0x000132C5
		public void Start()
		{
			this.cam = Camera.main;
			this.ais = Object.FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray<IAstarAI>();
			base.useGUILayout = false;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000150EE File Offset: 0x000132EE
		public void OnGUI()
		{
			if (this.onlyOnDoubleClick && this.cam != null && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
			{
				this.UpdateTargetPosition();
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00015125 File Offset: 0x00013325
		private void Update()
		{
			if (!this.onlyOnDoubleClick && this.cam != null)
			{
				this.UpdateTargetPosition();
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00015144 File Offset: 0x00013344
		public void UpdateTargetPosition()
		{
			Vector3 vector = Vector3.zero;
			bool flag = false;
			RaycastHit raycastHit;
			if (this.use2D)
			{
				vector = this.cam.ScreenToWorldPoint(Input.mousePosition);
				vector.z = 0f;
				flag = true;
			}
			else if (Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity, this.mask))
			{
				vector = raycastHit.point;
				flag = true;
			}
			if (flag && vector != this.target.position)
			{
				this.target.position = vector;
				if (this.onlyOnDoubleClick)
				{
					for (int i = 0; i < this.ais.Length; i++)
					{
						if (this.ais[i] != null)
						{
							this.ais[i].SearchPath();
						}
					}
				}
			}
		}

		// Token: 0x0400027F RID: 639
		public LayerMask mask;

		// Token: 0x04000280 RID: 640
		public Transform target;

		// Token: 0x04000281 RID: 641
		private IAstarAI[] ais;

		// Token: 0x04000282 RID: 642
		public bool onlyOnDoubleClick;

		// Token: 0x04000283 RID: 643
		public bool use2D;

		// Token: 0x04000284 RID: 644
		private Camera cam;
	}
}

using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F4 RID: 244
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_object_placer.php")]
	public class ObjectPlacer : MonoBehaviour
	{
		// Token: 0x06000A12 RID: 2578 RVA: 0x00042298 File Offset: 0x00040498
		private void Update()
		{
			if (Input.GetKeyDown("p"))
			{
				this.PlaceObject();
			}
			if (Input.GetKeyDown("r"))
			{
				base.StartCoroutine(this.RemoveObject());
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000422C8 File Offset: 0x000404C8
		public void PlaceObject()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity))
			{
				Vector3 point = raycastHit.point;
				GameObject gameObject = Object.Instantiate<GameObject>(this.go, point, Quaternion.identity);
				if (this.issueGUOs)
				{
					GraphUpdateObject ob = new GraphUpdateObject(gameObject.GetComponent<Collider>().bounds);
					AstarPath.active.UpdateGraphs(ob);
					if (this.direct)
					{
						AstarPath.active.FlushGraphUpdates();
					}
				}
			}
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00042342 File Offset: 0x00040542
		public IEnumerator RemoveObject()
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, float.PositiveInfinity))
			{
				if (raycastHit.collider.isTrigger || raycastHit.transform.gameObject.name == "Ground")
				{
					yield break;
				}
				Bounds b = raycastHit.collider.bounds;
				Object.Destroy(raycastHit.collider);
				Object.Destroy(raycastHit.collider.gameObject);
				if (this.issueGUOs)
				{
					yield return new WaitForEndOfFrame();
					GraphUpdateObject ob = new GraphUpdateObject(b);
					AstarPath.active.UpdateGraphs(ob);
					if (this.direct)
					{
						AstarPath.active.FlushGraphUpdates();
					}
				}
				b = default(Bounds);
			}
			yield break;
		}

		// Token: 0x0400063E RID: 1598
		public GameObject go;

		// Token: 0x0400063F RID: 1599
		public bool direct;

		// Token: 0x04000640 RID: 1600
		public bool issueGUOs = true;
	}
}

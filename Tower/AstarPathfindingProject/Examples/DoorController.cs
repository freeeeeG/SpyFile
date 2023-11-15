using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F0 RID: 240
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_door_controller.php")]
	public class DoorController : MonoBehaviour
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x00041FC5 File Offset: 0x000401C5
		public void Start()
		{
			this.bounds = base.GetComponent<Collider>().bounds;
			this.SetState(this.open);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00041FE4 File Offset: 0x000401E4
		private void OnGUI()
		{
			if (GUI.Button(new Rect(5f, this.yOffset, 100f, 22f), "Toggle Door"))
			{
				this.SetState(!this.open);
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0004201C File Offset: 0x0004021C
		public void SetState(bool open)
		{
			this.open = open;
			if (this.updateGraphsWithGUO)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.bounds);
				int num = open ? this.opentag : this.closedtag;
				if (num > 31)
				{
					Debug.LogError("tag > 31");
					return;
				}
				graphUpdateObject.modifyTag = true;
				graphUpdateObject.setTag = num;
				graphUpdateObject.updatePhysics = false;
				AstarPath.active.UpdateGraphs(graphUpdateObject);
			}
			if (open)
			{
				base.GetComponent<Animation>().Play("Open");
				return;
			}
			base.GetComponent<Animation>().Play("Close");
		}

		// Token: 0x0400062C RID: 1580
		private bool open;

		// Token: 0x0400062D RID: 1581
		public int opentag = 1;

		// Token: 0x0400062E RID: 1582
		public int closedtag = 1;

		// Token: 0x0400062F RID: 1583
		public bool updateGraphsWithGUO = true;

		// Token: 0x04000630 RID: 1584
		public float yOffset = 5f;

		// Token: 0x04000631 RID: 1585
		private Bounds bounds;
	}
}

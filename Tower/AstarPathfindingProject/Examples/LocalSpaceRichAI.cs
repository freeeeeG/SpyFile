using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000E7 RID: 231
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_local_space_rich_a_i.php")]
	public class LocalSpaceRichAI : RichAI
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x00040EDA File Offset: 0x0003F0DA
		private void RefreshTransform()
		{
			this.graph.Refresh();
			this.richPath.transform = this.graph.transformation;
			this.movementPlane = this.graph.transformation;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00040F0E File Offset: 0x0003F10E
		protected override void Start()
		{
			this.RefreshTransform();
			base.Start();
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00040F1C File Offset: 0x0003F11C
		protected override void CalculatePathRequestEndpoints(out Vector3 start, out Vector3 end)
		{
			this.RefreshTransform();
			base.CalculatePathRequestEndpoints(out start, out end);
			start = this.graph.transformation.InverseTransform(start);
			end = this.graph.transformation.InverseTransform(end);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00040F6F File Offset: 0x0003F16F
		protected override void Update()
		{
			this.RefreshTransform();
			base.Update();
		}

		// Token: 0x040005FE RID: 1534
		public LocalSpaceGraph graph;
	}
}

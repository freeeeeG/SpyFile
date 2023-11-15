using System;

namespace TemplateClasses
{
	// Token: 0x02000CC8 RID: 3272
	[Serializable]
	public class Cell
	{
		// Token: 0x060068B8 RID: 26808 RVA: 0x00279284 File Offset: 0x00277484
		public Cell()
		{
		}

		// Token: 0x060068B9 RID: 26809 RVA: 0x0027928C File Offset: 0x0027748C
		public Cell(int loc_x, int loc_y, SimHashes _element, float _temperature, float _mass, string _diseaseName, int _diseaseCount, bool _preventFoWReveal = false)
		{
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.element = _element;
			this.temperature = _temperature;
			this.mass = _mass;
			this.diseaseName = _diseaseName;
			this.diseaseCount = _diseaseCount;
			this.preventFoWReveal = _preventFoWReveal;
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060068BA RID: 26810 RVA: 0x002792DC File Offset: 0x002774DC
		// (set) Token: 0x060068BB RID: 26811 RVA: 0x002792E4 File Offset: 0x002774E4
		public SimHashes element { get; set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060068BC RID: 26812 RVA: 0x002792ED File Offset: 0x002774ED
		// (set) Token: 0x060068BD RID: 26813 RVA: 0x002792F5 File Offset: 0x002774F5
		public float mass { get; set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060068BE RID: 26814 RVA: 0x002792FE File Offset: 0x002774FE
		// (set) Token: 0x060068BF RID: 26815 RVA: 0x00279306 File Offset: 0x00277506
		public float temperature { get; set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060068C0 RID: 26816 RVA: 0x0027930F File Offset: 0x0027750F
		// (set) Token: 0x060068C1 RID: 26817 RVA: 0x00279317 File Offset: 0x00277517
		public string diseaseName { get; set; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060068C2 RID: 26818 RVA: 0x00279320 File Offset: 0x00277520
		// (set) Token: 0x060068C3 RID: 26819 RVA: 0x00279328 File Offset: 0x00277528
		public int diseaseCount { get; set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060068C4 RID: 26820 RVA: 0x00279331 File Offset: 0x00277531
		// (set) Token: 0x060068C5 RID: 26821 RVA: 0x00279339 File Offset: 0x00277539
		public int location_x { get; set; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060068C6 RID: 26822 RVA: 0x00279342 File Offset: 0x00277542
		// (set) Token: 0x060068C7 RID: 26823 RVA: 0x0027934A File Offset: 0x0027754A
		public int location_y { get; set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060068C8 RID: 26824 RVA: 0x00279353 File Offset: 0x00277553
		// (set) Token: 0x060068C9 RID: 26825 RVA: 0x0027935B File Offset: 0x0027755B
		public bool preventFoWReveal { get; set; }
	}
}

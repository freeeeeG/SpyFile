using System;

namespace TemplateClasses
{
	// Token: 0x02000CC9 RID: 3273
	[Serializable]
	public class StorageItem
	{
		// Token: 0x060068CA RID: 26826 RVA: 0x00279364 File Offset: 0x00277564
		public StorageItem()
		{
			this.rottable = new Rottable();
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x00279378 File Offset: 0x00277578
		public StorageItem(string _id, float _units, float _temp, SimHashes _element, string _disease, int _disease_count, bool _isOre)
		{
			this.rottable = new Rottable();
			this.id = _id;
			this.element = _element;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.isOre = _isOre;
			this.temperature = _temp;
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060068CC RID: 26828 RVA: 0x002793CB File Offset: 0x002775CB
		// (set) Token: 0x060068CD RID: 26829 RVA: 0x002793D3 File Offset: 0x002775D3
		public string id { get; set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060068CE RID: 26830 RVA: 0x002793DC File Offset: 0x002775DC
		// (set) Token: 0x060068CF RID: 26831 RVA: 0x002793E4 File Offset: 0x002775E4
		public SimHashes element { get; set; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060068D0 RID: 26832 RVA: 0x002793ED File Offset: 0x002775ED
		// (set) Token: 0x060068D1 RID: 26833 RVA: 0x002793F5 File Offset: 0x002775F5
		public float units { get; set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060068D2 RID: 26834 RVA: 0x002793FE File Offset: 0x002775FE
		// (set) Token: 0x060068D3 RID: 26835 RVA: 0x00279406 File Offset: 0x00277606
		public bool isOre { get; set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060068D4 RID: 26836 RVA: 0x0027940F File Offset: 0x0027760F
		// (set) Token: 0x060068D5 RID: 26837 RVA: 0x00279417 File Offset: 0x00277617
		public float temperature { get; set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060068D6 RID: 26838 RVA: 0x00279420 File Offset: 0x00277620
		// (set) Token: 0x060068D7 RID: 26839 RVA: 0x00279428 File Offset: 0x00277628
		public string diseaseName { get; set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060068D8 RID: 26840 RVA: 0x00279431 File Offset: 0x00277631
		// (set) Token: 0x060068D9 RID: 26841 RVA: 0x00279439 File Offset: 0x00277639
		public int diseaseCount { get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060068DA RID: 26842 RVA: 0x00279442 File Offset: 0x00277642
		// (set) Token: 0x060068DB RID: 26843 RVA: 0x0027944A File Offset: 0x0027764A
		public Rottable rottable { get; set; }

		// Token: 0x060068DC RID: 26844 RVA: 0x00279454 File Offset: 0x00277654
		public StorageItem Clone()
		{
			return new StorageItem(this.id, this.units, this.temperature, this.element, this.diseaseName, this.diseaseCount, this.isOre)
			{
				rottable = 
				{
					rotAmount = this.rottable.rotAmount
				}
			};
		}
	}
}

using System;

namespace Database
{
	// Token: 0x02000D1B RID: 3355
	public class Shirts : ResourceSet<Shirt>
	{
		// Token: 0x060069F9 RID: 27129 RVA: 0x00293710 File Offset: 0x00291910
		public Shirts()
		{
			this.Hot00 = base.Add(new Shirt("body_shirt_hot01"));
			this.Hot01 = base.Add(new Shirt("body_shirt_hot02"));
			this.Decor00 = base.Add(new Shirt("body_shirt_decor01"));
			this.Cold00 = base.Add(new Shirt("body_shirt_cold01"));
			this.Cold01 = base.Add(new Shirt("body_shirt_cold02"));
		}

		// Token: 0x04004CE1 RID: 19681
		public Shirt Hot00;

		// Token: 0x04004CE2 RID: 19682
		public Shirt Hot01;

		// Token: 0x04004CE3 RID: 19683
		public Shirt Decor00;

		// Token: 0x04004CE4 RID: 19684
		public Shirt Cold00;

		// Token: 0x04004CE5 RID: 19685
		public Shirt Cold01;
	}
}

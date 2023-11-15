using System;

// Token: 0x02000540 RID: 1344
public class Accessory : Resource
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06002094 RID: 8340 RVA: 0x000AF190 File Offset: 0x000AD390
	// (set) Token: 0x06002095 RID: 8341 RVA: 0x000AF198 File Offset: 0x000AD398
	public KAnim.Build.Symbol symbol { get; private set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06002096 RID: 8342 RVA: 0x000AF1A1 File Offset: 0x000AD3A1
	// (set) Token: 0x06002097 RID: 8343 RVA: 0x000AF1A9 File Offset: 0x000AD3A9
	public HashedString batchSource { get; private set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06002098 RID: 8344 RVA: 0x000AF1B2 File Offset: 0x000AD3B2
	// (set) Token: 0x06002099 RID: 8345 RVA: 0x000AF1BA File Offset: 0x000AD3BA
	public AccessorySlot slot { get; private set; }

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x0600209A RID: 8346 RVA: 0x000AF1C3 File Offset: 0x000AD3C3
	// (set) Token: 0x0600209B RID: 8347 RVA: 0x000AF1CB File Offset: 0x000AD3CB
	public KAnimFile animFile { get; private set; }

	// Token: 0x0600209C RID: 8348 RVA: 0x000AF1D4 File Offset: 0x000AD3D4
	public Accessory(string id, ResourceSet parent, AccessorySlot slot, HashedString batchSource, KAnim.Build.Symbol symbol, KAnimFile animFile = null, KAnimFile defaultAnimFile = null) : base(id, parent, null)
	{
		this.slot = slot;
		this.symbol = symbol;
		this.batchSource = batchSource;
		this.animFile = animFile;
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000AF1FE File Offset: 0x000AD3FE
	public bool IsDefault()
	{
		return this.animFile == this.slot.defaultAnimFile;
	}
}

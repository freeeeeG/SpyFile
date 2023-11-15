using System;
using System.Collections.Generic;

// Token: 0x02000541 RID: 1345
public class AccessorySlot : Resource
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x000AF216 File Offset: 0x000AD416
	// (set) Token: 0x0600209F RID: 8351 RVA: 0x000AF21E File Offset: 0x000AD41E
	public KAnimHashedString targetSymbolId { get; private set; }

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x060020A0 RID: 8352 RVA: 0x000AF227 File Offset: 0x000AD427
	// (set) Token: 0x060020A1 RID: 8353 RVA: 0x000AF22F File Offset: 0x000AD42F
	public List<Accessory> accessories { get; private set; }

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000AF238 File Offset: 0x000AD438
	public KAnimFile AnimFile
	{
		get
		{
			return this.file;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x060020A3 RID: 8355 RVA: 0x000AF240 File Offset: 0x000AD440
	// (set) Token: 0x060020A4 RID: 8356 RVA: 0x000AF248 File Offset: 0x000AD448
	public KAnimFile defaultAnimFile { get; private set; }

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x060020A5 RID: 8357 RVA: 0x000AF251 File Offset: 0x000AD451
	// (set) Token: 0x060020A6 RID: 8358 RVA: 0x000AF259 File Offset: 0x000AD459
	public int overrideLayer { get; private set; }

	// Token: 0x060020A7 RID: 8359 RVA: 0x000AF264 File Offset: 0x000AD464
	public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.overrideLayer = overrideLayer;
		this.defaultAnimFile = swap_build;
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x000AF2D4 File Offset: 0x000AD4D4
	public AccessorySlot(string id, ResourceSet parent, KAnimHashedString target_symbol_id, KAnimFile swap_build, KAnimFile defaultAnimFile = null, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = target_symbol_id;
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.defaultAnimFile = ((defaultAnimFile != null) ? defaultAnimFile : swap_build);
		this.overrideLayer = overrideLayer;
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x000AF340 File Offset: 0x000AD540
	public void AddAccessories(KAnimFile default_build, ResourceSet parent)
	{
		KAnim.Build build = default_build.GetData().build;
		default_build.GetData().build.GetSymbol(this.targetSymbolId);
		string value = this.Id.ToLower();
		for (int i = 0; i < build.symbols.Length; i++)
		{
			string text = HashCache.Get().Get(build.symbols[i].hash);
			if (text.StartsWith(value))
			{
				Accessory accessory = new Accessory(text, parent, this, this.file.batchTag, build.symbols[i], default_build, null);
				this.accessories.Add(accessory);
				HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
			}
		}
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000AF3F9 File Offset: 0x000AD5F9
	public Accessory Lookup(string id)
	{
		return this.Lookup(new HashedString(id));
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000AF408 File Offset: 0x000AD608
	public Accessory Lookup(HashedString full_id)
	{
		if (!full_id.IsValid)
		{
			return null;
		}
		return this.accessories.Find((Accessory a) => a.IdHash == full_id);
	}

	// Token: 0x04001264 RID: 4708
	private KAnimFile file;
}

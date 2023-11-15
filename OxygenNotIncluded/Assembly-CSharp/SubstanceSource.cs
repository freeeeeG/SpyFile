using System;
using KSerialization;

// Token: 0x02000513 RID: 1299
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
	// Token: 0x06001F31 RID: 7985 RVA: 0x000A6A78 File Offset: 0x000A4C78
	protected override void OnPrefabInit()
	{
		this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x000A6A8A File Offset: 0x000A4C8A
	protected override void OnSpawn()
	{
		this.pickupable.SetWorkTime(10f);
	}

	// Token: 0x06001F33 RID: 7987
	protected abstract CellOffset[] GetOffsetGroup();

	// Token: 0x06001F34 RID: 7988
	protected abstract IChunkManager GetChunkManager();

	// Token: 0x06001F35 RID: 7989 RVA: 0x000A6A9C File Offset: 0x000A4C9C
	public SimHashes GetElementID()
	{
		return this.primaryElement.ElementID;
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x000A6AAC File Offset: 0x000A4CAC
	public Tag GetElementTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.tag;
		}
		return result;
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x000A6AFC File Offset: 0x000A4CFC
	public Tag GetMaterialCategoryTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.GetMaterialCategoryTag();
		}
		return result;
	}

	// Token: 0x0400118D RID: 4493
	private bool enableRefresh;

	// Token: 0x0400118E RID: 4494
	private static readonly float MaxPickupTime = 8f;

	// Token: 0x0400118F RID: 4495
	[MyCmpReq]
	public Pickupable pickupable;

	// Token: 0x04001190 RID: 4496
	[MyCmpReq]
	private PrimaryElement primaryElement;
}

using System;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02000935 RID: 2357
[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
	// Token: 0x0600446D RID: 17517 RVA: 0x0017FD2C File Offset: 0x0017DF2C
	public ResourceRef(ResourceType resource)
	{
		this.Set(resource);
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x0017FD3B File Offset: 0x0017DF3B
	public ResourceRef()
	{
	}

	// Token: 0x170004CC RID: 1228
	// (get) Token: 0x0600446F RID: 17519 RVA: 0x0017FD43 File Offset: 0x0017DF43
	public ResourceGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x06004470 RID: 17520 RVA: 0x0017FD4B File Offset: 0x0017DF4B
	public ResourceType Get()
	{
		return this.resource;
	}

	// Token: 0x06004471 RID: 17521 RVA: 0x0017FD53 File Offset: 0x0017DF53
	public void Set(ResourceType resource)
	{
		this.guid = null;
		this.resource = resource;
	}

	// Token: 0x06004472 RID: 17522 RVA: 0x0017FD63 File Offset: 0x0017DF63
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.resource == null)
		{
			this.guid = null;
			return;
		}
		this.guid = this.resource.Guid;
	}

	// Token: 0x06004473 RID: 17523 RVA: 0x0017FD90 File Offset: 0x0017DF90
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.guid != null)
		{
			this.resource = Db.Get().GetResource<ResourceType>(this.guid);
			if (this.resource != null)
			{
				this.guid = null;
			}
		}
	}

	// Token: 0x04002D59 RID: 11609
	[Serialize]
	private ResourceGuid guid;

	// Token: 0x04002D5A RID: 11610
	private ResourceType resource;
}

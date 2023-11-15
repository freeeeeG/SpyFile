using System;

namespace Pathfinding
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	public abstract class PathModifier : IPathModifier
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000649 RID: 1609
		public abstract int Order { get; }

		// Token: 0x0600064A RID: 1610 RVA: 0x00024D8B File Offset: 0x00022F8B
		public void Awake(Seeker seeker)
		{
			this.seeker = seeker;
			if (seeker != null)
			{
				seeker.RegisterModifier(this);
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00024DA4 File Offset: 0x00022FA4
		public void OnDestroy(Seeker seeker)
		{
			if (seeker != null)
			{
				seeker.DeregisterModifier(this);
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00024DB6 File Offset: 0x00022FB6
		public virtual void PreProcess(Path path)
		{
		}

		// Token: 0x0600064D RID: 1613
		public abstract void Apply(Path path);

		// Token: 0x0400038C RID: 908
		[NonSerialized]
		public Seeker seeker;
	}
}

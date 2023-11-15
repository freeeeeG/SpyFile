using System;

namespace Pathfinding
{
	// Token: 0x0200007C RID: 124
	[Serializable]
	public abstract class MonoModifier : VersionedMonoBehaviour, IPathModifier
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x00024DC0 File Offset: 0x00022FC0
		protected virtual void OnEnable()
		{
			this.seeker = base.GetComponent<Seeker>();
			if (this.seeker != null)
			{
				this.seeker.RegisterModifier(this);
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00024DE8 File Offset: 0x00022FE8
		protected virtual void OnDisable()
		{
			if (this.seeker != null)
			{
				this.seeker.DeregisterModifier(this);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000651 RID: 1617
		public abstract int Order { get; }

		// Token: 0x06000652 RID: 1618 RVA: 0x00024E04 File Offset: 0x00023004
		public virtual void PreProcess(Path path)
		{
		}

		// Token: 0x06000653 RID: 1619
		public abstract void Apply(Path path);

		// Token: 0x0400038D RID: 909
		[NonSerialized]
		public Seeker seeker;
	}
}

using System;
using System.Collections.Generic;

// Token: 0x02000254 RID: 596
public class DeedManagerBase : Manager
{
	// Token: 0x06000B05 RID: 2821 RVA: 0x0003B648 File Offset: 0x00039A48
	public void RegisterHandler<T>(DeedManagerBase.DeedHandler<T> _handler) where T : DeedManagerBase.Deed
	{
		Type typeFromHandle = typeof(T);
		if (!this.m_handlerLookup.ContainsKey(typeFromHandle))
		{
			DeedManagerBase.DeedHandler<T> value = delegate(T _deed)
			{
			};
			this.m_handlerLookup.Add(typeFromHandle, value);
		}
		MulticastDelegate multicastDelegate = this.m_handlerLookup[typeof(T)];
		DeedManagerBase.DeedHandler<T> deedHandler = multicastDelegate as DeedManagerBase.DeedHandler<T>;
		deedHandler = (DeedManagerBase.DeedHandler<T>)Delegate.Combine(deedHandler, _handler);
		this.m_handlerLookup[typeof(T)] = deedHandler;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0003B6CC File Offset: 0x00039ACC
	public void UnregisterHandler<T>(DeedManagerBase.DeedHandler<T> _handler) where T : DeedManagerBase.Deed
	{
		Type typeFromHandle = typeof(T);
		MulticastDelegate multicastDelegate = this.m_handlerLookup[typeof(T)];
		DeedManagerBase.DeedHandler<T> source = multicastDelegate as DeedManagerBase.DeedHandler<T>;
		source = (DeedManagerBase.DeedHandler<T>)Delegate.Remove(source, _handler);
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0003B70E File Offset: 0x00039B0E
	protected virtual void OnDeedFired(DeedManagerBase.Deed _deed)
	{
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0003B710 File Offset: 0x00039B10
	public void FireDeed<T>(T _deed) where T : DeedManagerBase.Deed
	{
		Type type = _deed.GetType();
		if (this.m_handlerLookup.ContainsKey(type))
		{
			this.m_handlerLookup[type].DynamicInvoke(new object[]
			{
				_deed
			});
		}
		this.OnDeedFired(_deed);
	}

	// Token: 0x040008AA RID: 2218
	private Dictionary<Type, MulticastDelegate> m_handlerLookup = new Dictionary<Type, MulticastDelegate>();

	// Token: 0x02000255 RID: 597
	// (Invoke) Token: 0x06000B0B RID: 2827
	public delegate void DeedHandler<T>(T _deed) where T : DeedManagerBase.Deed;

	// Token: 0x02000256 RID: 598
	public abstract class Deed
	{
	}

	// Token: 0x02000257 RID: 599
	public abstract class PadDeed : DeedManagerBase.Deed, DeedManagerBase.IPadDeed
	{
		// Token: 0x06000B0F RID: 2831 RVA: 0x0003B773 File Offset: 0x00039B73
		public PadDeed(ControlPadInput.PadNum _pad, Publicity _scope)
		{
			this.m_pad = _pad;
			this.m_scope = _scope;
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000B10 RID: 2832 RVA: 0x0003B789 File Offset: 0x00039B89
		public ControlPadInput.PadNum Pad
		{
			get
			{
				return this.m_pad;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0003B791 File Offset: 0x00039B91
		public Publicity Scope
		{
			get
			{
				return this.m_scope;
			}
		}

		// Token: 0x040008AB RID: 2219
		private ControlPadInput.PadNum m_pad;

		// Token: 0x040008AC RID: 2220
		private Publicity m_scope;
	}

	// Token: 0x02000258 RID: 600
	public interface IPadDeed
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000B12 RID: 2834
		ControlPadInput.PadNum Pad { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000B13 RID: 2835
		Publicity Scope { get; }
	}
}

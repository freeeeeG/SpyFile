using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002A3 RID: 675
	public class PlayerAction : InputControlBase
	{
		// Token: 0x06000C8B RID: 3211 RVA: 0x00041288 File Offset: 0x0003F688
		public PlayerAction(string name, PlayerActionSet owner)
		{
			this.Raw = true;
			this.Name = name;
			this.Owner = owner;
			this.bindings = new ReadOnlyCollection<BindingSource>(this.visibleBindings);
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000412E2 File Offset: 0x0003F6E2
		// (set) Token: 0x06000C8D RID: 3213 RVA: 0x000412EA File Offset: 0x0003F6EA
		public string Name { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x000412F3 File Offset: 0x0003F6F3
		// (set) Token: 0x06000C8F RID: 3215 RVA: 0x000412FB File Offset: 0x0003F6FB
		public PlayerActionSet Owner { get; private set; }

		// Token: 0x06000C90 RID: 3216 RVA: 0x00041304 File Offset: 0x0003F704
		public void AddDefaultBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			if (binding.BoundTo != null)
			{
				throw new InControlException("Binding source is already bound to action " + binding.BoundTo.Name);
			}
			if (!this.defaultBindings.Contains(binding))
			{
				this.defaultBindings.Add(binding);
				binding.BoundTo = this;
			}
			if (!this.regularBindings.Contains(binding))
			{
				this.regularBindings.Add(binding);
				binding.BoundTo = this;
				if (binding.IsValid)
				{
					this.visibleBindings.Add(binding);
				}
			}
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x000413A3 File Offset: 0x0003F7A3
		public void AddDefaultBinding(params Key[] keys)
		{
			this.AddDefaultBinding(new KeyBindingSource(keys));
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x000413B1 File Offset: 0x0003F7B1
		public void AddDefaultBinding(Mouse control)
		{
			this.AddDefaultBinding(new MouseBindingSource(control));
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x000413BF File Offset: 0x0003F7BF
		public void AddDefaultBinding(InputControlType control)
		{
			this.AddDefaultBinding(new DeviceBindingSource(control));
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x000413D0 File Offset: 0x0003F7D0
		public bool AddBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				Debug.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			this.regularBindings.Add(binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Add(binding);
			}
			return true;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00041450 File Offset: 0x0003F850
		public bool InsertBindingAt(int index, BindingSource binding)
		{
			if (index < 0 || index > this.visibleBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			if (index == this.visibleBindings.Count)
			{
				return this.AddBinding(binding);
			}
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				Debug.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			int index2 = (index != 0) ? this.regularBindings.IndexOf(this.visibleBindings[index]) : 0;
			this.regularBindings.Insert(index2, binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Insert(index, binding);
			}
			return true;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00041534 File Offset: 0x0003F934
		public bool ReplaceBinding(BindingSource findBinding, BindingSource withBinding)
		{
			if (findBinding == null || withBinding == null)
			{
				return false;
			}
			if (withBinding.BoundTo != null)
			{
				Debug.LogWarning("Binding source is already bound to action " + withBinding.BoundTo.Name);
				return false;
			}
			int num = this.regularBindings.IndexOf(findBinding);
			if (num < 0)
			{
				Debug.LogWarning("Binding source to replace is not present in this action.");
				return false;
			}
			Debug.Log("index = " + num);
			findBinding.BoundTo = null;
			this.regularBindings[num] = withBinding;
			withBinding.BoundTo = this;
			num = this.visibleBindings.IndexOf(findBinding);
			if (num >= 0)
			{
				this.visibleBindings[num] = withBinding;
			}
			return true;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000415F4 File Offset: 0x0003F9F4
		internal bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			BindingSource bindingSource = this.FindBinding(binding);
			return !(bindingSource == null) && bindingSource.BoundTo == this;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00041630 File Offset: 0x0003FA30
		internal BindingSource FindBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return null;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				return this.regularBindings[num];
			}
			return null;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00041670 File Offset: 0x0003FA70
		internal void FindAndRemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				BindingSource bindingSource = this.regularBindings[num];
				if (bindingSource.BoundTo == this)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(num);
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x000416D0 File Offset: 0x0003FAD0
		internal int CountBindingsOfType(BindingSourceType bindingSourceType)
		{
			int num = 0;
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x00041728 File Offset: 0x0003FB28
		internal void RemoveFirstBindingOfType(BindingSourceType bindingSourceType)
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0004178C File Offset: 0x0003FB8C
		internal int IndexOfFirstInvalidBinding()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.regularBindings[i].IsValid)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x000417D0 File Offset: 0x0003FBD0
		public void RemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			if (binding.BoundTo != this)
			{
				throw new InControlException("Cannot remove a binding source not bound to this action.");
			}
			binding.BoundTo = null;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x000417FD File Offset: 0x0003FBFD
		public void RemoveBindingAt(int index)
		{
			if (index < 0 || index >= this.regularBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			this.regularBindings[index].BoundTo = null;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00041834 File Offset: 0x0003FC34
		public void ClearBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				this.regularBindings[i].BoundTo = null;
			}
			this.regularBindings.Clear();
			this.visibleBindings.Clear();
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00041888 File Offset: 0x0003FC88
		public void ResetBindings()
		{
			this.ClearBindings();
			this.regularBindings.AddRange(this.defaultBindings);
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				bindingSource.BoundTo = this;
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x000418F5 File Offset: 0x0003FCF5
		public void ListenForBinding()
		{
			this.ListenForBindingReplacing(null);
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00041900 File Offset: 0x0003FD00
		public void ListenForBindingReplacing(BindingSource binding)
		{
			BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
			bindingListenOptions.ReplaceBinding = binding;
			this.Owner.listenWithAction = this;
			int num = PlayerAction.bindingSourceListeners.Length;
			for (int i = 0; i < num; i++)
			{
				PlayerAction.bindingSourceListeners[i].Reset();
			}
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0004195F File Offset: 0x0003FD5F
		public void StopListeningForBinding()
		{
			if (this.IsListeningForBinding)
			{
				this.Owner.listenWithAction = null;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00041978 File Offset: 0x0003FD78
		public bool IsListeningForBinding
		{
			get
			{
				return this.Owner.listenWithAction == this;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00041988 File Offset: 0x0003FD88
		public ReadOnlyCollection<BindingSource> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00041990 File Offset: 0x0003FD90
		private void RemoveOrphanedBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				if (this.regularBindings[i].BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000419E0 File Offset: 0x0003FDE0
		internal void Update(ulong updateTick, float deltaTime, InputDevice device)
		{
			this.Device = device;
			this.UpdateBindings(updateTick, deltaTime);
			this.DetectBindings();
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000419F8 File Offset: 0x0003FDF8
		private void UpdateBindings(ulong updateTick, float deltaTime)
		{
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
					this.visibleBindings.Remove(bindingSource);
				}
				else
				{
					float value = bindingSource.GetValue(this.Device);
					if (base.UpdateWithValue(value, updateTick, deltaTime))
					{
						this.LastInputType = bindingSource.BindingSourceType;
					}
				}
			}
			base.Commit();
			this.Enabled = this.Owner.Enabled;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00041A9C File Offset: 0x0003FE9C
		private void DetectBindings()
		{
			if (this.IsListeningForBinding)
			{
				BindingSource bindingSource = null;
				BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
				int num = PlayerAction.bindingSourceListeners.Length;
				for (int i = 0; i < num; i++)
				{
					bindingSource = PlayerAction.bindingSourceListeners[i].Listen(bindingListenOptions, this.device);
					if (bindingSource != null)
					{
						break;
					}
				}
				if (bindingSource == null)
				{
					return;
				}
				Func<PlayerAction, BindingSource, bool> onBindingFound = bindingListenOptions.OnBindingFound;
				if (onBindingFound != null && !onBindingFound(this, bindingSource))
				{
					return;
				}
				if (this.HasBinding(bindingSource))
				{
					Action<PlayerAction, BindingSource, BindingSourceRejectionType> onBindingRejected = bindingListenOptions.OnBindingRejected;
					if (onBindingRejected != null)
					{
						onBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnAction);
					}
					return;
				}
				if (bindingListenOptions.UnsetDuplicateBindingsOnSet)
				{
					this.Owner.RemoveBinding(bindingSource);
				}
				if (!bindingListenOptions.AllowDuplicateBindingsPerSet && this.Owner.HasBinding(bindingSource))
				{
					Action<PlayerAction, BindingSource, BindingSourceRejectionType> onBindingRejected2 = bindingListenOptions.OnBindingRejected;
					if (onBindingRejected2 != null)
					{
						onBindingRejected2(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnActionSet);
					}
					return;
				}
				this.StopListeningForBinding();
				if (bindingListenOptions.ReplaceBinding == null)
				{
					if (bindingListenOptions.MaxAllowedBindingsPerType > 0U)
					{
						while ((long)this.CountBindingsOfType(bindingSource.BindingSourceType) >= (long)((ulong)bindingListenOptions.MaxAllowedBindingsPerType))
						{
							this.RemoveFirstBindingOfType(bindingSource.BindingSourceType);
						}
					}
					else if (bindingListenOptions.MaxAllowedBindings > 0U)
					{
						while ((long)this.regularBindings.Count >= (long)((ulong)bindingListenOptions.MaxAllowedBindings))
						{
							int index = Mathf.Max(0, this.IndexOfFirstInvalidBinding());
							this.regularBindings.RemoveAt(index);
						}
					}
					this.AddBinding(bindingSource);
				}
				else
				{
					this.ReplaceBinding(bindingListenOptions.ReplaceBinding, bindingSource);
				}
				this.UpdateVisibleBindings();
				Action<PlayerAction, BindingSource> onBindingAdded = bindingListenOptions.OnBindingAdded;
				if (onBindingAdded != null)
				{
					onBindingAdded(this, bindingSource);
				}
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00041C7C File Offset: 0x0004007C
		private void UpdateVisibleBindings()
		{
			this.visibleBindings.Clear();
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x00041CD6 File Offset: 0x000400D6
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x00041D00 File Offset: 0x00040100
		internal InputDevice Device
		{
			get
			{
				if (this.device == null)
				{
					this.device = this.Owner.Device;
					this.UpdateVisibleBindings();
				}
				return this.device;
			}
			set
			{
				if (this.device != value)
				{
					this.device = value;
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00041D1C File Offset: 0x0004011C
		internal void Load(BinaryReader reader)
		{
			this.ClearBindings();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				BindingSourceType bindingSourceType = (BindingSourceType)reader.ReadInt32();
				BindingSource bindingSource;
				switch (bindingSourceType)
				{
				case BindingSourceType.DeviceBindingSource:
					bindingSource = new DeviceBindingSource();
					break;
				case BindingSourceType.KeyBindingSource:
					bindingSource = new KeyBindingSource();
					break;
				case BindingSourceType.MouseBindingSource:
					bindingSource = new MouseBindingSource();
					break;
				case BindingSourceType.UnknownDeviceBindingSource:
					bindingSource = new UnknownDeviceBindingSource();
					break;
				default:
					throw new InControlException("Don't know how to load BindingSourceType: " + bindingSourceType);
				}
				bindingSource.Load(reader);
				this.AddBinding(bindingSource);
			}
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00041DC0 File Offset: 0x000401C0
		internal void Save(BinaryWriter writer)
		{
			this.RemoveOrphanedBindings();
			writer.Write(this.Name);
			int count = this.regularBindings.Count;
			writer.Write(count);
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				writer.Write((int)bindingSource.BindingSourceType);
				bindingSource.Save(writer);
			}
		}

		// Token: 0x040009E4 RID: 2532
		public BindingListenOptions ListenOptions;

		// Token: 0x040009E5 RID: 2533
		public BindingSourceType LastInputType;

		// Token: 0x040009E6 RID: 2534
		private List<BindingSource> defaultBindings = new List<BindingSource>();

		// Token: 0x040009E7 RID: 2535
		private List<BindingSource> regularBindings = new List<BindingSource>();

		// Token: 0x040009E8 RID: 2536
		private List<BindingSource> visibleBindings = new List<BindingSource>();

		// Token: 0x040009E9 RID: 2537
		private readonly ReadOnlyCollection<BindingSource> bindings;

		// Token: 0x040009EA RID: 2538
		private static readonly BindingSourceListener[] bindingSourceListeners = new BindingSourceListener[]
		{
			new DeviceBindingSourceListener(),
			new UnknownDeviceBindingSourceListener(),
			new KeyBindingSourceListener(),
			new MouseBindingSourceListener()
		};

		// Token: 0x040009EB RID: 2539
		private InputDevice device;
	}
}

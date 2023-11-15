using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200096F RID: 2415
public abstract class SimComponent : KMonoBehaviour, ISim200ms
{
	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x060046D6 RID: 18134 RVA: 0x00190054 File Offset: 0x0018E254
	public bool IsSimActive
	{
		get
		{
			return this.simActive;
		}
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x0019005C File Offset: 0x0018E25C
	protected virtual void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
	}

	// Token: 0x060046D8 RID: 18136 RVA: 0x0019005E File Offset: 0x0018E25E
	protected virtual void OnSimRegistered()
	{
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x00190060 File Offset: 0x0018E260
	protected virtual void OnSimActivate()
	{
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x00190062 File Offset: 0x0018E262
	protected virtual void OnSimDeactivate()
	{
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x00190064 File Offset: 0x0018E264
	protected virtual void OnSimUnregister()
	{
	}

	// Token: 0x060046DC RID: 18140
	protected abstract Action<int> GetStaticUnregister();

	// Token: 0x060046DD RID: 18141 RVA: 0x00190066 File Offset: 0x0018E266
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x0019006E File Offset: 0x0018E26E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SimRegister();
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x0019007C File Offset: 0x0018E27C
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x0019008A File Offset: 0x0018E28A
	public void SetSimActive(bool active)
	{
		this.simActive = active;
		this.dirty = true;
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x0019009A File Offset: 0x0018E29A
	public void Sim200ms(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x001900B0 File Offset: 0x0018E2B0
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			this.OnSimActivate();
			return;
		}
		this.OnSimDeactivate();
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x001900D8 File Offset: 0x0018E2D8
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			Action<int> static_unregister = this.GetStaticUnregister();
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle = Game.Instance.simComponentCallbackManager.Add(delegate(int handle, object data)
			{
				SimComponent.OnSimRegistered(this, handle, static_unregister);
			}, this, "SimComponent.SimRegister");
			this.OnSimRegister(cb_handle);
		}
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x00190140 File Offset: 0x0018E340
	private void SimUnregister()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			this.OnSimUnregister();
		}
		this.simHandle = -1;
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x0019015C File Offset: 0x0018E35C
	private static void OnSimRegistered(SimComponent instance, int handle, Action<int> static_unregister)
	{
		if (instance != null)
		{
			instance.simHandle = handle;
			instance.OnSimRegistered();
			return;
		}
		static_unregister(handle);
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x0019017C File Offset: 0x0018E37C
	[Conditional("ENABLE_LOGGER")]
	protected void Log(string msg)
	{
	}

	// Token: 0x04002EFA RID: 12026
	[SerializeField]
	protected int simHandle = -1;

	// Token: 0x04002EFB RID: 12027
	private bool simActive = true;

	// Token: 0x04002EFC RID: 12028
	private bool dirty = true;
}

using System;
using System.Collections.Generic;

// Token: 0x0200054B RID: 1355
public class DevPanelList
{
	// Token: 0x0600210B RID: 8459 RVA: 0x000B0078 File Offset: 0x000AE278
	public DevPanel AddPanelFor<T>() where T : DevTool, new()
	{
		return this.AddPanelFor(Activator.CreateInstance<T>());
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000B008C File Offset: 0x000AE28C
	public DevPanel AddPanelFor(DevTool devTool)
	{
		DevPanel devPanel = new DevPanel(devTool, this);
		this.activePanels.Add(devPanel);
		return devPanel;
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000B00B0 File Offset: 0x000AE2B0
	public Option<T> GetDevTool<T>() where T : DevTool
	{
		foreach (DevPanel devPanel in this.activePanels)
		{
			T t = devPanel.GetCurrentDevTool() as T;
			if (t != null)
			{
				return t;
			}
		}
		return Option.None;
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000B0128 File Offset: 0x000AE328
	public T AddOrGetDevTool<T>() where T : DevTool, new()
	{
		bool flag;
		T t;
		this.GetDevTool<T>().Deconstruct(out flag, out t);
		bool flag2 = flag;
		T t2 = t;
		if (!flag2)
		{
			t2 = Activator.CreateInstance<T>();
			this.AddPanelFor(t2);
		}
		return t2;
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000B0160 File Offset: 0x000AE360
	public void ClosePanel(DevPanel panel)
	{
		if (this.activePanels.Remove(panel))
		{
			panel.Internal_Uninit();
		}
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000B0178 File Offset: 0x000AE378
	public void Render()
	{
		if (this.activePanels.Count == 0)
		{
			return;
		}
		using (ListPool<DevPanel, DevPanelList>.PooledList pooledList = ListPool<DevPanel, DevPanelList>.Allocate())
		{
			for (int i = 0; i < this.activePanels.Count; i++)
			{
				DevPanel devPanel = this.activePanels[i];
				devPanel.RenderPanel();
				if (devPanel.isRequestingToClose)
				{
					pooledList.Add(devPanel);
				}
			}
			foreach (DevPanel panel in pooledList)
			{
				this.ClosePanel(panel);
			}
		}
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000B022C File Offset: 0x000AE42C
	public void Internal_InitPanelId(Type initialDevToolType, out string panelId, out uint idPostfixNumber)
	{
		idPostfixNumber = this.Internal_GetUniqueIdPostfix(initialDevToolType);
		panelId = initialDevToolType.Name + idPostfixNumber.ToString();
	}

	// Token: 0x06002112 RID: 8466 RVA: 0x000B024C File Offset: 0x000AE44C
	public uint Internal_GetUniqueIdPostfix(Type initialDevToolType)
	{
		uint result;
		using (HashSetPool<uint, DevPanelList>.PooledHashSet pooledHashSet = HashSetPool<uint, DevPanelList>.Allocate())
		{
			foreach (DevPanel devPanel in this.activePanels)
			{
				if (!(devPanel.initialDevToolType != initialDevToolType))
				{
					pooledHashSet.Add(devPanel.idPostfixNumber);
				}
			}
			for (uint num = 0U; num < 100U; num += 1U)
			{
				if (!pooledHashSet.Contains(num))
				{
					return num;
				}
			}
			Debug.Assert(false, "Something went wrong, this should only assert if there's over 100 of the same type of debug window");
			uint num2 = this.fallbackUniqueIdPostfixNumber;
			this.fallbackUniqueIdPostfixNumber = num2 + 1U;
			result = num2;
		}
		return result;
	}

	// Token: 0x040012A4 RID: 4772
	private List<DevPanel> activePanels = new List<DevPanel>();

	// Token: 0x040012A5 RID: 4773
	private uint fallbackUniqueIdPostfixNumber = 300U;
}

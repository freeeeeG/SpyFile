using System;
using UnityEngine;

// Token: 0x020009B0 RID: 2480
public class ScrollingListControlsHelper
{
	// Token: 0x0600308F RID: 12431 RVA: 0x000E4688 File Offset: 0x000E2A88
	public void Init(IScrollingListUI _gui, float _kerchunkInterval)
	{
		this.m_gui = _gui;
		this.m_kerchunkInterval = _kerchunkInterval;
		this.m_upButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIUp, PlayerInputLookup.Player.One);
		this.m_downButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIDown, PlayerInputLookup.Player.One);
		this.m_selectButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
		this.m_cancelButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x000E46D9 File Offset: 0x000E2AD9
	public void RegisterSelectionCallback(VoidGeneric<int> _callback)
	{
		this.m_selectionCallbacks = (VoidGeneric<int>)Delegate.Combine(this.m_selectionCallbacks, _callback);
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x000E46F2 File Offset: 0x000E2AF2
	public void UnregisterSelectionCallback(VoidGeneric<int> _callback)
	{
		this.m_selectionCallbacks = (VoidGeneric<int>)Delegate.Remove(this.m_selectionCallbacks, _callback);
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x000E470B File Offset: 0x000E2B0B
	public void RegisterSelectionChangeCallback(VoidGeneric<int, int> _callback)
	{
		this.m_selectionChangeCallbacks = (VoidGeneric<int, int>)Delegate.Combine(this.m_selectionChangeCallbacks, _callback);
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x000E4724 File Offset: 0x000E2B24
	public void UnregisterSelectionChangeCallback(VoidGeneric<int, int> _callback)
	{
		this.m_selectionChangeCallbacks = (VoidGeneric<int, int>)Delegate.Remove(this.m_selectionChangeCallbacks, _callback);
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x000E473D File Offset: 0x000E2B3D
	public void RegisterCancelCallback(VoidToVoid _callback)
	{
		this.m_cancelCallback = (VoidToVoid)Delegate.Combine(this.m_cancelCallback, _callback);
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x000E4756 File Offset: 0x000E2B56
	public void UnregisterCancelCallback(VoidToVoid _callback)
	{
		this.m_cancelCallback = (VoidToVoid)Delegate.Remove(this.m_cancelCallback, _callback);
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x000E4770 File Offset: 0x000E2B70
	public void Update()
	{
		GameObject gameObject = (this.m_gui as MonoBehaviour).gameObject;
		if (this.m_upButton.IsDown() || (this.m_downButton.IsDown() && !TimeManager.IsPaused(gameObject)))
		{
			this.m_moveTimer += TimeManager.GetDeltaTime(gameObject);
			if (this.m_moveTimer >= 0f)
			{
				this.m_moveTimer = -this.m_kerchunkInterval;
				int selection = this.m_gui.GetSelection();
				if (this.m_upButton.IsDown())
				{
					this.m_gui.MoveUp();
				}
				else
				{
					this.m_gui.MoveDown();
				}
				int selection2 = this.m_gui.GetSelection();
				if (selection != selection2)
				{
					this.m_selectionChangeCallbacks(selection, selection2);
				}
			}
		}
		else
		{
			this.m_moveTimer = 0f;
		}
		if (this.m_selectButton.JustPressed() && !TimeManager.IsPaused(gameObject))
		{
			this.m_selectionCallbacks(this.m_gui.GetSelection());
		}
		if (this.m_cancelButton.JustPressed() && !TimeManager.IsPaused(gameObject))
		{
			this.m_cancelCallback();
		}
	}

	// Token: 0x04002700 RID: 9984
	private IScrollingListUI m_gui;

	// Token: 0x04002701 RID: 9985
	private float m_kerchunkInterval;

	// Token: 0x04002702 RID: 9986
	private ILogicalButton m_upButton;

	// Token: 0x04002703 RID: 9987
	private ILogicalButton m_downButton;

	// Token: 0x04002704 RID: 9988
	private ILogicalButton m_selectButton;

	// Token: 0x04002705 RID: 9989
	private ILogicalButton m_cancelButton;

	// Token: 0x04002706 RID: 9990
	private float m_moveTimer;

	// Token: 0x04002707 RID: 9991
	private VoidGeneric<int> m_selectionCallbacks = delegate(int _selectionId)
	{
	};

	// Token: 0x04002708 RID: 9992
	private VoidGeneric<int, int> m_selectionChangeCallbacks = delegate(int _param1, int _param2)
	{
	};

	// Token: 0x04002709 RID: 9993
	private VoidToVoid m_cancelCallback = delegate()
	{
	};

	// Token: 0x020009B1 RID: 2481
	// (Invoke) Token: 0x0600309B RID: 12443
	public delegate void SelectionCallback(int _selectionId);
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B3A RID: 2874
public static class KleiItemsStatusRefresher
{
	// Token: 0x060058BF RID: 22719 RVA: 0x002082B0 File Offset: 0x002064B0
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize()
	{
		KleiItems.AddInventoryRefreshCallback(new KleiItems.InventoryRefreshCallback(KleiItemsStatusRefresher.OnRefreshResponseFromServer));
	}

	// Token: 0x060058C0 RID: 22720 RVA: 0x002082C4 File Offset: 0x002064C4
	private static void OnRefreshResponseFromServer()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x060058C1 RID: 22721 RVA: 0x00208314 File Offset: 0x00206514
	public static void Refresh()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x060058C2 RID: 22722 RVA: 0x00208364 File Offset: 0x00206564
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(Component component)
	{
		return KleiItemsStatusRefresher.AddOrGetListener(component.gameObject);
	}

	// Token: 0x060058C3 RID: 22723 RVA: 0x00208371 File Offset: 0x00206571
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(GameObject onGameObject)
	{
		return onGameObject.AddOrGet<KleiItemsStatusRefresher.UIListener>();
	}

	// Token: 0x04003C11 RID: 15377
	public static HashSet<KleiItemsStatusRefresher.UIListener> listeners = new HashSet<KleiItemsStatusRefresher.UIListener>();

	// Token: 0x02001A4F RID: 6735
	public class UIListener : MonoBehaviour
	{
		// Token: 0x060096B2 RID: 38578 RVA: 0x0033DC43 File Offset: 0x0033BE43
		public void Internal_RefreshUI()
		{
			if (this.refreshUIFn != null)
			{
				this.refreshUIFn();
			}
		}

		// Token: 0x060096B3 RID: 38579 RVA: 0x0033DC58 File Offset: 0x0033BE58
		public void OnRefreshUI(System.Action fn)
		{
			this.refreshUIFn = fn;
		}

		// Token: 0x060096B4 RID: 38580 RVA: 0x0033DC61 File Offset: 0x0033BE61
		private void OnEnable()
		{
			KleiItemsStatusRefresher.listeners.Add(this);
		}

		// Token: 0x060096B5 RID: 38581 RVA: 0x0033DC6F File Offset: 0x0033BE6F
		private void OnDisable()
		{
			KleiItemsStatusRefresher.listeners.Remove(this);
		}

		// Token: 0x04007933 RID: 31027
		private System.Action refreshUIFn;
	}
}

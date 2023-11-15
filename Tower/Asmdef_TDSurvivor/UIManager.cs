using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000138 RID: 312
public class UIManager : Singleton<UIManager>
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000809 RID: 2057 RVA: 0x0001EAE6 File Offset: 0x0001CCE6
	public Transform PopupUIAnchor_TopLevel
	{
		get
		{
			return this.node_PopupUIAnchor_TopLayer;
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x0600080A RID: 2058 RVA: 0x0001EAEE File Offset: 0x0001CCEE
	public Transform PopupUIAnchor_MidLevel
	{
		get
		{
			return this.node_PopupUIAnchor_MidLayer;
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0001EAF6 File Offset: 0x0001CCF6
	private void OnValidate()
	{
		if (this.canvas == null)
		{
			this.canvas = Object.FindObjectOfType<Canvas>();
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0001EB14 File Offset: 0x0001CD14
	private new void Awake()
	{
		if (this.dic_UIReferences == null)
		{
			this.dic_UIReferences = new Dictionary<string, AUI>();
		}
		for (int i = 0; i < this.list_UIReferences.Count; i++)
		{
			this.dic_UIReferences.Add(this.list_UIReferences[i].gameObject.name, this.list_UIReferences[i]);
		}
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0001EB78 File Offset: 0x0001CD78
	public Transform GetDynamicUIAnchor()
	{
		if (this.node_DynamicUIAnchor == null && this.canvas != null)
		{
			new GameObject("Node_DynamicSpawn").transform.SetParent(this.canvas.transform);
		}
		return this.node_DynamicUIAnchor;
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x0001EBC6 File Offset: 0x0001CDC6
	public AUI GetUI(string name)
	{
		if (this.dic_UIReferences.ContainsKey(name))
		{
			return this.dic_UIReferences[name];
		}
		Debug.Log("沒有名稱是" + name + "的UI");
		return null;
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0001EBFC File Offset: 0x0001CDFC
	public T GetUI<T>() where T : AUI
	{
		foreach (AUI aui in this.list_UIReferences)
		{
			if (aui is T)
			{
				return (T)((object)aui);
			}
		}
		T t = Object.FindObjectOfType<T>();
		if (t != null)
		{
			return t;
		}
		return default(T);
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0001EC7C File Offset: 0x0001CE7C
	public void RegisterUI(AUI ui)
	{
		if (this.list_UIReferences.Contains(ui))
		{
			return;
		}
		if (this.dic_UIReferences == null)
		{
			this.dic_UIReferences = new Dictionary<string, AUI>();
		}
		this.list_UIReferences.Add(ui);
		this.dic_UIReferences.Add(ui.name, ui);
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x0001ECC9 File Offset: 0x0001CEC9
	public float GetCanvasScale()
	{
		return 1f / this.canvas.transform.localScale.x;
	}

	// Token: 0x04000688 RID: 1672
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000689 RID: 1673
	[SerializeField]
	private List<AUI> list_UIReferences;

	// Token: 0x0400068A RID: 1674
	[SerializeField]
	[FormerlySerializedAs("node_PopupUIAnchor_TopLevel")]
	private Transform node_PopupUIAnchor_TopLayer;

	// Token: 0x0400068B RID: 1675
	[SerializeField]
	[FormerlySerializedAs("node_PopupUIAnchor_MidLevel")]
	private Transform node_PopupUIAnchor_MidLayer;

	// Token: 0x0400068C RID: 1676
	[SerializeField]
	private Transform node_DynamicUIAnchor;

	// Token: 0x0400068D RID: 1677
	private Dictionary<string, AUI> dic_UIReferences;
}

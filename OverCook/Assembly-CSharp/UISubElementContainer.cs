using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000373 RID: 883
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class UISubElementContainer : MonoBehaviour
{
	// Token: 0x060010D6 RID: 4310 RVA: 0x000601F1 File Offset: 0x0005E5F1
	public void RefreshSubElements()
	{
		this.EnsureImagesExist();
		this.m_debugActivated = true;
		this.RefreshSubObjectProperties();
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x00060206 File Offset: 0x0005E606
	protected virtual void OnRefreshSubObjectProperties(GameObject _container)
	{
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x00060208 File Offset: 0x0005E608
	protected virtual void OnCreateSubObjects(GameObject _container)
	{
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0006020C File Offset: 0x0005E60C
	private void RefreshSubObjectProperties()
	{
		if (this.m_container == null)
		{
			return;
		}
		RectTransform rect = this.m_container.RequireComponent<RectTransform>();
		UIUtils.SetupFillParentAreaRect(rect);
		this.OnRefreshSubObjectProperties(this.m_container);
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x0006024C File Offset: 0x0005E64C
	protected virtual void EnsureImagesExist()
	{
		this.DestroyAllChildrenWithName(this.c_oldContainerName);
		this.DestroyAllChildrenWithName(this.c_containerName);
		this.m_container = GameObjectUtils.CreateOnParent<RectTransform>(base.gameObject, this.c_containerName);
		this.m_container.SetActive(false);
		this.OnCreateSubObjects(this.m_container);
		this.m_container.SetActive(true);
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x000602AC File Offset: 0x0005E6AC
	private void DestroyAllChildrenWithName(string _name)
	{
		int childCount = base.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			Transform child = base.transform.GetChild(i);
			if (child.name == _name)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
		}
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0006031C File Offset: 0x0005E71C
	private void DestroyImageComponentsInEditor()
	{
		if (this.m_container && this.m_container.activeInHierarchy)
		{
			UnityEngine.Object.DestroyImmediate(this.m_container);
		}
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x0006034C File Offset: 0x0005E74C
	protected Image CreateImage(GameObject _parent, string _name)
	{
		GameObject gameObject = GameObjectUtils.CreateOnParent<Image>(_parent, _name);
		return gameObject.GetComponent<Image>();
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00060368 File Offset: 0x0005E768
	protected RectTransform CreateRect(GameObject _parent, string _name)
	{
		GameObject gameObject = GameObjectUtils.CreateOnParent<RectTransform>(_parent, _name);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		UIUtils.SetupFillParentAreaRect(component);
		return component;
	}

	// Token: 0x04000CFE RID: 3326
	protected readonly string c_oldContainerName = "GeneratedChildren_ProgressBar";

	// Token: 0x04000CFF RID: 3327
	protected readonly string c_containerName = "GeneratedChildren";

	// Token: 0x04000D00 RID: 3328
	protected GameObject m_container;

	// Token: 0x04000D01 RID: 3329
	private bool m_debugActivated;
}

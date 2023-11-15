using System;
using UnityEngine;

// Token: 0x02000130 RID: 304
[ExecuteInEditMode]
public class EditorPrefabAutoEncasement : MonoBehaviour
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x0002A618 File Offset: 0x00028A18
	// (set) Token: 0x0600058B RID: 1419 RVA: 0x0002A620 File Offset: 0x00028A20
	public GameObject EncasingPrefab
	{
		get
		{
			return this.m_owningPrefab;
		}
		set
		{
			this.m_owningPrefab = value;
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x0002A629 File Offset: 0x00028A29
	public void Refresh()
	{
		this.m_reimport = true;
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x0002A634 File Offset: 0x00028A34
	private void Awake()
	{
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			Transform parent = base.transform.parent;
			Transform transform = base.transform;
			GameObject gameObject = null;
			if (parent != null && parent.gameObject.GetComponent<SpawnedEncaserComponent>() != null)
			{
				transform = parent;
				gameObject = transform.gameObject;
				parent = transform.transform.parent;
			}
			GameObject gameObject2 = this.m_owningPrefab.InstantiateOnParent(parent, true);
			if (gameObject2.GetComponent<SpawnedEncaserComponent>() == null)
			{
				gameObject2.AddComponent<SpawnedEncaserComponent>();
			}
			gameObject2.transform.localPosition = transform.localPosition;
			gameObject2.transform.localRotation = transform.localRotation;
			gameObject2.transform.localScale = transform.localScale;
			base.transform.SetParent(gameObject2.transform);
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
			base.enabled = true;
		}
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002A725 File Offset: 0x00028B25
	private void Update()
	{
		if (this.m_reimport)
		{
			this.Awake();
			this.m_reimport = false;
		}
	}

	// Token: 0x040004AD RID: 1197
	[SerializeField]
	private GameObject m_owningPrefab;

	// Token: 0x040004AE RID: 1198
	private bool m_reimport;
}

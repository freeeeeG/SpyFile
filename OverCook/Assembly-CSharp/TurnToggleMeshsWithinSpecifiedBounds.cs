using System;
using UnityEngine;

// Token: 0x02000A6C RID: 2668
public class TurnToggleMeshsWithinSpecifiedBounds : MonoBehaviour
{
	// Token: 0x060034B8 RID: 13496 RVA: 0x000F7570 File Offset: 0x000F5970
	private void Start()
	{
		this.cachedRenderers = new Renderer[this.objectsToHandle.Length][];
		int num = this.objectsToHandle.Length;
		for (int i = 0; i < num; i++)
		{
			this.cachedRenderers[i] = this.objectsToHandle[i].GetComponentsInChildren<Renderer>();
		}
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x000F75C0 File Offset: 0x000F59C0
	private void Update()
	{
		for (int i = 0; i < this.objectsToHandle.Length; i++)
		{
			GameObject gameObject = this.objectsToHandle[i];
			if (gameObject != null)
			{
				Transform transform = gameObject.transform;
				if (transform != null)
				{
					if (this.IsWithinXRegion(transform) && this.IsWithinYRegion(transform) && this.IsWithinZRegion(transform))
					{
						this.SetRenderersToState(this.cachedRenderers[i], true);
					}
					else
					{
						this.SetRenderersToState(this.cachedRenderers[i], false);
					}
				}
			}
		}
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x000F7658 File Offset: 0x000F5A58
	private bool IsWithinXRegion(Transform targetTransform)
	{
		return !this.checkXBounds || this.IsValueInRangeInclusive(targetTransform.position.x, this.boxBottomLeftWorldSpace.position.x, this.boxTopRightWorldSpace.position.x);
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x000F76B4 File Offset: 0x000F5AB4
	private bool IsWithinYRegion(Transform targetTransform)
	{
		return !this.checkYBounds || this.IsValueInRangeInclusive(targetTransform.position.y, this.boxBottomLeftWorldSpace.position.y, this.boxTopRightWorldSpace.position.y);
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x000F7710 File Offset: 0x000F5B10
	private bool IsWithinZRegion(Transform targetTransform)
	{
		return !this.checkZBounds || this.IsValueInRangeInclusive(targetTransform.position.z, this.boxBottomLeftWorldSpace.position.z, this.boxTopRightWorldSpace.position.z);
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x000F776C File Offset: 0x000F5B6C
	private void SetRenderersToState(Renderer[] targetRenderers, bool state)
	{
		int num = targetRenderers.Length;
		for (int i = 0; i < num; i++)
		{
			targetRenderers[i].enabled = state;
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x000F7798 File Offset: 0x000F5B98
	private bool IsValueInRangeInclusive(float value, float min, float max)
	{
		return value >= min && value <= max;
	}

	// Token: 0x04002A40 RID: 10816
	[SerializeField]
	private GameObject[] objectsToHandle;

	// Token: 0x04002A41 RID: 10817
	[SerializeField]
	private Transform boxBottomLeftWorldSpace;

	// Token: 0x04002A42 RID: 10818
	[SerializeField]
	private Transform boxTopRightWorldSpace;

	// Token: 0x04002A43 RID: 10819
	[SerializeField]
	private bool checkXBounds;

	// Token: 0x04002A44 RID: 10820
	[SerializeField]
	private bool checkYBounds;

	// Token: 0x04002A45 RID: 10821
	[SerializeField]
	private bool checkZBounds;

	// Token: 0x04002A46 RID: 10822
	private Renderer[][] cachedRenderers;
}

using System;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class MB2_UpdateSkinnedMeshBoundsFromBones : MonoBehaviour
{
	// Token: 0x06000234 RID: 564 RVA: 0x00019D2C File Offset: 0x0001812C
	private void Start()
	{
		this.smr = base.GetComponent<SkinnedMeshRenderer>();
		if (this.smr == null)
		{
			Debug.LogError("Need to attach MB2_UpdateSkinnedMeshBoundsFromBones script to an object with a SkinnedMeshRenderer component attached.");
			return;
		}
		this.bones = this.smr.bones;
		bool updateWhenOffscreen = this.smr.updateWhenOffscreen;
		this.smr.updateWhenOffscreen = true;
		this.smr.updateWhenOffscreen = updateWhenOffscreen;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00019D96 File Offset: 0x00018196
	private void Update()
	{
		if (this.smr != null)
		{
			MB3_MeshCombiner.UpdateSkinnedMeshApproximateBoundsFromBonesStatic(this.bones, this.smr);
		}
	}

	// Token: 0x04000178 RID: 376
	private SkinnedMeshRenderer smr;

	// Token: 0x04000179 RID: 377
	private Transform[] bones;
}

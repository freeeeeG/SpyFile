using System;
using UnityEngine;

// Token: 0x020003FF RID: 1023
[RequireComponent(typeof(Terminal))]
public class TerminalCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000E93 RID: 3731
	public HoverIconUIController MoveIconPrefab;

	// Token: 0x04000E94 RID: 3732
	public Vector3 Iconoffset;

	// Token: 0x04000E95 RID: 3733
	public Material ActiveMaterial;

	// Token: 0x04000E96 RID: 3734
	public Material DisabledMaterial;

	// Token: 0x04000E97 RID: 3735
	public Material InUseMaterial;

	// Token: 0x04000E98 RID: 3736
	public Renderer[] ColouredMeshes = new Renderer[0];

	// Token: 0x04000E99 RID: 3737
	public Transform ShaftTransform;

	// Token: 0x04000E9A RID: 3738
	public float JoystickMaxAngle = 45f;

	// Token: 0x04000E9B RID: 3739
	public GameLoopingAudioTag Moving = GameLoopingAudioTag.MovingPlatform;

	// Token: 0x04000E9C RID: 3740
	public GameOneShotAudioTag StartMoving = GameOneShotAudioTag.MovingPlatformStart;

	// Token: 0x04000E9D RID: 3741
	public GameOneShotAudioTag StopMoving = GameOneShotAudioTag.MovingPlatformStop;

	// Token: 0x04000E9E RID: 3742
	[HideInInspector]
	public bool IsPlaying;
}

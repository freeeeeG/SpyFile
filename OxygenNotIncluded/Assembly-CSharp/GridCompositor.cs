using System;
using UnityEngine;

// Token: 0x0200090F RID: 2319
public class GridCompositor : MonoBehaviour
{
	// Token: 0x06004334 RID: 17204 RVA: 0x00178594 File Offset: 0x00176794
	public static void DestroyInstance()
	{
		GridCompositor.Instance = null;
	}

	// Token: 0x06004335 RID: 17205 RVA: 0x0017859C File Offset: 0x0017679C
	private void Awake()
	{
		GridCompositor.Instance = this;
		base.enabled = false;
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x001785AB File Offset: 0x001767AB
	private void Start()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/GridCompositor"));
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x001785C2 File Offset: 0x001767C2
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.material);
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x001785D1 File Offset: 0x001767D1
	public void ToggleMajor(bool on)
	{
		this.onMajor = on;
		this.Refresh();
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x001785E0 File Offset: 0x001767E0
	public void ToggleMinor(bool on)
	{
		this.onMinor = on;
		this.Refresh();
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x001785EF File Offset: 0x001767EF
	private void Refresh()
	{
		base.enabled = (this.onMinor || this.onMajor);
	}

	// Token: 0x04002BCD RID: 11213
	public Material material;

	// Token: 0x04002BCE RID: 11214
	public static GridCompositor Instance;

	// Token: 0x04002BCF RID: 11215
	private bool onMajor;

	// Token: 0x04002BD0 RID: 11216
	private bool onMinor;
}

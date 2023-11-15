using System;
using UnityEngine;

// Token: 0x020008CA RID: 2250
[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
	// Token: 0x06004125 RID: 16677 RVA: 0x0016CB01 File Offset: 0x0016AD01
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x06004126 RID: 16678 RVA: 0x0016CB2D File Offset: 0x0016AD2D
	public void SetReactable(Reactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x06004127 RID: 16679 RVA: 0x0016CB38 File Offset: 0x0016AD38
	private void OnExpire(object obj)
	{
		if (!this.reactable.IsReacting)
		{
			this.reactable.Cleanup();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x04002A69 RID: 10857
	private Reactable reactable;

	// Token: 0x04002A6A RID: 10858
	public float lifetime = 1f;
}

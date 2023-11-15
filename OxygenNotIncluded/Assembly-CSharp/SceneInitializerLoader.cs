using System;
using UnityEngine;

// Token: 0x02000954 RID: 2388
public class SceneInitializerLoader : MonoBehaviour
{
	// Token: 0x0600460A RID: 17930 RVA: 0x0018C2D8 File Offset: 0x0018A4D8
	private void Awake()
	{
		Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		KMonoBehaviour.isLoadingScene = false;
		Singleton<StateMachineManager>.Instance.Clear();
		Util.KInstantiate(this.sceneInitializer, null, null);
		if (SceneInitializerLoader.ReportDeferredError != null && SceneInitializerLoader.deferred_error.IsValid)
		{
			SceneInitializerLoader.ReportDeferredError(SceneInitializerLoader.deferred_error);
			SceneInitializerLoader.deferred_error = default(SceneInitializerLoader.DeferredError);
		}
	}

	// Token: 0x04002E64 RID: 11876
	public SceneInitializer sceneInitializer;

	// Token: 0x04002E65 RID: 11877
	public static SceneInitializerLoader.DeferredError deferred_error;

	// Token: 0x04002E66 RID: 11878
	public static SceneInitializerLoader.DeferredErrorDelegate ReportDeferredError;

	// Token: 0x020017BD RID: 6077
	public struct DeferredError
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06008F5C RID: 36700 RVA: 0x00322467 File Offset: 0x00320667
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.msg);
			}
		}

		// Token: 0x04006FCB RID: 28619
		public string msg;

		// Token: 0x04006FCC RID: 28620
		public string stack_trace;
	}

	// Token: 0x020017BE RID: 6078
	// (Invoke) Token: 0x06008F5E RID: 36702
	public delegate void DeferredErrorDelegate(SceneInitializerLoader.DeferredError deferred_error);
}

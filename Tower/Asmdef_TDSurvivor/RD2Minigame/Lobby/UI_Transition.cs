using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RD2Minigame.Lobby
{
	// Token: 0x020001C2 RID: 450
	public class UI_Transition : MonoBehaviour
	{
		// Token: 0x06000BCD RID: 3021 RVA: 0x0002E52C File Offset: 0x0002C72C
		private void Awake()
		{
			this.material = Object.Instantiate<Material>(this.image_Transition.material);
			this.image_Transition.material = this.material;
			this.material.SetFloat("_DissolveValue", 1f);
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002E580 File Offset: 0x0002C780
		private void OnEnable()
		{
			EventMgr.Register(eGameEvents.UI_TriggerTransition_Show, new Action(this.OnTriggerTransitionShow));
			EventMgr.Register(eGameEvents.UI_TriggerTransition_Hide, new Action(this.OnTriggerTransitionHide));
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0002E5B8 File Offset: 0x0002C7B8
		private void OnDisable()
		{
			EventMgr.Remove(eGameEvents.UI_TriggerTransition_Show, new Action(this.OnTriggerTransitionShow));
			EventMgr.Remove(eGameEvents.UI_TriggerTransition_Hide, new Action(this.OnTriggerTransitionHide));
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002E5F0 File Offset: 0x0002C7F0
		private void OnTriggerTransitionShow()
		{
			this.StartTransition(false);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0002E5F9 File Offset: 0x0002C7F9
		private void OnTriggerTransitionHide()
		{
			this.StartTransition(true);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0002E604 File Offset: 0x0002C804
		private void Update()
		{
			if (this.DEBUG_AutoTestOn)
			{
				if (this.testTime > 0f)
				{
					this.testTime -= Time.deltaTime;
					return;
				}
				this.testTime = (this.isUIOn ? 1f : 3f);
				this.StartTransition(!this.isUIOn);
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0002E662 File Offset: 0x0002C862
		[ContextMenu("Test_Transition_Hide")]
		private void TEST_ON()
		{
			this.StartTransition(true);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0002E66B File Offset: 0x0002C86B
		[ContextMenu("Test_Transition_Show")]
		private void TEST_OFF()
		{
			this.StartTransition(false);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0002E674 File Offset: 0x0002C874
		public void StartTransition(bool isOn)
		{
			if (this.coroutine_Transition != null)
			{
				base.StopCoroutine(this.coroutine_Transition);
			}
			this.coroutine_Transition = base.StartCoroutine(this.CR_Transition(isOn));
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0002E69D File Offset: 0x0002C89D
		private IEnumerator CR_Transition(bool isOn)
		{
			this.isUIOn = isOn;
			float start = isOn ? this.startShaderDissolveValue : this.endShaderDissolveValue;
			float end = isOn ? this.endShaderDissolveValue : this.startShaderDissolveValue;
			float duration = isOn ? this.transitionTime_Show : this.transitionTime_Hide;
			float timer = 0f;
			this.material.SetFloat("_Direction", isOn ? 0f : 1f);
			this.image_Transition.enabled = true;
			while (timer < duration)
			{
				float time = timer / duration;
				this.material.SetFloat("_DissolveValue", Mathf.Lerp(start, end, this.curve_Transition.Evaluate(time)));
				timer += Time.unscaledDeltaTime;
				yield return null;
			}
			this.material.SetFloat("_DissolveValue", end);
			if (isOn)
			{
				this.image_Transition.enabled = false;
			}
			this.coroutine_Transition = null;
			yield break;
		}

		// Token: 0x04000969 RID: 2409
		[SerializeField]
		private Image image_Transition;

		// Token: 0x0400096A RID: 2410
		[SerializeField]
		private float startShaderDissolveValue = 1f;

		// Token: 0x0400096B RID: 2411
		[SerializeField]
		private float endShaderDissolveValue;

		// Token: 0x0400096C RID: 2412
		[SerializeField]
		private float transitionTime_Show = 1f;

		// Token: 0x0400096D RID: 2413
		[SerializeField]
		private float transitionTime_Hide = 1f;

		// Token: 0x0400096E RID: 2414
		[SerializeField]
		private AnimationCurve curve_Transition;

		// Token: 0x0400096F RID: 2415
		[SerializeField]
		private bool DEBUG_AutoTestOn;

		// Token: 0x04000970 RID: 2416
		private Material material;

		// Token: 0x04000971 RID: 2417
		private bool isUIOn;

		// Token: 0x04000972 RID: 2418
		private float testTime;

		// Token: 0x04000973 RID: 2419
		private Coroutine coroutine_Transition;
	}
}

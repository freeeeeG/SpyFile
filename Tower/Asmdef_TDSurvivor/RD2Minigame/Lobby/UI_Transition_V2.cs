using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RD2Minigame.Lobby
{
	// Token: 0x020001C3 RID: 451
	public class UI_Transition_V2 : MonoBehaviour
	{
		// Token: 0x06000BD8 RID: 3032 RVA: 0x0002E6DC File Offset: 0x0002C8DC
		private void Awake()
		{
			this.material = Object.Instantiate<Material>(this.image_Transition.material);
			this.image_Transition.material = this.material;
			this.material.SetFloat("_DissolveValue", 1f);
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0002E730 File Offset: 0x0002C930
		private void OnEnable()
		{
			EventMgr.Register(eGameEvents.UI_TriggerTransition_Show, new Action(this.OnTriggerTransitionShow));
			EventMgr.Register(eGameEvents.UI_TriggerTransition_Hide, new Action(this.OnTriggerTransitionHide));
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0002E768 File Offset: 0x0002C968
		private void OnDisable()
		{
			EventMgr.Remove(eGameEvents.UI_TriggerTransition_Show, new Action(this.OnTriggerTransitionShow));
			EventMgr.Remove(eGameEvents.UI_TriggerTransition_Hide, new Action(this.OnTriggerTransitionHide));
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0002E7A0 File Offset: 0x0002C9A0
		private void OnTriggerTransitionShow()
		{
			this.StartTransition(false);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0002E7A9 File Offset: 0x0002C9A9
		private void OnTriggerTransitionHide()
		{
			this.StartTransition(true);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0002E7B4 File Offset: 0x0002C9B4
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

		// Token: 0x06000BDE RID: 3038 RVA: 0x0002E812 File Offset: 0x0002CA12
		[ContextMenu("Test_Transition_Hide")]
		private void TEST_ON()
		{
			this.StartTransition(true);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0002E81B File Offset: 0x0002CA1B
		[ContextMenu("Test_Transition_Show")]
		private void TEST_OFF()
		{
			this.StartTransition(false);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0002E824 File Offset: 0x0002CA24
		public void StartTransition(bool isOn)
		{
			if (this.coroutine_Transition != null)
			{
				base.StopCoroutine(this.coroutine_Transition);
			}
			this.coroutine_Transition = base.StartCoroutine(this.CR_Transition(isOn));
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002E84D File Offset: 0x0002CA4D
		private IEnumerator CR_Transition(bool isOn)
		{
			this.isUIOn = isOn;
			float start = isOn ? this.startShaderDissolveValue : this.endShaderDissolveValue;
			float end = isOn ? this.endShaderDissolveValue : this.startShaderDissolveValue;
			float duration = isOn ? this.transitionTime_Show : this.transitionTime_Hide;
			float timer = 0f;
			this.image_Transition.enabled = true;
			while (timer < duration)
			{
				float time = timer / duration;
				this.material.SetFloat("_DissolveValue", Mathf.Lerp(start, end, this.curve_Transition.Evaluate(time)));
				timer += Time.deltaTime;
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

		// Token: 0x04000974 RID: 2420
		[SerializeField]
		private Image image_Transition;

		// Token: 0x04000975 RID: 2421
		[SerializeField]
		private float startShaderDissolveValue = 1f;

		// Token: 0x04000976 RID: 2422
		[SerializeField]
		private float endShaderDissolveValue;

		// Token: 0x04000977 RID: 2423
		[SerializeField]
		private float transitionTime_Show = 1f;

		// Token: 0x04000978 RID: 2424
		[SerializeField]
		private float transitionTime_Hide = 1f;

		// Token: 0x04000979 RID: 2425
		[SerializeField]
		private AnimationCurve curve_Transition;

		// Token: 0x0400097A RID: 2426
		[SerializeField]
		private bool DEBUG_AutoTestOn;

		// Token: 0x0400097B RID: 2427
		private Material material;

		// Token: 0x0400097C RID: 2428
		private bool isUIOn;

		// Token: 0x0400097D RID: 2429
		private float testTime;

		// Token: 0x0400097E RID: 2430
		private Coroutine coroutine_Transition;
	}
}

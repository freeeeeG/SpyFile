using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;

namespace Tantawowa.TimelineEvents
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class TimelineEventBehaviour : PlayableBehaviour
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00007AC0 File Offset: 0x00005CC0
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (info.frameId == 0UL || info.deltaTime > 0f)
			{
				this.UpdateDelegates();
				if (this.invocationInfo != null)
				{
					this.invocationInfo.Invoke(this.IsMethodWithParam, this.ArgValue);
				}
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007B00 File Offset: 0x00005D00
		private void UpdateDelegates()
		{
			bool isEnabled = Application.isPlaying || this.InvokeEventsInEditMode;
			this.invocationInfo = this.GetInvocationInfo(isEnabled, this.HandlerKey, this.invocationInfo, this.IsMethodWithParam);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007B40 File Offset: 0x00005D40
		private EventInvocationInfo GetInvocationInfo(bool isEnabled, string methodKey, EventInvocationInfo currentInfo, bool methodWitharg)
		{
			if (currentInfo != null && currentInfo.Key == methodKey && !string.IsNullOrEmpty(methodKey) && !(methodKey.ToLower() == "none"))
			{
				return currentInfo;
			}
			Behaviour behaviour = null;
			string methodName = null;
			this.GetBehaviourAndMethod(isEnabled, methodKey, ref behaviour, ref methodName);
			if (behaviour != null)
			{
				MethodInfo methodInfo = behaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault((MethodInfo m) => m.Name == methodName && m.ReturnType == typeof(void) && m.GetParameters().Length == (methodWitharg ? 1 : 0));
				return new EventInvocationInfo(methodKey, behaviour, methodInfo);
			}
			return null;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007BD4 File Offset: 0x00005DD4
		private void GetBehaviourAndMethod(bool isEnabled, string key, ref Behaviour targetBehaviour, ref string methodName)
		{
			if (!isEnabled || string.IsNullOrEmpty(key) || key.ToLower() == "none")
			{
				return;
			}
			if (!string.IsNullOrEmpty(key))
			{
				int num = key.LastIndexOf('.');
				string text = key.Substring(0, num);
				methodName = key.Substring(num + 1, key.Length - (num + 1));
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(methodName))
				{
					throw new Exception("Unable to parse callback method: " + key);
				}
				targetBehaviour = null;
				if (this.TargetObject == null)
				{
					throw new Exception("No target set for key " + key);
				}
				foreach (Behaviour behaviour in this.TargetObject.GetComponents<Behaviour>())
				{
					if (text == behaviour.GetType().ToString())
					{
						targetBehaviour = behaviour;
						break;
					}
				}
				if (targetBehaviour == null)
				{
					throw new Exception("Unable to find target behaviour: key " + key + " typename " + text);
				}
			}
		}

		// Token: 0x0400019A RID: 410
		public string HandlerKey;

		// Token: 0x0400019B RID: 411
		public bool IsMethodWithParam;

		// Token: 0x0400019C RID: 412
		public bool InvokeEventsInEditMode;

		// Token: 0x0400019D RID: 413
		public GameObject TargetObject;

		// Token: 0x0400019E RID: 414
		public string ArgValue;

		// Token: 0x0400019F RID: 415
		private EventInvocationInfo invocationInfo;
	}
}

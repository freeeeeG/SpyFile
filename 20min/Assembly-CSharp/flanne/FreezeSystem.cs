using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000099 RID: 153
	public class FreezeSystem : MonoBehaviour
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x0001A375 File Offset: 0x00018575
		private void Awake()
		{
			FreezeSystem.SharedInstance = this;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001A380 File Offset: 0x00018580
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.freezeFXPrefab.name, this.freezeFXPrefab, 100, true);
			this.OP.AddObject(this.freezeFXLargePrefab.name, this.freezeFXLargePrefab, 100, true);
			this._currentTargets = new List<FreezeSystem.FreezeTarget>();
			this.durationMod = new StatMod();
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001A3EC File Offset: 0x000185EC
		public bool IsFrozen(GameObject target)
		{
			return this._currentTargets.Find((FreezeSystem.FreezeTarget bt) => bt.target == target) != null;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001A420 File Offset: 0x00018620
		public void Freeze(GameObject target)
		{
			if (!target.GetComponent<AIComponent>())
			{
				return;
			}
			FreezeSystem.FreezeTarget freezeTarget = this._currentTargets.Find((FreezeSystem.FreezeTarget bt) => bt.target == target);
			float num;
			if (target.tag.Contains("Champion"))
			{
				num = this.freezeDuration / 10f;
			}
			else
			{
				num = this.freezeDuration;
			}
			if (freezeTarget == null)
			{
				freezeTarget = new FreezeSystem.FreezeTarget(target, this.durationMod.Modify(num));
				base.StartCoroutine(this.StartFreezeCR(freezeTarget));
			}
			else if (num > freezeTarget.duration)
			{
				freezeTarget.duration = this.durationMod.Modify(num);
			}
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001A4EA File Offset: 0x000186EA
		private IEnumerator StartFreezeCR(FreezeSystem.FreezeTarget freezeTarget)
		{
			this.PostNotification(FreezeSystem.InflictFreezeEvent, freezeTarget.target);
			AIComponent component = freezeTarget.target.GetComponent<AIComponent>();
			if (component != null)
			{
				component.frozen = true;
			}
			Animator animator = freezeTarget.target.GetComponent<Animator>();
			if (animator != null)
			{
				animator.speed = 0f;
			}
			this._currentTargets.Add(freezeTarget);
			string name;
			if (freezeTarget.target.tag.Contains("Champion"))
			{
				name = this.freezeFXLargePrefab.name;
			}
			else
			{
				name = this.freezeFXPrefab.name;
			}
			GameObject freezeObj = this.OP.GetPooledObject(name);
			freezeObj.transform.SetParent(freezeTarget.target.transform);
			freezeObj.transform.localPosition = Vector3.zero;
			freezeObj.SetActive(true);
			while (freezeTarget.duration > 0f && freezeTarget.target.activeInHierarchy)
			{
				yield return null;
				freezeTarget.duration -= Time.deltaTime;
			}
			freezeTarget.target.GetComponent<AIComponent>().frozen = false;
			if (animator != null)
			{
				animator.speed = 1f;
			}
			this._currentTargets.Remove(freezeTarget);
			freezeObj.transform.SetParent(this.OP.transform);
			freezeObj.SetActive(false);
			yield break;
		}

		// Token: 0x04000360 RID: 864
		public static FreezeSystem SharedInstance;

		// Token: 0x04000361 RID: 865
		public static string InflictFreezeEvent = "FreezeSystem.InflictFreezeEvent";

		// Token: 0x04000362 RID: 866
		public StatMod durationMod;

		// Token: 0x04000363 RID: 867
		[SerializeField]
		private float freezeDuration = 1.5f;

		// Token: 0x04000364 RID: 868
		[SerializeField]
		private GameObject freezeFXPrefab;

		// Token: 0x04000365 RID: 869
		[SerializeField]
		private GameObject freezeFXLargePrefab;

		// Token: 0x04000366 RID: 870
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000367 RID: 871
		private ObjectPooler OP;

		// Token: 0x04000368 RID: 872
		private List<FreezeSystem.FreezeTarget> _currentTargets;

		// Token: 0x020002A6 RID: 678
		private class FreezeTarget
		{
			// Token: 0x06000E16 RID: 3606 RVA: 0x00033631 File Offset: 0x00031831
			public FreezeTarget(GameObject t, float d)
			{
				this.target = t;
				this.duration = d;
			}

			// Token: 0x04000A83 RID: 2691
			public GameObject target;

			// Token: 0x04000A84 RID: 2692
			public float duration;
		}
	}
}

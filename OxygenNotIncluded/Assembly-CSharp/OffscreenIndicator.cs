using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008C1 RID: 2241
[AddComponentMenu("KMonoBehaviour/scripts/OffscreenIndicator")]
public class OffscreenIndicator : KMonoBehaviour
{
	// Token: 0x060040F5 RID: 16629 RVA: 0x0016AB87 File Offset: 0x00168D87
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OffscreenIndicator.Instance = this;
	}

	// Token: 0x060040F6 RID: 16630 RVA: 0x0016AB95 File Offset: 0x00168D95
	protected override void OnForcedCleanUp()
	{
		OffscreenIndicator.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060040F7 RID: 16631 RVA: 0x0016ABA4 File Offset: 0x00168DA4
	private void Update()
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in this.targets)
		{
			this.UpdateArrow(keyValuePair.Value, keyValuePair.Key);
		}
	}

	// Token: 0x060040F8 RID: 16632 RVA: 0x0016AC04 File Offset: 0x00168E04
	public void ActivateIndicator(GameObject target)
	{
		if (!this.targets.ContainsKey(target))
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(target, "ui", false);
			if (uisprite != null)
			{
				this.ActivateIndicator(target, uisprite);
			}
		}
	}

	// Token: 0x060040F9 RID: 16633 RVA: 0x0016AC38 File Offset: 0x00168E38
	public void ActivateIndicator(GameObject target, GameObject iconSource)
	{
		if (!this.targets.ContainsKey(target))
		{
			MinionIdentity component = iconSource.GetComponent<MinionIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon").gameObject.SetActive(false);
				CrewPortrait reference = gameObject.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait");
				reference.gameObject.SetActive(true);
				reference.SetIdentityObject(component, true);
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x060040FA RID: 16634 RVA: 0x0016ACC4 File Offset: 0x00168EC4
	public void ActivateIndicator(GameObject target, global::Tuple<Sprite, Color> icon)
	{
		if (!this.targets.ContainsKey(target))
		{
			GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
			Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon");
			if (icon != null)
			{
				reference.sprite = icon.first;
				reference.color = icon.second;
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x060040FB RID: 16635 RVA: 0x0016AD2B File Offset: 0x00168F2B
	public void DeactivateIndicator(GameObject target)
	{
		if (this.targets.ContainsKey(target))
		{
			UnityEngine.Object.Destroy(this.targets[target]);
			this.targets.Remove(target);
		}
	}

	// Token: 0x060040FC RID: 16636 RVA: 0x0016AD5C File Offset: 0x00168F5C
	private void UpdateArrow(GameObject arrow, GameObject target)
	{
		if (target == null)
		{
			UnityEngine.Object.Destroy(arrow);
			this.targets.Remove(target);
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(target.transform.position);
		if ((double)vector.x > 0.3 && (double)vector.x < 0.7 && (double)vector.y > 0.3 && (double)vector.y < 0.7)
		{
			arrow.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").SetIdentityObject(null, true);
			arrow.SetActive(false);
			return;
		}
		arrow.SetActive(true);
		arrow.rectTransform().SetLocalPosition(Vector3.zero);
		Vector3 b = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		b.z = target.transform.position.z;
		Vector3 normalized = (target.transform.position - b).normalized;
		arrow.transform.up = normalized;
		this.UpdateTargetIconPosition(target, arrow);
	}

	// Token: 0x060040FD RID: 16637 RVA: 0x0016AE80 File Offset: 0x00169080
	private void UpdateTargetIconPosition(GameObject goTarget, GameObject indicator)
	{
		Vector3 vector = goTarget.transform.position;
		vector = Camera.main.WorldToViewportPoint(vector);
		if (vector.z < 0f)
		{
			vector.x = 1f - vector.x;
			vector.y = 1f - vector.y;
			vector.z = 0f;
			vector = this.Vector3Maxamize(vector);
		}
		vector = Camera.main.ViewportToScreenPoint(vector);
		vector.x = Mathf.Clamp(vector.x, this.edgeInset, (float)Screen.width - this.edgeInset);
		vector.y = Mathf.Clamp(vector.y, this.edgeInset, (float)Screen.height - this.edgeInset);
		indicator.transform.position = vector;
		indicator.GetComponent<HierarchyReferences>().GetReference<Image>("icon").rectTransform.up = Vector3.up;
		indicator.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").transform.up = Vector3.up;
	}

	// Token: 0x060040FE RID: 16638 RVA: 0x0016AF8C File Offset: 0x0016918C
	public Vector3 Vector3Maxamize(Vector3 vector)
	{
		float num = 0f;
		num = ((vector.x > num) ? vector.x : num);
		num = ((vector.y > num) ? vector.y : num);
		num = ((vector.z > num) ? vector.z : num);
		return vector / num;
	}

	// Token: 0x04002A46 RID: 10822
	public GameObject IndicatorPrefab;

	// Token: 0x04002A47 RID: 10823
	public GameObject IndicatorContainer;

	// Token: 0x04002A48 RID: 10824
	private Dictionary<GameObject, GameObject> targets = new Dictionary<GameObject, GameObject>();

	// Token: 0x04002A49 RID: 10825
	public static OffscreenIndicator Instance;

	// Token: 0x04002A4A RID: 10826
	[SerializeField]
	private float edgeInset = 25f;
}

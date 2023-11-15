using System;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
[AddComponentMenu("KMonoBehaviour/scripts/HelmetController")]
public class HelmetController : KMonoBehaviour
{
	// Token: 0x0600365E RID: 13918 RVA: 0x001259EC File Offset: 0x00123BEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HelmetController>(-1617557748, HelmetController.OnEquippedDelegate);
		base.Subscribe<HelmetController>(-170173755, HelmetController.OnUnequippedDelegate);
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x00125A18 File Offset: 0x00123C18
	private KBatchedAnimController GetAssigneeController()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (component.assignee != null)
		{
			GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
			if (assigneeGameObject)
			{
				return assigneeGameObject.GetComponent<KBatchedAnimController>();
			}
		}
		return null;
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x00125A54 File Offset: 0x00123C54
	private GameObject GetAssigneeGameObject(IAssignableIdentity ass_id)
	{
		GameObject result = null;
		MinionAssignablesProxy minionAssignablesProxy = ass_id as MinionAssignablesProxy;
		if (minionAssignablesProxy)
		{
			result = minionAssignablesProxy.GetTargetGameObject();
		}
		else
		{
			MinionIdentity minionIdentity = ass_id as MinionIdentity;
			if (minionIdentity)
			{
				result = minionIdentity.gameObject;
			}
		}
		return result;
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x00125A94 File Offset: 0x00123C94
	private void OnEquipped(object data)
	{
		Equippable component = base.GetComponent<Equippable>();
		this.ShowHelmet();
		GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
		assigneeGameObject.Subscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
		assigneeGameObject.Subscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
		assigneeGameObject.Subscribe(1347184327, new Action<object>(this.OnPathAdvanced));
		this.in_tube = false;
		this.is_flying = false;
		this.owner_navigator = assigneeGameObject.GetComponent<Navigator>();
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00125B20 File Offset: 0x00123D20
	private void OnUnequipped(object data)
	{
		this.owner_navigator = null;
		Equippable component = base.GetComponent<Equippable>();
		if (component != null)
		{
			this.HideHelmet();
			if (component.assignee != null)
			{
				GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
				if (assigneeGameObject)
				{
					assigneeGameObject.Unsubscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
					assigneeGameObject.Unsubscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
					assigneeGameObject.Unsubscribe(1347184327, new Action<object>(this.OnPathAdvanced));
				}
			}
		}
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x00125BAC File Offset: 0x00123DAC
	private void ShowHelmet()
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = new KAnimHashedString("snapTo_neck");
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			KAnimFile anim = Assets.GetAnim(this.anim_file);
			assigneeController.GetComponent<SymbolOverrideController>().AddSymbolOverride(kanimHashedString, anim.GetData().build.GetSymbol(kanimHashedString), 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, true);
		this.is_shown = true;
		this.UpdateJets();
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x00125C30 File Offset: 0x00123E30
	private void HideHelmet()
	{
		this.is_shown = false;
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = "snapTo_neck";
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			SymbolOverrideController component = assigneeController.GetComponent<SymbolOverrideController>();
			if (component == null)
			{
				return;
			}
			component.RemoveSymbolOverride(kanimHashedString, 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, false);
		this.UpdateJets();
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x00125C9A File Offset: 0x00123E9A
	private void UpdateJets()
	{
		if (this.is_shown && this.is_flying)
		{
			this.EnableJets();
			return;
		}
		this.DisableJets();
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x00125CBC File Offset: 0x00123EBC
	private void EnableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		if (this.jet_go)
		{
			return;
		}
		this.jet_go = this.AddTrackedAnim("jet", Assets.GetAnim("jetsuit_thruster_fx_kanim"), "loop", Grid.SceneLayer.Creatures, "snapTo_neck");
		this.glow_go = this.AddTrackedAnim("glow", Assets.GetAnim("jetsuit_thruster_glow_fx_kanim"), "loop", Grid.SceneLayer.Front, "snapTo_neck");
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x00125D38 File Offset: 0x00123F38
	private void DisableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.jet_go);
		this.jet_go = null;
		UnityEngine.Object.Destroy(this.glow_go);
		this.glow_go = null;
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x00125D68 File Offset: 0x00123F68
	private GameObject AddTrackedAnim(string name, KAnimFile tracked_anim_file, string anim_clip, Grid.SceneLayer layer, string symbol_name)
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return null;
		}
		string name2 = assigneeController.name + "." + name;
		GameObject gameObject = new GameObject(name2);
		gameObject.SetActive(false);
		gameObject.transform.parent = assigneeController.transform;
		gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name2);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			tracked_anim_file
		};
		kbatchedAnimController.initialAnim = anim_clip;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.sceneLayer = layer;
		gameObject.AddComponent<KBatchedAnimTracker>().symbol = symbol_name;
		bool flag;
		Vector3 position = assigneeController.GetSymbolTransform(symbol_name, out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(layer);
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		kbatchedAnimController.Play(anim_clip, KAnim.PlayMode.Loop, 1f, 0f);
		return gameObject;
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00125E63 File Offset: 0x00124063
	private void OnBeginRecoverBreath(object data)
	{
		this.HideHelmet();
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00125E6B File Offset: 0x0012406B
	private void OnEndRecoverBreath(object data)
	{
		this.ShowHelmet();
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x00125E74 File Offset: 0x00124074
	private void OnPathAdvanced(object data)
	{
		if (this.owner_navigator == null)
		{
			return;
		}
		bool flag = this.owner_navigator.CurrentNavType == NavType.Hover;
		bool flag2 = this.owner_navigator.CurrentNavType == NavType.Tube;
		if (flag2 != this.in_tube)
		{
			this.in_tube = flag2;
			if (this.in_tube)
			{
				this.HideHelmet();
			}
			else
			{
				this.ShowHelmet();
			}
		}
		if (flag != this.is_flying)
		{
			this.is_flying = flag;
			this.UpdateJets();
		}
	}

	// Token: 0x0400212D RID: 8493
	public string anim_file;

	// Token: 0x0400212E RID: 8494
	public bool has_jets;

	// Token: 0x0400212F RID: 8495
	private bool is_shown;

	// Token: 0x04002130 RID: 8496
	private bool in_tube;

	// Token: 0x04002131 RID: 8497
	private bool is_flying;

	// Token: 0x04002132 RID: 8498
	private Navigator owner_navigator;

	// Token: 0x04002133 RID: 8499
	private GameObject jet_go;

	// Token: 0x04002134 RID: 8500
	private GameObject glow_go;

	// Token: 0x04002135 RID: 8501
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnEquippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04002136 RID: 8502
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnUnequipped(data);
	});
}

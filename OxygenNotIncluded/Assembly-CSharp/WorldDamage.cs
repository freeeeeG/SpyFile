using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;

// Token: 0x02000A34 RID: 2612
[AddComponentMenu("KMonoBehaviour/scripts/WorldDamage")]
public class WorldDamage : KMonoBehaviour
{
	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06004E8B RID: 20107 RVA: 0x001B8583 File Offset: 0x001B6783
	// (set) Token: 0x06004E8C RID: 20108 RVA: 0x001B858A File Offset: 0x001B678A
	public static WorldDamage Instance { get; private set; }

	// Token: 0x06004E8D RID: 20109 RVA: 0x001B8592 File Offset: 0x001B6792
	public static void DestroyInstance()
	{
		WorldDamage.Instance = null;
	}

	// Token: 0x06004E8E RID: 20110 RVA: 0x001B859A File Offset: 0x001B679A
	protected override void OnPrefabInit()
	{
		WorldDamage.Instance = this;
	}

	// Token: 0x06004E8F RID: 20111 RVA: 0x001B85A2 File Offset: 0x001B67A2
	public void RestoreDamageToValue(int cell, float amount)
	{
		if (Grid.Damage[cell] > amount)
		{
			Grid.Damage[cell] = amount;
		}
	}

	// Token: 0x06004E90 RID: 20112 RVA: 0x001B85B6 File Offset: 0x001B67B6
	public float ApplyDamage(Sim.WorldDamageInfo damage_info)
	{
		return this.ApplyDamage(damage_info.gameCell, this.damageAmount, damage_info.damageSourceOffset, BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE);
	}

	// Token: 0x06004E91 RID: 20113 RVA: 0x001B85E4 File Offset: 0x001B67E4
	public float ApplyDamage(int cell, float amount, int src_cell, string source_name = null, string pop_text = null)
	{
		float result = 0f;
		if (Grid.Solid[cell])
		{
			float num = Grid.Damage[cell];
			result = Mathf.Min(amount, 1f - num);
			num += amount;
			bool flag = num > 0.15f;
			if (flag)
			{
				GameObject gameObject = Grid.Objects[cell, 9];
				if (gameObject != null)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						if (!component.invincible)
						{
							int damage = Mathf.RoundToInt(Mathf.Max((float)component.HitPoints - (1f - num) * (float)component.MaxHitPoints, 0f));
							gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
							{
								damage = damage,
								source = source_name,
								popString = pop_text
							});
						}
						else
						{
							num = 0f;
						}
					}
				}
			}
			Grid.Damage[cell] = Mathf.Min(1f, num);
			if (Grid.Damage[cell] >= 1f)
			{
				this.DestroyCell(cell);
			}
			else if (Grid.IsValidCell(src_cell) && flag)
			{
				Element element = Grid.Element[src_cell];
				if (element.IsLiquid && Grid.Mass[src_cell] > 1f)
				{
					int num2 = cell - src_cell;
					if (num2 == 1 || num2 == -1 || num2 == Grid.WidthInCells || num2 == -Grid.WidthInCells)
					{
						int num3 = cell + num2;
						if (Grid.IsValidCell(num3))
						{
							Element element2 = Grid.Element[num3];
							if (!element2.IsSolid && (!element2.IsLiquid || (element2.id == element.id && Grid.Mass[num3] <= 100f)) && (Grid.Properties[num3] & 2) == 0 && !this.spawnTimes.ContainsKey(num3))
							{
								this.spawnTimes[num3] = Time.realtimeSinceStartup;
								ushort idx = element.idx;
								float temperature = Grid.Temperature[src_cell];
								base.StartCoroutine(this.DelayedSpawnFX(src_cell, num3, num2, element, idx, temperature));
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06004E92 RID: 20114 RVA: 0x001B880A File Offset: 0x001B6A0A
	private void ReleaseGO(GameObject go)
	{
		go.DeleteObject();
	}

	// Token: 0x06004E93 RID: 20115 RVA: 0x001B8812 File Offset: 0x001B6A12
	private IEnumerator DelayedSpawnFX(int src_cell, int dest_cell, int offset, Element elem, ushort idx, float temperature)
	{
		float seconds = UnityEngine.Random.value * 0.25f;
		yield return new WaitForSeconds(seconds);
		Vector3 position = Grid.CellToPosCCC(dest_cell, Grid.SceneLayer.Front);
		GameObject gameObject = GameUtil.KInstantiate(this.leakEffect.gameObject, position, Grid.SceneLayer.Front, null, 0);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.TintColour = elem.substance.colour;
		component.onDestroySelf = new Action<GameObject>(this.ReleaseGO);
		SimMessages.AddRemoveSubstance(src_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, -1f, temperature, byte.MaxValue, 0, true, -1);
		if (offset == -1)
		{
			component.Play("side", KAnim.PlayMode.Once, 1f, 0f);
			component.FlipX = true;
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.right * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		else if (offset == Grid.WidthInCells)
		{
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.up * 0.5f);
			component.Play("floor", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			SimMessages.AddRemoveSubstance(dest_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, 1f, temperature, byte.MaxValue, 0, true, -1);
		}
		else if (offset == -Grid.WidthInCells)
		{
			component.Play("ceiling", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.up * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		else
		{
			component.Play("side", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.right * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		if (CameraController.Instance.IsAudibleSound(gameObject.transform.GetPosition(), this.leakSoundMigrated))
		{
			SoundEvent.PlayOneShot(this.leakSoundMigrated, gameObject.transform.GetPosition(), 1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06004E94 RID: 20116 RVA: 0x001B8850 File Offset: 0x001B6A50
	private void Update()
	{
		this.expiredCells.Clear();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		foreach (KeyValuePair<int, float> keyValuePair in this.spawnTimes)
		{
			if (realtimeSinceStartup - keyValuePair.Value > 1f)
			{
				this.expiredCells.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this.expiredCells)
		{
			this.spawnTimes.Remove(key);
		}
		this.expiredCells.Clear();
	}

	// Token: 0x06004E95 RID: 20117 RVA: 0x001B8924 File Offset: 0x001B6B24
	public void DestroyCell(int cell)
	{
		if (Grid.Solid[cell])
		{
			SimMessages.Dig(cell, -1, false);
		}
	}

	// Token: 0x06004E96 RID: 20118 RVA: 0x001B893B File Offset: 0x001B6B3B
	public void OnSolidStateChanged(int cell)
	{
		Grid.Damage[cell] = 0f;
	}

	// Token: 0x06004E97 RID: 20119 RVA: 0x001B894C File Offset: 0x001B6B4C
	public void OnDigComplete(int cell, float mass, float temperature, ushort element_idx, byte disease_idx, int disease_count)
	{
		Vector3 vector = Grid.CellToPos(cell, CellAlignment.RandomInternal, Grid.SceneLayer.Ore);
		Element element = ElementLoader.elements[(int)element_idx];
		Grid.Damage[cell] = 0f;
		WorldDamage.Instance.PlaySoundForSubstance(element, vector);
		float num = mass * 0.5f;
		if (num <= 0f)
		{
			return;
		}
		GameObject gameObject = element.substance.SpawnResource(vector, num, temperature, disease_idx, disease_count, false, false, false);
		Pickupable component = gameObject.GetComponent<Pickupable>();
		if (component != null && component.GetMyWorld() != null && component.GetMyWorld().worldInventory.IsReachable(component))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, Mathf.RoundToInt(num).ToString() + " " + element.name, gameObject.transform, 1.5f, false);
		}
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x001B8A28 File Offset: 0x001B6C28
	private void PlaySoundForSubstance(Element element, Vector3 pos)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			if (element.HasTag(GameTags.RefinedMetal))
			{
				text = "RefinedMetal";
			}
			else if (element.HasTag(GameTags.Metal))
			{
				text = "RawMetal";
			}
			else
			{
				text = "Rock";
			}
		}
		text = "Break_" + text;
		text = GlobalAssets.GetSound(text, false);
		if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos, false), 1f);
		}
	}

	// Token: 0x04003321 RID: 13089
	public KBatchedAnimController leakEffect;

	// Token: 0x04003322 RID: 13090
	[SerializeField]
	private FMODAsset leakSound;

	// Token: 0x04003323 RID: 13091
	[SerializeField]
	private EventReference leakSoundMigrated;

	// Token: 0x04003324 RID: 13092
	private float damageAmount = 0.00083333335f;

	// Token: 0x04003326 RID: 13094
	private const float SPAWN_DELAY = 1f;

	// Token: 0x04003327 RID: 13095
	private Dictionary<int, float> spawnTimes = new Dictionary<int, float>();

	// Token: 0x04003328 RID: 13096
	private List<int> expiredCells = new List<int>();
}

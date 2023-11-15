using System;
using System.Collections.Generic;
using ImGuiNET;
using STRINGS;

// Token: 0x02000574 RID: 1396
public class DevTool_StoryTrait_CritterManipulator : DevTool
{
	// Token: 0x060021F9 RID: 8697 RVA: 0x000BA7AC File Offset: 0x000B89AC
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Debug species lore unlock popup", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.Button_OpenSpecies(Tag.Invalid, "Unknown Species");
			ImGui.Separator();
			foreach (Tag species in this.GetCritterSpeciesTags())
			{
				this.Button_OpenSpecies(species, GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species).Unwrap());
			}
		}
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000BA82C File Offset: 0x000B8A2C
	public void Button_OpenSpecies(Tag species, string speciesName = null)
	{
		if (speciesName == null)
		{
			speciesName = species.Name;
		}
		else
		{
			speciesName = string.Format("\"{0}\" (ID: {1})", UI.StripLinkFormatting(speciesName), species);
		}
		if (ImGui.Button("Show popup for: " + speciesName))
		{
			GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(species);
		}
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000BA86C File Offset: 0x000B8A6C
	public IEnumerable<Tag> GetCritterSpeciesTags()
	{
		yield return GameTags.Creatures.Species.HatchSpecies;
		yield return GameTags.Creatures.Species.LightBugSpecies;
		yield return GameTags.Creatures.Species.OilFloaterSpecies;
		yield return GameTags.Creatures.Species.DreckoSpecies;
		yield return GameTags.Creatures.Species.GlomSpecies;
		yield return GameTags.Creatures.Species.PuftSpecies;
		yield return GameTags.Creatures.Species.PacuSpecies;
		yield return GameTags.Creatures.Species.MooSpecies;
		yield return GameTags.Creatures.Species.MoleSpecies;
		yield return GameTags.Creatures.Species.SquirrelSpecies;
		yield return GameTags.Creatures.Species.CrabSpecies;
		yield return GameTags.Creatures.Species.DivergentSpecies;
		yield return GameTags.Creatures.Species.StaterpillarSpecies;
		yield return GameTags.Creatures.Species.BeetaSpecies;
		yield break;
	}
}

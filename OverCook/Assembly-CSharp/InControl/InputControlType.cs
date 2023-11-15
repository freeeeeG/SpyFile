using System;

namespace InControl
{
	// Token: 0x020002B2 RID: 690
	public enum InputControlType
	{
		// Token: 0x04000A39 RID: 2617
		None,
		// Token: 0x04000A3A RID: 2618
		LeftStickUp,
		// Token: 0x04000A3B RID: 2619
		LeftStickDown,
		// Token: 0x04000A3C RID: 2620
		LeftStickLeft,
		// Token: 0x04000A3D RID: 2621
		LeftStickRight,
		// Token: 0x04000A3E RID: 2622
		LeftStickButton,
		// Token: 0x04000A3F RID: 2623
		RightStickUp,
		// Token: 0x04000A40 RID: 2624
		RightStickDown,
		// Token: 0x04000A41 RID: 2625
		RightStickLeft,
		// Token: 0x04000A42 RID: 2626
		RightStickRight,
		// Token: 0x04000A43 RID: 2627
		RightStickButton,
		// Token: 0x04000A44 RID: 2628
		DPadUp,
		// Token: 0x04000A45 RID: 2629
		DPadDown,
		// Token: 0x04000A46 RID: 2630
		DPadLeft,
		// Token: 0x04000A47 RID: 2631
		DPadRight,
		// Token: 0x04000A48 RID: 2632
		Action1,
		// Token: 0x04000A49 RID: 2633
		Action2,
		// Token: 0x04000A4A RID: 2634
		Action3,
		// Token: 0x04000A4B RID: 2635
		Action4,
		// Token: 0x04000A4C RID: 2636
		LeftTrigger,
		// Token: 0x04000A4D RID: 2637
		RightTrigger,
		// Token: 0x04000A4E RID: 2638
		LeftBumper,
		// Token: 0x04000A4F RID: 2639
		RightBumper,
		// Token: 0x04000A50 RID: 2640
		Command,
		// Token: 0x04000A51 RID: 2641
		Back,
		// Token: 0x04000A52 RID: 2642
		Start,
		// Token: 0x04000A53 RID: 2643
		Select,
		// Token: 0x04000A54 RID: 2644
		System,
		// Token: 0x04000A55 RID: 2645
		Options,
		// Token: 0x04000A56 RID: 2646
		Pause,
		// Token: 0x04000A57 RID: 2647
		Menu,
		// Token: 0x04000A58 RID: 2648
		Share,
		// Token: 0x04000A59 RID: 2649
		Home,
		// Token: 0x04000A5A RID: 2650
		View,
		// Token: 0x04000A5B RID: 2651
		Power,
		// Token: 0x04000A5C RID: 2652
		TiltX,
		// Token: 0x04000A5D RID: 2653
		TiltY,
		// Token: 0x04000A5E RID: 2654
		TiltZ,
		// Token: 0x04000A5F RID: 2655
		ScrollWheel,
		// Token: 0x04000A60 RID: 2656
		TouchPadTap,
		// Token: 0x04000A61 RID: 2657
		TouchPadXAxis,
		// Token: 0x04000A62 RID: 2658
		TouchPadYAxis,
		// Token: 0x04000A63 RID: 2659
		Analog0,
		// Token: 0x04000A64 RID: 2660
		Analog1,
		// Token: 0x04000A65 RID: 2661
		Analog2,
		// Token: 0x04000A66 RID: 2662
		Analog3,
		// Token: 0x04000A67 RID: 2663
		Analog4,
		// Token: 0x04000A68 RID: 2664
		Analog5,
		// Token: 0x04000A69 RID: 2665
		Analog6,
		// Token: 0x04000A6A RID: 2666
		Analog7,
		// Token: 0x04000A6B RID: 2667
		Analog8,
		// Token: 0x04000A6C RID: 2668
		Analog9,
		// Token: 0x04000A6D RID: 2669
		Analog10,
		// Token: 0x04000A6E RID: 2670
		Analog11,
		// Token: 0x04000A6F RID: 2671
		Analog12,
		// Token: 0x04000A70 RID: 2672
		Analog13,
		// Token: 0x04000A71 RID: 2673
		Analog14,
		// Token: 0x04000A72 RID: 2674
		Analog15,
		// Token: 0x04000A73 RID: 2675
		Analog16,
		// Token: 0x04000A74 RID: 2676
		Analog17,
		// Token: 0x04000A75 RID: 2677
		Analog18,
		// Token: 0x04000A76 RID: 2678
		Analog19,
		// Token: 0x04000A77 RID: 2679
		Button0,
		// Token: 0x04000A78 RID: 2680
		Button1,
		// Token: 0x04000A79 RID: 2681
		Button2,
		// Token: 0x04000A7A RID: 2682
		Button3,
		// Token: 0x04000A7B RID: 2683
		Button4,
		// Token: 0x04000A7C RID: 2684
		Button5,
		// Token: 0x04000A7D RID: 2685
		Button6,
		// Token: 0x04000A7E RID: 2686
		Button7,
		// Token: 0x04000A7F RID: 2687
		Button8,
		// Token: 0x04000A80 RID: 2688
		Button9,
		// Token: 0x04000A81 RID: 2689
		Button10,
		// Token: 0x04000A82 RID: 2690
		Button11,
		// Token: 0x04000A83 RID: 2691
		Button12,
		// Token: 0x04000A84 RID: 2692
		Button13,
		// Token: 0x04000A85 RID: 2693
		Button14,
		// Token: 0x04000A86 RID: 2694
		Button15,
		// Token: 0x04000A87 RID: 2695
		Button16,
		// Token: 0x04000A88 RID: 2696
		Button17,
		// Token: 0x04000A89 RID: 2697
		Button18,
		// Token: 0x04000A8A RID: 2698
		Button19,
		// Token: 0x04000A8B RID: 2699
		Count
	}
}

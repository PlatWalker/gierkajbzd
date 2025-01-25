using System;
using Unity.Behavior;

[BlackboardEnum]
public enum KonkubentState
{
    Idle,
	Chase,
	SimpleAttack,
	SpecialAttack,
	HitByPlayer
}
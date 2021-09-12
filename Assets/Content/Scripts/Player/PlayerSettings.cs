using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Walk")]
    public float MovementSpeed;
    public float MaxWalkingSlope;
    public bool VariableSpeedInput = true;

    [Header("Jump")]
    public float JumpHeight;
    public float SpeedInJump = 105f;

    [Header("Crouch")]
    public float CrouchHeight;
    public float crouchSpeed;

    [Header("Camera")]
    public float MaxViewingAngle;

    [Header("Health")]
    public float MaxHealth;
    public float HealthRechargeRate;

    [Header("Stamina")]
    public float MaxStamina;
    public float StaminaFallRate;
    public float StaminaRegenRate;

    [Header("Hunger")]
    public float maxHunger;
    public float hungerFallRate;

    [Header("Thirst")]
    public float maxThirst;
    public float thirstFallRate;

    [Header("Misc.")]
    public float ItemPickUpRange;
}

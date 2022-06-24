using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    private CharacterController2D CharacterController;
    private ExtraFunctions ExtraFunctions;

    private string Player = "Player";

    [HideInInspector] public int Health;
    [HideInInspector] public int HealthMax = 3;
    [HideInInspector] public int Stamina = 0;
    [HideInInspector] public int StaminaMax = 360;

    private int JumpDrain = 90;

    private bool CanRegen;

    void Start()
    {
        CharacterController = GameObject.Find(Player).GetComponent<CharacterController2D>();
        ExtraFunctions = GameObject.Find("ExtraFunctions").GetComponent<ExtraFunctions>();

        Health = HealthMax;
        Stamina = StaminaMax;
    }
    
    void FixedUpdate()
    {
        if (Health == 0 || Time.timeScale == 0f) return;

        if (Stamina < StaminaMax)
            CanRegen = true;
        else
            CanRegen = false;

        CharacterController.CanUse = Stamina > 0;
        CharacterController.CanJump = Stamina > 90;

        if (Input.GetKey(CharacterController.SprintKey))
        {
            if (CharacterController.CanUse)
                StartCoroutine(DrainStamina());
            CanRegen = false;
        }

        if (CanRegen && CharacterController.IsGrounded)
            StartCoroutine(RegenStamina());
    }
    
    void Update()
    {
        if (Health == 0 || Time.timeScale == 0f) return;

        if (Input.GetKeyDown(CharacterController.JumpKey) && CharacterController.CanJump)
        {
            if (CharacterController.IsGrounded)
            {
                DrainJumpStamina();
            }
            CanRegen = false;
        }
    }

    IEnumerator DrainStamina()
    {
        yield return new WaitForSecondsRealtime(StaminaMax / 360 / 6);
        Stamina--;
    }

    void DrainJumpStamina()
    {
        Stamina -= JumpDrain;
    }

    IEnumerator RegenStamina()
    {
        yield return new WaitForSecondsRealtime(StaminaMax / 360 / 10);
        Stamina++;
    }
}

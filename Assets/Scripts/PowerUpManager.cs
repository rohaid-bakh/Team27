using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Power Ups")]
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] float timeBetweenPowerUps = 15f;
    [SerializeField] float powerUpDuration = 15f;

    [Header("Power Up Abilities")]
    [SerializeField] float speedBoost = 2;
    [SerializeField] int healthBoost = 5;
    [SerializeField] int damageBoost = 2;

    [Header("Player")]
    PlayerCharacterController playerCharacter;
    PlayerAttack1 playerAttack;

    bool isPowerUpInPlay = false;

    System.Random random;

    

    void Start()
    {
        random = new System.Random();

        StartCoroutine(WaitAndStartPowerUpSpawner());
    }

    #region Spawner
    IEnumerator WaitAndStartPowerUpSpawner()
    {
        yield return new WaitUntil(() => FindObjectOfType<PlayerCharacterController>() != null);

        playerCharacter = FindObjectOfType<PlayerCharacterController>();
        playerAttack = FindObjectOfType<PlayerAttack1>();

        yield return StartCoroutine(SpawnPowerUps());
    }

    /// <summary>
    /// Loops through each power up in list, and spawns power up. Wait x time betweenpower ups.
    /// </summary>
    /// <returns>Nothing. IEnumerator used for "WaitForSeconds"</returns>
    IEnumerator SpawnPowerUps()
    {

        yield return new WaitForSeconds(timeBetweenPowerUps / 2);
        do
        {
            // wait until there are no power ups active, and when player is controllable and game is not paused
            yield return new WaitUntil(() => isPowerUpInPlay == false && CanControlPlayer() == true);

            float waitTime = timeBetweenPowerUps;

            // wait to spawn a new power up
            yield return new WaitForSeconds(waitTime);

            // choose random power up
            int powerUpIndex = random.Next(powerUps.Count);
            GameObject power = powerUps[powerUpIndex];

            // instantiate power up
            Instantiate(power,  // what object to instantiate
                transform.position, // where to spawn the object
                Quaternion.identity); // need to specify rotation

            isPowerUpInPlay = true;

        } while (true);
    }
    #endregion

    #region PowerUps

    public void ActivatePowerUp(string powerUpTag)
    {
        if(powerUpTag == EnumPowerUpTag.Health.ToString())
                StartCoroutine(HealthPowerUp());
        else if(powerUpTag == EnumPowerUpTag.Speed.ToString())
                StartCoroutine(SpeedPowerUp());
        else if (powerUpTag == EnumPowerUpTag.Strength.ToString())
                StartCoroutine(StrengthPowerUp());
    }

    // health
    private IEnumerator HealthPowerUp()
    {
        playerCharacter.characterHealth.AddHealth(healthBoost);
        
        // power up in play
        yield return new WaitForSeconds(powerUpDuration);

        isPowerUpInPlay = false;
    }

    // speed
    private IEnumerator SpeedPowerUp()
    {
        // update speed
        float originalMoveSpeed = playerCharacter.MoveSpeed;
        playerCharacter.MoveSpeed += speedBoost;

        Debug.Log($"Original move speed {originalMoveSpeed}; Current move speed {playerCharacter.MoveSpeed}");

        // power up in play
        yield return new WaitForSeconds(powerUpDuration);

        // revert speed back
        playerCharacter.MoveSpeed = originalMoveSpeed;

        isPowerUpInPlay = false;
    }

    // damage amount
    private IEnumerator StrengthPowerUp()
    {
        // update player damage
        int originalDamage = playerAttack.DamageAmount;
        playerAttack.DamageAmount += damageBoost;

        Debug.Log($"Original damage {originalDamage}; Current damage {playerAttack.DamageAmount}");

        // power up in play
        yield return new WaitForSeconds(powerUpDuration);

        // revert damage back
        playerAttack.DamageAmount = originalDamage;

        isPowerUpInPlay = false;
    }

    #endregion

    #region Helper Functions

    public bool CanControlPlayer()
    {
        if(playerCharacter?.isActiveAndEnabled == true)
        {
            if(playerCharacter?.CanControlPlayer == true)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
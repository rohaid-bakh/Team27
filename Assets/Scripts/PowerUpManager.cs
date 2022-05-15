using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Power Ups")]
    [SerializeField] List<GameObject> powerUps;
    [SerializeField] int maxTimeBetweenPowerUps = 15;
    [SerializeField] int spawnTimeSecondsVariance = 5;
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

    List<Transform> spawnPoints;

    void Start()
    {
        random = new System.Random();

        // get spawn points (children transforms)
        spawnPoints = GetComponentsInChildren<Transform>().ToList();

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
        do
        {
            // wait until there are no power ups active, and when player is controllable and game is not paused
            yield return new WaitUntil(() => isPowerUpInPlay == false && CanControlPlayer() == true);

            // create wait time, add some variance between spawn times 
            float waitTime = random.Next(maxTimeBetweenPowerUps - spawnTimeSecondsVariance, maxTimeBetweenPowerUps);

            // wait to spawn a new power up
            yield return new WaitForSeconds(waitTime);

            SpawnPowerUp();

        } while (true);
    }
    
    void SpawnPowerUp()
    {
        isPowerUpInPlay = true;

        // pick a random spawn point
        Transform spawnPoint = spawnPoints[random.Next(spawnPoints.Count)];

        // choose a random power up 
        int powerUpIndex = random.Next(powerUps.Count);

        // if player health is full, don't choose health power up [health power up is index 0]
        if (playerCharacter.characterHealth.GetCurrentHealth() == playerCharacter.characterHealth.stat.health && powerUpIndex == 0)
        {
            // random.next(min, max) returns an int greater than or equal to min, and less than max
            powerUpIndex += random.Next(1, 3);
        }
        GameObject power = powerUps[powerUpIndex];

        // sound effect
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.PowerUpAppear);

        // instantiate power up
        Instantiate(power,  // what object to instantiate
            spawnPoint.position, // where to spawn the object
            Quaternion.identity); // need to specify rotation
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
        
        // power up in play (for health it's instant)
        yield return new WaitForSeconds(0f);

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

        // debuff sound effect
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.PowerUpDebuff);

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

        // debuff sound effect
        AudioManager.instance?.PlaySoundEffect(EnumSoundName.PowerUpDebuff);

        // revert damage back
        playerAttack.DamageAmount = originalDamage;

        isPowerUpInPlay = false;
    }

    #endregion

    #region Helper Functions
    bool CanControlPlayer()
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
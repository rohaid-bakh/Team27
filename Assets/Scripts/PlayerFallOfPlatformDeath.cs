using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallOfPlatformDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(KillPlayer(other));
        }
    }

    IEnumerator KillPlayer(Collider other)
    {
        PlayerCharacterController player = other.GetComponent<PlayerCharacterController>();
        player.TakeDamage(9999999); // kill the player

        yield return new WaitForSeconds(1f);

        // stop any sound effects playing
        AudioManager.instance.StopPlayingAllSoundEffects();
    }
}

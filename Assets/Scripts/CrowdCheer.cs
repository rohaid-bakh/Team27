using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrowdCheer : MonoBehaviour
{
    List<Animator> spectatorAnimators;
    List<SpriteRenderer> spectatorSpriteRenderers;
    System.Random random;

    [SerializeField] List<Color> colors;

    // Use this for initialization
    void Start()
    {
        random = new System.Random();
        spectatorAnimators = FindObjectsOfType<Animator>().Where(x => x.gameObject.tag == "Spectator").ToList();
        spectatorSpriteRenderers = FindObjectsOfType<SpriteRenderer>().Where(x => x.gameObject.tag == "Spectator").ToList();
        Debug.Log(spectatorAnimators?.Count);

        if(spectatorAnimators?.Count > 0)
        {
            //AssignRandomColorsToSprites();
            Shuffle();
        }
    }

    public void CrowdCheering()
    {
        StartCoroutine(StartCrowdCheer());
    }

    void Shuffle()
    {
        int n = spectatorAnimators.Count();
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Animator value = spectatorAnimators[k];
            spectatorAnimators[k] = spectatorAnimators[n];
            spectatorAnimators[n] = value;
        }
    }

    void AssignRandomColorsToSprites()
    {
        foreach(SpriteRenderer spriteRend in spectatorSpriteRenderers)
        {
            // pick a random color
            // HSV is hue, saturation, and value. randomly generating the number for the color in the hue value. saturation can be between 0,40 and value is 100
            //float hue = Random.value;
            //float saturation = random.Next(0, 40) * 0.01f;
            //float value = 1;

            //Color newColor = Color.HSVToRGB(hue, saturation, value);

            int colorIndex = random.Next(colors.Count());
            Color newColor = colors[colorIndex];
            //Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);


            // apply to sprite render
            spriteRend.color = newColor;
        }
    }

    IEnumerator StartCrowdCheer()
    {
        foreach(Animator animator in spectatorAnimators)
        {
            animator.Play("Cheer");

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        foreach (Animator animator in spectatorAnimators)
        {
            animator.Play("Idle");

            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
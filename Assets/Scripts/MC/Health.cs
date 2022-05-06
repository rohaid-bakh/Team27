using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    private int hpbar;
    public Stats stat;
    [SerializeField]
    private Slider healthbar;
    private SpriteRenderer renderer;
    private Material defaultMat;
    void Awake()
    {
        hpbar = stat.health;
        healthbar.normalizedValue = 1f;
        renderer = GetComponentInChildren<SpriteRenderer>(); // might need to reconsider this if there's more sprite children
        defaultMat = renderer.material;
    }
    // returns true if character dies after the damge, false otherwise
    public bool TakeDamage(int damage)
    { // other classes call this

        hpbar -= damage;
        updateUIbar();
        StartCoroutine(HitFlash());

        CameraShake.Trauma = .3f; // Set the amount of camera shake 

        if (hpbar <= 0)
        {
            Death();
            return true;
        }

        return false;
    }

    private void updateUIbar(){
        healthbar.normalizedValue = 1f - ((float)(stat.health - hpbar) / (float)stat.health);
    }
    private void Death()
    {
        // place holder until we have a death script/idea.
        Debug.Log("This character has died.");
    }
    private IEnumerator HitFlash()
    {
        renderer.material = stat.flash; //switches material to white
        yield return new WaitForSeconds(.2f); // can make the length of the flash a variable
        renderer.material = defaultMat;

    }
}

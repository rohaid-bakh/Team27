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

    public int GetCurrentHealth()
    {
        return hpbar;
    }

    // returns true if character dies after the damge, false otherwise
    public bool TakeDamage(int damage)
    { // other classes call this

        hpbar -= damage;
        updateUIbar();
        StartCoroutine(HitFlash());

        CameraShake.Instance.Play(); // Set the camera shake 

        if (hpbar <= 0)
        {
            Death();
            return true;
        }

        return false;
    }

    public void AddHealth(int health)
    {
        hpbar += health;
        if (hpbar > stat.health)
            hpbar = stat.health;

        updateUIbar();
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

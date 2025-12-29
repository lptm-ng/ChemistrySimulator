using NUnit.Framework;
using UnityEngine;

public class FumeHoodStation : MonoBehaviour
{
    public bool isPlayerInZone = false;
    public ParticleSystem burnerFlame; // Bunsenbrennerpartikel

    private void Start()
    {
        if(burnerFlame != null) burnerFlame.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if(burnerFlame != null) burnerFlame.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if(burnerFlame != null) burnerFlame.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            ResetFlame();
        }
    }

    public void ResetFlame()
    {
        // Flamme --> nichtleuchtende Flamme Blau
        if (burnerFlame == null) return;
        var main = burnerFlame.main;
        main.startColor = Color.blue;
    }
}

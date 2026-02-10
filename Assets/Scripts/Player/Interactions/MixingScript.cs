using System;
using UnityEngine;

namespace Player.Interactions
{
    public class MixingScript : MonoBehaviour
    {

        public static MixingScript Instance;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        public static event Action<GameObject, Color> OnMix;        
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void OnEnable()
        {
            OnMix += ApplyColorChange;
        }

        public void OnDisable()
        {
            OnMix -= ApplyColorChange;
        }
        
        public void TriggerMix(GameObject containerObj, Color newColor)
        {
            Debug.Log($"[MixingScript] TriggerMix aufgerufen! Objekt: {containerObj.name}, Farbe: {newColor}");
            if (OnMix == null)
            {
                Debug.LogError("[MixingScript] FEHLER: Niemand hört auf das Event 'OnMix'! (Event ist null)");
            }
    
            OnMix?.Invoke(containerObj, newColor);
        }

        private void ApplyColorChange(GameObject obj, Color color)
        {
            if (obj == null)
            {
                Debug.LogError("FEHLER: Kein Container-Objekt an MixingScript übergeben!");
                return;
            }
            
            Debug.Log($"Starte Färbung für Container: {obj.name} mit Farbe: {color}");
            
            bool foundAny = false;
            
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            
            foreach (Renderer currentRenderer in renderers)
            {
                if (currentRenderer.gameObject.CompareTag("chemical")) 
                {
                    Debug.Log($"---> TREFFER! Färbe {currentRenderer.gameObject.name} um.");
            
                    // WICHTIG: Shader Property Name prüfen (siehe Punkt 2 unten)
                    currentRenderer.material.color = color; 
            
                    // Falls du URP nutzt, brauchst du evtl. das hier:
                    if (currentRenderer.material.HasProperty(BaseColor))
                    {
                        currentRenderer.material.SetColor(BaseColor, color);
                    }

                    foundAny = true;
                }
            }
            if (!foundAny)
            {
                Debug.LogError($"FEHLER: Habe im Objekt '{obj.name}' und seinen Kindern KEINEN Renderer mit dem Tag 'chemical' gefunden!");
            }
            
        }
        
    }
}
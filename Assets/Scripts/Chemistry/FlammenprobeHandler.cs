using System.Collections.Generic;
using UnityEngine;

namespace Chemistry
{
    public class FlammenprobeHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _flame;
        [SerializeField] private ParticleSystem _flameParticleSystem;

        public void Flammenprobe(GameObject currentChemcial)
        {
            ChemicalContainer dataList = currentChemcial.GetComponent<ChemicalContainer>();
            List<ChemicalData> list = dataList.contents;
            string chemicalName = list[0].chemicalName.Trim();

            Debug.Log("Chemical Name ist: " + chemicalName);
            switch (chemicalName)
            {
                case "Kalium":
                {
                    Debug.Log("FLAMME GEFAERBT ROT");
                    var main = _flameParticleSystem.main;
                    main.startColor = Color.red;
                    _flameParticleSystem.Play();
                    break;
                }
                case "Lithium":
                {
                    Debug.Log("FLAMME GEFAERBT ROT");
                    var main = _flameParticleSystem.main;
                    main.startColor = Color.red;
                    _flameParticleSystem.Play();
                    break;
                }
                default: break;
                //do other cases
            }
        }
    }
}
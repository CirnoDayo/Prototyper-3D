using UnityEngine;

public class CannonSelection : MonoBehaviour
{
    public InstantiateCannon instantiateCannonScript;
    public void SelectLightCanon()
    {
        instantiateCannonScript.InstantiateCannonOfType("Sniper");
    }

    public void SelectHeavyCanon()
    {
        instantiateCannonScript.InstantiateCannonOfType("HeavyCannon");
    }
}
using UnityEngine;

public class CanonSelection : MonoBehaviour
{
    public InstantiateCanon instantiateCanonScript; // Reference to the InstantiateCanon script

    public void SelectLightCanon()
    {
        instantiateCanonScript.InstantiateCanonOfType("LightCanon");
    }

    public void SelectHeavyCanon()
    {
        instantiateCanonScript.InstantiateCanonOfType("HeavyCanon");
    }
}
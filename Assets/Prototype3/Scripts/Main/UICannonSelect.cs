using UnityEngine;

public class UICannonSelect : MonoBehaviour
{
    public CannonInstantiating instantiateCannonScript;

    public void SelectSniper()
    {
        instantiateCannonScript.InstantiateCannonOfType("Sniper");
    }

    public void SelectHeavyCannon()
    {
        instantiateCannonScript.InstantiateCannonOfType("HeavyCannon");
    }
}
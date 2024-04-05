using UnityEngine;

public class UICannonSelect : MonoBehaviour
{
    public CannonManager cannonManagerScript;

    public void SelectSniper()
    {
        cannonManagerScript.InstantiateCannonOfType("Sniper");
    }

    public void SelectHeavyCannon()
    {
        cannonManagerScript.InstantiateCannonOfType("HeavyCannon");
    }
}
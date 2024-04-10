using UnityEngine;

public class UICannonSelect : MonoBehaviour
{
    public CannonManager cannonManagerScript;

    public void SelectSniper()
    {
        cannonManagerScript.InstantiateCannonOfType("Lan_Sniper");
    }

    public void SelectHeavyCannon()
    {
        cannonManagerScript.InstantiateCannonOfType("Lan_HeavyCannon");
    }
}
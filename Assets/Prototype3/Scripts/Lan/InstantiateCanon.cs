using UnityEngine;
using UnityEngine.UI; 
public class InstantiateCanon : MonoBehaviour
{
    public GameObject lightCanonPrefab;
    public GameObject heavyCanonPrefab;
    public Material transparentMaterial;
    public Camera gameCamera;

    private GameObject currentCanonInstance;
    private bool isPlacingCanon = false;
    private string selectedCanonType = "";
    private float placementDistance = 10f; // Distance from the camera to instantiate objects
    
    
    public int lightCanonLimit = 2;
    public int heavyCanonLimit = 2;
    private int lightCanonCount = 0;
    private int heavyCanonCount = 0;

    public Button lightCanonButton; // Assign in Unity Editor
    public Button heavyCanonButton; // Assign in Unity Editor
    public Text lightCanonText; // Assign in Unity Editor
    public Text heavyCanonText; // Assign in Unity Editor

    void Update()
    {
        if (isPlacingCanon && currentCanonInstance != null)
        {
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, placementDistance);
            Vector3 worldPosition = gameCamera.ScreenToWorldPoint(mouseScreenPosition);
            currentCanonInstance.transform.position = worldPosition;

            // Place the canon on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                InstantiateAtPosition(worldPosition);
                // Stop placing if Shift isn't held down
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                {
                    isPlacingCanon = false;
                    Destroy(currentCanonInstance);
                }
            }

            // Destroy the transparent object if Shift is released
            if (isPlacingCanon && (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
            {
                isPlacingCanon = false;
                Destroy(currentCanonInstance);
            }
        }
    }


    private void InstantiateAtPosition(Vector3 position)
    {
        // Instantiate the prefab at the given world position
        GameObject prefab = selectedCanonType == "LightCanon" ? lightCanonPrefab : heavyCanonPrefab;
        GameObject newInstance = Instantiate(prefab, position, Quaternion.identity);
        newInstance.GetComponent<Renderer>().material = GetOriginalMaterial(newInstance);

        // Update the count and UI for the appropriate canon type
        if (selectedCanonType == "LightCanon")
        {
            lightCanonCount++;
            lightCanonText.text = $"Light Canons Left: {lightCanonLimit - lightCanonCount}";
            if (lightCanonCount >= lightCanonLimit)
            {
                lightCanonButton.interactable = false;
                StopPlacingCanon();
            }
        }
        else if (selectedCanonType == "HeavyCanon")
        {
            heavyCanonCount++;
            heavyCanonText.text = $"Heavy Canons Left: {heavyCanonLimit - heavyCanonCount}";
            if (heavyCanonCount >= heavyCanonLimit)
            {
                heavyCanonButton.interactable = false;
                StopPlacingCanon();
            }
        }
    }

    private void StopPlacingCanon()
    {
        if (currentCanonInstance != null)
        {
            Destroy(currentCanonInstance);
        }

        isPlacingCanon = false;
    }

    
    // Call this method to reset canon limits (e.g., at the start of a new game or level)
    public void ResetCanonLimits()
    {
        lightCanonCount = 0;
        heavyCanonCount = 0;
        lightCanonText.text = $"Light Canons Left: {lightCanonLimit}";
        heavyCanonText.text = $"Heavy Canons Left: {heavyCanonLimit}";
        lightCanonButton.interactable = true;
        heavyCanonButton.interactable = true;
    }
    

    public void InstantiateCanonOfType(string canonType)
    {
        if (!isPlacingCanon)
        {
            selectedCanonType = canonType;
            GameObject prefab = canonType == "LightCanon" ? lightCanonPrefab : heavyCanonPrefab;
            currentCanonInstance = Instantiate(prefab);
            ApplyTransparentMaterial(currentCanonInstance);
            isPlacingCanon = true;
        }
    }

    private void ApplyTransparentMaterial(GameObject canon)
    {
        Renderer[] renderers = canon.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = transparentMaterial;
        }
    }

    private Material GetOriginalMaterial(GameObject canonInstance)
    {
        GameObject prefab = canonInstance.name.Contains("LightCanon") ? lightCanonPrefab : heavyCanonPrefab;
        return prefab.GetComponent<Renderer>().sharedMaterial;
    }
}

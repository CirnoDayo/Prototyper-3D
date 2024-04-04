using UnityEngine;

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

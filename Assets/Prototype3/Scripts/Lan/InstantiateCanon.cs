using UnityEngine;

public class InstantiateCanon : MonoBehaviour
{
    public GameObject lightCanonPrefab; // Assign in Unity Editor
    public GameObject heavyCanonPrefab; // Assign in Unity Editor
    public Material transparentMaterial; // Assign a transparent material in Unity Editor
    public Camera gameCamera; // Assign the main game camera in Unity Editor

    private GameObject currentCanonInstance;
    private bool isPlacingCanon = false;

    void Update()
    {
        if (isPlacingCanon && currentCanonInstance != null)
        {
            // Follow the mouse cursor
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Assuming the ground or the surface on which the canon is to be placed is at y = 0
                Vector3 positionToPlace = hit.point;
                currentCanonInstance.transform.position = new Vector3(positionToPlace.x, 0f, positionToPlace.z);
            }

            // Place the canon on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // Reset placing logic and apply the original material
                isPlacingCanon = false;
                currentCanonInstance.GetComponent<Renderer>().material = GetOriginalMaterial(currentCanonInstance);
            }
        }
    }

    public void InstantiateCanonOfType(string canonType)
    {
        if (!isPlacingCanon) // Prevent instantiating a new canon if one is already being placed
        {
            GameObject prefab = canonType == "LightCanon" ? lightCanonPrefab : heavyCanonPrefab;
            currentCanonInstance = Instantiate(prefab);
            ApplyTransparentMaterial(currentCanonInstance);
            isPlacingCanon = true; // Start the placing logic
        }
    }

    private void ApplyTransparentMaterial(GameObject canon)
    {
        Renderer[] renderers = canon.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material = transparentMaterial;
        }
    }

    private Material GetOriginalMaterial(GameObject canonInstance)
    {
        // This method returns the correct original material based on the instantiated prefab
        GameObject prefab = canonInstance.name.Contains("LightCanon") ? lightCanonPrefab : heavyCanonPrefab;
        return prefab.GetComponent<Renderer>().sharedMaterial;
    }
}

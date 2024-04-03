using UnityEngine;

public class InstantiateCanon : MonoBehaviour
{
    public GameObject lightCanonPrefab; // Assign in Unity Editor
    public GameObject heavyCanonPrefab; // Assign in Unity Editor
    public Material transparentMaterial; // Assign a transparent material in Unity Editor

    private GameObject currentCanonInstance;
    private string selectedCanonType = "";

    void Update()
    {
        if (currentCanonInstance != null)
        {
            // Follow the mouse cursor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentCanonInstance.transform.position = hit.point;
            }

            // Place the canon on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                currentCanonInstance.GetComponent<Renderer>().material = GetOriginalMaterial();
                currentCanonInstance = null; // Stop the update loop once the canon is placed
            }
        }
    }

    public void SetCanonType(string canonType)
    {
        if (currentCanonInstance == null) // Prevent instantiating a new canon if one is already being placed
        {
            selectedCanonType = canonType;
            GameObject prefab = canonType == "LightCanon" ? lightCanonPrefab : heavyCanonPrefab;
            currentCanonInstance = Instantiate(prefab);
            ApplyTransparentMaterial(currentCanonInstance);
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

    private Material GetOriginalMaterial()
    {
        // This method should return the original material of the canon.
        // For simplicity, let's assume each canon type has a single, specific material.
        // You might need a more sophisticated method for handling multiple materials or variations.
        if (selectedCanonType == "LightCanon")
        {
            return lightCanonPrefab.GetComponent<Renderer>().sharedMaterial; // Assuming the prefab has a Renderer
        }
        else // HeavyCanon
        {
            return heavyCanonPrefab.GetComponent<Renderer>().sharedMaterial;
        }
    }
}

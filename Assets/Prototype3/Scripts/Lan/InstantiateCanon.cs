using UnityEngine;

public class InstantiateCanon : MonoBehaviour
{
    public GameObject lightCanonPrefab;
    public GameObject heavyCanonPrefab;
    public Material transparentMaterial;
    public Camera gameCamera;

    private GameObject currentCanonInstance;
    private bool isPlacingCanon = false;
    private float placementDistance = 10f; // Distance from the camera at which the object is placed

    void Update()
    {
        if (isPlacingCanon && currentCanonInstance != null)
        {
            // Follow the mouse cursor in screen space
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, placementDistance);
            Vector3 worldPosition = gameCamera.ScreenToWorldPoint(mouseScreenPosition);
            currentCanonInstance.transform.position = worldPosition;

            // Place the canon on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                isPlacingCanon = false;
                currentCanonInstance.GetComponent<Renderer>().material = GetOriginalMaterial(currentCanonInstance);
            }
        }
    }

    public void InstantiateCanonOfType(string canonType)
    {
        if (!isPlacingCanon)
        {
            GameObject prefab = canonType == "LightCanon" ? lightCanonPrefab : heavyCanonPrefab;
            currentCanonInstance = Instantiate(prefab);
            currentCanonInstance.transform.position = gameCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, placementDistance)
            );
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
        return prefab.GetComponent<MeshRenderer>().sharedMaterial;
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightObjects : MonoBehaviour
{
    public Material highlightMaterial; // Material used for highlighting objects on mouse hover
    public Material selectionMaterial; // Material used for selected objects

    private Renderer currentHighlightedRenderer; // Renderer of the currently highlighted object
    private Renderer currentSelectedRenderer; // Renderer of the currently selected object
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>(); // Store original materials

    void Start()
    {
        // Find all objects tagged as "HighlightableObject"
        GameObject[] highlightableObjects = GameObject.FindGameObjectsWithTag("Highlight");

        foreach (GameObject obj in highlightableObjects)
        {
            // Get the Renderer component of each object
            Renderer objRenderer = obj.GetComponent<Renderer>();

            // Check if the object has a Renderer component
            if (objRenderer != null)
            {
                // Store the object's original material in the dictionary
                originalMaterials[objRenderer] = objRenderer.material;
            }
        }
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

                if (hitRenderer != null && hitRenderer != currentSelectedRenderer)
                {
                    if (currentHighlightedRenderer != hitRenderer)
                    {
                        RestoreHighlightedMaterial();

                        // Store original material if not already stored
                        if (!originalMaterials.ContainsKey(hitRenderer))
                        {
                            originalMaterials[hitRenderer] = hitRenderer.material;
                        }

                        currentHighlightedRenderer = hitRenderer;
                        hitRenderer.material = highlightMaterial;
                    }
                }
                else
                {
                    RestoreHighlightedMaterial();
                }
            }
            else
            {
                RestoreHighlightedMaterial();
            }
        }

        if (Input.GetMouseButtonDown(0) && currentHighlightedRenderer != null)
        {
            if (currentSelectedRenderer != null && currentSelectedRenderer != currentHighlightedRenderer)
            {
                RestoreSelectedMaterial();
            }

            if (!originalMaterials.ContainsKey(currentHighlightedRenderer))
            {
                originalMaterials[currentHighlightedRenderer] = currentHighlightedRenderer.material;
            }

            currentSelectedRenderer = currentHighlightedRenderer;
            currentHighlightedRenderer.material = selectionMaterial;

            currentHighlightedRenderer = null;
        }
    }

    private void RestoreHighlightedMaterial()
    {
        if (currentHighlightedRenderer != null && originalMaterials.ContainsKey(currentHighlightedRenderer))
        {
            currentHighlightedRenderer.material = originalMaterials[currentHighlightedRenderer];
            currentHighlightedRenderer = null;
        }
    }

    private void RestoreSelectedMaterial()
    {
        if (currentSelectedRenderer != null && originalMaterials.ContainsKey(currentSelectedRenderer))
        {
            currentSelectedRenderer.material = originalMaterials[currentSelectedRenderer];
            currentSelectedRenderer = null;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class CannonManager : MonoBehaviour
{
    [Header("Cannon Stats")]
    [Header("Heavy Cannon")]
    public int heavyCannonLimit = 2;
    private int heavyCannonCount = 0;
    public GameObject heavyCannonPrefab;
    [Header("Sniper")]
    public int sniperLimit = 2;
    private int sniperCount = 0;
    public GameObject sniperPrefab;
    [Space(16)]
    [Header("UI")]
    public Button sniperButton;
    public Button heavyCannonButton;
    public Text sniperText;
    public Text heavyCannonText;
    [Space(16)]
    [Header("Miscellanious")]
    public Material transparentMaterial;
    public Camera gameCamera;
    public LayerMask targetLayer;
    [SerializeField] private Vector3 rayPoint;
    [Space(16)]
    [Header("Private")]
    [SerializeField] private bool isPlacingCannon = false;
    [SerializeField] private string selectedCannonType = "";
    [SerializeField] private GameObject currentCannonInstance;
    Vector3 griddedPosition;
    private RaycastHit hitInfo;

    void Update()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, targetLayer.value);

        if (hitInfo.collider != null)
        {
            griddedPosition = hitInfo.collider.transform.position;
            griddedPosition.y += 2;
        }

        if (isPlacingCannon && currentCannonInstance != null)
        {
            currentCannonInstance.transform.position = griddedPosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (hit)
                {
                    griddedPosition.y += 1;
                    InstantiateAtPosition(griddedPosition);
                }

                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                {
                    isPlacingCannon = false;
                    Destroy(currentCannonInstance);
                }
            }

            if (isPlacingCannon && (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)))
            {
                isPlacingCannon = false;
                Destroy(currentCannonInstance);
            }
        }
    }

    private void InstantiateAtPosition(Vector3 position)
    {
        GameObject prefab = selectedCannonType == "Sniper" ? sniperPrefab : heavyCannonPrefab;
        GameObject newInstance = Instantiate(prefab, position, Quaternion.identity);
        newInstance.GetComponent<Renderer>().material = OriginalMaterial(newInstance);

        if (selectedCannonType == "Sniper")
        {
            sniperCount++;
            sniperText.text = $"{sniperLimit - sniperCount}";
            if (sniperCount >= sniperLimit)
            {
                sniperButton.interactable = false;
                StopPlacingCannon();
            }
        }
        else if (selectedCannonType == "HeavyCannon")
        {
            heavyCannonCount++;
            heavyCannonText.text = $"{heavyCannonLimit - heavyCannonCount}";
            if (heavyCannonCount >= heavyCannonLimit)
            {
                heavyCannonButton.interactable = false;
                StopPlacingCannon();
            }
        }
    }

    private void StopPlacingCannon()
    {
        if (currentCannonInstance != null)
        {
            Destroy(currentCannonInstance);
        }

        isPlacingCannon = false;
    }

    public void ResetCannonLimits()
    {
        sniperCount = 0;
        heavyCannonCount = 0;
        sniperText.text = $"{sniperLimit}";
        heavyCannonText.text = $"{heavyCannonLimit}";
        sniperButton.interactable = true;
        heavyCannonButton.interactable = true;
    }

    public void InstantiateCannonOfType(string cannonType)
    {
        if (!isPlacingCannon)
        {
            selectedCannonType = cannonType;
            GameObject prefab = cannonType == "Sniper" ? sniperPrefab : heavyCannonPrefab;
            currentCannonInstance = Instantiate(prefab);
            ApplyTransparentMaterial(currentCannonInstance);
            isPlacingCannon = true;
        }
    }

    private void ApplyTransparentMaterial(GameObject cannon)
    {
        Renderer[] renderers = cannon.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = transparentMaterial;
        }
    }

    private Material OriginalMaterial(GameObject cannonInstance)
    {
        GameObject prefab = cannonInstance.name.Contains("Sniper") ? sniperPrefab : heavyCannonPrefab;
        return prefab.GetComponent<Renderer>().sharedMaterial;
    }
}
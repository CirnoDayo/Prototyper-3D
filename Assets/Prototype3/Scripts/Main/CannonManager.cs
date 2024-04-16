using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CannonManager : MonoBehaviour
{
    [Header("Cannon Stats")]
    [Header("Heavy Cannon")]
    public int heavyCannonLimit;
    private int heavyCannonCount = 0;
    public GameObject heavyCannonPrefab;
    public GameObject fakeHeavyCannonPrefab;
    [Header("Sniper")]
    public int sniperLimit;
    private int sniperCount = 0;
    public GameObject sniperPrefab;
    public GameObject fakeSniperPrefab;
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


    #region SubscribeToCustomEvent

    private void OnEnable()
    {
        Lan_EventManager.UpdateSpawnedPoint += AddingTower;
    }

    private void OnDisable()
    {
        Lan_EventManager.UpdateSpawnedPoint -= AddingTower;    
    }


    #endregion

    private void Start()
    {
        sniperText.text = sniperLimit.ToString();
        heavyCannonText.text = heavyCannonLimit.ToString();
    }

    void Update()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, targetLayer.value);

        if (hitInfo.collider != null)
        {
            griddedPosition = hitInfo.collider.transform.position;
            griddedPosition.y += 5;
        }

        if (isPlacingCannon && currentCannonInstance != null)
        {
            currentCannonInstance.transform.position = griddedPosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (hit)
                {
                    griddedPosition.y += 2;
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
        GameObject prefab = selectedCannonType == "Lan_Sniper" ? sniperPrefab : heavyCannonPrefab;
        GameObject newInstance = Instantiate(prefab, position, Quaternion.identity);
        newInstance.GetComponent<Renderer>().material = OriginalMaterial(newInstance);

        if (selectedCannonType == "Lan_Sniper")
        {
            sniperCount++;
            sniperText.text = $"{sniperLimit - sniperCount}";
            if (sniperCount >= sniperLimit)
            {
                sniperButton.interactable = false;
                StopPlacingCannon();
            }
        }
        else if (selectedCannonType == "Lan_HeavyCannon")
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

    public void UpdateUI()//Call by UI Manager script
    {
        int sniperLeft = sniperLimit - sniperCount;
        int heavyCannonLeft = heavyCannonLimit - heavyCannonCount;
        
        sniperText.text = sniperLeft.ToString();
        heavyCannonText.text = heavyCannonLeft.ToString();
        
        if (sniperLeft > 0)
        {
            sniperButton.interactable = true;
        }
        else if (heavyCannonLeft > 0)
        {
            heavyCannonButton.interactable = true;
        }
        
    }

    private void AddingTower()//Call by custom event
    {
        int randomIndex = Random.Range(0, 2);
        if (randomIndex == 0)
        {
            sniperLimit++;
            
        }

        else
        {
            heavyCannonLimit++;
           
            
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
        
    }

    public void InstantiateCannonOfType(string cannonType)
    {
        if (!isPlacingCannon)
        {
            selectedCannonType = cannonType;
            GameObject prefab = cannonType == "Lan_Sniper" ? fakeSniperPrefab : fakeHeavyCannonPrefab;
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
        GameObject prefab = cannonInstance.name.Contains("Lan_Sniper") ? sniperPrefab : heavyCannonPrefab;
        return prefab.GetComponent<Renderer>().sharedMaterial;
    }
}
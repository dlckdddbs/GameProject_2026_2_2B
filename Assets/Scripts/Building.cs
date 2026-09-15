using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    [Header("건물 정보")]

    public BuildingType BuildType;     //여기
    public string buildingName = "건물";

    [System.Serializable]
    public class BuildingEvents
    {
        public UnityEvent<string> OndroverEntered;
        public UnityEvent<string> OnDriverExited;
        public UnityEvent<BuildingType> OnServiceUsed;
    }

    public BuildingEvents buildingEvents;

    private DeliveryOrderSystem orderSystem;

    private void Start()
    {
        SetupBuilding();
        orderSystem = FindFirstObjectByType<DeliveryOrderSystem>();
        CreateNameTag();
    }

    void HandleDriverService(DeliveryDriver dirver)
    {
        switch (BuildType)
        {
            case BuildingType.Restaurant:
                if (orderSystem != null)
                {
                    orderSystem.OnDriverEnteredRestaurant(this);
                }
                break;

            case BuildingType.Customer:
                if(orderSystem != null)
                {
                    orderSystem.OnDriverEnteredCustorm(this);
                }
                else 
                    dirver.CompletDelivery();
                break;

            case BuildingType.ChargingStation:
               
                dirver.ChargeBattery();
                break;
            
        }
        buildingEvents.OnServiceUsed?.Invoke(BuildType);
    }

    private void OnTriggerEnter(Collider other)
    {
        DeliveryDriver driver = other.GetComponent<DeliveryDriver>();
        if (driver != null)
        {
            buildingEvents.OndroverEntered?.Invoke(buildingName);
            HandleDriverService(driver);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DeliveryDriver driver = other.GetComponent<DeliveryDriver>();
        if (driver != null)
        {
            buildingEvents.OnDriverExited?.Invoke(buildingName);
            Debug.Log($"{buildingName} 을 떠났습니다.");
        }
    }
    void SetupBuilding()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            Material mat = renderer.material;

            switch (BuildType)
            {
                case BuildingType.Restaurant:
                    mat.color = Color.red;
                    break;
                case BuildingType.Customer:
                    mat.color = Color.green;
                    break;
                case BuildingType.ChargingStation:
                    mat.color = Color.yellow;
                    break;
            }
        }
        Collider col = GetComponent<Collider>();
        if (col != null) { col.isTrigger = true; }

    }

    void CreateNameTag()
    {

        GameObject nameTag = new GameObject("NameTag");
        nameTag.transform.SetParent(transform);
        nameTag.transform.localPosition = Vector3.up * 1.5f;

        TextMesh textMesh = nameTag.AddComponent<TextMesh>();
        textMesh.text = buildingName;
        textMesh.characterSize = 0.2f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.color = Color.white;
        textMesh.fontSize = 20;

        nameTag.AddComponent<Bilboard>();
    }

  

}

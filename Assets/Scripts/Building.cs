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

    private void Start()
    {
        SetupBuilding();
    }

    void HandleDriverService(DeliveryDriver dirver)
    {
        switch (BuildType)
        {
            case BuildingType.Restaurant:
                Debug.Log($"{buildingName} 에서 음식을 픽업 했습니다,");
                break;
            case BuildingType.Customer:
                Debug.Log($"{buildingName} 배달 완료,");
                dirver.CompletDelivery();
                break;
            case BuildingType.ChargingStation:
                Debug.Log($"{buildingName} 에서 배터리를 충전 했습니다.");
                dirver.ChargeBattery();
                break;
        }
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
                    buildingName = "음식점";
                    break;
                case BuildingType.Customer:
                    mat.color = Color.green;
                    buildingName = "고객 집";
                    break;
                case BuildingType.ChargingStation:
                    mat.color = Color.yellow;
                    buildingName = "충전소";
                    break;
            }
        }
        Collider col = GetComponent<Collider>();
        if (col != null) { col.isTrigger = true; }

    }

}

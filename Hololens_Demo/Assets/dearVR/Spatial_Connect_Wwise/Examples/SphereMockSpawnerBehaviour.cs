using System.Collections;
using UnityEngine;

[System.Serializable]
public struct SphereMock
{
    public string name;
    public Vector3 position;

    public SphereMock(string name, Vector3 position)
    {
        this.name = name;
        this.position = position;
    }
}

public class SphereMockSpawnerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject eventSpherePrefab = default;
    [SerializeField] private SphereMock[] sphereMocksAroundPlayer = null;
    [SerializeField] private SphereMock[] sphereMocksFixedPosition = null;

    private GameObject sphereParent_;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        
        sphereParent_ = new GameObject("EventSphere");

        SpawnAroundPlayer();
        SpawnAtFixedPosition();
        
        void SpawnAroundPlayer()
        {
            foreach (var sphereMock in sphereMocksAroundPlayer)
            {
                var sphereMockObj = Instantiate(eventSpherePrefab, sphereParent_.transform);
                sphereMockObj.name = sphereMock.name;
                sphereMockObj.transform.position = transform.position + sphereMock.position;
            }
        }

        void SpawnAtFixedPosition()
        {
            foreach (var sphereMock in sphereMocksFixedPosition)
            {
                var sphereMockObj = Instantiate(eventSpherePrefab, sphereParent_.transform);
                sphereMockObj.name = sphereMock.name;
                sphereMockObj.transform.position = sphereMock.position;
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(sphereParent_);
    }
}

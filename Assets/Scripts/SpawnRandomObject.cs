using UnityEngine;
public class SpawnRandomObject : MonoBehaviour
{
    public GameObject[] Objects;
    void Start()
    {
        int index = Random.Range(0, Objects.Length);
        Instantiate(Objects[index], this.transform);
        Objects[index].SetActive(true);
    }
}
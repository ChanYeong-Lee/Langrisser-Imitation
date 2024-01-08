using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSnowGenerator : MonoBehaviour
{
    public GameObject prefab;
    public float initX;
    private Queue<GameObject> queue = new Queue<GameObject>();

    private void Start()
    {
        PreGeneration();
        StartCoroutine(GenerateCoroutine());
    }

    private void PreGeneration()
    {
        for (int i = 0; i < 200; i++)
        {
            GameObject snow = Instantiate(prefab);
            snow.transform.parent = this.transform;
            snow.SetActive(false);
            queue.Enqueue(snow);
        }
    }
    IEnumerator GenerateCoroutine()
    {
        while (true)
        {
            float rndY = Random.Range(-4f, 10f);
            float rndScale = Random.Range(0.1f, 0.8f);
            float rndTime = Random.Range(0.05f, 0.5f);
            GameObject snow;
            if (queue.Count > 0)
            {
                snow = queue.Dequeue();
            }
            else
            {
                snow = Instantiate(prefab);
                snow.transform.parent = this.transform;
            }
            snow.transform.position = new Vector2(initX, rndY);
            snow.transform.localScale = new Vector2(rndScale, rndScale);
            snow.SetActive(true);
            snow.GetComponent<Rigidbody2D>().AddForce(new Vector2(180, 0));
            StartCoroutine(RemoveCoroutine(snow));
            yield return new WaitForSeconds(rndTime);
        
        }
    }

    IEnumerator RemoveCoroutine(GameObject snow)
    {
        yield return new WaitForSeconds(20f);
        snow.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        snow.SetActive(false);
        queue.Enqueue(snow);
    }
}

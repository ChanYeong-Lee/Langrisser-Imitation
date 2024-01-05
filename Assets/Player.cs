using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Node currentNode;
    public bool movable = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Node>(out Node node))
        {
            currentNode = node;
            print("You Reach New Node");
        }
    }

    public void Move(Node node)
    {
        if (movable)
        {
            StartCoroutine(MoveCoroutine(node));
        }
    }

    public void Move(List<Node> path)
    {
        if (movable)
        {
            StartCoroutine(MoveCoroutine(path));
        }
    }

    IEnumerator MoveCoroutine(Node node)
    {
        movable = false;
        Vector3 dir = node.transform.position - transform.position;
        dir.Normalize();
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - node.transform.position) < 0.0001f)
            {
                break;
            }
            transform.position = transform.position + (dir * Time.deltaTime);
            yield return null;
        }
        movable = true;
    }
    IEnumerator MoveCoroutine(List<Node> path)
    {
        foreach (Node node in path)
        {
            StartCoroutine(MoveCoroutine(node));
            yield return new WaitUntil(() => movable);
        }
    }
}
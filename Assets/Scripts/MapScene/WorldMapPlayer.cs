using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldMapPlayer : MonoBehaviour
{
    [HideInInspector] public WorldMapNode currentNode;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public UnityEvent OnMove;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<WorldMapNode>(out WorldMapNode node))
        {
            currentNode = node;
            print("You Reach New Node");
        }
        if (other.TryGetComponent<INodeEvent>(out INodeEvent INodeEvent))
        {
            INodeEvent.Execute();
        }
    }

    public void Move(WorldMapNode node)
    {
        if (false == isMoving)
        {
            StartCoroutine(MoveCoroutine(node));
            OnMove?.Invoke();
        }
    }

    public void Move(List<WorldMapNode> path)
    {
        if (false == isMoving)
        {
            StartCoroutine(MoveCoroutine(path));
            OnMove?.Invoke();
        }
    }

    IEnumerator MoveCoroutine(WorldMapNode node)
    {
        isMoving = true;
        Vector3 dir = node.transform.position - transform.position;
        dir.Normalize();
        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - node.transform.position) < 0.001f)
            {
                break;
            }
            transform.position = transform.position + (dir * Time.deltaTime);
            yield return null;
        }
        Debug.Log("moveEnd");
        isMoving = false;
    }

    IEnumerator MoveCoroutine(List<WorldMapNode> path)
    {
        foreach (WorldMapNode node in path)
        {
            StartCoroutine(MoveCoroutine(node));
            yield return new WaitWhile(() => isMoving);
        }
    }
}
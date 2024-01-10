using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectUI : MonoBehaviour
{
    public List<CharacterSelectElement> elements;
    public CharacterSelectElement prefab;

    private void Start()
    {
        float padding = 50;
        foreach (General general in PlayerData.Instance.generals)
        {

            Instantiate(prefab, (Vector2)transform.position + new Vector2(padding, 0), Quaternion.identity);
        }
    }

    public void TestSelection()
    {
        elements[0].SetGeneral(PlayerData.Instance.generals[0]);
    }
}

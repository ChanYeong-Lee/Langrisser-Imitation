using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData instance;
    public static PlayerData Instance { get { return instance; } }

    public Transform generalsParent;
    public Transform soldiersParent;
    public List<General> generals;
    public List<Soldier> soldiers;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool GetGeneral(General general)
    {
        if (generals.Exists((a) => a.GeneralType == general.GeneralType))
        {
            return false;
        }
        General newGeneral = Instantiate(DataManager.Instance.GetGeneral(general.GeneralType));
        newGeneral.transform.parent = generalsParent;
        newGeneral.gameObject.SetActive(false);
        generals.Add(newGeneral);
        return true;
    }
}

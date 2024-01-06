using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Ally,
    Enemy
}

public enum CharacterType
{

}

public class Object : MonoBehaviour
{
    public ObjectType type;
    public int objectID;
}

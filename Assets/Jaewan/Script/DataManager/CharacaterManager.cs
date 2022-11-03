using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character 
{
    Chunja,
    Yeonghee
}
public class CharacterManager : Singleton<CharacterManager>
{
    public Character selectCharacter;
}

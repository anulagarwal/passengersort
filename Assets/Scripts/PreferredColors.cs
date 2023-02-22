using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PreferredColors", order = 1)]

public class PreferredColors : ScriptableObject
{
    public List<CharacterColor> colors;
}

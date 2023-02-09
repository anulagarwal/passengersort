using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Row 
{
    public List<Character> characters;
    public CharacterColor color;
    public int maxCharacters;
    public Vector3 position;
    public bool isToSpawn;



    public void AddCharacter(Character c)
    {
        characters.Add(c);
    }

    public void MoveCharactersTo(Vector3 pos, Transform b)
    {
        foreach(Character c in characters)
        {
            c.MoveTo(pos, b);
        }
    }


   public void UpdateCharacterBuses(Bus b)
    {
        foreach (Character c in characters)
        {
            c.b = b;
        }
    }
    public void DisableAgents()
    {
        foreach (Character c in characters)
        {
            c.DisableAgent();
        }
    }
    public void EnableAgents()
    {
        foreach (Character c in characters)
        {
            c.EnableAgent();
        }
    }
    public bool IsStopped()
    {
        bool stop = false;

        foreach(Character c in characters)
        {
            if(c.isMoving)
            {
                stop = true;
            }
            else
            {
                return false;
            }
        }
        return stop;
    }
}

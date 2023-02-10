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
            int i = characters.FindIndex(x => x == c);
            Vector3 v = pos;
            if (i <= characters.Count / 2)
            {
                  v = new Vector3(pos.x - BusManager.Instance.xOffsetCharacter, pos.y, pos.z);
                //;
               // v = new Vector3(b.GetComponent<Bus>().GetTopRow().TransformPoint(pos).x - BusManager.Instance.xOffsetCharacter, pos.y, pos.z);

            }
            else
            {
                v = new Vector3(pos.x + BusManager.Instance.xOffsetCharacter, pos.y, pos.z);
              //  v = new Vector3(b.GetComponent<Bus>().GetTopRow().TransformPoint(pos).x + BusManager.Instance.xOffsetCharacter, pos.y, pos.z);

            }
            c.MoveTo(v, b);
        }
    }

    public void HighlightCharacters()
    {
        foreach(Character c in characters)
        {
            c.UpdateBodyMat(PassengerManager.Instance.GetHighlightBody(color));
            c.UpdateSkinMat(PassengerManager.Instance.GetHighlightSkin(color));
        }
    }

    public void DehighlightCharacters()
    {
        foreach (Character c in characters)
        {
            c.UpdateBodyMat(PassengerManager.Instance.GetOriginalBody(color));
            c.UpdateSkinMat(PassengerManager.Instance.GetOriginalSkin(color));
        }
    }

    public void UpdateCharacterRadius(float f)
    {
        foreach (Character c in characters)
        {
            c.UpdateRadius(f);
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
            if(c.state == CharacterState.Idle)
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

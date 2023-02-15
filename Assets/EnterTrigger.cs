using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    [SerializeField] public Bus b;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (other.gameObject.GetComponentInParent<Character>().b == b)
            {
                other.gameObject.GetComponentInParent<Character>().EnterBus(b);
                b.AddCharacter(other.gameObject.GetComponentInParent<Character>());
            }
            else
            {
                b.RemoveCharacter(other.gameObject.GetComponentInParent<Character>());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (other.gameObject.GetComponentInParent<Character>().b == b)
            {
                other.gameObject.GetComponentInParent<Character>().EnterBus(b);
                b.AddCharacter(other.gameObject.GetComponentInParent<Character>());
            }
            else
            {
                b.RemoveCharacter(other.gameObject.GetComponentInParent<Character>());
            }
        }
    }
}

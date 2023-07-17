using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour 
{
    public int id;
    public string text;

    public Answer(int id, string text)
    {
        this.id = id;
        this.text = text;
    }
}

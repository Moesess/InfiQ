using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public int id;
    public string text;

    public Question(int id, string text)
    {
        this.id = id;
        this.text = text;
    }
}

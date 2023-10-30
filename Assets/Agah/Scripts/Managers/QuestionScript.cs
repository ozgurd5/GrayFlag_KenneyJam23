using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionScript : MonoBehaviour
{
    [SerializeField]TMP_Text tmp;
    public void ChangeQuestion()
    {
        tmp.text = "Are you sure? (NO IF YOU'RE EPILEPTIC!)";
    }
}

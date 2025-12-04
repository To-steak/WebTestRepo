using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using TMPro;
using System.Collections;

public class KRtyper : MonoBehaviour
{
    public TMP_Text text;
    public string sentence;

    void Start()
    {
        
        StartCoroutine(Typer());
    }

    public IEnumerator Typer()
    {
        text.text = "";
        int len = sentence.GetTypingLength();
        for (int i = 0; i <= len; i++)
        {
            text.text = sentence.Typing(i);
            yield return new WaitForSeconds(0.5f);
        }
    }
}

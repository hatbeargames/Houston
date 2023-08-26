using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComp;
    public string[] TutorialLines;
    public float textSpeed;
    private int index;
    public GameObject hatch;
    [SerializeField] private GameObject controlsUI;
    private bool isTutorialComplete;
    private bool WPressed;
    private bool APressed;
    private bool DPressed;
    private bool LeftClickPressed;
    private bool RightClickPressed;
    // Start is called before the first frame update
    void Start()
    {
        textComp.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(textComp.text == TutorialLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComp.text = TutorialLines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //Type each character 1 by 1
        foreach(char c in TutorialLines[index].ToCharArray())
        {
            textComp.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < TutorialLines.Length - 1)
        {
            index++;
            textComp.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            hatch.SetActive(false);
            controlsUI.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}

using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CommandTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CommandManager.instance.Execute("moveCharDemo", "left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CommandManager.instance.Execute("moveCharDemo", "right");
        }
    }

    IEnumerator Running()
    {
        yield return CommandManager.instance.Execute("Print");
        yield return CommandManager.instance.Execute("Print_1p", "Hi!");
        yield return CommandManager.instance.Execute("Print_mp", "Line1", "Line2", "Line3");

        yield return CommandManager.instance.Execute("lambda");
        yield return CommandManager.instance.Execute("lambda_1p", "Hi! lambda");
        yield return CommandManager.instance.Execute("lambda_mp", "lambda1", "lambda2", "lambda3");

        yield return CommandManager.instance.Execute("process");
        yield return CommandManager.instance.Execute("process_1p", "3");
        yield return CommandManager.instance.Execute("process_mp", "process line 1", "process line 2", "process line 3");
    }
}

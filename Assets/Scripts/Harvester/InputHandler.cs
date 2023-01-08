using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[
    RequireComponent(typeof(PlayerInput))
]

public class InputHandler : MonoBehaviour
{
    public UnityEvent KeyPressed_Escape { get; private set; } = new UnityEvent();

    public static InputHandler GetReference()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
    }

#pragma warning disable IDE0051 //unused warning (it is used)
    //ESCAPE KEY
    private void Pause(InputAction.CallbackContext context)
    {
        print("ESCAPE PRESSED");
        KeyPressed_Escape.Invoke();
    }
#pragma warning restore IDE0051
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using XInputDotNetPure;
using UnityEngine.UI;



public class stackControllerPrueba : MonoBehaviour
{

    public GameObject stack;
    public GameObject stackDorado;
    private GameObject stackSpawner;
    public GameObject stackParent;
    private GameObject instantiatedStack;

    public float positionOffsetX = 0f;
    public float positionOffsetY = 0f;

    private bool stackOne;

    private int numeroStack;

    private float pitch = 0;
    private float pitchPlus = .05f;

    private int inputPlus = 1;

    private bool buttonCanBeActivated = true;

    public int minChanceGold;


    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private void Start()
    {
        stackOne = false;
    }

    void Update()
    {
        prevState = state;
        state = GamePad.GetState(playerIndex);

        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
            if (numeroStack >= 0 && numeroStack <= 58)
            {
                numeroStack += inputPlus;
                //print(numeroStack);
            }
            else if (numeroStack >= 59)
            {
                pitch += pitchPlus;

                stackOne = true;
                numeroStack = 0;
            }
        }

        if (positionOffsetX>13.3)
        {
            positionOffsetY += 1;
            positionOffsetX = 0;
        }

        if (stackOne)
        {
            GetRandomNumber();
            instantiatedStack = Instantiate(stackSpawner, new Vector3(-5+positionOffsetX,-4+positionOffsetY, 10) , transform.rotation);
            instantiatedStack.transform.SetParent(stackParent.transform);

            positionOffsetX += .7f;
            if (positionOffsetX > 13)
            {
                positionOffsetY += 1.75f;
                positionOffsetX = 0;
            }
            //Debug.Log(positionOffsetY);

            FindObjectOfType<AudioManager>().Play("stack", .5f + pitch);

            stackOne = false;

        }


    }


    public void OnButtonActivatePowerUpStack(int duracion, int intensidad)
    {
        if (buttonCanBeActivated)
        {
            inputPlus = intensidad;
            Invoke("PowerUpDisable", duracion);
            buttonCanBeActivated = false;
        }

    }

    private void PowerUpDisable()
    {
        inputPlus = 1;
        buttonCanBeActivated = true;
    }

    void GetRandomNumber()
    {
        int randomValue = Random.Range(1, 100);

        if (randomValue>=1 && randomValue<=minChanceGold)
        {
            stackSpawner = stack;
            FindObjectOfType<AudioManager>().Play("platosComplete", 1);

        }
        else if (randomValue>minChanceGold && randomValue<=100)
        {
            stackSpawner = stackDorado;
            FindObjectOfType<AudioManager>().Play("platosComplete", 7);
            FindObjectOfType<AudioManager>().Play("platosCompleteGold", 1);

        }
    }

    public void DefaultSpawnerPosition()
    {
        positionOffsetX = 0f;
        positionOffsetY = 0f;
        pitch = 0;
    }

}

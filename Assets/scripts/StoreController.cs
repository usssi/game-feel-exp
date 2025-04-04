using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class StoreController : MonoBehaviour
{
    public GameObject storeCanvas;
    public bool storeOn;
    public bool gamePaused;

    [Space]
    public GameObject shinyController;
    public GameObject comboController;
    public GameObject moneyText;

    public int maxMinShinyChance;
    public int maxDuration;

    [Space]
    public Button botonMulti;
    public Button botonDuration;
    public Button botonShiny;
    private float pitchMulti;
    private float pitchDuration;
    private float pitchShiny;

    public int money;
    public float currentMoney;
    public int initialMoney;

    public GameObject gamepadController;
    public GameObject playerController;
    public GameObject camController;

    public int precioMulti;
    public int precioTimer;
    public int precioShiny;

    private int precioMultiMulti;
    private int precioTimerMulti;
    private int precioShinyMulti;

    public Button firstSelectedButton;

    public TextMeshProUGUI precioXxText;
    public TextMeshProUGUI precioTimeText;
    public TextMeshProUGUI precioShinyText;

    public TextMeshProUGUI multitext, multiinfo, timetext, timeinfo, shinytext, shinyinfo;

    private bool yPressed;

    public GameObject timeBarFill;
    private bool timeAnimationBool;
    private float lerpedAnimationTimer;

    public GameObject multiplierTxt;
    private bool multiAnimationBool;
    private float lerpedAnimationMultiplier;

    public GameObject partMultiButton;
    public GameObject partTimerButton;
    public GameObject partShinyButton;
    public GameObject partMulti;
    public GameObject partTimer;
    public GameObject partShiny;

    public GameObject storecanvas2;

    public Image iconMulti;
    public Image iconTime;
    public Image iconShiny;

    public GameObject stackController;

    void Start()
    {
        storeCanvas.SetActive(false);
        storeOn = false;

        precioMultiMulti = 50;
        precioTimerMulti = 50;
        precioShinyMulti = 50;

        storecanvas2.GetComponent<Animator>().speed = 0;

        DisablePart();
    }

    // Update is called once per frame
    void Update()
    {
        TimeAnimation();
        MultiAnimation();

        if (gamePaused == false)
        {
            if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
            {
                StoreToggle();
            }
            if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                StoreToggle();
            }
        }

        //animacion money
        if (currentMoney != money)
        {
            if (initialMoney < money)
            {

                currentMoney += (1.5f * Time.deltaTime) * (money - initialMoney);
                if (currentMoney >= money)
                {
                    currentMoney = money;
                }

            }
            else
            {
                currentMoney -= (1.5f * Time.deltaTime) * (initialMoney - money);
                if (currentMoney <= money)
                {
                    currentMoney = money;
                }
            }
        }

        moneyText.GetComponent<TextMeshProUGUI>().text = "$" + currentMoney.ToString("0");

        /////////////////////////////////////////////////////////////////////////////
        if (storeOn)
        {
            storeCanvas.SetActive(true);
            storeOn = true;
            playerController.SetActive(false);
            comboController.GetComponent<comboController>().isPaused = true;
            //gamepadController.GetComponent<gamepadController>().canVibrate = false;
            camController.GetComponent<zoomController>().isInStore = true;
            camController.GetComponent<changeBG>().isInStore = true;

            if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                StoreToggle();
            }

        }

        precioXxText.text = "$" + precioMulti.ToString();
        precioTimeText.text = "$" + precioTimer.ToString();
        precioShinyText.text = "$" + precioShiny.ToString();


        if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            yPressed = true;
        }

        if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
        {
            yPressed = false;
        }

        if (yPressed)
        {
            if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                MoneyUP();
            }
            if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                MoneyZero();
            }
        }

        //money color
        if (money >= precioShiny)
        {
            precioShinyText.color = Color.green;
        }
        else
        {
            precioShinyText.color = Color.red;

        }

        if (money >= precioTimer)
        {
            precioTimeText.color = Color.green;
        }
        else
        {
            precioTimeText.color = Color.red;

        }

        if (money >= precioMulti)
        {
            precioXxText.color = Color.green;
        }
        else
        {
            precioXxText.color = Color.red;
        }

    }

    public void StoreToggle()
    {
        if (storeOn == false && comboController.GetComponent<comboController>().comboCanBeActivated &&
            !stackController.GetComponent<stackController>().isSelling)
        {
            firstSelectedButton.Select();
            storeCanvas.SetActive(true);
            storeOn = true;
            FindObjectOfType<AudioManager>().Play("storeOpen", 1f);
            playerController.SetActive(false);
            comboController.GetComponent<comboController>().isPaused = true;
            //gamepadController.GetComponent<gamepadController>().canVibrate = false;
            camController.GetComponent<zoomController>().isInStore = true;
            camController.GetComponent<changeBG>().isInStore = true;

            camController.GetComponent<cameraShake>().enabled = false;
            camController.GetComponent<changeBG>().enabled = false;
            camController.GetComponent<zoomController>().enabled = false;

            if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                StoreToggle();
            }

            storecanvas2.GetComponent<Animator>().speed = 2;

            storecanvas2.GetComponent<Animator>().SetBool("storeisopen", false);

            stackController.GetComponent<stackController>().CalculaordeProfit();

        }
        else if (storeOn)
        {
            StopAllCoroutines();
            storeCanvas.GetComponent<Animator>().SetBool("storeout", true);
            Invoke("CloseStore", .2f);

            storeOn = false;
            FindObjectOfType<AudioManager>().Play("storeClose", 1);
            playerController.SetActive(true);
            comboController.GetComponent<comboController>().isPaused = false;
            //gamepadController.GetComponent<gamepadController>().canVibrate = true;
            camController.GetComponent<zoomController>().isInStore = false;
            camController.GetComponent<changeBG>().isInStore = false;

            camController.GetComponent<cameraShake>().enabled = true;
            camController.GetComponent<changeBG>().enabled = true;
            camController.GetComponent<zoomController>().enabled = true;

            storecanvas2.GetComponent<Animator>().speed = 3;

            storecanvas2.GetComponent<Animator>().SetBool("storeisopen", true);
        }
    }

    public void StoreStackShiny()
    {

        if (money >= precioShiny)
        {
            FindObjectOfType<AudioManager>().Play("buyShiny", 1 + pitchShiny);
            pitchShiny -= .05f;

            if (shinyController.GetComponent<stackControllerPrueba>().minChanceGold > maxMinShinyChance)
            {
                shinyController.GetComponent<stackControllerPrueba>().minChanceGold -= 10;

            }
            initialMoney = (int)currentMoney;

            money = money - precioShiny;

            precioShiny += precioShinyMulti;

            precioShinyMulti += 50;

            var colors = botonShiny.colors;

            colors.pressedColor = Color.green;
            botonShiny.colors = colors;

            partShiny.SetActive(true);
            partShinyButton.SetActive(true);

            Invoke("DisablePart", .7f);

        }
        else
        {
            FindObjectOfType<AudioManager>().Play("buttonDenied", 1);

            var colors = botonShiny.colors;

            colors.pressedColor = Color.red;
            botonShiny.colors = colors;

        }

        if (shinyController.GetComponent<stackControllerPrueba>().minChanceGold == maxMinShinyChance)
        {
            botonShiny.interactable = false;
            //botonShiny.enabled = false;
            precioShinyText.enabled = false;
            firstSelectedButton.Select();

            shinytext.color = Color.grey;
            shinyinfo.color = Color.grey;

            iconShiny.color = Color.gray;

        }
    }

    public void StoreDuration()
    {

        if (money >= precioTimer)
        {
            timeAnimationBool = true;
            lerpedAnimationTimer = 0;

            FindObjectOfType<AudioManager>().Play("buyTime", 1 + pitchDuration);
            pitchDuration -= .1f;

            if (comboController.GetComponent<comboController>().duration < maxDuration)
            {
                comboController.GetComponent<comboController>().duration += 5;
                comboController.GetComponent<comboController>().durationTimerText = comboController.GetComponent<comboController>().duration + 1;
            }
            initialMoney = (int)currentMoney;

            money = money - precioTimer;

            precioTimer += precioTimerMulti;
            precioTimerMulti += 100;

            comboController.GetComponent<comboController>().minimunFillTime += .06f;

            var colors = botonDuration.colors;

            colors.pressedColor = Color.green;
            botonDuration.colors = colors;

            partTimer.SetActive(true);
            partTimerButton.SetActive(true);

            Invoke("DisablePart", .7f);

        }
        else
        {
            FindObjectOfType<AudioManager>().Play("buttonDenied", 1);

            var colors = botonDuration.colors;

            colors.pressedColor = Color.red;
            botonDuration.colors = colors;

        }

        if (comboController.GetComponent<comboController>().duration == maxDuration)
        {
            botonDuration.interactable = false;
            //botonDuration.enabled = false;
            precioTimeText.enabled = false;
            firstSelectedButton.Select();

            timetext.color = Color.grey;
            timeinfo.color = Color.grey;

            iconTime.color = Color.gray;

        }

    }

    public void StoreMultiplier()
    {
        if (money >= precioMulti)
        {
            FindObjectOfType<stackControllerPrueba>().InputPlusLogicMulti();
            FindObjectOfType<platosController>().InputPlusLogicMulti();

            multiAnimationBool = true;
            lerpedAnimationMultiplier = 0;

            FindObjectOfType<AudioManager>().Play("buyMulti", 1 + pitchMulti);
            pitchMulti -= .05f;

            int intensidad = comboController.GetComponent<comboController>().intensidad;

            if (intensidad < 6)
            {
                comboController.GetComponent<comboController>().intensidad += 1;

            }
            else if (intensidad >= 6 && intensidad < 10)
            {
                comboController.GetComponent<comboController>().intensidad = 10;

            }
            else if (intensidad >= 10 && intensidad < 12)
            {
                comboController.GetComponent<comboController>().intensidad = 12;

            }
            else if (intensidad >= 12 && intensidad < 15)
            {
                comboController.GetComponent<comboController>().intensidad = 15;

            }
            else if (intensidad >= 15 && intensidad < 20)
            {
                comboController.GetComponent<comboController>().intensidad = 20;

            }
            else if (intensidad >= 20 && intensidad < 30)
            {
                comboController.GetComponent<comboController>().intensidad = 30;

            }
            else if (intensidad >= 30 && intensidad < 60)
            {
                comboController.GetComponent<comboController>().intensidad = 60;

            }

            initialMoney = (int)currentMoney;

            money = money - precioMulti;

            precioMulti += precioMultiMulti;
            precioMultiMulti += 50;

            var colors = botonMulti.colors;

            colors.pressedColor = Color.green;
            botonMulti.colors = colors;

            partMultiButton.SetActive(true);
            partMulti.SetActive(true);

            Invoke("DisablePart", .7f);

        }
        else
        {
            FindObjectOfType<AudioManager>().Play("buttonDenied", 1);

            var colors = botonMulti.colors;

            colors.pressedColor = Color.red;
            botonMulti.colors = colors;
        }


        if (comboController.GetComponent<comboController>().intensidad >= 60)
        {
            botonMulti.interactable = false;
            //botonMulti.enabled = false;
            precioXxText.enabled = false;
            firstSelectedButton.Select();
            multitext.color = Color.grey;
            multiinfo.color = Color.grey;

            iconMulti.color = Color.gray;

        }

    }

    public void MoneyUP()
    {
        initialMoney = (int)currentMoney;

        FindObjectOfType<AudioManager>().Play("sellButton", 1);

        money += 9999;
    }

    public void MoneyZero()
    {
        initialMoney = (int)currentMoney;

        FindObjectOfType<AudioManager>().Play("sellButton", 1);

        money = 0;
    }

    public void CloseStore()
    {
        storeCanvas.SetActive(false);
    }

    public void TimeAnimation()
    {
        if (timeAnimationBool)
        {
            timeBarFill.GetComponent<Image>().color = Color.Lerp(Color.magenta, Color.white, lerpedAnimationTimer);
        }

        lerpedAnimationTimer += Time.deltaTime;

        if (lerpedAnimationTimer > 1)
        {
            lerpedAnimationTimer = 1;
            timeAnimationBool = false;
        }
    }

    public void MultiAnimation()
    {
        if (multiAnimationBool)
        {
            multiplierTxt.GetComponent<TextMeshProUGUI>().color = Color.Lerp(Color.yellow, Color.white, lerpedAnimationMultiplier);
        }

        lerpedAnimationMultiplier += Time.deltaTime;

        if (lerpedAnimationMultiplier > 1)
        {
            lerpedAnimationMultiplier = 1;
            multiAnimationBool = false;
        }
    }

    private void DisablePart()
    {
        partShiny.SetActive(false);
        partShinyButton.SetActive(false);

        partTimer.SetActive(false);
        partTimerButton.SetActive(false);

        partMulti.SetActive(false);
        partMultiButton.SetActive(false);
    }

}
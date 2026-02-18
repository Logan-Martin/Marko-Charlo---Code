using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    private static Button playButton;
    private static Button creditsButton;
    private static Button quitGameButton;
    private static Button closeCreditsMenuButton;

    public AudioSource partyAmbience;
    public AudioSource clickSfx;

    public GameObject creditsMenu;
    //
    public GameObject DayUI;
    //
    public UISystemMain UISystemMainScript;
    //

    public void PlayGame()
    {
        clickSfx.Play();
        Cursor.lockState = CursorLockMode.Locked;
        DayUI.SetActive(true);
        DayUI.transform.GetChild(0).gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        CloseCreditMenu();
        UISystemMainScript.ChangeCurrentTimeDay("day1");
        partyAmbience.Play();
    }

    private bool creditsMenuOpen = false;
    public void ToggleCreditMenu()
    {
        if (creditsMenuOpen == false)
        {
            creditsMenuOpen = true;
        }
        else
        {
            creditsMenuOpen = false;
        }
        creditsMenu.SetActive(creditsMenuOpen);
    }
    public void CloseCreditMenu()
    {
        clickSfx.Play();
        creditsMenuOpen = false;
        creditsMenu.SetActive(creditsMenuOpen);
    }

    public void QuitGame()
    {
        clickSfx.Play();
        print("quit the game!");
        Application.Quit();
    }

    //
    void Start()
    {
        playButton = this.transform.GetChild(2).GetComponent<Button>();
        creditsButton = this.transform.GetChild(3).GetComponent<Button>();
        closeCreditsMenuButton = creditsMenu.transform.GetChild(3).GetComponent<Button>();
        quitGameButton = this.transform.GetChild(4).GetComponent<Button>();

        //
        playButton.onClick.AddListener(PlayGame);
        creditsButton.onClick.AddListener(ToggleCreditMenu);
        closeCreditsMenuButton.onClick.AddListener(CloseCreditMenu);
        quitGameButton.onClick.AddListener(QuitGame);

    }

}

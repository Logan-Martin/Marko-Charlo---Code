using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystemMain : MonoBehaviour
{
    // day indoors visual novel
    // night outside forest-y
    // 3d mode trees, invisible barrier
    // Marco Polo, button to say Macro, hear back multiple Polo and desern (particle & audio)
    // keep track of correct vs not, enough perfect yay, if get wrong
    // ending visual novel based on correctness
    // Overall: Day (Visual Novel) -> Night (3D Sequence) w/ Choice -> Day (Visual Novel - 2 Endings)

    // Cameras:
    public Camera DayVisualNovelCamera;
    public Camera NightCamera;

    // Main Menu UI:
    public GameObject MainMenuUI; 

    // Day UI Main:
    public GameObject VisualNovelUI;
    //public AudioSource dialougeSfx;
    public GameObject NameTextbox;
    public GameObject DialougeTextbox;
    public GameObject Background;
    //
    public GameObject playerSprite;
    public GameObject friendSprite;
    //

    // Night Stuff:
    public GameObject NightUI;
    public GameObject Night_NameTextbox;
    public GameObject Night_DialougeTextbox;
    public GameObject spawnPart;
    public GameObject levelFolder;
    public GameObject notchNumberTextbox1;

    // Character Names:
    public static string Narrator = "";
    public static string PlayerName = "Mark";
    public static string FriendName = "Charlie";
    public static string MomName = "Mark's Mom";
    public static string DadName = "Mark's Dad";
    //
    //

    // Audio:
    public GameObject VoiceLinesFolder;
    public AudioSource marcoCallout1;
    //public static AudioSource marcoCallout2;
    //public static AudioSource marcoCallout3;
    public AudioSource partyAmbience;
    public AudioSource forestAmbience;
    public AudioSource pageTurnSfx;

    // ---//

    // Variables:
    private int day1Index = 1; // already set at 0 in startup
    private int day2Index = 0;
    private bool currentTextDonePlaying = true;
    private string currentTimeDay = "mainMenu"; // mainMenu, day1, night1, day2Bad, day2Good
    private bool ranAFirstTime = false;
    //
    private int currentLevelInNight = 1;
    private int goodAudioSignalFollowed = 0;
    private int totalNightLevels = 3;
    private float percentOfGoodNeededForGoodEnding = 1.0f;
    private int maxNotches = 3;
    private int currentNotches = 3;
    // --- //


    public void ChangeCurrentTimeDay(string newTimeString)
    {
        currentTimeDay = newTimeString;
    }

    // Player:
    public GameObject PlayerModel;

    void TogglePlayerMovement(bool toggle1)
    {
        NightCamera.GetComponent<MouseLook>().enabled = toggle1;
        PlayerModel.GetComponent<PlayerMovement>().enabled = toggle1;
    }
    // ---- //
    

    public List<List<string>> dialouge_Day1 = new List<List<string>> {
        // 0 = Name, 1 = Dialouge
        new List<string> { Narrator, "On Sunday afternoons, Dad invites the whole world to Sunday Barbeque. There has to be at least a hundred people here in our backyard. I tried to count them but they keep moving so it's hard." },
        new List<string> { Narrator, "I tried to count them but they keep moving so it's hard. The air smells of Dad's smoker, hickory wood." },
        new List<string> { Narrator, "\"Mark!\" Mom kneels down to my height. Her left hand holds a glass of red drink. \"How would you like to play in the pool? It's warm enough to open, finally.\" She has been in the kitchen all day preparing." },
        new List<string> { Narrator, "Something feels funny in my stomach. There's so many people here and I don't want to. I don't want them to see me. I don't want to see me. Mom is quiet and then says in her no-nonsense voice." },
        new List<string> { MomName, "\"Honey, let me open the pool. Mommy's very busy with all our guests, okay?\"" },
        new List<string> { Narrator, "I start to shake my head."},
        new List<string> { FriendName, "\"Mark!\"" }, // 0
        new List<string> { Narrator, "The feeling goes away because Charlie is here. Charlie is the best part of Sunday Barbeque because his dad never misses it, he lives two houses down from me so we get to see each other all the time. \"Charlie!" },
        new List<string> { Narrator, "I have to dodge adults and benches to meet him across the grass of my backyard. Distantly a dog barks, a high and belting sound. Maybe the dog feels left out. \"Charlie! Charlie?\"" },
        new List<string> { Narrator, "Something is wrong because Charlie isn't smiling. \"Charlie?\" I say again." },
        new List<string> { Narrator, "Charlie's dad is talking to Dad at the smoker. Charlie's mom has gone to help my Mom. It's just us two. Charlie typically loves Sundays, But when we lock eyes, I can tell that Charlie's usual sunday spirit is gone." },
        new List<string> { FriendName, "Charlie looks away and says \"I'm going tomorrow.\" He is so quiet I can barely hear him." }, // 1
        new List<string> { PlayerName, "\"Going where?\"" },
        new List<string> { FriendName, "A different catholic school. Dad is making me go. It's for boys, he said.\" Charlie's voice does not hold the word 'boys' comfortably." }, // 2 -- i = 12
        new List<string> { Narrator, "I knew this. Charlie was supposed to go away but he can't go. Especially not a boy's school. The feeling comes again. Mom's voice is behind me. \"Mark, why don't you and Charlie get changed to play together in the pool?\"" },
        new List<string> { Narrator, "Charlie looks like how I feel. I don't want to change." },
        new List<string> { FriendName, "but Mom… Charlie says quietly so the adults can't hear,\"I don't want to take off my shirt.\"" }, // 3
        new List<string> { PlayerName, "My eyes drift beyond the pool and to the woods behind my house. \"Let's play Marco Polo in the woods instead?\" It's easy to not talk about Charlie leaving. He can't leave. He can't. Charlie nods." },
        new List<string> { PlayerName, "\"Mom, can we play in the woods?\" I ask. Mom takes one big swig of her red drink." },
        new List<string> { MomName, "\"Sure, sweetie.\"" },
        new List<string> { PlayerName, "\"I'll be Marco, you be Polo?\"" },
        new List<string> { FriendName, "\"Yeah, okay.\"" }, // 4
        new List<string> { Narrator, "*time passes*" },
        new List<string> { MomName, "The sun is about to set. \"Charlie! It's time for you to go home!\" Charlie and I look at each other." },
        new List<string> { FriendName, "\"Mark… We're friends, right? Can I ask you something?\"" }, // 5
        new List<string> { PlayerName, "\"Yeah. Yeah, Charlie. What's wrong?\"" },
        new List<string> { FriendName, "\"Do you… like. Ever feel like…?\"" }, //6
        new List<string> { MomName, "\"Charlie! It's go time, NOW!\"" },
        new List<string> { Narrator, "Charlie turns to go. Mom stands behind me. \"You'll get to see him tomorrow, honey. Promise. Let's get you ready for bedtime, okay?\" She holds my shoulder. Her hand is so cold." },

    };

    public List<List<string>> dialouge_Night = new List<List<string>> {
        // 0 = Name, 1 = Dialouge
        new List<string> { PlayerName , "Hello person 2!" },
        new List<string> { FriendName, "Hi person 1!" },
        new List<string> { PlayerName, "Okay bye!" },
    };

    public List<List<string>> dialouge_Day2_GoodEnding = new List<List<string>> {
        // 0 = Name, 1 = Dialouge
        new List<string> { Narrator , "Charlie stands on his yard while his dad loads his suitaces into the back of his car. My whole body feels cold now that he is leaving. Mom has her hand in mine and squeezes sometimes." },
        new List<string> { Narrator , "I let her hand go and walk towards Charlie. Charlie's crying. He rubs his face in his sleeves but he doesn't hide he's crying. I'm crying too. \"…Do you ever hate being a boy?\" Charlie asks. His voice is so small." },
        new List<string> { FriendName , "\"…Do you ever hate being a boy?\" Charlie asks. His voice is so small." },
        new List<string> { PlayerName, "\"Sometimes,\" my voice is small too. But my feelings are big. \"You don't have to go.\"" },
        new List<string> { FriendName, "\"I don't want them to make me a boy.\"" }, // 11
        new List<string> { Narrator, "Charlie nods. He's still crying." },
        new List<string> { PlayerName, "\"Let's pick better names for ourselves. In the summer, we'll share them. We can be friends again. That's how we'll remember. I promise.\"" },
        new List<string> { Narrator, "I lean in for a hug. I squeeze really hard. this is the closest I've felt to Charlie since the first grade. My friend squeezes me just as hard." },
        new List<string> { Narrator, "My friend begins to go. In the summer we will share our names. I'll hold that promise for the both of us until then." },

    };

    public List<List<string>> dialouge_Day2_BadEnding = new List<List<string>> {
        // 0 = Name, 1 = Dialouge
        new List<string> { Narrator , "Charlie stands on his yard while his dad loads his suitcases into the back of his car. My whole body feels cold now that he is leaving. Mom has her hand in mine and squeezes sometimes." },
        new List<string> { Narrator, "I'm not going to cry because boys don't cry. Charlie doesn't cry either." },
        new List<string> { Narrator, "\"I'll see you in the summer,\" I say. But that's not true. Charlie's eyes are hard and his jaw is clenched. He nods and he gets in his dad's car." },
        new List<string> { Narrator, "I have dreams where the mask is on. Every night it feels more like a part of my face." },

    };

    //
    IEnumerator TeleportPlayerToSpawn()
    {
        TogglePlayerMovement(false);
        PlayerModel.transform.position = spawnPart.transform.position;

        yield return new WaitForSeconds(1f);
        
        Physics.SyncTransforms();
        PlayerModel.transform.position = spawnPart.transform.position;

        TogglePlayerMovement(true);
    }
    //
        
    void TryToPlayNextDialouge()
    {
        //print("Try to play next dialouge!");
        //print(currentTextDonePlaying);
        if (currentTextDonePlaying == true)
        {
            pageTurnSfx.Play();
            if (currentTimeDay == "day1")
            {
                if (day1Index == dialouge_Day1.Count)
                {
                    // Transition to Night?!
                    print("DONE WITH DAY 1!");
                    partyAmbience.Stop();
                    forestAmbience.Play();
                    currentTimeDay = "night1";
                    DayVisualNovelCamera.enabled = false;
                    NightCamera.enabled = true;
                    NightUI.SetActive(true);
                    TogglePlayerMovement(true);
                }
                else
                {
                    // Main update things:
                    NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day1[day1Index][0];
                    DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day1[day1Index][1];

                    if (dialouge_Day1[day1Index][0] == PlayerName )
                    {
                        friendSprite.SetActive(false);
                        playerSprite.SetActive(true);

                    }
                    else if (dialouge_Day1[day1Index][0] == FriendName)
                    {
                        friendSprite.SetActive(true);
                        playerSprite.SetActive(false);

                    }
                    else
                    {
                        friendSprite.SetActive(false);
                        playerSprite.SetActive(false);
                    }

                    // VOICE LINES:                    
                    if (day1Index == 6)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>().Play();
                    }
                    else if (day1Index == 11)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(1).GetComponent<AudioSource>().Play();
                    }
                    else if (day1Index == 13)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(2).GetComponent<AudioSource>().Play();
                    }
                    else if (day1Index == 16)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(3).GetComponent<AudioSource>().Play();
                    }
                    else if (day1Index == 21)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(4).GetComponent<AudioSource>().Play();
                    }
                    else if (day1Index == 24)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(5).GetComponent<AudioSource>().Play();
                    }
                    else if (day1Index == 26)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(6).GetComponent<AudioSource>().Play();
                    }

                    // --- //



                    // add:
                    day1Index++;
                    //
                    currentTextDonePlaying = true;
                }
            }
            else if (currentTimeDay == "day2Good")
            {
                if (day2Index == dialouge_Day2_GoodEnding.Count)
                {
                    // Transition to Main Menu
                    print("Done with Good Ending!");
                    ResetEverythingToMainMenu();
                }
                else
                {
                    // Main update things:
                    NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_GoodEnding[day2Index][0];
                    DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_GoodEnding[day2Index][1];

                    if (dialouge_Day2_GoodEnding[day2Index][0] == PlayerName)
                    {
                        friendSprite.SetActive(false);
                        playerSprite.SetActive(true);

                    }
                    else if (dialouge_Day2_GoodEnding[day2Index][0] == FriendName)
                    {
                        friendSprite.SetActive(true);
                        playerSprite.SetActive(false);

                    }
                    else
                    {
                        friendSprite.SetActive(false);
                        playerSprite.SetActive(false);
                    }

                    // VOICE LINES:                    
                    if (day2Index == 4)
                    {
                        VoiceLinesFolder.transform.GetChild(0).GetChild(11).GetComponent<AudioSource>().Play();
                    }


                    // add:
                    day2Index++;
                    //
                    currentTextDonePlaying = true;
                }
            }
            else if (currentTimeDay == "day2Bad")
            {
                if (day2Index == dialouge_Day2_BadEnding.Count)
                {
                    // Transition to Main Menu
                    print("Done with Bad Ending!");
                    ResetEverythingToMainMenu();
                }
                else
                {
                    // Main update things:
                    NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_BadEnding[day2Index][0];
                    DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_BadEnding[day2Index][1];

                    if (dialouge_Day2_BadEnding[day2Index][0] == PlayerName)
                    {
                        friendSprite.SetActive(false);
                        playerSprite.SetActive(true);

                    }
                    else if (dialouge_Day2_BadEnding[day2Index][0] == FriendName)
                    {
                        friendSprite.SetActive(true);
                        playerSprite.SetActive(false);

                    }
                    else
                    {
                        friendSprite.SetActive(false);
                        playerSprite.SetActive(false);
                    }

                    // add:
                    day2Index++;
                    //
                    currentTextDonePlaying = true;
                }
            }
            else
            {
                if (currentTimeDay == "night1") return;
                print("WARNING - currentTimeDay wrong string");
            }
        }
        else
        {
            print("current text not done!");
        }
        
    }

    IEnumerator PlayThisLevelsSoundCues_AfterSetTime(float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        // -------- //
        string levelName = "Level" + currentLevelInNight.ToString();
        Transform level = levelFolder.transform.Find(levelName);
        if (level != null)
        {
            if (ranAFirstTime == false)
            {
                ranAFirstTime = true;
                level.gameObject.SetActive(true);
            }

            foreach (Transform child in level)
            {
                child.gameObject.GetComponent<AudioSource>().Play();
                child.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            print("WARNING - Level name not valid/found");
        }
    }

    public void ResetMarcoButton()
    {
        currentNotches = maxNotches;
        notchNumberTextbox1.GetComponent<TMPro.TextMeshProUGUI>().text = currentNotches.ToString();
    }


    public void ResetLevelsGameObjectActive()
    {
        foreach (Transform child in levelFolder.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    //--//
    public void ShowDialougeAtNight()
    {
        //Night_DialougeTextbox
        //Night_NameTextbox
    }
    public void UpdateNightGuessCount(bool goodOrBad)
    {

        if (goodOrBad == true)
        {
            goodAudioSignalFollowed++;
            print("UPDATE GOOD SIGNAL FOLLOWED!");
        }
        ranAFirstTime = false; // hopefully to stop current sound cues from looping
        ResetLevelsGameObjectActive();
        currentLevelInNight++;
        ResetMarcoButton();
        StartCoroutine(TeleportPlayerToSpawn());

        //
        if (currentLevelInNight >= (totalNightLevels + 1) )
        {
            Cursor.lockState = CursorLockMode.None;
            TogglePlayerMovement(false);
            NightUI.SetActive(false);
            DayVisualNovelCamera.enabled = true;
            NightCamera.enabled = false;
            VisualNovelUI.SetActive(true);

            // Good Ending:
            if ( ((float)goodAudioSignalFollowed / totalNightLevels) >= percentOfGoodNeededForGoodEnding)
            {
                print("Good ending start!");
                currentTimeDay = "day2Good";
                NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_GoodEnding[0][0];
                DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_GoodEnding[0][1];
            }
            else
            {
                print("Bad ending start!");
                currentTimeDay = "day2Bad";
                NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_BadEnding[0][0];
                DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day2_BadEnding[0][1];
            }
        }

    }

    //
    public void ResetEverythingToMainMenu()
    {
        currentTimeDay = "mainMenu";
        Cursor.lockState = CursorLockMode.None;

        // Audio:
        partyAmbience.Stop();
        forestAmbience.Stop();

        // UI:
        VisualNovelUI.SetActive(false);
        NightUI.SetActive(false);
        MainMenuUI.SetActive(true);

        // Random:
        TogglePlayerMovement(false);
        ResetLevelsGameObjectActive();
        ResetMarcoButton();

        // Variables:
        day1Index = 0;
        day2Index = 0;
        currentLevelInNight = 1;
        goodAudioSignalFollowed = 0;
        ranAFirstTime = false;

        // Start stuff here:
        NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day1[0][0];
        DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day1[0][1];
    }
    //

    void Start() 
    {
        NameTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day1[0][0];
        DialougeTextbox.GetComponent<TMPro.TextMeshProUGUI>().text = dialouge_Day1[0][1];
        //
        notchNumberTextbox1.GetComponent<TMPro.TextMeshProUGUI>().text = currentNotches.ToString();
        print("inital UI info settting loaded");        
    }

    //
    public void OnClickForMarcoButton()
    {
        print("marco button pressed");
        if (currentNotches != 0 )
        {
            currentNotches--;
            notchNumberTextbox1.GetComponent<TMPro.TextMeshProUGUI>().text = currentNotches.ToString();
            // play player's marco voice line
            marcoCallout1.Play();

            // wait a bit, then play sound cues ("Polos")
            StartCoroutine(PlayThisLevelsSoundCues_AfterSetTime(2f));
        }
        else
        {
            print("empty notches!");
        }
    }

    //

    // Update is called once per frame
    void Update()
    {
        if (currentTimeDay == "day1" || currentTimeDay == "day2Good" || currentTimeDay == "day2Bad" )
        {
            // 0 = Left button
            if (Input.GetMouseButtonDown(0))
            {
                //print("mouse down fired!");
                TryToPlayNextDialouge();
            }
        }

        if (currentTimeDay == "night1" )
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnClickForMarcoButton();
            }
        }
        
    }



}

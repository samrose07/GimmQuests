/* This script was created by Samuel Rose for the GIMMStudio space, focused for quests.
 * 
 * The purpose of this script is to manage the interactivity for Jeanne's space.
 * The idea is to solve three questions based on 80s songs. A clip will play
 * and the player has to figure out the final word or phrase of the snippet.
 * After three correct answers, the prize will be awarded to them.
 * Songs used: immigrant song - led zeppelin
 *              take it easy - eagles
 *              gimme three steps - lynyrd skynyrd
 *              
 * Biodigital jazz, man
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JeanneManager : MonoBehaviour
{
    #region variables
    [SerializeField] private bool canGo = false;
    [SerializeField] private AudioClip isongQuestion;
    [SerializeField] private AudioClip isongAnswer;
    [SerializeField] private AudioClip takeItEasyQuestion;
    [SerializeField] private AudioClip takeItEasyAnswer;
    [SerializeField] private AudioClip gimmeThreeStepsQuestion;
    [SerializeField] private AudioClip gimmeThreeStepsAnswer;
    private AudioClip questionClipToUse;
    private AudioClip answerClipToUse;
    [SerializeField] private bool immigrantPlayed = false;
    [SerializeField] private bool takeItEasyPlayed = false;
    [SerializeField] private bool gimmeThreeStepsPlayed = false;
    [SerializeField] private TMP_Text leftText;
    [SerializeField] private TMP_Text centerText;
    [SerializeField] private TMP_Text rightText;
    [SerializeField] private TMP_Text doneText;
    private int correctText1 = 0;
    private int correctText2 = 0;
    private int correctText3 = 0;
    private int correctText = 0;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private GameObject playingPanel;
    [SerializeField] private GameObject falsePanel;
    [SerializeField] private GameObject correctPanel;
    private bool complete = false;
    [SerializeField] private GameObject artifactGlass;
    [SerializeField] private GameObject artifact;
    private enum Song
    {
        Immigrant,
        Take,
        Gimme,
        Nu
    }
    [HideInInspector] public int firstCorrectButton;
    [HideInInspector] public int secondCorrectButton;
    [HideInInspector] public int thirdCorrectButton;
    [SerializeField]private int correctButton;
    private Song currentSong;
    private List<Song> songList = new List<Song>();
    public AudioSource whereToPlay;

    #endregion

    #region start and update
    // Start is called before the first frame update
    void Start()
    {
        firstCorrectButton = Mathf.RoundToInt(Random.Range(1, 4));
        secondCorrectButton = Mathf.RoundToInt(Random.Range(1, 4));
        thirdCorrectButton = Mathf.RoundToInt(Random.Range(1, 4));
        correctText1 = firstCorrectButton;
        correctText2 = secondCorrectButton;
        correctText3 = thirdCorrectButton;
        songList.Add(Song.Immigrant);
        songList.Add(Song.Take);
        songList.Add(Song.Gimme);
        int curSong = Mathf.RoundToInt(Random.Range(1, 4));
        switch(curSong)
        {
            case 1:
                currentSong = Song.Immigrant;
                questionClipToUse = isongQuestion;
                answerClipToUse = isongAnswer;
                songList.Remove(Song.Immigrant);
                break;
            case 2:
                currentSong = Song.Take;
                questionClipToUse = takeItEasyQuestion;
                answerClipToUse = takeItEasyAnswer;
                songList.Remove(Song.Take);
                break;
            case 3:
                currentSong = Song.Gimme;
                questionClipToUse = gimmeThreeStepsQuestion;
                answerClipToUse = gimmeThreeStepsAnswer;
                songList.Remove(Song.Gimme);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(complete && artifactGlass.activeInHierarchy)
        {
            artifactGlass.SetActive(false);
            if(!artifact.GetComponent<Rigidbody>()) artifact.AddComponent<Rigidbody>();
        }
        if(canGo)
        {
            if (currentSong == Song.Nu)
            {
                correctPanel.SetActive(true);
                doneText.text = "YOU DID IT! Collect your artifact nearby!";
                complete = true;
            }
            switch (currentSong)
            {
                case Song.Immigrant:
                    correctButton = firstCorrectButton;
                    SetTexts(correctButton, Song.Immigrant);
                    questionClipToUse = isongQuestion;
                    answerClipToUse = isongAnswer;
                    immigrantPlayed = InitialSoundPlayed(questionClipToUse, immigrantPlayed);
                    break;
                case Song.Take:
                    correctButton = secondCorrectButton;
                    SetTexts(correctButton, Song.Take);
                    questionClipToUse = takeItEasyQuestion;
                    answerClipToUse = takeItEasyAnswer;
                    takeItEasyPlayed = InitialSoundPlayed(questionClipToUse, takeItEasyPlayed);
                    break;
                case Song.Gimme:
                    correctButton = thirdCorrectButton;
                    SetTexts(correctButton, Song.Gimme);
                    questionClipToUse = gimmeThreeStepsQuestion;
                    answerClipToUse = gimmeThreeStepsAnswer;
                    gimmeThreeStepsPlayed = InitialSoundPlayed(questionClipToUse, gimmeThreeStepsPlayed);
                    break;
            }
        }
        
    }
    #endregion

    #region created methods

    public void CanNowGo()
    {
        canGo = true;
    }
    private bool InitialSoundPlayed(AudioClip c, bool played)
    {
        //this method takes in an audioclip to play and checks if its been played already.
        //then sets the prospective played to true so it doesnt repeat.

        if (whereToPlay.isPlaying) return played;
        if(!played)
        {
            PlayClip(c);
            played = true;
            answerPanel.SetActive(false);
            playingPanel.SetActive(true);
            StartCoroutine(WaitForSound(whereToPlay, playingPanel));
        }
        return played;
    }

    /// <summary>
    /// Called when an answer button is pressed, Checks to see if the given answer is indeed, correct!
    /// </summary>
    /// <param name="pressedButton">Send the button that was pressed to check if it is the correct one.</param>
    /// <param name="nFLTRB">The button in numeric order from left to right.</param>
    public void AnswerGiven(GuessButtons pressedButton, int nFLTRB)
    {
        
        if(nFLTRB == correctButton)
        {
            CorrectAnswer();
            correctPanel.SetActive(true);
            StartCoroutine(FalseEnum(correctPanel));
        }
        else
        {
            print("else inside nFLTRB");
            ShowPanel(falsePanel);
            StartCoroutine(FalseEnum(falsePanel));
        }
    }

    /// <summary>
    /// Play an audio clip
    /// </summary>
    /// <param name="clip">the clip you want to play.</param>
    public void PlayClip(AudioClip clip)
    {
        whereToPlay.PlayOneShot(clip);
    }

    /// <summary>
    /// this method is called when a correct answer is pressed. In it,
    /// the script will check the current song and then mark it as played.
    /// Afterwhich it will do another RNG, setting the next song. If
    /// the number selects a state that has been played already, run the rng again.
    /// </summary>
    public void CorrectAnswer()
    {
        //play the clip of the answer here
        PlayClip(answerClipToUse);
        answerPanel.SetActive(false);
        //generates a new song and then removes that song from the list, so therefore it cannot be repeated.
        Song randSong = RandomGenerator();
        print("randSong is " + randSong);
        switch(randSong)
        {
            case Song.Immigrant:
                currentSong = Song.Immigrant;
                songList.Remove(Song.Immigrant);
                break;
            case Song.Take:
                currentSong = Song.Take;
                songList.Remove(Song.Take);
                break;
            case Song.Gimme:
                currentSong = Song.Gimme;
                songList.Remove(Song.Gimme);
                break;
            case Song.Nu:
                currentSong = Song.Nu;
                break;
        }
    }

    private Song RandomGenerator()
    {
        //lets pick a new song to use. If we don't have anymore, return the NULL state!
        Song ret;
        if(songList.Count <= 0)
        {
            return Song.Nu;
        }
        ret = songList[Mathf.RoundToInt(Random.Range(0, songList.Count))];

        return ret;
    }

    private IEnumerator WaitForSound(AudioSource source, GameObject panel)
    {
        //show the answers once the source is done playing the audio and hide the panel
        yield return new WaitUntil(() => source.isPlaying == false);
        ShowAnswers();
        HidePanel(panel);
    }

    private IEnumerator FalseEnum(GameObject panel)
    {
        //if false, wait for three seconds to hide the "false" panel
        yield return new WaitForSeconds(3);
        HidePanel(panel);
    }
    private void ShowAnswers()
    {
        //once the song is done playing we call this to show the possible choices
        answerPanel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        //set the sent panel to hidden
        panel.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        //set the sent panel to shown
        panel.SetActive(true);
    }
    private void SetTexts(int corButt, Song s)
    {
        //this method will set the texts related to each button to a certain option
        //to either the correct answer OR a false one that I choose to make.
        string songText = "";
        string wrongText = "";
        string wrongText2 = "";
        //this switch statement is weird.
        /* We base it off what the correct button is. Then we choose cases
         * of which song is playing based on the state, and picks the texts
         * in regards to that. FOR EXAMPLE:
         *      if the state indicates we are at immigrant song, and the left button is the correct one (as chosen in GuessButtons script for each button)
         *      we set the correct song text to *song text* and the other two to random answers.
         *      THEN we set the actual in-game UI texts to their respective texts. 
         *      This example is the first one shown below:
         */
        switch(corButt)
        {
            //left button
            case 3:
                switch(s)
                {
                    case Song.Immigrant:
                        songText = "From the midnight sun, where the hot springs flow";
                        wrongText = "Where my heart is cold covered in gold";
                        wrongText2 = "From the wolf howls that midnight oil";
                        break;
                    case Song.Gimme:
                        songText = "Lookin' for you know who";
                        wrongText = "Mad that I stole his boo";
                        wrongText2 = "Jealous I was too cool";
                        break;
                    case Song.Take:
                        songText = "Slowin' down to take a look at me";
                        wrongText = "Lookin' at all the sights to see";
                        wrongText2 = "Singin' me a little symphony";
                        break;
                }
                leftText.text = songText;
                centerText.text = wrongText;
                rightText.text = wrongText2;
                break;
            //center button
            case 2:
                switch (s)
                {
                    case Song.Immigrant:
                        songText = "From the midnight sun, where the hot springs flow";
                        wrongText = "Where my heart is cold covered in gold";
                        wrongText2 = "From the wolf howls that midnight oil";
                        break;
                    case Song.Gimme:
                        songText = "Lookin' for you know who";
                        wrongText = "Mad that I stole his boo";
                        wrongText2 = "Jealous I was too cool";
                        break;
                    case Song.Take:
                        songText = "Slowin' down to take a look at me";
                        wrongText = "Lookin' at all the sights to see";
                        wrongText2 = "Singin' me a little symphony";
                        break;
                }
                centerText.text = songText;
                leftText.text = wrongText;
                rightText.text = wrongText2;
                break;
            //rightButton
            case 1:
                switch (s)
                {
                    case Song.Immigrant:
                        songText = "From the midnight sun, where the hot springs flow";
                        wrongText = "Where my heart is cold covered in gold";
                        wrongText2 = "From the wolf howls that midnight oil";
                        break;
                    case Song.Gimme:
                        songText = "Lookin' for you know who";
                        wrongText = "Mad that I stole his boo";
                        wrongText2 = "Jealous I was too cool";
                        break;
                    case Song.Take:
                        songText = "Slowin' down to take a look at me";
                        wrongText = "Lookin' at all the sights to see";
                        wrongText2 = "Singin' me a little symphony";
                        break;
                }
                rightText.text = songText;
                centerText.text = wrongText;
                leftText.text = wrongText2;
                break;
        }
    }
    #endregion
}

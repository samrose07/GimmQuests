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

public class JeanneManager : MonoBehaviour
{
    #region variables

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
    [SerializeField] private Text leftText;
    [SerializeField] private Text centerText;
    [SerializeField] private Text rightText;
    private int correctText1 = 0;
    private int correctText2 = 0;
    private int correctText3 = 0;
    private int correctText = 0;
    [SerializeField] private GameObject answerPanel;
    [SerializeField] private GameObject playingPanel;
    [SerializeField] private GameObject falsePanel;
    [SerializeField] private GameObject correctPanel;
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

    /* TODO: Attach in game meaning to the buttons
     * 
     * 
     *  i.e. once first song is chosen, attach the
     *  audio clips to connected "ClipToUse" clips; DONE
     *  
     *  make a method to change Song and "clipTOUse" vars NOT NEEDED AND DONE
     *  
     *  make a method to play the clips DONE
     *  
     */

    // Update is called once per frame
    void Update()
    {
        if(currentSong == Song.Nu)
        {
            print("YOU DID IT YAYYYY");
        }
        switch(currentSong)
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

    //TODO:
    /*Public var for each button text
     * assign the text of each button when the correct button is selected
     * assign the text of each other button when the correct button is selected
     */


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
        yield return new WaitUntil(() => source.isPlaying == false);
        ShowAnswers();
        HidePanel(panel);
    }

    private IEnumerator FalseEnum(GameObject panel)
    {
        yield return new WaitForSeconds(3);
        HidePanel(panel);
    }
    private void ShowAnswers()
    {
        answerPanel.SetActive(true);
        //print("hey, i got here bro!");
    }

    private void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    private void SetTexts(int corButt, Song s)
    {
        string songText = "";
        string wrongText = "";
        string wrongText2 = "";
        switch(corButt)
        {
            //left button
            case 3:
                switch(s)
                {
                    case Song.Immigrant:
                        songText = "ImmigrantSong";
                        wrongText = "not immsong";
                        wrongText2 = "idek bro";
                        break;
                    case Song.Gimme:
                        songText = "GimmeSong";
                        wrongText = "not gimme";
                        wrongText2 = "idek hehe";
                        break;
                    case Song.Take:
                        songText = "Take song";
                        wrongText = "not take";
                        wrongText2 = "idek lmao";
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
                        songText = "ImmigrantSong";
                        wrongText = "not immsong";
                        wrongText2 = "idek bro";
                        break;
                    case Song.Gimme:
                        songText = "GimmeSong";
                        wrongText = "not gimme";
                        wrongText2 = "idek hehe";
                        break;
                    case Song.Take:
                        songText = "Take song";
                        wrongText = "not take";
                        wrongText2 = "idek lmao";
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
                        songText = "ImmigrantSong";
                        wrongText = "not immsong";
                        wrongText2 = "idek bro";
                        break;
                    case Song.Gimme:
                        songText = "GimmeSong";
                        wrongText = "not gimme";
                        wrongText2 = "idek hehe";
                        break;
                    case Song.Take:
                        songText = "Take song";
                        wrongText = "not take";
                        wrongText2 = "idek lmao";
                        break;
                }
                rightText.text = songText;
                centerText.text = wrongText;
                leftText.text = wrongText2;
                break;
        }
    }
}

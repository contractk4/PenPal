﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class MadlibsController : MonoBehaviour, ILetterController {

	public string cutScene;
	public TextMesh header;
    public Text[] labels;
	public InputField[] inputs;
    public TextMesh closing;
	public GameObject sendButton;
    private Queue<MadlibsEvent> madlibsQueue;
	private List<MadlibsEvent> madlibsHistory;
    private MadlibsEvent currentMLE;
	//Pray that we never change this... 
	public GraphicColorLerp fadeToBlack;
    public GraphicColorLerp fadeToTransparent;

    void Start () {
        madlibsQueue = GameManager.madLibsQueue;
		madlibsHistory = new List<MadlibsEvent>();

		header.text = buildHeader();
		PopulatePrompts();
    }

    // Update is called once per frame
    void Update () {
		
	}
    public string buildHeader()
    {
        return "Madlibs!";
    }
    /*public string buildClosing()
    {
        //this will probably be a "send/finish" button
    }*/

	void PopulatePrompts() {
		currentMLE = madlibsQueue.Dequeue();
		int labelIndex = 0;
		while (currentMLE.groupID != -1) { //my version of EOL
			madlibsHistory.Add(currentMLE);
			labels[labelIndex++].text = currentMLE.prompt;
			currentMLE = madlibsQueue.Dequeue();
		}
	}

	void PopulateResults() {
		string buf = "";
		for(int i = 0; i < madlibsHistory.Count; ++i) {
			buf+= madlibsHistory[i].text.Replace("{}", inputs[i].text);
		}
		GameManager.madLibsResults.Enqueue(buf);
		Debug.Log(buf);
	}
   public void clickSendButton() {
	   	PopulateResults();
        StartCoroutine(loadAfterDelay(cutScene,fadeToBlack.duration));
   }

   public IEnumerator loadAfterDelay(string sceneName, float delay)
    {
        fadeToBlack.startColorChange();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

	public LetterWritingController.State getState() {
		return LetterWritingController.State.LetterWriting;
	}
}
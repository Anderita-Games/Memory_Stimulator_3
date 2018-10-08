using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
	string[] Color_Names;
	Color32[] Colors;

	public UnityEngine.UI.Text Word;
	public UnityEngine.UI.Text Option_1;
	public UnityEngine.UI.Text Option_2;
	public UnityEngine.UI.Text Score_Text;
	public UnityEngine.UI.Text Time_Text;

	//Game Vars
	int Score;
	int Time_Started = 0;
	bool Game_Active = false;

	// Use this for initialization
	void Start () {
		Color_Names = new string[] {
			"Red", "Green", "Blue",
			"Orange", "Purple", "Yellow",
			"Pink", "Brown", "Black"
		};

		Colors = new Color32[] { //TODO?: http://www.color-hex.com/color-palette/20515
			new Color32(255,0,0,255), new Color32(0,255,0,255), new Color32(0,0,255,255),
			new Color32(255,165,0,255), new Color32(255,0,255,255), new Color32(255,255,0,255),
			new Color32(255,192,203,255), new Color32(165,42,42,255), new Color32(0,0,0,255)
		};

	}

	void Update () {
		Score_Text.text = "Score: " + Score;
		if (Game_Active == true) {
			Time_Text.text = "Time: " + (59 + Mathf.RoundToInt(Time_Started - Time.timeSinceLevelLoad));
			if (60 + (Time_Started - Time.timeSinceLevelLoad) <= 0) {
				Game_Active = false;
				Word.text = "GAMEOVER: NO TIME LEFT";
				Game_Over();
			}
		}
	}

	void Next () { //TODO: Add difficulty increase as user progresses
		int Color_Word = Random.Range(0, Color_Names.Length); //The word of the color
		int Color_Color = Random.Range(0, Colors.Length); //The color of the text

		Word.text = Color_Names[Color_Word].ToUpper();
		Word.color = Colors[Color_Color];

		int Random_Number = Random.Range(0,10);
		if (Random_Number == 0) {
			Option_1.text = Color_Names[Color_Word].ToUpper();
			Option_2.text = Color_Names[Color_Color].ToUpper();
		}else if (Random_Number == 1) {
			Option_2.text = Color_Names[Color_Word].ToUpper();
			Option_1.text = Color_Names[Color_Color].ToUpper();
		}else if (Random_Number > 5) {
			Option_1.text = Color_Names[Color_Color].ToUpper();
			Option_2.text = Color_Names[Random.Range(0, Color_Names.Length)].ToUpper();
		}else {
			Option_2.text = Color_Names[Color_Color].ToUpper();
			Option_1.text = Color_Names[Random.Range(0, Color_Names.Length)].ToUpper();
		}
	}

	public void Selected (UnityEngine.UI.Text Color) {
		if (Color.text == "RESTART") {
			SceneManager.LoadScene("Game");
		}
		if (Game_Active == false && Color.text == "BLACK" && Time_Started == 0) {
			Game_Active = true;
			Time_Started = Mathf.CeilToInt(Time.timeSinceLevelLoad);
			Score++;
			Next();
		}else if (Game_Active == true) {
			int i;
			for (i = 0; i < 9;i++) {
				if (Color.text == Color_Names[i].ToUpper()) {
					break;
				}
			}
			if (Word.color == Colors[i]) { //check if shit is gucci
				Score++;
				Next();
			}else { //It aint :(
				Game_Active = false;
				Word.text = "GAMEOVER: WRONG ANSWER";
				Game_Over();
			}
		}
	}

	void Game_Over () {
		if (Score > PlayerPrefs.GetInt("Highscore")) {
			PlayerPrefs.SetInt("Highscore", Score);
		}
		Time_Text.text =  "Highscore: " + PlayerPrefs.GetInt("Highscore");
		Option_1.text = "RESTART";
		Option_2.text = "RESTART";
	}
}

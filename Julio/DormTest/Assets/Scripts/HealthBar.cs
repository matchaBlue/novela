using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public static float health = 100;
	public static int medHealth;
	public LevelManager lv;
	public RectTransform healthBar;
	public static float maxHealth;

	void Start(){
		health = healthBar.sizeDelta.x;
		maxHealth = health;
		medHealth = 30;
		//health -= (health/10) * 9;
	}

	void Update(){
		healthControl();
	}

	int count = 0;
	void healthControl(){
		int barDivider = 10;

		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
		if(Time.frameCount%barDivider == 0){
			count += 1;
			if(count%barDivider == 0)
				health -= barDivider;
		}
		if(health <= 0){
			lv.LoadScene(0);
		}
	}

}
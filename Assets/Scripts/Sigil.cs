using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sigil : MonoBehaviour
{
	public Vector2 dir;
	public UnityEngine.UI.Image sigilSprite;

	public Sprite[] sigilSprites = new Sprite[9];
	private int _sigilDir = -1;

	public void UpdateSigilSprite() {
		int d = -1;
		if(dir.x > 0 && dir.y > 0) {
			d = 0;
		}
		else if(dir.x > 0 && dir.y == 0) {
			d = 1;
		}
		else if(dir.x > 0 && dir.y < 0) {
			d = 2;
		}
		else if(dir.x == 0 && dir.y < 0) {
			d = 3;
		}
		else if(dir.x == 0 && dir.y > 0) {
			d = 4;
		}
		else if(dir.x < 0 && dir.y > 0) {
			d = 5;
		}
		else if(dir.x < 0 && dir.y == 0) {
			d = 6;
		}
		else if(dir.x < 0 && dir.y < 0) {
			d = 7;
		}
		else {
			d = 8;
		}
		if(d != _sigilDir) {
			_sigilDir = d;
			sigilSprite.sprite = sigilSprites[d];
		}

	}
}
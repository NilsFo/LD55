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
		if(dir.y > 0.5 && Mathf.Abs(dir.x) < 0.5) {
			d = 0;
		}
		else if(dir.y < -0.5 && Mathf.Abs(dir.x) < 0.5) {
			d = 1;
		}
		else if(dir.x < -0.5 && 0.5 > Mathf.Abs(dir.y)) {
			d = 2;
		}
		else if(dir.x > 0.5 && 0.5 > Mathf.Abs(dir.y)) {
			d = 3;
		}
		else if(dir.x > 0 && dir.y < 0) {
			d = 5;
		}
		else if(dir.x < 0 && dir.y < 0) {
			d = 6;
		}
		else if(dir.x < 0 && dir.y > 0) {
			d = 7;
		}
		else if(dir.x > 0 && dir.y > 0) {
			d = 8;
		}
		else {
			d = 4;
		}
		if(d != _sigilDir) {
			_sigilDir = d;
			sigilSprite.sprite = sigilSprites[d];
		}

	}

	public static int GetIndex(Vector2 input)
	{
		int d = -1;
		
		if(input.y > 0.5 && Mathf.Abs(input.x) < 0.5) {
			d = 0;
		}
		else if(input.y < -0.5 && Mathf.Abs(input.x) < 0.5) {
			d = 1;
		}
		else if(input.x < -0.5 && 0.5 > Mathf.Abs(input.y)) {
			d = 2;
		}
		else if(input.x > 0.5 && 0.5 > Mathf.Abs(input.y)) {
			d = 3;
		}
		else if(input.x > 0 && input.y < 0) {
			d = 5;
		}
		else if(input.x < 0 && input.y < 0) {
			d = 6;
		}
		else if(input.x < 0 && input.y > 0) {
			d = 7;
		}
		else if(input.x > 0 && input.y > 0) {
			d = 8;
		}
		else {
			d = 4;
		}

		return d;
	}
	
}
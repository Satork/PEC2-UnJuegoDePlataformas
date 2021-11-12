using System;
using UnityEngine;

public class CoinController : MonoBehaviour {
	private QuestionBlockController _questionBlockController;

	private void Start() {
		_questionBlockController = FindObjectOfType<QuestionBlockController>();
	}
	
	
}

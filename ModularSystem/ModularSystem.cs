// Main script of the modular system
// Author: Heavyskymobile - Rozx
// Date: 2016-11-05
// Version 0.1a


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ModularSystem{
	
	[ExecuteInEditMode]
	public class ModularSystem : MonoBehaviour {
		
		// the random.seed value as int
		public int randomSeed;
		
		// the seed generate mode
		public RandomSeedMode mySeedMode = RandomSeedMode.Default;
		
		// the part generation mode
		public StartingMethod myStartMethod = StartingMethod.Awake;
		
		
		public List<PartSet> partSetList = new List<PartSet>();
		
		
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			
			
		}
	}
}
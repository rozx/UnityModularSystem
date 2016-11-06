// Defines modular classes here
// Author: Heavyskymobile - Rozx
// Date: 2016-11-05
// Version 0.1a

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModularSystem{

	
	
	[System.Serializable]
	public class PartSet{
		
		public bool isActivate;
		
		public string name;
		
		public Transform attachTransform;
		
		public List<Part> partList = new List<Part>();
	}
	
	
	
	[System.Serializable]
	public class Part{
		
		public string name;
		
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
		
		public GameObject prefab;
		
		// Spawn weight
		public int weight;
		
	}
	
	
	public enum StartingMethod{
		
		Awake, OnCall
	}
	
	
	public enum RandomSeedMode{
		
		Default, Manual, PositionBased
	}


}
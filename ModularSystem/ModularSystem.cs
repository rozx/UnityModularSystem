// Main script of the modular system
// Author: Heavyskymobile - Rozx
// Date: 2016-11-07
// Version 0.3


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace ModularSystem{
	
	
	public class ModularSystem : MonoBehaviour {
		
		// the random.seed value as int
		public int randomSeed;
		
		// the seed generate mode
		public RandomSeedMode mySeedMode;
		
		// the part generation mode
		public StartingMethod myStartMethod;
		
		public List<PartSet> partSetList = new List<PartSet>();
		
		[SerializeField]
		private List<Dictionary<Part, int>> weightedPartSetList = new List<Dictionary<Part, int>>();
		
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			
			if(weightedPartSetList.Count > 0) weightedPartSetList.Clear();
			
			// Initialize the weighted random list
			
			foreach(PartSet ps in partSetList.ToArray()){
				
				
				
				foreach (Part p in ps.partList.ToArray())
				{
					
					int index = partSetList.IndexOf(ps);

					//if(p.weight == 0) p.weight = 1;
					
					weightedPartSetList.Add(new Dictionary<Part,int>());
					weightedPartSetList[index].Add(p, p.weight);
					
				}
				
			}
			
			// Initialize the random seed
			
			
			switch(mySeedMode){
				
			case RandomSeedMode.Default:
				
				// do nothing when it's on default mode
				
				break;
				
			case RandomSeedMode.Manual:
				
				// use assigned random seed
				
				Random.InitState(randomSeed);
				
				
				
				break;
				
			case RandomSeedMode.PositionBased:
				
				// use position to generate seed
				
				int _randomSeed = transform.position.GetHashCode();
				
				Random.InitState(_randomSeed);
				
				break;
				
			}
			
		}
		
		// Use this for initialization
		void Start () {
			
			
			if(myStartMethod == StartingMethod.Awake){
				
				// generate on Start
				
				StartGenerate();
				
			}
		
		}
		
		
		
		public void StartGenerate(){
			
			// Start generating gameobejcts
			
			foreach(PartSet _ps in partSetList.ToArray()){
				
				if(!_ps.isActivate) return;
				
				
				int index = partSetList.IndexOf(_ps);
				Part _part = WeightedRandomizer.From(weightedPartSetList[index]).TakeOne();
				
				GameObject _gameobject = Instantiate(_part.prefab,_ps.attachTransform.position, Quaternion.Euler(_ps.attachTransform.localEulerAngles)) as GameObject;
				
				_gameobject.transform.SetParent(_ps.attachTransform,true);
				
				_gameobject.transform.position += _part.position;
				_gameobject.transform.localEulerAngles = _part.rotation;
				_gameobject.transform.localScale = _part.scale;
				
			}
			
		}
	}
}
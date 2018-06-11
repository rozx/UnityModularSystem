// Editor script of modular system
// Author: Heavyskymobile - Rozx
// Date: 2018-06-11
// Version 0.4



using UnityEngine;
using UnityEditor;
using UnityEditorInternal;  
using System.Collections.Generic;

namespace ModularSystem{

	[CustomEditor(typeof(ModularSystem))]
	public class ModularEditor : Editor{
		
		
		public ModularSystem modularSystem;
		
		private bool partSetFoldout = true;
		private List<ReorderableList> partLists = new List<ReorderableList>();
		
		private int drawingListIndex;
		private int partListIndex;
		private ReorderableList activateList;
		private GameObject lastPreviewGameObject;
		private Part selectingPart;
		private List<GameObject> previewGameObjects = new List<GameObject>();
		
		
		// This function is called when the object is loaded.
		protected void OnEnable()
		{
			
			modularSystem = (ModularSystem)target;

            // setup the listener for play state change

            EditorApplication.playModeStateChanged += OnPlayModeChange;
			
			UpdatePartList();
			
		}
		
		protected void OnDisable()
		{
			
			// remove the previewing gameobject
			
			if(lastPreviewGameObject) DestroyImmediate(lastPreviewGameObject);
			
			// remove all preview gameobjects
			
			ClearPreviewGameObjects();
			
		}

        void OnPlayModeChange(PlayModeStateChange playMode){

            // if playmode have changed to "play", clear the preview.

            switch(playMode){
                case PlayModeStateChange.ExitingEditMode:

                    // remove the previewing gameobject

                    if (lastPreviewGameObject) DestroyImmediate(lastPreviewGameObject);

                    // remove all preview gameobjects

                    ClearPreviewGameObjects();

                    break;
            }

        }
		
		void OnSceneGUI( )
		{
			// draw handles
			
			foreach(PartSet ps in modularSystem.partSetList.ToArray()){
				
				Handles.color = Color.blue;
				GUIStyle style = new GUIStyle();
				style.normal.textColor = Color.red;
				
				Handles.Label(ps.attachTransform.position, ps.name + "[" + ps.partList.Count + "]", style);
				Handles.DrawWireCube(ps.attachTransform.position,new Vector3(0.2f,0.2f,0.2f));
				
				if(activateList!= null && activateList.list == ps.partList){
				
					ps.attachTransform.position = Handles.PositionHandle(ps.attachTransform.position,Quaternion.identity);
				}
				
				
				
			}
		}
		
		
		
		public override void OnInspectorGUI()
		{
			
			//modularSystem = (ModularSystem)target;
			
			EditorGUILayout.BeginVertical();
			
			// display the basic information
			
			
			// display random seed options
			
			
			modularSystem.mySeedMode = (RandomSeedMode)EditorGUILayout.EnumPopup("Random Seed Mode",modularSystem.mySeedMode);
			
			
			switch(modularSystem.mySeedMode){
				
				
			case RandomSeedMode.Default:
				
				EditorGUILayout.HelpBox("Default seed mode is the mode that allow system and unity engine automatically generate the random seed.",MessageType.Info);
				
				
				break;

			case RandomSeedMode.Manual:
			
				EditorGUILayout.BeginHorizontal();
				
				modularSystem.randomSeed = EditorGUILayout.IntField("Random Seed:", modularSystem.randomSeed);
				
				if(GUILayout.Button("Randomize seed")){
					
					modularSystem.randomSeed = GetRandomizeSeed();
					
				}
				
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.HelpBox("Manual Seed mode allow you to manually configure the random seed.",MessageType.Info);
				
				
				break;
				
				
			case RandomSeedMode.PositionBased:
				
				EditorGUILayout.HelpBox("Position based mode allow the random seed generated based on the root gameobject's position, so a gameobject at the same position will get same random seed everytime.",MessageType.Info);
				
				
				break;
				
			}
			
			
			// begin area of starting mode
			
			modularSystem.myStartMethod = (StartingMethod)EditorGUILayout.EnumPopup("Generation Method:", modularSystem.myStartMethod);
			
			
			switch(modularSystem.myStartMethod){
				
			case StartingMethod.Awake:
				
				EditorGUILayout.HelpBox("Procedural generation will start at the Begining of the game.",MessageType.Info);
				
				break;
				
			case StartingMethod.OnCall:
				
				EditorGUILayout.HelpBox("Procedural generation will be executed when player called [ModuleSystem.StartGenerate] function.",MessageType.Info);
				
				break;
				
				
				
			}
			
			// show preview button
			
			if(GUILayout.Button("[ Preview ]")){
				
				Preview();
			}
			
			if(GUILayout.Button("[ Stop Preview ]")){
				
				ClearPreviewGameObjects();
			}
			
			
			EditorGUILayout.HelpBox("While previewing in the editor view, the seed mode will not be applied.",MessageType.Info);
			
			// begin area of part set settings
			
			partSetFoldout = EditorGUILayout.Foldout(partSetFoldout,"Part set settings");
		
			
			if(partSetFoldout){
			
					EditorGUILayout.HelpBox("Here is the area where you can set up procedure generated gameobjects.",MessageType.Info);
					
					// for each partset
					
					
					foreach(PartSet ps in modularSystem.partSetList.ToArray()){
						
						
						ps.isActivate = EditorGUILayout.BeginToggleGroup(ps.name,ps.isActivate);
						
						if(ps.isActivate){
						
						// indivudial partset settings
						
						ps.name = EditorGUILayout.TextField("Part Set Name:", ps.name);
						
						ps.attachTransform = (Transform)EditorGUILayout.ObjectField("Attached Transform: ",ps.attachTransform,typeof(Transform),true);
							
							if(ps.attachTransform && ps.attachTransform.gameObject){
								
								if(ps.name.Contains("new Part Set") || ps.name == "") ps.name = ps.attachTransform.gameObject.name;
								
							}
							
						
						EditorGUILayout.Separator();
						
						// start of part settings
						
						int index = modularSystem.partSetList.IndexOf(ps);
						
						drawingListIndex = index;
						
						partLists[index].DoLayoutList();
						
							
						// Duplicate part set
							
							if(GUILayout.Button("Duplicate Part Set:[" + ps.name + "]")){
							
								if (EditorUtility.DisplayDialog("Duplicate Item", "Do you really want to create a new copy of [" + ps.name + "]?", "Yes","Cancel"))
								{
									DuplicatePartSet(ps);
								}
							
						}
						
						
						// delete part set button
						
						if(GUILayout.Button("Delete Part Set:[" + ps.name + "]")){
							
							if (EditorUtility.DisplayDialog("Delete Item", "Do you really want to delete [" + ps.name + "]?", "Yes","Cancel"))
							{
								DeletePartSet(ps);
							}
							
						}
					}
					
					
					EditorGUILayout.EndToggleGroup();
					
					EditorGUILayout.Separator();

					
				}
				
				
				// button of create new part set
				
				EditorGUILayout.Separator();
				
				if(GUILayout.Button("Create New Part Set")){
					
					CreatePartSet();
					
				}
				
			}
			
			
			
			EditorGUILayout.EndVertical();
			
			// update the preview gameobject
			
			if(lastPreviewGameObject) {
				

				
				//Transform attachTransform = modularSystem.partSetList[drawingListIndex].attachTransform;
				lastPreviewGameObject.transform.localPosition = selectingPart.position;
				lastPreviewGameObject.transform.localScale = selectingPart.scale;
				lastPreviewGameObject.transform.localEulerAngles = selectingPart.rotation;
				
				
				
			}
			
			
			Repaint();
			
			if(GUI.changed) 
			{

				EditorUtility.SetDirty(target);
			}
		}
		
		
		public int GetRandomizeSeed(){
			
			
			
			Random.InitState(System.DateTime.Now.GetHashCode());
			
			int seed = Mathf.RoundToInt( Random.Range(-2147483648,2147483648));

			if(seed == modularSystem.randomSeed) seed = GetRandomizeSeed();
			
			return seed;
			
		}
		
		
		public void CreatePartSet(){
			
			PartSet newPartset = new PartSet();
			
			newPartset.isActivate = true;
			newPartset.name = "new Part Set";
			
			modularSystem.partSetList.Add(newPartset);
			
			UpdatePartList();
			
		}
		
		public void DeletePartSet(PartSet ps){
			
			modularSystem.partSetList.Remove(ps);
			
			UpdatePartList();
			
		}
		
		
		public void DuplicatePartSet(PartSet ps){
			
			PartSet _ps = new PartSet();
			
			_ps.attachTransform = ps.attachTransform;
			_ps.isActivate = ps.isActivate;
			_ps.name = ps.name;
			
			foreach (Part item in ps.partList)
			{
				
				_ps.partList.Add(item);
				
			}
			
			
			modularSystem.partSetList.Add(_ps);
			
			UpdatePartList();
			
			
		}
		
		
		public void CreateNewPart(PartSet ps){
			
			Part newPart = new Part();
			
			newPart.name = "NewPart[" + ps.partList.Count.ToString() + "]";
			newPart.position = Vector3.zero;
			newPart.rotation = Vector3.zero;
			newPart.scale = Vector3.one;
			
			newPart.weight = 0;
			
			ps.partList.Add(newPart);
			
			
		}
		
		
		public void CreateNewPart(ReorderableList rl){
			
			Part newPart = new Part();
			
			newPart.name = "NewPart[" + rl.list.Count.ToString() + "]";
			newPart.position = Vector3.zero;
			newPart.rotation = Vector3.zero;
			newPart.scale = Vector3.one;
			
			newPart.weight = 1;
			
			rl.list.Add(newPart);
			
			
		}
		
		
		public void DeletePart(Part p, PartSet ps){
			
			ps.partList.Remove(p);
			
		}
		
		public void SelectPartElement(ReorderableList list){
			
			ClearPreviewGameObjects();
			
			int index = list.index;
			activateList = list;
			selectingPart = (Part)list.list[index];
			Transform attachTransform = modularSystem.partSetList[drawingListIndex].attachTransform;
			
			//Debug.Log(selectingPart.rotation);
			
			
			
			if(selectingPart.prefab && attachTransform){
				
				if(lastPreviewGameObject){
					
					DestroyImmediate(lastPreviewGameObject);
					
				}
				
				// preview the prefab
				
				GameObject previewGameObject = Instantiate(selectingPart.prefab,attachTransform.position,Quaternion.identity) as GameObject;
				
				previewGameObject.transform.SetParent(attachTransform,true);
				
				previewGameObject.transform.localPosition += selectingPart.position;
				previewGameObject.transform.localEulerAngles = selectingPart.rotation;
				previewGameObject.transform.localScale = selectingPart.scale;
				
				
				
				lastPreviewGameObject = previewGameObject;
				
			}
			
			
			
		}
		
		public void DrawHeader(Rect rect){
			
			EditorGUI.LabelField(new Rect(rect.x, rect.y , rect.width , 15), "Part Set: " + modularSystem.partSetList[drawingListIndex].name);
			
		}
		
		
		public void DrawPartElement(Rect rect, int index, bool isActive, bool isFocused){
			
			// Draw each element in the list
			
			Part p = modularSystem.partSetList[drawingListIndex].partList[index];
			
			p.name = EditorGUI.TextField(new Rect(rect.x + 10, rect.y + 5, rect.width - 18, 15), "Name: ",p.name);
			p.position = EditorGUI.Vector3Field(new Rect(rect.x + 10, rect.y + 30, rect.width - 18, 20), "Position:", p.position);
			p.rotation = EditorGUI.Vector3Field(new Rect(rect.x + 10, rect.y + 50, rect.width - 18, 20), "Rotation:", p.rotation);
			p.scale = EditorGUI.Vector3Field(new Rect(rect.x + 10, rect.y + 70, rect.width - 18, 20), "Scale:", p.scale);
			p.weight = EditorGUI.IntField(new Rect(rect.x + 10, rect.y + 90, rect.width - 18, 15),"Random Weight:",p.weight);
			
			p.prefab = (GameObject)EditorGUI.ObjectField(new Rect(rect.x + 10, rect.y + 110, rect.width - 18, 15),"Part Prefab:",p.prefab,typeof(GameObject),true);
			
			if(p.prefab && p.name.Contains("NewPart") || p.name == "") p.name = p.prefab.name;
		}
		
		
		public void UpdatePartList(){
			
			partLists.Clear();
			
			foreach(PartSet ps in modularSystem.partSetList.ToArray()){
				
				
				partLists.Add(new ReorderableList(ps.partList,typeof(Part), true, true, true, true));
				
				int index = modularSystem.partSetList.IndexOf(ps);
				
				
				partLists[index].elementHeight = 130f;
				
				// list drawing events
				partLists[index].drawElementCallback = DrawPartElement;
				partLists[index].onAddCallback = CreateNewPart;
				partLists[index].drawHeaderCallback = DrawHeader;
				partLists[index].onSelectCallback = SelectPartElement;
				
			}
			
		}
		
		
		public void ClearPreviewGameObjects(){
			
			if(previewGameObjects.Count > 0) {
			
			
			foreach(GameObject item in previewGameObjects.ToArray()){
				
				DestroyImmediate(item);
				
				
			}
			
			
				previewGameObjects.Clear();
			}
			
		}
		
		
		public void Preview(){
			
			
			// clear the preview first
			
			ClearPreviewGameObjects();
			
			GameObject _preview = new GameObject();
			
			_preview.name = "_preview";
			
			
			
			_preview.transform.SetParent(modularSystem.gameObject.transform,false);

			
			
			
			foreach (PartSet _ps in modularSystem.partSetList.ToArray())
			{
				
				if(_ps.isActivate){
				
					// for each partset, spawn gameobjects
					
					int index = Mathf.RoundToInt(Random.Range(0,_ps.partList.Count));
					
					GameObject _part = Instantiate(_ps.partList[index].prefab,Vector3.zero, Quaternion.Euler(_ps.attachTransform.localEulerAngles)) as GameObject;
					
					_part.transform.SetParent(_preview.transform,false);
					
					_part.transform.position = _ps.attachTransform.position + _ps.partList[index].position;
					_part.transform.localEulerAngles += _ps.partList[index].rotation;
					_part.transform.localScale = _ps.partList[index].scale;
					
					
					
					
					
					previewGameObjects.Add(_part);
				}
				
			}
			
			previewGameObjects.Add(_preview);
			
		}
		
		
		
	}
}

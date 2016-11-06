// Editor script of modular system
// Author: Heavyskymobile - Rozx
// Date: 2016-11-05
// Version 0.1a



using UnityEngine;
using UnityEditor;
using UnityEditorInternal;  
using System.Collections.Generic;

namespace ModularSystem{

	[CustomEditor(typeof(ModularSystem))]
	public class ModularEditor : Editor{
		
		
		public ModularSystem modularSystem;
		
		private bool partSetFoldout;
		private List<ReorderableList> partLists = new List<ReorderableList>();
		
		private int drawingListIndex;
		
		// This function is called when the object is loaded.
		protected void OnEnable()
		{
			
			modularSystem = (ModularSystem)target;
			
			UpdatePartList();
			
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
					
					
					
				}
				
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.HelpBox("Manual Seed mode allow you to manually configure the random seed.",MessageType.Info);
				
				
				break;
				
				
			case RandomSeedMode.PositionBased:
				
				EditorGUILayout.HelpBox("Position based mode allow the random seed generated based on the root gameobject's position, so a gameobject at the same position will get same random seed everytime.",MessageType.Info);
				
				
				break;
				
			}
			
			
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
					
					EditorGUILayout.Separator();
					
					// start of part settings
					
					int index = modularSystem.partSetList.IndexOf(ps);
					
					drawingListIndex = index;
					
					partLists[index].DoLayoutList();
					

					
					// button of new part
					
					if(GUILayout.Button("New Part")){
						
						CreateNewPart(ps);
						
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
			
			Repaint();
			
		}
		
		
		public int GetRandomizeSeed(){
			
			return 0;
			
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
		
		
		public void CreateNewPart(PartSet ps){
			
			Part newPart = new Part();
			
			newPart.name = "NewPart[" + ps.partList.Count.ToString() + "]";
			newPart.position = Vector3.zero;
			newPart.rotation = Vector3.zero;
			newPart.scale = Vector3.zero;
			
			newPart.weight = 0;
			
			ps.partList.Add(newPart);
			
			
		}
		
		
		public void CreateNewPart(ReorderableList rl){
			
			Part newPart = new Part();
			
			newPart.name = "NewPart[" + rl.list.Count.ToString() + "]";
			newPart.position = Vector3.zero;
			newPart.rotation = Vector3.zero;
			newPart.scale = Vector3.zero;
			
			newPart.weight = 0;
			
			rl.list.Add(newPart);
			
			
		}
		
		
		public void DeletePart(Part p, PartSet ps){
			
			ps.partList.Remove(p);
			
		}
		
		public void SelectPartElement(ReorderableList list){
			
			
			
		}
		
		public void DrawHeader(Rect rect){
			
		}
		
		
		public void DrawPartElement(Rect rect, int index, bool isActive, bool isFocused){
			
			Part p = modularSystem.partSetList[drawingListIndex].partList[index];
			
			p.name = EditorGUI.TextField(new Rect(rect.x + 10, rect.y + 5, rect.width - 18, 15), "Name: ",p.name);
			p.position = EditorGUI.Vector3Field(new Rect(rect.x + 10, rect.y + 30, rect.width - 18, 20), "Position:", p.position);
			p.rotation = EditorGUI.Vector3Field(new Rect(rect.x + 10, rect.y + 50, rect.width - 18, 20), "Rotation:", p.rotation);
			p.scale = EditorGUI.Vector3Field(new Rect(rect.x + 10, rect.y + 70, rect.width - 18, 20), "Scale:", p.scale);
			p.weight = EditorGUI.IntField(new Rect(rect.x + 10, rect.y + 90, rect.width - 18, 15),"Random Weight:",p.weight);
			
			p.prefab = (GameObject)EditorGUI.ObjectField(new Rect(rect.x + 10, rect.y + 110, rect.width - 18, 15),"Part Prefab:",p.prefab,typeof(GameObject),true);
			
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
				
			}
			
		}
		
		
		
	}
}

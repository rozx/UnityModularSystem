# Unity Modular System
Unity Modular system let's you create game object with procedural generated parts. 

With editable random seed mode and starting method, it provides the user unlimited possiablity.

No coding skill required.

# How to use it?

1. First, attach the modular system script to the gameobject (as root) you want to have random procedural generated parts.
2. Create some empty gameobjects under the root gameobject you just created, those are the basic position your procedural generated parts will based on.
3. In the inspector, under the modular system script, create some partsets and drag those empty gameobjects to the "attachTransform".
4. Add prefabs for each part sets.
5. Adjust them individually.
6. You can press "Preview" to preview the final product.
7. Click on individual part will let you preview the part, drag will adjust the postion of attach transform. You can modify the position, rotation and scale for each parts individually.

# Screenshots

- Editor View

![Editor View](https://raw.githubusercontent.com/rozx/UnityModularSystem/master/Screenshots/editor.PNG)

- Example Preview

![Example](https://raw.githubusercontent.com/rozx/UnityModularSystem/master/Screenshots/preview.PNG)

# Update Logs

##2016-11-07 V0.3
- Done: Attached transform handlers & lable
		Modular system script(Now it can be generated when scene started).
		Weight based randomizer.
		Random seed mode and starting method are working properly.

##2016-11-06 V0.2
- Done: Generate preview in editor(Can be previewed with either singel or all part(s))
		Example scene using free asset from asset store.

##2016-11-05 V0.1b
- Done: Name is now based on gameobjct's name if user hasn't define it.
		Random Seed manually generate.
		Starting method configuration.

##2016-11-05 V0.1a

- Done: Basic classes defined.
		Editor Window complete.
- To Do: Monobehaviour script.

Maze Factory
By Atomic Crew

How to use:
 - Create an empty game object, let's rename it to MyMaze
 - Drag the Maze Factory > MazeFactory script onto the MyMaze object
 - Create a wall element (a cube, mesh, trees etc.) and make sure it has a collider attached
 - Drag the wall element to the "Block" property of MyMaze object in the inspector
 - Check "preview in editor" propery in the inspector
 - Click "update in editor" in the inspector
 - You now have your new maze available in the editor
 - Check "random seed" in the inspector to randomize between updates or when you press play
 
Suggestion: if you like to exchange levels, save your game or create a multiplayer game where you want your maze to match your opponents:
Leave random seed disabled and use your widthCells + heightCells + seed value as your level identifier.
 
Suggestion 2: if you would like your entrance and exit point to remain at the same location to fit a static scene with a random maze for instance:
Check use "fixed begin end" in the inspector and specify with "fixed begin y" and "fixed end y" where those points are.

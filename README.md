# VoxelTerrainUnity
a monobehavior with 2 other scripts that allow you to generate voxel based terrains in unity. and it can be ported to other engines if you wanted.

# please read
licensed under MIT License
please credit the author (Mylo) by including a link to [their github](https://github.com/gitmylo/)

# how to use
1. Create an empty gameobject, this will be your world manager. add the World generator script to it
2. Create another empty gameobject anywhere, name it "chunk" (without quotes) and add a mesh filter and mesh renderer to it, and optionally also a meshcollider if you want collision
3. Drag the chunk gameobject into a folder like for example a prefabs folder
4. Now select the world manager gameobject and set "chunk object" to your chunk prefab.
5. Feel free to play around with the other options. then start the game, and it will generate a voxel based terrain in one mesh per chunk. you can also press the enter key to regenerate the terrain. the terrain will be stored inside of the world manager

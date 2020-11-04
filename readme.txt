# Simple A* Pathfinding Algorithm

Class `PathFinder` is not a MonoBehavior class. You need to use it in your MonoBehavior class. PathFinder requires size (e.g Capsules's collider radius) to calculate collision while finding the paths. And also `LayerMask` obstacles.
PathFinder class doesn't require you to have grid. It generates nodes while searching the path. And it will keep in mind size of the object you have provided.

### Todos
- [ ] Implement MinHeap to find node with lowest score
- [ ] Implement smooth moving along the shortest paths
- [ ] Implement Y axis
- [ ] Implement axis freeze (object can move only X|Y)
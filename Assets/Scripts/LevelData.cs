using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public int level;
    public List<CellPosition> cells;  
    public float rotationAngle;
}

[Serializable]
public class CellPosition
{
    public int x;
    public int y;
    public string type;
    public List<int> targetRotations;
}

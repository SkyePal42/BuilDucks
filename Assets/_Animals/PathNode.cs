public class PathNode
{

    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;

    public PathNode(int x, int y)
    {
        Initialise(x,y,true);
    }
    public PathNode(int x, int y, bool Walkable)
    {
        Initialise(x,y,Walkable);
    }

    public void Initialise(int x, int y, bool Walkable)
    {
        this.x = x;
        this.y = y;
        isWalkable = Walkable;
        gCost = 99999999;
        CalculateFCost();
        cameFromNode = null;
    }

    public string PrintString() {
        if (cameFromNode != null) return cameFromNode.PrintString() + "to" + x.ToString() + ":" + y.ToString();
        return x.ToString() + ":" + y.ToString();
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
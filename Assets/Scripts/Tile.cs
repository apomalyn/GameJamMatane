public struct Tile{

    public readonly int x;
    public readonly int y;
    
    public Tile(int x, int y){
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj){
        if (obj.GetType() == typeof(Tile))
            return ((Tile) obj).x == x && ((Tile) obj).y == y;
        return false;
    }

    public override string ToString(){
        return x + ":" + y;
    }
}
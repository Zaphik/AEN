namespace Panda.MGModel;

// idk why i have this but it felt wrong to have 3 stuff doing the same thing without a base
public interface ISpawner
{
    int Side { get; set; }
    int VertexSidePos { get; set; }
    int HorizonSidePos { get; set; }

    GameServiceContainer Services { get; set; }
    void Update(Vector2 OFFSET, Grid GRID);
    void SpawnMob(Vector2 OFFSET, Grid GRID);
}
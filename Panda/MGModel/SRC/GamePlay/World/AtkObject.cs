namespace Panda.MGModel;

public class AtkObject : AnimatedSprite
{
    private bool dead;

    public bool Dead
    {
        get => dead;
        protected set => dead = value;
    }


    private bool killed;

    public bool Killed
    {
        get => killed;
        private set => killed = value;
    }

    // The velocity of the attack object
    public float velo { get; protected set; }

    // The current health and the max health of the attack object
    public float currHealth { get; protected set; }
    public float maxHealth { get; protected init; }

    // The hit distance of the attack object
    public float hitDist;


    public AtkObject(string PATH, Vector2 POS, Vector2 SIZE, int ROWS, int COLUMNS, int TICK,
        GameServiceContainer SERVICES) : base(PATH, POS, SIZE, ROWS, COLUMNS, TICK, SERVICES)
    {
        // Sets the values of the attack object
        Dead = false;


        velo = 2.0f;

        hitDist = 35.0f;

        currHealth = 1;
        maxHealth = currHealth;
    }


    public virtual void Update(Vector2 OFFSET)
    {
        base.Update();
    }


    public virtual void GetHit(float DAMAGE)
    {
        // Decreases the health of the attack object and checks if it is dead
        currHealth -= DAMAGE;
        if (currHealth <= 0)
        {
            Dead = true;
            Killed = true;
        }
    }

    public override void Draw(Vector2 OFFSET, GameServiceContainer SERVICES)
    {
        base.Draw(OFFSET, SERVICES);
    }
}
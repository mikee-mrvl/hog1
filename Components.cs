using System;
using System.Collections.Generic;

abstract class Component
{
    public Entity Container { get; set; }
    public virtual void Update() { }
    public virtual void Added(Component component)
    {
        //if added, adjust the speed
        //once it's served its purpose, restore original speed and remove the component

        //(This is talking about the runfaster)

    }
}

//speed is a variable, container.X-speed
//does speed go in the container?
//container.speed -- speed in x and y direction??
//opposite of a power up?? slow speed/no speed

class KeyboardMoverComponent : Component
{
    public override void Update()
    {
        Console.WriteLine("");
        Console.WriteLine("Your Location is: ({0}, {1}). WASD?", Container.X, Container.Y);
        char direction = char.ToLower(Console.ReadKey(true).KeyChar);
        Console.WriteLine("");
        if (Container.mySpeedRemaining != 0)
        {
            Container.mySpeedRemaining--;
        }
        else
        {
            Container.mySpeed = 1;
        }
        switch (direction)
        {
            //TBD so I updated this, hopefully this is an okay way to do all this?

            case '4':
            case 'a':
                Container.X -= Container.mySpeed;
                break;
            case '6':
            case 'd':
                Container.X += Container.mySpeed;
                break;
            case '2':
            case 's':
                Container.Y -= Container.mySpeed;
                break;
            case '8':
            case 'w':
                Container.Y += Container.mySpeed;
                break;
        }
    }
}

class ChasePlayerComponent : Component
{
    Entity victim;

    public ChasePlayerComponent(Entity victim)
    {
        this.victim = victim;
    }
    public override void Update()
    {
        int xDelta = victim.X - Container.X;
        int yDelta = victim.Y - Container.Y;

        Func<int, int> direction = value => value / Math.Abs(value);
        if (Math.Abs(xDelta) > Math.Abs(yDelta))
        {
            Container.X += direction(xDelta);
        }
        else
        {
            Container.Y += direction(yDelta);
        }
    }
}

class RenderComponent : Component
{
    public override void Update()
    {
        Console.WriteLine("{0} is at: ({1}, {2})", Container.Name, Container.X, Container.Y);
    }


}

class MapComponent : Component
{
    char[,] map;
    List<Entity> gameObjects;
    Entity ourself;
    int maxX;
    int maxY;
    public MapComponent(List<Entity> gameObjects, Entity ourself, int maxX, int maxY)
    {
        this.maxX = maxX;
        this.maxY = maxY;
        this.gameObjects = gameObjects;
        this.ourself = ourself;
        map = new char[maxX, maxY];
        this.Update();
    }
    public override void Update()
    {
        for (int x=0;x<maxX;++x)
        {
            for (int y = 0; y < maxY; ++y)
            {
                map[x, y] = '-';
            }
        }
        foreach (Entity e in gameObjects)
        {
            if (!e.Equals(this.ourself))
            {
                map[e.X, e.Y] = e.Name[0];
            }
        }
        for (int y = maxY-1; y >= 0; --y)
        {
            for (int x = 0; x < maxX; ++x)
            {
                Console.Write("{0} ", map[x, y]);
            }
            Console.WriteLine("");
        }
    }


}

class KeepInBoundsComponent : Component
{
    int maxX;
    int maxY;
    bool wrap;
    public KeepInBoundsComponent(int maxX, int maxY, bool wrap = false)
    {
        this.maxX = maxX;
        this.maxY = maxY;
        this.wrap = wrap;
    }
    public override void Update()
    {
        if (Container.X < 0)
        {
            Container.X = (wrap) ? maxX-1 : 0;
        }
        else if (Container.X >= maxX)
        {
            Container.X = (wrap) ? 0 : maxX -1;
        }
        if (Container.Y < 0)
        {
            Container.Y = (wrap) ? maxY-1 : 0;
        }
        else if (Container.Y >= maxY)
        {
            Container.Y = (wrap) ? 0 : maxY -1;
        }
    }
}

class ContactDetectorComponent : Component
{
    //Entity target;
    //public List<Entity> targetList = new List<Entity> ();
    //public ContactDetectorComponent(Entity target)
    //{
    //    //this.targetList.Add(target);
    //    this.target = target;
    //}

    //public override void Update()
    //{
    //    foreach(Entity target in targetList)
    //    {
    //        InContact = Container.X == target.X && Container.Y == target.Y;
    //        if (InContact)
    //        {
    //            break;
    //        }
    //    }
    //}
    public bool InContact(Entity target)
    {
        return Container.X == target.X && Container.Y == target.Y;
    }
    //public void Add(Entity target2)
    //{
    //    this.targetList.Add(target2);
    //}
    //
    //public bool InContact { get; private set; }
}

class SpeedComponent : Component
{
    public float speed { get; set; }

    public SpeedComponent(int mySpeed)
    {
        speed = mySpeed;
    }
}
class SpecialPowerHandoffComponent : Component
{
    Component theSpecialPower;
    Entity target;
    ContactDetectorComponent detector;

    public SpecialPowerHandoffComponent(Entity target, Component theSpecialPower, ContactDetectorComponent detector)
    {
        this.target = target;
        this.theSpecialPower = theSpecialPower;
        this.detector = detector;
    }
    //MAKE A SPEED COMPONENT
    public override void Update()
    {
        //ContactDetectorComponent detector = Container.GetComponent<ContactDetectorComponent>();
        if (detector.InContact(target))
        {
            //target.AddFirstComponent(theSpecialPower);
            theSpecialPower.Update();
        }
    }
}

class RunFasterComponent : Component
{
    Entity target;
    int howFast;
    int howLong;
    public RunFasterComponent(Entity target, int howFast, int howLong)
    {
        this.target = target;
        this.howFast = howFast;
        this.howLong = howLong;
    }
    public override void Update()
    {
        target.mySpeed = howFast;
        target.mySpeedRemaining = howLong;
    }
}

class KillOnContactComponent : Component
{
    List<Entity> targetList = new List<Entity>();
    ContactDetectorComponent detector;
    //TBD Who contains this? The killer or the thing being killed?

    public KillOnContactComponent(Entity target, ContactDetectorComponent detector)
    {
        this.targetList.Add(target);
        this.detector = detector;
    }
    public void Add(Entity target2)
    {
        this.targetList.Add(target2);
    }
    public override void Update()
    {
        //TBD I don't understand how this detector knows what it's detecting??
        //But it is what he said to do...

        //ContactDetectorComponent detector = Container.GetComponent<ContactDetectorComponent>();
        //if (detector == null)
        //    return;
        foreach(Entity target in targetList)
        {
            if (detector.InContact(target))
            {
                Console.WriteLine("{0} collided with {1} -- GAME OVER!!!", target.Name, Container.Name);
                char.ToLower(Console.ReadKey(true).KeyChar);
                Environment.Exit(0);
                //TBD Is it SIMPLE to kill the player and make this decision in a different place? probably not...
            }
        }
    }
}

class TurnCounterComponent : Component
{
    int myCount = 0;
    int maxCount = 0;
    public int turnCount { get { return myCount; } }
    public TurnCounterComponent(int turns)
    {
        this.maxCount = turns;
    }
    public override void Update()
    {
        myCount++;

        if(myCount == maxCount)
        {
            Console.WriteLine("You won!!! Game over.");
            char.ToLower(Console.ReadKey(true).KeyChar);
            Environment.Exit(0);
        }
    }
}

class RandomRamblerComponent : Component
{
    Random rand = new Random();
    public override void Update()
    {
        int direction = rand.Next(0, 4);
        switch (direction)
        {
            case 0:
                Container.X -= Container.mySpeed;
                break;
            case 1:
                Container.X += Container.mySpeed;
                break;
            case 2:
                Container.Y -= Container.mySpeed;
                break;
            case 3:
                Container.Y += Container.mySpeed;
                break;
        }
    }
}
//At least need TWO monsters.
//Player's turn, then monsters' turn
//Use the design pattern to implement the game
//Add one or two fun things to make it FUN

using System;
using System.Collections.Generic;

class Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public int mySpeed { get; set; }  // might be nice to be float, so bad guys could be diff speeds: 1.5 fast, or .8 slower..
    public string Name { get; set; }
    public int mySpeedRemaining;

    //list that gets created during updates, if they were done then
    //they would add themselves to a destroy list
    List<Component> components = new List<Component>();
    //public List<Component> destroyList = new List<Component>();
    public void Update()
    {
        foreach(Component component in components)
        {
            component.Update();
        }
        //TBD check list of components to see if clean up is needed
        //each component can have a field that says remove me?
        //another foreach on destroy list and remove from component list

        //foreach(Component component in destroyList)
        //{
        //    components.Remove(component);
        //}
    }

    public void AddComponent(Component component)
    {
        component.Container = this;
        components.Add(component);
        //component.Added();
    }
    //public void RemoveComponent(Component component)
    //{
    //    components.Remove(component);
    //}
    //
    //public void AddFirstComponent(Component component)
    //{
    //    component.Container = this;
    //    components.Insert(0, component);
    //    //component.Added();
    //}

    //public void Destroy(Component component)
    //{
    //    destroyList.Add(component);
    //}
    public T GetComponent<T>() where T : Component
    {
        foreach(Component component in components)
        {
            if (component.GetType().Equals(typeof(T)))
                return (T)component;
        }
        return null;

    }
}

class MainClass
{
    static void Main()
    {
        int map_x = 20;
        int map_y = 20;
        int game_turns = 50;

        Random rand = new Random();
        int thisX = rand.Next(0, map_x);
        int thisY = rand.Next(0, map_y);
        Entity player = new Entity { X = thisX, Y = thisY, mySpeed = 1, Name = "Hermione"};

        thisX = rand.Next(0, map_x);
        thisY = rand.Next(0, map_y);
        Entity bellatrix = new Entity { X = thisX, Y = thisY, mySpeed = 1, Name = "BELLATRIX" };

        thisX = rand.Next(0, map_x);
        thisY = rand.Next(0, map_y);
        Entity voldemort = new Entity { X = thisX, Y = thisY, mySpeed = 1, Name = "VOLDEMORT" };

        thisX = rand.Next(0, map_x);
        thisY = rand.Next(0, map_y);
        Entity randGhost = new Entity { X = thisX, Y = thisY, mySpeed = 1, Name = "GHOST" };
        Entity map = new Entity { Name = "map" };

        //TBD possibly change how locations are initialized

        player.AddComponent(new KeyboardMoverComponent());
        player.AddComponent(new KeepInBoundsComponent(map_x, map_y, true));
        ContactDetectorComponent detector = new ContactDetectorComponent();
        player.AddComponent(detector);
        KillOnContactComponent killComponent = new KillOnContactComponent(bellatrix,detector);
        killComponent.Add(voldemort);
        killComponent.Add(randGhost);
        player.AddComponent(killComponent);
        player.AddComponent(new TurnCounterComponent(game_turns));
        player.AddComponent(new RenderComponent());

        bellatrix.AddComponent(new ChasePlayerComponent(player));
        bellatrix.AddComponent(new KeepInBoundsComponent(map_x, map_y));
        detector = new ContactDetectorComponent();
        bellatrix.AddComponent(detector);
        //bellatrix.AddComponent(new ContactDetectorComponent());
        bellatrix.AddComponent(new KillOnContactComponent(player,detector));
        bellatrix.AddComponent(new RenderComponent());

        //TBD random rambling?
        //TBD counter

        voldemort.AddComponent(new ChasePlayerComponent(player));
        voldemort.AddComponent(new KeepInBoundsComponent(map_x, map_y));
        detector = new ContactDetectorComponent();
        voldemort.AddComponent(detector);
        //voldemort.AddComponent(new ContactDetectorComponent());
        voldemort.AddComponent(new KillOnContactComponent(player,detector));
        voldemort.AddComponent(new RenderComponent());

        randGhost.AddComponent(new RandomRamblerComponent());
        randGhost.AddComponent(new KeepInBoundsComponent(map_x, map_y));
        randGhost.AddComponent(new RenderComponent());


        thisX = rand.Next(0, map_x);
        thisY = rand.Next(0, map_y);
        Entity powerUp = new Entity { X = thisX, Y = thisY, Name = "X" };
        detector = new ContactDetectorComponent();
        powerUp.AddComponent(detector);
        //powerUp.AddComponent(new ContactDetectorComponent());
        powerUp.AddComponent(new SpecialPowerHandoffComponent(player, new RunFasterComponent(player,2,3), detector));

        thisX = rand.Next(0, map_x);
        thisY = rand.Next(0, map_y);
        Entity powerUp2 = new Entity { X = thisX, Y = thisY, Name = "Y" };
        detector = new ContactDetectorComponent();
        powerUp2.AddComponent(detector);
        //powerUp2.AddComponent(new ContactDetectorComponent());
        powerUp2.AddComponent(new SpecialPowerHandoffComponent(player, new RunFasterComponent(player, 2, 8),detector));


        List<Entity> gameObjects = new List<Entity>();

        gameObjects.Add(player);
        gameObjects.Add(bellatrix);
        gameObjects.Add(voldemort);
        gameObjects.Add(randGhost);
        gameObjects.Add(powerUp);
        gameObjects.Add(powerUp2);
        gameObjects.Add(map);

        map.AddComponent(new MapComponent(gameObjects, map, map_x, map_y));

        while (true)
        {
            foreach(Entity e in gameObjects)
            {
                e.Update();
            }
        }
    }
}
using System;

abstract class Animal
{
    //Can REFERENCE abstract objects, but not create a NEW instance of one
    // can't say new Animal. But you can USE animal.

    //Cow and Dog are CONCRETE types because they CAN be instantiated
    public int Age { get; set; }
    public int Weight { get; set; }

    public abstract void MakeSound();
    //{ when it was virtual, this was filled in. Abstract, nothing TO fill in

    //    Console.WriteLine("");
    //}
}

class Cow : Animal
{
    //this cow class INHERITS attributes from the animal class
    //this is like putting numstomachs in animal
    public int NumStomachs { get; set; }
    public override void MakeSound()
    {
        System.Console.WriteLine("Moooo moo moo moo");
    }
    //what does protected do? shouldn't use a lot.
}

//cast vs conversion?

class Dog : Animal
{
    public int NumLicks { get; set; }
    public override void MakeSound()
    {
        System.Console.WriteLine("WOOOOOFF");
    }
}
//class MainClass
//{
//    static Random randy = new Random();
//    static bool RandomBool { get { return randy.Next() % 2 == 0; } }
//    static void Main()
//    {
//        Animal animal;
//        //Compile time type? Run time type? ...
//        //Compiler hides the dog/cow stuff behind animal stuff, which is visible
//        //WHAT is a CAST?! I have no clue. And conversions. Weird, confusing explanations
//
//        if (RandomBool)
//            animal = new Cow();
//        else
//            animal = new Dog();
//
//        animal.MakeSound();
//
//        //Animal cow = new Cow();
//        //cow is still sitting there, all we can see are the animal parts though
//        //cow still HAS num stomachs but you can't see it. UPCAST.
//        //cow.MakeSound();
//
//        //Compiler: what makesound are we talking about?! goes to animal
//        //---> Static binding
//
//        //Polymorphism: many forms of the makeSound function
//    }
//}
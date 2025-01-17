**SOURCE CODE EXPLANATION**

**OOP Principal Using SOLID**
SOLID was OOP Principal programming used to make a clean - flexible and also scalable , this principal was introduced by **Robert C Martin** ,
I am using SOLID at some part of the implementation on this test task 
1. **SRP** -> Single Responsibility Principal , which has been used on the _Actor_ Script , by creating 2 of important classes
   which is _StatsManager_ that only do 1 task to maintain and managing a stats on the _Actor_ class and_ StateManager_ that only do 1
   single task which is to maintain all the state that happen on that _Actor_
2. **LSP** -> Liskov Subtitution Principle , which has been created by determine _Actor_ class as parent , with _Enemy_ and _Player_ as its child
   , in this principal we doesnt need to change stats needs for each_ Enemy_ and _Player_ from the_ Actor_ but we can actually overriding the method
   from the _Actor_ class to determine new stats on each _Enemy_ and _Player_ without changing the logic on their parent, Include all methods they need
   for Action
3. **OCP** -> Open Closed Principal, at _AiInterface_ Class it can be used to the _Actor_ that need AI Logic , by creating interface for this AI , we can
   create new modification based on its Interface , from this case _Enemy_ class is need this interface so that it can be used to do **RandomAction** Method
   that actually needs to create simple AI Logic on this test
4. **ISP** -> Interface Segregation Principal , make Interface cut into pieces so that classes only need to use important interface that they needs , in this case
   Buff script was made and store 2 classes that needs to perform Buff and Debuff , which is _AttackBuff_ Class and _DefenseDebuff_ class , these 2 classes use different
   interface they need to perform different function - while _AttackBuff_ use _IBuff_ Interface (that can be expand depends on the cases) and _DefenseDebuff_ use _IDebuff_
   (that can actually be expands too depends on the cases), in this case its just create 1 single simple method to enchance damage and lower the defense
5. **DIP** -> Dependency Inversion Principal, Principe to not let Code become **Chain Dependency** by giving 1 Class depends only to certain interface or abstraction , in this     test, i'm implementing _AiInterface_ so that any _Actor_ that needs AI logic can used this interface which is what _Enemy_ class did then execute AI logic.

**Script Used**
There are 7 scripts needed to accomplish this Test task.
Those scripts are :
1. Actor : Store all classes needed to define the behaviour of Enemy and Player
2. Player : Determine Player as a child class of Actor
3. Enemy : Determine Enemy as a child class of Actor
4. GameCore : Managing the button interaction and turn logic with several animation handle and health damage spawner
5. Buff : Store all Buff and Debuff classes needs to execute buff and debuff on the battle logic 
6. AIInterface : even this isnt important for now , this interface can still be useful if later we want to add more ability into the AI, for now only RandomAction method in there
7. LoopAnimation : Store all animation logic for UI and Damage Dealt Animation

**Library Used**
I'm using some libraries to accomplished certain task on the battle logic , these 2 Libraries are require which is :
1. **UniTask** -> This library is much more better than using Coroutines to handle Asynchronous operation , since it doesnt generate garbage collector , i am using it
     to handle all Asynchronous task that happen between action and turn
2. **LeanTween** -> This library is more light to be used to handle UI Animation - instead we use Unity Animator or Timeline that require heavier process , by using Leantween
   it will allow me to create animation and call it in 1 single frame

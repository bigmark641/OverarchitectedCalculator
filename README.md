# Overarchitected Calculator
This was originally started to play around with some functional coding styles, but eventually I ended up adding a lot of OOP concepts into it too.

### Functional concepts:

All of the logic of the Calculator Engine, Operation Plugins, and most of the Unit Tests is done in a very functional style:
* No imperative code
* No mutable state

The only exceptions to this are:
* Initializing immutable properties within constructors
* I kept a single member mutable variable within the Calculator class that contains the calculator's current state.  (I could have avoided this by passing it in with each command but chose not to.)

### Object Oriented concepts

Each class is only accessed outside of its folder via an interface.  All concrete classes are instantiated and injected in a top-level runner harness.

The high-level calculator logic is completely abstracted away from the operations it can perform (including trivial commands such as addition and subtraction).  All operations are implemented and injected via Operation Plugins, and any number of new operations can be added and invoked without any changes to the Calculator Engine itself.

### Overall strategy

The goal of this was to overarchitect a simple app by fusing Functional and OOP styles, without feeling bad about all the additional complexity that overarchitecting causes.  Clearly, this is probably not the best way to write a simple calculator, but that wasn't the goal.

I spent most of my time focusing on the core business logic classes (Calculator Engine and the Calculator Plugins) and their unit tests.  The Text Calculator and Program runner harnesses are pretty messy and could use a lot of improvement.

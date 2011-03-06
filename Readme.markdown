GridBook, Large Scale Othello
=============================

# Introduction

This is a project where I am intending to do a large-scale distributed calculation
of an opening book for the board game Othello. Positions are stored in a relational
database (I have chosen PostgreSQL) using [NHibernate](http://nhforge.org/Default.aspx) object/relational mapper.

The project is still in its infancy, but the goal is to use [Edax](http://abulmo.perso.neuf.fr/edax/4.0/index.htm), a very good Othello
engine, to calculate new positions. I am also intending to use a service bus, probably
[Rhino Service Bus](http://hibernatingrhinos.com/open-source/rhino-service-bus), to distribute work to clients potentially all over the world.

# Code

The code is being developed in a [Domain-Driven Design](http://en.wikipedia.org/wiki/Domain-driven_design) style. Here's the layout:

* GridBook - This is the main assembly. It contains the Domain classes and service classes,
as well as mappings for [FluentNHibernate](http://fluentnhibernate.org/).
* GridBook.Test - Tests for the domain classes. Uses in-memory [SQLite](http://www.sqlite.org/) to run database tests quickly.
All tests are using AAA (Arrange/Act/Assert), Behaviour-Driven Development style. This works particularly nice with DDD.
* GridBook.CommandLine - kind of service program that contains many subprograms for different tasks,
such as conversion functions (book file to csv), etc. Uses [Castle Windsor](http://docs.castleproject.org/Windsor.MainPage.ashx) DI container for easy
extensibility.

# Contact

If you are interested in any part of this project, don't hesitate to contact me directly.
My name is Daniel Lidström and you can reach me by email: [dlidstrom@gmail.com](mailto:dlidstrom@gmail.com)

# License

The code is licensed using the Boost license.

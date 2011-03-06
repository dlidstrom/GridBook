# GridBook - Large Scale Othello

# Introduction

This is a project where I am intending to do a large-scale distributed calculation
of an opening book for the board game Othello. Positions are stored in a relational
database (I have chosen PostgreSQL) using NHibernate object/relational mapper.

The project is still in its infancy, but the goal is to use Edax, a very good Othello
engine, to calculate new positions. I am also intending to use a service bus, probably
Rhino Service Bus, to distribute work to clients potentially all over the world.

# Code

The code is being developed in a Domain-Driven Design style. Here's the layout:

* GridBook - This is the main assembly. It contains the Domain classes and service classes,
as well as mappings for FluentNHibernate.
* GridBook.Test - Tests for the domain classes. Uses in-memory SQLite to run database tests quickly.
* GridBook.CommandLine - kind of service program that contains many subprograms for different tasks,
such as conversion functions (book file to csv), etc. Uses Castle Windsor DI container for easy
extensibility.

# Contact

If you are interested in any part of this project, don't hesitate to contact me directly.
My name is Daniel Lidström and you can reach me by email: dlidstrom@gmail.com

# License

The code is licensed using the Boost license.

<img align="left" width="116" height="116" src="https://github.com/jdrames/TodoApp/raw/main/.github/icon.png" />

# Todo App Solution

<br />

This is a solution for creating a ASP.NET Core following the principles of Clean Architecture. It uses Razor Pages to display a simple Todo List application.

## Technologies
* ASP.NET Core 5
* Entity Framework Core 5
* MediatR
* AutoMapper
* FluentValidation
* Docker

## Overview

### Domain

This will contain all the entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all the application logic. It is dependent on the domain layer only.

This layer defines interfaces that are implemented by outside layers.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, etc. These classes should be based on interfaces defined in the application layer.

## Getting Started

In Development.
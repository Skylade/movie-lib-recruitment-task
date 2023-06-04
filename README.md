# Interview task - backend developer

## Introduction

Application Movie Library was created to manage the movies available on the new streaming service called **InsysGO**. Main developer Stefan before his vacation implemented only a database connection and filled it with example data. Your task will be to finish the work started by Stefan within given days, after which Stefan will come back to evaluate your work. Below you will find the requirements and a list of tasks left to you, some of them are marked **\***, these are bonus tasks that you do not have to implement, but their implementation will speed up the launch of **InsysGO**.

## How to run

To start the application you will need:
- IDE (i.e. Visual Studio, Visual Studio Code)
- Installed .NET core 3.1

Set **MovieLibrary.Api** as a startup project and after run SQLite database will be connected automatically you don't need to install anything.

## Database description

Application uses the SQLite database with entity framework core using eager loading approach. Database file is located in the **MovieLibrary.db** in **MovieLibrary.Api** project. You can use DB Browser for SQLite to browse the database. Below you can find a description of all the entities.

### Movie
Contains data about movies
```c#
public int Id { get; set; }
public string Title { get; set; }
public string Description { get; set; }
public int Year { get; set; }
public decimal ImdbRating { get; set; }
```

### Category
Contains data about categories
```c#
public int Id { get; set; }
public string Name { get; set; }
```

### MovieCategory
Contains data about movies categories
```c#
public int Id { get; set; }
public int MovieId { get; set; }
public int CategoryId { get; set; }
```

## Requirements

- Application data access layer together with all database releated objects (entities, database context, migrations) should be stored in **MovieLibrary.Data** project
- Application business logic should be stored in **MovieLibrary.Core** project
- All endpoints should be published using **MovieLibrary.Api** project

## Tasks

- Create data access layer using repository pattern
- Create api endpoints for CRUD operations for Movie and Category entities:
    - CRUD operations for Movie entity should be available at: **/v1/MovieManagement**
    - CRUD operations for Category entity should be available at: **/v1/CategoryManagement**
- Create api endpoint for filtering Movies:
    - should be available at: **/v1/Movie/Filter**
    - should return all movies data together with information about its categories
    - should filter by following parameters (all parameters are optional): 
        - by text (should return all movies that title contains text given in parameter)
        - by list of categories (return all movies assigned to these categories)
        - by movie minimum and maximum IMDB rating
    - returned data should be sorted descending by IMDB rating 
    - endpoint should return data as JSON
    - should allow paging
- Create tests for application

## Examples

### Filter movies by text

Parameters:

- text = harry

```json
{
    [
        {
            "Id": 1,
            "Title": "Harry Potter and the Sorcerer's Stone",
            "Description": "...",
            "Year": 2001,
            "Imdb": 7.6,
            "Categories": [
                {
                    "Id": 2,
                    "Name": "Adventure"
                },
                ...
            ]
        },
        {
            "Id": 2,
            "Title": "Harry Potter and the Chamber of Secrets",
            "Description": "...",
            "Year": 2002,
            "Imdb": 6.8,
            "Categories": [
                {
                    "Id": 2,
                    "Name": "Adventure"
                },
                ...
            ]
        },
    ]
}
```

### Filter movies by list of categories

Parameters:

- categoriesId = 3,11

```json
{
    [
        {
            "Id": 8,
            "Title": "La vita è bella",
            "Description": "...",
            "Year": 1997,
            "Imdb": 8.6,
            "Categories": [
                {
                    "Id": 3,
                    "Name": "Comedy"
                },
                ...
            ]
        },
        {
            "Id": 10,
            "Title": "Intouchables",
            "Description": "...",
            "Year": 2011,
            "Imdb": 8.5,
            "Categories": [
                {
                    "Id": 3,
                    "Name": "Comedy"
                },
                ...
            ]
        }
    ]
}
```

### Filter movies by minimum and maximum IMDB

Parameters:

- minImdb = 8.1
- maxImdb = 8.3

```json
{
    [
        {
            "Id": 11,
            "Title": "The Prestige",
            "Description": "...",
            "Year": 2006,
            "Imdb": 8.2,
            "Categories": [
                {
                    "Id": 5,
                    "Name": "Drama"
                },
                ...
            ]
        }
    ]
}
```
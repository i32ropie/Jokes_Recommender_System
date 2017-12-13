# Jokes_Recommender_System

Presentation is uploaded in root directory as "Jokes-recommender-system.pdf"

Structure:
  - Web application is written in C# ASP.NET MVC following conventional application structure and the main logic and recommending alghoritms are in files "JokeFacade.cs" and "RatingFacade.cs" in "Jokes_Recommender_System/Jokes_recommender_system/Jokes_recommender_system/Models/Facades" directory.
  - Optimicer script is written in Python3 and using the external library [RAKE-NLTK](https://github.com/csurfer/rake-nltk)
  
Team roles:
  - Adam Teršl: Web scraping data set, creating database model, developing web application, presenting
  - Andrej Boniš: Term frequency/inverse document frequency recommendation, collaborative filtering recommendation by category, policy of likes/dislikes, presenting
  - Eduardo Roldán: Filtering and balancing the dataset.
  - Patrik Novák: Similar joke recommendation, different joke recommendation, presenting.

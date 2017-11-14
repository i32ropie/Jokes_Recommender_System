# Jokes_Recommender_System
Consultations summary:

- Data set:
  - hundreds of jokes is sufficent (a hundred per category)
  - we should have balanced categories (similar quantity in each category)
  - we can try to make readability of the jokes as another attribute for recommendations
  - keywords analysis
  
- Recommending:
  - like/dislike better then starts (easier)
  - different jokes vs similar jokes, probably better different, but we can implement both ways
  - intelectual jokes vs stupid ones based on some keywords and length (hard)
  - he said some techniques TF and IDF, I dont know what it means yet
  - we should focus on content based on single jokes + collaborative filtering on categories, meaning that if a lot of users like for         example jokes about blondies and also about cops, we can recommend cop joke to a new user that is reading blonde joke
  - at least two recommendation methods

- Evaluation:
  - we should choose between time spent on the site with top threshold (minutes), so we wont have hours from users that only leave the         site opened in tab and number of jokes read
  - we should have at least 3 evaluation techniques
  - we should compare recommendadtion methods with random recommending
  
- Presentation:
  - presentation should involve a lot about data set (obtaining data set, analysis of keywords etc., balancing categories, own experience)
  - each recommendation technique presented by one person
  

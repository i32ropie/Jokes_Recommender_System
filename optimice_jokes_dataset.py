#!/usr/bin/python3

"""
For the use of this script, we do not start from the file
jokes_dataSet_notOptimized.csv but from jokes_dataSet_notOptimized.json file
where we have already prefiltered the file deleting an empty attribute from
the jokes, deleting the empty jokes and changing the length and title tags to
a one word tag. For doing that thing we use some simple regex.
"""

import json
import operator
# For extracting the keywords we will use https://github.com/csurfer/rake-nltk
from rake_nltk import Rake

r = Rake()

# Load the jokes
with open('jokes_dataSet_notOptimized.json') as f:
    jokes = json.load(f)
print("[·] Jokes dataset loaded.")
print("\t[·] Total of jokes loaded: {}".format(len(jokes)))

# First of all lets remove the categories with less than 100 jokes
print("[·] Deleting categories with less than 100 jokes...")

# First we see how many jokes of each category we have.
jokes_categories = {}
for x in jokes:
    if not jokes_categories.get(x.get('Category')):
        jokes_categories[x.get('Category')] = 1
    else:
        jokes_categories[x.get('Category')] += 1

# Then we iterate over a new list so we can delete jokes directly from the
# variable.
for x in [x for x in jokes]:
    if jokes_categories.get(x['Category']) < 100:
        jokes.remove(x)
print("\t[·] Deleted categories.")
print("\t[·] Total of jokes loaded: {}".format(len(jokes)))

print("[·] Extracting keywords...")
for x in jokes:
    # We make it extract the keywords from the joke
    r.extract_keywords_from_text(x['Joke'])
    # We look at r.degree items, sort them by it value, filter the ones that are
    # words and grab the first 4
    x['Keywords'] = [y[0] for y in sorted(r.degree.items(
    ), key=operator.itemgetter(1), reverse=True) if y[0].isalpha()][:4]
print("\t[·] Keywords extracted.")

# Now we will filter again the jokes by category to see the amount of short/long
# jokes we have in each category.
jokes_categories = {}
for x in jokes:
    if not jokes_categories.get(x.get('Category')):
        jokes_categories[x.get('Category')] = [x]
    else:
        jokes_categories[x.get('Category')].append(x)

# And we will show them to have an idea of how things are going
short_jokes = 0
long_jokes = 0
print("[·] Showing the balance of the dataset.\n")
print("| Category                  |  Long | Short |")
print("|---------------------------+-------+-------|")
for x in jokes_categories:
    print("| {:25} | {:5} | {:5} |".format(x,
                                           len([y for y in jokes_categories[x] if y['Length'] >= 25]),
                                           len([y for y in jokes_categories[x] if y['Length'] < 25])))
    long_jokes += len([y for y in jokes_categories[x] if y['Length'] >= 25])
    short_jokes += len([y for y in jokes_categories[x] if y['Length'] < 25])
print(
    "\nTotal of long jokes: {}\nTotal of short jokes: {}".format(
        long_jokes,
        short_jokes))

"""
As we can see, the dataset is not balanced.

| Category                  |  Long | Short |
|---------------------------+-------+-------|
| Animal                    |   126 |   304 |
| Blonde                    |    54 |   139 |
| Blue Collar               |    81 |    87 |
| Dark Humor                |    80 |   102 |
| Dirty                     |    88 |   267 |
| Doctor                    |   137 |   249 |
| Fat                       |     8 |   126 |
| Food                      |    72 |   162 |
| God                       |   111 |   176 |
| Gross                     |   106 |   230 |
| Insults                   |    83 |   361 |
| Kids                      |   181 |   275 |
| Lookin' Good              |   107 |   314 |
| Marriage                  |   168 |   256 |
| Men/Women                 |   106 |   233 |
| Miscellaneous             |    89 |   259 |
| Money                     |    92 |   208 |
| Nationality               |   111 |   195 |
| News & Politics           |   138 |   229 |
| Partying & Bad Behavior   |   187 |   251 |
| Pick-Up Lines             |     0 |   267 |
| Police & Military         |    85 |   147 |
| Pop Culture & Celebrity   |   130 |   338 |
| School                    |    86 |    96 |
| Sports & Athletes         |    96 |   144 |
| Technology                |    63 |   123 |
| Travel & Car              |    96 |   178 |
| Work                      |    97 |   156 |
| Yo' Mama                  |     1 |   439 |

Total of long jokes: 2779
Total of short jokes: 6311

The next step will be taking 100 jokes of each category and trying to balance
the total of long jokes with the total of short jokes.

For doing that, we will start taking all the short jokes from the categories:
    Yo' Mama
    Pick-Up Lines
    Fat

The reason is that they have none or very few long jokes.

For having a balanced dataset we will take from the remaining categories 44
short jokes and 56 long jokes.
"""


print("\n[·] Balancing the dataset.")
long_jokes = 0
short_jokes = 0
jokes_2 = []
for x in jokes_categories:
    if x not in ["Yo' Mama", "Pick-Up Lines", "Fat"]:
        for y in jokes_categories[x]:
            if short_jokes == 44 and long_jokes == 56:
                continue
            elif y['Length'] < 25 and short_jokes < 44:
                jokes_2.append(y)
                short_jokes += 1
            elif y['Length'] >= 25 and long_jokes < 56:
                jokes_2.append(y)
                long_jokes += 1
        long_jokes = short_jokes = 0
    else:
        for y in jokes_categories[x]:
            if short_jokes == 100:
                continue
            elif y['Length'] < 25 and short_jokes < 100:
                jokes_2.append(y)
                short_jokes += 1
        short_jokes = 0
print("\t[·] Dataset balanced.")

# Now we will see the balanced dataset.

jokes_categories_2 = {}
for x in jokes_2:
    if not jokes_categories_2.get(x.get('Category')):
        jokes_categories_2[x.get('Category')] = [x]
    else:
        jokes_categories_2[x.get('Category')].append(x)

short_jokes = 0
long_jokes = 0
print("[·] Showing the balance of the dataset.\n")
print("| Category                  |  Long | Short |")
print("|---------------------------+-------+-------|")
for x in jokes_categories_2:
    print("| {:25} | {:5} | {:5} |".format(x,
                                           len([y for y in jokes_categories_2[x] if y['Length'] >= 25]),
                                           len([y for y in jokes_categories_2[x] if y['Length'] < 25])))
    long_jokes += len([y for y in jokes_categories_2[x] if y['Length'] >= 25])
    short_jokes += len([y for y in jokes_categories_2[x] if y['Length'] < 25])
print(
    "\nTotal of long jokes: {}\nTotal of short jokes: {}".format(
        long_jokes,
        short_jokes))

"""
As we can see, or dataset is now balanced.

| Category                  |  Long | Short |
|---------------------------+-------+-------|
| Animal                    |    56 |    44 |
| Blonde                    |    54 |    44 |
| Blue Collar               |    56 |    44 |
| Dark Humor                |    56 |    44 |
| Dirty                     |    56 |    44 |
| Doctor                    |    56 |    44 |
| Fat                       |     0 |   100 |
| Food                      |    56 |    44 |
| God                       |    56 |    44 |
| Gross                     |    56 |    44 |
| Insults                   |    56 |    44 |
| Kids                      |    56 |    44 |
| Lookin' Good              |    56 |    44 |
| Marriage                  |    56 |    44 |
| Men/Women                 |    56 |    44 |
| Miscellaneous             |    56 |    44 |
| Money                     |    56 |    44 |
| Nationality               |    56 |    44 |
| News & Politics           |    56 |    44 |
| Partying & Bad Behavior   |    56 |    44 |
| Pick-Up Lines             |     0 |   100 |
| Police & Military         |    56 |    44 |
| Pop Culture & Celebrity   |    56 |    44 |
| School                    |    56 |    44 |
| Sports & Athletes         |    56 |    44 |
| Technology                |    56 |    44 |
| Travel & Car              |    56 |    44 |
| Work                      |    56 |    44 |
| Yo' Mama                  |     0 |   100 |

Total of long jokes: 1454
Total of short jokes: 1444

Last remaining thing is to save our dataset.
"""

with open('jokes.json', 'w') as f:
    json.dump(jokes_2, f, indent=4)

print("\n[·] Balanced dataset saved as 'jokes.json'\n")


using Core.Locations;

string alphabeticalLocation = "game:entities:player:components:transform";
Location location1 = new(alphabeticalLocation);
Location location2 = new(alphabeticalLocation);

bool equals = location1 == location2;
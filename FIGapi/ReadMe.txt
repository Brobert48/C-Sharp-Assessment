Endpoints for your testing:
• Create a Team
	Send a POST request to https://localhost:{YOURPORT}/Teams
		Example body:
			{
				"Name" : "Avalanche",
				"Location" : "Colorado" 
			}
• Create a Player
	Send a POST request to https://localhost:{YOURPORT}/Players
			Example body:
				{
					"FirstName": "Nathan",
					"LastName": "MacKinnon",
					"TeamId": 1 
				}
• Add or Remove a Player from a Team
	Send a PUT request to https://localhost:{YOURPORT}/Players
			Example body:
				{
					"FirstName": "Nathan",
					"LastName": "MacKinnon",
					"TeamId": {New TeamId or null for no team} 
				}
• Query for Players
	o Query by ID
		Send a GET request to https://localhost:{YOURPORT}/Players?Id={ID GOES HERE}
	o Query All Players
		Send a GET request to https://localhost:{YOURPORT}/Players
	o Query All Players matching a given Last Name
		Send a GET request to https://localhost:{YOURPORT}/Players?LastName={LastName GOES HERE}
	o Query for all Players on a Team
		Send a GET request to https://localhost:{YOURPORT}/Players?Team={TeamId GOES HERE}

• Query for Teams
	o Query by ID
		Send a GET request to https://localhost:{YOURPORT}/Teams?Id={ID GOES HERE}
	o Query All Teams
		Send a GET request to https://localhost:{YOURPORT}/Teams
	o Query All Teams ordered by Name or Location
		Send a GET request to https://localhost:{YOURPORT}/Teams?orderBy={Name/Location}
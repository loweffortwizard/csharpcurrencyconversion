# csharpcurrencyconversion
C# library for real-time currency conversion and exchange rates using OpenExchange and Fixer API.

Overview. 

Summary:
C# library for real-time currency conversion and exchange rates using Fixer and OpenExchange API.

How To Use:

Set up:
The fixer.io and openexchange requires an API key. A free key is avalible for both (requies account creation) allowing up to 1000 requests/month

API key must be defined through the 'SetApiKey' method before converting. Something like:
	Fixer.SetApiKey("your api key here");
	OpenEx.SetApiKey("your api key here");
  
You may wish to review avalible symbols for conversion. Seen under region tags in 'Symbols.cs'

Convert:
Import Library to existing project (like this):
	'using Conversion;'

	double sixtyFiveUsToGb = OpenEx.Convert(Symbols.USD, Symbols.GBP, 65); 
	double sixtySixUsToGb = OpenEx.Convert(Symbols.USD, Symbols.GBP, 66);

	console.writeline(sixtyFiveUsToGb);
	console.writeline(sixtySixUsToGb);

Acknowledgements:
Developed for a university project.
Developed and based on the work from Ryan Morrin's "FixerSharp".
Makes use of the fixer and open exchange api's for real-time exchange rates.
